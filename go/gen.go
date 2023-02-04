package main

import (
	"crypto/rand"
	"os"
)

// 32KB a frame
const DefaultSize = 32 * 1024
const FrameNums = 10

func GenerateFrameFile() {
	// open files
	file1, _ := os.OpenFile("测试文件1", os.O_RDWR|os.O_CREATE, 0777)
	file2, _ := os.OpenFile("测试文件2", os.O_RDWR|os.O_CREATE, 0777)
	defer file1.Close()
	defer file2.Close()

	// write data
	isFirst := true
	for i := 0; i < FrameNums; i++ {
		data := randomFrameData()
		file1.Write(data)

		if isFirst {
			isFirst = false
			file2.Write(data[4:])

		} else {
			file2.Write(data)
		}
	}
}

func randomFrameData() []byte {
	data := make([]byte, DefaultSize)
	rand.Read(data)
	data[0] = 0xAA
	data[1] = 0xBB
	data[2] = 0xCC
	data[3] = 0xDD
	return data
}

func main() {
	GenerateFrameFile()
}
