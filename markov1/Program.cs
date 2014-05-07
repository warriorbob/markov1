using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace markov1
{
    class Program
    {
        static void Main(string[] args)
        {
            var text = File.ReadAllText(@"words.txt");
            string[] words = text.Split(new char[0], StringSplitOptions.RemoveEmptyEntries);
            var bigDict = fillCorpus(words);
            string markovText = generateMarkovText(words, bigDict, 30);
            Console.WriteLine(markovText);
        }

        private static string generateMarkovText(string[] words, Dictionary<Tuple<string, string>, List<string>> bigDict, int numWords)
        {
            Random random = new Random();
            int seed = random.Next(words.Length);
            string w1 = words[seed];
            string w2 = words[seed+1];
            string output = "";
            try
            {
                for (int i = 0; i < numWords; i++)
                {
                    if (i > 0)
                        output += " ";

                    List<string> candidates = bigDict[new Tuple<string, string>(w1, w2)];
                    output += w1;
                    w1 = w2;
                    w2 = candidates[random.Next(candidates.Count)];
                }

            }
            catch (System.Collections.Generic.KeyNotFoundException ex)
            {
                // We're done.
            }
            finally
            {
                if (output[output.Length-1] != '.')
                    output += ".";
            }
            return output;
        }
        
        //Make a dictionary
        static Dictionary<Tuple<string, string>, List<string>> fillCorpus(string[] words)
        {
            Dictionary<Tuple<string, string>, List<string>> dict = new Dictionary<Tuple<string, string>, List<string>>();

            if (words.Length > 3)
            {
                for (int i = 0; i < words.Length - 2; i++)
                {
                    Tuple<string, string> key = new Tuple<string, string>(words[i], words[i + 1]);

                    if (dict.ContainsKey(key))
                    {
                        dict[key].Add(words[i+2]);
                    }
                    else
                    {
                        var newList = new List<string>();
                        newList.Add(words[i+2]);
                        dict.Add(key, newList);
                    }
                }
                return dict;
            }
            else
            {
                return dict;     // Shouldn't happen on any text that is actually interesting
            }
        }
    }
}
