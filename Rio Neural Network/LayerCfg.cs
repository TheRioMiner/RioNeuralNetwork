//RioNeuralNetwork: License information is available here - "https://github.com/TheRioMiner/RioNeuralNetwork/blob/master/LICENSE" or in file "LICENCE"

using System;

namespace Rio_Neural_Network
{
	public struct LayerCfg
	{
		public int NeuronsCount;
		public ActivationType ActivationType;
		public float LayerLearnRate;


		public LayerCfg(int neuronsCount, ActivationType activationType, float LayerLearnRate = 1f)
		{
			this.NeuronsCount = neuronsCount;
			this.ActivationType = activationType;
			this.LayerLearnRate = LayerLearnRate;
		}

		public LayerCfg(int neuronsCount, string activationType, float LayerLearnRate = 1f)
		{
			this.NeuronsCount = neuronsCount;
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
			this.LayerLearnRate = LayerLearnRate;
		}

		public LayerCfg(int neuronsCount, float LayerLearnRate = 1f) : this(neuronsCount, ActivationType.Sigmoid, LayerLearnRate)
		{ }
	}
}
