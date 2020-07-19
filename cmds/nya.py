import send
async def run(usr, arg:str):
    if(not arg):
        arg='0'
    try:
        t=int(arg)
        r=""
        for _ in range(t):
            r+="nya "
        r=r.strip()
        if(r):
            await send.console(r)
        else:
            await send.console('nya?')
    except Exception as e:
        await send.console(f"nya {e.__str__()}?")

