using System;

namespace tcp_demo
{
    class Test
    {
        public static void Main(string[] args)
        {
            var gen = new GenFrameFile();
            gen.GenerateFrameFile();
        }
    }

    class GenFrameFile
    {
        // 32kb a frame
        const int FrameSize = 32 * 1024;
        const int FrameNums = 10;

        public void GenerateFrameFile()
        {
            for (int i = 0; i < FrameNums; i++)
            {
                byte[] frameData = RandomFrameData();
                SaveToFile("测试文件1", frameData);
                if (i == 0)
                {
                    SaveToFile("测试文件2", frameData.Skip(4).ToArray());
                }
                else
                {
                    SaveToFile("测试文件2", frameData);
                }
            }
            
        }

        public static void SaveToFile(string filename, byte[] data)
        {
            FileStream file = new(filename,
            FileMode.Append, FileAccess.Write);

            file.Write(data, 0, data.Length);
            file.Close();
        }

        public byte[] RandomFrameData()
        {
            byte[] data = new byte[FrameSize];
            Random random = new();

            random.NextBytes(data);
            data[0] = 0xAA;
            data[1] = 0xBB;
            data[2] = 0xCC;
            data[3] = 0xDD;
            return data;
        }

    }
}
