using Newtonsoft.Json.Linq;

namespace Items {
  public abstract class IDegradable : IItem {
    public int damage {get; protected set;}
    public abstract int maxDamage {get;}
    override public void Deserialize(JObject obj) {
      damage = (int)obj["damage"];
    }

    override public JObject Serialize() {
      JObject ret = new JObject();
      ret["damage"] = damage;
      return ret;
    }
    
    override public string Display() {
      return $"{damage} damage out of {maxDamage}";
    }
  }
}