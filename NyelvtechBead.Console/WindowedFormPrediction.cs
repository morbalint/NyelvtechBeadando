using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using NyelvtechBeadandoML.Model;

namespace NyelvtechBead.Console
{
    public static class WindowedFormPrediction
    {
        private static Func<LineWithSentence, IEnumerable<string>> getInputData(int windowHalfWidth)
            => (LineWithSentence line) =>
            {
                var sentence = line.Sentence.Select(x => x.Split()).ToArray();
                var tokens = line.Line.Split();
                var idx = int.Parse(tokens[0]) - 1;

                var start = idx - windowHalfWidth;
                var end = idx + windowHalfWidth + 1;

                var output = Enumerable.Empty<string>();

                if (start < 0)
                {
                    output = Enumerable.Repeat("_", 2*(0 - start));
                    start = 0;
                }

                var tail = Enumerable.Empty<string>(); ;
                if (end > sentence.Length)
                {
                    tail = Enumerable.Repeat("_", 2*(end - sentence.Length));
                    end = sentence.Length;
                }

                return output.Concat(sentence[start..end].SelectMany(x => new string[] { x[1], x[2] })).Concat(tail);
            };

        private static Func<LineWithSentence, IEnumerable<List<string>>> transformToTrainingData(int windowHalfWidth)
        {
            var inputTransformer = getInputData(windowHalfWidth);
            return (LineWithSentence line) =>
            {
                var sentence = line.Sentence.Select(x => x.Split()).ToArray();
                var tokens = line.Line.Split();
                var mwes = Helpers.getLabels(sentence.Select(x => x[^1]), tokens[^1]);

                var output = inputTransformer(line);

                return mwes
                    .Select(mwe => mwe.Split(':'))
                    .Select(x => x.Length > 1 ? string.Join(':', x[1..^0]) : x[0])
                    .Select(mwe => { var rtn = output.ToList(); rtn.Add(mwe); return rtn; });
            };
        }

        private static Func<ConsumeModel, Func<int, LineWithSentence, (int, string)>> predict(int windowHalfWidth)
        {
            var inputTransformer = getInputData(windowHalfWidth);
            return
                (ConsumeModel model) => 
                (int counter, LineWithSentence line) =>
                {
                    if (Helpers.isSkipLine(line.Line))
                    {
                        return (counter, line.Line);
                    }

                    if(int.Parse(line.Line.Split()[0]) == 1)
                    {
                        counter = 1;
                    }

                    var words = inputTransformer(line).ToArray();

                    var input = new ModelInput()
                    {
                        FORM_m3 = words[0],
                        LEMMA_m3 = words[1],
                        FORM_m2 = words[2],
                        LEMMA_m2 = words[3],
                        FORM_m1 = words[4],
                        LEMMA_m1 = words[5],
                        FORM0 = words[6],
                        LEMMA0 = words[7],
                        FORM_p1 = words[8],
                        LEMMA_p1 = words[9],
                        FORM_p2 = words[10],
                        LEMMA_p2 = words[11],
                        FORM_p3 = words[12],
                        LEMMA_p3 = words[13],
                        PARSEME_MWE = "_",
                    };
                    var prediction = model.Predict(input).Prediction;

                    var lineStart = string.Join('\t', line.Line.Split()[0..^1]) + "\t";

                    if (prediction == "*")
                    {
                        return (counter, lineStart + prediction);
                    }
                    else
                    {
                        var outLine = $"{lineStart}{counter}:{prediction}";
                        counter++;
                        return (counter, outLine);
                    }
                };
        }

        private static IEnumerable<string> prepareTrainingData(int windowHalfWidth, IEnumerable<string> lines) 
            => lines
                .Select(SentenceBasedPrediction.sentenceAssigner, new List<string>())
                .Where(x => x.Sentence != null)
                .SelectMany(transformToTrainingData(windowHalfWidth))
                .Select(x => string.Join('\t', x));

        public static void mainPrepareTraining() => 
            File.WriteAllLines("train.mlnet.sen_w3_form.tsv",
                prepareTrainingData(3, File.ReadAllLines("train.cupt")));

        public static void mainPredict() => 
            File.WriteAllLines("pred.mlnet.sen_w3_form.cupt",
                File.ReadAllLines("test.cupt")
                .Select(SentenceBasedPrediction.sentenceAssigner, new List<string>())
                .Select(predict(3)(new ConsumeModel()), 1));

    }
}
