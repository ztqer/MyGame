using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadScene : MonoBehaviour
{
    private Socket socket;
    private ManualResetEvent connectDone,receiveDone;
    private byte[] buffer = new byte[1024];
    private GameSetBuilder gameSetBuilder = new GameSetBuilder();

    //异步连接服务器，若用户名正确，则跳转场景
    public void Load()
    {
        try
        {
            socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            connectDone = new ManualResetEvent(false);
            receiveDone = new ManualResetEvent(false);
            socket.BeginConnect("122.51.240.150", 888, new AsyncCallback(ConnectCallBack), socket);
            //socket.BeginConnect("192.168.1.103", 43158, new AsyncCallback(ConnectCallBack), socket);
            connectDone.WaitOne();
            socket.Send(Encoding.ASCII.GetBytes(GameObject.Find("Name_Input").GetComponent<InputField>().text));
            socket.BeginReceive(buffer, 0, buffer.Length, SocketFlags.None,new AsyncCallback(ReceiveCallBack), socket);
            receiveDone.WaitOne();
            if (Regex.IsMatch(Encoding.ASCII.GetString(buffer), "pass"))
            {
                socket.Close();
                gameSetBuilder.ReadChoices(GameObject.Find("Name_Input_Text").GetComponent<Text>().text, (PublicData.AssistantName)GameObject.Find("Assistant_Dropdown").transform.GetComponent<Dropdown>().value);
                GameSet.gameSet=gameSetBuilder.Build();
                SceneManager.LoadScene("FightScene");
              }
        }
        catch (Exception e)
        {
            Debug.Log(e);
        }
    }
    private void ConnectCallBack(IAsyncResult result)
    {
        Debug.Log("连接服务器");
        Socket socket = (Socket)result.AsyncState;
        socket.EndConnect(result);
        connectDone.Set();
    }
    private void ReceiveCallBack(IAsyncResult result)
    {
        try
        {
            Socket socket = (Socket)result.AsyncState;
            int bytesRead = socket.EndReceive(result);
            if (bytesRead != 0)
            {
                Debug.Log("收到消息:" + Encoding.ASCII.GetString(buffer, 0, bytesRead));
            }
            receiveDone.Set();
        }
        catch (Exception e)
        {
            Console.WriteLine("Receive失败:" + e.Message);
        }
    }
}