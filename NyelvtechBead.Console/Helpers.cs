using System.Collections.Generic;
using System.Linq;

namespace NyelvtechBead.Console
{
    public static class Helpers
    {
        public static string getLabel(List<string> sentence, int wordIndex) => getLabel(sentence, sentence[wordIndex]);

        public static string getLabel(IEnumerable<string> sentence, string label)
        {
            if (int.TryParse(label, out _))
            {
                label = sentence.FirstOrDefault(x => x.StartsWith(label + ":")) ?? label;
            }

            return label.Contains(":") ? label.Substring(label.IndexOf(":") + 1) : label;
        }

        public static IEnumerable<string> getLabels(IEnumerable<string> sentence, string label)
        {
            var sentenceLabels = sentence.SelectMany(x => x.Split(';'));
            return label.Split(';').Select(x => getLabel(sentenceLabels, x));
        }

        public static bool isSkipLine(string line) => string.IsNullOrWhiteSpace(line) || line.StartsWith("#") || line.Split().Length < 10;

    }
}
