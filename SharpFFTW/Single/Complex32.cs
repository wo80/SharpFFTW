
namespace SharpFFTW.Single
{
    using System;
    using System.Runtime.InteropServices;

    /// <summary>
    /// Struct representing a complex number using two floats.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct Complex32
    {
        /// <summary>
        /// Gets the real part of the complex number.
        /// </summary>
        public readonly float Real;

        /// <summary>
        /// Gets the imaginary part of the complex number.
        /// </summary>
        public readonly float Imaginary;

        /// <summary>
        /// Creates a new Complex32 value.
        /// </summary>
        /// <param name="real">The real part.</param>
        /// <param name="imaginary">The imaginary part.</param>
        public Complex32(float real, float imaginary)
        {
            this.Real = real;
            this.Imaginary = imaginary;
        }
    }
}
