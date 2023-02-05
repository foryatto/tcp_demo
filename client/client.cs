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

            // get filename from agrs
            if (args.Length > 0)
            {
                for (int i = 0; i < args.Length; i++)
                {
                    client.sendToServer(args[i]);
                }
            }
            else
            {
                // send default files to server
                client.sendToServer("测试文件1");
                client.sendToServer("测试文件2");
            }

            Console.WriteLine("done.");
        }
    }

    public class Client
    {
        private string _addr;
        private int _port;

        // 文件序号
        private byte _fileNumber;

        const int BufSize = 1024;

        public Client(string addr, int port)
        {
            this._addr = addr;
            this._port = port;
        }

        public void sendToServer(string filename)
        {
            var client = new TcpClient();
            client.Connect(_addr, _port);
            var stream = client.GetStream();

            byte[] data = new byte[BufSize];

            FileStream file = new FileStream(filename,
            FileMode.Open, FileAccess.Read);

            int n;
            while ((n = file.Read(data, 0, data.Length)) > 0)
            {
                stream.Write(data, 0, n);
            }

            file.Close();
            client.Close();
        }

    }
}
