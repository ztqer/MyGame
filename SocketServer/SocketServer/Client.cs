using System;
using System.Net.Sockets;
using System.Text;

namespace SocketServer
{
    public class Client
    {
        public Socket socket;
        public byte[] buffer = new byte[1024];
    }
}
