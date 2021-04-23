using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using DSharpPlus;
using DSharpPlus.Entities;
using Items;

namespace Commands {
  class Inv : ICommand {
    public string help {get;} = "See your inventory as well as other game data";
    public string helpShort {get;} = "See your inventory & game data";
    public string[] signature {get;} = new string[] {"@?player"};
    public TypoableString[] aliases {get;} = new TypoableString[] {new TypoableString("inv", 0), new TypoableString("inventory", 2)};
    public string category {get;} = "Game";
    public bool admin {get;} = false;
    public async Task Exec(DiscordClient client, Args args, DiscordMessage msg, Guild guild, User user) {
      if (args.users.ContainsKey("player")) {
        SendInv(msg, await Database.getUser(args.users["player"]), (await client.GetUserAsync(args.users["player"])).Username);
      } else {
        SendInv(msg, user, msg.Author.Username);
      }
    }

    public static async void SendInv(DiscordMessage msg, User user, string username) {
      string healthBar = new string('▨', (int)(user.health / 100.0 * 17)).PadRight(17, '▢');
      await msg.RespondAsync($@"```
  {username}'s inventory

       Health - {user.health}
+--------------------------+
|{healthBar}|
+--------------------------+

Coins - {user.coins}
Electricity generation - {user.electricity}kw
Magic - {user.magic}

Items
{GenerateItemList(user)}
```");
    }

    private static string GenerateItemList(User user) {
      List<string> itemString = new List<string>();
      List<int> quantity = new List<int>();

      foreach (IItem item in user.items) {
        string display = item.Display();
        string str = item.name + (display == "" ? "" : " - " + display);

        if (itemString.Contains(str)) {
          quantity[itemString.IndexOf(str)]++;
          continue;
        }

        itemString.Add(str);
        quantity.Add(1);
      }

      List<string> ret = new List<string>();
      for (int i = 0; i < itemString.Count; i++) {
        ret.Add($"x{quantity[i]} | {itemString[i]}");
      }

      return string.Join("\n", ret);
    }
  }
}