//RioNeuralNetwork: License information is available here - "https://github.com/TheRioMiner/RioNeuralNetwork/blob/master/LICENSE" or in file "LICENCE"

namespace RioNeuralNetwork
{
    public class WeightsFillInfo
    {
        public float CoefficientOfWeights;
        public bool NegativeWeights;

        public WeightsFillInfo(float CoefficientOfWeights, bool NegativeWeights = true)
        {
            this.CoefficientOfWeights = CoefficientOfWeights;
            this.NegativeWeights = NegativeWeights;
        }

        public WeightsFillInfo(bool NegativeWeights = true) : this(1f, NegativeWeights)
        { }
    }
}
