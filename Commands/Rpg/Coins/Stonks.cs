using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using DSharpPlus;
using DSharpPlus.Entities;
using Items;

namespace Commands {
  [CommandAttribute(15)]
  class Stonks : ICommand {
    public string help {get;} = "Trade stocks in game using real life prices";
    public string helpShort {get;} = "Trade stocks";
    public string[] signature {get;} = new string[] {"action:buy|sell", "<ticker>", "#?coins"};
    public TypoableString[] aliases {get;} = new TypoableString[] {new TypoableString("stonks", 1), new TypoableString("stocks", 1), new TypoableString("stock", 1), new TypoableString("stonk", 1)};
    public string category {get;} = "Rpg";
    public bool admin {get;} = false;
    public async Task Exec(DiscordClient client, Args args, DiscordMessage msg, Guild guild, User user) {
      if (args.enums["action"] == "buy") {
        if (!args.nums.ContainsKey("coins")) {
          throw new CommandException("Must provide an amount of coins worth of the stock to buy");
        }

        if (user.coins < args.nums["coins"]) {
          throw new CommandException("You don't have enough coins to buy that much stock");
        }

        Stonk stonk = new Stonk(args.strings["ticker"]);
        stonk.owner = user;
        await stonk.buyStock(user, (int)args.nums["coins"]);

        await msg.RespondAsync($"Successfully bought **{args.nums["coins"]} coins** worth of **{args.strings["ticker"]}**");
      } else {
        foreach (IItem item in user.items) {
          if (item is Stonk) {
            Stonk stonk = item as Stonk;

            if (stonk.ticker == args.strings["ticker"]) {
              int sellPrice = await stonk.sellStock();
              await msg.RespondAsync($"Successfully sold **{args.strings["ticker"]}** for **{sellPrice} coins**, that you originally bought for **{stonk.coinAmt} coins**");
              return;
            }
          }
        }
      }
    }
  }
}