// Decompiled with JetBrains decompiler
// Type: VaderSharp.SentimentAnalysisResults
// Assembly: VaderSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 793A66C8-332C-4AC7-AFC8-D75362595257
// Assembly location: C:\Users\Dass\.nuget\packages\codingupastorm.vadersharp\1.0.4\lib\net35\VaderSharp.dll

namespace ConsoleApp1
{
  /// <summary>A model to represent the result of analysis.</summary>
  public class SentimentAnalysisResults
  {
    /// <summary>
    /// The proportion of words in the sentence with negative valence.
    /// </summary>
    public double Negative { get; set; }

    /// <summary>
    /// The proportion of words in the sentence with no valence.
    /// </summary>
    public double Neutral { get; set; }

    /// <summary>
    /// The proportion of words in the sentence with positive valence.
    /// </summary>
    public double Positive { get; set; }

    /// <summary>Normalized sentiment score between -1 and 1.</summary>
    public double Compound { get; set; }
  }
}
