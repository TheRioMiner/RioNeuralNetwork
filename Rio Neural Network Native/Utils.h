//RioNeuralNetwork: License information is available here - "https://github.com/TheRioMiner/RioNeuralNetwork/blob/master/LICENSE" or in file "LICENCE"

#pragma once
#include <iostream>
#include <immintrin.h>


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



static void Utils_ConvertBitmap32BppToFloatArrayRGB(float* floatArrayPtr, void* bitmapScan0Ptr, int bitmapWidth, int bitmapHeight)
{
    int i = 0;
    PixelDataBGRA px;
    for (int y = 0; y < bitmapHeight; y++)
    {
        int yIndex = (y * bitmapWidth);
        for (int x = 0; x < bitmapWidth; x++)
        {
            px = *((PixelDataBGRA*)bitmapScan0Ptr + x + yIndex);
            *(floatArrayPtr + i++) = (px.R / 255.0f);
            *(floatArrayPtr + i++) = (px.G / 255.0f);
            *(floatArrayPtr + i++) = (px.B / 255.0f);
        }
    }
}

static void Utils_ConvertBitmap24BppToFloatArrayRGB(float* floatArrayPtr, void* bitmapScan0Ptr, int bitmapWidth, int bitmapHeight)
{
    int i = 0;
    PixelDataBGR px;
    for (int y = 0; y < bitmapHeight; y++)
    {
        int yIndex = (y * bitmapWidth);
        for (int x = 0; x < bitmapWidth; x++)
        {
            px = *((PixelDataBGR*)bitmapScan0Ptr + x + yIndex);
            *(floatArrayPtr + i++) = (px.R / 255.0f);
            *(floatArrayPtr + i++) = (px.G / 255.0f);
            *(floatArrayPtr + i++) = (px.B / 255.0f);
        }
    }
}


static void Utils_ConvertBitmap32BppToFloatArrayGrayscale(float* floatArrayPtr, void* bitmapScan0Ptr, int bitmapWidth, int bitmapHeight)
{
    int i = 0;
    PixelDataBGRA px;
    for (int y = 0; y < bitmapHeight; y++)
    {
        int yIndex = (y * bitmapWidth);
        for (int x = 0; x < bitmapWidth; x++)
        {
            px = *((PixelDataBGRA*)bitmapScan0Ptr + x + yIndex);
            *(floatArrayPtr + i++) = ((px.R * 0.299f) + (px.G * 0.587f) + (px.B * 0.114f)) / 255.0f;
        }
    }
}

static void Utils_ConvertBitmap24BppToFloatArrayGrayscale(float* floatArrayPtr, void* bitmapScan0Ptr, int bitmapWidth, int bitmapHeight)
{
    int i = 0;
    PixelDataBGR px;
    for (int y = 0; y < bitmapHeight; y++)
    {
        int yIndex = (y * bitmapWidth);
        for (int x = 0; x < bitmapWidth; x++)
        {
            px = *((PixelDataBGR*)bitmapScan0Ptr + x + yIndex);
            *(floatArrayPtr + i++) = ((px.R * 0.299f) + (px.G * 0.587f) + (px.B * 0.114f)) / 255.0f;
        }
    }
}


static void Utils_ConvertFloatArrayRGBToBitmap32Bpp(float* floatArrayPtr, void* bitmapScan0Ptr, int bitmapWidth, int bitmapHeight)
{
    int i = 0;
    float val = 0.0f;
    PixelDataBGRA px;
    px.A = 255;
    for (int y = 0; y < bitmapHeight; y++)
    {
        int yIndex = (y * bitmapWidth);
        for (int x = 0; x < bitmapWidth; x++)
        {
            //RED
            {
                val = floatArrayPtr[i];
                if (val > 1.0f)
                    val = 1.0f;
                if (val < 0.0f)
                    val = 0.0f;
                px.R = (unsigned char)(val * 255.0f);
                i++;
            }

            //GREEN
            {
                val = floatArrayPtr[i];
                if (val > 1.0f)
                    val = 1.0f;
                if (val < 0.0f)
                    val = 0.0f;
                px.G = (unsigned char)(val * 255.0f);
                i++;
            }

            //BLUE
            {
                val = floatArrayPtr[i];
                if (val > 1.0f)
                    val = 1.0f;
                if (val < 0.0f)
                    val = 0.0f;
                px.B = (unsigned char)(val * 255.0f);
                i++;
            }

            //Set pixel
            *((PixelDataBGRA*)bitmapScan0Ptr + x + yIndex) = px;
        }
    }
}

static void Utils_ConvertFloatArrayRGBToBitmap24Bpp(float* floatArrayPtr, void* bitmapScan0Ptr, int bitmapWidth, int bitmapHeight)
{
    int i = 0;
    float val = 0.0f;
    PixelDataBGR px;
    for (int y = 0; y < bitmapHeight; y++)
    {
        int yIndex = (y * bitmapWidth);
        for (int x = 0; x < bitmapWidth; x++)
        {
            //RED
            {
                val = floatArrayPtr[i];
                if (val > 1.0f)
                    val = 1.0f;
                if (val < 0.0f)
                    val = 0.0f;
                px.R = (unsigned char)(val * 255.0f);
                i++;
            }

            //GREEN
            {
                val = floatArrayPtr[i];
                if (val > 1.0f)
                    val = 1.0f;
                if (val < 0.0f)
                    val = 0.0f;
                px.G = (unsigned char)(val * 255.0f);
                i++;
            }

            //BLUE
            {
                val = floatArrayPtr[i];
                if (val > 1.0f)
                    val = 1.0f;
                if (val < 0.0f)
                    val = 0.0f;
                px.B = (unsigned char)(val * 255.0f);
                i++;
            }

            //Set pixel
            *((PixelDataBGR*)bitmapScan0Ptr + x + yIndex) = px;
        }
    }
}


static void Utils_ConvertFloatArrayGrayscaleToBitmap32Bpp(float* floatArrayPtr, void* bitmapScan0Ptr, int bitmapWidth, int bitmapHeight)
{
    int i = 0;
    float val = 0.0f;
    PixelDataBGRA px;
    px.A = 255;
    for (int y = 0; y < bitmapHeight; y++)
    {
        int yIndex = (y * bitmapWidth);
        for (int x = 0; x < bitmapWidth; x++)
        {
            int i = (yIndex + x);

            val = floatArrayPtr[i];
            if (val > 1.0f)
                val = 1.0f;
            if (val < 0.0f)
                val = 0.0f;

            val *= 255.0f; 

            unsigned char result = (unsigned char)val;
            px.R = result;
            px.G = result;
            px.B = result;
            *((PixelDataBGRA*)bitmapScan0Ptr + i) = px;
        }
    }
}

static void Utils_ConvertFloatArrayGrayscaleToBitmap24Bpp(float* floatArrayPtr, void* bitmapScan0Ptr, int bitmapWidth, int bitmapHeight)
{
    int i = 0;
    float val = 0.0f;
    PixelDataBGR px;
    for (int y = 0; y < bitmapHeight; y++)
    {
        int yIndex = (y * bitmapWidth);
        for (int x = 0; x < bitmapWidth; x++)
        {
            int i = (yIndex + x);

            val = floatArrayPtr[i];
            if (val > 1.0f)
                val = 1.0f;
            if (val < 0.0f)
                val = 0.0f;

            val *= 255.0f;

            unsigned char result = (unsigned char)val;
            px.R = result;
            px.G = result;
            px.B = result;
            *((PixelDataBGR*)bitmapScan0Ptr + i) = px;
        }
    }
}