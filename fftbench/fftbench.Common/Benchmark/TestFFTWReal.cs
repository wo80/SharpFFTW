
namespace fftbench.Benchmark
{
    using SharpFFTW;
    using SharpFFTW.Double;

    public class TestFFTWReal : ITest
    {
        double[] data;

        RealArray input;
        ComplexArray output;

        Plan plan;

        public bool Enabled { get; set; }

        public string Name => ToString();

        public int Size { get; private set; }

        public void Initialize(double[] data)
        {
            int length = Size = data.Length;

            this.data = (double[])data.Clone();

            input = new RealArray(data);
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

            using (var data1 = new RealArray(length))
            using (var data2 = new ComplexArray(length / 2 + 1))
            using (var plan1 = Plan.Create1(length, data1, data2, Options.Estimate))
            using (var plan2 = Plan.Create1(length, data2, data1, Options.Estimate))
            {
                data1.Set(input);

                plan1.Execute();

                var temp = data2.ToArray();
                var spectrum = Helper.ComputeSpectrum(temp);

                plan2.Execute();

                data1.CopyTo(input);

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

        public override string ToString()
        {
            return "FFTW (real)";
        }
    }
}
