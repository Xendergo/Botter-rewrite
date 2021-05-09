using Newtonsoft.Json.Linq;

namespace Items {
  [Item(
    8.5F,
    "coal-power-plant",
    3,
    100,
    "Electricity",
    "Generate 200kw while you have coal, consumes coal every 20 seconds",
    "Generate 200kw while you have coal, consumes coal every 20 seconds"
  )]
  class CoalPowerPlant : IItem, IPowerGen {
    private int timeLeft = 0;
    public override string name {get;} = "coal-power-plant";
    public bool enabled {get; set;} = true;
    private bool prevEnabled = true;
    
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

      timeLeft--;

      return 200;
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