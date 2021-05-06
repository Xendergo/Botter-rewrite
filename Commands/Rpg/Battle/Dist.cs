using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using DSharpPlus;
using DSharpPlus.Entities;

namespace Commands {
  [Command(23)]
  class Dist : IBattleCommand {
    public override string help {get;} = "Set your distance to the other player (distance to other players in unchanged (physics is ignored for simplicity))";
    public override string helpShort {get;} = "Set your distance to another player";
    public override string[] signature {get;} = new string[] {"@target", "#dist"};
    public override TypoableString[] aliases {get;} = new TypoableString[] {new TypoableString("dist", 1), new TypoableString("distance", 3), new TypoableString("d", 0)};
    protected override async Task Exec(DiscordClient client, Args args, DiscordMessage msg, Guild guild, BattleEntity user, Battle battle) {
      BattleEntity target = await Database.getUser(args.users["target"]);
      int dist = (int)args.nums["dist"];
      battle.setDistance(user, target, dist);
      await msg.RespondAsync($"Your distance to {await target.username} is now **{dist} units**");
    }
  }
}