using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using DSharpPlus;
using DSharpPlus.Entities;
using DSharpPlus.EventArgs;

namespace Commands {
  [Command(2)]
  class Snipe : ICommand {
    public string help {get;}
    public string helpShort {get;}
    public string[] signature {get;}
    public TypoableString[] aliases {get;}
    public string category {get;}
    public bool admin {get;} = false;
    public Snipe() {
      help = "Snipe the most recently deleted message, deleted messages are saved for at least 10 minutes";
      helpShort = "Snipe a deleted message";
      signature = new string[] {};
      aliases = new TypoableString[] {new TypoableString("snipe", 1)};
      category = "Sniping";
    }

    public async Task Exec(DiscordClient client, Args args, DiscordMessage msg, Guild guild, User user) {
      Channel channel = guild.getChannel(msg.ChannelId);

      if (channel.DeletedMessageStack.Last is null) {
        await msg.RespondAsync("There's nothing to snipe!");
        return;
      }

      SnipeMessage(channel, msg, user);
    }

    public static async void SnipeMessage(Channel channel, DiscordMessage msg, User sniper) {
      if (channel.DeletedMessageStack.Last is null) return;

      DeletedMessage snipedMsg = channel.popDeletedMessage();
      DiscordEmbedBuilder embed = new DiscordEmbedBuilder();

      sniper.stats.PeopleSniped++;
      if (sniper.id == snipedMsg.AuthorID) {
        sniper.stats.SelfSniped++;
        sniper.stats.GotSniped++;
      } else {
        User snipee = await Database.getUser(snipedMsg.AuthorID);
        snipee.stats.GotSniped++;
      }

      embed.WithColor(new DiscordColor(0xFF0000));
      embed.WithDescription(snipedMsg.Message);
      embed.WithFooter($"- {snipedMsg.AuthorUsername}");

      await msg.RespondAsync(embed);

      if (snipedMsg.Embeds.Count > 0) {
        await msg.RespondAsync("Embeds:");

        foreach (DiscordEmbed snipedEmbed in snipedMsg.Embeds) {
          await msg.RespondAsync(snipedEmbed);
        }
      }
    }

    public static async Task MessageDelete(DiscordClient client, MessageDeleteEventArgs args) {
      Guild guild = await Database.getGuild(args.Guild.Id);

      Channel channel = guild.getChannel(args.Channel.Id);

      channel.pushDeletedMessage(args.Message);
    }

    public static async Task DankMemerResponse(DiscordMessage msg, DiscordGuild guild) {
      var messages = await msg.Channel.GetMessagesBeforeAsync(msg.Id, 5);

      User sniper = null;
      foreach (var message in messages) {
        if (message.Content.IndexOf("pls snipe") != -1) {
          sniper = await Database.getUser(message.Author.Id);
        }
      }

      if (sniper is null) {
        sniper = await Database.getUser(msg.Author.Id);
      }

      Snipe.SnipeMessage((await Database.getGuild(guild.Id)).getChannel(msg.ChannelId), msg, sniper);
    }
  }
}