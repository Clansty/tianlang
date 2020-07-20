def name(name:str) -> (int, int):
    from db import db
    sql=f"SELECT id FROM people WHERE name='{name}'"
    cursor=db.cursor()
    cursor.execute(sql)
    all=cursor.fetchall()
    if len(all)==0:
        return(0,0)
    if len(all)>1:
        return(2,0)
    