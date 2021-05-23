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
        static bool b_play = false, b_write = false, b_letter = false;

        static string current_word = "", letter_word;

        static TelegramBotClient Bot;
        static void Main(string[] args)
        {
            Bot = new TelegramBotClient("...");

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
                    string text_start = "Commands:\n/play - start a new game\n/help - " +
                        "displays the list of available commands\n" +
                        "/letter - to guess a letter\n/word - to guess the whole word";
                    await Bot.SendTextMessageAsync(message.From.Id, text_start);
                    break;
                case "/play":
                    string text_play = "Let's start! I have selected the word." +
                        " So choose a command /letter or /word to start guessing!" +
                        " Your word is :\n" + Generate_word();
                    await Bot.SendTextMessageAsync(message.From.Id, text_play);
                    b_play = true;
                    b_write = false;
                    b_letter = false;
                    break;
                case "/help":
                    string text_help = "In our game i'm going to select the word and you need to guess it." +
                        " You can see how to play it in real life here.\n" +
                        "https://www.wikihow.com/Play-Hangman";
                    await Bot.SendTextMessageAsync(message.From.Id, text_help);
                    break;
                case "/letter":
                    string text_letter = "Okey, sent me a letter!";
                    await Bot.SendTextMessageAsync(message.From.Id, text_letter);
                    b_write = false;
                    b_letter = true;
                    break;
                case "/word":
                    string text_word = "Okey, sent me a whole word!";
                    await Bot.SendTextMessageAsync(message.From.Id, text_word);
                    b_write = true;
                    b_letter = false;
                    break;
                default:
                    if (b_play)
                    {
                        if (b_write)
                        {
                            await Bot.SendTextMessageAsync(message.From.Id, Compare_word(current_word, message.Text));
                        }
                        else if (b_letter)
                        {
                            await Bot.SendTextMessageAsync(message.From.Id, Find_letter(message.Text[0]));
                        }
                        else
                        {
                            string text_p = "Choose the command /letter or /word to start guessing!";
                            await Bot.SendTextMessageAsync(message.From.Id, text_p);
                        }
                    }
                    else if (b_letter || b_write)
                    {
                        await Bot.SendTextMessageAsync(message.From.Id, "Sorry, I thought that you were gone... Please,"+
                            "enter /play again");
                    }
                    else
                    {
                        string text_def = "I don't know this command";
                        await Bot.SendTextMessageAsync(message.From.Id, text_def);
                    }
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
                current_word = Get_word(rand.Next(1,11));
                
                Console.WriteLine(current_word);
                string result = "";
                for(int i = 0; i < current_word.Length; ++i)
                {
                    result += "_ ";
                }
                return result ;
            }

            static string Find_letter(char letter)
            {
                if(!current_word.Contains(letter))
                return "I'm so sorry...There is no letter like that in my word.";

                string letter_result = "Your word after guessing:\n", temp_word = "";
                for(int i =0; i < current_word.Length; ++i)
                {
                    if (current_word[i] == letter)
                    {
                        temp_word += letter;
                        temp_word += " ";
                    }
                    else
                    {
                            temp_word += "_ ";
                    } 
                }

                if (letter_word == null)
                {
                    letter_word = temp_word;
                }
                    
                else
                {
                    int i = 0;
                    foreach(var ch in letter_word)
                    {
                        if (letter_word[i] == '_' || letter_word[i] == ' ')
                        {
                            letter_word = letter_word.Remove(i, 1);
                            letter_word = letter_word.Insert(i, temp_word[i].ToString());
                        }
                        ++i;
                    }
                   
                }
                letter_result += letter_word;
                
                if(letter_word.Replace(" ", "") == current_word)
                {
                    return "Congratulation! You won the game ! To start a new game enter /play";
                }

                return letter_result;
            }

            static string Compare_word(string str1, string str2)
            {
                if(str1 == str2)
                {
                    return "Congratulation! You won the game ! To start a new game enter /play";
                }
                b_play = false;
                return "I'm so sorry, but you lost...";
            }
        }
    }
}
