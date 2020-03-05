//RioNeuralNetwork: License information is available here - "https://github.com/TheRioMiner/RioNeuralNetwork/blob/master/LICENSE" or in file "LICENCE"

#pragma once
#include "Activation.h"

struct LayerCfg
{
public:
    int NeuronsCount;
    int NeuronsWeightsSize;
    ActivationTypes ActivationType;
    float LayerLearnRate;
};