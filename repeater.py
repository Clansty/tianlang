import groups
import send

global last
global times
dd = "打断"
dg = "出现了打断怪"
last = ""
times = 1


async def handle(msg: str):
    global last
    global times
    if(msg == last):
        times += 1
        if(times == 4):
            times = 1
            if(last == dd):
                await send.major(dg)
                last = dg
            else:
                await send.major(dd)
                last = dd
    else:
        times = 1
        last = msg
