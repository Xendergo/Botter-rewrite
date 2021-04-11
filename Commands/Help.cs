using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using DSharpPlus;
using DSharpPlus.Entities;

namespace Commands {
  class Help : ICommand {
    public string help {get;}
    public string helpShort {get;}
    public string[] signature {get;}
    public TypoableString[] aliases {get;}
    public string category {get;}
    public bool admin {get;} = false;
    public Help() {
      help = "Get a list of all commands, or help for a specific one";
      helpShort = "Get help for a command";
      signature = new string[] {"<?command>"};
      aliases = new TypoableString[] {new TypoableString("help", 1), new TypoableString("h", 0), new TypoableString("?", 0)};
      category = "Help";
    }

    public async Task Exec(DiscordClient client, string[] args, DiscordMessage msg, Guild guild, User user) {
      if (args.Length == 0) {
        string prefixText = guild.prefix != "" ? $"or `{guild.prefix}`" : "";
        await msg.RespondAsync($@"
For a prefix you can use `botter` {prefixText}

Here's a list of commands:
{GenerateCommandList()}

Bot written by Xendergo
        ");
      } else {
        TypoableString corrected = TypoableString.FindClosestString(args[0], CommandManager.commands.Keys);

        if (corrected is null) {
          await msg.RespondAsync($"There is no command with name `{args[0]}`");
          return;
        }

        ICommand cmd = CommandManager.commands[corrected];
        string aliases = cmd.aliases.Length > 1 ? $", aliases: `{string.Join("`, `", cmd.aliases.Skip(1))}`" : "";
        await msg.RespondAsync($@"
`{cmd.aliases[0]}` {aliases}
`botter {cmd.aliases[0]} {string.Join(' ', cmd.signature)}`
{cmd.help}
        ");
      }
    }

    private string GenerateCommandList() {
      List<string> commands = new List<string>();

      string pCategory = "";
      foreach (ICommand command in CommandManager.commandsSet) {
        if (pCategory != command.category) {
          commands.Add("");
          commands.Add($"**{command.category}:**");
        }
        commands.Add($"`{command.aliases[0].value}`: {command.helpShort}");
        pCategory = command.category;
      }

      return string.Join('\n', commands);
    }
  }
}