using System.Threading.Tasks;

namespace Items {
  abstract class ISellable : IItem {
    public abstract int sellPrice {get;}
    public async Task<int> sell() {
      int price = sellPrice;
      owner.TransferCoins(price);
      await removeSelf();
      return price;
    }
  }
}