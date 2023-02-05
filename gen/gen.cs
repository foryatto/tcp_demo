using System;

namespace tcp_demo
{
    class GenFrameFile {

        const int FrameSize = 32 * 1024;
        const int FrameNums = 10;
        const int Size = FrameSize * FrameNums;

        public static void Main(string[] args) {
            var gen = new GenFrameFile();
            gen.generateFrameFile();
        }
        
        public void generateFrameFile()
        {
            byte[] frameData = randomFrameData();
            saveToFile("测试文件1", frameData);

            byte[] frameData2 = new byte[Size-4];
            for(int i = 0; i < Size-4; i++)
            {
                frameData2[i] = frameData[i+4];
            }
            saveToFile("测试文件2", frameData2);
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
            byte[] data = new byte[Size];
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
