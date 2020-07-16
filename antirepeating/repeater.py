from nonebot import message_preprocessor, NoneBot, aiocqhttp
from nonebot.plugin import PluginManager
import groups

global last
global times
dd = "打断"
dg = "出现了打断怪"
last=""
times=1

@message_preprocessor
async def _(bot: NoneBot, event: aiocqhttp.Event, plugin_manager: PluginManager):
    global last
    global times
    if(event.group_id != groups.test):
        return
    if(event.raw_message == last):
        times += 1
        if(times == 4):
            times = 1
            if(last == dd):
                await bot.send_group_msg(group_id=groups.test, message=dg)
                last = dg
            else:
                await bot.send_group_msg(group_id=groups.test, message=dd)
                last = dd
    else:
        times = 1
        last = event.raw_message
