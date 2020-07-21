from mirai import Mirai, Plain, MessageChain, Friend, Group, Member, FriendMessage, GroupMessage, TempMessage, Image, MemberJoinEvent
import asyncio
import groups
from appmgr import app
from user import User
from cmds import praseCommand
from db import db
import setup

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
        
    @app.receiver('MemberJoinEvent')
    async def _(g:Group, usr: Member):
        if g.id==groups.major:
            await setup.start(usr.id)

    async def FMHandler(usr, msg):
        u=User(usr)
        ss= u.get('status','step')
        if ss[0]==1:
            import setup
            await setup.enter(u,ss[1],msg.messageChain.getFirstComponent(Plain).text)
        pass
    app.run()
