using System;
using System.Diagnostics;
using Npgsql;
using System.Threading.Tasks;
using System.Collections.Generic;
using Botter_rewrite;
using Items;
using Newtonsoft.Json.Linq;
using Microsoft.CodeAnalysis;

static class Database {
  private static NpgsqlConnection conn;
  public static Dictionary<ulong, Guild> guildCache = new Dictionary<ulong, Guild>();
  public static Dictionary<ulong, User> userCache = new Dictionary<ulong, User>();

  public static async Task Connect() {
    Process.Start(Program.dataPath + "/startDB.bat");
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

    tryAgain:
    try {
      ret = await cmd.ExecuteReaderAsync();
    } catch (PostgresException e) {
      throw new Exception(e.Message);
    } catch (NpgsqlOperationInProgressException e) {
      goto tryAgain;
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
      reader.Dispose();
      return null;
    }

    reader.Dispose();

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
      ret = new User(id, new Stats {
        GotSniped = 0,
        PeopleSniped = 0,
        PeopleKilled = 0,
        SelfSniped = 0,
        Died = 0,
        Searched = 0,
        Interactions = 0
      },0,
        0,
        0);
      await createUser(id);
    } else {
      ret = new User(ulong.Parse(rows[0]), new Stats {
        GotSniped = int.Parse(rows[1]),
        PeopleSniped = int.Parse(rows[2]),
        PeopleKilled = int.Parse(rows[3]),
        SelfSniped = int.Parse(rows[4]),
        Died = int.Parse(rows[5]),
        Searched = int.Parse(rows[6]),
        Interactions = int.Parse(rows[7])
      },int.Parse(rows[8]),
        int.Parse(rows[9]),
        int.Parse(rows[10]));
    }

    ret.items = await getItems(id, ret);

    userCache[id] = ret;
    return ret;
  }

  private static async Task<List<IItem>> getItems(ulong id, User user) {
    using (NpgsqlDataReader reader = await execCommand($"SELECT * FROM items WHERE id = '{id}'")) {
      List<IItem> ret = new List<IItem>();

      while (await reader.ReadAsync()) {
        long itemId = reader.GetInt64(0);
        string name = reader.GetString(2);
        string data = reader.GetString(3);

        ItemClassData itemEntry;
        
        if (ItemRegistry.items.ContainsKey(new TypoableString(name, 0))) {
          itemEntry = ItemRegistry.items[new TypoableString(name, 0)].classData;
        } else {
          itemEntry = ItemRegistry.notForSaleItems[name];
        }

        IItem item = (IItem) Activator.CreateInstance(itemEntry.clazz, itemEntry.constructorArgs);
        item.Deserialize(JObject.Parse(data));
        item.id = new Optional<long>(itemId);
        item.owner = user;

        ret.Add(item);
      }
      return ret;
    }
  }

  public static async Task setPrefix(ulong id, string prefix) {
    (await execCommand($"UPDATE guilds SET prefix = '{prefix}' WHERE id = '{id}'")).Dispose();
  }

  public static async Task updateUser(User user) {
    Stats stats = user.stats;
    (await execCommand($@"UPDATE users SET
gotsniped = '{stats.GotSniped}',
peoplesniped = '{stats.PeopleSniped}',
peoplekilled = '{stats.PeopleKilled}',
selfsniped = '{stats.SelfSniped}',
died = '{stats.Died}',
searched = '{stats.Searched}',
interactions = '{stats.Interactions}',
coins = {user.coins},
magic = {user.magic},
health = {user.health}
WHERE id = '{user.id}'")).Dispose();

    foreach (IItem item in user.items) {
      await saveItem(item, user.id);
    }
  }

  public static async Task saveItem(IItem item, ulong userId) {
    if (item.id.HasValue) {
      (await execCommand($@"UPDATE items SET
data = '{item.Serialize().ToString()}'
WHERE itemId = {item.id.Value}")).Dispose();
    } else {
      NpgsqlDataReader reader = await execCommand($@"INSERT INTO items(id, name, data) VALUES (
'{userId}',
'{item.name}',
'{item.Serialize().ToString()}')
RETURNING itemId");
      await reader.ReadAsync();
      item.id = reader.GetInt64(0);
      reader.Dispose();
    }
  }

  public static async Task deleteItem(long id) {
    (await execCommand($"DELETE FROM items WHERE itemId = {id}")).Close();
  }

  public static async Task createGuild(ulong id) {
    (await execCommand($"INSERT INTO guilds(id, prefix) VALUES ({id}, '')")).Close();
  }

  public static async Task createUser(ulong id) {
    (await execCommand($"INSERT INTO users(id) VALUES ({id})")).Close();
  }

  public static async Task<int> countUsers() {
    using (NpgsqlDataReader reader = await execCommand("SELECT COUNT(*) FROM users")) {
      await reader.ReadAsync();
      return reader.GetInt32(0);
    }
  }

  public static async Task<int> countGuilds() {
    using (NpgsqlDataReader reader = await execCommand("SELECT COUNT(*) FROM guilds")) {
      await reader.ReadAsync();
      return reader.GetInt32(0);
    }
  }
}