using Newtonsoft.Json.Linq;
using DSharpPlus.Entities;
using System.Threading.Tasks;
using StatusEffects;

namespace Items {
  [Item(
    10,
    "bandage",
    1,
    10,
    "Health",
    "Give youself regen for 10 minutes when used, you'll ultimately regen 12 health after the 10 minutes",
    "Regen a bit more health when used than bandaids"
  )]
  class Bandage : IConsumable {
    override public string name {get;} = "bandage";

    override protected async Task Consume(DiscordMessage msg) {
      IStatusEffect.AddStatusEffect<Regen>(owner, 600, 0.02F, msg.Channel);
      await msg.RespondAsync($"You used a **bandage** and got **regen** for **10 minutes**");
    }

    override public JObject Serialize() {
      return new JObject();
    }

    override public void Deserialize(JObject str) {
      return;
    }
    override public string Display() {
      return "";
    }
  }
}