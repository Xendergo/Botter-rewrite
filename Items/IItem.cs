using Newtonsoft.Json.Linq;
using Microsoft.CodeAnalysis;

namespace Items {
  public abstract class IItem {
    public Optional<int> id;
    public string name {get;}
    public User owner {get;}
    protected void removeSelf() {
      owner.items.Remove(this);
    }
    public abstract JObject Serialize();
    public abstract void Deserialize(JObject str);
  }
}