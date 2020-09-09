﻿using System.Linq;
using Telegram.Bot.Args;

namespace Clansty.tianlang
{
    internal static class Tg
    {
        internal const string Token = "712657902:AAG4eachKPVtfZeLpeR-8lAbDVy_6AMwASg";
        internal static async void OnMsg(object sender, MessageEventArgs e)
        {
            if (e.Message.Text != null)
            {
                var action = e.Message.Text.Split(' ')[0];
                switch (action)
                {
                    case "/id":
                        C.TG.SendTextMessageAsync(e.Message.Chat, 
                            $"会话 ID: {e.Message.Chat.Id}\n" +
                            $"用户 ID: {e.Message.From.Id}");
                        return;
                    case "/start":
                        return;
                }

                ;
                if (G.Map.ContainsKey(e.Message.Chat.Id))
                {
                    C.QQ.SendGroupMsg(G.Map[e.Message.Chat.Id],
                        e.Message.From.FirstName + ":\n" +
                        Utf.Encode(e.Message.Text));
                }
            }

            string ps = null;
            if (e.Message.Photo != null)
            {
                ps = e.Message.Photo.Last().FileId;
            }

            if (e.Message.Sticker != null)
            {
                ps = e.Message.Sticker.FileId;
            }

            if (ps is null) return;
            if (!G.Map.ContainsKey(e.Message.Chat.Id)) return;
            var targ = G.Map[e.Message.Chat.Id];
            var pf = await C.TG.GetFileAsync(ps);
            var hash = await C.QQ.UploadGroupPic(targ, $"https://api.telegram.org/file/bot{Token}/{pf.FilePath}");
            var tos = e.Message.From.FirstName + ":\n";
            if (!string.IsNullOrWhiteSpace(e.Message.Caption))
            {
                tos += e.Message.Caption + "\n";
            }

            tos = Utf.Encode(tos);
            tos += hash;
            C.QQ.SendGroupMsg(targ, tos);
        }
    }
}