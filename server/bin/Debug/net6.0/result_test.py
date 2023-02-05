with open("测试文件1", "rb") as f1:
    with open("1_all", "rb") as f2:
        b1 = f1.read()
        b2 = f2.read()
        print(b1==b2, len(b1), len(b2))


