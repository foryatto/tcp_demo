files = input().split(" ")

with open(files[0], "rb") as f1:
    with open(files[1]+"_0", "rb") as f2:
        d1 = f1.read()
        d2 = f2.read()

        print(d1 == d2, len(d1), len(d2))

    all = b''
    for i in range(1,11):
        filename = files[1]+"_"+f"{i}"
        with open(filename, "rb") as temp:
            frame = temp.read()
            all += frame
            if frame[0]==0xAA and frame[1]==0xBB and frame[2]==0xCC and frame[3]==0xDD:
                print(f"文件{filename}前四个字节为AA BB CC DD")
    print(d1 == all, len(d1), len(all))


