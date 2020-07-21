def enrollment(s: str) -> int:
    s = s.strip()
    if s == None or s == "":
        return 0
    # 去除多彩群名片标记
    if s.startswith('<') and '>' in s:
        s = s.partition('>')[2].strip()  # 文本取右边
    s = s.replace("丨", " | ") #gun
    s = s.replace("｜", " | ") #全角
    if '|' in s:
        m=enrollment(s.partition('|')[0].strip())
        # 就是，有|的时候处理|左边的，不行再整个处理
        if m>0:
            return m
    #左边无效或无左边
    #!年级中文对应 enr 在这里
    if '高一' in s or '高1' in s:
        return 2020
    if '高二' in s or '高2' in s:
        return 2019
    if '高三' in s or '高3' in s:
        return 2018
    if '级' in s:
        # 有级先考虑，不然就是届
        s=s.partition('级')[0].strip()
        s=''.join(list(filter(str.isnumeric, s))) # 只取数字
        s=int(s)
        if 0<s<100:
            s+=2000
        if s>2000:
            return s
        return 0
    if '届' in s:
        s=s.partition('届')[0].strip()
    s=''.join(list(filter(str.isnumeric, s))) # 只取数字
    if not s:
        return 0
    s=int(s)
    if 0<s<100:
        s+=2000
    if s>2000:
        return s-3
    return 0
def junior(s: str)->bool:
    s = s.strip()
    if s == None or s == "":
        return False
    s = s.replace("丨", " | ") #gun
    s = s.replace("｜", " | ") #全角
    if '|' in s:
        return junior(s.partition('|')[0].strip())
    return "初中" in s or "初三" in s or "初3" in s
