from mirai import Mirai, Plain, MessageChain, Friend, Group, Member, FriendMessage, GroupMessage, TempMessage, Image
import asyncio
import groups
from appmgr import app
from user import User
from cmds import praseCommand
from db import db

if(__name__ == "__main__"):

    groups.init(True)

    @app.receiver("GroupMessage")
    async def _(group: Group, member: Member, message: GroupMessage):
        if(group.id == groups.major):
            import repeater
            await repeater.handle(message.toString())
        if(group.id==groups.console and message.messageChain.getFirstComponent(Plain).text):
            await praseCommand(member, message.messageChain.getFirstComponent(Plain).text)

    @app.receiver("TempMessage")
    async def _(usr: Member, message: TempMessage):
        await FMHandler(usr,message)

    @app.receiver("FriendMessage")
    async def _(usr: Friend, message: FriendMessage):
        await FMHandler(usr,message)

    async def FMHandler(usr, msg):
        pass
    app.run()
