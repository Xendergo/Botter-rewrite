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
    items.Add(new TypoableString("golden-sword", 2), new ItemEntry {
      name = "Golden-sword",
      price = 25,
      category = "Weapons",
      description = "Deal 10 damage, breaks after 15 uses",
      shortDescription = "Deals a lot of damage, breaks quickly",
      classData = new ItemClassData {
        clazz = typeof(GoldenSword),
        constructorArgs = null
      }
    });

    items.Add(new TypoableString("petrified-poop-sword", 4), new ItemEntry {
      name = "petrified-poop-sword",
      price = 40,
      category = "Weapons",
      description = "Deal 1 damage, but give the illness effect for 30 seconds, breaks after 25 uses",
      shortDescription = "Give your target an illness",
      classData = new ItemClassData {
        clazz = typeof(PetrifiedPoopSword),
        constructorArgs = null
      }
    });

    items.Add(new TypoableString("taser", 1), new ItemEntry {
      name = "taser",
      price = 40,
      category = "Weapons",
      description = "Deal 0 damage, but give your opponent stun, but also give your stun as you hold the taser to them. Consumes 2kw. Effects last 10 seconds",
      shortDescription = "Tase your target",
      classData = new ItemClassData {
        clazz = typeof(Taser),
        constructorArgs = null
      }
    });

    items.Add(new TypoableString("slingshot", 1), new ItemEntry {
      name = "slingshot",
      price = 0,
      category = "Weapons",
      description = "Deal 5 damage, can be used 50 times",
      shortDescription = "Deal 5 damage, lasts a while",
      classData = new ItemClassData {
        clazz = typeof(Slingshot),
        constructorArgs = null
      }
    });

    items.Add(new TypoableString("potato", 1), new ItemEntry {
      name = "potato",
      price = 2,
      category = "Food",
      description = "Heal 2 health when used, also used as ammo for potato cannon",
      shortDescription = "Heal 2 health when used, also used as ammo for potato cannon",
      classData = new ItemClassData {
        clazz = typeof(Food),
        constructorArgs = new Object[] {"potato", 2}
      }
    });

    items.Add(new TypoableString("bond", 1), new ItemEntry {
      name = "bond",
      price = 10,
      category = "Coins",
      description = "Lend out money, you get 1 coin in interest for every day you wait to sell this item",
      shortDescription = "Lend out money and get back more in interest",
      classData = new ItemClassData {
        clazz = typeof(Bond),
        constructorArgs = new Object[] {}
      }
    });

    items.Add(new TypoableString("solar-panel", 2), new ItemEntry {
      name = "solar-panel",
      price = 10,
      category = "Electricity",
      description = "Generate 10kw for 10 minutes",
      shortDescription = "Generate 10kw for 10 minutes",
      classData = new ItemClassData {
        clazz = typeof(SolarPanel),
        constructorArgs = new Object[] {}
      }
    });

    items.Add(new TypoableString("bandaid", 1), new ItemEntry {
      name = "bandaid",
      price = 3,
      category = "Health",
      description = "Give youself regen for 10 minutes when used, you'll ultimately regen 6 health after the 10 minutes",
      shortDescription = "Regen a bit of health when used",
      classData = new ItemClassData {
        clazz = typeof(Bandaid),
        constructorArgs = new object[] {}
      }
    });

    items.Add(new TypoableString("bandage", 1), new ItemEntry {
      name = "bandage",
      price = 10,
      category = "Health",
      description = "Give youself regen for 10 minutes when used, you'll ultimately regen 12 health after the 10 minutes",
      shortDescription = "Regen a bit more health when used than bandaids",
      classData = new ItemClassData {
        clazz = typeof(Bandage),
        constructorArgs = new object[] {}
      }
    });

    notForSaleItems.Add("Stonk", new ItemClassData {
      constructorArgs = new Object[] {},
      clazz = typeof(Stonk)
    });
  }
}