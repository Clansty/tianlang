from user import User
import send
async def start(uin:int):
    u=User(uin, True)
    chk=u.check()
    if chk[0] and u.getGrade() !='未知':
        raise Exception('看上去此人已经实名了或无需认证，更换实名信息需要走管理人工验证流程')
    u.set(status=1, step=1)
    await send.private(uin, '你好，我是甜狼，本群的人工智能管理')
    await send.private(uin, '为了保证本群的安全与秩序，请告诉我你的真实姓名只要姓名来验证你的身份，这并不会在群里公开\nPS：只要姓名，不要加别的东西')
    print(f"qq{uin}新人向导启动")

