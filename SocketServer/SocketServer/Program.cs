using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;

namespace SocketServer
{
    class MainClass
    {
        private static Socket tcpSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        private static EndPoint endPoint = new IPEndPoint(IPAddress.Parse("172.17.0.7"), 888);
        //private static EndPoint endPoint = new IPEndPoint(IPAddress.Parse("192.168.1.103"), 43158);
        private static int maxClient=10;
        public static ManualResetEvent allDone = new ManualResetEvent(false);

        private static void Main(string[] args)
        {
            try
            {
                tcpSocket.Bind(endPoint);
                tcpSocket.Listen(maxClient);
                Console.WriteLine("服务端就绪，" + endPoint);
                while (true)
                {
                    allDone.Reset();
                    tcpSocket.BeginAccept(new AsyncCallback(AcceptCallBack), tcpSocket);
                    allDone.WaitOne();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Tcp连接失败"+e.ToString());
            }
        }

        private static void AcceptCallBack(IAsyncResult result)
        {
            try
            {
                Socket tcpSocket = (Socket)result.AsyncState;
                Client client=new Client();
                Socket socket = tcpSocket.EndAccept(result);
                client.socket = socket;
                Console.WriteLine(socket.RemoteEndPoint+"已连接");
                socket.BeginReceive(client.buffer,0,client.buffer.Length, SocketFlags.None, new AsyncCallback(ReceiveCallBack), client);
            }
            catch (Exception e)
            {
                Console.WriteLine("Accept失败:" + e.Message);
            }
        }

        private static void ReceiveCallBack(IAsyncResult result)
        {
            try
            {
                Client client=(Client)result.AsyncState;
                int bytesRead=client.socket.EndReceive(result);
                if (bytesRead != 0)
                {
                    Console.WriteLine("收到消息:" + Encoding.ASCII.GetString(client.buffer, 0, bytesRead));
                    MysqlUtility.SqlTask("INSERT INTO login_record (IP,name,time) VALUES (\"" + client.socket.RemoteEndPoint + "\",\""+ Encoding.ASCII.GetString(client.buffer, 0, bytesRead) + "\",NOW())");
                    Send(client.socket, "pass");
                }
                //if (Regex.IsMatch(Encoding.ASCII.GetString(client.buffer), "ztq")) 
                //{
                //    Send(client.socket, "pass");
                //}
                //else
                //{
                //    Send(client.socket, "fail");
                //}
                //client.socket.BeginReceive(client.buffer, 0, client.buffer.Length, SocketFlags.None,  new AsyncCallback(ReceiveCallBack), client);
            }
            catch (Exception e)
            {
                Console.WriteLine("Receive失败:" + e.Message);
            }
        }

        private static void Send(Socket socket, String data)
        {  
            byte[] byteData = Encoding.ASCII.GetBytes(data);
            socket.BeginSend(byteData, 0, byteData.Length, 0, new AsyncCallback(SendCallback), socket);
        }

        private static void SendCallback(IAsyncResult result)
        {
            try
            { 
                Socket socket = (Socket)result.AsyncState;
                int bytesSent = socket.EndSend(result);
                Console.WriteLine("成功发送消息");
                socket.Shutdown(SocketShutdown.Both);
                socket.Close();
                allDone.Set();
            }
            catch (Exception e)
            {
                Console.WriteLine("Send失败："+e.ToString());
            }
        }
    }
}
