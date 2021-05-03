using System;

namespace StatusEffects {
  class Poison : IStatusEffect {
    public override string name {get;} = "Poison";
    protected override void onTick() {
      if (MathF.Round(ticksLeft % (1 / intensity)) == 0) {
        owner.health--;
      }
    }
  }
}