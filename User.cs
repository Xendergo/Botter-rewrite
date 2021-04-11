using System.Collections.Generic;
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
  public ulong id;

  /// <summary>The user's statistics, mutations are automatically saved when the command is done executing</summary>
  public Stats stats;
  public User(ulong id, Stats stats) : base(id, Database.userCache) {
    this.id = id;
    this.stats = stats;
  }

  public async void updateStats() {
    await Database.updateStats(id, stats);
  }
}