class UserNotFound(Exception):
    pass

class User:
    uin: int

    def __init__(self, user, new:bool=False):
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
            if not new:
                raise UserNotFound()
            sql=f"INSERT INTO user (id) VALUES ({self.uin})"
            cursor.execute(sql)

    def get(self, *rows: str):
        from db import db
        cursor = db.cursor()
        sql = f"SELECT {','.join(rows)} FROM user WHERE id = {self.uin}"
        cursor.execute(sql)
        results = cursor.fetchall()
        return(results[0])
    
    def getP(self, *rows: str):
        from db import db
        cursor = db.cursor()
        sql = f"SELECT person.{',person.'.join(rows)} FROM person,user WHERE user.bind=person.id and user.id={self.uin}"
        cursor.execute(sql)
        results = cursor.fetchall()
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
    
    def check(self) -> (bool,int):
        from realname import check
        return check(self)
    
    def __repr__(self):
        res=self.check()
        if res[1]==0:
            sql=f"SELECT person.enrollment,person.branch,person.junior,person.name,person.class FROM person,user WHERE user.bind=person.id and user.id={self.uin}"
            from db import db
            cursor=db.cursor()
            cursor.execute(sql)
            al=cursor.fetchall()
            return f"qq: {self.uin}\nenrollment: {al[0][0]}\nbranch: {al[0][1]}\njunior: {al[0][2]}\nname: {al[0][3]}\nclass: {al[0][4]}\ngrade: {self.getGrade()}"
        tr=self.get('enrollment','name')
        return f"qq: {self.uin}\nenrollment: {tr[0]}\nname: {tr[1]}\ngrade: {self.getGrade()}"
  
    def getJunior(self) -> str:
        #?有毛病，现在这种写法无法实现双重身份，所以初中就写在 user 表里
        if self.check()[1]==0:
            if self.getP('junior')[0]:
                return True
        return self.get('junior')[0]
    
    def getGrade(self) -> str:
        # enr=2020,junior=true -> 高一/2020届初中
        # enr=2017,junior=true -> 2020届初中
        enr=self.getEnrollment()
        res=self.check()
        t=""
        jun=False
        if res[1]==0:
            jun=self.getJunior()
        if enr==2018:
            t+="高三"
        if enr==2019:
            t+="高二"
        if enr==2020:
            t+="高一"
        # 各种物种
        if 2000<enr<2018:
            #已毕业
            if jun:
                return f"{enr+3}届初中"
            else:
                return f"{enr+3}届"
        #双重身份
        if jun:
            return(f"{t}/{enr}届初中")
        return t