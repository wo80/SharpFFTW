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
    public class fftwf_plan
    {
        static Mutex FFTW_Lock = new Mutex();

        protected IntPtr handle;
        public IntPtr Handle
        { get { return handle; } }

        public void Execute()
        {
            fftwf.execute(handle);
        }

        ~fftwf_plan()
        {
            fftwf.destroy_plan(handle);
        }

        #region Plan Creation
        //Complex<->Complex transforms
        public static fftwf_plan dft_1d(int n, fftwf_complexarray input, fftwf_complexarray output, fftw_direction direction, fftw_flags flags)
        {
            FFTW_Lock.WaitOne();
            fftwf_plan p = new fftwf_plan();
            p.handle = fftwf.dft_1d(n, input.Handle, output.Handle, direction, flags);
            FFTW_Lock.ReleaseMutex();

            return p;
        }

        public static fftwf_plan dft_2d(int nx, int ny, fftwf_complexarray input, fftwf_complexarray output, fftw_direction direction, fftw_flags flags)
        {
            FFTW_Lock.WaitOne();
            fftwf_plan p = new fftwf_plan();
            p.handle = fftwf.dft_2d(nx, ny, input.Handle, output.Handle, direction, flags);
            FFTW_Lock.ReleaseMutex();

            return p;
        }

        public static fftwf_plan dft_3d(int nx, int ny, int nz, fftwf_complexarray input, fftwf_complexarray output, fftw_direction direction, fftw_flags flags)
        {
            FFTW_Lock.WaitOne();
            fftwf_plan p = new fftwf_plan();
            p.handle = fftwf.dft_3d(nx, ny, nz, input.Handle, output.Handle, direction, flags);
            FFTW_Lock.ReleaseMutex();

            return p;
        }

        public static fftwf_plan dft(int rank, int[] n, fftwf_complexarray input, fftwf_complexarray output, fftw_direction direction, fftw_flags flags)
        {
            FFTW_Lock.WaitOne();
            fftwf_plan p = new fftwf_plan();
            p.handle = fftwf.dft(rank, n, input.Handle, output.Handle, direction, flags);
            FFTW_Lock.ReleaseMutex();

            return p;
        }

        //Real->Complex transforms
        public static fftwf_plan dft_r2c_1d(int n, fftwf_complexarray input, fftwf_complexarray output, fftw_flags flags)
        {
            FFTW_Lock.WaitOne();
            fftwf_plan p = new fftwf_plan();
            p.handle = fftwf.dft_r2c_1d(n, input.Handle, output.Handle, flags);
            FFTW_Lock.ReleaseMutex();

            return p;
        }

        public static fftwf_plan dft_r2c_2d(int nx, int ny, fftwf_complexarray input, fftwf_complexarray output, fftw_flags flags)
        {
            FFTW_Lock.WaitOne();
            fftwf_plan p = new fftwf_plan();
            p.handle = fftwf.dft_r2c_2d(nx, ny, input.Handle, output.Handle, flags);
            FFTW_Lock.ReleaseMutex();

            return p;
        }

        public static fftwf_plan dft_r2c_3d(int nx, int ny, int nz, fftwf_complexarray input, fftwf_complexarray output, fftw_flags flags)
        {
            FFTW_Lock.WaitOne();
            fftwf_plan p = new fftwf_plan();
            p.handle = fftwf.dft_r2c_3d(nx, ny, nz, input.Handle, output.Handle, flags);
            FFTW_Lock.ReleaseMutex();

            return p;
        }

        public static fftwf_plan dft_r2c(int rank, int[] n, fftwf_complexarray input, fftwf_complexarray output, fftw_flags flags)
        {
            FFTW_Lock.WaitOne();
            fftwf_plan p = new fftwf_plan();
            p.handle = fftwf.dft_r2c(rank, n, input.Handle, output.Handle, flags);
            FFTW_Lock.ReleaseMutex();

            return p;
        }

        //Complex->Real
        public static fftwf_plan dft_c2r_1d(int n, fftwf_complexarray input, fftwf_complexarray output, fftw_direction direction, fftw_flags flags)
        {
            FFTW_Lock.WaitOne();
            fftwf_plan p = new fftwf_plan();
            p.handle = fftwf.dft_c2r_1d(n, input.Handle, output.Handle, flags);
            FFTW_Lock.ReleaseMutex();

            return p;
        }

        public static fftwf_plan dft_c2r_2d(int nx, int ny, fftwf_complexarray input, fftwf_complexarray output, fftw_direction direction, fftw_flags flags)
        {
            FFTW_Lock.WaitOne();
            fftwf_plan p = new fftwf_plan();
            p.handle = fftwf.dft_c2r_2d(nx, ny, input.Handle, output.Handle, flags);
            FFTW_Lock.ReleaseMutex();

            return p;
        }

        public static fftwf_plan dft_c2r_3d(int nx, int ny, int nz, fftwf_complexarray input, fftwf_complexarray output, fftw_direction direction, fftw_flags flags)
        {
            FFTW_Lock.WaitOne();
            fftwf_plan p = new fftwf_plan();
            p.handle = fftwf.dft_c2r_3d(nx, ny, nz, input.Handle, output.Handle, flags);
            FFTW_Lock.ReleaseMutex();

            return p;
        }

        public static fftwf_plan dft_c2r(int rank, int[] n, fftwf_complexarray input, fftwf_complexarray output, fftw_direction direction, fftw_flags flags)
        {
            FFTW_Lock.WaitOne();
            fftwf_plan p = new fftwf_plan();
            p.handle = fftwf.dft_c2r(rank, n, input.Handle, output.Handle, flags);
            FFTW_Lock.ReleaseMutex();

            return p;
        }

        //Real<->Real
        public static fftwf_plan r2r_1d(int n, fftwf_complexarray input, fftwf_complexarray output, fftw_kind kind, fftw_flags flags)
        {
            FFTW_Lock.WaitOne();
            fftwf_plan p = new fftwf_plan();
            p.handle = fftwf.r2r_1d(n, input.Handle, output.Handle, kind, flags);
            FFTW_Lock.ReleaseMutex();

            return p;
        }

        public static fftwf_plan r2r_2d(int nx, int ny, fftwf_complexarray input, fftwf_complexarray output, fftw_kind kindx, fftw_kind kindy, fftw_flags flags)
        {
            FFTW_Lock.WaitOne();
            fftwf_plan p = new fftwf_plan();
            p.handle = fftwf.r2r_2d(nx, ny, input.Handle, output.Handle, kindx, kindy, flags);
            FFTW_Lock.ReleaseMutex();

            return p;
        }

        public static fftwf_plan r2r_3d(int nx, int ny, int nz, fftwf_complexarray input, fftwf_complexarray output,
            fftw_kind kindx, fftw_kind kindy, fftw_kind kindz, fftw_flags flags)
        {
            FFTW_Lock.WaitOne();
            fftwf_plan p = new fftwf_plan();
            p.handle = fftwf.r2r_3d(nx, ny, nz, input.Handle, output.Handle,
                kindx, kindy, kindz, flags);
            FFTW_Lock.ReleaseMutex();

            return p;
        }

        public static fftwf_plan r2r(int rank, int[] n, fftwf_complexarray input, fftwf_complexarray output,
            fftw_kind[] kind, fftw_flags flags)
        {
            FFTW_Lock.WaitOne();
            fftwf_plan p = new fftwf_plan();
            p.handle = fftwf.r2r(rank, n, input.Handle, output.Handle,
                kind, flags);
            FFTW_Lock.ReleaseMutex();

            return p;
        }
        #endregion
    }
}