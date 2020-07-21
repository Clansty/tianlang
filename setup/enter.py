from user import User
import send

async def enter(u:User, step:int, msg:str):
    async def finish():
        u.setNamecard()
        u.set(status=0, step=0)
        await send.private(u,f'你的群名片已修改为 {u.properNamecard()}')
        await send.private(u,"目前我们需要的信息就这么多，祝你在群里玩的开心")
        #todo 广告位
        await send.console(repr(u))
    u.set(name=msg.strip())
    chk=u.check()
    if chk[1]==0:
        await finish()
        return
    if chk[1]==3:
        await send.private(u, '看上去此实名身份已经有另一个账号加入，请联系管理员处理')
        await finish()
        return
    
