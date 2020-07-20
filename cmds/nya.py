desc='nya nya nya!'
args=True

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
            return r
        else:
            return 'nya?'
    except Exception as e:
        return f"nya {e.__str__()}?"

