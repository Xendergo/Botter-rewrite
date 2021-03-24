using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using DSharpPlus;
using DSharpPlus.Entities;
using DSharpPlus.EventArgs;

namespace Commands {
  class Snipe : ICommand {
    public string help {get;}
    public string helpShort {get;}
    public string[] signature {get;}
    public TypoableString[] aliases {get;}
    public string category {get;}
    public Snipe() {
      help = "Snipe the most recently deleted message, deleted messages are saved for at least 10 minutes";
      helpShort = "Snipe a deleted message";
      signature = new string[] {};
      aliases = new TypoableString[] {new TypoableString("snipe", 1)};
      category = "Sniping";
    }

    public async Task Exec(DiscordClient client, string[] args, DiscordMessage msg, Guild guild) {
      Channel channel = guild.getChannel(msg.ChannelId);

      if (channel.DeletedMessageStack.Last is null) {
        await msg.RespondAsync("There's nothing to snipe!");
        return;
      }

      DeletedMessage snipedMsg = channel.popDeletedMessage();
      DiscordEmbedBuilder embed = new DiscordEmbedBuilder();

      embed.WithColor(new DiscordColor(0xFF0000));
      embed.WithDescription(snipedMsg.Message);
      embed.WithFooter($"- {snipedMsg.AuthorUsername}");

      await msg.RespondAsync(embed);
    }

    public static async Task MessageDelete(DiscordClient client, MessageDeleteEventArgs args) {
      Guild guild = await Database.getGuild(args.Guild.Id);

      Channel channel = guild.getChannel(args.Channel.Id);

      channel.pushDeletedMessage(args.Message);
    }
  }
}