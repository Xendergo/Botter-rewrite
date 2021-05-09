namespace StatusEffects {
  [Effect(4, "Annoyance", "Make your commands take longer to run, and make your shots less accurate")]
  class Annoyance : IStatusEffect {
    public override string name {get;} = "Annoyance";
  }
}