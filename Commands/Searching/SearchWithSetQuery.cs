using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using DSharpPlus;
using DSharpPlus.Entities;

namespace Commands {
  class SearchWithSetQuery : ICommand {
    public string help {get;}
    public string helpShort {get;}
    public string[] signature {get;}
    public TypoableString[] aliases {get;}
    public string category {get;}
    private string query;
    public bool admin {get;} = false;
    public SearchWithSetQuery(string name, string query, int maxTypos) {
      this.query = query;
      help = $"Search for a random image of a {query} on google";
      helpShort = $"Search for a {query}";
      signature = new string[] {};
      aliases = new TypoableString[] {new TypoableString(name, maxTypos)};
      category = "Searching";
    }

    public async Task Exec(DiscordClient client, Args args, DiscordMessage msg, Guild guild, User user) {
      try {
        await msg.RespondAsync(await Search.RequestImage(query, user));
      } catch (ArgumentNullException e) {
        await msg.RespondAsync("Couldn't find a result for that");
      } catch {
        await msg.RespondAsync("Making that request failed, botter could be getting rate limited");
      }
    }
  }
}