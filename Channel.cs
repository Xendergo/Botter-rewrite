using System.Collections.Generic;
using System;
using DSharpPlus.Entities;

public struct DeletedMessage {
  public string AuthorUsername;
  public string Message;
}

public class Channel
{
  public LinkedList<Exception> ErrorStack = new LinkedList<Exception>();
  public LinkedList<DeletedMessage> DeletedMessageStack = new LinkedList<DeletedMessage>();
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
      Message = msg.Content
    });
  }

  public DeletedMessage popDeletedMessage() {
    DeletedMessage msg = DeletedMessageStack.Last.Value;
    DeletedMessageStack.RemoveLast();
    return msg;
  }
}
