using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using DSharpPlus;
using DSharpPlus.Entities;

namespace Commands {
  [CommandAttribute(16)]
  class CalcTax : ICommand {
    public string help {get;} = "Calculate the amount of sales tax you would have to pay for a transaction";
    public string helpShort {get;} = "Calculate tax for a transaction";
    public string[] signature {get;} = new string[] {"#coins"};
    public TypoableString[] aliases {get;} = new TypoableString[] {new TypoableString("tax", 0), new TypoableString("calctax", 1), new TypoableString("calculatetax", 3)};
    public string category {get;} = "Rpg";
    public bool admin {get;} = false;
    public async Task Exec(DiscordClient client, Args args, DiscordMessage msg, Guild guild, User user) {
      int amt = (int) args.nums["coins"];
      int tax = User.calcTax(amt, user.coins + amt);
      await msg.RespondAsync($@"Your tax on receiving **{amt} coins** would be **{tax} coins**
(1.3 ^ ((**{user.coins}** + **{amt}**) / 1000) - 1) * sqrt(|**{amt}**|) = **{tax}**");
    }
  }
}