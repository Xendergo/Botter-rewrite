using System;
using System.Threading.Tasks;
using DSharpPlus.Entities;
using DSharpPlus.EventArgs;
using Botter_rewrite;
using StatusEffects;

namespace Items {
  abstract class IRanged : IWeapon<string> {
    protected override async Task<string> DoAttack(BattleEntity target, BattleEntity attacker, DiscordMessage msg) {
      float dist = target.battle.getDistance(attacker, target);
      float time = dist / 50;

      DiscordMessage reply = await msg.RespondAsync($"**{await attacker.username}** shot you with a **{name}**, you have **{time} seconds** to dodge (react to this message)");
      await reply.CreateReactionAsync(DiscordEmoji.FromUnicode("🏹"));
      ReactionCollector collector = new ReactionCollector(Program.client, reply, (int)(time * 1000));

      bool dodged = false;

      collector.onReaction += (MessageReactionAddEventArgs args) => {
        if (args.User.Id.GetHashCode() == (int)target.idCode) {
          dodged = true;
        }
      };

      await Task.Delay((int)(time * 1000));

      float annoyanceIntensity = attacker.GetEffectStrength<Annoyance>();

      // Generate a random number within a particular distribution dependent on the distance & annoyance strength
      double v = InverseLogisticCurve(new Random().NextDouble(), dist * (1 + annoyanceIntensity * 0.2));

      float illnessIntensity = attacker.GetEffectStrength<Illness>();

      int damageToDealAfterIllness = (int)(damageToDeal * (1 - illnessIntensity));

      collector.Dispose();

      if (!dodged && v > -1 && v < 1) {
        string message = OnHit(target, attacker, msg);
        target.health -= damageToDealAfterIllness;
        return $"Oof, **{await attacker.username}** shot **{await target.username}** with a **{name}**, dealing **{damageToDeal}hp**, leaving them with **{target.health}hp**" + (message == "" ? "" : " - " + message);
      }

      if (dodged && v > 4 && v < 6) {
        string message = OnHit(target, attacker, msg);
        target.health -= damageToDealAfterIllness;
        return $"Oof, **{await attacker.username}** missed so hard that that they shot **{await target.username}** despite dodging with a **{name}**, dealing **{damageToDeal}hp**, leaving them with **{target.health}hp**" + (message == "" ? "" : " - " + message);
      }

      if (dodged && v >= -1 && v <= 1) {
        string message = OnMiss(target, attacker, msg);
        return $"Oof, **{await target.username}** dodged **{await target.username}'s** shot!" + (message == "" ? "" : " - " + message);
      }

      {
        string message = OnMiss(target, attacker, msg);
        return $"Oof, **{await target.username}** dodged **{await target.username}'s** shot! (though they would've missed anyways)" + (message == "" ? "" : " - " + message);
      }
    }

    public abstract string ammo {get;}
    public abstract int damageToDeal {get;}
    protected virtual string OnHit(BattleEntity target, BattleEntity attacker, DiscordMessage msg) { return ""; }
    protected virtual string OnMiss(BattleEntity target, BattleEntity attacker, DiscordMessage msg) { return ""; }

    protected double InverseLogisticCurve(double v, double dist) {
      return Math.Log(v / (1 - v)) * (dist * 0.01);
    }
  }
}