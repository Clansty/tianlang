from mirai import Mirai, Plain, MessageChain, Friend, Group, Member, FriendMessage, GroupMessage, TempMessage
import asyncio
import db
import groups
from appmgr import app
from user import User

if(__name__ == "__main__"):

    groups.init(True)
    db.init()

    @app.receiver("GroupMessage")
    async def _(group: Group, member: Member, message: GroupMessage):
        if(group.id == groups.major):
            import repeater
            await repeater.handle(message.toString())

    @app.receiver("TempMessage")
    async def _(group: Group, usr: Member, message: TempMessage):
        await FMHandler(usr,message)

    @app.receiver("FriendMessage")
    async def _(usr: Friend, message: FriendMessage):
        await FMHandler(usr,message)

    async def FMHandler(usr, msg):
        u=User(usr)
        print(u.get('name'))
        print(u.get('name', 'nick'))

    app.run()
