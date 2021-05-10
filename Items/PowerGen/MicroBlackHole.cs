using Newtonsoft.Json.Linq;
using StatusEffects;

namespace Items {
  [Item(
    8.9F,
    "micro-black-hole",
    2,
    200,
    "Electricity",
    "Generate 10000kw for 10 seconds, once expired, deals 100 damage to you and gives everyone stun",
    "Generate 10000kw for 10 seconds, once expired, deals 100 damage to you and gives everyone stun"
  )]
  class MicroBlackHole : IItem, IPowerGen {
    private int timeLeft = 10;
    public override string name {get;} = "micro-black-hole";
    public bool enabled {get; set;} = true;
    
    public int CalculatePower() {
      return 10000;
    }

    public int GeneratePower() {
      timeLeft--;
      if (timeLeft == 0) {
        removeSelf();

        owner.battle.MostRecentChannel.SendMessageAsync($"Your black hole has expired! Everyone is stunned now");

        owner.health -= 100;

        foreach (var player in owner.battle.players) {
          IStatusEffect.AddStatusEffect<Stun>(player, 10, 1, owner.battle.MostRecentChannel);
        }
      }

      return 10;
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