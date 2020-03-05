//RioNeuralNetwork: License information is available here - "https://github.com/TheRioMiner/RioNeuralNetwork/blob/master/LICENSE" or in file "LICENCE"

namespace RioNeuralNetwork
{
    public struct LearnInfo
    {
        public float LearnRate;
        public float Alpha;

        public uint Epochs;
        public uint ExampleIndex;
        public float ErrorPerEpoch;
        public float ErrorPerExample;
        public float LearnUntilError;

        public LearnInfo(float learnRate, float alpha)
        {
            this.LearnRate = learnRate;
            this.Alpha = alpha;

            this.Epochs = 0;
            this.ExampleIndex = 0;
            this.ErrorPerEpoch = 0f;
            this.ErrorPerExample = 0f;
            this.LearnUntilError = 0f;
        }
    }
}
