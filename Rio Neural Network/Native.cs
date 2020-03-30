//RioNeuralNetwork: License information is available here - "https://github.com/TheRioMiner/RioNeuralNetwork/blob/master/LICENSE" or in file "LICENCE"

using System;
using System.Runtime.InteropServices;

namespace RioNeuralNetwork
{
    public static unsafe class Native
    {
        [DllImport("msvcrt.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr memcpy(void* dest, void* src, UIntPtr count);
        [DllImport("msvcrt.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int memcmp(byte[] b1, byte[] b2, UIntPtr count);
        [DllImport("msvcrt.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int memcmp(void* ptr1, void* ptr2, UIntPtr count);

        [DllImport("kernel32.dll")]
        private static extern IntPtr LoadLibrary(string lpLibFileName);
        [DllImport("kernel32.dll")]
        private static extern bool FreeLibrary(IntPtr hModule);
        [DllImport("kernel32.dll")]
        private static extern IntPtr GetProcAddress(IntPtr hModule, string lpProcName);


        private static IntPtr _loadedModuleHandle;
        private const string NativeDll64 = "RioNeuralNetworkNative64.dll";
        private const string NativeDll32 = "RioNeuralNetworkNative32.dll";



        private static T LoadDelegate<T>(string procName)
        {
            //If module not loaded
            if (_loadedModuleHandle == IntPtr.Zero)
            {
                //Get module name depends on platform bitness
                string moduleName = null;
                if (IntPtr.Size == 8)
                    moduleName = NativeDll64;
                else
                    moduleName = NativeDll32;

                //Load module
                _loadedModuleHandle = LoadLibrary(moduleName);

                //Module loaded successfully?
                if (_loadedModuleHandle == IntPtr.Zero)
                    throw new Exception($"Native module: \"{moduleName}\" - could not be loaded!");
            }

            //Load function pointer
            IntPtr procAddress = GetProcAddress(_loadedModuleHandle, procName);
            if (procAddress == IntPtr.Zero)
                throw new Exception($"Function: \"{procName}\" - could not be loaded!");

            //Convert native function pointer to managed delegate
            return (T)(object)Marshal.GetDelegateForFunctionPointer(procAddress, typeof(T));
        }



        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate IntPtr CreateInstanceDelegate(IntPtr layersCfgArrayPtr, int layersCfgArraySize);
        public static CreateInstanceDelegate CreateInstance = LoadDelegate<CreateInstanceDelegate>("CreateInstance");

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate void DeleteInstanceDelegate(IntPtr instancePtr);
        public static DeleteInstanceDelegate DeleteInstance = LoadDelegate<DeleteInstanceDelegate>("DeleteInstance");


        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate float* ForwardPropagate_PtrDelegate(IntPtr instance, float* inputArrayPtr, int inputArraySize, bool setInputToFirstLayerOutput);
        public static ForwardPropagate_PtrDelegate ForwardPropagate_Ptr = LoadDelegate<ForwardPropagate_PtrDelegate>("ForwardPropagate_Ptr");

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate bool ForwardPropagate_CpyDelegate(IntPtr instance, float* inputArrayPtr, int inputArraySize, float* outputArrayPtr, bool setInputToFirstLayerOutput);
        public static ForwardPropagate_CpyDelegate ForwardPropagate_Cpy = LoadDelegate<ForwardPropagate_CpyDelegate>("ForwardPropagate_Cpy");

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate bool BackwardPropagateErrorDelegate(IntPtr instance, float* expectedArrayPtr, int expectedArraySize);
        public static BackwardPropagateErrorDelegate BackwardPropagateError = LoadDelegate<BackwardPropagateErrorDelegate>("BackwardPropagateError");

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate bool UpdateWeightsDelegate(IntPtr instance, float* inputArrayPtr, float learRate, float alpha);
        public static UpdateWeightsDelegate UpdateWeights = LoadDelegate<UpdateWeightsDelegate>("UpdateWeights");



        //<---- Helpful ---->

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate void FloatArrayFillDelegate(float* floatArrayPtr, int floatArraySize, float value);
        public static FloatArrayFillDelegate FloatArrayFill = LoadDelegate<FloatArrayFillDelegate>("FloatArrayFill");


        //ConvertBitmapToFloatArray
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate void ConvertBitmapToFloatArrayRGBDelegate(float* floatArrayPtr, IntPtr bitmapScan0Ptr, int bitmapStride, int bitmapWidth, int bitmapHeight, bool is32bpp);
        public static ConvertBitmapToFloatArrayRGBDelegate ConvertBitmapToFloatArrayRGB = LoadDelegate<ConvertBitmapToFloatArrayRGBDelegate>("ConvertBitmapToFloatArrayRGB");

        //ConvertBitmapToFloatArrayYUVI
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate void ConvertBitmapToFloatArrayYUVIDelegate(float* floatArrayPtr, IntPtr bitmapScan0Ptr, int bitmapStride, int bitmapWidth, int bitmapHeight, bool is32bpp, byte yuviStep);
        public static ConvertBitmapToFloatArrayYUVIDelegate ConvertBitmapToFloatArrayYUVI = LoadDelegate<ConvertBitmapToFloatArrayYUVIDelegate>("ConvertBitmapToFloatArrayYUVI");

        //ConvertBitmapToFloatArrayGrayscale
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate void ConvertBitmapToFloatArrayGrayscaleDelegate(float* floatArrayPtr, IntPtr bitmapScan0Ptr, int bitmapStride, int bitmapWidth, int bitmapHeight, bool is32bpp);
        public static ConvertBitmapToFloatArrayGrayscaleDelegate ConvertBitmapToFloatArrayGrayscale = LoadDelegate<ConvertBitmapToFloatArrayGrayscaleDelegate>("ConvertBitmapToFloatArrayGrayscale");
       


        //ConvertFloatArrayRGBToBitmap
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate void ConvertFloatArrayRGBToBitmapDelegate(float* floatArrayPtr, IntPtr bitmapScan0Ptr, int bitmapStride, int bitmapWidth, int bitmapHeight, bool is32bpp);
        public static ConvertFloatArrayRGBToBitmapDelegate ConvertFloatArrayRGBToBitmap = LoadDelegate<ConvertFloatArrayRGBToBitmapDelegate>("ConvertFloatArrayRGBToBitmap");

        //ConvertFloatArrayYUVIToBitmap
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate void ConvertFloatArrayYUVIToBitmapDelegate(float* floatArrayPtr, IntPtr bitmapScan0Ptr, int bitmapStride, int bitmapWidth, int bitmapHeight, bool is32bpp, byte yuviStep);
        public static ConvertFloatArrayYUVIToBitmapDelegate ConvertFloatArrayYUVIToBitmap = LoadDelegate<ConvertFloatArrayYUVIToBitmapDelegate>("ConvertFloatArrayYUVIToBitmap");

        //ConvertFloatArrayGrayscaleToBitmap
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate void ConvertFloatArrayGrayscaleToBitmapDelegate(float* floatArrayPtr, IntPtr bitmapScan0Ptr, int bitmapStride, int bitmapWidth, int bitmapHeight, bool is32bpp);
        public static ConvertFloatArrayGrayscaleToBitmapDelegate ConvertFloatArrayGrayscaleToBitmap = LoadDelegate<ConvertFloatArrayGrayscaleToBitmapDelegate>("ConvertFloatArrayGrayscaleToBitmap");
    }
}
