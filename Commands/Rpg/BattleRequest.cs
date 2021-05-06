using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using DSharpPlus;
using DSharpPlus.Entities;
using DSharpPlus.EventArgs;

namespace Commands {
  [Command(18)]
  class BattleRequest : ICommand {
    public string help {get;} = "Request to have a fight with someone";
    public string helpShort {get;} = "Start a fight with someone";
    public string[] signature {get;} = new string[] {"@player"};
    public TypoableString[] aliases {get;} = new TypoableString[] {new TypoableString("battlerequest", 3), new TypoableString("fightrequest", 3), new TypoableString("requestfight", 3), new TypoableString("requestbattle", 3), new TypoableString("b", 0)};
    public string category {get;} = "Rpg";
    public bool admin {get;} = false;
    public async Task Exec(DiscordClient client, Args args, DiscordMessage msg, Guild guild, User user) {
      ulong otherId = args.users["player"];
      DiscordMessage request = await msg.RespondAsync($"<@!{otherId}>, <@!{msg.Author.Id}> has requested to fight you, React to accept");
      
      ReactionCollector collector = new ReactionCollector(client, request, 60000);

      collector.onReaction += (MessageReactionAddEventArgs reactionArgs) => {
        if (reactionArgs.User.Id != otherId) return;

        msg.RespondAsync($"<@!{msg.Author.Id}>, <@!{otherId}> has accepted your battle request!");

        connectBattles(user, otherId, msg.Channel);
        collector.Dispose();
      };

      collector.onDispose += () => {
        msg.RespondAsync($"<@!{msg.Author.Id}>, <@!{otherId}> has not accepted your battle request, it is no longer valid");
      };

      await request.CreateReactionAsync(DiscordEmoji.FromName(client, ":white_check_mark:"));
    }

    private static async void connectBattles(User user1, ulong user2id, DiscordChannel channel) {
      User user2 = await Database.getUser(user2id);

      if (user1.battle is null) user1.battle = new Battle(channel, user1);
      if (user2.battle is null) user2.battle = new Battle(channel, user2);

      user1.battle.mergeBattle(user2.battle);
    }
  }
}