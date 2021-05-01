using System;
using DSharpPlus.Entities;

namespace StatusEffects {
  public abstract class IStatusEffect {
    public int ticksLeft {get; protected set;} = 120;
    public float intensity {get; protected set;} = 10;
    public BattleEntity owner;
    public Predicate<IStatusEffect> ToRemove;
    public abstract string name {get;}
    public DiscordChannel channel {get; private set;}
    public void tick() {
      ticksLeft--;
      if (ticksLeft == 0 || ToRemove(this)) RemoveSelf();
      onTick();
    }
    protected virtual void onTick() {}
    protected virtual void onCreate() {}
    public void RemoveSelf() {
      channel.SendMessageAsync($"You no longer have the effect **{name}**");
      owner.effects.Remove(this);
    }
    public static T AddStatusEffect<T>(BattleEntity battleEntity, int time, float intensity, DiscordChannel channel, Predicate<IStatusEffect> predicate) where T : IStatusEffect {
      T effect = Activator.CreateInstance<T>();
      effect.ticksLeft = time;
      effect.intensity = intensity;
      effect.owner = battleEntity;
      effect.channel = channel;
      effect.ToRemove = predicate;
      battleEntity.AddStatusEffectOnlytoBeUsedByIStatusEffect(effect);
      effect.onCreate();
      return effect;
    }

    public static T AddStatusEffect<T>(BattleEntity battleEntity, int time, float intensity, DiscordChannel channel) where T : IStatusEffect {
      return AddStatusEffect<T>(battleEntity, time, intensity, channel, (effect) => false);
    }
  }
}