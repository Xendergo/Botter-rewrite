using DSharpPlus.Entities;
using System;

namespace Items {
  class PotatoGun : IRanged {
    public override int maxDamage {get;} = 20;
    public override string name {get;} = "potato-gun";
    public override string ammo {get;} = "potato";
    public override int damageToDeal {get;} = 12;
  }
}