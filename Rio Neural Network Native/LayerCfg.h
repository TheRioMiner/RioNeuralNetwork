//RioNeuralNetwork: License information is available here - "https://github.com/TheRioMiner/RioNeuralNetwork/blob/master/LICENSE" or in file "LICENCE"

#pragma once
#include "Activation.h"

struct LayerCfg
{
public:
    int NeuronsCount;
    ActivationTypes ActivationType;
    float LayerLearnRate;

    LayerCfg(int NeuronsCount, ActivationTypes ActivationType, float LayerLearnRate)
    {
        LayerCfg::NeuronsCount = NeuronsCount;
        LayerCfg::ActivationType = ActivationType;
        LayerCfg::LayerLearnRate = LayerLearnRate;
    }

    LayerCfg(int NeuronsCount) : LayerCfg(NeuronsCount, ActivationTypes::SIGMOID, 1.0f)
    {}
};