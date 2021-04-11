using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using DSharpPlus;
using DSharpPlus.Entities;

namespace Commands {
  class Prefix : ICommand {
    public string help {get;}
    public string helpShort {get;}
    public string[] signature {get;}
    public TypoableString[] aliases {get;}
    public string category {get;}
    public bool admin {get;} = true;
    public Prefix() {
      help = "Change the server's prefix, `botter` will still be a usable prefix";
      helpShort = "Change the server's prefix";
      signature = new string[] {"<prefix>"};
      aliases = new TypoableString[] {new TypoableString("prefix", 1)};
      category = "Settings";
    }

    public async Task Exec(DiscordClient client, string[] args, DiscordMessage msg, Guild guild) {
      await guild.setPrefix(args[0]);
    }
  }
}