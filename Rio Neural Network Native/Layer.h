//RioNeuralNetwork: License information is available here - "https://github.com/TheRioMiner/RioNeuralNetwork/blob/master/LICENSE" or in file "LICENCE"

#pragma once

using namespace std;
#include <vector>

#include "Utils.h"
#include "Activations.h"
#include "LayerCfg.h"

#define NOINLINE __declspec(noinline)

struct Layer
{
public:
	int NeuronsCount;
	int NeuronsWeightsSize;
	Activation* ActivationFunc;
	float** Weights;
	float** WeightsMomentum;
	float* Outputs;
	float* Errors;
	float LearnRate;
	ActivationTypes ActivationType;
	ThreadingMode ThreadingMode;
	


	NOINLINE Layer();

	NOINLINE Layer(LayerCfg layerCfg);

	NOINLINE ~Layer();
};