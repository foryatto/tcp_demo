using System;
using System.Net;
using System.Net.Http;
using System.Net.Sockets;

namespace tcp_demo
{
    class Test
    { 
        public static void Main(string[] args)
        {
            var client = new Client("127.0.0.1", 34567);
   
            client.sendToServer("测试文件1");
            Console.WriteLine("done.");
        }
    }

    public class Client
    {
        private TcpClient _client;
        private NetworkStream _stream;

        const int BufSize = 1024;

        public Client(string addr, int port)
        {
            this._client = new TcpClient();
            this._client.Connect(addr, port);
            this._stream = this._client.GetStream();
        }

        public void sendToServer(string filename)
        {
            byte[] data = new byte[BufSize];

            // get data from file
            FileStream file = new FileStream(filename,
            FileMode.Open, FileAccess.Read);

            while (file.Read(data, 0, data.Length) > 0)
            {
                this._stream.Write(data, 0, data.Length);
            }
           
            file.Close();
            this._client.Close();
        }
    }
}
