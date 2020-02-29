//RioNeuralNetwork: License information is available here - "https://github.com/TheRioMiner/RioNeuralNetwork/blob/master/LICENSE" or in file "LICENCE"

using System;

namespace Rio_Neural_Network
{
	public unsafe struct Layer
	{
		public readonly int NeuronsCount;
		public readonly int NeuronsWeightsSize;
		public float LayerLearnRate;
		public readonly float** Weights;
		public readonly float** WeightsMomentum;
		public readonly float* Outputs;
		public readonly float* Errors;
		public readonly ActivationType ActivationType;
		private readonly IntPtr ActivationFunc;
	}
}
