//RioNeuralNetwork: License information is available here - "https://github.com/TheRioMiner/RioNeuralNetwork/blob/master/LICENSE" or in file "LICENCE"

using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.CompilerServices;

namespace RioNeuralNetwork
{
	public static class NeuralUtils
	{
		/// <summary>
		/// Determine if the AVX processor supports the instruction?
		/// (Does not determine OS support!)
		/// </summary>
		/// <returns></returns>
		public static bool IsProcessorSupportAVX()
        {
			return Native.IsProcessorSupportAVX();
        }

		/// <summary>
		/// Get maximum thread count that will be used in "ThreadingMode.Max"
		/// </summary>
		/// <returns></returns>
		public static int GetMaxThreadNum()
        {
			return Native.GetProcsNum();
        }

		/// <summary>
		/// Get half of maximum thread count
		/// </summary>
		/// <returns></returns>
		public static int GetHalfFromMaxThreadNum()
		{
			return (Native.GetProcsNum() / 2);
		}

		/// <summary>
		/// Copy data from source array to destination array
		/// (If not chosen strict size check, then size will be choosen by smallest array size)
		/// </summary>
		/// <param name="dest">Destination array</param>
		/// <param name="src">Source array</param>
		public static unsafe void FloatArrayCopy(float[] dest, float[] src, bool strictSizeCheck = true)
        {
			if (strictSizeCheck && dest.Length != src.Length)
				throw new ArgumentOutOfRangeException("Size of arrays is different!");

			int size = Math.Min(dest.Length, src.Length);
			fixed (float* destPtr = dest)
			fixed (float* srcPtr = src)
				Native.memcpy(destPtr, srcPtr, (UIntPtr)(size * 4));
        }

		/// <summary>
		/// Add noise to float array (USE OF "SEED" IS USELESS IF YOU USE MULTITHREADING MODE!!!)
		/// </summary>
		/// <param name="arrayPtr">Array pointer</param>
		/// <param name="arraySize">Array size</param>
		/// <param name="noiseCoeff">Coefficient of noise. For example: 0.1f with Negative=true will generate -0.1 to 0.1</param>
		/// <param name="negative">Generate negative side of float value.</param>
		/// <param name="limit">Limit value after adding into array?</param>
		/// <param name="seed">Seed for random, 0 for random seed. (USELESS IN MULTITHREADING MODE!!!)</param>
		/// <param name="threadingMode">Threading mode. Using multithreading not recommended for small arrays (~4k or smaller)</param>
		public static unsafe void FloatArrayRandomAdd(float* arrayPtr, int arraySize, float noiseCoeff, bool negative, bool limit, int seed = 0, ThreadingMode threadingMode = ThreadingMode.Default)
		{
			if (seed == 0)
				seed = Environment.TickCount;
			Native.FloatArrayRandomAdd(arrayPtr, arraySize, noiseCoeff, negative, limit, seed, threadingMode);
		}

		/// <summary>
		/// Add noise to float array (USE OF "SEED" IS USELESS IF YOU USE MULTITHREADING MODE!!!)
		/// </summary>
		/// <param name="array">Float array</param>
		/// <param name="noiseCoeff">Coefficient of noise. For example: 0.1f with Negative=true will generate -0.1 to 0.1</param>
		/// <param name="negative">Generate negative side of float value.</param>
		/// <param name="limit">Limit value after adding into array?</param>
		/// <param name="seed">Seed for random, 0 for random seed. (USELESS IN MULTITHREADING MODE!!!)</param>
		/// <param name="threadingMode">Threading mode. Using multithreading not recommended for small arrays (~4k or smaller)</param>
		public static unsafe void FloatArrayRandomAdd(float[] array, float noiseCoeff, bool negative, bool limit, int seed = 0, ThreadingMode threadingMode = ThreadingMode.Default)
        {
			if (seed == 0)
				seed = Environment.TickCount;
			fixed (float* arrayPtr = array)
				Native.FloatArrayRandomAdd(arrayPtr, array.Length, noiseCoeff, negative, limit, seed, threadingMode);
		}

