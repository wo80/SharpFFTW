using NUnit.Framework;
using System.Reflection;
using System.Runtime.InteropServices;
using System;

namespace SharpFFTW.Tests
{
    [SetUpFixture]
    public class Setup
    {
        [OneTimeSetUp]
        public void GlobalSetup()
        {
            NativeLibrary.SetDllImportResolver(Assembly.GetExecutingAssembly(), DllImportResolver);
        }

        private static IntPtr DllImportResolver(string library, Assembly assembly, DllImportSearchPath? searchPath)
        {
            // Trying to find FFTW on Ubuntu.
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux) && library.StartsWith("fftw"))
            {
                return NativeLibrary.Load("lib" + library + ".so.3", assembly, searchPath);
            }

            // Otherwise, fall back to default import resolver.
            return IntPtr.Zero;
        }
    }
}
