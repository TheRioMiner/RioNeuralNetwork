//RioNeuralNetwork: License information is available here - "https://github.com/TheRioMiner/RioNeuralNetwork/blob/master/LICENSE" or in file "LICENCE"

#pragma once

using namespace std;
#include <iostream>
#include <immintrin.h>
#include <omp.h>

#include "Layer.h"
#include "LayerCfg.h"

#define NOINLINE __declspec(noinline)

struct NeuralNetwork
{
public:
    int LayersCount;
    Layer** Layers;


    NOINLINE NeuralNetwork(LayerCfg* layersCfg, int layersCount);

    NOINLINE ~NeuralNetwork();


    NOINLINE float* ForwardPropagate(float* input, bool setInputToFirstLayerOutput);

    NOINLINE void BackwardPropagateError(float* expected);

    NOINLINE void UpdateWeights(float* inputArrayPtr, int inputArraySize, float learnRate, float alpha);

private:
    NOINLINE void InternalUpdateWeights(float* neuronWeightsPtr, float* neuronWeightsMomentumPtr, float* inputPtr, int inputSize, float errorPlusCoeff, float alpha);
    NOINLINE float Activate(float* weights, float* inputs, int n);
};