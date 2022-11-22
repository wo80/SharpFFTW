using System;

namespace SharpFFTW.Tests
{
    class Program
    {
        const int FFT_SIZE = 8192;

        static void Main(string[] args)
        {
            Double.TestNative.Run(FFT_SIZE);
            Double.TestManaged.Run(FFT_SIZE);

            Single.TestNative.Run(FFT_SIZE);
            Single.TestManaged.Run(FFT_SIZE);

            Console.WriteLine("\nDone. Press any key to exit.");
            Console.ReadLine();
        }
    }
}
