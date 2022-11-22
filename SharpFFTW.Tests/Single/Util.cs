using System;

namespace SharpFFTW.Tests.Single
{
    internal static class Util
    {
        public static float[] GenerateSignal(int n)
        {
            float[] data = new float[n];

            for (int i = 0; i < n; i++)
            {
                data[i] = i % 50;
            }

            return data;
        }

        public static bool CheckResults(int n, int scale, float[] data, float eps = 1e-3f)
        {
            for (int i = 0; i < n; i++)
            {
                // We need to scale down, due to FFTW scaling by N.
                float result = data[i] / scale;

                // Check against original value.
                if (float.IsNaN(result) || Math.Abs(result - (i % 50)) > eps)
                {
                    return false;
                }
            }

            // Yeah, alright. So this was kind of a trivial test and of course
            // it's gonna work. But still.
            return true;
        }

        public static void PrintResults(int n, int scale, float[] data, float eps = 1e-3f)
        {
            if (CheckResults(n, scale, data, eps))
            {
                Write("ok", true);
            }
            else
            {
                Write("failed", false);
            }
        }

        public static void Write(string message, bool ok)
        {
            var color = Console.ForegroundColor;

            if (ok)
            {
                Console.ForegroundColor = ConsoleColor.DarkGreen;
                Console.WriteLine(message);
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(message);
            }

            Console.ForegroundColor = color;
        }
    }
}
