using Microsoft.ML;
using NyelvtechBeadandoML.Model;

namespace NyelvtechBead.Console
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
        public ModelOutput Predict(ModelInput input) =>
            // Use model to make prediction on input data
            predEngine.Predict(input);
    }
}
