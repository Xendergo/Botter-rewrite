using System.Reflection;
using System.Collections.Generic;
using System;
using System.Threading.Tasks;
using DSharpPlus.Entities;
using Items;

public struct ItemEntry {
  public string name;
  public int price;
  public string category;
  public string description;
  public string shortDescription;
  public ItemClassData classData;
}

public struct ItemClassData {
  public object[] constructorArgs;
  public Type clazz;
}

public static class ItemRegistry {
  public static Dictionary<TypoableString, ItemEntry> items = new Dictionary<TypoableString, ItemEntry>();
  public static Dictionary<string, ItemClassData> notForSaleItems = new Dictionary<string, ItemClassData>();
  public static async Task BuyItem(string name, DiscordMessage msg, User user) {
    TypoableString itemName = TypoableString.FindClosestString(name, items.Keys);

    if (itemName is null) {
      await msg.RespondAsync($"There is no item named *{name}*");
      return;
    }

    ItemEntry entry = items[itemName];

    if (entry.price > user.coins) {
      await msg.RespondAsync($"You would need **{entry.price - user.coins}** more coins to afford this item");
      return;
    }

    IItem item = (IItem)Activator.CreateInstance(entry.classData.clazz, entry.classData.constructorArgs);
    item.id = new Microsoft.CodeAnalysis.Optional<long>();
    item.owner = user;
    user.TransferCoins(-entry.price);
    user.items.Add(item);

    await msg.RespondAsync($"You now have another **{itemName}** and **{user.coins} coins**");
  }

  public static void RegisterItems() {
    var itemsList = TypesWithAttribute.GetOrderedTypesWithAttribute<ItemAttribute, IItem>();

    foreach(var listing in itemsList) {
      ItemAttribute attribute = listing.Item1;

      int price = attribute.price;

      #if DEBUG
        price = 0;
      #endif

      ItemEntry entry = new ItemEntry {
        name = attribute.name.value,
        price = price,
        category = attribute.category,
        description = attribute.description,
        shortDescription = attribute.shortDescription,
        classData = new ItemClassData {
          clazz = listing.Item2,
          constructorArgs = attribute.args
        }
      };

      items.Add(attribute.name, entry);
    }

    var hiddenItems = TypesWithAttribute.GetUnorderedTypesWithAttribute<HiddenItemAttribute, IItem>();

    foreach (var listing in hiddenItems) {
      ItemClassData entry = new ItemClassData {
        clazz = listing.Item2,
        constructorArgs = listing.Item1.args
      };

      notForSaleItems.Add(listing.Item1.name, entry);
    }
  }
}