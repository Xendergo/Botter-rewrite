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
  public object[] constructorArgs;
  public Type clazz;
}

public static class ItemRegistry {
  public static Dictionary<TypoableString, ItemEntry> items = new Dictionary<TypoableString, ItemEntry>();
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

    IItem item = (IItem)Activator.CreateInstance(entry.clazz, entry.constructorArgs);
    item.id = new Microsoft.CodeAnalysis.Optional<long>();
    item.owner = user;
    user.TransferCoins(-entry.price);
    user.items.Add(item);

    await msg.RespondAsync($"You now have another **{itemName}** and **{user.coins} coins**");
  }

  public static void RegisterItems() {
    items.Add(new TypoableString("golden-sword", 2), new ItemEntry {
      name = "Golden-sword",
      price = 25,
      category = "Weapons",
      description = "Deal 10 damage, breaks after 15 uses",
      shortDescription = "Deals a lot of damage, breaks quickly",
      clazz = typeof(GoldenSword),
      constructorArgs = null
    });

    items.Add(new TypoableString("potato", 1), new ItemEntry {
      name = "potato",
      price = 2,
      category = "Food",
      description = "Heal 2 health when used, also used as ammo for potato cannon",
      shortDescription = "Heal 2 health when used, also used as ammo for potato cannon",
      clazz = typeof(Food),
      constructorArgs = new Object[] {"potato", 2}
    });
  }
}