// The code in this file is provided courtesy of Tamas Szalay. Some functionality has been added.

namespace SharpFFTW.Single
{
    using System;
    using System.Runtime.InteropServices;
    using System.Threading;

    /// <summary>
    /// Creates, stores, and destroys FFTW plans.
    /// </summary>
    public class Plan : AbstractPlan<float>
    {
        private static readonly Mutex mutex = new Mutex();

        private Plan(IntPtr handle, AbstractArray<float> input,
            AbstractArray<float> output, bool ownsArrays)
            : base(handle, input, output, ownsArrays)
        {
        }

        /// <summary>
        /// Export FFTW wisdom to file.
        /// </summary>
        public static bool Export(string filename)
        {
            return NativeMethods.fftwf_export_wisdom_to_filename(filename) > 0;
        }

        /// <summary>
        /// Import FFTW wisdom from file.
        /// </summary>
        public static bool Import(string filename)
        {
            return NativeMethods.fftwf_import_wisdom_from_filename(filename) > 0;
        }

        /// <inheritdoc />
        public override void Execute()
        {
            NativeMethods.fftwf_execute(handle);
        }

        /// <inheritdoc />
        public override string ToString()
        {
            // NOTE: this leaks native memory, since the returned char* pointer isn't free'd.
            var p = NativeMethods.fftwf_sprint_plan(handle);
            
            return Marshal.PtrToStringAnsi(p);
        }

        /// <inheritdoc />
        public override void Dispose(bool disposing)
        {
            if (!hasDisposed)
            {
                if (handle != IntPtr.Zero)
                {
                    NativeMethods.fftwf_destroy_plan(handle);
                    handle = IntPtr.Zero;
                }

                if (ownsArrays)
                {
                    input.Dispose();
                    output.Dispose();
                }
            }

            hasDisposed = disposing;
        }

        #region 1D plan creation

        /// <summary>
        /// Create a 1D complex transform plan (fftwf_plan_dft_1d).
        /// </summary>
        /// <param name="n">The logical size of the transform.</param>
        /// <param name="input">FFTW array of 8-byte complex numbers.</param>
        /// <param name="output">FFTW array of 8-byte complex numbers.</param>
        /// <param name="direction">Specifies the direction of the transform.</param>
        /// <param name="flags">Flags that specify the behavior of the planner.</param>
        /// <returns>The FFTW plan.</returns>
        public static Plan Create1(int n, ComplexArray input, ComplexArray output, Direction direction, Options flags)
        {
            mutex.WaitOne();
            var handle = NativeMethods.fftwf_plan_dft_1d(n, input.Handle, output.Handle, direction, flags);
            mutex.ReleaseMutex();

            return new Plan(handle, input, output, false);
        }

        /// <summary>
        /// Create a 1D real to complex transform plan (fftwf_plan_dft_r2c_1d).
        /// </summary>
        /// <param name="n">The logical size of the transform.</param>
        /// <param name="input">FFTW array of 4-byte real numbers.</param>
        /// <param name="output">FFTW array of 8-byte complex numbers.</param>
        /// <param name="flags">Flags that specify the behavior of the planner.</param>
        /// <returns>The FFTW plan.</returns>
        public static Plan Create1(int n, RealArray input, ComplexArray output, Options flags)
        {
            mutex.WaitOne();
            var handle = NativeMethods.fftwf_plan_dft_r2c_1d(n, input.Handle, output.Handle, flags);
            mutex.ReleaseMutex();

            return new Plan(handle, input, output, false);
        }

        /// <summary>
        /// Create a 1D complex to real transform plan (fftwf_plan_dft_c2r_1d).
        /// </summary>
        /// <param name="n">The logical size of the transform.</param>
        /// <param name="input">FFTW array of 8-byte complex numbers.</param>
        /// <param name="output">FFTW array of 4-byte real numbers.</param>
        /// <param name="flags">Flags that specify the behavior of the planner.</param>
        /// <returns>The FFTW plan.</returns>
        public static Plan Create1(int n, ComplexArray input, RealArray output, Options flags)
        {
            mutex.WaitOne();
            var handle = NativeMethods.fftwf_plan_dft_c2r_1d(n, input.Handle, output.Handle, flags);
            mutex.ReleaseMutex();

            return new Plan(handle, input, output, false);
        }

