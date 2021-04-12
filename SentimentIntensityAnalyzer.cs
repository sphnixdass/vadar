// Decompiled with JetBrains decompiler
// Type: VaderSharp.SentimentIntensityAnalyzer
// Assembly: VaderSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 793A66C8-332C-4AC7-AFC8-D75362595257
// Assembly location: C:\Users\Dass\.nuget\packages\codingupastorm.vadersharp\1.0.4\lib\net35\VaderSharp.dll

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace ConsoleApp1
{
  /// <summary>
  /// An abstraction to represent the sentiment intensity analyzer.
  /// </summary>
  public class SentimentIntensityAnalyzer
  {
    private const double ExclIncr = 0.292;
    private const double QuesIncrSmall = 0.18;
    private const double QuesIncrLarge = 0.96;

    private static Dictionary<string, double> Lexicon { get; }

    private static string[] LexiconFullFile { get; }

    static SentimentIntensityAnalyzer()
    {
     // using (Stream manifestResourceStream = typeof (SentimentIntensityAnalyzer).Assembly.GetManifestResourceStream(@"C:\Users\Dass\source\repos\ConsoleApp1\ConsoleApp1\VaderSharp.vader_lexicon.txt"))
      //{
        using (StreamReader streamReader = new StreamReader(@"C:\Users\Dass\source\repos\ConsoleApp1\ConsoleApp1\VaderSharp.vader_lexicon.txt"))
        {
          SentimentIntensityAnalyzer.LexiconFullFile = streamReader.ReadToEnd().Split('\n');
          SentimentIntensityAnalyzer.Lexicon = SentimentIntensityAnalyzer.MakeLexDic();
        }
      //}
    }

    private static Dictionary<string, double> MakeLexDic()
    {
      Dictionary<string, double> dictionary = new Dictionary<string, double>();
      foreach (string str in SentimentIntensityAnalyzer.LexiconFullFile)
      {
        string[] strArray = str.Trim().Split('\t');
        dictionary.Add(strArray[0], double.Parse(strArray[1]));
      }
      return dictionary;
    }

    /// <summary>
    /// Return metrics for positive, negative and neutral sentiment based on the input text.
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    public SentimentAnalysisResults PolarityScores(string input)
    {
      SentiText sentiText = new SentiText(input);
      IList<double> sentiments = (IList<double>) new List<double>();
      IList<string> wordsAndEmoticons = sentiText.WordsAndEmoticons;
      for (int index = 0; index < wordsAndEmoticons.Count; ++index)
      {
        string str = wordsAndEmoticons[index];
        double valence = 0.0;
        if (index < wordsAndEmoticons.Count - 1 && str.ToLower() == "kind" && wordsAndEmoticons[index + 1] == "of" || SentimentUtils.BoosterDict.ContainsKey(str.ToLower()))
          sentiments.Add(valence);
        else
          sentiments = this.SentimentValence(valence, sentiText, str, index, sentiments);
      }
      return this.ScoreValence(this.ButCheck(wordsAndEmoticons, sentiments), input);
    }

    private IList<double> SentimentValence(
      double valence,
      SentiText sentiText,
      string item,
      int i,
      IList<double> sentiments)
    {
      string lower = item.ToLower();
      if (!SentimentIntensityAnalyzer.Lexicon.ContainsKey(lower))
      {
        sentiments.Add(valence);
        return sentiments;
      }
      bool isCapDifferential = sentiText.IsCapDifferential;
      IList<string> wordsAndEmoticons = sentiText.WordsAndEmoticons;
      valence = SentimentIntensityAnalyzer.Lexicon[lower];
      if (isCapDifferential && item.IsUpper())
      {
        if (valence > 0.0)
          valence += 0.733;
        else
          valence -= 0.733;
      }
      for (int startI = 0; startI < 3; ++startI)
      {
        if (i > startI && !SentimentIntensityAnalyzer.Lexicon.ContainsKey(wordsAndEmoticons[i - (startI + 1)].ToLower()))
        {
          double num = SentimentUtils.ScalarIncDec(wordsAndEmoticons[i - (startI + 1)], valence, isCapDifferential);
          if (startI == 1 && num != 0.0)
            num *= 0.95;
          if (startI == 2 && num != 0.0)
            num *= 0.9;
          valence += num;
          valence = this.NeverCheck(valence, wordsAndEmoticons, startI, i);
          if (startI == 2)
            valence = this.IdiomsCheck(valence, wordsAndEmoticons, i);
        }
      }
      valence = this.LeastCheck(valence, wordsAndEmoticons, i);
      sentiments.Add(valence);
      return sentiments;
    }

    private IList<double> ButCheck(IList<string> wordsAndEmoticons, IList<double> sentiments)
    {
      bool flag1 = wordsAndEmoticons.Contains("BUT");
      bool flag2 = wordsAndEmoticons.Contains("but");
      if (!flag1 && !flag2)
        return sentiments;
      int num = flag1 ? wordsAndEmoticons.IndexOf("BUT") : wordsAndEmoticons.IndexOf("but");
      for (int index = 0; index < sentiments.Count; ++index)
      {
        double sentiment = sentiments[index];
        if (index < num)
        {
          sentiments.RemoveAt(index);
          sentiments.Insert(index, sentiment * 0.5);
        }
        else if (index > num)
        {
          sentiments.RemoveAt(index);
          sentiments.Insert(index, sentiment * 1.5);
        }
      }
      return sentiments;
    }

    private double LeastCheck(double valence, IList<string> wordsAndEmoticons, int i)
    {
      if (i > 1 && !SentimentIntensityAnalyzer.Lexicon.ContainsKey(wordsAndEmoticons[i - 1].ToLower()) && wordsAndEmoticons[i - 1].ToLower() == "least")
      {
        if (wordsAndEmoticons[i - 2].ToLower() != "at" && wordsAndEmoticons[i - 2].ToLower() != "very")
          valence *= -0.74;
      }
      else if (i > 0 && !SentimentIntensityAnalyzer.Lexicon.ContainsKey(wordsAndEmoticons[i - 1].ToLower()) && wordsAndEmoticons[i - 1].ToLower() == "least")
        valence *= -0.74;
      return valence;
    }

    private double NeverCheck(double valence, IList<string> wordsAndEmoticons, int startI, int i)
    {
      if (startI == 0)
      {
        if (SentimentUtils.Negated((IList<string>) new List<string>()
        {
          wordsAndEmoticons[i - 1]
        }))
          valence *= -0.74;
      }
      if (startI == 1)
      {
        if (wordsAndEmoticons[i - 2] == "never" && (wordsAndEmoticons[i - 1] == "so" || wordsAndEmoticons[i - 1] == "this"))
          valence *= 1.5;
        else if (SentimentUtils.Negated((IList<string>) new List<string>()
        {
          wordsAndEmoticons[i - (startI + 1)]
        }))
          valence *= -0.74;
      }
      if (startI == 2)
      {
        if (wordsAndEmoticons[i - 3] == "never" && (wordsAndEmoticons[i - 2] == "so" || wordsAndEmoticons[i - 2] == "this") || (wordsAndEmoticons[i - 1] == "so" || wordsAndEmoticons[i - 1] == "this"))
          valence *= 1.25;
        else if (SentimentUtils.Negated((IList<string>) new List<string>()
        {
          wordsAndEmoticons[i - (startI + 1)]
        }))
          valence *= -0.74;
      }
      return valence;
    }

    private double IdiomsCheck(double valence, IList<string> wordsAndEmoticons, int i)
    {
      string str1 = wordsAndEmoticons[i - 1] + " " + wordsAndEmoticons[i];
      string str2 = wordsAndEmoticons[i - 2] + " " + wordsAndEmoticons[i - 1] + " " + wordsAndEmoticons[i];
      string key1 = wordsAndEmoticons[i - 2] + " " + wordsAndEmoticons[i - 1];
      string str3 = wordsAndEmoticons[i - 3] + " " + wordsAndEmoticons[i - 2] + " " + wordsAndEmoticons[i - 1];
      string key2 = wordsAndEmoticons[i - 3] + " " + wordsAndEmoticons[i - 2];
      string[] strArray = new string[5]
      {
        str1,
        str2,
        key1,
        str3,
        key2
      };
      foreach (string key3 in strArray)
      {
        if (SentimentUtils.SpecialCaseIdioms.ContainsKey(key3))
        {
          valence = SentimentUtils.SpecialCaseIdioms[key3];
          break;
        }
      }
      if (wordsAndEmoticons.Count - 1 > i)
      {
        string key3 = wordsAndEmoticons[i] + " " + wordsAndEmoticons[i + 1];
        if (SentimentUtils.SpecialCaseIdioms.ContainsKey(key3))
          valence = SentimentUtils.SpecialCaseIdioms[key3];
      }
      if (wordsAndEmoticons.Count - 1 > i + 1)
      {
        string key3 = wordsAndEmoticons[i] + " " + wordsAndEmoticons[i + 1] + " " + wordsAndEmoticons[i + 2];
        if (SentimentUtils.SpecialCaseIdioms.ContainsKey(key3))
          valence = SentimentUtils.SpecialCaseIdioms[key3];
      }
      if (SentimentUtils.BoosterDict.ContainsKey(key2) || SentimentUtils.BoosterDict.ContainsKey(key1))
        valence += -0.293;
      return valence;
    }

    private double PunctuationEmphasis(string text) => this.AmplifyExclamation(text) + SentimentIntensityAnalyzer.AmplifyQuestion(text);

    private double AmplifyExclamation(string text)
    {
      int num = text.Count<char>((Func<char, bool>) (x => x == '!'));
      if (num > 4)
        num = 4;
      return (double) num * 0.292;
    }

    private static double AmplifyQuestion(string text)
    {
      int num = text.Count<char>((Func<char, bool>) (x => x == '?'));
      if (num < 1)
        return 0.0;
      return num <= 3 ? (double) num * 0.18 : 0.96;
    }

    private static SentimentIntensityAnalyzer.SiftSentiments SiftSentimentScores(
      IList<double> sentiments)
    {
      SentimentIntensityAnalyzer.SiftSentiments siftSentiments = new SentimentIntensityAnalyzer.SiftSentiments();
      foreach (double sentiment in (IEnumerable<double>) sentiments)
      {
        if (sentiment > 0.0)
          siftSentiments.PosSum += sentiment + 1.0;
        if (sentiment < 0.0)
          siftSentiments.NegSum += sentiment - 1.0;
        if (sentiment == 0.0)
          ++siftSentiments.NeuCount;
      }
      return siftSentiments;
    }

    private SentimentAnalysisResults ScoreValence(
      IList<double> sentiments,
      string text)
    {
      if (sentiments.Count == 0)
        return new SentimentAnalysisResults();
      double num1 = sentiments.Sum();
      double num2 = this.PunctuationEmphasis(text);
      double num3 = SentimentUtils.Normalize(num1 + (double) Math.Sign(num1) * num2);
      SentimentIntensityAnalyzer.SiftSentiments siftSentiments = SentimentIntensityAnalyzer.SiftSentimentScores(sentiments);
      if (siftSentiments.PosSum > Math.Abs(siftSentiments.NegSum))
        siftSentiments.PosSum += num2;
      else if (siftSentiments.PosSum < Math.Abs(siftSentiments.NegSum))
        siftSentiments.NegSum -= num2;
      double num4 = siftSentiments.PosSum + Math.Abs(siftSentiments.NegSum) + (double) siftSentiments.NeuCount;
      return new SentimentAnalysisResults()
      {
        Compound = Math.Round(num3, 4),
        Positive = Math.Round(Math.Abs(siftSentiments.PosSum / num4), 3),
        Negative = Math.Round(Math.Abs(siftSentiments.NegSum / num4), 3),
        Neutral = Math.Round(Math.Abs((double) siftSentiments.NeuCount / num4), 3)
      };
    }

    private class SiftSentiments
    {
      public double PosSum { get; set; }

      public double NegSum { get; set; }

      public int NeuCount { get; set; }
    }
  }
}
