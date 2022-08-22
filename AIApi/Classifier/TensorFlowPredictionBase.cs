﻿using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using TensorFlow;

namespace AIApi.Classifier
{
    public interface IClassifier
    {
        Task<IEnumerable<LabelConfidence>> ClassifyImageAsync(byte[] image);
    }

    public abstract class TensorFlowPredictionBase : IClassifier
    {
        protected TensorFlowPredictionSettings _modelSettings;

        /// <summary>
        /// Classifiy Image using Deep Neural Networks
        /// </summary>
        /// <param name="image">image (jpeg) file to be analyzed</param>
        /// <returns>labels related to the image</returns>
        public Task<IEnumerable<LabelConfidence>> ClassifyImageAsync(byte[] image)
        {
            return ProcessAsync(image, _modelSettings);
        }

        protected async Task<IEnumerable<LabelConfidence>> ProcessAsync(byte[] image, TensorFlowPredictionSettings settings)
        {
            var (model, labels) = await LoadModelAndLabelsAsync(settings.ModelFilename, settings.LabelsFilename);
            var imageTensor = LoadImage(image);

            var labelsToReturn = Eval(model, imageTensor, settings.InputTensorName, settings.OutputTensorName, labels)
                                    .Where(c => c.Probability >= settings.Threshold)
                                    .OrderByDescending(c => c.Probability);
            return labelsToReturn;
        }

        protected abstract Task<(TFGraph, string[])> LoadModelAndLabelsAsync(string modelFilename, string labelsFilename);
        protected abstract TFTensor LoadImage(byte[] image);
        private IEnumerable<LabelConfidence> Eval(TFGraph graph, TFTensor imageTensor, string inputTensorName, string outputTensorName, string[] labels)
        {
            using var session = new TFSession(graph);
            var runner = session.GetRunner();

            // Create an input layer to feed (tensor) image, 
            // fetch label in output layer
            var input = graph[inputTensorName][0];

            var output = graph[outputTensorName][0];

            runner.AddInput(input, imageTensor).Fetch(output);

            var results = runner.Run();

            // convert output tensor in float array
            var probabilities = (float[,])results[0].GetValue(jagged: false);

            var idx = 0;
            return labels.Select(l => new LabelConfidence() { Label = l, Probability = probabilities[0, idx++] }).ToArray();
        }
    }
}
