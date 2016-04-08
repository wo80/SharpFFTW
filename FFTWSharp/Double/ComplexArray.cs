using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Runtime.InteropServices;
using System.Text;

namespace FFTWSharp.Double
{
    /// <summary>
    /// So FFTW can manage its own memory nicely
    /// </summary>
    public class ComplexArray
    {
        private IntPtr handle;
        public IntPtr Handle
        { get { return handle; } }

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
            this.handle = fftw.malloc(this.length * 16);
        }

        /// <summary>
        /// Creates an FFTW-compatible array from array of doubles
        /// </summary>
        /// <param name="data">Array of doubles, alternating real and imaginary</param>
        public ComplexArray(double[] data)
        {
            this.length = data.Length / 2;
            this.handle = fftw.malloc(this.length * 16);

            this.SetData(data);
        }

        /// <summary>
        /// Creates an FFTW-compatible array from array of Complex numbers
        /// </summary>
        /// <param name="data">Array of Complex numbers</param>
        public ComplexArray(Complex[] data)
        {
            this.length = data.Length;
            this.handle = fftw.malloc(this.length * 16);

            this.SetData(data);
        }

        /// <summary>
        /// Set the data to an array of complex numbers
        /// </summary>
        public void SetData(double[] data)
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

            double[] data_in = new double[data.Length * 2];
            for (int i = 0; i < data.Length; i++)
            {
                data_in[2 * i] = data[i].Real;
                data_in[2 * i + 1] = data[i].Imaginary;
            }

            Marshal.Copy(data_in, 0, handle, this.length * 2);
        }

        /// <summary>
        /// Set the data to zeros.
        /// </summary>
        public void SetZeroData()
        {
            double[] data_in = new double[this.Length * 2];
            // C# arrays always initialized to 0
            Marshal.Copy(data_in, 0, handle, this.length * 2);
        }

        /// <summary>
        /// Get the data out
        /// </summary>
        /// <returns></returns>
        public Complex[] GetData_Complex()
        {
            double[] datad = new double[length * 2];
            Marshal.Copy(handle, datad, 0, length * 2);
            Complex[] data = new Complex[length];

            for (int i = 0; i < length; i++)
            {
                data[i] = new Complex(datad[2 * i], datad[2 * i + 1]);
            }

            return data;
        }

        public double[] GetData_Real()
        {
            double[] datad = new double[length * 2];
            Marshal.Copy(handle, datad, 0, length * 2);
            double[] data = new double[length];

            for (int i = 0; i < length; i++)
            {
                data[i] = datad[2 * i];
            }

            return data;
        }

        /// <summary>
        /// Get the data out
        /// </summary>
        /// <returns></returns>
        public double[] GetData_double()
        {
            double[] datad = new double[length * 2];
            Marshal.Copy(handle, datad, 0, length * 2);

            return datad;
        }

        ~ComplexArray()
        {
            fftw.free(handle);
        }
    }
}
