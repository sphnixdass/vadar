// Decompiled with JetBrains decompiler
// Type: VaderSharp.SentimentUtils
// Assembly: VaderSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 793A66C8-332C-4AC7-AFC8-D75362595257
// Assembly location: C:\Users\Dass\.nuget\packages\codingupastorm.vadersharp\1.0.4\lib\net35\VaderSharp.dll

using System;
using System.Collections.Generic;

namespace ConsoleApp1
{
  internal static class SentimentUtils
  {
    public const double BIncr = 0.293;
    public const double BDecr = -0.293;
    public const double CIncr = 0.733;
    public const double NScalar = -0.74;
    public static readonly string[] PuncList = new string[17]
    {
      ".",
      "!",
      "?",
      ",",
      ";",
      ":",
      "-",
      "'",
      "\"",
      "!!",
      "!!!",
      "??",
      "???",
      "?!?",
      "!?!",
      "?!?!",
      "!?!?"
    };
    public static readonly string[] Negate = new string[59]
    {
      "aint",
      "arent",
      "cannot",
      "cant",
      "couldnt",
      "darent",
      "didnt",
      "doesnt",
      "ain't",
      "aren't",
      "can't",
      "couldn't",
      "daren't",
      "didn't",
      "doesn't",
      "dont",
      "hadnt",
      "hasnt",
      "havent",
      "isnt",
      "mightnt",
      "mustnt",
      "neither",
      "don't",
      "hadn't",
      "hasn't",
      "haven't",
      "isn't",
      "mightn't",
      "mustn't",
      "neednt",
      "needn't",
      "never",
      "none",
      "nope",
      "nor",
      "not",
      "nothing",
      "nowhere",
      "oughtnt",
      "shant",
      "shouldnt",
      "uhuh",
      "wasnt",
      "werent",
      "oughtn't",
      "shan't",
      "shouldn't",
      "uh-uh",
      "wasn't",
      "weren't",
      "without",
      "wont",
      "wouldnt",
      "won't",
      "wouldn't",
      "rarely",
      "seldom",
      "despite"
    };
    public static readonly Dictionary<string, double> BoosterDict = new Dictionary<string, double>()
    {
      {
        "absolutely",
        0.293
      },
      {
        "amazingly",
        0.293
      },
      {
        "awfully",
        0.293
      },
      {
        "completely",
        0.293
      },
      {
        "considerably",
        0.293
      },
      {
        "decidedly",
        0.293
      },
      {
        "deeply",
        0.293
      },
      {
        "effing",
        0.293
      },
      {
        "enormously",
        0.293
      },
      {
        "entirely",
        0.293
      },
      {
        "especially",
        0.293
      },
      {
        "exceptionally",
        0.293
      },
      {
        "extremely",
        0.293
      },
      {
        "fabulously",
        0.293
      },
      {
        "flipping",
        0.293
      },
      {
        "flippin",
        0.293
      },
      {
        "fricking",
        0.293
      },
      {
        "frickin",
        0.293
      },
      {
        "frigging",
        0.293
      },
      {
        "friggin",
        0.293
      },
      {
        "fully",
        0.293
      },
      {
        "fucking",
        0.293
      },
      {
        "greatly",
        0.293
      },
      {
        "hella",
        0.293
      },
      {
        "highly",
        0.293
      },
      {
        "hugely",
        0.293
      },
      {
        "incredibly",
        0.293
      },
      {
        "intensely",
        0.293
      },
      {
        "majorly",
        0.293
      },
      {
        "more",
        0.293
      },
      {
        "most",
        0.293
      },
      {
        "particularly",
        0.293
      },
      {
        "purely",
        0.293
      },
      {
        "quite",
        0.293
      },
      {
        "really",
        0.293
      },
      {
        "remarkably",
        0.293
      },
      {
        "so",
        0.293
      },
      {
        "substantially",
        0.293
      },
      {
        "thoroughly",
        0.293
      },
      {
        "totally",
        0.293
      },
      {
        "tremendously",
        0.293
      },
      {
        "uber",
        0.293
      },
      {
        "unbelievably",
        0.293
      },
      {
        "unusually",
        0.293
      },
      {
        "utterly",
        0.293
      },
      {
        "very",
        0.293
      },
      {
        "almost",
        -0.293
      },
      {
        "barely",
        -0.293
      },
      {
        "hardly",
        -0.293
      },
      {
        "just enough",
        -0.293
      },
      {
        "kind of",
        -0.293
      },
      {
        "kinda",
        -0.293
      },
      {
        "kindof",
        -0.293
      },
      {
        "kind-of",
        -0.293
      },
      {
        "less",
        -0.293
      },
      {
        "little",
        -0.293
      },
      {
        "marginally",
        -0.293
      },
      {
        "occasionally",
        -0.293
      },
      {
        "partly",
        -0.293
      },
      {
        "scarcely",
        -0.293
      },
      {
        "slightly",
        -0.293
      },
      {
        "somewhat",
        -0.293
      },
      {
        "sort of",
        -0.293
      },
      {
        "sorta",
        -0.293
      },
      {
        "sortof",
        -0.293
      },
      {
        "sort-of",
        -0.293
      }
    };
    public static readonly Dictionary<string, double> SpecialCaseIdioms = new Dictionary<string, double>()
    {
      {
        "the shit",
        3.0
      },
      {
        "the bomb",
        3.0
      },
      {
        "bad ass",
        1.5
      },
      {
        "yeah right",
        -2.0
      },
      {
        "cut the mustard",
        2.0
      },
      {
        "kiss of death",
        -1.5
      },
      {
        "hand to mouth",
        -2.0
      }
    };

