using System.Threading.Tasks;
using System;
public class ElectricityConsumer {
  public User user {get;}
  public Func<ElectricityConsumer, int> electricityConsumption {get;}
  // Called when there's not enough power to sustain this consumer
  public Action<ElectricityConsumer> OutOfPower;
  // Called when there's now enough power to sustain this consumer
  public Action<ElectricityConsumer> HasPowerAgain;
  public Action<ElectricityConsumer> OnRemoved;
  public bool hasPower = true;
  bool removed = false;
  public ElectricityConsumer(User user, Func<ElectricityConsumer, int> electricityConsumption) {
    this.electricityConsumption = electricityConsumption;
    this.user = user;
  }

  public void RemoveSelf() {
    if (OnRemoved is not null) {
      OnRemoved(this);
    }
    OutOfPower = null;
    HasPowerAgain = null;
    OnRemoved = null;
    user.consumers.Remove(this);
    removed = true;
  }

  public async void DestroyAfter(int time) {
    await Task.Delay(time);
    if (removed) return;
    RemoveSelf();
  }
}