using System;
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
    public static DiscordClient client;
    static void Main(string[] args)
    {
      CommandManager.AddCommands();
      var config = new ConfigurationBuilder().AddJsonFile("C:/All-items/projects/Botter rewrite/settings.json").Build();
      GoogleAPIKey = config.GetConnectionString("googleAPIKey");
      CseId = config.GetConnectionString("cseId");
      MainAsync(config.GetConnectionString(args[0])).GetAwaiter().GetResult();
    }

    static async Task MainAsync(string token) {
      await Database.Connect();

      DiscordClient nonStaticClient = new DiscordClient(new DiscordConfiguration() {
          Token = token,
          TokenType = TokenType.Bot
      });

      client = nonStaticClient;

      nonStaticClient.MessageCreated += CommandManager.OnMessage;
      nonStaticClient.MessageDeleted += Snipe.MessageDelete;
      nonStaticClient.MessageUpdated += EditHistory.OnEdit;

      await nonStaticClient.ConnectAsync();

      await Task.Delay(-1);
    }

    static void OnProcessExit() {
      foreach (User user in Database.userCache.Values) {
        user.updateData();
      }
    }
  }
}
