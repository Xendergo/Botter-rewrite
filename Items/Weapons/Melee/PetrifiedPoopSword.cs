using System.Threading.Tasks;
using DSharpPlus.Entities;
using System.Threading.Tasks;
using StatusEffects;

namespace Items {
  public class PetrifiedPoopSword : IMelee {
    override public string name {get;} = "petrified-poop-sword";
    public override int maxDamage {get;} = 25;
    public override int damageToDeal {get;} = 1;
    override protected async Task<string> DoAttack(User target, DiscordMessage msg) {
      IStatusEffect.AddStatusEffect<Illness>(target, 30, 0.25F, msg.Channel);
      return $"You got **Illness** for **30 seconds**";
    }
  }
}