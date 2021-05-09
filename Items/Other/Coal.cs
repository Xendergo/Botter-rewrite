using Newtonsoft.Json.Linq;
using DSharpPlus.Entities;
using System.Threading.Tasks;

namespace Items {
  [Item(
    11,
    "coal",
    1,
    5,
    "Other",
    "Fuel for the coal power plant",
    "Fuel for the coal power plant"
  )]
  class Coal : IItem {
    override public string name {get;} = "coal";
    override public string Display() {
      return "";
    }
  }
}