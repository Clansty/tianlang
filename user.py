class UserNotFound(Exception):
    pass

class User:
    uin: int

    def __init__(self, user):
        from mirai import Friend, Member
        if type(user) == Friend:
            self.uin = user.id
        elif type(user) == Member:
            self.uin = user.id
        else:
            self.uin = user
            
        from db import db
        cursor= db.cursor()
        sql=f"SELECT id FROM user WHERE id={self.uin}"
        cursor.execute(sql)
        al=cursor.fetchall()
        if len(al)==0:
            raise UserNotFound()

    def get(self, *rows: str):
        from db import db
        cursor = db.cursor()
        sql = f"SELECT {','.join(rows)} FROM user WHERE id = {self.uin}"
        cursor.execute(sql)
        results = cursor.fetchall()
        if len(results) == 0:
            raise Exception('no such user')
        else:
            return(results[0])

    def set(self, **kvps):
        mod = ""

        for i in kvps:
            if type(kvps[i]) == str:
                value = f"'{kvps[i]}'"
            else:
                value = kvps[i]
            mod += f",{i} = {value}"
        mod=mod.lstrip(',')
        sql=f"UPDATE user SET {mod} WHERE id = {self.uin}"
        from db import db
        try:
            cursor = db.cursor()
            cursor.execute(sql)
            db.commit()
        except Exception as err:
            db.rollback()
            print(f"err: {err.__str__()}")
            
    def getEnrollment(self) -> int:
        #TODO 先尝试绑定
        sql=f"SELECT person.enrollment FROM person,user WHERE user.bind=person.id and user.id={self.uin}"
        from db import db
        cursor=db.cursor()
        cursor.execute(sql)
        al=cursor.fetchall()
        if len(al)==1:
            return al[0][0]
        return self.get('enrollment')[0]