using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using DSharpPlus;
using DSharpPlus.Entities;
using DSharpPlus.EventArgs;

namespace Commands {
  class EditHistory : ICommand {
    public string help {get;}
    public string helpShort {get;}
    public string[] signature {get;}
    public TypoableString[] aliases {get;}
    public string category {get;}
    public bool admin {get;} = false;
    public EditHistory() {
      help = "Get previous versions of a message that was edited, edits are saved for at least 10 minutes, to find the id of a message, go to your user settings, go to `appearance`, then turn on developer mode. Then right click on the message and copy the id.";
      helpShort = "See how a message was edited";
      signature = new string[] {"<messageId>"};
      aliases = new TypoableString[] {new TypoableString("edithistory", 3)};
      category = "Sniping";
    }

    public async Task Exec(DiscordClient client, Args args, DiscordMessage msg, Guild guild, User user) {
      ulong id;
      try {
        id = ulong.Parse(args.strings["messageId"]);
      } catch {
        await msg.RespondAsync("The id must be an integer");
        return;
      }

      Channel channel = guild.getChannel(msg.ChannelId);

      if (!channel.EditedMessages.ContainsKey(id)) {
        DiscordMessage msgGotten = await (await client.GetGuildAsync(guild.id)).GetChannel(msg.ChannelId).GetMessageAsync(id);

        if (msgGotten is null) {
          await msg.RespondAsync("There's no message with that id in this channel");
        } else if (msgGotten.IsEdited) {
          await msg.RespondAsync("That message was edited too long ago");
        } else {
          await msg.RespondAsync("That message was never edited");
        }

        return;
      }

      EditedMessage editedMessage = channel.EditedMessages[id];

      DiscordEmbedBuilder embed = new DiscordEmbedBuilder();

      embed.WithColor(new DiscordColor(0xFF0000));
      embed.WithDescription(string.Join(" =>\n", editedMessage.history));
      embed.WithFooter($"- {editedMessage.AuthorUsername}");

      await msg.RespondAsync(embed);
    }

    public static async Task OnEdit(DiscordClient client, MessageUpdateEventArgs args) {
      Guild guild = await Database.getGuild(args.Guild.Id);

      Channel channel = guild.getChannel(args.Channel.Id);

      channel.AddEdit(args.MessageBefore, args.Message);
    }
  }
}