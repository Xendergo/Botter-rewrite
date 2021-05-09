using System;

namespace StatusEffects {
  [Effect(2, "Regen", "Regenerate health over time")]
  class Regen : IStatusEffect {
    public override string name {get;} = "Regen";
    protected override void onTick() {
      if (MathF.Round(ticksLeft % (1 / intensity)) == 0) {
        owner.health++;
      }
    }
  }
}