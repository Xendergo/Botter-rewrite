using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using DSharpPlus;
using DSharpPlus.Entities;

namespace Commands {
  class Debug : ICommand {
    public string help {get;}
    public string helpShort {get;}
    public string[] signature {get;}
    public TypoableString[] aliases {get;}
    public string category {get;}
    public bool admin {get;} = false;
    public Debug() {
      help = "Get details about any exceptions that occured recently";
      helpShort = "Get info about recent errors";
      signature = new string[] {};
      aliases = new TypoableString[] {new TypoableString("debug", 1)};
      category = "Advanced";
    }

    public async Task Exec(DiscordClient client, Args args, DiscordMessage msg, Guild guild, User user) {
      Channel channel = guild.getChannel(msg.ChannelId);

      if (channel.ErrorStack.Last is null) {
        await msg.RespondAsync("There are no errors to debug");
        return;
      }

      await msg.RespondAsync($@"```
{channel.popError()}
```");
    }
  }
}