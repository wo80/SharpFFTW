
namespace fftbench.Benchmark
{
    using NWaves.Transforms;

    public class TestNWaves : ITest
    {
        float[] re, im;

        Fft fft;

        public string Name => ToString();

        public int Size { get; private set; }

        public bool Enabled { get; set; }

        public void Initialize(double[] data)
        {
            int length = Size = data.Length;

            fft = new Fft(length);

            re = new float[length];
            im = new float[length];

            for (int i = 0; i < length; i++)
            {
                re[i] = (float)data[i];
                im[i] = 0f;
            }
        }

        public void FFT(bool forward)
        {
            if (forward)
            {
                fft.Direct(re, im);
            }
            else
            {
                fft.Inverse(re, im);
            }
        }

        public double[] Spectrum(double[] input, bool scale)
        {
            int length = input.Length;

            var fft = new Fft(length);

            var re = new float[length];
            var im = new float[length];

            for (int i = 0; i < length; i++)
            {
                re[i] = (float)input[i];
                im[i] = 0f;
            }

            fft.Direct(re, im);

            var spectrum = Helper.ComputeSpectrum(length, re, im);

            fft.Inverse(re, im);

            for (int i = 0; i < length; i++)
            {
                input[i] = re[i];
            }

            return spectrum;
        }

        public override string ToString()
        {
            return "NWaves";
        }
    }
}
