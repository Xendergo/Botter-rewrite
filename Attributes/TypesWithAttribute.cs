using System;
using System.Collections.Generic;
using System.Reflection;

static class TypesWithAttribute {
  public static List<(A, Type)> GetOrderedTypesWithAttribute<A, T>() where A : Attribute, OrderedAttribute {
    List<(A, Type)> ret = new List<(A, Type)>();

    foreach (Type type in Assembly.GetExecutingAssembly().GetTypes()) {
      if (!typeof(T).IsAssignableFrom(type)) continue;

      foreach (A attribute in type.GetCustomAttributes<A>()) {
        int index = ret.FindIndex((v) => v.Item1.ordinal > attribute.ordinal);
        if (index == -1) index = ret.Count;

        ret.Insert(index, (attribute, type));
      }
    }

    return ret;
  }

  public static List<(A, Type)> GetUnorderedTypesWithAttribute<A, T>() where A : Attribute {
    List<(A, Type)> ret = new List<(A, Type)>();

    foreach (Type type in Assembly.GetExecutingAssembly().GetTypes()) {
      if (!typeof(T).IsAssignableFrom(type)) continue;

      foreach (A listing in type.GetCustomAttributes<A>()) {
        ret.Add((listing, type));
      }
    }

    return ret;
  }
}