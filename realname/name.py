def name(name:str) -> (int, int):
    from db import db
    sql=f"SELECT id FROM person WHERE name='{name}'"
    cursor=db.cursor()
    cursor.execute(sql)
    all=cursor.fetchall()
    if len(all)==0:
        return(0,0)
    if len(all)>1:
        return(len(all),0)
    # 只有一个
    pid=all[0][0]
    sql=f"SELECT id FROM user WHERE bind={pid}"
    cursor.execute(sql)
    all=cursor.fetchall()
    if len(all)==0:
        return(1,0)
    return(1,all[0][0])