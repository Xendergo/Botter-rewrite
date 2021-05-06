using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using DSharpPlus;
using DSharpPlus.Entities;

namespace Commands {
  [Command(27)]
  class Src : ICommand {
    public string help {get;} = "Get a link to Botter's github repository";
    public string helpShort {get;} = "Get a link to the source code";
    public string[] signature {get;} = new string[] {};
    public TypoableString[] aliases {get;} = new TypoableString[] {new TypoableString("source", 1), new TypoableString("src", 0)};
    public string category {get;} = "Advanced";
    public bool admin {get;} = false;
    public async Task Exec(DiscordClient client, Args args, DiscordMessage msg, Guild guild, User user) {
      await msg.RespondAsync("https://github.com/Xendergo/Botter-rewrite");
    }
  }
}