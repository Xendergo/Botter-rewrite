using Newtonsoft.Json.Linq;
using Microsoft.CodeAnalysis;

namespace Items {
  // public abstract class 
  public class Stonk : IItem {
    override public string name {get;} = "Stonk";
    public float priceBought;
    public float amt;
    override public JObject Serialize() {
      JObject ret = new JObject();
      ret["priceBought"] = priceBought;
      ret["amt"] = amt;
      return ret;
    }
    override public void Deserialize(JObject obj) {
      priceBought = (float)obj["priceBought"];
      amt = (float)obj["amt"];
    }
  }
}