class User:
    uin:int
    def __init__(self, user):
        if(user.__class__.__name__=='Friend'):
            self.uin=user.id
        elif(user.__class__.__name__=='Member'):
            self.uin=user.id
        else:
            self.uin=user
    
    def get(self, *rows:str):
        from db import db
        cursor=db.cursor()
        sql=f"SELECT {','.join(rows)} FROM user WHERE id = {self.uin}"
        cursor.execute(sql)
        results=cursor.fetchall()
        if(len(results)==0):
            raise Exception('no such user')
        else:
            return(results[0])