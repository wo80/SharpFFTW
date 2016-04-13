// The code in this file is provided courtesy of Tamas Szalay. Some functionality has been added.

namespace FFTWSharp.Double
{
    using System;
    using System.Threading;

    /// <summary>
    /// Creates, stores, and destroys FFTW plans.
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

        /// <summary>
        /// Create complex transform plan (fftw_plan_dft_1d).
        /// </summary>
        /// <param name="n">The logical size of the transform.</param>
        /// <param name="input">FFTW array of 16-byte complex numbers.</param>
        /// <param name="output">FFTW array of 16-byte complex numbers.</param>
        /// <param name="direction">Specifies the direction of the transform.</param>
        /// <param name="flags">Flags that specify the behavior of the planner.</param>
        /// <returns>The FFTW plan.</returns>
        public static Plan Create1(int n, ComplexArray input, ComplexArray output, Direction direction, Options flags)
        {
            mutex.WaitOne();
            var handle = NativeMethods.plan_dft_1d(n, input.Handle, output.Handle, direction, flags);
            mutex.ReleaseMutex();

            return new Plan(handle);
        }

        /// <summary>
        /// Create real to complex transform plan (fftw_plan_dft_r2c_1d).
        /// </summary>
        /// <param name="n">The logical size of the transform.</param>
        /// <param name="input">FFTW array of 8-byte real numbers.</param>
        /// <param name="output">FFTW array of 16-byte complex numbers.</param>
        /// <param name="flags">Flags that specify the behavior of the planner.</param>
        /// <returns>The FFTW plan.</returns>
        public static Plan Create1(int n, RealArray input, ComplexArray output, Options flags)
        {
            mutex.WaitOne();
            var handle = NativeMethods.plan_dft_r2c_1d(n, input.Handle, output.Handle, flags);
            mutex.ReleaseMutex();

            return new Plan(handle);
        }

        /// <summary>
        /// Create complex to real transform plan (fftw_plan_dft_c2r_1d).
        /// </summary>
        /// <param name="n">The logical size of the transform.</param>
        /// <param name="input">FFTW array of 16-byte complex numbers.</param>
        /// <param name="output">FFTW array of 8-byte real numbers.</param>
        /// <param name="flags">Flags that specify the behavior of the planner.</param>
        /// <returns>The FFTW plan.</returns>
        public static Plan Create1(int n, ComplexArray input, RealArray output, Options flags)
        {
            mutex.WaitOne();
            var handle = NativeMethods.plan_dft_c2r_1d(n, input.Handle, output.Handle, flags);
            mutex.ReleaseMutex();

            return new Plan(handle);
        }

        /// <summary>
        /// Create real transform plan (fftw_plan_r2r_1d).
        /// </summary>
        /// <param name="n">The logical size of the transform.</param>
        /// <param name="input">FFTW array of 8-byte real numbers.</param>
        /// <param name="output">FFTW array of 8-byte real numbers.</param>
        /// <param name="kind">The kind of real-to-real transform to compute.</param>
        /// <param name="flags">Flags that specify the behavior of the planner.</param>
        /// <returns>The FFTW plan.</returns>
        public static Plan Create1(int n, RealArray input, RealArray output, Transform kind, Options flags)
        {
            mutex.WaitOne();
            var handle = NativeMethods.plan_r2r_1d(n, input.Handle, output.Handle, kind, flags);
            mutex.ReleaseMutex();

            return new Plan(handle);
        }

        #endregion

        #region 2D plan creation

        /// <summary>
        /// Create complex transform plan (fftw_plan_dft_2d).
        /// </summary>
        /// <param name="nx">The logical size of the transform along the first dimension.</param>
        /// <param name="ny">The logical size of the transform along the second dimension.</param>
        /// <param name="input">FFTW array of 16-byte complex numbers.</param>
        /// <param name="output">FFTW array of 16-byte complex numbers.</param>
        /// <param name="direction">Specifies the direction of the transform.</param>
        /// <param name="flags">Flags that specify the behavior of the planner.</param>
        /// <returns>The FFTW plan.</returns>
        public static Plan Create2(int nx, int ny, ComplexArray input, ComplexArray output, Direction direction, Options flags)
        {
            mutex.WaitOne();
            var handle = NativeMethods.plan_dft_2d(nx, ny, input.Handle, output.Handle, direction, flags);
            mutex.ReleaseMutex();

            return new Plan(handle);
        }

