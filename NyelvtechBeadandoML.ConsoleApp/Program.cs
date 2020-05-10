// This file was auto-generated by ML.NET Model Builder. 

using System;
using System.IO;
using System.Linq;
using Microsoft.ML;
using NyelvtechBeadandoML.Model;

namespace NyelvtechBeadandoML.ConsoleApp
{
    class Program
    {
        //Dataset to use for predictions 
        private const string DATA_FILEPATH = @"C:\Users\balint\OneDrive\itk\Nyelvtech\beadando\HU\train.tsv";

        static void Main(string[] args)
        {
            // Create single instance of sample data from first line of dataset for model input
            ModelInput sampleData = CreateSingleDataSample(DATA_FILEPATH);

            // Make a single prediction on the sample data and print results
            var predictionResult = ConsumeModel.Predict(sampleData);

            Console.WriteLine("Using model to make single prediction -- Comparing actual PARSEME_MWE with predicted PARSEME_MWE from sample data...\n\n");
            Console.WriteLine($"ID: {sampleData.ID}");
            Console.WriteLine($"FORM: {sampleData.FORM}");
            Console.WriteLine($"LEMMA: {sampleData.LEMMA}");
            Console.WriteLine($"UPOS: {sampleData.UPOS}");
            Console.WriteLine($"XPOS: {sampleData.XPOS}");
            Console.WriteLine($"FEATS: {sampleData.FEATS}");
            Console.WriteLine($"HEAD: {sampleData.HEAD}");
            Console.WriteLine($"DEPREL: {sampleData.DEPREL}");
            Console.WriteLine($"DEPS: {sampleData.DEPS}");
            Console.WriteLine($"MISC: {sampleData.MISC}");
            Console.WriteLine($"\n\nActual PARSEME_MWE: {sampleData.PARSEME_MWE} \nPredicted PARSEME_MWE value {predictionResult.Prediction} \nPredicted PARSEME_MWE scores: [{String.Join(",", predictionResult.Score)}]\n\n");
            Console.WriteLine("=============== End of process, hit any key to finish ===============");
            Console.ReadKey();
        }

        // Change this code to create your own sample data
        #region CreateSingleDataSample
        // Method to load single row of dataset to try a single prediction
        private static ModelInput CreateSingleDataSample(string dataFilePath)
        {
            // Create MLContext
            MLContext mlContext = new MLContext();

            // Load dataset
            IDataView dataView = mlContext.Data.LoadFromTextFile<ModelInput>(
                                            path: dataFilePath,
                                            hasHeader: true,
                                            separatorChar: '\t',
                                            allowQuoting: true,
                                            allowSparse: false);

            // Use first line of dataset as model input
            // You can replace this with new test data (hardcoded or from end-user application)
            ModelInput sampleForPrediction = mlContext.Data.CreateEnumerable<ModelInput>(dataView, false)
                                                                        .First();
            return sampleForPrediction;
        }
        #endregion
    }
}