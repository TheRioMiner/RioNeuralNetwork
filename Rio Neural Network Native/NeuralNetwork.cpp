//RioNeuralNetwork: License information is available here - "https://github.com/TheRioMiner/RioNeuralNetwork/blob/master/LICENSE" or in file "LICENCE"

#include "NeuralNetwork.h"


NeuralNetwork::NeuralNetwork(LayerCfg* layersCfg, int layersCount)
{
    //Set layers count
    LayersCount = layersCount;

    //Init layers
    Layers = new Layer*[LayersCount];
    for (int i = 0; i < LayersCount; i++)
    {
        auto l = layersCfg[i];
        if (i > 0)
            Layers[i] = new Layer(l.NeuronsCount, layersCfg[i - 1].NeuronsCount, l.ActivationType, l.LayerLearnRate);
        else
            Layers[i] = new Layer(l.NeuronsCount, l.NeuronsCount, l.ActivationType, l.LayerLearnRate);
    }
}

NeuralNetwork::~NeuralNetwork()
{
    //Dispose layers
    for (int i = 0; i < LayersCount; i++)
        delete Layers[i];

    //Dispose layers array
    delete[] Layers;
}


//AVX2 accelerated activation
float NeuralNetwork::Activate(float* weights, float* inputs, int size)
{
    int n = (size - 1);
    float a = weights[n];/*bias?*/;

    //Main cycle
    int i = 0;
    int mainCount = (n & -8);
    __m256 mainResult = _mm256_setzero_ps();
    for (; i < mainCount; i += 8)
    {
        //First result
        __m256 ni = _mm256_load_ps(inputs + i);
        __m256 w = _mm256_load_ps(weights + i);
        __m256 result = _mm256_mul_ps(ni, w);

        //Sum results
        mainResult = _mm256_add_ps(mainResult, result);
    }
    //Get result from main cycle
    if (i != 0) //We have result?
    {
        __m128 lo = _mm256_extractf128_ps(mainResult, 0);
        __m128 hi = _mm256_extractf128_ps(mainResult, 1);
        lo = _mm_add_ps(lo, hi);
        __declspec(align(sizeof(__m128))) float tmp[4];
        _mm_store_ps(tmp, lo);
        a += tmp[0] + tmp[1] + tmp[2] + tmp[3];
    }

    //End tail cycle
    for (; i < n; i++)
        a += inputs[i] * weights[i];

    //Return result!
    return a;
}

float* NeuralNetwork::ForwardPropagate(float* inputPtr)
{
    //Propagate first layer
    Layer* pFirstLayer = Layers[0];
    for (int j = 0; j < pFirstLayer->NeuronsCount; j++)
    {
        //Activate
        float activation = Activate(pFirstLayer->Weights[j], inputPtr, pFirstLayer->NeuronsWeightsSize);

        //And transfer to output
        pFirstLayer->Outputs[j] = pFirstLayer->ActivationFunc->Transfer(activation);
    }

    //Propagate to another layers
    for (int i = 1; i < LayersCount; i++)
    {
        Layer* pPrevLayer = Layers[i - 1];
        Layer* pCurrLayer = Layers[i];
        for (int j = 0; j < pCurrLayer->NeuronsCount; j++)
        {
            //Activate
            float activation = Activate(pCurrLayer->Weights[j], pPrevLayer->Outputs, pCurrLayer->NeuronsWeightsSize);

            //And transfer to output
            pCurrLayer->Outputs[j] = pCurrLayer->ActivationFunc->Transfer(activation);
        }
    }

    //Return results (pointer outputs of last layer neurons)
    return Layers[LayersCount - 1]->Outputs;
}

