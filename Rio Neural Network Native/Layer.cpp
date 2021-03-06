//RioNeuralNetwork: License information is available here - "https://github.com/TheRioMiner/RioNeuralNetwork/blob/master/LICENSE" or in file "LICENCE"

#include "Layer.h"


//Default constructor
Layer::Layer()
{
	NeuronsCount = 0;
	NeuronsWeightsSize = 0;

	LearnRate = 1.0f;
	ThreadingMode = ThreadingMode::INVALID;

	Weights = nullptr;
	WeightsMomentum = nullptr;
	Outputs = nullptr;
	Errors = nullptr;

	ActivationFunc = nullptr;
	ActivationType = ActivationTypes::INVALID;
}

Layer::Layer(LayerCfg layerCfg)
{
	//Set neurons count and neurons weights size
	NeuronsCount = layerCfg.NeuronsCount;
	NeuronsWeightsSize = layerCfg.NeuronsWeightsSize + 1/*bias*/;

	//Set layer learn rate
	LearnRate = layerCfg.LayerLearnRate;

	//Set threading mode
	ThreadingMode = layerCfg.LayerThreadingMode;

	//Init weights
	Weights = new float*[NeuronsCount];
	WeightsMomentum = new float*[NeuronsCount];
	for (int i = 0; i < NeuronsCount; i++)
	{
		Weights[i] = (float*)_mm_malloc(NeuronsWeightsSize * sizeof(float), sizeof(__m256));
		WeightsMomentum[i] = (float*)_mm_malloc(NeuronsWeightsSize * sizeof(float), sizeof(__m256));

		//Fill with zeroes
		Utils_FloatArrayFill(&Weights[i][0], 0.0f, NeuronsWeightsSize);
		Utils_FloatArrayFill(&WeightsMomentum[i][0], 0.0f, NeuronsWeightsSize);
	}

	//Init output and error arrays
	{
		Outputs = new float[NeuronsCount];
		Errors = new float[NeuronsCount];

		//Fill with zeroes
		Utils_FloatArrayFill(&Outputs[0], 0.0f, NeuronsCount);
		Utils_FloatArrayFill(&Errors[0], 0.0f, NeuronsCount);
	}

	//Init activation
	ActivationType = layerCfg.ActivationType;
	switch (ActivationType)
	{
		case ActivationTypes::SIGMOID:
			ActivationFunc = Activations::Sigmoid;
			break;
		case ActivationTypes::TANH:
			ActivationFunc = Activations::Tanh;
			break;
		case ActivationTypes::RELU:
			ActivationFunc = Activations::ReLU;
			break;
		case ActivationTypes::LRELU:
			ActivationFunc = Activations::LReLU;
			break;

		default:
			throw "Invalid activation type!";
	}
}

//Destructor
Layer::~Layer()
{
	//Delete weights and weights momentum arrays
	{
		for (int i = 0; i < NeuronsCount; i++)
		{
			_mm_free(Weights[i]);
			Weights[i] = nullptr;

			_mm_free(WeightsMomentum[i]);
			WeightsMomentum[i] = nullptr;
		}

		delete[] Weights;
		Weights = nullptr;
		delete[] WeightsMomentum;
		WeightsMomentum = nullptr;
	}

	//Delete outputs and errors arrays
	{
		delete[] Outputs;
		Outputs = nullptr;

		delete[] Errors;
		Errors = nullptr;
	}

	//Discard neurons count and weights size
	NeuronsCount = 0;
	NeuronsWeightsSize = 0;

	//Discard layer learn rate
	LearnRate = 0.0f;

	//Discard layer threading mode
	ThreadingMode = ThreadingMode::INVALID;

	//Discard activation
	ActivationType = ActivationTypes::INVALID;
	ActivationFunc = nullptr;
}