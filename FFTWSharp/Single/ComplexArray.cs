using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Runtime.InteropServices;
using System.Text;

namespace FFTWSharp.Single
{
    /// <summary>
    /// To simplify FFTW memory management
    /// </summary>
    public abstract class ComplexArray
    {
        private IntPtr handle;
        public IntPtr Handle
        { get { return handle; } }

        // The logical length of the array (# of complex numbers, not elements)
        private int length;
        public int Length
        { get { return length; } }

        /// <summary>
        /// Creates a new array of complex numbers
        /// </summary>
        /// <param name="length">Logical length of the array</param>
        public ComplexArray(int length)
        {
            this.length = length;
            this.handle = fftwf.malloc(this.length * 8);
        }

        /// <summary>
        /// Creates an FFTW-compatible array from array of floats, initializes to single precision only
        /// </summary>
        /// <param name="data">Array of floats, alternating real and imaginary</param>
        public ComplexArray(float[] data)
        {
            this.length = data.Length / 2;
            this.handle = fftwf.malloc(this.length * 8);

            this.SetData(data);
        }

        /// <summary>
        /// Creates an FFTW-compatible array from array of Complex numbers
        /// </summary>
        /// <param name="data">Array of Complex numbers</param>
        public ComplexArray(Complex[] data)
        {
            this.length = data.Length;
            this.handle = fftwf.malloc(this.length * 16);

            this.SetData(data);
        }

        /// <summary>
        /// Set the data to an array of complex numbers
        /// </summary>
        public void SetData(float[] data)
        {
            if (data.Length / 2 != this.length)
                throw new ArgumentException("Array length mismatch!");

            Marshal.Copy(data, 0, handle, this.length * 2);
        }

        /// <summary>
        /// Set the data to an array of complex numbers
        /// </summary>
        public void SetData(Complex[] data)
        {
            if (data.Length != this.length)
                throw new ArgumentException("Array length mismatch!");

            float[] data_in = new float[data.Length * 2];
            for (int i = 0; i < data.Length; i++)
            {
                data_in[2 * i] = (float)data[i].Real;
                data_in[2 * i + 1] = (float)data[i].Imaginary;
            }

            Marshal.Copy(data_in, 0, handle, this.length * 2);
        }

        /// <summary>
        /// Set the data to zeros
        /// </summary>
        public void SetZeroData()
        {
            float[] data_in = new float[this.Length * 2];
            // C# arrays always initialized to 0
            Marshal.Copy(data_in, 0, handle, this.length * 2);
        }

        /// <summary>
        /// Get the data out as Complex numbers
        /// </summary>
        public Complex[] GetData_Complex()
        {
            float[] dataf = new float[length * 2];
            Marshal.Copy(handle, dataf, 0, length * 2);
            Complex[] data = new Complex[length];

            for (int i = 0; i < length; i++)
            {
                data[i] = new Complex(dataf[2 * i], dataf[2 * i + 1]);
            }

            return data;
        }

        /// <summary>
        /// Get the real elements out
        /// </summary>
        public float[] GetData_Real()
        {
            float[] dataf = new float[length * 2];
            Marshal.Copy(handle, dataf, 0, length * 2);
            float[] data = new float[length];

            for (int i = 0; i < length; i++)
            {
                data[i] = dataf[2 * i];
            }

            return data;
        }

        /// <summary>
        /// Get the full array of floats out (alternating real and imaginary)
        /// </summary>
        public float[] GetData_float()
        {
            float[] dataf = new float[length * 2];
            Marshal.Copy(handle, dataf, 0, length * 2);

            return dataf;
        }

        ~ComplexArray()
        {
            fftwf.free(handle);
        }
    }
}
