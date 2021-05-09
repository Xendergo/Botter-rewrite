namespace Items {
  public interface IPowerGen : ITogglable {
    public int GeneratePower();
    public int CalculatePower();
  }
}