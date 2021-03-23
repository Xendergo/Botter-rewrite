using System.Collections.Generic;
using System;
class TypoableString {
  public string value;
  public int maxErrors;

  public TypoableString(string value, int maxErrors) {
    this.value = value;
    this.maxErrors = maxErrors;
  }

  // Allow for use in dictionaries
  public override int GetHashCode() {
    return value.GetHashCode();
  }

  public override string ToString() {
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

  // https://github.com/tallesl/wagner-fischer/blob/master/lib/wagner-fischer.js
  private static int LevenshteinDistance(string a, string b) {
    int[,] grid = new int[a.Length + 1, b.Length + 1];

    for (int i = 1; i <= a.Length; i++) {
      grid[i, 0] = i;
    }

    for (int j = 1; j <= b.Length; j++) {
      grid[0, j] = j;
    }

    for (int j = 1; j <= b.Length; j++) {
      for (int i = 1; i <= a.Length; i++) {
        grid[i, j] = a[i - 1] == b[j - 1] ? grid[i - 1, j - 1]
                                            :
                                            Math.Min(Math.Min(
                                            grid[i - 1, j] + 1,
                                            grid[i, j - 1] + 1),
                                            grid[i - 1, j - 1] + 1);
      }
    }

    return grid[a.Length, b.Length];
  }
}