using System.Threading.Tasks;
using DSharpPlus.Entities;
using StatusEffects;

namespace Items {
  [Item(
    2,
    "taser",
    4,
    40,
    "Weapons",
    "Deal 0 damage, but give your opponent stun, but also give your stun as you hold the taser to them. Consumes 2kw. Effects last 10 seconds",
    "Tase your target"
  )]
  public class Taser : IMelee {
    override public string name {get;} = "taser";
    public override int maxDamage {get;} = 25;
    public override int damageToDeal {get;} = 1;
    override protected async Task<string> DoAttack(BattleEntity target, BattleEntity stabber, DiscordMessage msg) {
      Stun effect1 = IStatusEffect.AddStatusEffect<Stun>(target, 10, 1F, msg.Channel);
      Stun effect2 = IStatusEffect.AddStatusEffect<Stun>(target, 10, 1F, msg.Channel);
      ElectricityConsumer consumer = new ElectricityConsumer(stabber, (c) => 2);

      consumer.OutOfPower += async (c) => {
        c.RemoveSelf();
        effect1.RemoveSelf();
        effect2.RemoveSelf();
        await msg.RespondAsync($"You ran out of power! You can no longer tase {await target.username}");
      };

      consumer.DestroyAfter(10000);

      stabber.consumers.Add(consumer);
      return $"You are tasing {await target.username}, and your target got **Stun** for **10 seconds**";
    }
  }
}