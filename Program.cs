using System.IO;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DSharpPlus;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.Json;

namespace Botter_rewrite
{
    class Program
    {
        public static Dictionary<string, ICommand> commands = new Dictionary<string, ICommand>();
        static void Main(string[] args)
        {
            var config = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("settings.json").Build();
            MainAsync(config.GetConnectionString(args[0])).GetAwaiter().GetResult();
        }

        static async Task MainAsync(string token) {
            DiscordClient client = new DiscordClient(new DiscordConfiguration() {
                Token = token,
                TokenType = TokenType.Bot
            });

            await client.ConnectAsync();
            await Task.Delay(-1);
        }
    }
}
