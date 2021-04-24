namespace Items {
  public class GoldenSword : IWeapon {
    override public string name {get;} = "golden-sword";
    public override int maxDamage {get;} = 15;
    override public void Attack(User target) {
      throw new System.NotImplementedException();
    }
  }
}