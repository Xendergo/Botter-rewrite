using System.Threading.Tasks;
using DSharpPlus.Entities;
using System.Threading.Tasks;

namespace Items {
  public class GoldenSword : IMelee {
    override public string name {get;} = "golden-sword";
    public override int maxDamage {get;} = 15;
    public override int damageToDeal {get;} = 15;
    override protected async Task<string> DoAttack(User target, User stabber, DiscordMessage msg) {
      return "";
    }
  }
}