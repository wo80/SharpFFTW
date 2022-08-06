
namespace fftbench.Benchmark
{
    using MathNet.Numerics;
    using MathNet.Numerics.IntegralTransforms;

    public class TestMathNet : BaseTest
    {
        public bool UseMKL { get; set; }

        private bool ProviderConfigured { get; set; }

        private void ConfigureProvider()
        {
            if (!ProviderConfigured)
            {
                if (UseMKL)
                {
                    Control.UseNativeMKL();
                }
                else
                {
                    Control.UseManaged();
                }

                ProviderConfigured = true;
            }
        }

        public override void FFT(bool forward)
        {
            ConfigureProvider();

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
            ConfigureProvider();

            var data = ToComplex(input);

            Fourier.Forward(data, FourierOptions.Default);

            var spectrum = ComputeSpectrum(data);

            Fourier.Inverse(data, FourierOptions.Default);

            ToDouble(data, input);

            return spectrum;
        }

        public override string ToString()
        {
            return UseMKL ? "Math.NET+MKL" : "Math.NET";
        }
    }
}
