using System.Diagnostics;
using Npgsql;
using System.Threading.Tasks;
using System.Collections.Generic;

class Database {
  private static NpgsqlConnection conn;
  private static Dictionary<ulong, Guild> guildCache = new Dictionary<ulong, Guild>();
  public static async Task Connect() {
    Process.Start("startDB.bat");
    string connstring = "Server=localhost;Port=5433;Username=postgres";
    conn = new NpgsqlConnection(connstring);
    await conn.OpenAsync();
  }

  private static async Task<NpgsqlDataReader> execCommand(string req) {
    NpgsqlCommand cmd = new NpgsqlCommand(req, conn);
    NpgsqlDataReader ret = await cmd.ExecuteReaderAsync();
    cmd.Dispose();
    return ret;
  }

  private static async Task<List<string>> readRow(string req) {
    NpgsqlDataReader reader = await execCommand(req);
    List<string> values = new List<string>();

    try {
      await reader.ReadAsync();
      for (int i = 0; i < reader.FieldCount; i++) {
        values.Add(reader.GetString(i));
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
      return guildCache[id];
    }

    List<string> rows = await readRow($"SELECT * FROM guilds WHERE id = '{id}'");

    Guild ret;
    if (rows is null) {
      ret = new Guild(id, "", guildCache);
    } else {
      ret = new Guild(ulong.Parse(rows[0]), rows[1], guildCache);
    }

    guildCache[id] = ret;
    return ret;
  }
}