void NeuralNetwork::BackwardPropagateError(float* expectedPtr)
{
    int lastLayerIndex = (LayersCount - 1);
    for (int i = lastLayerIndex; i >= 0; i--)
    {
        Layer* pLayer = Layers[i];
        if (i != lastLayerIndex)
        {
            for (int j = 0; j < pLayer->NeuronsCount; j++)
            {
                //Calc error
                float error = 0.0f;
                Layer* pNextLayer = Layers[i + 1];
                int neuronsCount = pNextLayer->NeuronsCount;
                for (int k = 0; k < neuronsCount; k++)
                    error += pNextLayer->Weights[k][j] * pNextLayer->Errors[k];

                //Set
                pLayer->Errors[j] = error * pLayer->ActivationFunc->TransferDerivative(pLayer->Outputs[j]);
            }
        }
        else
        {
            for (int j = 0; j < pLayer->NeuronsCount; j++)
            {
                float error = expectedPtr[j] - pLayer->Outputs[j];
                pLayer->Errors[j] = error * pLayer->ActivationFunc->TransferDerivative(pLayer->Outputs[j]);
            }
        }
    }
}

//AVX2 accelerated update weights
void NeuralNetwork::InternalUpdateWeights(float* neuronWeightsPtr, float* neuronWeightsMomentumPtr, float* inputPtr, int inputSize, float errorPlusCoeff, float alpha)
{
    //Main cycle
    int i = 0;
    int mainCount = (inputSize & -8);
    __m256 coeff256 = _mm256_set1_ps(errorPlusCoeff);
    __m256 alpha256 = _mm256_set1_ps(alpha);
    for (; i < mainCount; i += 8)
    {
        __m256 input256 = _mm256_load_ps(inputPtr + i);
        input256 = _mm256_mul_ps(input256, coeff256);
        __m256 momentum256 = _mm256_load_ps(neuronWeightsMomentumPtr + i);
        momentum256 = _mm256_mul_ps(momentum256, alpha256);
        __m256 weightsAddResult256 = _mm256_add_ps(input256, momentum256);
        __m256 weigths256 = _mm256_load_ps(neuronWeightsPtr + i);
        weigths256 = _mm256_add_ps(weigths256, weightsAddResult256);
        _mm256_store_ps(neuronWeightsPtr + i, weigths256); //store weights
        _mm256_store_ps(neuronWeightsMomentumPtr + i, input256); //store weights momentum
    }

    //End tail cycle
    for (; i < inputSize; i++)
    {
        float weigthsMomentum = neuronWeightsMomentumPtr[i];
        float result = errorPlusCoeff * inputPtr[i];
        neuronWeightsPtr[i] += result + alpha * weigthsMomentum;
        neuronWeightsMomentumPtr[i] = result;
    }
}

void NeuralNetwork::UpdateWeights(float* inputArrayPtr, int inputArraySize, float learnRate, float alpha)
{
    float* inputPtr = inputArrayPtr;
    int inputSize = inputArraySize;
    for (int i = 0; i < LayersCount; i++)
    {
        //If its not first layer then get input from prev layers
        if (i != 0)
        {
            inputPtr = Layers[i - 1]->Outputs;
            inputSize = Layers[i - 1]->NeuronsCount;
        }

        Layer* pLayer = Layers[i];
        for (int j = 0; j < pLayer->NeuronsCount; j++)
        {
            float neuronError = pLayer->Errors[j];
            float errorPlusCoeff = (learnRate * pLayer->LayerLearnRate) * neuronError;
            float* neuronWeightsPtr = pLayer->Weights[j];
            float* neuronWeightsMomentumPtr = pLayer->WeightsMomentum[j];
            InternalUpdateWeights(neuronWeightsPtr, neuronWeightsMomentumPtr, inputPtr, inputSize, errorPlusCoeff, alpha);

            //Calc last
            float lastMomentum = neuronWeightsMomentumPtr[pLayer->NeuronsWeightsSize - 1];
            neuronWeightsPtr[pLayer->NeuronsWeightsSize - 1] += errorPlusCoeff + alpha * lastMomentum;
            neuronWeightsMomentumPtr[pLayer->NeuronsWeightsSize - 1] = errorPlusCoeff;
        }
    }
}