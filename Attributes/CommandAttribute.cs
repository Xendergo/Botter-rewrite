using System;

[AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
public class CommandAttribute : Attribute, OrderedAttribute {
  public object[] args = null;
  public float ordinal {get;}
  public CommandAttribute(float ordinal) {
    this.ordinal = ordinal;
  }
}
