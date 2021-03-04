using System;

namespace fftbench.Benchmark
{
    static class Helper
    {
        public static float[] ConvertToFloat(double[] input)
        {
            int length = input.Length;

            var result = new float[length];

            for (int i = 0; i < length; i++)
            {
                result[i] = (float)input[i];
            }

            return result;
        }

        public static void CopyToDouble(float[] input, double[] traget)
        {
            int length = input.Length;

            for (int i = 0; i < length; i++)
            {
                traget[i] = input[i];
            }
        }

        #region Single precision

        public static double[] ComputeSpectrum(float[] fft)
        {
            int length = fft.Length / 2;

            var result = new double[length];

            for (int i = 0; i < length; i++)
            {
                float x = fft[2 * i];
                float y = fft[2 * i + 1];

                result[i] = Math.Sqrt(x * x + y * y);
            }

            return result;
        }

        public static double[] ComputeSpectrum(int length, float[] re, float[] im)
        {
            var result = new double[length];

            for (int i = 0; i < length; i++)
            {
                double x = re[i];
                double y = im[i];

                result[i] = Math.Sqrt(x * x + y * y);
            }

            return result;
        }

        public static float[] ToComplex(float[] data)
        {
            int length = data.Length;

            var result = new float[2 * length];

            for (int i = 0; i < length; i++)
            {
                result[2 * i] = data[i];
            }

            return result;
        }

        public static void ToDouble(float[] data, float[] target)
        {
            int length = data.Length / 2;

            for (int i = 0; i < length; i++)
            {
                target[i] = data[2 * i];
            }
        }

        #endregion

        #region Double precision

        public static double[] ComputeSpectrum(double[] fft)
        {
            int length = fft.Length / 2;

            var result = new double[length];

            for (int i = 0; i < length; i++)
            {
                double x = fft[2 * i];
                double y = fft[2 * i + 1];

                result[i] = Math.Sqrt(x * x + y * y);
            }

            return result;
        }

        public static double[] ToComplex(double[] data)
        {
            int length = data.Length;

            var result = new double[2 * length];

            for (int i = 0; i < length; i++)
            {
                result[2 * i] = data[i];
            }

            return result;
        }

        public static void ToDouble(double[] data, double[] target)
        {
            int length = data.Length / 2;

            for (int i = 0; i < length; i++)
            {
                target[i] = data[2 * i];
            }
        }

        #endregion
    }
}
