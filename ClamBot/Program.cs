using Discord;
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
            => new Program().MainAsync().GetAwaiter().GetResult();

        public async Task MainAsync()
        {
            _client = new DiscordSocketClient();
            _client.Log += Log;
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
    }
}