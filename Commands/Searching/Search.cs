using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using DSharpPlus;
using DSharpPlus.Entities;
using GoogleApi.Entities.Search.Image.Request;
using Botter_rewrite;
using GoogleApi.Entities.Search.Common;
using GoogleApi.Entities.Search.Common.Enums;
using GoogleApi;

namespace Commands {
  [CommandAttribute(6)]
  class Search : ICommand {
    private static Random rand = new Random();
    public string help {get;}
    public string helpShort {get;}
    public string[] signature {get;}
    public TypoableString[] aliases {get;}
    public string category {get;}
    public bool admin {get;} = false;
    public Search() {
      help = "Search google for a random image using your search query";
      helpShort = "Search for a random image";
      signature = new string[] {"<query>"};
      aliases = new TypoableString[] {new TypoableString("search", 1)};
      category = "Searching";
    }

    public async Task Exec(DiscordClient client, Args args, DiscordMessage msg, Guild guild, User user) {
      try {
        await msg.RespondAsync(await RequestImage(args.strings["query"], user));
      } catch (ArgumentNullException e) {
        await msg.RespondAsync("Couldn't find a result for that");
      } catch {
        await msg.RespondAsync("Making that request failed, botter could be getting rate limited");
      }
    }

    public static async Task<string> RequestImage(string query, User user) {
      user.stats.Searched++;
      
      ImageSearchRequest req = new ImageSearchRequest{
        Key = Program.GoogleAPIKey,
        SearchEngineId = Program.CseId,
        Query = query,
        Options = new SearchOptions{
          SafetyLevel = SafetyLevel.Off,
          Number = 1,
          StartIndex = rand.Next(50)
        }
      };

      var response = await GoogleSearch.ImageSearch.QueryAsync(req);

      return response.Items.First().Link;
    }
  }
}