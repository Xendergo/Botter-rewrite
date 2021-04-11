using System.Security.Cryptography;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using DSharpPlus;
using DSharpPlus.Entities;

namespace Commands {
  class StatsCmd : ICommand {
    public string help {get;} = "See your statistics, or ping someone to see their statistics";
    public string helpShort {get;} = "See your statistics";
    public string[] signature {get;} = new string[] {"@?user"};
    public TypoableString[] aliases {get;} = new TypoableString[] {new TypoableString("stats", 1), new TypoableString("statistics", 3)};
    public string category {get;} = "Misc";
    public bool admin {get;} = false;
    public async Task Exec(DiscordClient client, string[] args, DiscordMessage msg, Guild guild, User user) {
      if (args.Length == 0) {
        await SendStats(msg, user, msg.Author.Username);
      } else {
        await SendStats(msg, await Database.getUser(msg.MentionedUsers.First().Id), msg.MentionedUsers.First().Username);
      }
    }

    public static async Task SendStats(DiscordMessage message, User user, string name) {
      DiscordEmbedBuilder embed = new DiscordEmbedBuilder();

      embed.WithColor(new DiscordColor(0x00FF00));
      embed.WithTitle("Stats of " + name);
      embed.WithDescription($@"Times they got sniped - {user.stats.GotSniped}
Times they sniped someone - {user.stats.PeopleSniped}
Times they sniped themselves - {user.stats.SelfSniped}
Kill count - {user.stats.PeopleKilled}
Death count - {user.stats.Died}
Images searched for - {user.stats.Searched}
Times they interacted with botter - {user.stats.Interactions}");

      await message.RespondAsync(embed);
    }
  }
}