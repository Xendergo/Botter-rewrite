using DSharpPlus.Entities;
using System;
using StatusEffects;

namespace Items {
  [Item(
    5,
    "frog-launcher",
    1,
    60,
    "Weapons",
    "Deal 0 damage, can be used 30 times, requires frogs for ammo, gives your opponent poison for 40 seconds, doing 20 damage in total",
    "Deal 0 damage, gives your opponent poison"
  )]
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