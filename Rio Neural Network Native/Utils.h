//RioNeuralNetwork: License information is available here - "https://github.com/TheRioMiner/RioNeuralNetwork/blob/master/LICENSE" or in file "LICENCE"

#pragma once
#include <iostream>
#include <random>
#include <immintrin.h>
#include <omp.h>
#include <intrin.h>
#include "YUVICfg.h"
#include "ThreadingMode.h"


#define NOINLINE __declspec(noinline)


#pragma pack(push, 1)

struct PixelDataBGRA
{
    unsigned char B;
    unsigned char G;
    unsigned char R;
    unsigned char A;
};

struct PixelDataBGR
{
    unsigned char B;
    unsigned char G;
    unsigned char R;
};

#pragma pack(pop)



NOINLINE static bool Utils_IsProcessorSupportAVX()
{
    int cpuInfo[4] = { 0 };
    __cpuid(cpuInfo, 0);
    if (cpuInfo[0])
    {
        __cpuid(cpuInfo, 1);
        if (cpuInfo[2] & (1 << 28))
        {
            //Supported!
            return true;
        }
    }

    //Not supported!
    return false;
}

_inline static int Utils_GetProcsNum()
{
    return omp_get_num_procs();
}

_inline static bool Utils_BeginThreading(ThreadingMode threadingMode)
{
    switch (threadingMode)
    {
        case ThreadingMode::SINGLETHREAD:
        case ThreadingMode::INVALID:
            return false;
        case ThreadingMode::MAX:
            omp_set_num_threads(omp_get_num_procs());
            return true;
        default:
            omp_set_num_threads((int)threadingMode);
            return true;
    }
}

NOINLINE static void Utils_FloatArrayFill(float* arrayPtr, float value, int size)
{
    //Convert float to int by mem casting
    int intVal = *(int*)(&value);

    //Use memset
    memset(arrayPtr, intVal, size * sizeof(float));
}

NOINLINE static void Utils_FloatArrayRandomAdd(float* floatArrayPtr, int floatArraySize, float randomCoeff, bool negative, bool limit, int seed, ThreadingMode threadingMode)
{
    if (Utils_BeginThreading(threadingMode))
    {
        //Multi-thread
        #pragma omp parallel
        {
            auto rand = std::mt19937(seed + omp_get_thread_num());
            auto urd = std::uniform_real_distribution<>(negative ? -1 : 0, 1);

            #pragma omp for
            for (int i = 0; i < floatArraySize; i++)
            {
                //Get value
                float val = floatArrayPtr[i];

                //Add noise to value
                val += ((float)(urd(rand)) * randomCoeff);

                //Limiting
                if (limit)
                {
                    //Upper limit
                    if (val > 1.0f)
                        val = 1.0f;

                    //Down limit
                    if (negative)
                    {
                        if (val < -1.0f)
                            val = -1.0f;
                    }
                    else
                    {
                        if (val < 0.0f)
                            val = 0.0f;
                    }
                }

                //Set back value
                floatArrayPtr[i] = val;
            }
        }
    }
    else
    {
        //Single-thread
        auto rand = std::mt19937(seed + omp_get_thread_num());
        auto urd = std::uniform_real_distribution<>(negative ? -1 : 0, 1);
        for (int i = 0; i < floatArraySize; i++)
        {
            //Get value
            float val = floatArrayPtr[i];

            //Add noise to value
            val += ((float)(urd(rand)) * randomCoeff);

            //Limiting
            if (limit)
            {
                //Upper limit
                if (val > 1.0f)
                    val = 1.0f;

                //Down limit
                if (negative)
                {
                    if (val < -1.0f)
                        val = -1.0f;
                }
                else
                {
                    if (val < 0.0f)
                        val = 0.0f;
                }
            }

            //Set back value
            floatArrayPtr[i] = val;
        }
    }
}

NOINLINE static void Utils_FloatArrayRandomFill(float* floatArrayPtr, int floatArraySize, float randomCoeff, bool negative, int seed, ThreadingMode threadingMode)
{
    if (Utils_BeginThreading(threadingMode))
    {
        //Multi-thread
        #pragma omp parallel
        {
            auto rand = std::mt19937(seed + omp_get_thread_num());
            auto urd = std::uniform_real_distribution<>(negative ? -1 : 0, 1);

            #pragma omp for
            for (int i = 0; i < floatArraySize; i++)
            {
                float randVal = ((float)(urd(rand)) * randomCoeff);
                floatArrayPtr[i] = randVal;
            }
        }
    }
    else
    {
        //Single-thread
        auto rand = std::mt19937(seed + omp_get_thread_num());
        auto urd = std::uniform_real_distribution<>(negative ? -1 : 0, 1);
        for (int i = 0; i < floatArraySize; i++)
        {
            float randVal = ((float)(urd(rand)) * randomCoeff);
            floatArrayPtr[i] = randVal;
        }
    }
}


