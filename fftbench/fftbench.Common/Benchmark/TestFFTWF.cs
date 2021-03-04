
namespace fftbench.Benchmark
{
    using SharpFFTW;
    using SharpFFTW.Single;

    public class TestFFTWF : ITest
    {
        float[] data;

        RealArray input;
        ComplexArray output;

        Plan plan;

        public bool Enabled { get; set; }

        public string Name => ToString();

        public int Size { get; private set; }

        public void Initialize(double[] data)
        {
            int length = Size = data.Length;

            this.data = new float[length];

            for (int i = 0; i < length; i++)
            {
                this.data[i] = (float)data[i];
            }

            input = new RealArray(this.data);
            output = new ComplexArray(length / 2 + 1);

            plan = Plan.Create1(length, input, output, Options.Estimate);
        }

        public void FFT(bool forward)
        {
            input.Set(data);

            plan.Execute();
        }

        public double[] Spectrum(double[] input, bool scale)
        {
            int length = input.Length;

            var sinput = Helper.ConvertToFloat(input);
            
            using (var data1 = new RealArray(sinput))
            using (var data2 = new ComplexArray(length / 2 + 1))
            using (var plan1 = Plan.Create1(length, data1, data2, Options.Estimate))
            using (var plan2 = Plan.Create1(length, data2, data1, Options.Estimate))
            {
                plan1.Execute();

                var temp = data2.ToArray();
                var spectrum = Helper.ComputeSpectrum(temp);

                plan2.Execute();

                data1.CopyTo(sinput);

                Helper.CopyToDouble(sinput, input);

                if (scale)
                {
                    for (int i = 0; i < length; i++)
                    {
                        input[i] /= length;
                    }
                }

                return spectrum;
            }
        }

        public static bool LibraryExists()
        {
            try
            {
                var ptr = NativeMethods.fftwf_malloc(8);
                NativeMethods.fftwf_free(ptr);
                return true;
            }
            catch
            {
            }

            return false;
        }

        public override string ToString()
        {
            return "FFTWF (real)";
        }
    }
}
