Go语言实现版本

```
go run gen.go   // generate files
cd server && go run server.go
cd client && go run client.go -file "测试文件1"
```
# 说明
1. 运行gen.go生成测试文件1和测试文件2(文件1默认`320kb`)
2. 启动tcp服务端, 监听`127.0.0.1:34567`
3. 启动客户端, 读取测试文件, 默认每次读取 `1024 bytes`, 读取之后发给服务端
4. 服务器端接收数据，解析，分帧，存储