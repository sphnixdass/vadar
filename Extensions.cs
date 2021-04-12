// Decompiled with JetBrains decompiler
// Type: VaderSharp.Extensions
// Assembly: VaderSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 793A66C8-332C-4AC7-AFC8-D75362595257
// Assembly location: C:\Users\Dass\.nuget\packages\codingupastorm.vadersharp\1.0.4\lib\net35\VaderSharp.dll

using System;
using System.Linq;

namespace ConsoleApp1
{
  internal static class Extensions
  {
    /// <summary>Determine if word is ALL CAPS</summary>
    /// <param name="word"></param>
    /// <returns></returns>
    public static bool IsUpper(this string word) => !word.Any<char>(new Func<char, bool>(char.IsLower));

    /// <summary>Removes punctuation from word</summary>
    /// <param name="word"></param>
    /// <returns></returns>
    public static string RemovePunctuation(this string word) => new string(word.Where<char>((Func<char, bool>) (c => !char.IsPunctuation(c))).ToArray<char>());
  }
}
