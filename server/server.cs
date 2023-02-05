using System;
using System.Net;
using System.Net.Sockets;

namespace tcp_demo
{
    class Test
    {
        public static void Main(string[] args)
        {
            Server.Run("127.0.0.1", 34567);
        }
    }
    class Server
    {
        // cache 1MB
        private const int CacheSize = 1 * 1024 * 1024;
        public static void Run(string addr, int port)
        {
            TcpListener listen = new TcpListener(IPAddress.Parse(addr), port);
            listen.Start();

            Console.WriteLine($"Server is running on {addr}:{port}");

            int fileNum = 0;

            while (true)
            {
                TcpClient client = listen.AcceptTcpClient();
                fileNum++;
                Thread th = new Thread(() =>
                {
                    handler(client, fileNum);
                });
                th.Start();
            }
        }

        static void handler(TcpClient client, int fileNum)
        {
            // cache
            byte[] cacheData = new byte[CacheSize];
            int currentCacheSize = 0;

            NetworkStream stream = client.GetStream();
            byte[] data = new byte[1024];

            // parse frame
            int idx = 1;
            int prev = 0;
            bool found = false;

            int count = 0;

            while (true)
            {
                int n;
                try
                {
                    n = stream.Read(data, 0, data.Length);
                }
                catch (IOException _)
                {
                    Console.WriteLine("连接已断开");
                    break;
                }
                if (n > 0)
                {
                    for (int i = 0; i < n; i++)
                    {
                        cacheData[currentCacheSize] = data[i];
                        currentCacheSize++;
                    }
                    for (; idx < currentCacheSize - 3; idx++)
                    {
                        if (cacheData[idx] != 0xAA)
                        {
                            continue;
                        }
                        if (cacheData[idx + 1] == (0xBB) && cacheData[idx + 2] == (0xCC) && cacheData[idx + 3] == (0xDD))
                        {
                            found = true;
                            break;
                        }
                    }
                    if (found)
                    {
                        count++;
                        saveToFile(fileNum, count, cacheData, prev, idx);
                        prev = idx;

                        idx++;
                        found = false;
                    }
                }
                else
                {
                    break;
                }
            }
            count++;
            saveToFile(fileNum, count, cacheData, prev, currentCacheSize);
            saveToFile(fileNum, 0, cacheData, 0, currentCacheSize);

        }

        static void saveToFile(int fileNum, int count, byte[] data, int start, int end)
        {
            string dirPath = $"file_{fileNum}";
            if (!Directory.Exists(dirPath))
            {
                Directory.CreateDirectory(dirPath);
            }
            string filePath = Path.Combine(dirPath, $"{fileNum}_{count}");
            FileStream file = new FileStream(filePath,
            FileMode.OpenOrCreate, FileAccess.Write);

            for (int i = start; i < end; i++)
            {
                file.WriteByte(data[i]);
            }
            file.Close();
        }

    }

}
