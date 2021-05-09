using Newtonsoft.Json.Linq;

namespace Items {
  [Item(
    8,
    "solar-panel",
    2,
    10,
    "Electricity",
    "Generate 10kw for 10 minutes",
    "Generate 10kw for 10 minutes"
  )]
  class SolarPanel : IItem, IPowerGen {
    private int timeLeft = 600;
    public override string name {get;} = "solar-panel";
    public bool enabled {get; set;} = true;
    
    public int CalculatePower() {
      return 10;
    }

    public int GeneratePower() {
      timeLeft--;
      if (timeLeft == 0) removeSelf();
      return 10;
    }

    public override string Display() {
      return $"Seconds left - {timeLeft}";
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