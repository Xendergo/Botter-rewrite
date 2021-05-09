using System.Collections.Generic;
using System;
using NUnit.Framework;

public class TypoableString : IEquatable<TypoableString> {
  public string value;
  public int maxErrors;

  public TypoableString(string value, int maxErrors) {
    this.value = value;
    this.maxErrors = maxErrors;
  }

  public bool Equals(TypoableString other) {
    if (other is null) return false;

    return other.value == value;
  }

  public override bool Equals(object obj)
  {
    if (obj is not TypoableString) return false;

    return ((TypoableString) obj).value.Equals(value);
  }

  // Allow for use in dictionaries
  override public int GetHashCode() {
    return value.GetHashCode();
  }

  override public string ToString() {
    return value;
  }

  public static TypoableString FindClosestString(string str, IEnumerable<TypoableString> options) {
    int min = int.MaxValue;
    TypoableString ret = null;
    foreach (TypoableString option in options) {
      int v = LevenshteinDistance(str, option.value);
      if (v <= option.maxErrors && v < min) {
        min = v;
        ret = option;
      }
    }

    return ret;
  }

  // https://www.dotnetperls.com/levenshtein
  // https://stackoverflow.com/questions/10178043/levenshtein-edit-distance-algorithm-that-supports-transposition-of-two-adjacent
  public static int LevenshteinDistance(string a, string b) {
    int[,] grid = new int[a.Length + 1, b.Length + 1];

    if (a.Length == 0) {
      return b.Length;
    }

    if (b.Length == 0) {
      return a.Length;
    }

    for (int i = 0; i <= a.Length; i++) {
      grid[i, 0] = i;
    }

    for (int j = 0; j <= b.Length; j++) {
      grid[0, j] = j;
    }

    for (int i = 1; i <= a.Length; i++) {
      for (int j = 1; j <= b.Length; j++) {
        int cost = (b[j - 1] == a[i - 1]) ? 0 : 1;
        grid[i, j] = Math.Min(
        Math.Min(grid[i - 1, j] + 1, grid[i, j - 1] + 1),
        grid[i - 1, j - 1] + cost);

        if (i > 1 && j > 1 && (a[i - 1] == b[j - 2]) && (a[i - 2] == b[j - 1])) {
          grid[i, j] = Math.Min(
            grid[i, j],
            grid[i - 2, j - 2] + cost
          );
        }
      }
    }

    return grid[a.Length, b.Length];
  }
}

public class Employee : IEquatable<Employee>
{
    public string Account;
    public string Name;

    public Employee(string account, string name)
    {
        this.Account = account;
        this.Name = name;
    }

    public bool Equals(Employee other)
    {
        if (other == null) return false;
        return (this.Account.Equals(other.Account));
    }
}