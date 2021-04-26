using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using DSharpPlus;
using DSharpPlus.Entities;
using Items;
using StatusEffects;

namespace Commands {
  class Stab : IBattleCommand {
    public override string help {get;} = "Stab someone";
    public override string helpShort {get;} = "Stab someone";
    public override string[] signature {get;} = new string[] {"@target", "<weapon>"};
    public override TypoableString[] aliases {get;} = new TypoableString[] {new TypoableString("stab", 1), new TypoableString("s", 0)};
    protected override async Task Exec(DiscordClient client, Args args, DiscordMessage msg, Guild guild, User user, Battle battle) {
      IMelee weapon = user.GetItem(args.strings["weapon"]) as IMelee;

      if (weapon is null) {
        throw new CommandException("That item isn't a melee weapon");
      }

      User target = await Database.getUser(args.users["target"]);

      float illnessIntensity;

      Optional<Illness> illness = user.GetEffect<Illness>();

      if (illness.HasValue) {
        illnessIntensity = illness.Value.intensity;
      } else {
        illnessIntensity = 0;
      }

      int damage = (int)(weapon.damageToDeal * (1 - illnessIntensity));

      target.health -= damage;
      string message = await weapon.Attack(await Database.getUser(args.users["target"]), msg);

      await msg.RespondAsync($"Oof, **{await user.username}** stabbed **{await target.username}** with a **{weapon.name}**, dealing **{damage}hp**, leaving them with **{target.health}hp**" + (message == "" ? "" : " - " + message));
    }
  }
}