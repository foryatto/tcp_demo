package main

import (
	"fmt"
	"log"
	"net"
	"os"
	"sync"
)

var (
	fileNum int = 0
	mutx    sync.Mutex
)

func main() {
	listen, err := net.Listen("tcp", "localhost:34567")
	if err != nil {
		panic(err)
	}
	fmt.Printf("Server is running on 127.0.0.1:34567")
	for {
		conn, err := listen.Accept()
		if err != nil {
			log.Println(err)
			continue
		}
		mutx.Lock()
		fileNum++
		mutx.Unlock()
		go getFrameData(conn, fileNum)
	}

}

func getFrameData(conn net.Conn, fnum int) {

	defer conn.Close()

	data := []byte{}
	count := 0

	idx := 0
	lastIdx := 0
	found := false

	for {
		var buf [1024]byte
		n, err := conn.Read(buf[:])
		if err != nil {
			log.Println(err)
			break
		}
		if n == 0 {
			break
		}

		data = append(data, buf[:n]...)

		for ; idx < len(data)-3; idx++ {
			if data[idx] != byte(0xAA) {
				continue
			}
			if data[idx+1] == byte(0xBB) && data[idx+2] == byte(0xCC) && data[idx+3] == byte(0xDD) {
				found = true
				break
			}
		}
		if found {
			if idx != 0 {
				count++
				saveTofile(fmt.Sprintf("%d_%d", fnum, count), data[lastIdx:idx])
				lastIdx = idx
			}
			idx++
			found = false
		}
	}
	if !found {
		count++
		saveTofile(fmt.Sprintf("%d_%d", fnum, count), data[lastIdx:idx])
	}
}

func saveTofile(filename string, data []byte) {
	file, _ := os.OpenFile(filename, os.O_RDWR|os.O_CREATE, 0777)
	file.Write(data)
	defer file.Close()
}

// func getSequenceNumber(b []byte) uint32 {
// 	return binary.BigEndian.Uint32(b)
// }
