using System;
using System.Text;
using System.Net;
using System.Net.Sockets;

namespace tcp_demo
{
    class Test
    {
        public static void Main(string[] args)
        {
            new Server("127.0.0.1", 34567).Run();
        }
    }
    class Server
    {
        private readonly string _addr;
        private readonly int _port;

        public Server(string addr, int port)
        {
            _addr = addr;
            _port = port;
        }

        public void Run()
        {
            TcpListener listen = new(IPAddress.Parse(this._addr), this._port);
            listen.Start();

            Console.WriteLine($"Server is running on {this._addr}:{this._port}");

            while (true)
            {
                TcpClient client = listen.AcceptTcpClient();

                Thread th = new(() =>
                {
                    ClientHandler.FrameParse(client);
                })
                {
                    // 后台线程随着主线程的结束而结束,释放资源
                    IsBackground = true
                };
                th.Start();

            }
        }
    }

    class ClientHandler
    {
        private static readonly byte[] FrameHead = { 0xAA, 0xBB, 0xCC, 0xDD };

        public static void FrameParse(TcpClient client)
        {
            string? filename = null;

            client.NoDelay = true;
            NetworkStream stream = client.GetStream();
            byte[] data = new byte[1024];

            Queue<byte> queue = new();
            int idx = 0;
            int frameCount = 0;

            int startIdx = 0;

            while (true)
            {
                int n;
                try
                {
                    n = stream.Read(data, 0, data.Length);
                }
                catch (IOException)
                {
                    Console.WriteLine("connection dropped.");
                    break;
                }
                if (n > 0)
                {
                    if (filename == null)
                    {
                        int length = data[0];
                        filename = Encoding.UTF8.GetString(data.Skip(1).Take(length).ToArray());
                        Console.WriteLine(filename);
                        if (n == length + 1)
                        {
                            continue;
                        }
                        startIdx = length + 1;
                    }

                    for (int i = startIdx; i < n; i++)
                    {
                        if (startIdx != 0)
                        {
                            startIdx = 0;
                        }
                        queue.Enqueue(data[i]);
                        if (data[i] == FrameHead[idx])
                        {
                            idx++;
                        }
                        else
                        {
                            idx = 0;
                        }
                        if (idx == 4)
                        {
                            idx = 0;
                            if (queue.Count <= 4)
                            {
                                continue;
                            }
                            frameCount++;
                            int curLength = queue.Count;
                            SaveToFile(filename, frameCount, queue.ToArray(), 0, curLength - 4);

                            // clear queue
                            for (int j = 0; j < curLength - 4; j++)
                            {
                                queue.Dequeue();
                            }
                        }
                    }

                }
                else
                {
                    break;
                }
            }
            if (queue.Count > 0)
            {
                frameCount++;
                SaveToFile(filename ?? "default", frameCount, queue.ToArray(), 0, queue.Count);
                queue.Clear();
            }

            stream.Close();
            client.Close();
        }

        static void SaveToFile(string fileName, int count, byte[] data, int start, int end)
        {
            if (!Directory.Exists(fileName))
            {
                Directory.CreateDirectory(fileName);
            }
            string filePath = Path.Combine(fileName, $"{fileName}_{count}");

            FileStream file = new(filePath,
            FileMode.OpenOrCreate, FileAccess.Write);

            for (int i = start; i < end; i++)
            {
                file.WriteByte(data[i]);
            }
            file.Close();
        }

        /*
        static void outdated(TcpClient client, int fileNum)
        {
            // cache
            byte[] cacheData = new byte[1 * 1024 * 1024];
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
                catch (IOException)
                {
                    Console.WriteLine("connection dropped.");
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
                        SaveToFile($"file_{fileNum}", count, cacheData, prev, idx);
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
            SaveToFile($"file_{fileNum}", count, cacheData, prev, currentCacheSize);
        }
        */

    }

}
