using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using DSharpPlus;
using DSharpPlus.Entities;

namespace Commands {
  [CommandAttribute(18)]
  class Menu : ICommand {
    public string help {get;} = "See a list of all the items you can buy, or get info for a specific one";
    public string helpShort {get;} = "See a list of all the items you can buy";
    public string[] signature {get;} = new string[] {"<?item>"};
    public TypoableString[] aliases {get;} = new TypoableString[] {new TypoableString("menu", 1), new TypoableString("store", 2)};
    public string category {get;} = "Rpg";
    public bool admin {get;} = false;
    public async Task Exec(DiscordClient client, Args args, DiscordMessage msg, Guild guild, User user) {
      if (args.strings.ContainsKey("item")) {
        TypoableString corrected = TypoableString.FindClosestString(args.strings["item"], ItemRegistry.items.Keys);

        if (corrected is null) {
          await msg.RespondAsync($"There is no command with name `{args.strings["item"]}`");
          return;
        }

        ItemEntry item = ItemRegistry.items[corrected];
        await msg.RespondAsync($@"
**{item.name}**
Costs **{item.price}c**, {(item.price > user.coins ? "You need **" + (item.price - user.coins) + "** more coins to afford this item" : "You can afford this item")}
{item.description}
        ");
      } else {
        await msg.RespondAsync($@"
Note that there is a tax whenever coins are exchanged, depending on how many coins the user has and the amount of coins being transacted

All the items available:
{GenerateItemList()}");
      }
    }

    private static string GenerateItemList() {
      List<string> items = new List<string>();

      string pCategory = "";
      foreach (KeyValuePair<TypoableString, ItemEntry> item in ItemRegistry.items) {
        if (pCategory != item.Value.category) {
          items.Add("");
          items.Add($"**{item.Value.category}:**");
        }

        items.Add($"`{item.Key.value}` - `{item.Value.price}c`: {item.Value.shortDescription}");
        pCategory = item.Value.category;
      }

      return string.Join('\n', items);
    }
  }
}