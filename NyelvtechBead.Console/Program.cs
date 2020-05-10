using System;
using System.IO;
using System.Linq;
using Microsoft.ML;
using NyelvtechBeadandoML.Model;

namespace NyelvtechBead.Console
{
    class Program
    {
        public class ConsumeModel
        {
            // Create new MLContext
            private readonly MLContext mlContext = new MLContext();
            private readonly PredictionEngine<ModelInput, ModelOutput> predEngine;
            public readonly DataViewSchema modelInputSchema;

            public ConsumeModel()
            {
                // Load model & create prediction engine
                string modelPath = @"MLModel.zip";
                ITransformer mlModel = mlContext.Model.Load(modelPath, out modelInputSchema);
                predEngine = mlContext.Model.CreatePredictionEngine<ModelInput, ModelOutput>(mlModel);
            }

            // For more info on consuming ML.NET models, visit https://aka.ms/model-builder-consume
            // Method for consuming model in your app
            public ModelOutput Predict(ModelInput input)
            {
                // Use model to make prediction on input data
                var result = predEngine.Predict(input);
                return result;
            }
        }

        static string predict(ConsumeModel model, string line)
        {
            if(line.Length < 10 || line.StartsWith("#"))
            {
                return line;
            }
            var words = line.Split();
            var input = new ModelInput()
            {
                ID = float.Parse(words[0]),
                FORM = words[1],
                LEMMA = words[2],
                UPOS = words[3],
                XPOS = words[4],
                FEATS = words[5],
                HEAD = float.Parse(words[6]),
                DEPREL = words[7],
                DEPS = words[8],
                MISC = words[9],
                PARSEME_MWE = words[10],
            };
            var prediction = model.Predict(input);
            return string.Join('\t', words.Take(words.Length-1).Append(prediction.Prediction));
        }

        static void Main(string[] args)
        {
            var original = File.ReadAllLines("test.blind.cupt");

            var model = new ConsumeModel();

            var qq = original.Select(x => predict(model, x));

            File.WriteAllLines("pred.mlnet2.cupt",qq);
            

            //// Add input data
            //var input = new ModelInput();

            //// Load model and predict output of sample data
            //ModelOutput result = ConsumeModel.Predict(input);
        }
    }
}
