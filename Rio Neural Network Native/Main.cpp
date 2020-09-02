//RioNeuralNetwork: License information is available here - "https://github.com/TheRioMiner/RioNeuralNetwork/blob/master/LICENSE" or in file "LICENCE"

#include "Utils.h"
#include "Activations.h"
#include "NeuralNetwork.h"


extern "C"
{
    __declspec(dllexport) NeuralNetwork* CreateInstance(LayerCfg* layersCfgArrayPtr, int layersCfgArraySize)
    {
        return new NeuralNetwork(layersCfgArrayPtr, layersCfgArraySize);
    }

    __declspec(dllexport) void DeleteInstance(NeuralNetwork* instance)
    {
        if (instance)
            delete instance;
    }



    __declspec(dllexport) float* ForwardPropagate_Ptr(NeuralNetwork* instance, float* inputPtr, int inputSize, bool setInputToFirstLayerOutput)
    {
        if (!instance)
            return nullptr; //Null pointer to instance

        if (!inputPtr || inputSize < instance->Layers[0]->NeuronsCount)
            return nullptr; //Invalid input array

        //Forward propagate!
        float* resultPtr = instance->ForwardPropagate(inputPtr, setInputToFirstLayerOutput);

        //Return result (pointer to last layer neuron outputs)
        return resultPtr;
    }

    __declspec(dllexport) int ForwardPropagate_Cpy(NeuralNetwork* instance, float* inputArrayPtr, int inputArraySize, float* outputArrayPtr, bool setInputToFirstLayerOutput)
    {
        if (!instance)
            return false; //Null pointer to instance

        if (!inputArrayPtr || inputArraySize < instance->Layers[0]->NeuronsCount)
            return false; //Invalid input array pointer or size

        if (!outputArrayPtr)
            return false; //Invalid output arrray pointer

        //Forward propagate!
        float* resultPtr = instance->ForwardPropagate(inputArrayPtr, setInputToFirstLayerOutput);

        //Copy results to output array
        int outputNeuronsCount = instance->Layers[instance->LayersCount - 1]->NeuronsCount;
        memcpy(outputArrayPtr, resultPtr, (outputNeuronsCount * sizeof(float)));

        //Success!
        return true;
    }

    __declspec(dllexport) int BackwardPropagateError(NeuralNetwork* instance, float* expectedArrayPtr, int expectedArraySize)
    {
        if (!instance)
            return false; //Null pointer to instance

        //Check input size
        if (expectedArraySize < instance->Layers[instance->LayersCount - 1]->NeuronsCount)
            return false; //Size of expected arrray is invalid

        //Backward propagate
        instance->BackwardPropagateError(expectedArrayPtr);

        //Success
        return true;
    }

    __declspec(dllexport) int UpdateWeights(NeuralNetwork* instance, float* inputArrayPtr, float learRate, float alpha)
    {
        if (!instance)
            return false; //Null pointer to instance

        //Update weights
        instance->UpdateWeights(inputArrayPtr, instance->Layers[0]->NeuronsCount, learRate, alpha);

        //Success
        return true;
    }



    //<---- Helpful ---->

    __declspec(dllexport) bool IsProcessorSupportAVX()
    {
        return Utils_IsProcessorSupportAVX();
    }

    __declspec(dllexport) int GetProcsNum()
    {
        return Utils_GetProcsNum();
    }

    __declspec(dllexport) void FloatArrayFill(float* floatArrayPtr, float value, int floatArraySize)
    {
        Utils_FloatArrayFill(floatArrayPtr, value, floatArraySize);
    }

    __declspec(dllexport) void FloatArrayRandomAdd(float* floatArrayPtr, int floatArraySize, float noiseCoeff, bool negative, bool limit, int seed, ThreadingMode threadingMode)
    {
        Utils_FloatArrayRandomAdd(floatArrayPtr, floatArraySize, noiseCoeff, negative, limit, seed, threadingMode);
    }

    __declspec(dllexport) void FloatArrayRandomFill(float* floatArrayPtr, int floatArraySize, float noiseCoeff, bool negative, int seed, ThreadingMode threadingMode)
    {
        Utils_FloatArrayRandomFill(floatArrayPtr, floatArraySize, noiseCoeff, negative, seed, threadingMode);
    }


    __declspec(dllexport) float MeanSquaredError(float* etalonPtr, float* predictedPtr, int size, ThreadingMode threadingMode)
    {
        return Utils_MeanSquaredError(etalonPtr, predictedPtr, size, threadingMode);
    }


    __declspec(dllexport) void ConvertBitmapToFloatArrayRGB(float* floatArrayPtr, unsigned char* bitmapScan0Ptr, int bitmapStride, int bitmapWidth, int bitmapHeight, bool is32bpp)
    {
        Utils_ConvertBitmapToFloatArrayRGB(floatArrayPtr, bitmapScan0Ptr, bitmapStride, bitmapWidth, bitmapHeight, is32bpp);
    }

    __declspec(dllexport) void ConvertBitmapToFloatArrayYUVI(float* floatArrayPtr, unsigned char* bitmapScan0Ptr, int bitmapStride, int bitmapWidth, int bitmapHeight, bool is32bpp, unsigned char yuviStep)
    {
        YUVICfg yuviCfg = YUVICfg(yuviStep);
        Utils_ConvertBitmapToFloatArrayYUVI(floatArrayPtr, bitmapScan0Ptr, bitmapStride, bitmapWidth, bitmapHeight, is32bpp, &yuviCfg);
    }

    __declspec(dllexport) void ConvertBitmapToFloatArrayGrayscale(float* floatArrayPtr, unsigned char* bitmapScan0Ptr, int bitmapStride, int bitmapWidth, int bitmapHeight, bool is32bpp)
    {
        Utils_ConvertBitmapToFloatArrayGrayscale(floatArrayPtr, bitmapScan0Ptr, bitmapStride, bitmapWidth, bitmapHeight, is32bpp);
    }



    __declspec(dllexport) void ConvertFloatArrayRGBToBitmap(float* floatArrayPtr, unsigned char* bitmapScan0Ptr, int bitmapStride, int bitmapWidth, int bitmapHeight, bool is32bpp)
    {
        Utils_ConvertFloatArrayRGBToBitmap(floatArrayPtr, bitmapScan0Ptr, bitmapStride, bitmapWidth, bitmapHeight, is32bpp);
    }

    __declspec(dllexport) void ConvertFloatArrayYUVIToBitmap(float* floatArrayPtr, unsigned char* bitmapScan0Ptr, int bitmapStride, int bitmapWidth, int bitmapHeight, bool is32bpp, unsigned char yuviStep)
    {
        YUVICfg yuviCfg = YUVICfg(yuviStep);
        Utils_ConvertFloatArrayYUVIToBitmap(floatArrayPtr, bitmapScan0Ptr, bitmapStride, bitmapWidth, bitmapHeight, is32bpp, &yuviCfg);
    }

    __declspec(dllexport) void ConvertFloatArrayGrayscaleToBitmap(float* floatArrayPtr, unsigned char* bitmapScan0Ptr, int bitmapStride, int bitmapWidth, int bitmapHeight, bool is32bpp)
    {
        Utils_ConvertFloatArrayGrayscaleToBitmap(floatArrayPtr, bitmapScan0Ptr, bitmapStride, bitmapWidth, bitmapHeight, is32bpp);
    }
}