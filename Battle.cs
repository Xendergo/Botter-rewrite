using System.Collections.Generic;
using DSharpPlus.Entities;
using System;

public class Battle {
  public HashSet<User> players = new HashSet<User>();
  private Dictionary<ulong, Dictionary<ulong, int>> distances = new Dictionary<ulong, Dictionary<ulong, int>>();
  public DiscordChannel MostRecentChannel;

  public Battle(DiscordChannel channel, User firstPlayer) {
    MostRecentChannel = channel;
    players.Add(firstPlayer);
  }

  public int getDistance(User player1, User player2) {
    if (!players.Contains(player1)) throw new CommandException("Can't get distance between these players, one of them isn't in the battle");
    if (!players.Contains(player2)) throw new CommandException("Can't get distance between these players, one of them isn't in the battle");

    ulong[] ids;
    if (player1.id > player2.id) {
      ids = new ulong[] {player1.id, player2.id};
    } else {
      ids = new ulong[] {player2.id, player1.id};
    }

    if (!distances.ContainsKey(ids[0])) {
      distances[ids[0]] = new Dictionary<ulong, int>();
    }

    if (!distances[ids[0]].ContainsKey(ids[1])) {
      distances[ids[0]][ids[1]] = 5;
    }

    return distances[ids[0]][ids[1]];
  }

  public void setDistance(User player1, User player2, int dist) {
    ulong[] ids;
    if (player1.id > player2.id) {
      ids = new ulong[] {player1.id, player2.id};
    } else {
      ids = new ulong[] {player2.id, player1.id};
    }

    if (!distances.ContainsKey(ids[0])) {
      distances[ids[0]] = new Dictionary<ulong, int>();
    }

    distances[ids[0]][ids[1]] = dist;
  }

  public void mergeBattle(Battle otherBattle) {
    foreach (User player in otherBattle.players) {
      players.Add(player);
      player.battle = this;
    }

    foreach (ulong key in otherBattle.distances.Keys) {
      distances[key] = otherBattle.distances[key];
    }
  }
}