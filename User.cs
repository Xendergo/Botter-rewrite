using System;
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
  /// <summary>The user's statistics, mutations are automatically saved when the player is removed form cache
  /// (with the exception of removing items from the player, unless you use the item's `removeSelf` method)</summary>
  public Stats stats;
  private int _health;
  public int health {
    get {
      return _health;
    }

    set {
      _health = Math.Clamp(value, 0, 100);
    }
  }
  public int electricity;
  public int magic;
  public int coins {get; protected set;}
  public Battle battle = null;
  public Task<string> username;
  public List<IItem> items = new List<IItem>();
  public User(ulong id, Stats stats, int coins, int magic, int electricity, int health) : base(id, Database.userCache) {
    this.id = id;
    this.stats = stats;
    this.health = health;
    this.electricity = electricity;
    this.magic = magic;
    this.coins = coins;
    username = getUsername();
  }

  public void TransferCoins(int amt) {
    coins += amt;
    coins -= calcTax(amt);
  }

  public int calcTax(int amt) {
    return calcTax(amt, coins);
  }
  public static int calcTax(int amt, int wealth) {
    // Sales tax dependent on wealth
    // Equasion where c is wealth & p is amt
    // (1.3 ^ (c / 1000) - 1) * sqrt(|p|)

    return (int)MathF.Truncate(MathF.Min(wealth, MathF.Abs(amt) * (MathF.Pow(1.3F, wealth * 0.0001F) - 1)));
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