using System.Collections.Generic;
using System.Threading.Tasks;

namespace AIApi.Classifier
{
    public interface ITensorFlowPredictionStrategy
    {
        Task<IEnumerable<string>> ClassifyImageAsync(byte[] image);
    }
}