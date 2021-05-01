using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using DSharpPlus;
using DSharpPlus.Entities;
using StatusEffects;

abstract class IBattleCommand : ICommand {
  public abstract string help {get;}
  public abstract string helpShort {get;}
  // If the command involves doing something to another player, the other player must be called `target` in the signature
  public abstract string[] signature {get;}
  public abstract TypoableString[] aliases {get;}
  public string category {get;} = "Battle";
  public bool admin {get;} = false;
  public async Task Exec(DiscordClient client, Args args, DiscordMessage msg, Guild guild, User user) {
    if (user.battle is null) {
      await msg.RespondAsync("You must be in a battle to run this command, try using the `battlerequest` command to start a battle");
      return;
    }

    if (args.users.ContainsKey("target")) {
      User target = await Database.getUser(args.users["target"]);

      if (target.battle != user.battle) {
        await msg.RespondAsync($"You must be in the same battle to use this command, to invite them to your battle, run `botter battlerequest @{user.username}`");
        return;
      }
    }

    if (user.GetEffect<Stun>().HasValue) {
      throw new CommandException("You're stunned, you can't use a battle command");
    }

    await Exec(client, args, msg, guild, user, user.battle);
  }

  protected abstract Task Exec(DiscordClient client, Args args, DiscordMessage msg, Guild guild, BattleEntity user, Battle battle);
}