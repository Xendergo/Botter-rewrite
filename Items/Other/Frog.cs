using Newtonsoft.Json.Linq;
using DSharpPlus.Entities;
using System.Threading.Tasks;

namespace Items {
  [Item(
    10,
    "frog",
    1,
    3,
    "Other",
    "Ammo for frog launcher, poisonous in general",
    "Poisonous and ammo for frog launcher"
  )]
  class Frog : IItem {
    override public string name {get;} = "frog";
    override public string Display() {
      return "";
    }
  }
}