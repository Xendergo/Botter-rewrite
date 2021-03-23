using System.Threading.Tasks;
using DSharpPlus;
using DSharpPlus.Entities;

interface ICommand {
  public string help {get;}
  public string helpShort {get;}
  // The arguments required for the command to execute
  // <string> [user] <?optional>
  public string[] signature {get;}
  // First alias is considered the main one
  public TypoableString[] aliases {get;}
  public string category {get;}
  public Task Exec(DiscordClient client, string[] args, DiscordMessage message, Guild guild);
}