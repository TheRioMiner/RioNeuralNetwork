//RioNeuralNetwork: License information is available here - "https://github.com/TheRioMiner/RioNeuralNetwork/blob/master/LICENSE" or in file "LICENCE"

using System;
using System.Drawing;
using System.Drawing.Imaging;

namespace Rio_Neural_Network
{
	public static class Utils
	{
		public static unsafe float[] ConvertBitmap32BppToFloatArrayRGB(Bitmap bitmap)
		{
			//Check that bit per point is a 32bpp (yes its very genius solution, but very simple)
			if (!bitmap.PixelFormat.ToString().Contains("32bpp"))
				throw new ArgumentException("Bitmap not 32-bit per pixel!");

			float[] resultArray = new float[bitmap.Width * bitmap.Height * 3];
			fixed (float* resultArrayPtr = &resultArray[0])
			{
				//Lock the bitmap's bits
				var bitmapData = bitmap.LockBits(new Rectangle(0, 0, bitmap.Width, bitmap.Height), ImageLockMode.ReadOnly, bitmap.PixelFormat);
				{
					Native.ConvertBitmap32BppToFloatArrayRGB(resultArrayPtr, bitmapData.Scan0, bitmap.Width, bitmap.Height);
				}
				//Unlock bitmap
				bitmap.UnlockBits(bitmapData);
			}
			return resultArray;
		}

		public static float[] ConvertBitmap32BppToFloatArrayRGB(string fileName)
		{
			using (var bitmap = new Bitmap(fileName))
				return ConvertBitmap32BppToFloatArrayRGB(bitmap);
		}


		public static unsafe float[] ConvertBitmap32BppToFloatArrayGrayscale(Bitmap bitmap)
		{
			//Check that bit per point is a 32bpp (yes its very genius solution, but very simple)
			if (!bitmap.PixelFormat.ToString().Contains("32bpp"))
				throw new ArgumentException("Bitmap not 32-bit per pixel!");

			float[] resultArray = new float[bitmap.Width * bitmap.Height];
			fixed (float* resultArrayPtr = &resultArray[0])
			{
				//Lock the bitmap's bits
				var bitmapData = bitmap.LockBits(new Rectangle(0, 0, bitmap.Width, bitmap.Height), ImageLockMode.ReadOnly, bitmap.PixelFormat);
				{
					Native.ConvertBitmap32BppToFloatArrayGrayscale(resultArrayPtr, bitmapData.Scan0, bitmap.Width, bitmap.Height);
				}
				//Unlock bitmap
				bitmap.UnlockBits(bitmapData);
			}
			return resultArray;
		}

		public static unsafe float[] ConvertBitmap32BppToFloatArrayGrayscale(string fileName)
		{
			using (var bitmap = new Bitmap(fileName))
				return ConvertBitmap32BppToFloatArrayGrayscale(bitmap);
		}


		public static unsafe Bitmap ConvertFloatArrayRGBToBitmap32Bpp(float[] imageDataArray, int width, int height)
		{
			if ((width * height * 3) != imageDataArray.Length)
				throw new ArgumentOutOfRangeException("imageDataArray", "ImageDataArray lenght invalid!");

			var bitmap = new Bitmap(width, height, PixelFormat.Format32bppRgb);
			fixed (float* imageDataArrayPtr = &imageDataArray[0])
			{
				//Lock the bitmap's bits
				var bitmapData = bitmap.LockBits(new Rectangle(0, 0, bitmap.Width, bitmap.Height), ImageLockMode.WriteOnly, bitmap.PixelFormat);
				{
					Native.ConvertFloatArrayRGBToBitmap32Bpp(imageDataArrayPtr, bitmapData.Scan0, bitmap.Width, bitmap.Height);
				}
				//Unlock bitmap
				bitmap.UnlockBits(bitmapData);
			}
			return bitmap;
		}

		public static unsafe Bitmap ConvertFloatArrayGrayscaleToBitmap32Bpp(float[] imageDataArray, int width, int height)
		{
			if ((width * height) != imageDataArray.Length)
				throw new ArgumentOutOfRangeException("imageDataArray", "ImageDataArray lenght invalid!");

			var bitmap = new Bitmap(width, height, PixelFormat.Format32bppRgb);
			fixed (float* imageDataArrayPtr = &imageDataArray[0])
			{
				//Lock the bitmap's bits
				var bitmapData = bitmap.LockBits(new Rectangle(0, 0, bitmap.Width, bitmap.Height), ImageLockMode.WriteOnly, bitmap.PixelFormat);
				{
					Native.ConvertFloatArrayGrayscaleToBitmap32Bpp(imageDataArrayPtr, bitmapData.Scan0, bitmap.Width, bitmap.Height);
				}
				//Unlock bitmap
				bitmap.UnlockBits(bitmapData);
			}
			return bitmap;
		}
	}
}
