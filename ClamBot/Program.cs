using Discord;
using Discord.Commands;
using Discord.WebSocket;
using System;
using System.IO;
using System.Threading.Tasks;

namespace ClamBot
{
    public class Program
    {
        private DiscordSocketClient _client;

        public static void Main(string[] args)
        {
            try
            {
                new Program().MainAsync().GetAwaiter().GetResult();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return;
            }
        }

        public async Task MainAsync()
        {
            _client = new DiscordSocketClient();

            _client.Log += Log;
            _client.MessageReceived += MessageReceived;

            string token = await ReadToken();

            await _client.LoginAsync(TokenType.Bot, token);
            await _client.StartAsync();

            await Task.Delay(-1);
        }

        private Task Log(LogMessage msg)
        {
            Console.WriteLine(msg.ToString());
            return Task.CompletedTask;
        }

        private async Task<string> ReadToken()
        {
            string fileToken;

            using (StreamReader sr = new StreamReader(File.OpenRead("ClientDetails.txt")))
            {
                fileToken = await sr.ReadLineAsync();
            }

            return fileToken;
        }

        private async Task MessageReceived(SocketMessage message)
        {
            if (message.Content == "!ping")
            {
                await message.Channel.SendMessageAsync("clam");
                await Task.Delay(1000);
                await message.Channel.SendMessageAsync("i mean pong");
            }
            else if (message.Content == "!clam")
            {
                await GetRandomClam(message);
            }
            else if (message.Content == "!guppy")
            {
                await message.Channel.SendMessageAsync("fine, i'll go to bed");
                throw new Exception("Going to bed");
            }
        }

        private async Task GetRandomClam(SocketMessage msg)
        {
            await msg.Channel.SendMessageAsync("what, you want me to google a random clam or something? pfft. searching costs money, kiddo...");
        }
    }
}