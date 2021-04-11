using System;
using System.Diagnostics;
using Npgsql;
using System.Threading.Tasks;
using System.Collections.Generic;

class Database {
  private static NpgsqlConnection conn;
  private static Dictionary<ulong, Guild> guildCache = new Dictionary<ulong, Guild>();
  public static Dictionary<ulong, User> userCache = new Dictionary<ulong, User>();

  public static async Task Connect() {
    Process.Start("C:/All items/projects/Botter rewrite/startDB.bat");
    string connstring = "Server=localhost;Port=5433;Username=postgres";

    tryConnect:
    try {
      conn = new NpgsqlConnection(connstring);
      await conn.OpenAsync();
    } catch {
      goto tryConnect;
    }
  }

  private static async Task<NpgsqlDataReader> execCommand(string req) {
    NpgsqlCommand cmd = new NpgsqlCommand(req, conn);

    NpgsqlDataReader ret;
    doCommand:
    try {
      ret = await cmd.ExecuteReaderAsync();
    } catch (NpgsqlOperationInProgressException e) {
      await Task.Delay(100);
      goto doCommand;
    }

    cmd.Dispose();
    return ret;
  }

  private static async Task<List<string>> readRow(string req) {
    NpgsqlDataReader reader = await execCommand(req);
    List<string> values = new List<string>();

    try {
      await reader.ReadAsync();
      for (int i = 0; i < reader.FieldCount; i++) {
        values.Add(reader.GetValue(i).ToString());
      }
    } catch {
      reader.Close();
      return null;
    }

    reader.Close();

    return values;
  }

  public static async Task<Guild> getGuild(ulong id) {
    if (guildCache.ContainsKey(id)) {
      guildCache[id].resetKill();
      return guildCache[id];
    }

    List<string> rows = await readRow($"SELECT * FROM guilds WHERE id = '{id}'");

    Guild ret;
    if (rows is null) {
      ret = new Guild(id, "", guildCache);
      await createGuild(id);
    } else {
      ret = new Guild(ulong.Parse(rows[0]), rows[1], guildCache);
    }

    guildCache[id] = ret;
    return ret;
  }

  public static async Task<User> getUser(ulong id) {
    if (userCache.ContainsKey(id)) {
      userCache[id].resetKill();
      return userCache[id];
    }

    List<string> rows = await readRow($"SELECT * FROM users WHERE id = '{id}'");

    User ret;
    if (rows is null) {
      ret = new User(id, new Stats() {
        GotSniped = 0,
        PeopleSniped = 0,
        PeopleKilled = 0,
        SelfSniped = 0,
        Died = 0,
        Searched = 0,
        Interactions = 0
      });
      await createUser(id);
    } else {
      ret = new User(ulong.Parse(rows[0]), new Stats() {
        GotSniped = int.Parse(rows[1]),
        PeopleSniped = int.Parse(rows[2]),
        PeopleKilled = int.Parse(rows[3]),
        SelfSniped = int.Parse(rows[4]),
        Died = int.Parse(rows[5]),
        Searched = int.Parse(rows[6]),
        Interactions = int.Parse(rows[7])
      });
    }

    userCache[id] = ret;
    return ret;
  }

  public static async Task setPrefix(ulong id, string prefix) {
    (await execCommand($"UPDATE guilds SET prefix = '{prefix}' WHERE id = '{id}'")).Dispose();
  }

  public static async Task updateStats(ulong id, Stats stats) {
    (await execCommand($@"UPDATE users SET
gotsniped = '{stats.GotSniped}',
peoplesniped = '{stats.PeopleSniped}',
peoplekilled = '{stats.PeopleKilled}',
selfsniped = '{stats.SelfSniped}',
died = '{stats.Died}',
searched = '{stats.Searched}',
interactions = '{stats.Interactions}'
WHERE id = '{id}'")).Dispose();
  }

  public static async Task createGuild(ulong id) {
    (await execCommand($"INSERT INTO guilds(id, prefix) VALUES ({id}, '')")).Dispose();
  }

  public static async Task createUser(ulong id) {
    (await execCommand($"INSERT INTO users(id) VALUES ({id})")).Dispose();
  }
}