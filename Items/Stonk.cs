using Newtonsoft.Json.Linq;
using Microsoft.CodeAnalysis;

namespace Items {
  // public abstract class 
  public class Stonk : IItem {
    override public string name {get;} = "Stonk";
    public float priceBought;
    public float coinAmt;
    
    override public JObject Serialize() {
      JObject ret = new JObject();
      ret["priceBought"] = priceBought;
      ret["coinAmt"] = coinAmt;
      return ret;
    }

    override public void Deserialize(JObject obj) {
      priceBought = (float)obj["priceBought"];
      coinAmt = (float)obj["coinAmt"];
    }

    public void buyStock(User user, int coins) {
      float stockPrice = getPriceOfStock();
      user.TransferCoins(coins);
    }

    public void sellStock(User user, int coins) {
      float stockPrice = getPriceOfStock();
      user.TransferCoins(coins);
    }

    private float getPriceOfStock() {
      return (float) 1.0;
    }
  }
}