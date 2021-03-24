using System.IO;
using System.Threading.Tasks;
using DSharpPlus;
using Microsoft.Extensions.Configuration;
using Commands;

namespace Botter_rewrite
{
    class Program
    {
        static void Main(string[] args)
        {
            CommandManager.AddCommands();
            var config = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("settings.json").Build();
            MainAsync(config.GetConnectionString(args[0])).GetAwaiter().GetResult();
        }

        static async Task MainAsync(string token) {
            await Database.Connect();
            
            DiscordClient client = new DiscordClient(new DiscordConfiguration() {
                Token = token,
                TokenType = TokenType.Bot
            });

            client.MessageCreated += CommandManager.OnMessage;
            client.MessageDeleted += Snipe.MessageDelete;

            await client.ConnectAsync();

            await Task.Delay(-1);
        }
    }
}
