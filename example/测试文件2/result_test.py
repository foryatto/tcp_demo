filename = input()

with open(filename, "rb") as f1:
    d1 = f1.read()

    d2 = b''
    for i in range(1,11):
        subFile = filename+"_"+f"{i}"
        with open(subFile, "rb") as temp:
            frame = temp.read()
            d2 += frame
            if frame[0]==0xAA and frame[1]==0xBB and frame[2]==0xCC and frame[3]==0xDD:
                print(f"{subFile}前四个字节为AA BB CC DD")
    print(d1 == d2, len(d1), len(d2))


