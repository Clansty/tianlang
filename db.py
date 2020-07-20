import pymysql
db = pymysql.connect(
    host="cdb-pi7fvpvu.cd.tencentcdb.com",
    port=10058,
    user="root",
    passwd="t00rrooT",
    db="tianlang_dev")
print('db connected')
    