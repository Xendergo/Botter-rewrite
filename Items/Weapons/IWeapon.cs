using DSharpPlus.Entities;
using System.Threading.Tasks;

namespace Items {
  public abstract class IWeapon : IDegradable {
    public async Task<string> Attack(User target, DiscordMessage msg) {
      damage++;

      if (damage == maxDamage) {
        await removeSelf();
        await msg.RespondAsync($"Your {name} broke!");
      }

      return await DoAttack(target, msg);
    }

    protected abstract Task<string> DoAttack(User target, DiscordMessage msg);
  }
}