using Newtonsoft.Json.Linq;
using Microsoft.CodeAnalysis;

namespace Items {
  public abstract class IItem {
    public Optional<long> id;
    public abstract string name {get;}
    public User owner;
    protected void removeSelf() {
      owner.items.Remove(this);

      if (id.HasValue) {
        Database.deleteItem(id.Value);
      }
    }
    public abstract JObject Serialize();
    public abstract void Deserialize(JObject str);
  }
}