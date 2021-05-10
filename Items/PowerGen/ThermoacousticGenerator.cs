using System;
using Newtonsoft.Json.Linq;
using StatusEffects;

namespace Items {
  [Item(
    8.75F,
    "thermoacoustic-generator",
    5,
    15,
    "Electricity",
    "Generate 100kw for 3 minutes, gives you annoyance while in use",
    "Generate 100kw for 3 minutes, gives you annoyance while in use"
  )]
  class ThermoacousticGenerator : IItem, IPowerGen {
    private int timeLeft = 180;
    public override string name {get;} = "thermoacoustic-generator";
    public bool enabled {get; set;} = true;
    private bool generatingPower = false;
    private Annoyance annoyanceEffect = null;
    
    public int CalculatePower() {
      return 100;
    }

    public int GeneratePower() {
      if (timeLeft == 0) {
        removeSelf();

        // If the item gets removed, it can't get ticked
        AfterBattleTick();
      }

      generatingPower = true;

      timeLeft--;

      return 200;
    }

    public override void BattleTick() {
      generatingPower = false;
    }

    public override void AfterBattleTick() {
      if (generatingPower && annoyanceEffect is null) {
        annoyanceEffect = IStatusEffect.AddStatusEffect<Annoyance>(owner, -1, 0.25F, owner.battle?.MostRecentChannel);
        owner.battle?.MostRecentChannel?.SendMessageAsync("Your thermoacoustic generator is active, you now have annoyance effect");
      } else if (!generatingPower && annoyanceEffect is not null) {
        annoyanceEffect.RemoveSelf();
        annoyanceEffect = null;
      }
    }

    public override string Display() {
      return $"Seconds left: {timeLeft} - enabled: {enabled}";
    }

    public override JObject Serialize() {
      JObject ret = new JObject();
      ret["timeLeft"] = timeLeft;
      return ret;
    }

    public override void Deserialize(JObject str) {
      timeLeft = (int)str["timeLeft"];
    }
  }
}