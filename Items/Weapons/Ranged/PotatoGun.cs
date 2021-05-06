using DSharpPlus.Entities;
using System;

namespace Items {
  [Item(
    4,
    "potato-gun",
    1,
    50,
    "Weapons",
    "Deal 12 damage, can be used 20 times, requires potatoes as ammo",
    "Deal 12 damage, can be used 20 times, requires potatoes as ammo"
  )]
  class PotatoGun : IRanged {
    public override int maxDamage {get;} = 20;
    public override string name {get;} = "potato-gun";
    public override string ammo {get;} = "potato";
    public override int damageToDeal {get;} = 12;
  }
}