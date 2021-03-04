
namespace fftbench.Benchmark
{
    using MathNet.Numerics.IntegralTransforms;

    public class TestMathNet : BaseTest
    {
        public override void FFT(bool forward)
        {
            data.CopyTo(copy, 0);

            if (forward)
            {
                Fourier.Forward(copy, FourierOptions.Default);
            }
            else
            {
                Fourier.Inverse(copy, FourierOptions.Default);
            }
        }

        public override double[] Spectrum(double[] input, bool scale)
        {
            var data = ToComplex(input);

            Fourier.Forward(data, FourierOptions.Default);

            var spectrum = ComputeSpectrum(data);

            Fourier.Inverse(data, FourierOptions.Default);

            ToDouble(data, input);

            return spectrum;
        }

        public override string ToString()
        {
            return "Math.NET";
        }
    }
}
