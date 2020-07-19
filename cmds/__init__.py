import os
from importlib import import_module as imp

async def praseCommand(usr, msg:str):
    msg=msg.strip()
    msg=msg.split(maxsplit=1)
    if(os.path.exists(f"cmds/{msg[0]}.py")):
        cmd=imp(f"cmds.{msg[0]}")
        if(msg.__len__()==1):
            arg=None
        else:
            arg=msg[1]
        await cmd.run(usr,arg)
