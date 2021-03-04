
namespace fftbench
{
    using MathNet.Numerics;

    public class SignalGenerator
    {
        public static double[] Sine(int size = 1024)
        {
            const int N = 64; // sampling rate

            return Generate.Sinusoidal(size, N, 1.0, 20.0);
        }

        public static double[] Sine2(int size = 1024)
        {
            const int N = 64; // sampling rate

            var a = Generate.Sinusoidal(size, N, 1.0, 20.0);
            var b = Generate.Sinusoidal(size, N, 2.0, 10.0);

            for (int i = 0; i < size; i++)
            {
                a[i] += b[i];
            }

            return a;
        }

        public static double[] Sine3(int size = 1024)
        {
            const int N = 64; // sampling rate

            var a = Generate.Sinusoidal(size, N, 1.0, 20.0);
            var b = Generate.Sinusoidal(size, N, 2.0, 10.0);
            var c = Generate.Sinusoidal(size, N, 4.0,  5.0);

            for (int i = 0; i < size; i++)
            {
                a[i] += (b[i] + c[i]);
            }

            return a;
        }

        public static double[] Square(int size = 1024)
        {
            const int N = 32;

            return Generate.Square(size, N, N, -20.0, 20.0);
        }

        public static double[] Sawtooth(int size = 1024)
        {
            const int N = 32;

            return Generate.Sawtooth(size, N, -20.0, 20.0);
        }

        public static double[] Triangle(int size = 1024)
        {
            const int N = 32;

            return Generate.Triangle(size, N, N, -20.0, 20.0);
        }

        private static double[] ApplyWindow(double[] signal)
        {
            int n = signal.Length;

            var window = Window.Hamming(n);

            for (int i = 0; i < n; i++)
            {
                signal[i] *= window[i];
            }

            return signal;
        }
    }
}
