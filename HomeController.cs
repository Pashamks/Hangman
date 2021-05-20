
using System.Web.Mvc;
using Telegram.Bot.Types;

namespace Hangman.Controllers
{
    public class HomeController : Controller
    {
        private const string Token = "";
        private readonly Telegram.Bot.TelegramBotClient _client = new(Token);
        [HttpPost]
        public async void Post([FromBody] Update update)
        {
            if (update == null)
            {
                return;
            }
            var message = update.Message;
            if (message?.Type == MessageType.Text)
            {
                await _client.SendTextMessageAsync(message.Chat.Id, message.Text);
            }
        }
}