    /// <summary>Determine if input contains negation words</summary>
    /// <param name="inputWords"></param>
    /// <param name="includenT"></param>
    /// <returns></returns>
    public static bool Negated(IList<string> inputWords, bool includenT = true)
    {
      foreach (string str in SentimentUtils.Negate)
      {
        if (inputWords.Contains(str))
          return true;
      }
      if (includenT)
      {
        foreach (string inputWord in (IEnumerable<string>) inputWords)
        {
          if (inputWord.Contains("n't"))
            return true;
        }
      }
      if (inputWords.Contains("least"))
      {
        int num = inputWords.IndexOf("least");
        if (num > 0 && inputWords[num - 1] != "at")
          return true;
      }
      return false;
    }

    /// <summary>Normalizes score to be between -1 and 1</summary>
    /// <param name="score"></param>
    /// <param name="alpha"></param>
    /// <returns></returns>
    public static double Normalize(double score, double alpha = 15.0)
    {
      double num = score / Math.Sqrt(score * score + alpha);
      if (num < -1.0)
        return -1.0;
      return num > 1.0 ? 1.0 : num;
    }

    /// <summary>
    /// Checks whether some but not all of words in input are ALL CAPS
    /// </summary>
    /// <param name="words"></param>
    /// <returns></returns>
    public static bool AllCapDifferential(IList<string> words)
    {
      int num1 = 0;
      foreach (string word in (IEnumerable<string>) words)
      {
        if (word.IsUpper())
          ++num1;
      }
      int num2 = words.Count - num1;
      return num2 > 0 && num2 < words.Count;
    }

    /// <summary>
    /// Check if preceding words increase, decrease or negate the valence
    /// </summary>
    /// <param name="word"></param>
    /// <param name="valence"></param>
    /// <param name="isCapDiff"></param>
    /// <returns></returns>
    public static double ScalarIncDec(string word, double valence, bool isCapDiff)
    {
      string lower = word.ToLower();
      if (!SentimentUtils.BoosterDict.ContainsKey(lower))
        return 0.0;
      double num = SentimentUtils.BoosterDict[lower];
      if (valence < 0.0)
        num *= -1.0;
      if (word.IsUpper() & isCapDiff)
        num += valence > 0.0 ? 0.733 : -0.733;
      return num;
    }
  }
}
