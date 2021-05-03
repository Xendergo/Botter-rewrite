using DSharpPlus.Entities;
using System;
using StatusEffects;

namespace Items {
  class FrogLauncher : IRanged {
    public override int maxDamage {get;} = 30;
    public override string name {get;} = "frog-launcher";
    public override string ammo {get;} = "frog";
    public override int damageToDeal {get;} = 0;
    protected override string OnHit(BattleEntity target, BattleEntity attacker, DiscordMessage msg) {
      IStatusEffect.AddStatusEffect<Poison>(target, 40, 0.5F, msg.Channel);
      return "You now have **poison** for **40 seconds**";
    }
  }
}