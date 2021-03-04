
namespace fftbench.Benchmark
{
    using Accord.Math;
    using Accord.Math.Transforms;

    public class TestAccord : BaseTest
    {
        public override void FFT(bool forward)
        {
            data.CopyTo(copy, 0);

            FourierTransform2.FFT(copy, forward ?
                FourierTransform.Direction.Forward :
                FourierTransform.Direction.Backward);
        }

        public override double[] Spectrum(double[] input, bool scale)
        {
            var data = ToComplex(input);

            FourierTransform2.FFT(data, FourierTransform.Direction.Forward);

            var spectrum = ComputeSpectrum(data);

            FourierTransform2.FFT(data, FourierTransform.Direction.Backward);

            ToDouble(data, input);

            return spectrum;
        }

        public override string ToString()
        {
            return "Accord";
        }
    }
}
