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