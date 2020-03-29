//RioNeuralNetwork: License information is available here - "https://github.com/TheRioMiner/RioNeuralNetwork/blob/master/LICENSE" or in file "LICENCE"

#pragma once
#include <iostream>
#include <immintrin.h>
#include "YUVICfg.h"


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


//AVX2 accelerated fast fill float array with desired value
static void Utils_AVX2_FloatArrayFill(float* arrayPtr, int size, float value)
{
    //Main cycle
    int i = 0;
    int mainCount = (size & -8);
    __m256 value256 = _mm256_set1_ps(value);
    for (; i < mainCount; i += 8)
        _mm256_storeu_ps(arrayPtr + i, value256);

    //End tail cycle
    for (; i < size; i++)
        *(arrayPtr + i) = value;
}



static void Utils_ConvertBitmapToFloatArrayRGB(float* floatArrayPtr, void* bitmapScan0Ptr, int bitmapWidth, int bitmapHeight, bool is32bpp)
{
    int i = 0;
    for (int y = 0; y < bitmapHeight; y++)
    {
        int yIndex = (y * bitmapWidth);
        for (int x = 0; x < bitmapWidth; x++)
        {
            if (is32bpp)
            {
                PixelDataBGRA px = *((PixelDataBGRA*)bitmapScan0Ptr + x + yIndex);
                *(floatArrayPtr + i++) = (px.R / 255.0f);
                *(floatArrayPtr + i++) = (px.G / 255.0f);
                *(floatArrayPtr + i++) = (px.B / 255.0f);
            }
            else 
            {
                PixelDataBGR px = *((PixelDataBGR*)bitmapScan0Ptr + x + yIndex);
                *(floatArrayPtr + i++) = (px.R / 255.0f);
                *(floatArrayPtr + i++) = (px.G / 255.0f);
                *(floatArrayPtr + i++) = (px.B / 255.0f);
            }
        }
    }
}

static void Utils_ConvertBitmapToFloatArrayYUVI(float* floatArrayPtr, void* bitmapScan0Ptr, int bitmapWidth, int bitmapHeight, bool is32bpp, YUVICfg* yuviCfg)
{
    int i = 0;
    for (int y = 0; y < bitmapHeight; y++)
    {
        int yIndex = (y * bitmapWidth);
        for (int x = 0; x < bitmapWidth; x++)
        {
            if (is32bpp)
            {
                PixelDataBGRA px = *((PixelDataBGRA*)bitmapScan0Ptr + x + yIndex);
                int U = (int)((-0.148f * px.R - 0.291f * px.G + 0.439f * px.B + 128.0f) / yuviCfg->UVDiv);
                int V = (int)((0.439f * px.R - 0.368f * px.G - 0.071f * px.B + 128.0f) / yuviCfg->UVDiv);
                int index = (U + (V * yuviCfg->Step));
                *(floatArrayPtr + i++) = (0.257f * px.R + 0.504f * px.G + 0.098f * px.B + 16.0f) / 255.0f; //Y
                *(floatArrayPtr + i++) = (index / yuviCfg->IndexRangeDiv); //UVI
            }
            else
            {
                PixelDataBGR px = *((PixelDataBGR*)bitmapScan0Ptr + x + yIndex);
                int U = (int)((-0.148f * px.R - 0.291f * px.G + 0.439f * px.B + 128.0f) / yuviCfg->UVDiv);
                int V = (int)((0.439f * px.R - 0.368f * px.G - 0.071f * px.B + 128.0f) / yuviCfg->UVDiv);
                int index = (U + (V * yuviCfg->Step));
                *(floatArrayPtr + i++) = (0.257f * px.R + 0.504f * px.G + 0.098f * px.B + 16.0f) / 255.0f; //Y
                *(floatArrayPtr + i++) = (index / yuviCfg->IndexRangeDiv); //UVI
            }
        }
    }
}

static void Utils_ConvertBitmapToFloatArrayGrayscale(float* floatArrayPtr, void* bitmapScan0Ptr, int bitmapWidth, int bitmapHeight, bool is32bpp)
{
    int i = 0;
    for (int y = 0; y < bitmapHeight; y++)
    {
        int yIndex = (y * bitmapWidth);
        for (int x = 0; x < bitmapWidth; x++)
        {
            if (is32bpp)
            {
                PixelDataBGRA px = *((PixelDataBGRA*)bitmapScan0Ptr + x + yIndex);
                *(floatArrayPtr + i++) = ((px.R * 0.299f) + (px.G * 0.587f) + (px.B * 0.114f)) / 255.0f;
            }
            else
            {
                PixelDataBGR px = *((PixelDataBGR*)bitmapScan0Ptr + x + yIndex);
                *(floatArrayPtr + i++) = ((px.R * 0.299f) + (px.G * 0.587f) + (px.B * 0.114f)) / 255.0f;
            }
        }
    }
}



static void Utils_ConvertFloatArrayRGBToBitmap(float* floatArrayPtr, void* bitmapScan0Ptr, int bitmapWidth, int bitmapHeight, bool is32bpp)
{
    int i = 0;
    float val = 0.0f;
    for (int y = 0; y < bitmapHeight; y++)
    {
        int yIndex = (y * bitmapWidth);
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
                *((PixelDataBGRA*)bitmapScan0Ptr + x + yIndex) = px;
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
                *((PixelDataBGR*)bitmapScan0Ptr + x + yIndex) = px;
            }
        }
    }
}

static void Utils_ConvertFloatArrayYUVIToBitmap(float* floatArrayPtr, void* bitmapScan0Ptr, int bitmapWidth, int bitmapHeight, bool is32bpp, YUVICfg* yuviCfg)
{
    int i = 0;
    for (int y = 0; y < bitmapHeight; y++)
    {
        int yIndex = (y * bitmapWidth);
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
                *((PixelDataBGRA*)bitmapScan0Ptr + x + yIndex) = px;
            }
            else 
            {
                PixelDataBGR px;
                px.R = (unsigned char)R;
                px.G = (unsigned char)G;
                px.B = (unsigned char)B;
                *((PixelDataBGR*)bitmapScan0Ptr + x + yIndex) = px;
            }
        }
    }
}

static void Utils_ConvertFloatArrayGrayscaleToBitmap(float* floatArrayPtr, void* bitmapScan0Ptr, int bitmapWidth, int bitmapHeight, bool is32bpp)
{
    float val = 0.0f;
    for (int y = 0; y < bitmapHeight; y++)
    {
        int yIndex = (y * bitmapWidth);
        for (int x = 0; x < bitmapWidth; x++)
        {
            int i = (yIndex + x);

            val = floatArrayPtr[i];
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
                *((PixelDataBGRA*)bitmapScan0Ptr + i) = px;
            }
            else 
            { 
                PixelDataBGR px;
                px.R = result;
                px.G = result;
                px.B = result;
                *((PixelDataBGR*)bitmapScan0Ptr + i) = px;
            }
        }
    }
}