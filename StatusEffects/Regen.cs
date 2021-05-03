using System;

namespace StatusEffects {
  class Regen : IStatusEffect {
    public override string name {get;} = "Regen";
    protected override void onTick() {
      if (MathF.Round(ticksLeft % (1 / intensity)) == 0) {
        owner.health++;
      }
    }
  }
}