        /// <summary>
        /// Create real to complex transform plan (fftw_plan_dft_r2c_2d).
        /// </summary>
        /// <param name="nx">The logical size of the transform along the first dimension.</param>
        /// <param name="ny">The logical size of the transform along the second dimension.</param>
        /// <param name="input">FFTW array of 8-byte real numbers.</param>
        /// <param name="output">FFTW array of 16-byte complex numbers.</param>
        /// <param name="flags">Flags that specify the behavior of the planner.</param>
        /// <returns>The FFTW plan.</returns>
        public static Plan Create2(int nx, int ny, RealArray input, ComplexArray output, Options flags)
        {
            mutex.WaitOne();
            var handle = NativeMethods.plan_dft_r2c_2d(nx, ny, input.Handle, output.Handle, flags);
            mutex.ReleaseMutex();

            return new Plan(handle);
        }

        /// <summary>
        /// Create complex to real transform plan (fftw_plan_dft_c2r_2d).
        /// </summary>
        /// <param name="nx">The logical size of the transform along the first dimension.</param>
        /// <param name="ny">The logical size of the transform along the second dimension.</param>
        /// <param name="input">FFTW array of 16-byte complex numbers.</param>
        /// <param name="output">FFTW array of 8-byte real numbers.</param>
        /// <param name="flags">Flags that specify the behavior of the planner.</param>
        /// <returns>The FFTW plan.</returns>
        public static Plan Create2(int nx, int ny, ComplexArray input, RealArray output, Options flags)
        {
            mutex.WaitOne();
            var handle = NativeMethods.plan_dft_c2r_2d(nx, ny, input.Handle, output.Handle, flags);
            mutex.ReleaseMutex();

            return new Plan(handle);
        }

        /// <summary>
        /// Create real transform plan (fftw_plan_r2r_2d).
        /// </summary>
        /// <param name="nx">The logical size of the transform along the first dimension.</param>
        /// <param name="ny">The logical size of the transform along the second dimension.</param>
        /// <param name="input">FFTW array of 8-byte real numbers.</param>
        /// <param name="output">FFTW array of 8-byte real numbers.</param>
        /// <param name="kindx">The kind of real-to-real transform to compute along the first dimension.</param>
        /// <param name="kindy">The kind of real-to-real transform to compute along the second dimension.</param>
        /// <param name="flags">Flags that specify the behavior of the planner.</param>
        /// <returns>The FFTW plan.</returns>
        public static Plan Create2(int nx, int ny, RealArray input, RealArray output, Transform kindx, Transform kindy, Options flags)
        {
            mutex.WaitOne();
            var handle = NativeMethods.plan_r2r_2d(nx, ny, input.Handle, output.Handle, kindx, kindy, flags);
            mutex.ReleaseMutex();

            return new Plan(handle);
        }

        #endregion

        #region 3D plan creation

        /// <summary>
        /// Create complex transform plan (fftw_plan_dft_3d).
        /// </summary>
        /// <param name="nx">The logical size of the transform along the first dimension.</param>
        /// <param name="ny">The logical size of the transform along the second dimension.</param>
        /// <param name="nz">The logical size of the transform along the third dimension.</param>
        /// <param name="input">FFTW array of 16-byte complex numbers.</param>
        /// <param name="output">FFTW array of 16-byte complex numbers.</param>
        /// <param name="direction">Specifies the direction of the transform.</param>
        /// <param name="flags">Flags that specify the behavior of the planner.</param>
        /// <returns>The FFTW plan.</returns>
        public static Plan Create3(int nx, int ny, int nz, ComplexArray input, ComplexArray output, Direction direction, Options flags)
        {
            mutex.WaitOne();
            var handle = NativeMethods.plan_dft_3d(nx, ny, nz, input.Handle, output.Handle, direction, flags);
            mutex.ReleaseMutex();

            return new Plan(handle);
        }

        /// <summary>
        /// Create real to complex transform plan (fftw_plan_dft_r2c_3d).
        /// </summary>
        /// <param name="nx">The logical size of the transform along the first dimension.</param>
        /// <param name="ny">The logical size of the transform along the second dimension.</param>
        /// <param name="nz">The logical size of the transform along the third dimension.</param>
        /// <param name="input">FFTW array of 8-byte real numbers.</param>
        /// <param name="output">FFTW array of 16-byte complex numbers.</param>
        /// <param name="flags">Flags that specify the behavior of the planner.</param>
        /// <returns>The FFTW plan.</returns>
        public static Plan Create3(int nx, int ny, int nz, RealArray input, ComplexArray output, Options flags)
        {
            mutex.WaitOne();
            var handle = NativeMethods.plan_dft_r2c_3d(nx, ny, nz, input.Handle, output.Handle, flags);
            mutex.ReleaseMutex();

            return new Plan(handle);
        }

