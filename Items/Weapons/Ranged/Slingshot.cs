using DSharpPlus.Entities;
using System;

namespace Items {
  [Item(
    3,
    "slingshot",
    1,
    20,
    "Weapons",
    "Deal 5 damage, can be used 50 times, required no ammo",
    "Deal 5 damage, lasts a while, requires no ammo"
  )]
  class Slingshot : IRanged {
    public override int maxDamage {get;} = 50;
    public override string name {get;} = "slingshot";
    public override string ammo {get;} = null;
    public override int damageToDeal {get;} = 5;
  }
}