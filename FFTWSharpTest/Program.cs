using System;

namespace FFTWSharpTest
{
    class Program
    {
        const int FFT_SIZE = 8192;

        static void Main(string[] args)
        {
            Double.TestNative1.Run(FFT_SIZE);
            Double.TestManaged1.Run(FFT_SIZE);

            Single.TestNative1.Run(FFT_SIZE);
            Single.TestManaged1.Run(FFT_SIZE);

            Console.WriteLine("\nDone. Press any key to exit.");
            Console.ReadLine();
        }
    }
}
