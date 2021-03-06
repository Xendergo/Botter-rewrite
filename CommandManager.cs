using System.Text.RegularExpressions;
using System.Linq;
using System;
using System.Collections.Generic;
using DSharpPlus.EventArgs;
using DSharpPlus;
using DSharpPlus.Entities;
using System.Threading.Tasks;
using StatusEffects;
using Commands;

struct Args {
  public Dictionary<string, string> strings;
  public Dictionary<string, ulong> users;
  public Dictionary<string, string> enums;
  public Dictionary<string, long> nums;
}

static class CommandManager {
  private static Regex isStringType = new Regex("^<.+>$");
  private static Regex isUserType = new Regex("^@");
  private static Regex isPing = new Regex("^<@!.+>$");
  public static HashSet<ICommand> commandsSet = new HashSet<ICommand>();
  public static Dictionary<TypoableString, ICommand> commands = new Dictionary<TypoableString, ICommand>();
  public static async Task OnMessage(DiscordClient client, MessageCreateEventArgs messageArgs) {
    DiscordMessage msg = messageArgs.Message;
    if (msg?.Author?.IsBot == true) {
      if (msg.Content == "There's nothing to snipe!" && msg.Author.Id == 270904126974590976 /*dank memer*/) {
        await Snipe.DankMemerResponse(msg, messageArgs.Guild);
      }

      return;
    }

    string[] text = msg.Content.ToLower().Split(" ");

    Guild guild = await Database.getGuild(messageArgs.Guild.Id);

    Channel channel = guild.getChannel(msg.ChannelId);

    if (!guild.checkPrefix(text[0])) return;

    if (text.Length < 2) {
      await msg.RespondAsync("You need to provide a command");
      return;
    }

    User user = await Database.getUser(msg.Author.Id);

    user.stats.Interactions++;

    try {
      TypoableString corrected = TypoableString.FindClosestString(text[1], commands.Keys);

      if (corrected is null) {
        await msg.RespondAsync($"There is no command with name `{text[1]}`, say `botter help` is you don't know what to say");
        return;
      }

      string[] args = new string[text.Length - 2];
      Array.Copy(text, 2, args, 0, args.Length);

      ICommand command = commands[corrected];

      if (command.admin && !(await messageArgs?.Guild?.GetMemberAsync(msg.Author.Id)).PermissionsIn(msg.Channel).HasPermission(Permissions.Administrator)) {
        await msg.RespondAsync("You need admin permissions to run this command");
        return;
      }

      Args argsStruct;
      try {
        argsStruct = parseArgs(args, command.signature);
      } catch (Exception e) {
        await msg.RespondAsync(e.Message + $", type `botter help {corrected}` for extra help");
        return;
      }

      await Task.Delay((int)(user.GetEffectStrength<Annoyance>() * 1000));

      try {
        await command.Exec(client, argsStruct, msg, guild, user);
      } catch (CommandException e) {
        await msg.RespondAsync(e.Message);
      }
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
    var commandsList = TypesWithAttribute.GetOrderedTypesWithAttribute<CommandAttribute, ICommand>();

    foreach (var command in commandsList) {
      commandsSet.Add((ICommand)Activator.CreateInstance(command.Item2, command.Item1.args));
    }

    AddCommandsToDictionary();
  }

  public static Args parseArgs(string[] args, string[] sig) {
    Args ret = new Args {
      strings = new Dictionary<string, string>(),
      users = new Dictionary<string, ulong>(),
      enums = new Dictionary<string, string>(),
      nums = new Dictionary<string, long>()
    };

    for (int i = 0; i < sig.Length; i++) {
      if (i >= args.Length) {
        if (sig[i].IndexOf("?") == -1) {
          throw new Exception($"Required argument `{sig[i]}` is missing");
        } else {
          continue;
        }
      }

      bool argIsPing = isPing.IsMatch(args[i]);
      bool sigIsEnum = sig[i].IndexOf(':') != -1;
      bool sigIsNum = sig[i].StartsWith("#");

      if (sig[i].StartsWith("@") && !argIsPing) {
        throw new Exception($"The argument `{sig[i]}` is a user but a string was provided (did you ping them?)");
      }

      if ((sig[i].StartsWith("<") || sigIsEnum) && argIsPing) {
        throw new Exception($"The argument `{sig[i]}` must be a string, not a user");
      }
      
      if (sigIsNum && argIsPing) {
        throw new Exception($"The argument `{sig[i]}` must be a number, not a user");
      }

      string argName;
      if (argIsPing || sigIsNum) {
        argName = sig[i].Substring(1);
      } else if (sigIsEnum) {
        argName = new string(sig[i].TakeWhile(v => v != ':').ToArray());
      } else {
        argName = sig[i].Substring(1, sig[i].Length - 2);
      }

      if (argName.StartsWith("?")) {
        argName = argName.Substring(1);
      }

      if (argIsPing) {
        ret.users[argName] = ulong.Parse(args[i].Substring(3, args[i].Length - 4));
      } else if (sigIsEnum) {
        string[] options = sig[i].Substring(sig[i].IndexOf(":") + 1).Split('|');

        if (!options.Contains(args[i])) {
          throw new Exception($"Argument {args[i]} must be one of {string.Join(", ", options)}");
        }

        ret.enums[argName] = args[i];
      } else if (sigIsNum) {
        try {
          ret.nums[argName] = long.Parse(args[i]);
        } catch {
          throw new Exception($"The argument {args[i]} must be an integer (is it too big to parse?)");
        }
      } else {
        ret.strings[argName] = args[i];
      }
    }

    return ret;
  }
}