from mirai import Mirai

qq = 2981882373  # 字段 qq 的值
authKey = 'QwQQAQOwO'  # 字段 authKey 的值
# httpapi所在主机的地址端口,如果 setting.yml 文件里字段 "enableWebsocket" 的值为 "true" 则需要将 "/" 换成 "/ws", 否则将接收不到消息.
mirai_api_http_locate = 'localhost:8080/ws'
app = Mirai(f"mirai://{mirai_api_http_locate}?authKey={authKey}&qq={qq}")

