// The code in this file is provided courtesy of Tamas Szalay. Some functionality has been added.

// FFTWSharp
// ===========
// Basic C# wrapper for FFTW.
//
// Features
// ============
//    * Unmanaged function calls to main FFTW functions for both single and double precision
//    * Basic managed wrappers for FFTW plans and unmanaged arrays
//    * Test program that demonstrates basic functionality
//
// Notes
// ============
//    * Most of this was written in 2005
//    * Slightly updated since to get it running with Visual Studio Express 2010
//    * If you have a question about FFTW, ask the FFTW people, and not me. I did not write FFTW.
//    * If you have a question about this wrapper, probably still don't ask me, since I wrote it almost a decade ago.

namespace FFTWSharp.Single
{
    using System;
    using System.Threading;

    /// <summary>
    /// Creates, stores, and destroys fftw plans
    /// </summary>
    public class Plan : AbstractPlan
    {
        private static readonly Mutex mutex = new Mutex();

        public Plan(IntPtr handle)
            : base(handle)
        {
        }

        public override void Execute()
        {
            NativeMethods.execute(handle);
        }

        public override void Dispose(bool disposing)
        {
            if (!hasDisposed)
            {
                if (handle != IntPtr.Zero)
                {
                    NativeMethods.destroy_plan(handle);
                    handle = IntPtr.Zero;
                }
            }

            hasDisposed = disposing;
        }

        #region 1D plan creation

        public static Plan Create1(int n, ComplexArray input, ComplexArray output, Direction direction, Options flags)
        {
            mutex.WaitOne();
            var handle = NativeMethods.plan_dft_1d(n, input.Handle, output.Handle, direction, flags);
            mutex.ReleaseMutex();

            return new Plan(handle);
        }

        public static Plan Create1(int n, RealArray input, ComplexArray output, Options flags)
        {
            mutex.WaitOne();
            var handle = NativeMethods.plan_dft_r2c_1d(n, input.Handle, output.Handle, flags);
            mutex.ReleaseMutex();

            return new Plan(handle);
        }

        public static Plan Create1(int n, ComplexArray input, RealArray output, Options flags)
        {
            mutex.WaitOne();
            var handle = NativeMethods.plan_dft_c2r_1d(n, input.Handle, output.Handle, flags);
            mutex.ReleaseMutex();

            return new Plan(handle);
        }

        public static Plan Create1(int n, RealArray input, RealArray output, Transform kind, Options flags)
        {
            mutex.WaitOne();
            var handle = NativeMethods.plan_r2r_1d(n, input.Handle, output.Handle, kind, flags);
            mutex.ReleaseMutex();

            return new Plan(handle);
        }

        #endregion

        #region 2D plan creation

        public static Plan Create2(int nx, int ny, ComplexArray input, ComplexArray output, Direction direction, Options flags)
        {
            mutex.WaitOne();
            var handle = NativeMethods.plan_dft_2d(nx, ny, input.Handle, output.Handle, direction, flags);
            mutex.ReleaseMutex();

            return new Plan(handle);
        }

        public static Plan Create2(int nx, int ny, RealArray input, ComplexArray output, Options flags)
        {
            mutex.WaitOne();
            var handle = NativeMethods.plan_dft_r2c_2d(nx, ny, input.Handle, output.Handle, flags);
            mutex.ReleaseMutex();

            return new Plan(handle);
        }

        public static Plan Create2(int nx, int ny, ComplexArray input, RealArray output, Options flags)
        {
            mutex.WaitOne();
            var handle = NativeMethods.plan_dft_c2r_2d(nx, ny, input.Handle, output.Handle, flags);
            mutex.ReleaseMutex();

            return new Plan(handle);
        }

        public static Plan Create2(int nx, int ny, RealArray input, RealArray output, Transform kindx, Transform kindy, Options flags)
        {
            mutex.WaitOne();
            var handle = NativeMethods.plan_r2r_2d(nx, ny, input.Handle, output.Handle, kindx, kindy, flags);
            mutex.ReleaseMutex();

            return new Plan(handle);
        }

        #endregion

        #region 3D plan creation

        public static Plan Create3(int nx, int ny, int nz, ComplexArray input, ComplexArray output, Direction direction, Options flags)
        {
            mutex.WaitOne();
            var handle = NativeMethods.plan_dft_3d(nx, ny, nz, input.Handle, output.Handle, direction, flags);
            mutex.ReleaseMutex();

            return new Plan(handle);
        }

        public static Plan Create3(int nx, int ny, int nz, RealArray input, ComplexArray output, Options flags)
        {
            mutex.WaitOne();
            var handle = NativeMethods.plan_dft_r2c_3d(nx, ny, nz, input.Handle, output.Handle, flags);
            mutex.ReleaseMutex();

            return new Plan(handle);
        }

        public static Plan Create3(int nx, int ny, int nz, ComplexArray input, RealArray output, Options flags)
        {
            mutex.WaitOne();
            var handle = NativeMethods.plan_dft_c2r_3d(nx, ny, nz, input.Handle, output.Handle, flags);
            mutex.ReleaseMutex();

            return new Plan(handle);
        }

        public static Plan Create3(int nx, int ny, int nz, RealArray input, RealArray output,
            Transform kindx, Transform kindy, Transform kindz, Options flags)
        {
            mutex.WaitOne();
            var handle = NativeMethods.plan_r2r_3d(nx, ny, nz, input.Handle, output.Handle,
                kindx, kindy, kindz, flags);
            mutex.ReleaseMutex();

            return new Plan(handle);
        }

        #endregion

        #region General plan creation

        public static Plan Create(int rank, int[] n, ComplexArray input, ComplexArray output, Direction direction, Options flags)
        {
            mutex.WaitOne();
            var handle = NativeMethods.plan_dft(rank, n, input.Handle, output.Handle, direction, flags);
            mutex.ReleaseMutex();

            return new Plan(handle);
        }

        public static Plan Create(int rank, int[] n, RealArray input, ComplexArray output, Options flags)
        {
            mutex.WaitOne();
            var handle = NativeMethods.plan_dft_r2c(rank, n, input.Handle, output.Handle, flags);
            mutex.ReleaseMutex();

            return new Plan(handle);
        }

        public static Plan Create(int rank, int[] n, ComplexArray input, RealArray output, Options flags)
        {
            mutex.WaitOne();
            var handle = NativeMethods.plan_dft_c2r(rank, n, input.Handle, output.Handle, flags);
            mutex.ReleaseMutex();

            return new Plan(handle);
        }

        public static Plan Create(int rank, int[] n, RealArray input, RealArray output,
            Transform[] kind, Options flags)
        {
            mutex.WaitOne();
            var handle = NativeMethods.plan_r2r(rank, n, input.Handle, output.Handle,
                kind, flags);
            mutex.ReleaseMutex();

            return new Plan(handle);
        }

        #endregion
    }
}