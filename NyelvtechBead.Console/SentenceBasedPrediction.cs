using System.Collections.Generic;

namespace NyelvtechBead.Console
{
    public static class SentenceBasedPrediction
    {
        public static (List<string>, LineWithSentence) sentenceAssigner(List<string> sentence, string line)
        {
            if (Helpers.isSkipLine(line))
            {
                if (sentence.Count > 0)
                {
                    sentence = new List<string>();
                }
                return (sentence, new LineWithSentence
                {
                    Line = line,
                    Sentence = null
                });
            }
            else
            {
                sentence.Add(line);
                return (sentence, new LineWithSentence
                {
                    Line = line,
                    Sentence = sentence
                });
            }
        }
    }
}
