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


static void Utils_ConvertBitmap32BppToFloatArrayRGB(float* floatArrayPtr, void* bitmapStridePtr, int bitmapWidth, int bitmapHeight)
{
    int i = 0;
    PixelDataBGRA px;
    for (int y = 0; y < bitmapHeight; y++)
    {
        int yIndex = (y * bitmapWidth);
        for (int x = 0; x < bitmapWidth; x++)
        {
            px = *((PixelDataBGRA*)bitmapStridePtr + x + yIndex);
            *(floatArrayPtr + i++) = (px.R / 255.0f);
            *(floatArrayPtr + i++) = (px.G / 255.0f);
            *(floatArrayPtr + i++) = (px.B / 255.0f);
        }
    }
}

static void Utils_ConvertBitmap32BppToFloatArrayGrayscale(float* floatArrayPtr, void* bitmapStridePtr, int bitmapWidth, int bitmapHeight)
{
    int i = 0;
    PixelDataBGRA px;
    for (int y = 0; y < bitmapHeight; y++)
    {
        int yIndex = (y * bitmapWidth);
        for (int x = 0; x < bitmapWidth; x++)
        {
            px = *((PixelDataBGRA*)bitmapStridePtr + x + yIndex);
            float val = ((px.R + px.G + px.B) / 765.0f);
            *(floatArrayPtr + i++) = val;
        }
    }
}


static void Utils_ConvertFloatArrayRGBToBitmap32Bpp(float* floatArrayPtr, void* bitmapStridePtr, int bitmapWidth, int bitmapHeight)
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
                val = floatArrayPtr[i] * 255.0f;

                //Limit
                if (val > 255.0f)
                    val = 255.0f;
                if (val < 0.0f)
                    val = 0.0f;

                px.R = (unsigned char)val; //Set

                i++; //Increment index in float array
            }

            //GREEN
            {
                val = floatArrayPtr[i] * 255.0f;

                //Limit
                if (val > 255.0f)
                    val = 255.0f;
                if (val < 0.0f)
                    val = 0.0f;

                px.G = (unsigned char)val; //Set

                i++; //Increment index in float array
            }

            //BLUE
            {
                val = floatArrayPtr[i] * 255.0f;

                //Limit
                if (val > 255.0f)
                    val = 255.0f;
                if (val < 0.0f)
                    val = 0.0f;

                px.B = (unsigned char)val; //Set

                i++; //Increment index in float array
            }

            //Set pixel
            *((PixelDataBGRA*)bitmapStridePtr + x + yIndex) = px;
        }
    }
}

static void Utils_ConvertFloatArrayGrayscaleToBitmap32Bpp(float* floatArrayPtr, void* bitmapStridePtr, int bitmapWidth, int bitmapHeight)
{
    int i = 0;
    PixelDataBGRA px;
    px.A = 255;
    for (int y = 0; y < bitmapHeight; y++)
    {
        int yIndex = (y * bitmapWidth);
        for (int x = 0; x < bitmapWidth; x++)
        {
            int i = (yIndex + x);

            float val = floatArrayPtr[i];

            //Limit
            if (val > 1.0f)
                val = 1.0f;
            if (val < 0.0f)
                val = 0.0f;

            //Normalize to byte range
            val *= 255.0f;

            //Write pixeldata
            unsigned char result = (unsigned char)val;
            px.R = result;
            px.G = result;
            px.B = result;
            *((PixelDataBGRA*)bitmapStridePtr + x + yIndex) = px;
        }
    }
}