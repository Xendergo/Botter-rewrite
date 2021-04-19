using System;
using DSharpPlus.Entities;
using DSharpPlus;
using DSharpPlus.EventArgs;
using System.Threading.Tasks;
public class ReactionCollector : IDisposable {
  public delegate void OnReaction(MessageReactionAddEventArgs args);
  public delegate void OnDispose();
  public OnReaction onReaction;
  public OnDispose onDispose;
  private DiscordClient client;
  private ulong id;
  public ReactionCollector(DiscordClient client, DiscordMessage msg, int time) {
    client.MessageReactionAdded += onReactionAdded;
    id = msg.Id;
    this.client = client;
    DisposeWhenDone(time);
  }

  private async Task onReactionAdded(DiscordClient client, MessageReactionAddEventArgs args) {
    if (args.Message.Id == id) {
      if (onDispose is not null) {
        onReaction(args);
      }
    }
  }

  private async void DisposeWhenDone(int time) {
    await Task.Delay(time);
    if (onDispose is not null) {
      onDispose();
    }
    Dispose();
  }

  public void Dispose() {
    GC.SuppressFinalize(this);

    client.MessageReactionAdded -= onReactionAdded;

    onReaction = null;
    onDispose = null;
  }
}