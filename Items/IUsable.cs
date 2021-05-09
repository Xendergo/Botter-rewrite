using DSharpPlus.Entities;
using System.Threading.Tasks;

namespace Items {
  interface IUsable {
    public Task Use(DiscordMessage msg);
  }
}