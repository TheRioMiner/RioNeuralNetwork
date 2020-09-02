//RioNeuralNetwork: License information is available here - "https://github.com/TheRioMiner/RioNeuralNetwork/blob/master/LICENSE" or in file "LICENCE"

using System;

namespace RioNeuralNetwork
{
	public unsafe struct Layer
	{
		public readonly int NeuronsCount;
		public readonly int NeuronsWeightsSize;
		private readonly IntPtr ActivationFunc; //Ptr to native function of the activation
		public readonly float** Weights;
		public readonly float** WeightsMomentum;
		public readonly float* Outputs;
		public readonly float* Errors;
		public float LearnRate;
		public readonly ActivationType ActivationType;
		public ThreadingMode ThreadingMode;



		public LayerCfg ToLayerCfg()
		{
			int neuronsWeights = (NeuronsWeightsSize > 1) ? (NeuronsWeightsSize - 1/*bias*/) : 0;
			return new LayerCfg(NeuronsCount, neuronsWeights, ActivationType, LearnRate, ThreadingMode);
		}
	}
}
