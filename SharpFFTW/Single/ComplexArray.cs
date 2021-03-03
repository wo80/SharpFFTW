
namespace SharpFFTW.Single
{
    using System;
    using System.Runtime.InteropServices;

    /// <summary>
    /// Native array of complex (2*4-byte) floating point numbers.
    /// </summary>
    /// <remarks>
    /// The native memory is managed by FFTW (malloc, free).
    /// </remarks>
    public class ComplexArray : AbstractArray<float>
    {
        private const int SIZE = 8; // sizeof(Complex32)

        /// <summary>
        /// Creates a new array of complex numbers.
        /// </summary>
        /// <param name="length">Logical length of the array.</param>
        public ComplexArray(int length)
            : base(length)
        {
            Handle = NativeMethods.fftwf_malloc(this.Length * SIZE);
        }

        /// <summary>
        /// Creates an FFTW-compatible array from array of floats.
        /// </summary>
        /// <param name="data">Array of floats, alternating real and imaginary.</param>
        public ComplexArray(float[] data)
            : this(data.Length / 2)
        {
            this.Set(data);
        }

        /// <summary>
        /// Creates an FFTW-compatible array from an array of complex numbers.
        /// </summary>
        /// <param name="data">Array of complex numbers.</param>
        public ComplexArray(Complex32[] data)
            : this(data.Length)
        {
            this.Set(data);
        }

        /// <inheritdoc />
        public override void Dispose(bool disposing)
        {
            if (!hasDisposed)
            {
                if (Handle != IntPtr.Zero)
                {
                    NativeMethods.fftwf_free(Handle);
                    Handle = IntPtr.Zero;
                }
            }

            hasDisposed = disposing;
        }

        /// <summary>
        /// Set the data to an array of complex numbers.
        /// </summary>
        /// <param name="source">Array of floats, alternating real and imaginary.</param>
        public override void Set(float[] source)
        {
            int size = 2 * Length;

            if (source.Length != size)
            {
                throw new ArgumentException("Array length mismatch.");
            }

            Marshal.Copy(source, 0, Handle, size);
        }

        /// <summary>
        /// Set the data to an array of complex numbers.
        /// </summary>
        /// <param name="source">Array of complex numbers.</param>
        public void Set(Complex32[] source)
        {
            if (source.Length != this.Length)
            {
                throw new ArgumentException("Array length mismatch.");
            }

            var temp = GetTemporaryData(2 * Length);

            for (int i = 0; i < source.Length; i++)
            {
                temp[2 * i] = source[i].Real;
                temp[2 * i + 1] = source[i].Imaginary;
            }

            Marshal.Copy(temp, 0, Handle, this.Length * 2);
        }

        /// <summary>
        /// Set the data to zeros.
        /// </summary>
        public override void Clear()
        {
            var temp = GetTemporaryData(2 * Length);

            Array.Clear(temp, 0, temp.Length);

            Marshal.Copy(temp, 0, Handle, this.Length * 2);
        }

        /// <summary>
        /// Copy data to array of complex number.
        /// </summary>
        /// <param name="target">Array of complex numbers.</param>
        public void CopyTo(Complex32[] target)
        {
            if (target.Length != this.Length)
            {
                throw new Exception();
            }

            var temp = GetTemporaryData(2 * Length);

            CopyTo(temp);

            for (int i = 0; i < Length; i++)
            {
                target[i] = new Complex32(temp[2 * i], temp[2 * i + 1]);
            }
        }

        /// <summary>
        /// Copy data to array of floats.
        /// </summary>
        /// <param name="target">Array of floats, alternating real and imaginary.</param>
        public override void CopyTo(float[] target)
        {
            int size = 2 * Length;

            if (target.Length != size)
            {
                throw new Exception();
            }

            Marshal.Copy(Handle, target, 0, size);
        }

        /// <summary>
        /// Copy data to array of floats.
        /// </summary>
        /// <param name="target">Array of floats, alternating real and imaginary.</param>
        /// <param name="real">If true, only real part is considered.</param>
        public void CopyTo(float[] target, bool real)
        {
            if (!real)
            {
                CopyTo(target);
                return;
            }

            var temp = GetTemporaryData(2 * Length);

            CopyTo(temp);

            for (int i = 0; i < Length; i++)
            {
                target[i] = temp[2 * i];
            }
        }

        /// <summary>
        /// Get data as floats.
        /// </summary>
        /// <returns>Array of floats, alternating real and imaginary.</returns>
        public override float[] ToArray()
        {
            int size = 2 * Length;

            float[] data = new float[size];

            Marshal.Copy(Handle, data, 0, size);

            return data;
        }
    }
}
