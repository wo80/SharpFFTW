
namespace fftbench.Benchmark
{
    using Lomont;

    public class TestLomontReal : ITest
    {
        double[] data;
        double[] copy;

        LomontFFT fft = new LomontFFT();

        public string Name => ToString();

        public int Size { get; private set; }

        public bool Enabled { get; set; }

        public void Initialize(double[] data)
        {
            int length = Size = data.Length;

            this.copy = (double[])data.Clone();
            this.data = new double[length];
        }

        public void FFT(bool forward)
        {
            data.CopyTo(copy, 0);

            fft.RealFFT(copy, forward);
        }

        public double[] Spectrum(double[] input, bool scale)
        {
            var fft = new LomontFFT();

            fft.RealFFT(input, true);

            var spectrum = Helper.ComputeSpectrum(input);

            fft.RealFFT(input, false);

            return spectrum;
        }

        public override string ToString()
        {
            return "Lomont (real)";
        }
    }
}