NOINLINE static float Utils_MeanSquaredError(float* etalonPtr, float* predictedPtr, int size, ThreadingMode threadingMode = ThreadingMode::DEFAULT)
{
    float sum = 0.0f;

    //Main cycle
    int i = 0;
    int mainCount = (size & -8);
    __m256 sum256 = _mm256_setzero_ps();
    if (Utils_BeginThreading(threadingMode))
    {
        //Multi-thread
        #pragma omp parallel for 
        for (i = 0; i < mainCount; i += 8)
        {
            __m256 etal = _mm256_load_ps(etalonPtr + i);
            __m256 pred = _mm256_load_ps(predictedPtr + i);
            etal = _mm256_sub_ps(etal, pred); //Get diff and store in etal
            etal = _mm256_mul_ps(etal, etal); //Get square of diff
            sum256 = _mm256_add_ps(sum256, etal); //Add squared diff into sum
        }
    }
    else
    {
        //Single-thread
        for (i = 0; i < mainCount; i += 8)
        {
            __m256 etal = _mm256_load_ps(etalonPtr + i);
            __m256 pred = _mm256_load_ps(predictedPtr + i);
            etal = _mm256_sub_ps(etal, pred); //Get diff and store in etal
            etal = _mm256_mul_ps(etal, etal); //Get square of diff
            sum256 = _mm256_add_ps(sum256, etal); //Add squared diff into sum
        }
    }
    //Extract sum from main cycle
    if (i != 0) //We have sum?
    {
        __m128 lo = _mm256_extractf128_ps(sum256, 0);
        __m128 hi = _mm256_extractf128_ps(sum256, 1);
        lo = _mm_add_ps(lo, hi);
        __declspec(align(sizeof(__m128))) float tmp[4];
        _mm_store_ps(tmp, lo);
        sum += tmp[0] + tmp[1] + tmp[2] + tmp[3];
    }

    //End tail cycle
    for (; i < size; i++)
    {
        float diff = *(etalonPtr + i) - *(predictedPtr + i); //Get diff
        diff = (diff * diff); //Square diff
        sum += diff; //Add squared diff into sum
    }

    return (sum / size);
}


NOINLINE static void Utils_ConvertBitmapToFloatArrayRGB(float* floatArrayPtr, unsigned char* bitmapScan0Ptr, int bitmapStride, int bitmapWidth, int bitmapHeight, bool is32bpp)
{
    int i = 0;
    int rowSize = abs(bitmapStride);
    for (int y = 0; y < bitmapHeight; y++)
    {
        int yIndex = (y * rowSize);
        for (int x = 0; x < bitmapWidth; x++)
        {
            if (is32bpp)
            {
                #pragma warning (suppress : 26451)
                PixelDataBGRA px = *(PixelDataBGRA*)(bitmapScan0Ptr + ((x * 4) + yIndex));
                *(floatArrayPtr + i++) = (px.R / 255.0f);
                *(floatArrayPtr + i++) = (px.G / 255.0f);
                *(floatArrayPtr + i++) = (px.B / 255.0f);
            }
            else 
            {
                #pragma warning (suppress : 26451)
                PixelDataBGR px = *(PixelDataBGR*)(bitmapScan0Ptr + ((x * 3) + yIndex));
                *(floatArrayPtr + i++) = (px.R / 255.0f);
                *(floatArrayPtr + i++) = (px.G / 255.0f);
                *(floatArrayPtr + i++) = (px.B / 255.0f);
            }
        }
    }
}

