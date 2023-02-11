# tcp_demo

- `gen` 生成帧文件
- `client` 客户端
- `server` 服务端

# 运行
- 运行`gen.cs`生成测试文件
- 运行`server.cs`启动服务端
- 运行`client.cs`启动客户端上传文件 (与测试文件在同一目录下)

# 优化
- [x] 传输文件的时候带上文件名, 服务端从字节数据中解析出文件名, 无需额外重命名
( 注意`TCP粘包问题`, 采用byte[0]表示文件名的长度, 由服务器端解析处理 )

- [x] 分帧功能改用队列, 字节数据存入队列, 遇到帧头则将队列里的数据存入文件, 清空队列

# 测试
```
$ python result_test.py
测试文件1
测试文件1_1前四个字节为AA BB CC DD
测试文件1_2前四个字节为AA BB CC DD
测试文件1_3前四个字节为AA BB CC DD
测试文件1_4前四个字节为AA BB CC DD
测试文件1_5前四个字节为AA BB CC DD
测试文件1_6前四个字节为AA BB CC DD
测试文件1_7前四个字节为AA BB CC DD
测试文件1_8前四个字节为AA BB CC DD
测试文件1_9前四个字节为AA BB CC DD
测试文件1_10前四个字节为AA BB CC DD
True 327680 327680

$ python result_test.py
测试文件2
测试文件2_2前四个字节为AA BB CC DD
测试文件2_3前四个字节为AA BB CC DD
测试文件2_4前四个字节为AA BB CC DD
测试文件2_5前四个字节为AA BB CC DD
测试文件2_6前四个字节为AA BB CC DD
测试文件2_7前四个字节为AA BB CC DD
测试文件2_8前四个字节为AA BB CC DD
测试文件2_9前四个字节为AA BB CC DD
测试文件2_10前四个字节为AA BB CC DD
True 327676 327676
```