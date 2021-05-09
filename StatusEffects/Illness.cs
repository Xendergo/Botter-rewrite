namespace StatusEffects {
  [Effect(1, "Illness", "Reduces the amount of damage you deal in a fight")]
  class Illness : IStatusEffect {
    public override string name {get;} = "Illness";
  }
}