using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using DSharpPlus;
using DSharpPlus.Entities;

namespace Commands {
  class Sacrifice : ICommand {
    public string help {get;} = "Sacrifice part of your health to satan and get coins in return";
    public string helpShort {get;} = "Sacrifice health to get coins";
    public string[] signature {get;} = new string[] {"<amt>"};
    public TypoableString[] aliases {get;} = new TypoableString[] {new TypoableString("sacrifice", 3)};
    public string category {get;} = "Advanced";
    public bool admin {get;} = false;
    public async Task Exec(DiscordClient client, Args args, DiscordMessage msg, Guild guild, User user) {
      int amt;

      try {
        amt = int.Parse(args.strings["amt"]);
      } catch {
        await msg.RespondAsync($"\"{args.strings["amt"]}\" is an invalid number, or it's too big to parse");
        return;
      }

      if (user.health <= amt) {
        await msg.RespondAsync("You can't sacrifice more health than you have");
      } else if (amt < 0) {
        await msg.RespondAsync("Satan laughs you out of the room when you say you want to buy human flesh from him");
      } else if (amt == 0) {
        await msg.RespondAsync("Satan is confused and slightly annoyed that you wanted to trade nothing for nothing.");
      } else {
        user.health -= amt;
        user.TransferCoins(amt);
        await msg.RespondAsync($"Satan is very pleased by your offer and accepts it. You now have **{user.health}hp** and **{user.coins}c**");
      }
    }
  }
}