		/// <summary>
		/// Fill randomly the float array (USE OF "SEED" IS USELESS IF YOU USE MULTITHREADING MODE!!!)
		/// </summary>
		/// <param name="arrayPtr">Array pointer</param>
		/// <param name="arraySize">Array size</param>
		/// <param name="noiseCoeff">Coefficient of noise. For example: 0.1f with Negative=true will generate -0.1 to 0.1</param>
		/// <param name="negative">Generate negative side of float value.</param>
		/// <param name="seed">Seed for random, 0 for random seed. (USELESS IN MULTITHREADING MODE!!!)</param>
		/// <param name="threadingMode">Threading mode. Using multithreading not recommended for small arrays (~4k or smaller)</param>
		public static unsafe void FloatArrayRandomFill(float* arrayPtr, int arraySize, float noiseCoeff, bool negative, int seed = 0, ThreadingMode threadingMode = ThreadingMode.Default)
        {
			if (seed == 0)
				seed = Environment.TickCount;
			Native.FloatArrayRandomFill(arrayPtr, arraySize, noiseCoeff, negative, seed, threadingMode);
		}

		/// <summary>
		/// Fill randomly the float array (USE OF "SEED" IS USELESS IF YOU USE MULTITHREADING MODE!!!)
		/// </summary>
		/// <param name="array">Array</param>
		/// <param name="noiseCoeff">Coefficient of noise. For example: 0.1f with Negative=true will generate -0.1 to 0.1</param>
		/// <param name="negative">Generate negative side of float value.</param>
		/// <param name="seed">Seed for random, 0 for random seed. (USELESS IN MULTITHREADING MODE!!!)</param>
		/// <param name="threadingMode">Threading mode. Using multithreading not recommended for small arrays (~4k or smaller)</param>
		public static unsafe void FloatArrayRandomAdd(float[] array, float noiseCoeff, bool negative, int seed = 0, ThreadingMode threadingMode = ThreadingMode.Default)
		{
			if (seed == 0)
				seed = Environment.TickCount;
			fixed (float* arrayPtr = array)
				Native.FloatArrayRandomFill(arrayPtr, array.Length, noiseCoeff, negative, seed, threadingMode);
		}

		public static unsafe float CalcMeanSquaredError(float* etalonPtr, float* predictedPtr, int size, ThreadingMode threadingMode = ThreadingMode.Default)
		{
			return Native.MeanSquaredError(etalonPtr, predictedPtr, size, threadingMode);
		}

		public static unsafe float CalcMeanSquaredError(float[] etalon, float* predictedPtr, ThreadingMode threadingMode = ThreadingMode.Default)
		{
			fixed (float* etalonPtr = etalon)
				return Native.MeanSquaredError(etalonPtr, predictedPtr, etalon.Length, threadingMode);
		}

		public static unsafe float CalcMeanSquaredError(float[] etalon, float[] predicted, ThreadingMode threadingMode = ThreadingMode.Default)
		{
			fixed (float* etalonPtr = etalon)
			fixed (float* predictedPtr = predicted)
			{
				int size = Math.Min(etalon.Length, predicted.Length); //Get size from smallest array
				return Native.MeanSquaredError(etalonPtr, predictedPtr, size, threadingMode);
			}
		}

		public static unsafe float CalcRootMeanSquaredError(float* etalonPtr, float* predictedPtr, int size, ThreadingMode threadingMode = ThreadingMode.Default)
		{
			return (float)Math.Sqrt(CalcMeanSquaredError(etalonPtr, predictedPtr, size, threadingMode));
		}

		public static unsafe float CalcRootMeanSquaredError(float[] etalon, float* predictedPtr, ThreadingMode threadingMode = ThreadingMode.Default)
		{
			return (float)Math.Sqrt(CalcMeanSquaredError(etalon, predictedPtr, threadingMode));
		}

		public static unsafe float CalcRootMeanSquaredError(float[] etalon, float[] predicted, ThreadingMode threadingMode = ThreadingMode.Default)
		{
			return (float)Math.Sqrt(CalcMeanSquaredError(etalon, predicted, threadingMode));
		}