NOINLINE static void Utils_ConvertBitmapToFloatArrayYUVI(float* floatArrayPtr, unsigned char* bitmapScan0Ptr, int bitmapStride, int bitmapWidth, int bitmapHeight, bool is32bpp, YUVICfg* yuviCfg)
{
    int i = 0;
    int rowSize = abs(bitmapStride);
    for (int y = 0; y < bitmapHeight; y++)
    {
        int yIndex = (y * rowSize);
        for (int x = 0; x < bitmapWidth; x++)
        {
            if (is32bpp)
            {
                #pragma warning (suppress : 26451)
                PixelDataBGRA px = *(PixelDataBGRA*)(bitmapScan0Ptr + ((x * 4) + yIndex));
                int U = (int)((-0.148f * px.R - 0.291f * px.G + 0.439f * px.B + 128.0f) / yuviCfg->UVDiv);
                int V = (int)((0.439f * px.R - 0.368f * px.G - 0.071f * px.B + 128.0f) / yuviCfg->UVDiv);
                int index = (U + (V * yuviCfg->Step));
                *(floatArrayPtr + i++) = (0.257f * px.R + 0.504f * px.G + 0.098f * px.B + 16.0f) / 255.0f; //Y
                *(floatArrayPtr + i++) = (index / yuviCfg->IndexRangeDiv); //UVI
            }
            else
            {
                #pragma warning (suppress : 26451)
                PixelDataBGR px = *(PixelDataBGR*)(bitmapScan0Ptr + ((x * 3) + yIndex));
                int U = (int)((-0.148f * px.R - 0.291f * px.G + 0.439f * px.B + 128.0f) / yuviCfg->UVDiv);
                int V = (int)((0.439f * px.R - 0.368f * px.G - 0.071f * px.B + 128.0f) / yuviCfg->UVDiv);
                int index = (U + (V * yuviCfg->Step));
                *(floatArrayPtr + i++) = (0.257f * px.R + 0.504f * px.G + 0.098f * px.B + 16.0f) / 255.0f; //Y
                *(floatArrayPtr + i++) = (index / yuviCfg->IndexRangeDiv); //UVI
            }
        }
    }
}

NOINLINE static void Utils_ConvertBitmapToFloatArrayGrayscale(float* floatArrayPtr, unsigned char* bitmapScan0Ptr, int bitmapStride, int bitmapWidth, int bitmapHeight, bool is32bpp)
{
    int i = 0;
    int rowSize = abs(bitmapStride);
    for (int y = 0; y < bitmapHeight; y++)
    {
        int yIndex = (y * rowSize);
        for (int x = 0; x < bitmapWidth; x++)
        {
            if (is32bpp)
            {
                #pragma warning (suppress : 26451)
                PixelDataBGRA px = *(PixelDataBGRA*)(bitmapScan0Ptr + ((x * 4) + yIndex));
                *(floatArrayPtr + i++) = ((px.R * 0.299f) + (px.G * 0.587f) + (px.B * 0.114f)) / 255.0f;
            }
            else
            {
                #pragma warning (suppress : 26451)
                PixelDataBGR px = *(PixelDataBGR*)(bitmapScan0Ptr + ((x * 3) + yIndex));
                *(floatArrayPtr + i++) = ((px.R * 0.299f) + (px.G * 0.587f) + (px.B * 0.114f)) / 255.0f;
            }
        }
    }
}



NOINLINE static void Utils_ConvertFloatArrayRGBToBitmap(float* floatArrayPtr, unsigned char* bitmapScan0Ptr, int bitmapStride, int bitmapWidth, int bitmapHeight, bool is32bpp)
{
    int i = 0;
    float val = 0.0f;
    int rowSize = abs(bitmapStride);
    for (int y = 0; y < bitmapHeight; y++)
    {
        int yIndex = (y * rowSize);
        for (int x = 0; x < bitmapWidth; x++)
        {
            if (is32bpp)
            {
                PixelDataBGRA px;
                px.A = 255;

                //RED
                val = floatArrayPtr[i++];
                if (val > 1.0f) val = 1.0f;
                if (val < 0.0f) val = 0.0f;
                px.R = (unsigned char)(val * 255.0f);

                //GREEN
                val = floatArrayPtr[i++];
                if (val > 1.0f) val = 1.0f;
                if (val < 0.0f) val = 0.0f;
                px.G = (unsigned char)(val * 255.0f);

                //BLUE
                val = floatArrayPtr[i++];
                if (val > 1.0f) val = 1.0f;
                if (val < 0.0f) val = 0.0f;
                px.B = (unsigned char)(val * 255.0f);

                //Set pixel
                #pragma warning (suppress : 26451)
                *(PixelDataBGRA*)(bitmapScan0Ptr + ((x * 4) + yIndex)) = px;
            }
            else 
            {
                PixelDataBGR px;

                //RED
                val = floatArrayPtr[i++];
                if (val > 1.0f) val = 1.0f;
                if (val < 0.0f) val = 0.0f;
                px.R = (unsigned char)(val * 255.0f);

                //GREEN
                val = floatArrayPtr[i++];
                if (val > 1.0f) val = 1.0f;
                if (val < 0.0f) val = 0.0f;
                px.G = (unsigned char)(val * 255.0f);

                //BLUE
                val = floatArrayPtr[i++];
                if (val > 1.0f) val = 1.0f;
                if (val < 0.0f) val = 0.0f;
                px.B = (unsigned char)(val * 255.0f);

                //Set pixel
                #pragma warning (suppress : 26451)
                *(PixelDataBGR*)(bitmapScan0Ptr + ((x * 3) + yIndex)) = px;
            }
        }
    }
}

