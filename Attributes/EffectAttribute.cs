using System;

[AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
public class EffectAttribute : Attribute, OrderedAttribute {
  public float ordinal {get;}
  public string name {get;}
  public string description {get;}
  public EffectAttribute(float ordinal, string name, string description) {
    this.ordinal = ordinal;
    this.name = name;
    this.description = description;
  }
}
