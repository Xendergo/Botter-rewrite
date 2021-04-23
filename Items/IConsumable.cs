using DSharpPlus.Entities;
using System.Threading.Tasks;

namespace Items {
  abstract class IConsumable : IUsable {
    override public async Task Use(DiscordMessage msg) {
      removeSelf();
      await Consume(msg);
    }

    protected abstract Task Consume(DiscordMessage msg);
  }
}