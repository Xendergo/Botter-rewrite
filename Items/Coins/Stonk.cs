using Newtonsoft.Json.Linq;
using System.Threading.Tasks;

namespace Items {
  [HiddenItem("Stonk")]
  public class Stonk : IItem {
    override public string name {get;} = "Stonk";
    public float priceBought;
    public float coinAmt;
    public string ticker;
    
    public Stonk(string ticker) {
      this.ticker = ticker;
    }

    public Stonk() {}

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
      coinAmt = coins;
      user.items.Add(this);
    }

    public async Task<int> sellStock() {
      float currentPrice = await getPriceOfStock();
      int sellPrice = (int)(coinAmt * (currentPrice / priceBought));
      owner.TransferCoins(sellPrice);
      await removeSelf();
      return sellPrice;
    }

    private async Task<float> getPriceOfStock() {
      float v = await Util.fetchStockData(ticker);
      if (v == -1) throw new CommandException("There was an error getting the price of that stock (does that ticker exist?)");
      return v;
    }

    override public string Display() {
      return $"{ticker} - invested {coinAmt} coins while price was ${priceBought}";
    }
  }
}