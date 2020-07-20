from realname import name, knownEnrollments
from user import User

def check(u:User) -> (bool,int):
    # bool: UserV3.Verified
    # int: UserV3.VerifyMsg, 0=OK, 1=unsupported, 2=notFound, 3=occupied
    # bind 有值就是 OK
    if u.get('bind')[0] is not None:
        return(True, 0)
    # 不支持
    enr=u.getEnrollment()
    if not enr in knownEnrollments.knownList:
        return(True, 1)
    # 开始绑定
    # 获得姓名 pid
    from db import db
    sql=f"SELECT id FROM person WHERE name='{u.get('name')[0]}'"
    cursor=db.cursor()
    cursor.execute(sql)
    al=cursor.fetchall()
    if len(al)==0:
        return(False, 2)
    if len(al)>1:
        # TODO: 核对其他信息
        pass
    pid=al[0][0]
    # 检测是否 pid 占用
    sql=f"SELECT id FROM user WHERE bind={pid}"
    cursor=db.cursor()
    cursor.execute(sql)
    al=cursor.fetchall()
    if len(al)>0:
        return(False,3)
    # 绑定
    u.set(bind=pid)
    return check(u)