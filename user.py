class User:
    uin:int
    def __init__(self, user):
        if(user.__class__.__name__=='Friend'):
            self.uin=user.id
        elif(user.__class__.__name__=='Member'):
            self.uin=user.id
        else:
            self.uin=user
    
    