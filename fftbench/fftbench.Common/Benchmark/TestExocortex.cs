
namespace fftbench.Benchmark
{
    using Exocortex.DSP;

    public class TestExocortex : BaseTest
    {
        public override void FFT(bool forward)
        {
            data.CopyTo(copy, 0);

            Fourier.FFT(copy, copy.Length, forward ?
                FourierDirection.Forward :
                FourierDirection.Backward);
        }

        public override double[] Spectrum(double[] input, bool scale)
        {
            var data = ToComplex(input);

            Fourier.FFT(data, data.Length, FourierDirection.Forward);

            var spectrum = ComputeSpectrum(data);

            Fourier.FFT(data, data.Length, FourierDirection.Backward);

            ToDouble(data, input);

            return spectrum;
        }

        public override string ToString()
        {
            return "Exocortex";
        }
    }
}
