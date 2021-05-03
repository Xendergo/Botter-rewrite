using Newtonsoft.Json.Linq;
using DSharpPlus.Entities;
using System.Threading.Tasks;

namespace Items {
  class Frog : IItem {
    override public string name {get;} = "frog";
    override public string Display() {
      return "";
    }
  }
}