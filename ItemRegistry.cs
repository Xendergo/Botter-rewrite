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
    List<(float, TypoableString, ItemEntry)> itemsList = new List<(float, TypoableString, ItemEntry)>();

    foreach (Type type in Assembly.GetExecutingAssembly().GetTypes()) {
      if (!typeof(IItem).IsAssignableFrom(type)) continue;

      foreach (ItemAttribute listing in type.GetCustomAttributes<ItemAttribute>()) {
        int index = itemsList.FindIndex((v) => v.Item1 > listing.ordinal);
        if (index == -1) index = itemsList.Count;

        ItemEntry entry = new ItemEntry {
          name = listing.name.value,
          price = listing.price,
          category = listing.category,
          description = listing.description,
          shortDescription = listing.shortDescription,
          classData = new ItemClassData {
            clazz = type,
            constructorArgs = listing.args
          }
        };

        itemsList.Insert(index, (listing.ordinal, listing.name, entry));
      }
    }

    foreach ((float, TypoableString, ItemEntry) command in itemsList) {
      items.Add(command.Item2, command.Item3);
    }

    foreach (Type type in Assembly.GetExecutingAssembly().GetTypes()) {
      if (!typeof(IItem).IsAssignableFrom(type)) continue;

      foreach (HiddenItemAttribute listing in type.GetCustomAttributes<HiddenItemAttribute>()) {
        ItemClassData entry = new ItemClassData {
          clazz = type,
          constructorArgs = listing.args
        };

        notForSaleItems.Add(listing.name, entry);
      }
    }
  }
}