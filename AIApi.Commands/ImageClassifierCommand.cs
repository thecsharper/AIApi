using System.Runtime.Serialization;

namespace AIApi.Commands
{
    public class ImageClassifierCommand
    {
        [DataMember]
        public string ClassificationResponse { get; set; }

        public ImageClassifierCommand(string classificationResponse)
        {
            ClassificationResponse = classificationResponse;
        }
    }
}
