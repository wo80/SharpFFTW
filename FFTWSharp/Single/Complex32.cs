
namespace FFTWSharp.Single
{
    using System;
    using System.Runtime.InteropServices;

    [StructLayout(LayoutKind.Sequential)]
    public struct Complex32
    {
        public readonly float Real;
        public readonly float Imaginary;

        public Complex32(float real, float imaginary)
        {
            this.Real = real;
            this.Imaginary = imaginary;
        }
    }
}