        /// <summary>
        /// Create a 1D real transform plan (fftwf_plan_r2r_1d).
        /// </summary>
        /// <param name="n">The logical size of the transform.</param>
        /// <param name="input">FFTW array of 4-byte real numbers.</param>
        /// <param name="output">FFTW array of 4-byte real numbers.</param>
        /// <param name="kind">The kind of real-to-real transform to compute.</param>
        /// <param name="flags">Flags that specify the behavior of the planner.</param>
        /// <returns>The FFTW plan.</returns>
        public static Plan Create1(int n, RealArray input, RealArray output, Transform kind, Options flags)
        {
            mutex.WaitOne();
            var handle = NativeMethods.fftwf_plan_r2r_1d(n, input.Handle, output.Handle, kind, flags);
            mutex.ReleaseMutex();

            return new Plan(handle, input, output, false);
        }

        #endregion

        #region 2D plan creation

        /// <summary>
        /// Create a 2D complex transform plan (fftwf_plan_dft_2d).
        /// </summary>
        /// <param name="nx">The logical size of the transform along the first dimension.</param>
        /// <param name="ny">The logical size of the transform along the second dimension.</param>
        /// <param name="input">FFTW array of 8-byte complex numbers.</param>
        /// <param name="output">FFTW array of 8-byte complex numbers.</param>
        /// <param name="direction">Specifies the direction of the transform.</param>
        /// <param name="flags">Flags that specify the behavior of the planner.</param>
        /// <returns>The FFTW plan.</returns>
        public static Plan Create2(int nx, int ny, ComplexArray input, ComplexArray output, Direction direction, Options flags)
        {
            mutex.WaitOne();
            var handle = NativeMethods.fftwf_plan_dft_2d(nx, ny, input.Handle, output.Handle, direction, flags);
            mutex.ReleaseMutex();

            return new Plan(handle, input, output, false);
        }

        /// <summary>
        /// Create a 2D real to complex transform plan (fftwf_plan_dft_r2c_2d).
        /// </summary>
        /// <param name="nx">The logical size of the transform along the first dimension.</param>
        /// <param name="ny">The logical size of the transform along the second dimension.</param>
        /// <param name="input">FFTW array of 4-byte real numbers.</param>
        /// <param name="output">FFTW array of 8-byte complex numbers.</param>
        /// <param name="flags">Flags that specify the behavior of the planner.</param>
        /// <returns>The FFTW plan.</returns>
        public static Plan Create2(int nx, int ny, RealArray input, ComplexArray output, Options flags)
        {
            mutex.WaitOne();
            var handle = NativeMethods.fftwf_plan_dft_r2c_2d(nx, ny, input.Handle, output.Handle, flags);
            mutex.ReleaseMutex();

            return new Plan(handle, input, output, false);
        }

        /// <summary>
        /// Create a 2D complex to real transform plan (fftwf_plan_dft_c2r_2d).
        /// </summary>
        /// <param name="nx">The logical size of the transform along the first dimension.</param>
        /// <param name="ny">The logical size of the transform along the second dimension.</param>
        /// <param name="input">FFTW array of 8-byte complex numbers.</param>
        /// <param name="output">FFTW array of 4-byte real numbers.</param>
        /// <param name="flags">Flags that specify the behavior of the planner.</param>
        /// <returns>The FFTW plan.</returns>
        public static Plan Create2(int nx, int ny, ComplexArray input, RealArray output, Options flags)
        {
            mutex.WaitOne();
            var handle = NativeMethods.fftwf_plan_dft_c2r_2d(nx, ny, input.Handle, output.Handle, flags);
            mutex.ReleaseMutex();

            return new Plan(handle, input, output, false);
        }

        /// <summary>
        /// Create a 2D real transform plan (fftwf_plan_r2r_2d).
        /// </summary>
        /// <param name="nx">The logical size of the transform along the first dimension.</param>
        /// <param name="ny">The logical size of the transform along the second dimension.</param>
        /// <param name="input">FFTW array of 4-byte real numbers.</param>
        /// <param name="output">FFTW array of 4-byte real numbers.</param>
        /// <param name="kindx">The kind of real-to-real transform to compute along the first dimension.</param>
        /// <param name="kindy">The kind of real-to-real transform to compute along the second dimension.</param>
        /// <param name="flags">Flags that specify the behavior of the planner.</param>
        /// <returns>The FFTW plan.</returns>
        public static Plan Create2(int nx, int ny, RealArray input, RealArray output, Transform kindx, Transform kindy, Options flags)
        {
            mutex.WaitOne();
            var handle = NativeMethods.fftwf_plan_r2r_2d(nx, ny, input.Handle, output.Handle, kindx, kindy, flags);
            mutex.ReleaseMutex();

            return new Plan(handle, input, output, false);
        }

