using System.Collections.Generic;
using System.Threading.Tasks;
using DSharpPlus.Entities;
using DSharpPlus;
using Botter_rewrite;
using Items;
public struct Stats {
  public int GotSniped;
  public int PeopleSniped;
  public int PeopleKilled;
  public int SelfSniped;
  public int Died;
  public int Searched;
  public int Interactions;
}

public class User : Cacheable<ulong, User> {
  /// <summary>The user's statistics, mutations are automatically saved when the command is done executing</summary>
  public Stats stats;
  public int health;
  public int electricity;
  public int magic;
  public int coins;
  public Battle battle = null;
  public Task<string> username;
  public HashSet<IItem> items = new HashSet<IItem>();
  public User(ulong id, Stats stats, int coins, int magic, int electricity, int health) : base(id, Database.userCache) {
    this.id = id;
    this.stats = stats;
    this.health = health;
    this.electricity = electricity;
    this.magic = magic;
    this.coins = coins;
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