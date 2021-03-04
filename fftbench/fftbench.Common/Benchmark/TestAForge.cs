
namespace fftbench.Benchmark
{
    using Accord.Math;

    public class TestAForge : BaseTest
    {
        public override void FFT(bool forward)
        {
            data.CopyTo(copy, 0);
            
            FourierTransform.FFT(copy, forward ?
                FourierTransform.Direction.Forward :
                FourierTransform.Direction.Backward);
        }

        public override double[] Spectrum(double[] input, bool scale)
        {
            var data = ToComplex(input);

            FourierTransform.FFT(data, FourierTransform.Direction.Forward);

            var spectrum = ComputeSpectrum(data);

            FourierTransform.FFT(data, FourierTransform.Direction.Backward);

            ToDouble(data, input);

            return spectrum;
        }

        public override string ToString()
        {
            return "AForge";
        }
    }
}
