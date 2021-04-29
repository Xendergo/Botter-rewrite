using DSharpPlus.Entities;
using System.Threading.Tasks;

namespace Items {
  public abstract class IWeapon : IDegradable {
    public async Task<string> Attack(User target, User attacker, DiscordMessage msg) {
      damage++;

      if (damage == maxDamage) {
        await removeSelf();
        await msg.RespondAsync($"Your {name} broke!");
      }

      return await DoAttack(target, attacker, msg);
    }

    protected abstract Task<string> DoAttack(User target, User attacker, DiscordMessage msg);
  }
}