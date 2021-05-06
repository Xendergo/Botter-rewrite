using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using DSharpPlus;
using DSharpPlus.Entities;
using Items;

namespace Commands {
  [CommandAttribute(26)]
  class Shoot : IBattleCommand {
    public override string help {get;} = "Shoot someone";
    public override string helpShort {get;} = "Shoot someone";
    public override string[] signature {get;} = new string[] {"@target", "<weapon>"};
    public override TypoableString[] aliases {get;} = new TypoableString[] {new TypoableString("shoot", 1), new TypoableString("o", 0)};
    protected override async Task Exec(DiscordClient client, Args args, DiscordMessage msg, Guild guild, BattleEntity user, Battle battle) {
      IRanged weapon = user.GetItem(args.strings["weapon"]) as IRanged;

      if (weapon is null) {
        throw new CommandException("That item isn't a ranged weapon");
      }

      BattleEntity target = await Database.getUser(args.users["target"]);

      if (weapon.ammo is not null) {
        await user.GetItem(weapon.ammo).removeSelf();
      }

      string message = await weapon.Attack(await Database.getUser(args.users["target"]), user, msg);

      await msg.RespondAsync(message);
    }
  }
}