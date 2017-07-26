using Discord;
using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace ClamBot
{
    public class Program
    {
        private DiscordSocketClient _client;
        private static Random rand = new Random();

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

            using (StreamReader sr = new StreamReader(File.OpenRead("./ClientDetails.txt")))
            {
                fileToken = await sr.ReadLineAsync();
            }

            return fileToken;
        }

        private async Task MessageReceived(SocketMessage message)
        {
            if (message.Content == "!ping" || message.Content.StartsWith("!ping "))
            {
                await message.Channel.SendMessageAsync("clam");
                await Task.Delay(1000);
                await message.Channel.SendMessageAsync("i mean pong");
            }
            else if (message.Content == "!clam" || message.Content.StartsWith("!clam "))
            {
                GetRandomClam(message);
            }
            else if (message.Content == "!guppy" || message.Content.StartsWith("!guppy"))
            {
                await message.Channel.SendMessageAsync("fine, i'll go to bed");
                throw new Exception("Going to bed");
            }
        }

        private void GetRandomClam(SocketMessage msg)
        {
            Console.WriteLine("Getting files in the folder...");
            List<string> clamPics = Directory.EnumerateFiles("./clams/") as List<string>;
            Console.WriteLine("wow gottem");

            int index = rand.Next(0, clamPics.Count);

            msg.Channel.SendFileAsync(clamPics[index]?.ToString(), $"Debug test: image {index}");

            return;
        }
    }
}