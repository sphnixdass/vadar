using System;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;


namespace ConsoleApp1
{



    class Program
    {

        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");

            SentimentIntensityAnalyzer analyzer = new SentimentIntensityAnalyzer();

            var results = analyzer.PolarityScores("Wow, this package is amazingly easy to use");

            Console.WriteLine("Positive score: " + results.Positive);
            Console.WriteLine("Negative score: " + results.Negative);
            Console.WriteLine("Neutral score: " + results.Neutral);
            Console.WriteLine("Compound score: " + results.Compound);
        }
    }
}
