namespace Items {
  public class GoldenSword : IWeapon {
    override public string name {get;} = "golden-sword";
    override public void Attack(User target) {
      throw new System.NotImplementedException();
    }
  }
}