using Newtonsoft.Json.Linq;
using DSharpPlus.Entities;
using System.Threading.Tasks;

namespace Items {
  [Item(
    6,
    "potato",
    1,
    2,
    "Food",
    "Heal 2 health when used, also used as ammo for potato cannon",
    "Heal 2 health when used, also used as ammo for potato cannon",
    args = new object[] {"potato", 2}
  )]
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

    override public string Display() {
      return "";
    }
  }
}