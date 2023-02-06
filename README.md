# tcp_demo

- gen 生成帧文件
- client 客户端
- server 服务端

# 运行
```
./gen/bin/Debug/net6.0/gen.exe
./server/bin/Debug/net6.0/server.exe
./client/bin/Debug/net6.0/client.exe (帧文件与client.exe要处于同级目录)
```

# 可优化
- [ ] 传输文件的时候带上文件名, 服务端从字节数据中解析出文件名, 无需额外重命名
- [ ] 分帧功能改用队列, 字节数据存入队列, 遇到帧头则将队列里的数据存入文件, 清空队列