NOINLINE static void Utils_ConvertFloatArrayYUVIToBitmap(float* floatArrayPtr, unsigned char* bitmapScan0Ptr, int bitmapStride, int bitmapWidth, int bitmapHeight, bool is32bpp, YUVICfg* yuviCfg)
{
    int i = 0;
    int rowSize = abs(bitmapStride);
    for (int y = 0; y < bitmapHeight; y++)
    {
        int yIndex = (y * rowSize);
        for (int x = 0; x < bitmapWidth; x++)
        {
            //Get Y and UVI
            float Y = (floatArrayPtr[i++] * 255.0f) - 16.0f;
            float UVI = floatArrayPtr[i++];

            //Get U and V from UVI
            int cIndex = (int)(UVI * yuviCfg->IndexRangeDiv);
            unsigned char bU = (unsigned char)((cIndex % yuviCfg->Step) * yuviCfg->UVDiv);
            unsigned char bV = (unsigned char)((cIndex / yuviCfg->Step) * yuviCfg->UVDiv);

            //Normalize U and V
            float U = (bU - 128.0f);
            float V = (bV - 128.0f);

            //RED
            float R = 1.164f * Y + 1.596f * V;
            if (R < 0.0f) R = 0.0f;
            if (R > 255.0f) R = 255.0f;

            //GREEN
            float G = 1.164f * Y - 0.392f * U - 0.813f * V;
            if (G < 0.0f) G = 0.0f;
            if (G > 255.0f) G = 255.0f;

            //BLUE
            float B = 1.164f * Y + 2.017f * U;
            if (B < 0.0f) B = 0.0f;
            if (B > 255.0f) B = 255.0f;

            if (is32bpp)
            {
                PixelDataBGRA px;
                px.A = 255;
                px.R = (unsigned char)R;
                px.G = (unsigned char)G;
                px.B = (unsigned char)B;
                #pragma warning (suppress : 26451)
                *(PixelDataBGRA*)(bitmapScan0Ptr + ((x * 4) + yIndex)) = px;
            }
            else 
            {
                PixelDataBGR px;
                px.R = (unsigned char)R;
                px.G = (unsigned char)G;
                px.B = (unsigned char)B;
                #pragma warning (suppress : 26451)
                *(PixelDataBGR*)(bitmapScan0Ptr + ((x * 3) + yIndex)) = px;
            }
        }
    }
}

NOINLINE static void Utils_ConvertFloatArrayGrayscaleToBitmap(float* floatArrayPtr, unsigned char* bitmapScan0Ptr, int bitmapStride, int bitmapWidth, int bitmapHeight, bool is32bpp)
{
    int i = 0;
    float val = 0.0f;
    int rowSize = abs(bitmapStride);
    for (int y = 0; y < bitmapHeight; y++)
    {
        int yIndex = (y * rowSize);
        for (int x = 0; x < bitmapWidth; x++)
        {
            val = floatArrayPtr[i++];
            if (val > 1.0f) val = 1.0f;
            if (val < 0.0f) val = 0.0f;

            val *= 255.0f; 

            unsigned char result = (unsigned char)val;
            if (is32bpp)
            {
                PixelDataBGRA px;
                px.A = 255;
                px.R = result;
                px.G = result;
                px.B = result;
                #pragma warning (suppress : 26451)
                *(PixelDataBGRA*)(bitmapScan0Ptr + (x * 4 + yIndex)) = px;
            }
            else 
            { 
                PixelDataBGR px;
                px.R = result;
                px.G = result;
                px.B = result;
                #pragma warning (suppress : 26451)
                *(PixelDataBGR*)(bitmapScan0Ptr + (x * 3 + yIndex)) = px;
            }
        }
    }
}