using Discord;
using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
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
                Console.ReadLine();
                return;
            }
        }

        public async Task MainAsync()
        {
            // Stupid .NET Core design choices
            Directory.SetCurrentDirectory(Path.GetDirectoryName(Assembly.GetEntryAssembly().Location));

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
            using (StreamReader sr = new StreamReader(File.OpenRead("./ClientDetails.txt")))
            {
                return await sr.ReadLineAsync();
            }
        }

        private async Task MessageReceived(SocketMessage message)
        {
            // This bot only works in #clams on Lars & Friends.
            if (message.Channel.Id != 339906131227050004)
            {
                return;
            }

            if (message.Content == "!ping" || message.Content.StartsWith("!ping "))
            {
                await Ping(message);
            }
            else if (message.Content == "!clam" || message.Content.StartsWith("!clam "))
            {
                await GetRandomClam(message);
            }
            else if (message.Content == "!guppy" || message.Content.StartsWith("!guppy"))
            {
                await message.Channel.SendMessageAsync("fine, i'll go to bed");
                throw new Exception("Going to bed");
            }
        }

        private async Task Ping(SocketMessage msg)
        {
            if (rand.Next() % 16 == 0)
            {
                await msg.Channel.SendMessageAsync("pong");
            }
            else
            {
                await msg.Channel.SendMessageAsync("clam");
                await Task.Delay(500);
                await msg.Channel.SendMessageAsync("er");
                await msg.Channel.SendMessageAsync("pong");
            }
        }

        private async Task GetRandomClam(SocketMessage msg)
        {            
            List<string> clamPics = Directory.EnumerateFiles("./clams").ToList();

            int index = rand.Next(0, clamPics.Count);

            await msg.Channel.SendFileAsync(clamPics[index].ToString());
        }
    }
}