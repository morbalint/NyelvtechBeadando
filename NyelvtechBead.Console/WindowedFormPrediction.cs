using NyelvtechBeadandoML.Model;

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

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
                    output = Enumerable.Repeat("_", 5 * (0 - start));
                    start = 0;
                }

                var tail = Enumerable.Empty<string>(); ;
                if (end > sentence.Length)
                {
                    tail = Enumerable.Repeat("_", 5 * (end - sentence.Length));
                    end = sentence.Length;
                }

                return output
                .Concat(sentence[start..end]
                    .SelectMany(x => new string[] { x[1], x[2], x[3], x[6], x[7] }))
                .Concat(tail);
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

                    if (int.Parse(line.Line.Split()[0]) == 1)
                    {
                        counter = 1;
                    }

                    var words = inputTransformer(line).ToArray();

                    var input = new ModelInput()
                    {
                        FORMm3 = words[0],
                        LEMMAm3 = words[1],
                        UPOSm3 = words[2],
                        HEADm3 = words[3],
                        DEPRELm3 = words[4],
                        FORMm2 = words[5],
                        LEMMAm2 = words[6],
                        UPOSm2 = words[7],
                        HEADm2 = words[8],
                        DEPRELm2 = words[9],
                        FORMm1 = words[10],
                        LEMMAm1 = words[11],
                        UPOSm1 = words[12],
                        HEADm1 = words[13],
                        DEPRELm1 = words[14],
                        FORM0 = words[15],
                        LEMMA0 = words[16],
                        UPOS0 = words[17],
                        HEAD0 = float.Parse(words[18]),
                        DEPREL0 = words[19],
                        FORMp1 = words[20],
                        LEMMAp1 = words[21],
                        UPOSp1 = words[22],
                        HEADp1 = words[23],
                        DEPRELp1 = words[24],
                        FORMp2 = words[25],
                        LEMMAp2 = words[26],
                        UPOSp2 = words[27],
                        HEADp2 = words[28],
                        DEPRELp2 = words[29],
                        FORMp3 = words[30],
                        LEMMAp3 = words[31],
                        UPOSp3 = words[32],
                        HEADp3 = words[33],
                        DEPRELp3 = words[34],
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
                .ToList() // need to materialize all lines, to assign the correct sentence to them.
                .Where(x => x.Sentence != null)
                .SelectMany(transformToTrainingData(windowHalfWidth))
                .Select(x => string.Join('\t', x));

        public static void mainPrepareTraining() =>
            File.WriteAllLines("train.mlnet.sen_w3_12367.tsv",
                prepareTrainingData(3, File.ReadAllLines("train.cupt")));

        public static void mainPredict() =>
            File.WriteAllLines("pred.mlnet.sen_w3_12367.cupt",
                File.ReadAllLines("test.cupt")
                .Select(SentenceBasedPrediction.sentenceAssigner, new List<string>())
                .Select(predict(3)(new ConsumeModel()), 1));
    }
}
