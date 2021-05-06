using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using DSharpPlus;
using DSharpPlus.Entities;

namespace Commands {
  [Command(19)]
  class Buy : ICommand {
    public string help {get;} = "Buy an item from the store";
    public string helpShort {get;} = "Buy an item";
    public string[] signature {get;} = new string[] {"<item>", "<?amt>"};
    public TypoableString[] aliases {get;} = new TypoableString[] {new TypoableString("buy", 0), new TypoableString("purchase", 3)};
    public string category {get;} = "Rpg";
    public bool admin {get;} = false;
    public async Task Exec(DiscordClient client, Args args, DiscordMessage msg, Guild guild, User user) {
      await ItemRegistry.BuyItem(args.strings["item"], msg, user);
    }
  }
}