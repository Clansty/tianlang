import nonebot
import config
from os import path
import db

db.init()
nonebot.init(config)
nonebot.load_plugins('cmds', 'cmds')
nonebot.run()
