using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Timers;

namespace pattern
{
    class Program
    {
        static string _text;
        static string _pattern;
        static void Main(string[] args)
        {
            if (args.Length != 2)
            {
                Console.WriteLine("Usage: pattern textToParse pattern");
                return;
            }
            _text = args[0];
            _pattern = args[1];
            DebugAlgo(BruteForceStringMatch);
            DebugAlgo(Mp);
            DebugAlgo(Kmp);
            Console.ReadKey();
        }

        delegate int CallBack(string text, string match);



        static void DebugAlgo(CallBack patternMatcher)
        {
            var result = TestAlgo(patternMatcher);
            Console.WriteLine("time:"+result.Item2+" founds:"+result.Item1);
        }

        static Tuple<int, long> TestAlgo(CallBack patternMatcher)
        {
            
            return TestAlgo(patternMatcher, _text, _pattern);
        }

        static Tuple<int, long> TestAlgo(CallBack patternMatcher, string text, string match)
        {
            var watch = System.Diagnostics.Stopwatch.StartNew();
            int output = patternMatcher(text, match);
            watch.Stop();
            return new Tuple<int, long>(output, watch.ElapsedMilliseconds);
        }

        static bool CheckWordInText(string text, int indexText, string pattern)
        {
            if (indexText + pattern.Length > text.Length) return false;
            return !pattern.Where((t, i) => text[i + indexText] != t).Any();
        }

        static int Mp(string text, string pattern)
        {

            var indexText = FoundIndexes(text, pattern);

            return indexText.Count(i => CheckWordInText(text, i, pattern));
        }

        static IEnumerable<int> FoundIndexes(string text, string pattern)
        {
            var foundsIndex = new List<int>();
            for (int i = 0; i < text.Length; i++)
            {
                if (text[i] == pattern[0])
                {
                    foundsIndex.Add(i);
                }
            }

            return foundsIndex;
        }

        static bool CheckCharThenWord(string text, int indexText, string pattern)
        {
            return text[indexText] == pattern[0] && CheckWordInText(text, indexText, pattern);
        }

        static int Kmp(string text, string pattern)
        {
            int secondCharPos = pattern.Substring(1).FirstOrDefault((c => c==pattern[0]));
            int found = 0;
            for (int i = 0; i < text.Length; i++)
            {
                int skip = 0;
                bool ok = true;
                for (int j = 0; j < pattern.Length; j++)
                {
                    if (text[i + j] != pattern[j])
                    {
                        skip = j;
                        ok = false;
                        break;
                    }
                }

                if (!ok && skip == 0)
                {
                    continue;
                }
                if (ok)
                {
                    found++;
                    i += pattern.Length-1;
                    continue;
                }
                if(skip>secondCharPos)
                {
                    i += secondCharPos;
                }
                else
                {
                    i += skip;
                }

                if (i + pattern.Length > text.Length)
                {
                    break;
                }
            }

            return found;
        }


        static int BruteForceStringMatch(string text, string pattern)
        {
            return text.Where((t, i) => CheckWordInText(text, i, pattern)).Count();
        }
    }
}
