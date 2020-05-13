using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using Microsoft.ML;
using NyelvtechBeadandoML.Model;

namespace NyelvtechBead.Console
{

    public class Token
    {
        public Guid Id { get; set; }

        public int PositionInsentence { get; set; }

        public string Form { get; set; }

        public string Lemma { get; set; }

        public string UPOS { get; set; }

        public string XPOS { get; set; }

        public string Feats { get; set; }

        public int Head { get; set; }

        public string Deps { get; set; }

        public string Misc { get; set; }

        public string Mwe { get; set; }
    }

    public class Sentence
    {
        public readonly List<Token> Tokens = new List<Token>();
    }


    public class Program
    {

        static string predict(ConsumeModel model, string line)
        {
            if (line.Length < 10 || line.StartsWith("#"))
            {
                return line;
            }
            var words = line.Split();
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
                PARSEME_MWE = words[14],
            };
            var prediction = model.Predict(input);
            return string.Join('\t', words.Take(words.Length - 1).Append(prediction.Prediction));
        }

        public static List<List<string[]>> transform2Sentences(string[] lines)
        {
            var sentences = new List<List<string[]>>();
            var 文 = new List<string[]>();
            foreach (var line in lines.Where(x => !x.StartsWith("#")))
            {
                if (!string.IsNullOrWhiteSpace(line))
                {
                    var words = line.Split();
                    if (words.Length < 10)
                    {
                        throw new Exception("too few columns!");
                    }
                    文.Add(words);
                }
                else if (文.Count > 0)
                {
                    sentences.Add(文);
                    文 = new List<string[]>();
                }
            }
            return sentences;
        }

        public static string getLabel(List<string> sentence, int wordIndex)
        {
            var label = sentence[wordIndex];

            if (int.TryParse(label, out _))
            {
                label = sentence.FirstOrDefault(x => x.StartsWith(label + ":")) ?? label;
            }

            return label.Contains(":") ? label.Substring(label.IndexOf(":") + 1) : label;
        }

        public static Func<List<string[]>, string[]> writeSentence(int window) => (List<string[]> sentence) =>
        {
            var rtn = new List<string>();
            for (var i = 0; i < sentence.Count; i++)
            {
                var line = "";
                for (var k = i - window; k <= i + window; k++)
                {
                    if (k < 0 || k >= sentence.Count)
                    {
                        line += "_\t_\t";
                    }
                    else
                    {
                        line += sentence[k][1] + "\t" + sentence[k][2] + "\t";
                    }
                }
                line += getLabel(sentence.Select(x => x[10]).ToList(), i);
                rtn.Add(line);
            }
            return rtn.ToArray();
        };

        public static Action<string, string> transform(int window)
        {
            var writer = writeSentence(window);
            return (string input, string output) =>
            {
                var original = File.ReadAllLines(input);
                var sentences = transform2Sentences(original);
                var outlines = sentences.SelectMany(writer);
                File.WriteAllLines(output, outlines);
            };
        }

        public static void predictUsingCurrentModel(string input, string output)
        {
            var original = File.ReadAllLines(input);

            var model = new ConsumeModel();

            var qq = original.Select(x => predict(model, x)).ToList();
            var fooooooooo = qq.Select(x => x.Split().Last()).GroupBy(x => x).Select(x => (x.Key, x.Count())).ToArray();
            foreach(var foo in fooooooooo)
            {
                System.Console.WriteLine(foo);
            }
            File.WriteAllLines(output, qq);
        }

        public static Func<List<string[]>, string[]> writeSentence2(int window) => (List<string[]> sentence) =>
        {
            var rtn = new List<string>();
            for (var i = 0; i < sentence.Count; i++)
            {
                var line = string.Join("<#>", sentence[i]) + "\t";
                for (var k = i - window; k <= i + window; k++)
                {
                    if (k < 0 || k >= sentence.Count)
                    {
                        line += "_\t_\t";
                    }
                    else
                    {
                        line += sentence[k][1] + "\t" + sentence[k][2] + "\t";
                    }
                }
                line += getLabel(sentence.Select(x => x[10]).ToList(), i);
                rtn.Add(line);
            }
            return rtn.ToArray();
        };


        public static List<List<string[]>> transform2Sentences2(string[] lines)
        {
            var sentences = new List<List<string[]>>();
            var 文 = new List<string[]>();
            foreach (var line in lines)
            {
                if (line.StartsWith("#"))
                {
                    sentences.Add(new List<string[]>() { new string[] { line } });
                }
                else
                {
                    if (!string.IsNullOrWhiteSpace(line))
                    {
                        var words = line.Split();
                        if (words.Length < 10)
                        {
                            throw new Exception("too few columns!");
                        }
                        文.Add(words);
                    }
                    else if (文.Count > 0)
                    {
                        sentences.Add(文);
                        文 = new List<string[]>();
                    }
                }
            }
            return sentences;
        }

        public static void predictUsingCurrentModel2(string input, string output)
        {
            var original = File.ReadAllLines(input);

            var model = new ConsumeModel();

            var qq = original.Select(x => predict(model, x)).ToList();
            var t = qq.Select(x => x.Split().Last()).Distinct().ToArray();
            File.WriteAllLines(output, qq);
        }

        static void Main(string[] args)
        {
            //predictUsingCurrentModel("test.blind.windowd_form_lemma.tsv", "pred.windowd_form_lemma.tsv");

            transform(3)("train.cupt", "train.windowd_form_lemma.tsv");
            transform(3)("test.blind.cupt", "test.blind.windowd_form_lemma.tsv");
            transform(3)("test.cupt", "test.windowd_form_lemma.tsv");

            //// Add input data
            //var input = new ModelInput();

            //// Load model and predict output of sample data
            //ModelOutput result = ConsumeModel.Predict(input);
        }
    }
}