		/// <summary>
		/// Returns sub neural network from full neural network (like substring but for neural network layers)
		/// Very helpful in getting encoder and decoder in autoencoder neural networks
		/// </summary>
		/// <param name="network">Original neural network</param>
		/// <param name="layerIndex">Starting layer for cutting</param>
		/// <param name="size">How much layers need</param>
		/// <returns>Cutted neural network</returns>
		public static unsafe NeuralNetwork GetSubNeuralNetwork(this NeuralNetwork network, int layerIndex, int size)
		{
			//Protection againts fools
			if (size < 2)
				throw new ArgumentOutOfRangeException("fromLayer", "Layers in new neural network need to be equal or greater that 2!");
			if (layerIndex < 0)
				throw new ArgumentOutOfRangeException("fromLayer", "\"fromLayer\" is negative!");
			if ((layerIndex + size) > network.LayersCount)
				throw new ArgumentOutOfRangeException("toLayer", "\"size\" is too big!");

			//Copy layers cfg
			var layersCfg = new LayerCfg[size];
			for (int newI = 0, origI = layerIndex; newI < layersCfg.Length; newI++, origI++)
				layersCfg[newI] = network[origI].ToLayerCfg();

			//And make new network from configuration depends on original network
			var newNetwork = new NeuralNetwork(layersCfg, true/*Fully copy cfg!*/);

			//Copy layers data
			for (int l = 0; l < size; l++)
			{
				//Get layers
				var newLayer = newNetwork[l];
				var origLayer = network[layerIndex + l];

				//Check that all ok
				if (newLayer.NeuronsWeightsSize != origLayer.NeuronsWeightsSize)
					throw new Exception("Layers neuron weights size mismatch!");
				if (newLayer.NeuronsCount != origLayer.NeuronsCount)
					throw new Exception("Layers neurons count mismatch!");

				//Copy weights data
				for (int i = 0; i < newLayer.NeuronsCount; i++)
				{
					Native.memcpy(newLayer.Weights[i], origLayer.Weights[i], (UIntPtr)(newLayer.NeuronsWeightsSize * sizeof(float)));
					Native.memcpy(newLayer.WeightsMomentum[i], origLayer.WeightsMomentum[i], (UIntPtr)(newLayer.NeuronsWeightsSize * sizeof(float)));
				}

				//Copy output and errors data
				Native.memcpy(newLayer.Outputs, origLayer.Outputs, (UIntPtr)(newLayer.NeuronsCount * sizeof(float)));
				Native.memcpy(newLayer.Errors, origLayer.Errors, (UIntPtr)(newLayer.NeuronsCount * sizeof(float)));
			}

			//Return new network!
			return newNetwork;
		}



		private static void CheckBitmapBitness(Bitmap bitmap, out bool is32bpp)
		{
			//Get bitness
			switch (bitmap.PixelFormat)
			{
				case PixelFormat.Format32bppArgb:
				case PixelFormat.Format32bppRgb:
					is32bpp = true;
					break;
				case PixelFormat.Format24bppRgb:
					is32bpp = false;
					break;

				default: //Invalid pixelformat
					throw new ArgumentException("Bitmap invalid pixelformat!\nIts should be 32bpp or 24bpp!");
			}
		}

		public static unsafe float[] ConvertBitmapToFloatArrayRGB(Bitmap bitmap)
		{
			//Check and get bitness
			CheckBitmapBitness(bitmap, out bool is32bpp);

			float[] resultArray = new float[bitmap.Width * bitmap.Height * 3];
			fixed (float* resultArrayPtr = &resultArray[0])
			{
				//Lock the bitmap's bits
				var bitmapData = bitmap.LockBits(new Rectangle(0, 0, bitmap.Width, bitmap.Height), ImageLockMode.ReadOnly, bitmap.PixelFormat);
				{
					Native.ConvertBitmapToFloatArrayRGB(resultArrayPtr, bitmapData.Scan0, bitmapData.Stride, bitmap.Width, bitmap.Height, is32bpp);
				}
				//Unlock bitmap
				bitmap.UnlockBits(bitmapData);
			}
			return resultArray;
		}

		public static float[] ConvertBitmapToFloatArrayRGB(string fileName)
		{
			using (var bitmap = new Bitmap(fileName))
				return ConvertBitmapToFloatArrayRGB(bitmap);
		}


		public static unsafe float[] ConvertBitmapToFloatArrayYUVI(Bitmap bitmap, byte yuviStep = 80)
		{
			//Check and get bitness
			CheckBitmapBitness(bitmap, out bool is32bpp);

			float[] resultArray = new float[bitmap.Width * bitmap.Height * 2];
			fixed (float* resultArrayPtr = &resultArray[0])
			{
				//Lock the bitmap's bits
				var bitmapData = bitmap.LockBits(new Rectangle(0, 0, bitmap.Width, bitmap.Height), ImageLockMode.ReadOnly, bitmap.PixelFormat);
				{
					Native.ConvertBitmapToFloatArrayYUVI(resultArrayPtr, bitmapData.Scan0, bitmapData.Stride, bitmap.Width, bitmap.Height, is32bpp, yuviStep);
				}
				//Unlock bitmap
				bitmap.UnlockBits(bitmapData);
			}
			return resultArray;
		}

		public static float[] ConvertBitmapToFloatArrayYUVI(string fileName, byte yuviStep = 80)
		{
			using (var bitmap = new Bitmap(fileName))
				return ConvertBitmapToFloatArrayYUVI(bitmap, yuviStep);
		}


