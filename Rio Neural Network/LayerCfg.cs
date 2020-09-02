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
		public ThreadingMode LayerThreadingMode;


		public LayerCfg(int neuronsCount, ActivationType activationType, float layerLearnRate = 1f, ThreadingMode threadingMode = ThreadingMode.Default)
		{
			this.NeuronsCount = neuronsCount;
			this.NeuronsWeightsSize = 0; //Must be setted further
			this.ActivationType = activationType;
			this.LayerLearnRate = layerLearnRate;
			this.LayerThreadingMode = threadingMode;
		}

		internal LayerCfg(int neuronsCount, int neuronsWeightsSize, ActivationType activationType, float layerLearnRate, ThreadingMode threadingMode)
		{
			this.NeuronsCount = neuronsCount;
			this.NeuronsWeightsSize = neuronsWeightsSize;
			this.ActivationType = activationType;
			this.LayerLearnRate = layerLearnRate;
			this.LayerThreadingMode = threadingMode;
		}

		public LayerCfg(int neuronsCount, string activationType, float layerLearnRate = 1f, ThreadingMode threadingMode = ThreadingMode.Default)
		{
			this.NeuronsCount = neuronsCount;
			this.NeuronsWeightsSize = 0; //Must be setted further
			string actType = (activationType == null) ? string.Empty : activationType.ToLower();
			switch (actType)
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
				case "lrelu":
				case "leakyrelu":
					this.ActivationType = ActivationType.LReLU;
					break;

				default:
					throw new ArgumentException($"\"{activationType}\" - invalid activation type!");
			}
			this.LayerLearnRate = layerLearnRate;
			this.LayerThreadingMode = threadingMode;
		}

		public LayerCfg(int neuronsCount, float layerLearnRate = 1f, ThreadingMode threadingMode = ThreadingMode.Default) : this(neuronsCount, ActivationType.Sigmoid, layerLearnRate, threadingMode)
		{ }
	}
}
