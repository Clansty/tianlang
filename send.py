import groups
import nonebot


def major(msg: str):
    group(groups.major, msg)


def console(msg: str):
    group(groups.console, msg)


def test(msg: str):
    group(groups.test, msg)


async def group(groupid, msg: str):
    await nonebot.get_bot().send_group_msg(group_id=groupid, message=msg)
