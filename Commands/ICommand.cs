using System.Threading.Tasks;
using DSharpPlus;
using DSharpPlus.Entities;

interface ICommand {
  public string help {get;}
  public string helpShort {get;}
  // The arguments required for the command to execute
  // <string> @user enum:option1|option2 #number <?optional>
  // All optionals must come after all non-optionals
  public string[] signature {get;}
  // First alias is considered the main one
  public TypoableString[] aliases {get;}
  public string category {get;}
  public bool admin {get;}
  public Task Exec(DiscordClient client, Args args, DiscordMessage message, Guild guild, User user);
}