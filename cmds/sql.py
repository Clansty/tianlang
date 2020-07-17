import groups, db
from nonebot import on_command, CommandSession, CommandGroup

sql=CommandGroup('sql', only_to_me=False)
@sql.command('query')
async def _(session: CommandSession):
    try:        
        cursor=db.getdb().cursor()
        arg=session.current_arg_text.strip()
        if(arg):
            cursor.execute(arg)
            results=cursor.fetchall()
            tosend=""
            for row in results:
                tosend+=row.__str__()
            tosend=tosend.strip('\n')
            if(tosend):
                await session.send(tosend)
            else:
                await session.send('无结果')
        else:
            await session.send('参数不够')
    except Exception as err:
        await session.send(err.__str__())

@sql.command('exec')
async def _(session: CommandSession):
    cursor = db.getdb().cursor()
    arg=session.current_arg_text.strip()
    if(arg):
        try:
            cursor.execute(arg)
            db.getdb().commit()
            await session.send('执行成功')
        except Exception as err:
            db.getdb().rollback()
            await session.send(err.__str__())
    else:
        await session.send('参数不够')
