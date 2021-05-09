namespace StatusEffects {
  [Effect(3, "Stun", "Stuns you, prevents you from executing battle commands")]
  class Stun : IStatusEffect {
    public override string name {get;} = "Stun";
  }
}