// Decompiled with JetBrains decompiler
// Type: VaderSharp.SentiText
// Assembly: VaderSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 793A66C8-332C-4AC7-AFC8-D75362595257
// Assembly location: C:\Users\Dass\.nuget\packages\codingupastorm.vadersharp\1.0.4\lib\net35\VaderSharp.dll

using System;
using System.Collections.Generic;
using System.Linq;

namespace ConsoleApp1
{
  internal class SentiText
  {
    private string Text { get; }

    public IList<string> WordsAndEmoticons { get; }

    public bool IsCapDifferential { get; }

    public SentiText(string text)
    {
      this.Text = text;
      this.WordsAndEmoticons = this.GetWordsAndEmoticons();
      this.IsCapDifferential = SentimentUtils.AllCapDifferential(this.WordsAndEmoticons);
    }

    /// <summary>
    /// Returns mapping of the form {'cat,': 'cat'}, {',cat': 'cat'}
    /// </summary>
    /// <returns></returns>
    private Dictionary<string, string> WordsPlusPunc()
    {
      IEnumerable<string> strings = ((IEnumerable<string>) this.Text.RemovePunctuation().Split()).Where<string>((Func<string, bool>) (x => x.Length > 1));
      Dictionary<string, string> dictionary = new Dictionary<string, string>();
      foreach (string str in strings)
      {
        foreach (string punc in SentimentUtils.PuncList)
        {
          if (!dictionary.ContainsKey(str + punc))
          {
            dictionary.Add(str + punc, str);
            dictionary.Add(punc + str, str);
          }
        }
      }
      return dictionary;
    }

    /// <summary>
    /// Removes leading and trailing punctuation. Leaves contractions and most emoticons.
    /// </summary>
    /// <returns></returns>
    private IList<string> GetWordsAndEmoticons()
    {
      IList<string> list = (IList<string>) ((IEnumerable<string>) this.Text.Split()).Where<string>((Func<string, bool>) (x => x.Length > 1)).ToList<string>();
      Dictionary<string, string> dictionary = this.WordsPlusPunc();
      for (int index = 0; index < list.Count; ++index)
      {
        if (dictionary.ContainsKey(list[index]))
          list[index] = dictionary[list[index]];
      }
      return list;
    }
  }
}
