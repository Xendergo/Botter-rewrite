using System.Linq;
using System;
using System.Collections.Generic;
using DSharpPlus.EventArgs;
using DSharpPlus;
using DSharpPlus.Entities;
using System.Threading.Tasks;
using Commands;

class CommandManager {
  public static HashSet<ICommand> commandsSet = new HashSet<ICommand>();
  public static Dictionary<TypoableString, ICommand> commands = new Dictionary<TypoableString, ICommand>();
  public static async Task OnMessage(DiscordClient client, MessageCreateEventArgs messageArgs) {
    DiscordMessage msg = messageArgs.Message;
    if (msg?.Author?.IsBot == true) return;

    string[] text = msg.Content.ToLower().Split(" ");

    Guild guild = await Database.getGuild(messageArgs.Guild.Id);

    Channel channel = guild.getChannel(msg.ChannelId);

    if (!guild.checkPrefix(text[0])) return;

    if (text.Length < 2) {
      await msg.RespondAsync("You need to provide a command");
      return;
    }

    try {
      
      TypoableString corrected = TypoableString.FindClosestString(text[1], commands.Keys);

      if (corrected is null) {
        await msg.RespondAsync($"There is no command with name `{text[1]}`, say `botter help` is you don't know what to say");
        return;
      }

      string[] args = new string[text.Length - 2];
      Array.Copy(text, 2, args, 0, args.Length);

      await commands[corrected].Exec(client, args, msg, guild);
    } catch (Exception e) {
      channel.Error(e, msg);
    }
  }

  public static void AddCommandsToDictionary() {
    foreach (ICommand command in commandsSet) {
      foreach (TypoableString str in command.aliases) {
        commands.Add(str, command);
      }
    }
  }

  public static void AddCommands() {
    commandsSet.Add(new Help());

    commandsSet.Add(new Snipe());
    commandsSet.Add(new EditHistory());
    commandsSet.Add(new Shotgun());

    commandsSet.Add(new Src());
    commandsSet.Add(new Debug());

    AddCommandsToDictionary();
  }
}