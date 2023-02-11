using System;
using System.Text;
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

            // user input
            if (args.Length > 0)
            {
                for (int i = 0; i < args.Length; i++)
                {
                    client.SendToServer(args[i]);
                }
            }
            else
            {
                // send default files to server
                client.SendToServer("测试文件1");
                client.SendToServer("测试文件2");
            }

            Console.WriteLine("done.");
        }
    }

    public class Client
    {
        private readonly string _addr;
        private readonly int _port;

        const int BufSize = 1024;

        public Client(string addr, int port)
        {
            _addr = addr;
            _port = port;
        }

        public void SendToServer(string filename)
        {
            // create client and stream
            TcpClient client = new();

            client.Connect(_addr, _port);
            
            var stream = client.GetStream();

            // send filename to server
            byte[] nameBytes = Encoding.UTF8.GetBytes(filename);
            byte[] buffer = new byte[nameBytes.Length + 1];
            buffer[0] = (byte)nameBytes.Length;
            for(int i = 0; i < nameBytes.Length; i++)
            {
                buffer[i+1] = (byte)nameBytes[i];
            }
            stream.Write(buffer);
            Console.WriteLine($"send {filename} to server.");


            // read datas from file
            // and send to server
            byte[] data = new byte[BufSize];

            FileStream file = new FileStream(filename,
            FileMode.Open, FileAccess.Read);

            int n;
            while ((n = file.Read(data, 0, data.Length)) > 0)
            {
                stream.Write(data, 0, n);
            }

            // close
            file.Close();
            client.Close();
            stream.Close();
        }

    }
}
