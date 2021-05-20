using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services.Description;
using Telegram.Bot;

namespace Hangman.Models
{
    public class play : Command
    {
        public override string Name => "play";

        public override async void ExecuteAsync(Message message, TelegramBotClient client)
        {
            var chatId = message.Chat.Id;
            var messageId = message.MessageId;

            await client.SendTextMessageAsync(chatId, "Try to guess word!", replyToMessageId: messageId);
        }
    }
}