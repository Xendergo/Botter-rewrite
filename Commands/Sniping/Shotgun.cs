using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using DSharpPlus;
using DSharpPlus.Entities;

namespace Commands {
  class Shotgun : ICommand {
    public string help {get;}
    public string helpShort {get;}
    public string[] signature {get;}
    public TypoableString[] aliases {get;}
    public string category {get;}
    public Shotgun() {
      help = "Snipe all the messages in a channel that are saved";
      helpShort = "Snipe all deleted messages";
      signature = new string[] {};
      aliases = new TypoableString[] {new TypoableString("shotgun", 1)};
      category = "Sniping";
    }

    public async Task Exec(DiscordClient client, string[] args, DiscordMessage msg, Guild guild) {
      Channel channel = guild.getChannel(msg.ChannelId);

      if (channel.DeletedMessageStack.Last is null) {
        await msg.RespondAsync("There's nothing to snipe!");
        return;
      }

      while (channel.DeletedMessageStack.Last is not null) {
        Snipe.SnipeMessage(channel, msg);
      }
    }
  }
}