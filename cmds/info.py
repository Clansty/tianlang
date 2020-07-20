desc='根据 id 查人'
args=True

async def run(usr, arg):
    from user import User,UserNotFound
    try:
        u=User(int(arg))
        return u.__repr__()
    except UserNotFound:
        return "无记载"
    except Exception as e:
        return e.__repr__()