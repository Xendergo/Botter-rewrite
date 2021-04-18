using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using DSharpPlus;
using DSharpPlus.Entities;

namespace Commands {
  class Inv : ICommand {
    public string help {get;} = "See your inventory as well as other game data";
    public string helpShort {get;} = "See your inventory & game data";
    public string[] signature {get;} = new string[] {"@?player"};
    public TypoableString[] aliases {get;} = new TypoableString[] {new TypoableString("inv", 0), new TypoableString("inventory", 2)};
    public string category {get;} = "Game";
    public bool admin {get;} = false;
    public async Task Exec(DiscordClient client, Args args, DiscordMessage msg, Guild guild, User user) {
      await msg.RespondAsync("https://github.com/Xendergo/Botter-rewrite");
    }
  }
}