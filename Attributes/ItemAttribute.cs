using System;

[AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
public class ItemAttribute : Attribute {
  public object[] args = null;
  public float ordinal {get;}
  public TypoableString name {get;}
  public int price {get;}
  public string category {get;}
  public string description {get;}
  public string shortDescription {get;}
  public ItemAttribute(float ordinal, string name, int maxErrors, int price, string category, string description, string shortDescription) {
    this.ordinal = ordinal;
    this.name = new TypoableString(name, maxErrors);
    this.price = price;
    this.category = category;
    this.description = description;
    this.shortDescription = shortDescription;
  }
}

[AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
public class HiddenItemAttribute : Attribute {
  public object[] args = null;
  public string name {get;}
  public HiddenItemAttribute(string name) {
    this.name = name;
  }
}

