using System.Threading.Tasks;
using DSharpPlus.Entities;

namespace Items {
  [Item(
    0,
    "golden-sword",
    2,
    25,
    "Weapons",
    "Deal 10 damage, breaks after 15 uses",
    "Deal 10 damage, breaks after 15 uses"
  )]
  public class GoldenSword : IMelee {
    override public string name {get;} = "golden-sword";
    public override int maxDamage {get;} = 15;
    public override int damageToDeal {get;} = 15;
    override protected async Task<string> DoAttack(BattleEntity target, BattleEntity stabber, DiscordMessage msg) {
      return "";
    }
  }
}