        #endregion

        #region 3D plan creation

        /// <summary>
        /// Create a 3D complex transform plan (fftwf_plan_dft_3d).
        /// </summary>
        /// <param name="nx">The logical size of the transform along the first dimension.</param>
        /// <param name="ny">The logical size of the transform along the second dimension.</param>
        /// <param name="nz">The logical size of the transform along the third dimension.</param>
        /// <param name="input">FFTW array of 8-byte complex numbers.</param>
        /// <param name="output">FFTW array of 8-byte complex numbers.</param>
        /// <param name="direction">Specifies the direction of the transform.</param>
        /// <param name="flags">Flags that specify the behavior of the planner.</param>
        /// <returns>The FFTW plan.</returns>
        public static Plan Create3(int nx, int ny, int nz, ComplexArray input, ComplexArray output, Direction direction, Options flags)
        {
            mutex.WaitOne();
            var handle = NativeMethods.fftwf_plan_dft_3d(nx, ny, nz, input.Handle, output.Handle, direction, flags);
            mutex.ReleaseMutex();

            return new Plan(handle, input, output, false);
        }

        /// <summary>
        /// Create a 3D real to complex transform plan (fftwf_plan_dft_r2c_3d).
        /// </summary>
        /// <param name="nx">The logical size of the transform along the first dimension.</param>
        /// <param name="ny">The logical size of the transform along the second dimension.</param>
        /// <param name="nz">The logical size of the transform along the third dimension.</param>
        /// <param name="input">FFTW array of 4-byte real numbers.</param>
        /// <param name="output">FFTW array of 8-byte complex numbers.</param>
        /// <param name="flags">Flags that specify the behavior of the planner.</param>
        /// <returns>The FFTW plan.</returns>
        public static Plan Create3(int nx, int ny, int nz, RealArray input, ComplexArray output, Options flags)
        {
            mutex.WaitOne();
            var handle = NativeMethods.fftwf_plan_dft_r2c_3d(nx, ny, nz, input.Handle, output.Handle, flags);
            mutex.ReleaseMutex();

            return new Plan(handle, input, output, false);
        }

        /// <summary>
        /// Create a 3D complex to real transform plan (fftwf_plan_dft_c2r_3d).
        /// </summary>
        /// <param name="nx">The logical size of the transform along the first dimension.</param>
        /// <param name="ny">The logical size of the transform along the second dimension.</param>
        /// <param name="nz">The logical size of the transform along the third dimension.</param>
        /// <param name="input">FFTW array of 8-byte complex numbers.</param>
        /// <param name="output">FFTW array of 4-byte real numbers.</param>
        /// <param name="flags">Flags that specify the behavior of the planner.</param>
        /// <returns>The FFTW plan.</returns>
        public static Plan Create3(int nx, int ny, int nz, ComplexArray input, RealArray output, Options flags)
        {
            mutex.WaitOne();
            var handle = NativeMethods.fftwf_plan_dft_c2r_3d(nx, ny, nz, input.Handle, output.Handle, flags);
            mutex.ReleaseMutex();

            return new Plan(handle, input, output, false);
        }

        /// <summary>
        /// Create a 3D real transform plan  (fftwf_plan_r2r_3d).
        /// </summary>
        /// <param name="nx">The logical size of the transform along the first dimension.</param>
        /// <param name="ny">The logical size of the transform along the second dimension.</param>
        /// <param name="nz">The logical size of the transform along the third dimension.</param>
        /// <param name="input">FFTW array of 4-byte real numbers.</param>
        /// <param name="output">FFTW array of 4-byte real numbers.</param>
        /// <param name="kindx">The kind of real-to-real transform to compute along the first dimension.</param>
        /// <param name="kindy">The kind of real-to-real transform to compute along the second dimension.</param>
        /// <param name="kindz">The kind of real-to-real transform to compute along the third dimension.</param>
        /// <param name="flags">Flags that specify the behavior of the planner.</param>
        /// <returns>The FFTW plan.</returns>
        public static Plan Create3(int nx, int ny, int nz, RealArray input, RealArray output,
            Transform kindx, Transform kindy, Transform kindz, Options flags)
        {
            mutex.WaitOne();
            var handle = NativeMethods.fftwf_plan_r2r_3d(nx, ny, nz, input.Handle, output.Handle,
                kindx, kindy, kindz, flags);
            mutex.ReleaseMutex();

            return new Plan(handle, input, output, false);
        }

