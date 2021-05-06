using Newtonsoft.Json.Linq;
using System;

namespace Items {
  [Item(
    7,
    "bond",
    1,
    10,
    "Coins",
    "Lend out money, you get 1 coin in interest for every day you wait to sell this item",
    "Lend out money and get back more in interest"
  )]
  class Bond : ISellable {
    long buyTime;
    public override string name {get;} = "bond";
    public override int sellPrice {get {
      return (int)(10 + (DateTimeOffset.Now.ToUnixTimeMilliseconds() - buyTime) / 86400000);
    }}

    public Bond() {
      buyTime = DateTimeOffset.Now.ToUnixTimeMilliseconds();
    }

    public override void Deserialize(JObject str) {
      buyTime = (long)str["buyTime"];
    }
    public override JObject Serialize() {
      JObject ret = new JObject();
      ret["buyTime"] = buyTime;
      return ret;
    }

    public override string Display() {
      long time = (DateTimeOffset.Now.ToUnixTimeMilliseconds() - buyTime) / 86400000;
      return $"bought {time} days ago, can be cashed in for {10 + time} coins";
    }
  }
}