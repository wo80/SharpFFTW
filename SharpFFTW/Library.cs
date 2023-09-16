using System;
using System.Reflection;
using System.Runtime.InteropServices;

namespace SharpFFTW
{
#if NET5_0_OR_GREATER
    public class Library
    {
        public static void SetImportResolver()
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
#endif
}
