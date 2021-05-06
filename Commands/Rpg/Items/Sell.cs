using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using DSharpPlus;
using DSharpPlus.Entities;
using Items;

namespace Commands {
  [Command(20)]
  class Sell : ICommand {
    public string help {get;} = "Sell an item to get your money back";
    public string helpShort {get;} = "Sell an item";
    public string[] signature {get;} = new string[] {"<item>", "<?amt>"};
    public TypoableString[] aliases {get;} = new TypoableString[] {new TypoableString("sell", 1)};
    public string category {get;} = "Rpg";
    public bool admin {get;} = false;
    public async Task Exec(DiscordClient client, Args args, DiscordMessage msg, Guild guild, User user) {
      string itemName = TypoableString.FindClosestString(args.strings["item"], ItemRegistry.items.Keys).value;

      if (itemName == null) {
        throw new CommandException($"There is no item named {args.strings["item"]}");
      }

      foreach (IItem item in user.items) {
        if (item.name == itemName) {
          if (item is not ISellable) {
            throw new CommandException($"The item {itemName} can't be sold");
          }

          int price = await (item as ISellable).sell();

          await msg.RespondAsync($"You sold one **{itemName}** for **{price} coins**");
          return;
        }
      }

      throw new CommandException($"You don't have a {itemName}");
    }
  }
}