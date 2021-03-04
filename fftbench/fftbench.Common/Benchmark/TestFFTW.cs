
namespace fftbench.Benchmark
{
    using SharpFFTW;
    using SharpFFTW.Double;

    public class TestFFTW : ITest
    {
        ComplexArray input;
        ComplexArray output;

        Plan plan;

        public string Name => ToString();

        public int Size { get; private set; }

        public bool Enabled { get; set; }

        public void Initialize(double[] data)
        {
            int length = Size = data.Length;

            var temp = new double[2 * length];

            for (int i = 0; i < length; i++)
            {
                temp[2 * i] = data[i];
                temp[2 * i + 1] = 0.0;
            }

            input = new ComplexArray(length);
            output = new ComplexArray(length);

            input.Set(temp);

            plan = Plan.Create1(length, input, output, Direction.Forward, Options.Estimate);
        }

        public void FFT(bool forward)
        {
            plan.Execute();
        }

        public double[] Spectrum(double[] input, bool scale)
        {
            int length = input.Length;

            var zinput = Helper.ToComplex(input);

            using (var data1 = new ComplexArray(zinput))
            using (var data2 = new ComplexArray(length))
            using (var plan1 = Plan.Create1(length, data1, data2, Direction.Forward, Options.Estimate))
            using (var plan2 = Plan.Create1(length, data2, data1, Direction.Backward, Options.Estimate))
            {
                plan1.Execute();

                var temp = data2.ToArray();
                var spectrum = Helper.ComputeSpectrum(temp);

                plan2.Execute();

                data1.CopyTo(input, true);

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
                var ptr = NativeMethods.fftw_malloc(8);
                NativeMethods.fftw_free(ptr);
                return true;
            }
            catch
            {
            }

            return false;
        }

        public override string ToString()
        {
            return "FFTW";
        }
    }
}
