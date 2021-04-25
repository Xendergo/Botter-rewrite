using System;
using DSharpPlus.Entities;

namespace StatusEffects {
  public abstract class IStatusEffect {
    public int ticksLeft {get; protected set;} = 120;
    public float intensity {get; protected set;} = 10;
    public User owner;
    public abstract string name {get;}
    public DiscordChannel channel {get;}
    public void tick() {
      ticksLeft--;
      if (ticksLeft == 0) RemoveSelf();
      onTick();
    }
    protected virtual void onTick() {}
    protected virtual void onCreate() {}
    public void RemoveSelf() {
      owner.effects.Remove(this);
    }
    public static T AddStatusEffect<T>(User user, int time, float intensity) where T : IStatusEffect {
      T effect = Activator.CreateInstance<T>();
      effect.ticksLeft = time;
      effect.intensity = intensity;
      user.AddStatusEffect(effect);
      return effect;
    }
  }
}