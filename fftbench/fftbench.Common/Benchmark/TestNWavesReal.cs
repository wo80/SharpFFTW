
namespace fftbench.Benchmark
{
    using NWaves.Transforms;

    public class TestNWavesReal : ITest
    {
        float[] data, re, im;

        RealFft fft;

        public string Name => ToString();

        public int Size { get; private set; }

        public bool Enabled { get; set; }

        public void Initialize(double[] data)
        {
            int length = Size = data.Length;

            fft = new RealFft(length);

            this.re = new float[length];
            this.im = new float[length];
            this.data = new float[length];

            for (int i = 0; i < length; i++)
            {
                this.data[i] = (float)data[i];
            }
        }

        public void FFT(bool forward)
        {
            if (forward)
            {
                fft.Direct(data, re, im);
            }
            else
            {
                fft.Inverse(re, im, data);
            }
        }

        public double[] Spectrum(double[] input, bool scale)
        {
            int length = input.Length;

            var data = Helper.ConvertToFloat(input);
            var re = new float[length];
            var im = new float[length];

            var fft = new RealFft(length);

            fft.Direct(data, re, im);

            var spectrum = Helper.ComputeSpectrum(length / 2, re, im);

            fft.Inverse(re, im, data);

            return spectrum;
        }

        public override string ToString()
        {
            return "NWaves (real)";
        }
    }
}
