using Newtonsoft.Json.Linq;
using System.Threading.Tasks;

namespace Items {
  // public abstract class 
  public class Stonk : IItem {
    override public string name {get;} = "Stonk";
    public float priceBought;
    public float coinAmt;
    public string ticker;
    
    public Stonk(string ticker) {
      this.ticker = ticker;
    }
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
      priceBought = await getPriceOfStock();
      user.TransferCoins(-coins);
    }

    public async Task sellStock(User user, int coins) {
      priceBought = await getPriceOfStock();
      user.TransferCoins(coins);
    }

    private async Task<float> getPriceOfStock() {
      float v = await Util.fetchStockData(ticker);
      if (v == -1) throw new CommandException("That ticker doesn't exist");
      return v;
    }
    override public string Display() {
      return $"**{ticker}** - invested **{coinAmt} coins** while price was **${priceBought}**";
    }
  }
}