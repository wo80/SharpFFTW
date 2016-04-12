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

using System;
using System.Runtime.InteropServices;
using System.Numerics;
using System.Threading;

namespace FFTWSharp.Single
{
    /// <summary>
    /// Creates, stores, and destroys fftw plans
    /// </summary>
    public class Plan : AbstractPlan
    {
        static Mutex FFTW_Lock = new Mutex();

        public Plan(IntPtr handle)
            : base(handle)
        {
        }

        public void Execute()
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

        #region Plan Creation
        //Complex<->Complex transforms
        public static Plan dft_1d(int n, ComplexArray input, ComplexArray output, Direction direction, Options flags)
        {
            FFTW_Lock.WaitOne();

            var handle = NativeMethods.plan_dft_1d(n, input.Handle, output.Handle, direction, flags);
            FFTW_Lock.ReleaseMutex();

            return new Plan(handle);
        }

        public static Plan dft_2d(int nx, int ny, ComplexArray input, ComplexArray output, Direction direction, Options flags)
        {
            FFTW_Lock.WaitOne();

            var handle = NativeMethods.plan_dft_2d(nx, ny, input.Handle, output.Handle, direction, flags);
            FFTW_Lock.ReleaseMutex();

            return new Plan(handle);
        }

        public static Plan dft_3d(int nx, int ny, int nz, ComplexArray input, ComplexArray output, Direction direction, Options flags)
        {
            FFTW_Lock.WaitOne();

            var handle = NativeMethods.plan_dft_3d(nx, ny, nz, input.Handle, output.Handle, direction, flags);
            FFTW_Lock.ReleaseMutex();

            return new Plan(handle);
        }

        public static Plan dft(int rank, int[] n, ComplexArray input, ComplexArray output, Direction direction, Options flags)
        {
            FFTW_Lock.WaitOne();

            var handle = NativeMethods.plan_dft(rank, n, input.Handle, output.Handle, direction, flags);
            FFTW_Lock.ReleaseMutex();

            return new Plan(handle);
        }

        //Real->Complex transforms
        public static Plan dft_r2c_1d(int n, ComplexArray input, ComplexArray output, Options flags)
        {
            FFTW_Lock.WaitOne();

            var handle = NativeMethods.plan_dft_r2c_1d(n, input.Handle, output.Handle, flags);
            FFTW_Lock.ReleaseMutex();

            return new Plan(handle);
        }

        public static Plan dft_r2c_2d(int nx, int ny, ComplexArray input, ComplexArray output, Options flags)
        {
            FFTW_Lock.WaitOne();

            var handle = NativeMethods.plan_dft_r2c_2d(nx, ny, input.Handle, output.Handle, flags);
            FFTW_Lock.ReleaseMutex();

            return new Plan(handle);
        }

        public static Plan dft_r2c_3d(int nx, int ny, int nz, ComplexArray input, ComplexArray output, Options flags)
        {
            FFTW_Lock.WaitOne();

            var handle = NativeMethods.plan_dft_r2c_3d(nx, ny, nz, input.Handle, output.Handle, flags);
            FFTW_Lock.ReleaseMutex();

            return new Plan(handle);
        }

        public static Plan dft_r2c(int rank, int[] n, ComplexArray input, ComplexArray output, Options flags)
        {
            FFTW_Lock.WaitOne();

            var handle = NativeMethods.plan_dft_r2c(rank, n, input.Handle, output.Handle, flags);
            FFTW_Lock.ReleaseMutex();

            return new Plan(handle);
        }

        //Complex->Real
        public static Plan dft_c2r_1d(int n, ComplexArray input, ComplexArray output, Direction direction, Options flags)
        {
            FFTW_Lock.WaitOne();

            var handle = NativeMethods.plan_dft_c2r_1d(n, input.Handle, output.Handle, flags);
            FFTW_Lock.ReleaseMutex();

            return new Plan(handle);
        }

        public static Plan dft_c2r_2d(int nx, int ny, ComplexArray input, ComplexArray output, Direction direction, Options flags)
        {
            FFTW_Lock.WaitOne();

            var handle = NativeMethods.plan_dft_c2r_2d(nx, ny, input.Handle, output.Handle, flags);
            FFTW_Lock.ReleaseMutex();

            return new Plan(handle);
        }

        public static Plan dft_c2r_3d(int nx, int ny, int nz, ComplexArray input, ComplexArray output, Direction direction, Options flags)
        {
            FFTW_Lock.WaitOne();

            var handle = NativeMethods.plan_dft_c2r_3d(nx, ny, nz, input.Handle, output.Handle, flags);
            FFTW_Lock.ReleaseMutex();

            return new Plan(handle);
        }

        public static Plan dft_c2r(int rank, int[] n, ComplexArray input, ComplexArray output, Direction direction, Options flags)
        {
            FFTW_Lock.WaitOne();

            var handle = NativeMethods.plan_dft_c2r(rank, n, input.Handle, output.Handle, flags);
            FFTW_Lock.ReleaseMutex();

            return new Plan(handle);
        }

        //Real<->Real
        public static Plan r2r_1d(int n, ComplexArray input, ComplexArray output, Transform kind, Options flags)
        {
            FFTW_Lock.WaitOne();

            var handle = NativeMethods.plan_r2r_1d(n, input.Handle, output.Handle, kind, flags);
            FFTW_Lock.ReleaseMutex();

            return new Plan(handle);
        }

        public static Plan r2r_2d(int nx, int ny, ComplexArray input, ComplexArray output, Transform kindx, Transform kindy, Options flags)
        {
            FFTW_Lock.WaitOne();

            var handle = NativeMethods.plan_r2r_2d(nx, ny, input.Handle, output.Handle, kindx, kindy, flags);
            FFTW_Lock.ReleaseMutex();

            return new Plan(handle);
        }

        public static Plan r2r_3d(int nx, int ny, int nz, ComplexArray input, ComplexArray output,
            Transform kindx, Transform kindy, Transform kindz, Options flags)
        {
            FFTW_Lock.WaitOne();

            var handle = NativeMethods.plan_r2r_3d(nx, ny, nz, input.Handle, output.Handle,
                kindx, kindy, kindz, flags);
            FFTW_Lock.ReleaseMutex();

            return new Plan(handle);
        }

        public static Plan r2r(int rank, int[] n, ComplexArray input, ComplexArray output,
            Transform[] kind, Options flags)
        {
            FFTW_Lock.WaitOne();

            var handle = NativeMethods.plan_r2r(rank, n, input.Handle, output.Handle,
                kind, flags);
            FFTW_Lock.ReleaseMutex();

            return new Plan(handle);
        }
        #endregion
    }
}