		public static unsafe float[] ConvertBitmapToFloatArrayGrayscale(Bitmap bitmap)
		{
			//Check and get bitness
			CheckBitmapBitness(bitmap, out bool is32bpp);

			float[] resultArray = new float[bitmap.Width * bitmap.Height];
			fixed (float* resultArrayPtr = &resultArray[0])
			{
				//Lock the bitmap's bits
				var bitmapData = bitmap.LockBits(new Rectangle(0, 0, bitmap.Width, bitmap.Height), ImageLockMode.ReadOnly, bitmap.PixelFormat);
				{
					Native.ConvertBitmapToFloatArrayGrayscale(resultArrayPtr, bitmapData.Scan0, bitmapData.Stride, bitmap.Width, bitmap.Height, is32bpp);
				}
				//Unlock bitmap
				bitmap.UnlockBits(bitmapData);
			}
			return resultArray;
		}

		public static float[] ConvertBitmapToFloatArrayGrayscale(string fileName)
		{
			using (var bitmap = new Bitmap(fileName))
				return ConvertBitmapToFloatArrayGrayscale(bitmap);
		}



		public static unsafe Bitmap ConvertFloatArrayRGBToBitmap(float[] imageDataArray, int width, int height, bool bitmap32bpp = false)
		{
			if ((width * height * 3) != imageDataArray.Length)
				throw new ArgumentOutOfRangeException("imageDataArray", "ImageDataArray lenght invalid!");

			var bitmap = new Bitmap(width, height, bitmap32bpp ? PixelFormat.Format32bppRgb : PixelFormat.Format24bppRgb);
			fixed (float* imageDataArrayPtr = &imageDataArray[0])
			{
				//Lock the bitmap's bits
				var bitmapData = bitmap.LockBits(new Rectangle(0, 0, bitmap.Width, bitmap.Height), ImageLockMode.WriteOnly, bitmap.PixelFormat);
				{
					Native.ConvertFloatArrayRGBToBitmap(imageDataArrayPtr, bitmapData.Scan0, bitmapData.Stride, bitmap.Width, bitmap.Height, bitmap32bpp);
				}
				//Unlock bitmap
				bitmap.UnlockBits(bitmapData);
			}
			return bitmap;
		}

		public static unsafe Bitmap ConvertFloatArrayYUVIToBitmap(float[] imageDataArray, int width, int height, byte yuviStep = 80, bool bitmap32bpp = false)
		{
			if ((width * height * 2) != imageDataArray.Length)
				throw new ArgumentOutOfRangeException("imageDataArray", "ImageDataArray lenght invalid!");

			var bitmap = new Bitmap(width, height, bitmap32bpp ? PixelFormat.Format32bppRgb : PixelFormat.Format24bppRgb);
			fixed (float* imageDataArrayPtr = &imageDataArray[0])
			{
				//Lock the bitmap's bits
				var bitmapData = bitmap.LockBits(new Rectangle(0, 0, bitmap.Width, bitmap.Height), ImageLockMode.WriteOnly, bitmap.PixelFormat);
				{
					Native.ConvertFloatArrayYUVIToBitmap(imageDataArrayPtr, bitmapData.Scan0, bitmapData.Stride, bitmap.Width, bitmap.Height, bitmap32bpp, yuviStep);
				}
				//Unlock bitmap
				bitmap.UnlockBits(bitmapData);
			}
			return bitmap;
		}

		public static unsafe Bitmap ConvertFloatArrayGrayscaleToBitmap(float[] imageDataArray, int width, int height, bool bitmap32bpp = false)
		{
			if ((width * height) != imageDataArray.Length)
				throw new ArgumentOutOfRangeException("imageDataArray", "ImageDataArray lenght invalid!");

			var bitmap = new Bitmap(width, height, bitmap32bpp ? PixelFormat.Format32bppRgb : PixelFormat.Format24bppRgb);
			fixed (float* imageDataArrayPtr = &imageDataArray[0])
			{
				//Lock the bitmap's bits
				var bitmapData = bitmap.LockBits(new Rectangle(0, 0, bitmap.Width, bitmap.Height), ImageLockMode.WriteOnly, bitmap.PixelFormat);
				{
					Native.ConvertFloatArrayGrayscaleToBitmap(imageDataArrayPtr, bitmapData.Scan0, bitmapData.Stride, bitmap.Width, bitmap.Height, bitmap32bpp);
				}
				//Unlock bitmap
				bitmap.UnlockBits(bitmapData);
			}
			return bitmap;
		}
	}
}
