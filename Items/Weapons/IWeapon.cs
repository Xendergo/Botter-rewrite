using DSharpPlus.Entities;
using System.Threading.Tasks;

namespace Items {
  public abstract class IWeapon<T> : IDegradable {
    public async Task<T> Attack(BattleEntity target, BattleEntity attacker, DiscordMessage msg) {
      damage++;

      if (damage == maxDamage) {
        await removeSelf();
        await msg.RespondAsync($"Your {name} broke!");
      }

      return await DoAttack(target, attacker, msg);
    }

    protected abstract Task<T> DoAttack(BattleEntity target, BattleEntity attacker, DiscordMessage msg);
  }
}