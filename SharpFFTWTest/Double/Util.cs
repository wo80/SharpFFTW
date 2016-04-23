using System;

namespace SharpFFTWTest.Double
{
    internal static class Util
    {
        public static double[] GenerateSignal(int n)
        {
            double[] data = new double[n];

            for (int i = 0; i < n; i++)
            {
                data[i] = i % 50;
            }

            return data;
        }

        public static bool CheckResults(int n, int scale, double[] data, double eps = 1e-3)
        {
            for (int i = 0; i < n; i++)
            {
                // We need to scale down, due to FFTW scaling by N.
                double result = data[i] / scale;

                // Check against original value.
                if (double.IsNaN(result) || Math.Abs(result - (i % 50)) > eps)
                {
                    return false;
                }
            }

            // Yeah, alright. So this was kind of a trivial test and of course
            // it's gonna work. But still.
            return true;
        }

        public static void WriteResult(bool ok)
        {
            var color = Console.ForegroundColor;

            if (ok)
            {
                Console.ForegroundColor = ConsoleColor.DarkGreen;
                Console.WriteLine("ok");
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("failed");
            }

            Console.ForegroundColor = color;
        }
    }
}
