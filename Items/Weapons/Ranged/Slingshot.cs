using DSharpPlus.Entities;
using System;

namespace Items {
  class Slingshot : IRanged {
    public override int maxDamage {get;} = 50;
    public override string name {get;} = "slingshot";
    public override string ammo {get;} = null;
    public override int damageToDeal {get;} = 5;
  }
}