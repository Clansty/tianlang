import os
from importlib import import_module as imp
import send

# !暂时不做权限验证
async def praseCommand(usr, msg:str):
    msg=msg.strip()
    msg=msg.split(maxsplit=1)
    if(os.path.exists(f"cmds/{msg[0]}.py")):
        cmd=imp(f"cmds.{msg[0]}")
        if cmd.args:
            # 命令需要参数
            if len(msg)==1:
                #没给参数
                await send.console(f"{cmd}: {cmd.desc}\n\n参数不够")
                return
            await send.console(await cmd.run(usr,msg[1]))
            return
        await send.console(await cmd.run(usr))
