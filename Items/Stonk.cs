using Newtonsoft.Json.Linq;
using Microsoft.CodeAnalysis;
using System.Threading.Tasks;

namespace Items {
  // public abstract class 
  public class Stonk : IItem {
    override public string name {get;} = "Stonk";
    public float priceBought;
    public float coinAmt;
    public string ticker;
    
    override public JObject Serialize() {
      JObject ret = new JObject();
      ret["priceBought"] = priceBought;
      ret["coinAmt"] = coinAmt;
      ret["ticker"] = ticker;
      return ret;
    }

    override public void Deserialize(JObject obj) {
      priceBought = (float)obj["priceBought"];
      coinAmt = (float)obj["coinAmt"];
      ticker = (string)obj["ticker"];
    }

    public async Task buyStock(User user, int coins) {
      float stockPrice = await getPriceOfStock();
      user.TransferCoins(-coins);
    }

    public async Task sellStock(User user, int coins) {
      float stockPrice = await getPriceOfStock();
      user.TransferCoins(coins);
    }

    private async Task<float> getPriceOfStock() {
      return await Util.fetchStockData(ticker);
    }
    override public string Display() {
      return $"**{ticker}** - invested **{coinAmt} coins** while price was **${priceBought}**";
    }
  }
}