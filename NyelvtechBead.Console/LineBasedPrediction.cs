using System.Linq;
using Microsoft.ML;
using NyelvtechBeadandoML.Model;

namespace NyelvtechBead.Console
{
    /// <summary>
    /// most of the related code was lost when I started to rewrite the program for sentence based prediction
    /// </summary>
    public static class LineBasedPrediction
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
    }
}
