using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using DSharpPlus;
using DSharpPlus.Entities;
using Items;

namespace Commands {
  [Command(21)]
  class Use : ICommand {
    public string help {get;} = "Use an item";
    public string helpShort {get;} = "Use an item";
    public string[] signature {get;} = new string[] {"<item>"};
    public TypoableString[] aliases {get;} = new TypoableString[] {new TypoableString("use", 1)};
    public string category {get;} = "Rpg";
    public bool admin {get;} = false;
    public async Task Exec(DiscordClient client, Args args, DiscordMessage msg, Guild guild, User user) {
      TypoableString itemName = TypoableString.FindClosestString(args.strings["item"], ItemRegistry.items.Keys);

      if (itemName is null) {
        await msg.RespondAsync($"There is no item named *{args.strings["item"]}*");
        return;
      }

      foreach (IItem item in user.items) {
        if (item.name == itemName.value) {
          if (item is not IUsable) {
            await msg.RespondAsync("This item can't be used");
            return;
          }

          await (item as IUsable).Use(msg);
          return;
        }
      }

      await msg.RespondAsync("You don't have this item");
    }
  }
}