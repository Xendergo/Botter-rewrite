using System;

namespace StatusEffects {
  class Regen : IStatusEffect {
    public override string name {get;} = "Regen";
    protected override void onTick() {
      Console.WriteLine(MathF.Round(ticksLeft % (1 / intensity)));
      Console.WriteLine(ticksLeft);
      if (MathF.Round(ticksLeft % (1 / intensity)) == 0) {
        owner.health++;
      }
    }
  }
}