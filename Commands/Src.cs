using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using DSharpPlus;
using DSharpPlus.Entities;

namespace Commands {
  class Src : ICommand {
    public string help {get;}
    public string helpShort {get;}
    public string[] signature {get;}
    public TypoableString[] aliases {get;}
    public string category {get;}
    public Src() {
      help = "Get a link to Botter's github repository";
      helpShort = "Get a link to the source code";
      signature = new string[] {};
      aliases = new TypoableString[] {new TypoableString("source", 1), new TypoableString("src", 0)};
      category = "Advanced";
    }

    public async Task Exec(DiscordClient client, string[] args, DiscordMessage msg, Guild guild) {
      await msg.RespondAsync("https://github.com/Xendergo/Botter-rewrite");
    }
  }
}