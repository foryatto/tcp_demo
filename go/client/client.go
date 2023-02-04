package main

import (
	"flag"
	"fmt"
	"log"
	"net"
	"os"
)

var fileName = flag.String("file", "测试文件2", "文件名")

func main() {
	flag.Parse()

	server := "127.0.0.1:34567"

	conn, err := net.Dial("tcp", server)
	if err != nil {
		log.Panic(err)
	}

	fmt.Println("connection success")
	defer conn.Close()

	file, _ := os.Open(*fileName)
	buf := make([]byte, 1024)

	for {
		n, err := file.Read(buf)
		if err != nil {
			log.Panic(err)
		}
		if n == 0 {
			break
		}
		conn.Write(buf)
	}

}
