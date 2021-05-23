using System;
using Telegram.Bot;
using Telegram.Bot.Args;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;
using Telegram.Bot.Types.InlineQueryResults;
using System.IO;
using System.Threading.Tasks;
namespace Hangman
{
    class Program
    {
        static TelegramBotClient Bot;
        static void Main(string[] args)
        {
            Bot = new TelegramBotClient("1828186140:AAFfj8EelqMQczCvbz2La6XQbBa5W525uB4");

            Bot.OnMessage += BotOnMessageRecived;
            Bot.OnCallbackQuery += Bot_OnCallbackQuery;

            var me = Bot.GetMeAsync().Result;

            Console.WriteLine(me.FirstName);

            Bot.StartReceiving();
            Console.ReadLine();
            Bot.StopReceiving();
        }

        private static void Bot_OnCallbackQuery(object sender, Telegram.Bot.Args.CallbackQueryEventArgs e)
        {
            throw new NotImplementedException();
        }

        private static async void BotOnMessageRecived(object sender, Telegram.Bot.Args.MessageEventArgs e)
        {
            var message = e.Message;

            if (message == null || message.Type != MessageType.Text)
            {
                Console.WriteLine("Erorr!");
                return;
            }

            Console.WriteLine(message.Text);

            switch (message.Text)
            {
                case "/start":
                    string text_start = "Commands:\n/play - start a new game\n/help - "+
                        "displays the list of available commands\n" +
                        "/letter - to guess a letter\n/word - to guess the whole word";
                    await Bot.SendTextMessageAsync(message.From.Id, text_start);
                        break;
                case "/play":
                    string text_play = "Let's start! I have selected the word."+
                        " So choose command /letter or /word to start guessing!" +
                        " Your word is :\n" + Generate_word();
                    await Bot.SendTextMessageAsync(message.From.Id, text_play);
                    break;
                case "/help":
                    string text_help = "In our game i'm going to select the word and you need to guess it."+
                        " You can see how to play it in real life here.\n"+
                        "https://www.wikihow.com/Play-Hangman";
                    await Bot.SendTextMessageAsync(message.From.Id, text_help);
                    break;
                case "/letter":
                    string text_letter = "Okey, sent me a letter!";
                    await Bot.SendTextMessageAsync(message.From.Id, text_letter);
                    break;
                case "/word":
                    string text_word = "Okey, sent me a whole word!";
                    await Bot.SendTextMessageAsync(message.From.Id, text_word);

                    break;
                default:
                    string text_def = "I don't know this command";
                    await Bot.SendTextMessageAsync(message.From.Id, text_def);
                    break;
            }
            // read current word from file
            static  string Get_word(int index)
            {
                string path = "C:/Users/38095/Desktop/Hangman/Hangman/words.txt", line="";

                using (StreamReader sr = new StreamReader(path, System.Text.Encoding.Default))
                {
                    for(int i = 0; i != index; ++i)
                    {
                        if ((line = sr.ReadLine()) == null)
                        {
                            Console.WriteLine("Error");
                            return " ";
                        }
                    }
                }
               
                return line;
            }
            // generate line of _ 
            static string Generate_word(){
                Random rand = new Random();
                string val = Get_word(rand.Next(1,4));
                Console.WriteLine(val);
                string result = "";
                for(int i = 0; i < val.Length; ++i)
                {
                    result += "_ ";
                }
                return result ;
            }
        }
    }
}