        /// <summary>
        /// Create complex to real transform plan (fftw_plan_dft_c2r_3d).
        /// </summary>
        /// <param name="nx">The logical size of the transform along the first dimension.</param>
        /// <param name="ny">The logical size of the transform along the second dimension.</param>
        /// <param name="nz">The logical size of the transform along the third dimension.</param>
        /// <param name="input">FFTW array of 16-byte complex numbers.</param>
        /// <param name="output">FFTW array of 8-byte real numbers.</param>
        /// <param name="flags">Flags that specify the behavior of the planner.</param>
        /// <returns>The FFTW plan.</returns>
        public static Plan Create3(int nx, int ny, int nz, ComplexArray input, RealArray output, Options flags)
        {
            mutex.WaitOne();
            var handle = NativeMethods.plan_dft_c2r_3d(nx, ny, nz, input.Handle, output.Handle, flags);
            mutex.ReleaseMutex();

            return new Plan(handle);
        }

        /// <summary>
        /// Create real transform plan  (fftw_plan_r2r_3d).
        /// </summary>
        /// <param name="nx">The logical size of the transform along the first dimension.</param>
        /// <param name="ny">The logical size of the transform along the second dimension.</param>
        /// <param name="nz">The logical size of the transform along the third dimension.</param>
        /// <param name="input">FFTW array of 8-byte real numbers.</param>
        /// <param name="output">FFTW array of 8-byte real numbers.</param>
        /// <param name="kindx">The kind of real-to-real transform to compute along the first dimension.</param>
        /// <param name="kindy">The kind of real-to-real transform to compute along the second dimension.</param>
        /// <param name="kindz">The kind of real-to-real transform to compute along the third dimension.</param>
        /// <param name="flags">Flags that specify the behavior of the planner.</param>
        /// <returns>The FFTW plan.</returns>
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

        /// <summary>
        /// Create complex transform plan (fftw_plan_dft).
        /// </summary>
        /// <param name="rank">Number of dimensions.</param>
        /// <param name="n">Array containing the logical size along each dimension.</param>
        /// <param name="input">FFTW array of 16-byte complex numbers.</param>
        /// <param name="output">FFTW array of 16-byte complex numbers.</param>
        /// <param name="direction"></param>
        /// <param name="flags">Flags that specify the behavior of the planner.</param>
        /// <returns>The FFTW plan.</returns>
        public static Plan Create(int rank, int[] n, ComplexArray input, ComplexArray output, Direction direction, Options flags)
        {
            mutex.WaitOne();
            var handle = NativeMethods.plan_dft(rank, n, input.Handle, output.Handle, direction, flags);
            mutex.ReleaseMutex();

            return new Plan(handle);
        }

        /// <summary>
        /// Create real to complex transform plan (fftw_plan_dft_r2c).
        /// </summary>
        /// <param name="rank">Number of dimensions.</param>
        /// <param name="n">Array containing the logical size along each dimension.</param>
        /// <param name="input">FFTW array of 8-byte real numbers.</param>
        /// <param name="output">FFTW array of 16-byte complex numbers.</param>
        /// <param name="flags">Flags that specify the behavior of the planner.</param>
        /// <returns>The FFTW plan.</returns>
        public static Plan Create(int rank, int[] n, RealArray input, ComplexArray output, Options flags)
        {
            mutex.WaitOne();
            var handle = NativeMethods.plan_dft_r2c(rank, n, input.Handle, output.Handle, flags);
            mutex.ReleaseMutex();

            return new Plan(handle);
        }

        /// <summary>
        /// Create complex to real transform plan (fftw_plan_dft_c2r).
        /// </summary>
        /// <param name="rank">Number of dimensions.</param>
        /// <param name="n">Array containing the logical size along each dimension.</param>
        /// <param name="input">FFTW array of 16-byte complex numbers.</param>
        /// <param name="output">FFTW array of 8-byte real numbers.</param>
        /// <param name="flags">Flags that specify the behavior of the planner.</param>
        /// <returns>The FFTW plan.</returns>
        public static Plan Create(int rank, int[] n, ComplexArray input, RealArray output, Options flags)
        {
            mutex.WaitOne();
            var handle = NativeMethods.plan_dft_c2r(rank, n, input.Handle, output.Handle, flags);
            mutex.ReleaseMutex();

            return new Plan(handle);
        }

        /// <summary>
        /// Create real transform plan (fftw_plan_r2r).
        /// </summary>
        /// <param name="rank">Number of dimensions.</param>
        /// <param name="n">Array containing the logical size along each dimension.</param>
        /// <param name="input">FFTW array of 8-byte real numbers.</param>
        /// <param name="output">FFTW array of 8-byte real numbers.</param>
        /// <param name="kind">An array containing the kind of real-to-real transform to compute along each dimension.</param>
        /// <param name="flags">Flags that specify the behavior of the planner.</param>
        /// <returns>The FFTW plan.</returns>
        public static Plan Create(int rank, int[] n, RealArray input, RealArray output, Transform[] kind, Options flags)
        {
            mutex.WaitOne();
            var handle = NativeMethods.plan_r2r(rank, n, input.Handle, output.Handle, kind, flags);
            mutex.ReleaseMutex();

            return new Plan(handle);
        }

        #endregion
    }
}