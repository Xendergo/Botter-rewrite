using System.Collections.Generic;
using System;
using System.Threading.Tasks;
using Items;
using StatusEffects;
using DSharpPlus.Entities;

public interface BattleEntity {
  int health {get; set;}
  int magic {get; set;}
  int coins {get;}
  Battle battle {get; set;}
  Task<string> username {get;}
  List<IItem> items {get; set;}
  List<IStatusEffect> effects {get;}
  List<ElectricityConsumer> consumers {get;}
  ulong idCode {get;}
  void TransferCoins(int amt);
  int calcTax(int amt);
  IItem GetItem(string name);
  T GetItem<T>() where T : IItem;
  Optional<T> GetEffect<T>() where T : IStatusEffect;
  float GetEffectStrength<T>() where T : IStatusEffect;
  int CalculatePower();
  void BattleTick();
  void AddStatusEffectOnlytoBeUsedByIStatusEffect(IStatusEffect effect);
}

public abstract class BattleEntityImpl<K, V> : Cacheable<K, V>, BattleEntity {
  private int _health;
  public int health {
    get {
      return _health;
    }

    set {
      _health = Math.Clamp(value, 0, 100);
    }
  }

  public int magic {get; set;}
  public int coins {get; private set;}
  public Battle battle {get; set;} = null;
  public abstract Task<string> username {get; protected set;}
  public List<IItem> items {get; set;} = new List<IItem>();
  public List<IStatusEffect> effects {get;} = new List<IStatusEffect>();
  public List<ElectricityConsumer> consumers {get;} = new List<ElectricityConsumer>();
  public ulong idCode {get {
    return (ulong)id.GetHashCode();
  }}
  private bool isTicking = false;

  protected BattleEntityImpl(int coins, int magic, int health, K id, Dictionary<K, V> cache) : base(id, cache) {
    this.magic = magic;
    this.health = health;
    this.coins = coins;
  }

  public void TransferCoins(int amt) {
    coins += amt;
    coins -= calcTax(amt);
  }

  public int calcTax(int amt) {
    return calcTax(amt, coins);
  }

  public static int calcTax(int amt, int wealth) {
    // Sales tax dependent on wealth
    // Equasion where c is wealth & p is amt
    // (1.3 ^ (c / 1000) - 1) * sqrt(|p|)

    return (int)MathF.Truncate(MathF.Min(wealth, MathF.Abs(amt) * (MathF.Pow(1.3F, wealth * 0.0001F) - 1)));
  }

  public IItem GetItem(string name) {
    TypoableString itemName = TypoableString.FindClosestString(name, ItemRegistry.items.Keys);

    if (itemName is null) {
      throw new CommandException($"There is no item named *{name}*");
    }

    foreach (IItem item in items) {
      if (item.name == itemName.value) {
        return item;
      }
    }

    throw new CommandException($"You don't have a {itemName.value}");
  }

  public T GetItem<T>() where T : IItem {
    foreach (IItem item in items) {
      if (item is T) {
        return item as T;
      }
    }

    return null;
  }
  
  public Optional<T> GetEffect<T>() where T : IStatusEffect {
    float maxIntensity = 0;
    Optional<T> ret = new Optional<T>();

    foreach (IStatusEffect effect in effects) {
      if (effect is T && effect.intensity > maxIntensity) {
        ret = new Optional<T>(effect as T);
        maxIntensity = effect.intensity;
      }
    }

    return ret;
  }

  public float GetEffectStrength<T>() where T : IStatusEffect {
    float totalIntensity = 0;

    foreach (IStatusEffect effect in effects) {
      if (effect is T) {
        totalIntensity += effect.intensity;
      }
    }

    return totalIntensity;
  }

  public int CalculatePower() {
    int total = 0;
    foreach (IItem item in items) {
      if (item is IPowerGen && (item as IPowerGen).enabled) {
        total += (item as IPowerGen).CalculatePower();
      }
    }

    return total;
  }

  public void BattleTick() {
    resetKill();

    foreach (var item in items) {
      item.BattleTick();
    }

    int itemIndex = items.Count;
    int spareElectricity = 0;

    for (int i = consumers.Count - 1; i >= 0; i--) {
      ElectricityConsumer consumer = consumers[i];

      int consumed = consumer.electricityConsumption(consumer);

      while (spareElectricity < consumed) {
        itemIndex--;

        if (itemIndex == -1) {
          if (consumer.OutOfPower is not null) {
            consumer.OutOfPower(consumer);
          }
          consumer.hasPower = false;

          goto nextConsumer;
        }

        if (items[itemIndex] is IPowerGen && (items[itemIndex] as IPowerGen).enabled) {
          spareElectricity += (items[itemIndex] as IPowerGen).GeneratePower();
        }
      }

      spareElectricity -= consumed;
      if (!consumer.hasPower) {
        consumer.hasPower = true;
        
        if (consumer.HasPowerAgain is not null) {
          consumer.HasPowerAgain(consumer);
        }
      }

      nextConsumer:;
    }

    foreach (var item in items) {
      item.AfterBattleTick();
    }
  }

  private async void TickEffects() {
    isTicking = true;
    while (effects.Count > 0) {
      lock (effects) {
        for (int i = effects.Count - 1; i >= 0; i--) {
          effects[i].tick();
        }
      }

      resetKill();

      await Task.Delay(1000);
    }
    isTicking = false;
  }

  // NEVER CALL - Always use IStatusEffect.AddStatusEffect
  public void AddStatusEffectOnlytoBeUsedByIStatusEffect(IStatusEffect effect) {
    lock (effects) {
      effects.Add(effect);
    }

    if (!isTicking) TickEffects();
  }
}