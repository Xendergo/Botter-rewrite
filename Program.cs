using System.IO;
using System.Threading.Tasks;
using DSharpPlus;
using Microsoft.Extensions.Configuration;
using Commands;

namespace Botter_rewrite
{
    class Program
    {
        public static string GoogleAPIKey;
        public static string CseId;
        static void Main(string[] args)
        {
            CommandManager.AddCommands();
            var config = new ConfigurationBuilder().AddJsonFile("C:/All items/projects/Botter rewrite/settings.json").Build();
            GoogleAPIKey = config.GetConnectionString("googleAPIKey");
            CseId = config.GetConnectionString("cseId");
            MainAsync(config.GetConnectionString(args[0])).GetAwaiter().GetResult();
        }

        static async Task MainAsync(string token) {
            connect:

            try {
                await Database.Connect();
            } catch {
                goto connect;
            }

            DiscordClient client = new DiscordClient(new DiscordConfiguration() {
                Token = token,
                TokenType = TokenType.Bot
            });

            client.MessageCreated += CommandManager.OnMessage;
            client.MessageDeleted += Snipe.MessageDelete;
            client.MessageUpdated += EditHistory.OnEdit;

            await client.ConnectAsync();

            await Task.Delay(-1);
        }
    }
}
