import pymysql
global my
def init():
    global my
    my = pymysql.connect(
        host="cdb-pi7fvpvu.cd.tencentcdb.com",
        port=10058,
        user="root",
        passwd="t00rrooT",
        db="tianlang_dev")
    
def getdb():
    global my
    return my