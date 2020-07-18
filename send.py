import groups
from mirai import Plain, Mirai
from appmgr import app


async def major(msg: str):
    await group(groups.major, msg)


async def console(msg: str):
    await group(groups.console, msg)


async def test(msg: str):
    await group(groups.test, msg)


async def group(groupid, msg: str):
    await app.sendGroupMessage(groupid, [Plain(msg)])
