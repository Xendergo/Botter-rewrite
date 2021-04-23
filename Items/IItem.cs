using Newtonsoft.Json.Linq;
using Microsoft.CodeAnalysis;
using System.Threading.Tasks;

namespace Items {
  public abstract class IItem {
    public Optional<long> id;
    public abstract string name {get;}
    public User owner;
    protected async Task removeSelf() {
      owner.items.Remove(this);

      if (id.HasValue) {
        await Database.deleteItem(id.Value);
      }
    }
    public abstract string Display();
    public abstract JObject Serialize();
    public abstract void Deserialize(JObject str);
  }
}