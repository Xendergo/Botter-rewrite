using System;

namespace StatusEffects {
  [Effect(0, "Poison", "Do damage over time")]
  class Poison : IStatusEffect {
    public override string name {get;} = "Poison";
    protected override void onTick() {
      if (MathF.Round(ticksLeft % (1 / intensity)) == 0) {
        owner.health--;
      }
    }
  }
}