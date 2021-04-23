using Newtonsoft.Json.Linq;
using DSharpPlus.Entities;
using System.Threading.Tasks;

namespace Items {
  class Food : IConsumable {
    override public string name {get;}
    private int health;
    public Food(string name, int health) {
      this.name = name;
      this.health = health;
    }
    override protected async Task Consume(DiscordMessage msg) {
      owner.health += health;
      await msg.RespondAsync($"You ate a **{name}** and gained **{health}hp**, leaving you with **{owner.health}hp**");
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