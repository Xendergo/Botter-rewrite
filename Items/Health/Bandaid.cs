using Newtonsoft.Json.Linq;
using DSharpPlus.Entities;
using System.Threading.Tasks;
using StatusEffects;

namespace Items {
  class Bandaid : IConsumable {
    override public string name {get;} = "bandaid";

    override protected async Task Consume(DiscordMessage msg) {
      IStatusEffect.AddStatusEffect<Regen>(owner, 600, 0.01F, msg.Channel);
      await msg.RespondAsync($"You used a **bandaid** and got **regen** for **10 minutes**");
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