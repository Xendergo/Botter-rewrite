using System.Collections.Generic;
class Guild : Cacheable<ulong, Guild> {
  public string prefix;

  public Guild(ulong id, string prefix, Dictionary<ulong, Guild> cache) : base(id, cache) {
    this.prefix = prefix;
  }

  public bool checkPrefix(string str) {
    return str == prefix || str == "botter";
  }
}