
namespace FFTWSharp.Single
{
    using System;
    using System.Runtime.InteropServices;

    /// <summary>
    /// Double array pointing to the native FFTW memory.
    /// </summary>
    public class RealArray : AbstractArray<float>
    {
        private const int SIZE = 4; // sizeof(float)

        /// <summary>
        /// Creates a new array of floats.
        /// </summary>
        /// <param name="length">Logical length of the array.</param>
        public RealArray(int length)
            : base(length)
        {
            Handle = NativeMethods.malloc(this.Length * SIZE);
        }

        /// <summary>
        /// Creates an FFTW-compatible array from array of floats.
        /// </summary>
        /// <param name="data">Array of floats, alternating real and imaginary.</param>
        public RealArray(float[] data)
            : this(data.Length)
        {
            this.Set(data);
        }

        public override void Dispose(bool disposing)
        {
            if (!hasDisposed)
            {
                if (Handle != IntPtr.Zero)
                {
                    NativeMethods.free(Handle);
                    Handle = IntPtr.Zero;
                }
            }

            hasDisposed = disposing;
        }

        /// <summary>
        /// Set the data to an array of complex numbers.
        /// </summary>
        /// <param name="source">Array of floats, alternating real and imaginary.</param>
        public void Set(float[] source)
        {
            int size = Length;

            if (source.Length != size)
            {
                throw new ArgumentException("Array length mismatch.");
            }

            Marshal.Copy(source, 0, Handle, size);
        }

        /// <summary>
        /// Set the data to zeros.
        /// </summary>
        public void Clear()
        {
            var temp = GetTemporaryData(Length);

            Array.Clear(temp, 0, temp.Length);

            Marshal.Copy(temp, 0, Handle, this.Length);
        }

        /// <summary>
        /// Copy data to array of floats.
        /// </summary>
        /// <param name="target">Array of floats, alternating real and imaginary.</param>
        public void CopyTo(float[] target)
        {
            int size = Length;

            if (target.Length != size)
            {
                throw new Exception();
            }

            Marshal.Copy(Handle, target, 0, size);
        }

        /// <summary>
        /// Get data as floats.
        /// </summary>
        /// <returns>Array of floats, alternating real and imaginary.</returns>
        public float[] ToArray()
        {
            int size = Length;

            float[] data = new float[size];

            Marshal.Copy(Handle, data, 0, size);

            return data;
        }
    }
}
