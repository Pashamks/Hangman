
using System.Web.Services.Description;
using Telegram.Bot;
using Telegram.Bot.Types;
using Message = Telegram.Bot.Types.Message;

namespace Hangman.Models
{
    public class Help : Command
    {
        public override string Name => "help";

     
        public override async void Execute(Message message, TelegramBotClient client)
        {
            var chatId = message.Chat.Id;
            var messageId = message.MessageId;

            await client.SendTextMessageAsync(chatId, "play  - start a new game\n" +
                "letter – to guess a letter\n" +
                "word – to guess a whole word\n" +
                "help – displays the list of available commands", replyToMessageId: messageId);
        }
    }
}