using System.Collections.Generic;
using System;
using DSharpPlus.Entities;

public class Channel
{
  public LinkedList<Exception> ErrorStack = new LinkedList<Exception>();
  public void Error(Exception e, DiscordMessage msg) {
    msg.RespondAsync("There was an unexpected error running that command, send `botter debug` for details");

    ErrorStack.AddLast(e);
  }

  public Exception popError() {
    Exception e = ErrorStack.Last.Value;
    ErrorStack.RemoveLast();
    return e;
  }
}
