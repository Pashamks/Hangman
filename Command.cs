using System.Web.Services.Description;
using Telegram.Bot;

namespace Hangman.Models
{
    public abstract class Command
    {
        public abstract string Name { get; }
        public abstract void ExecuteAsync(Message message, TelegramBotClient client);
        public bool Contains (string command)
        {
            return command.Contains(this.Name) && command.Contains(AppSettings.Name);
        }
    }
}