        #endregion

        #region General plan creation

        /// <summary>
        /// Create a complex transform plan (fftwf_plan_dft).
        /// </summary>
        /// <param name="rank">Number of dimensions.</param>
        /// <param name="n">Array containing the logical size along each dimension.</param>
        /// <param name="input">FFTW array of 8-byte complex numbers.</param>
        /// <param name="output">FFTW array of 8-byte complex numbers.</param>
        /// <param name="direction"></param>
        /// <param name="flags">Flags that specify the behavior of the planner.</param>
        /// <returns>The FFTW plan.</returns>
        public static Plan Create(int rank, int[] n, ComplexArray input, ComplexArray output, Direction direction, Options flags)
        {
            mutex.WaitOne();
            var handle = NativeMethods.fftwf_plan_dft(rank, n, input.Handle, output.Handle, direction, flags);
            mutex.ReleaseMutex();

            return new Plan(handle, input, output, false);
        }

        /// <summary>
        /// Create a real to complex transform plan (fftwf_plan_dft_r2c).
        /// </summary>
        /// <param name="rank">Number of dimensions.</param>
        /// <param name="n">Array containing the logical size along each dimension.</param>
        /// <param name="input">FFTW array of 4-byte real numbers.</param>
        /// <param name="output">FFTW array of 8-byte complex numbers.</param>
        /// <param name="flags">Flags that specify the behavior of the planner.</param>
        /// <returns>The FFTW plan.</returns>
        public static Plan Create(int rank, int[] n, RealArray input, ComplexArray output, Options flags)
        {
            mutex.WaitOne();
            var handle = NativeMethods.fftwf_plan_dft_r2c(rank, n, input.Handle, output.Handle, flags);
            mutex.ReleaseMutex();

            return new Plan(handle, input, output, false);
        }

        /// <summary>
        /// Create a complex to real transform plan (fftwf_plan_dft_c2r).
        /// </summary>
        /// <param name="rank">Number of dimensions.</param>
        /// <param name="n">Array containing the logical size along each dimension.</param>
        /// <param name="input">FFTW array of 8-byte complex numbers.</param>
        /// <param name="output">FFTW array of 4-byte real numbers.</param>
        /// <param name="flags">Flags that specify the behavior of the planner.</param>
        /// <returns>The FFTW plan.</returns>
        public static Plan Create(int rank, int[] n, ComplexArray input, RealArray output, Options flags)
        {
            mutex.WaitOne();
            var handle = NativeMethods.fftwf_plan_dft_c2r(rank, n, input.Handle, output.Handle, flags);
            mutex.ReleaseMutex();

            return new Plan(handle, input, output, false);
        }

        /// <summary>
        /// Create a real transform plan (fftwf_plan_r2r).
        /// </summary>
        /// <param name="rank">Number of dimensions.</param>
        /// <param name="n">Array containing the logical size along each dimension.</param>
        /// <param name="input">FFTW array of 4-byte real numbers.</param>
        /// <param name="output">FFTW array of 4-byte real numbers.</param>
        /// <param name="kind">An array containing the kind of real-to-real transform to compute along each dimension.</param>
        /// <param name="flags">Flags that specify the behavior of the planner.</param>
        /// <returns>The FFTW plan.</returns>
        public static Plan Create(int rank, int[] n, RealArray input, RealArray output, Transform[] kind, Options flags)
        {
            mutex.WaitOne();
            var handle = NativeMethods.fftwf_plan_r2r(rank, n, input.Handle, output.Handle, kind, flags);
            mutex.ReleaseMutex();

            return new Plan(handle, input, output, false);
        }

        #endregion
    }
}