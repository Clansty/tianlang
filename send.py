import groups
from mirai import Plain, Mirai
from appmgr import app
from user import User

async def major(msg):
    await group(groups.major, msg)


async def console(msg):
    await group(groups.console, msg)


async def test(msg):
    await group(groups.test, msg)


async def group(groupid, msg):
    await app.sendGroupMessage(groupid, msg)

async def private(uin, msg):
    if type(uin) is User:
        uin=uin.uin
    await app.sendTempMessage(groups.major, uin, msg)