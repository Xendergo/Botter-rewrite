using System.Collections.Generic;
using System;
using DSharpPlus.Entities;

public struct DeletedMessage {
  public string AuthorUsername;
  public ulong AuthorID;
  public string Message;
  public IReadOnlyList<DiscordEmbed> Embeds;
}

public class EditedMessage : Cacheable<ulong, EditedMessage> {
  public string AuthorUsername;
  public LinkedList<string> history = new LinkedList<string>();
  public EditedMessage(ulong id, string Username, string first, Dictionary<ulong, EditedMessage> cache) : base(id, cache) {
    AuthorUsername = Username;
    history.AddLast(first);
  }
}

public class Channel
{
  public LinkedList<Exception> ErrorStack = new LinkedList<Exception>();
  public LinkedList<DeletedMessage> DeletedMessageStack = new LinkedList<DeletedMessage>();
  public Dictionary<ulong, EditedMessage> EditedMessages = new Dictionary<ulong, EditedMessage>();
  public void Error(Exception e, DiscordMessage msg) {
    msg.RespondAsync("There was an unexpected error running that command, send `botter debug` for details");

    ErrorStack.AddLast(e);
  }

  public Exception popError() {
    Exception e = ErrorStack.Last.Value;
    ErrorStack.RemoveLast();
    return e;
  }

  public void pushDeletedMessage(DiscordMessage msg) {
    DeletedMessageStack.AddLast(new DeletedMessage() {
      AuthorUsername = msg.Author.Username,
      AuthorID = msg.Author.Id,
      Message = msg.Content,
      Embeds = msg.Embeds
    });
  }

  public DeletedMessage popDeletedMessage() {
    DeletedMessage msg = DeletedMessageStack.Last.Value;
    DeletedMessageStack.RemoveLast();
    return msg;
  }

  public void AddEdit(DiscordMessage prev, DiscordMessage current) {
    ulong id = prev.Id;
    if (!EditedMessages.ContainsKey(id)) {
      EditedMessages.Add(id, new EditedMessage(id, prev.Author.Username, prev.Content, EditedMessages));
    }

    EditedMessages[id].history.AddLast(current.Content);
    EditedMessages[id].resetKill();
  }
}
