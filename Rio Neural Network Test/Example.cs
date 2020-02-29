//RioNeuralNetwork: License information is available here - "https://github.com/TheRioMiner/RioNeuralNetwork/blob/master/LICENSE" or in file "LICENCE"

namespace Rio_Neural_Network_Test
{
    public struct Example
    {
        public float[] Input;
        public float[] DesiredResult;

        public Example(float[] Input, float[] DesiredResult)
        {
            this.Input = Input;
            this.DesiredResult = DesiredResult;
        }
    }
}
