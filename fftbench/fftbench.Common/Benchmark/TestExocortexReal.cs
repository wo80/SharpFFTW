
namespace fftbench.Benchmark
{
    using Exocortex.DSP;
    using System.Numerics;

    public class TestExocortexReal : ITest
    {
        float[] data;
        float[] copy;

        public string Name => ToString();

        public int Size { get; private set; }

        public bool Enabled { get; set; }

        public void Initialize(double[] data)
        {
            int length = Size = data.Length;

            this.copy = new float[length];
            this.data = new float[length];

            for (int i = 0; i < length; i++)
            {
                this.data[i] = (float)data[i];
            }
        }

        public void FFT(bool forward)
        {
            data.CopyTo(copy, 0);

            Fourier.RFFT(copy, copy.Length, forward ?
                FourierDirection.Forward :
                FourierDirection.Backward);
        }

        public double[] Spectrum(double[] input, bool scale)
        {
            var data = ToComplex(input);

            Fourier.FFT(data, data.Length, FourierDirection.Forward);

            var spectrum = ComputeSpectrum(data);

            Fourier.FFT(data, data.Length, FourierDirection.Backward);

            ToDouble(data, input);

            return spectrum;
        }

        private double[] ComputeSpectrum(Complex[] fft)
        {
            int length = fft.Length / 2;

            var result = new double[length];

            for (int i = 0; i < length; i++)
            {
                result[i] = fft[i].Magnitude;
            }

            return result;
        }

        private Complex[] ToComplex(double[] data)
        {
            int length = data.Length;

            var result = new Complex[length];

            for (int i = 0; i < length; i++)
            {
                result[i] = new Complex(data[i], 0.0);
            }

            return result;
        }

        private void ToDouble(Complex[] data, double[] target)
        {
            int length = data.Length;

            for (int i = 0; i < length; i++)
            {
                target[i] = data[i].Real;
            }
        }

        public override string ToString()
        {
            return "Exocortex (real)";
        }
    }
}
