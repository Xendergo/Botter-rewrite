using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using DSharpPlus;
using DSharpPlus.Entities;

namespace Commands {
  class LeaveBattle : IBattleCommand {
    public override string help {get;} = "Leave a battle";
    public override string helpShort {get;} = "Leave a battle";
    public override string[] signature {get;} = new string[] {};
    public override TypoableString[] aliases {get;} = new TypoableString[] {new TypoableString("leave", 1), new TypoableString("leavebattle", 3)};
    protected override async Task Exec(DiscordClient client, Args args, DiscordMessage msg, Guild guild, User user, Battle battle) {
      battle.players.Remove(user);
      user.battle = null;
      await msg.RespondAsync("You successfully left the battle");
      await battle.MostRecentChannel.SendMessageAsync($"<@!{user.id}> left the battle");
    }
  }
}