
namespace fftbench.Benchmark {
    using Cavern.Utilities;

    public class TestCavern : ITest {
        public string Name => ToString();

        public int Size => data.Length;

        public bool Enabled { get; set; }

        Complex[] data;

        FFTCache cache;

        public void Initialize(double[] data) {
            this.data = new Complex[data.Length];
            cache = new FFTCache(data.Length);
            for (int i = 0; i < data.Length; i++) {
                this.data[i].Real = (float)data[i];
            }
        }

        public void FFT(bool forward) {
            Complex[] workingSet = (Complex[])data.Clone();
            Measurements.InPlaceFFT(workingSet, cache);
        }

        public double[] Spectrum(double[] input, bool scale) {
            Complex[] workingSet = (Complex[])data.Clone();
            Measurements.InPlaceFFT(workingSet, cache);

            float[] spectrumSource = Measurements.GetSpectrum(workingSet);
            double[] spectrum = new double[spectrumSource.Length];
            Helper.CopyToDouble(spectrumSource, spectrum);

            Measurements.InPlaceIFFT(workingSet, cache);
            float[] ifftResult = Measurements.GetRealPart(workingSet);
            Helper.CopyToDouble(ifftResult, input);

            return spectrum;
        }

        public override string ToString() {
            return "Cavern";
        }
    }
}
