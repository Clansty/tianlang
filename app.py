import nonebot
import config
from os import path
import db
import groups

db.init()
groups.init(True)
nonebot.init(config)
nonebot.load_plugins('cmds', 'cmds')
nonebot.load_plugins('antirepeating', 'antirepeating')
nonebot.run()
