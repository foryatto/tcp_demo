using System;

namespace tcp_demo
{
    class GenFrameFile
    {

        const int FrameSize = 32 * 1024;
        const int FrameNums = 10;
        const int Size = FrameSize * FrameNums;

        public static void Main(string[] args)
        {
            var gen = new GenFrameFile();
            gen.generateFrameFile();
        }

        public void generateFrameFile()
        {
            for (int i = 0; i < FrameNums; i++)
            {
                byte[] frameData = randomFrameData();
                saveToFile("测试文件1", frameData);
                if (i == 0)
                {
                    byte[] frameData2 = new byte[FrameSize - 4];
                    for(int j = 0; j < FrameSize - 4; j++)
                    {
                        frameData2[j] = frameData[j + 4];
                    }
                    saveToFile("测试文件2", frameData2);
                }
                else
                {
                    saveToFile("测试文件2", frameData);
                }
            }
            
        }

        public void saveToFile(string filename, byte[] data)
        {
            FileStream file = new FileStream(filename,
            FileMode.OpenOrCreate, FileAccess.Write);

            file.Write(data, 0, data.Length);
            file.Close();
        }

        public byte[] randomFrameData()
        {
            byte[] data = new byte[FrameSize];
            Random random = new Random();

            random.NextBytes(data);
            data[0] = 0xAA;
            data[1] = 0xBB;
            data[2] = 0xCC;
            data[3] = 0xDD;
            return data;
        }

    }
}
