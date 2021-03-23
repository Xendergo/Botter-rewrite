using System.Collections.Generic;
class Guild : Cacheable<ulong, Guild> {
  public string prefix;
  private Dictionary<ulong, Channel> channels;

  public Guild(ulong id, string prefix, Dictionary<ulong, Guild> cache) : base(id, cache) {
    this.prefix = prefix;
    channels = new Dictionary<ulong, Channel>();
  }

  public bool checkPrefix(string str) {
    base.resetKill();
    return str == prefix || str == "botter";
  }

  // Channels don't contain any data that needs to be persisted, so I think this is reasonable
  public Channel getChannel(ulong id) {
    base.resetKill();

    if (!channels.ContainsKey(id)) {
      channels[id] = new Channel();
    }

    return channels[id]; 
  }
}