using DSharpPlus.Entities;
using System.Threading.Tasks;

namespace Items {
  abstract class IUsable : IItem {
    public abstract Task Use(DiscordMessage msg);
  }
}