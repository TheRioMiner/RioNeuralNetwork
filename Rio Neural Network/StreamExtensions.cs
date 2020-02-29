//RioNeuralNetwork: License information is available here - "https://github.com/TheRioMiner/RioNeuralNetwork/blob/master/LICENSE" or in file "LICENCE"

using System;
using System.IO;
using System.Text;
using System.Linq;
using System.Runtime.InteropServices;

namespace Rio_Neural_Network
{
    internal static class StreamExtension
    {
        public static void Write<T>(this Stream str, long position, T value) where T : struct
        {
            str.Position = position;
            str.Write(value);
        }

        public static void Write<T>(this Stream str, long position, T value, int offset, int length) where T : struct
        {
            str.Position = position;
            str.Write(value, offset, length);
        }

        public static void Write<T>(this Stream str, T value) where T : struct
        {
            byte[] data = value.ToBytes();
            str.Write(data, 0, data.Length);
        }

        public static void Write<T>(this Stream str, T value, int offset, int length) where T : struct
        {
            str.Position += offset;
            byte[] data = value.ToBytes();
            byte[] wrData = new byte[length];
            Array.Copy(data, offset, wrData, 0, length);
            str.Write(wrData, 0, length);
        }


        public static T Read<T>(this Stream str, IntPtr position) where T : struct
        {
            str.Position = (long)position;
            return str.Read<T>();
        }

        public static T Read<T>(this Stream str, long position) where T : struct
        {
            str.Position = position;
            return str.Read<T>();
        }

        public static T Read<T>(this Stream str) where T : struct
        {
            var data = new byte[Marshal.SizeOf(typeof(T))];
            var cnt = str.Read(data, 0, data.Length);
            if (cnt != data.Length)
                throw new Exception($"Failed to read a \"{typeof(T).Name}\" from stream: Read {cnt} of {data.Length} bytes.");
            return data.ToT<T>();
        }

        public static T[] ReadArray<T>(this Stream str, long position, int count) where T : struct
        {
            str.Position = position;
            return str.ReadArray<T>(count);
        }

        public static T[] ReadArray<T>(this Stream str, int count) where T : struct
        {
            var sz = Marshal.SizeOf(typeof(T));
            var data = new byte[sz * count];
            var cnt = str.Read(data, 0, data.Length);
            if (cnt != data.Length)
                throw new Exception($"Failed to read a \"{typeof(T).Name}[{count}]\" from stream: Read {cnt} of {data.Length} bytes.");

            var res = new T[count];
            var dt = new byte[sz];
            for (var i = 0; i < count; i++)
            {
                Array.Copy(data, sz * i, dt, 0, sz);
                res[i] = dt.ToT<T>();
            }

            return res;
        }

        public static string ReadString(this Stream str, IntPtr position, int length, Encoding encoding)
        {
            str.Position = (long)position;
            return ReadString(str, length, encoding);
        }

        public static string ReadString(this Stream str, long position, int length, Encoding encoding)
        {
            str.Position = position;
            return ReadString(str, length, encoding);
        }

        public static string ReadString(this Stream str, int length, Encoding encoding)
        {
            var data = new byte[length];
            str.Read(data, 0, length);
            return encoding.GetString(data.TakeWhile(x => x != 0).ToArray());
        }


        private static unsafe byte[] ToBytes<T>(this T value) where T : struct
        {
            var data = new byte[Marshal.SizeOf(typeof(T))];
            fixed (byte* b = data)
                Marshal.StructureToPtr(value, (IntPtr)b, true);
            return data;
        }

        private static unsafe T ToT<T>(this byte[] data, T defVal = default(T)) where T : struct
        {
            var structure = defVal;
            fixed (byte* b = data)
                structure = (T)Marshal.PtrToStructure((IntPtr)b, typeof(T));
            return structure;
        }
    }
}
