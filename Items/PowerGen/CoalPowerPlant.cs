using System;
using Newtonsoft.Json.Linq;
using StatusEffects;

namespace Items {
  [Item(
    8.5F,
    "coal-power-plant",
    3,
    100,
    "Electricity",
    "Generate 200kw while you have coal, consumes coal every 20 seconds, gives you illness while in use",
    "Generate 200kw, consumes coal, gives you illness while in use"
  )]
  class CoalPowerPlant : IItem, IPowerGen {
    private int timeLeft = 0;
    public override string name {get;} = "coal-power-plant";
    public bool enabled {get; set;} = true;
    private bool prevEnabled = true;
    private bool generatingPower = false;
    private Illness illnessEffect = null;
    
    public int CalculatePower() {
      return prevEnabled ? 200 : 0;
    }

    public int GeneratePower() {
      if (timeLeft == 0) {
        Coal coal = owner.GetItem<Coal>();

        if (coal is null) {
          if (prevEnabled) {
            owner.battle.MostRecentChannel.SendMessageAsync("You ran out of coal! Coal power plant is inactive");
            prevEnabled = false;
          }

          return 0;
        }

        coal.removeSelf();

        timeLeft = 20;
      }

      prevEnabled = true;
      generatingPower = true;

      timeLeft--;

      return 200;
    }

    public override void BattleTick() {
      generatingPower = false;
    }

    public override void AfterBattleTick() {
      if (generatingPower && illnessEffect is null) {
        illnessEffect = IStatusEffect.AddStatusEffect<Illness>(owner, -1, 0.125F, owner.battle?.MostRecentChannel);
        owner.battle?.MostRecentChannel?.SendMessageAsync("Your coal power plant is active, you now have illness effect");
      } else if (!generatingPower && illnessEffect is not null) {
        illnessEffect.RemoveSelf();
        illnessEffect = null;
      }
    }

    public override string Display() {
      return $"enabled: {enabled}";
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