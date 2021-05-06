using System.Threading.Tasks;
using DSharpPlus.Entities;
using StatusEffects;

namespace Items {
  [Item(
    1,
    "petrified-poop-sword",
    4,
    40,
    "Weapons",
    "Deal 1 damage, but give the illness effect for 30 seconds, breaks after 25 uses",
    "Give your target an illness"
  )]
  public class PetrifiedPoopSword : IMelee {
    override public string name {get;} = "petrified-poop-sword";
    public override int maxDamage {get;} = 25;
    public override int damageToDeal {get;} = 1;
    override protected async Task<string> DoAttack(BattleEntity target, BattleEntity stabber, DiscordMessage msg) {
      IStatusEffect.AddStatusEffect<Illness>(target, 30, 0.25F, msg.Channel);
      return $"You got **Illness** for **30 seconds**";
    }
  }
}