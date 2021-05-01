using System.Threading.Tasks;
using DSharpPlus.Entities;
using Botter_rewrite;

// public class temp : BattleEntityImpl<ulong, temp> {
//   public override Task<string> username { get => throw new System.NotImplementedException(); protected set => throw new System.NotImplementedException(); }
//   public temp() : base(0, 0, 0, 0, null) {

//   }
// }

public struct Stats {
  public int GotSniped;
  public int PeopleSniped;
  public int PeopleKilled;
  public int SelfSniped;
  public int Died;
  public int Searched;
  public int Interactions;
}

public sealed class User : BattleEntityImpl<ulong, User> {
  /// <summary>The user's statistics, mutations are automatically saved when the player is removed form cache
  /// (with the exception of removing items from the player, unless you use the item's `removeSelf` method)</summary>
  public Stats stats;
  public override Task<string> username {get; protected set;}
  public User(ulong id, Stats stats, int coins, int magic, int health) : base(coins, magic, health, id, Database.userCache) {
    this.id = id;
    this.stats = stats;
    username = getUsername();
  }

  public async Task updateData() {
    await Database.updateUser(this);
  }

  protected override async void onKill() {
    if (battle is not null) {
      battle.players.Remove(this);
      await battle.MostRecentChannel.SendMessageAsync($"{await username} left the battle due to inactivity");
    }

    await updateData();
  }
  public async Task<DiscordUser> getUser() {
    return await Program.client.GetUserAsync(id);
  }

  private async Task<string> getUsername() {
    return (await getUser()).Username;
  }
}