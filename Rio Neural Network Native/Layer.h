//RioNeuralNetwork: License information is available here - "https://github.com/TheRioMiner/RioNeuralNetwork/blob/master/LICENSE" or in file "LICENCE"

#pragma once

using namespace std;
#include <vector>

#include "Utils.h"
#include "Activations.h"
#include "LayerCfg.h"


struct Layer
{
public:
	int NeuronsCount;
	int NeuronsWeightsSize;
	float LayerLearnRate;
	float** Weights;
	float** WeightsMomentum;
	float* Outputs;
	float* Errors;
	ActivationTypes ActivationType;
	Activation* ActivationFunc;
	

	Layer();

	Layer(LayerCfg layerCfg);

	~Layer();
};