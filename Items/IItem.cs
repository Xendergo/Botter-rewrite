using System.Xml;
using Newtonsoft.Json.Linq;
using Microsoft.CodeAnalysis;
using System.Threading.Tasks;

namespace Items {
  public abstract class IItem {
    public Optional<long> id;
    public abstract string name {get;}
    public User owner;
    public async Task removeSelf() {
      owner.items.Remove(this);

      if (id.HasValue) {
        await Database.deleteItem(id.Value);
      }
    }
    public abstract string Display();
    public virtual JObject Serialize() { return new JObject(); }
    public virtual void Deserialize(JObject str) {}
    /// <summary>
    /// Called when a battle ticks a player, before the player does anything else with that
    /// </summary>
    public virtual void BattleTick() {}
    /// <summary>
    /// Called when a battle ticks a player, after the player does everything with that
    /// </summary>
    public virtual void AfterBattleTick() {}
  }
}