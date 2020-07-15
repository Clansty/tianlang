from nonebot import on_command, CommandSession
import db


# on_command 装饰器将函数声明为一个命令处理器
# 这里 weather 为命令的名字，同时允许使用别名「天气」「天气预报」「查天气」
@on_command('info')
async def info(session: CommandSession):
    cursor=db.getdb().cursor()
    sql = "SELECT * FROM user WHERE id = '%s' "
    arg=session.current_arg_text.strip()
    if(arg):
        cursor.execute(sql % arg)
        results=cursor.fetchall()
        for row in results:
            print(row)
            await session.send(row.__str__())


