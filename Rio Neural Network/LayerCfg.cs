//RioNeuralNetwork: License information is available here - "https://github.com/TheRioMiner/RioNeuralNetwork/blob/master/LICENSE" or in file "LICENCE"

using System;

namespace RioNeuralNetwork
{
	public struct LayerCfg
	{
		public int NeuronsCount;
		internal int NeuronsWeightsSize;
		public ActivationType ActivationType;
		public float LayerLearnRate;


		public LayerCfg(int neuronsCount, ActivationType activationType, float layerLearnRate = 1f)
		{
			this.NeuronsCount = neuronsCount;
			this.NeuronsWeightsSize = 0; //Must be setted further
			this.ActivationType = activationType;
			this.LayerLearnRate = layerLearnRate;
		}

		internal LayerCfg(int neuronsCount, int neuronsWeightsSize, ActivationType activationType, float layerLearnRate)
		{
			this.NeuronsCount = neuronsCount;
			this.NeuronsWeightsSize = neuronsWeightsSize;
			this.ActivationType = activationType;
			this.LayerLearnRate = layerLearnRate;
		}

		public LayerCfg(int neuronsCount, string activationType, float layerLearnRate = 1f)
		{
			this.NeuronsCount = neuronsCount;
			this.NeuronsWeightsSize = 0; //Must be setted further
			switch (activationType.ToLower())
			{
				case "":
				case "default":
				case "sigmoid":
				case "logistic":
					this.ActivationType = ActivationType.Sigmoid;
					break;
				case "tanh":
					this.ActivationType = ActivationType.Tanh;
					break;
				case "relu":
					this.ActivationType = ActivationType.ReLU;
					break;

				default:
					throw new ArgumentException($"\"{activationType}\" - invalid activation type!");
			}
			this.LayerLearnRate = layerLearnRate;
		}

		public LayerCfg(int neuronsCount, float layerLearnRate = 1f) : this(neuronsCount, ActivationType.Sigmoid, layerLearnRate)
		{ }
	}
}
