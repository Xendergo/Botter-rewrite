using System.Collections.Generic;
using System.Threading.Tasks;
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
  public User(ulong id, Stats stats, int coins, int magic, int electricity, int health) : base(id, Database.userCache) {
    this.id = id;
    this.stats = stats;
    this.health = health;
    this.electricity = electricity;
    this.magic = magic;
    this.coins = coins;
  }

  public async Task updateData() {
    await Database.updateStats(id, stats);
  }

  protected override async void onKill() {
    await updateData();
  }
}