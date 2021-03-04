
namespace fftbench.Benchmark
{
    using Lomont;

    public class TestLomont : ITest
    {
        double[] copy;
        double[] data;

        LomontFFT fft = new LomontFFT();

        public string Name => ToString();

        public int Size { get; private set; }

        public bool Enabled { get; set; }

        public void Initialize(double[] data)
        {
            int length = Size = data.Length;

            this.data = new double[2 * length];
            this.copy = new double[2 * length];

            for (int i = 0; i < length; i++)
            {
                this.data[2 * i] = data[i];
                this.data[2 * i + 1] = 0.0;
            }
        }

        public void FFT(bool forward)
        {
            data.CopyTo(copy, 0);

            fft.FFT(copy, forward);
        }

        public double[] Spectrum(double[] input, bool scale)
        {
            var fft = new LomontFFT();

            var data = Helper.ToComplex(input);

            fft.FFT(data, true);

            var spectrum = Helper.ComputeSpectrum(data);

            fft.FFT(data, false);

            Helper.ToDouble(data, input);

            return spectrum;
        }

        public override string ToString()
        {
            return "Lomont";
        }
    }
}
