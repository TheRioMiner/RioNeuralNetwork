//RioNeuralNetwork: License information is available here - "https://github.com/TheRioMiner/RioNeuralNetwork/blob/master/LICENSE" or in file "LICENCE"

#pragma once

using namespace std;
#include <iostream>
#include <immintrin.h>

#include "Layer.h"
#include "LayerCfg.h"


struct NeuralNetwork
{
public:
    int LayersCount;
    Layer** Layers;


    NeuralNetwork(LayerCfg* layersCfg, int layersCount);

    ~NeuralNetwork();


    float* ForwardPropagate(float* input);

    void BackwardPropagateError(float* expected);

    void UpdateWeights(float* inputArrayPtr, int inputArraySize, float learnRate, float alpha);

private:
    void InternalUpdateWeights(float* neuronWeightsPtr, float* neuronWeightsMomentumPtr, float* inputPtr, int inputSize, float errorPlusCoeff, float alpha);
    float Activate(float* weights, float* inputs, int n);
};