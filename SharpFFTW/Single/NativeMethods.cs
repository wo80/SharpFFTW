// The code in this file is provided courtesy of Tamas Szalay. Some functionality has been added.

using System;
using System.Runtime.InteropServices;

namespace SharpFFTW.Single
{
    /// <summary>
    /// Contains the basic interface FFTW functions for single-precision (float) operations.
    /// </summary>
    public static class NativeMethods
    {
        private const string Library = "fftw3f";

        /// <summary>
        /// Allocates FFTW-optimized unmanaged memory.
        /// </summary>
        /// <param name="length">Amount to allocate, in bytes.</param>
        /// <returns>Pointer to allocated memory</returns>
        [DllImport(Library,
             EntryPoint = "fftwf_malloc",
             ExactSpelling = true,
             CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr fftwf_malloc(int length);

        /// <summary>
        /// Allocates FFTW-optimized unmanaged memory.
        /// </summary>
        /// <param name="length">Amount to allocate.</param>
        /// <returns>Pointer to allocated memory</returns>
        [DllImport(Library,
             EntryPoint = "fftwf_alloc_real",
             ExactSpelling = true,
             CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr fftwf_alloc_real(int length);

        /// <summary>
        /// Allocates FFTW-optimized unmanaged memory.
        /// </summary>
        /// <param name="length">Amount to allocate.</param>
        /// <returns>Pointer to allocated memory</returns>
        [DllImport(Library,
             EntryPoint = "fftwf_alloc_complex",
             ExactSpelling = true,
             CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr fftwf_alloc_complex(int length);

        /// <summary>
        /// Deallocates memory allocated by FFTW malloc.
        /// </summary>
        /// <param name="mem">Pointer to memory to release.</param>
        [DllImport(Library,
             EntryPoint = "fftwf_free",
             ExactSpelling = true,
             CallingConvention = CallingConvention.Cdecl)]
        public static extern void fftwf_free(IntPtr mem);

        /// <summary>
        /// Deallocates an FFTW plan and all associated resources.
        /// </summary>
        /// <param name="plan">Pointer to the plan to release.</param>
        [DllImport(Library,
             EntryPoint = "fftwf_destroy_plan",
             ExactSpelling = true,
             CallingConvention = CallingConvention.Cdecl)]
        public static extern void fftwf_destroy_plan(IntPtr plan);

        /// <summary>
        /// Clears all memory used by FFTW, resets it to initial state. Does not replace destroy_plan and free
        /// </summary>
        /// <remarks>After calling fftw_cleanup, all existing plans become undefined, and you should not 
        /// attempt to execute them nor to destroy them. You can however create and execute/destroy new plans, 
        /// in which case FFTW starts accumulating wisdom information again. 
        /// fftw_cleanup does not deallocate your plans; you should still call fftw_destroy_plan for this purpose.</remarks>
        [DllImport(Library,
             EntryPoint = "fftwf_cleanup",
             ExactSpelling = true,
             CallingConvention = CallingConvention.Cdecl)]
        public static extern void fftwf_cleanup();

        /// <summary>
        /// Sets the maximum time that can be used by the planner.
        /// </summary>
        /// <param name="seconds">Maximum time, in seconds.</param>
        /// <remarks>This function instructs FFTW to spend at most seconds seconds (approximately) in the planner. 
        /// If seconds == -1.0 (the default value), then planning time is unbounded. 
        /// Otherwise, FFTW plans with a progressively wider range of algorithms until the the given time limit is 
        /// reached or the given range of algorithms is explored, returning the best available plan. For example, 
        /// specifying fftw_flags.Patient first plans in Estimate mode, then in Measure mode, then finally (time 
        /// permitting) in Patient. If fftw_flags.Exhaustive is specified instead, the planner will further progress to 
        /// Exhaustive mode. 
        /// </remarks>
        [DllImport(Library,
             EntryPoint = "fftwf_set_timelimit",
             ExactSpelling = true,
             CallingConvention = CallingConvention.Cdecl)]
        public static extern void fftwf_set_timelimit(double seconds);

        /// <summary>
        /// Executes an FFTW plan, provided that the input and output arrays still exist
        /// </summary>
        /// <param name="plan">Pointer to the plan to execute.</param>
        /// <remarks>execute (and equivalents) is the only function in FFTW guaranteed to be thread-safe.</remarks>
        [DllImport(Library,
             EntryPoint = "fftwf_execute",
             ExactSpelling = true,
             CallingConvention = CallingConvention.Cdecl)]
        public static extern void fftwf_execute(IntPtr plan);

        /// <summary>
        /// Creates a plan for a 1-dimensional complex-to-complex DFT.
        /// </summary>
        /// <param name="n">The logical size of the transform.</param>
        /// <param name="input">Pointer to an array of 8-byte complex numbers.</param>
        /// <param name="output">Pointer to an array of 8-byte complex numbers.</param>
        /// <param name="direction">Specifies the direction of the transform.</param>
        /// <param name="flags">Flags that specify the behavior of the planner.</param>
        [DllImport(Library,
             EntryPoint = "fftwf_plan_dft_1d",
             ExactSpelling = true,
             CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr fftwf_plan_dft_1d(int n, IntPtr input, IntPtr output,
            Direction direction, Options flags);

        /// <summary>
        /// Creates a plan for a 2-dimensional complex-to-complex DFT.
        /// </summary>
        /// <param name="nx">The logical size of the transform along the first dimension.</param>
        /// <param name="ny">The logical size of the transform along the second dimension.</param>
        /// <param name="input">Pointer to an array of 8-byte complex numbers.</param>
        /// <param name="output">Pointer to an array of 8-byte complex numbers.</param>
        /// <param name="direction">Specifies the direction of the transform.</param>
        /// <param name="flags">Flags that specify the behavior of the planner.</param>
        [DllImport(Library,
             EntryPoint = "fftwf_plan_dft_2d",
             ExactSpelling = true,
             CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr fftwf_plan_dft_2d(int nx, int ny, IntPtr input, IntPtr output,
            Direction direction, Options flags);

        /// <summary>
        /// Creates a plan for a 3-dimensional complex-to-complex DFT.
        /// </summary>
        /// <param name="nx">The logical size of the transform along the first dimension.</param>
        /// <param name="ny">The logical size of the transform along the second dimension.</param>
        /// <param name="nz">The logical size of the transform along the third dimension.</param>
        /// <param name="input">Pointer to an array of 8-byte complex numbers.</param>
        /// <param name="output">Pointer to an array of 8-byte complex numbers.</param>
        /// <param name="direction">Specifies the direction of the transform.</param>
        /// <param name="flags">Flags that specify the behavior of the planner.</param>
        [DllImport(Library,
             EntryPoint = "fftwf_plan_dft_3d",
             ExactSpelling = true,
             CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr fftwf_plan_dft_3d(int nx, int ny, int nz, IntPtr input, IntPtr output,
            Direction direction, Options flags);

        /// <summary>
        /// Creates a plan for an n-dimensional complex-to-complex DFT.
        /// </summary>
        /// <param name="rank">Number of dimensions.</param>
        /// <param name="n">Array containing the logical size along each dimension.</param>
        /// <param name="input">Pointer to an array of 8-byte complex numbers.</param>
        /// <param name="output">Pointer to an array of 8-byte complex numbers.</param>
        /// <param name="direction">Specifies the direction of the transform.</param>
        /// <param name="flags">Flags that specify the behavior of the planner.</param>
        [DllImport(Library,
             EntryPoint = "fftwf_plan_dft",
             ExactSpelling = true,
             CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr fftwf_plan_dft(int rank, int[] n, IntPtr input, IntPtr output,
            Direction direction, Options flags);

        /// <summary>
        /// Creates a plan for a 1-dimensional real-to-complex DFT
        /// </summary>
        /// <param name="n">Number of REAL (input) elements in the transform.</param>
        /// <param name="input">Pointer to an array of 4-byte real numbers.</param>
        /// <param name="output">Pointer to an array of 8-byte complex numbers.</param>
        /// <param name="flags">Flags that specify the behavior of the planner.</param>
        [DllImport(Library,
             EntryPoint = "fftwf_plan_dft_r2c_1d",
             ExactSpelling = true,
             CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr fftwf_plan_dft_r2c_1d(int n, IntPtr input, IntPtr output, Options flags);

        /// <summary>
        /// Creates a plan for a 2-dimensional real-to-complex DFT
        /// </summary>
        /// <param name="nx">Number of REAL (input) elements in the transform along the first dimension.</param>
        /// <param name="ny">Number of REAL (input) elements in the transform along the second dimension.</param>
        /// <param name="input">Pointer to an array of 4-byte real numbers.</param>
        /// <param name="output">Pointer to an array of 8-byte complex numbers.</param>
        /// <param name="flags">Flags that specify the behavior of the planner.</param>
        [DllImport(Library,
             EntryPoint = "fftwf_plan_dft_r2c_2d",
             ExactSpelling = true,
             CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr fftwf_plan_dft_r2c_2d(int nx, int ny, IntPtr input, IntPtr output, Options flags);

        /// <summary>
        /// Creates a plan for a 3-dimensional real-to-complex DFT
        /// </summary>
        /// <param name="nx">Number of REAL (input) elements in the transform along the first dimension.</param>
        /// <param name="ny">Number of REAL (input) elements in the transform along the second dimension.</param>
        /// <param name="nz">Number of REAL (input) elements in the transform along the third dimension.</param>
        /// <param name="input">Pointer to an array of 4-byte real numbers.</param>
        /// <param name="output">Pointer to an array of 8-byte complex numbers.</param>
        /// <param name="flags">Flags that specify the behavior of the planner.</param>
        [DllImport(Library,
             EntryPoint = "fftwf_plan_dft_r2c_3d",
             ExactSpelling = true,
             CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr fftwf_plan_dft_r2c_3d(int nx, int ny, int nz, IntPtr input, IntPtr output, Options flags);

        /// <summary>
        /// Creates a plan for an n-dimensional real-to-complex DFT
        /// </summary>
        /// <param name="rank">Number of dimensions.</param>
        /// <param name="n">Array containing the number of REAL (input) elements along each dimension.</param>
        /// <param name="input">Pointer to an array of 4-byte real numbers.</param>
        /// <param name="output">Pointer to an array of 8-byte complex numbers.</param>
        /// <param name="flags">Flags that specify the behavior of the planner.</param>
        [DllImport(Library,
             EntryPoint = "fftwf_plan_dft_r2c",
             ExactSpelling = true,
             CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr fftwf_plan_dft_r2c(int rank, int[] n, IntPtr input, IntPtr output, Options flags);

        /// <summary>
        /// Creates a plan for a 1-dimensional complex-to-real DFT
        /// </summary>
        /// <param name="n">Number of REAL (output) elements in the transform.</param>
        /// <param name="input">Pointer to an array of 8-byte complex numbers.</param>
        /// <param name="output">Pointer to an array of 4-byte real numbers.</param>
        /// <param name="flags">Flags that specify the behavior of the planner.</param>
        [DllImport(Library,
             EntryPoint = "fftwf_plan_dft_c2r_1d",
             ExactSpelling = true,
             CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr fftwf_plan_dft_c2r_1d(int n, IntPtr input, IntPtr output, Options flags);

        /// <summary>
        /// Creates a plan for a 2-dimensional complex-to-real DFT
        /// </summary>
        /// <param name="nx">Number of REAL (output) elements in the transform along the first dimension.</param>
        /// <param name="ny">Number of REAL (output) elements in the transform along the second dimension.</param>
        /// <param name="input">Pointer to an array of 8-byte complex numbers.</param>
        /// <param name="output">Pointer to an array of 4-byte real numbers.</param>
        /// <param name="flags">Flags that specify the behavior of the planner.</param>
        [DllImport(Library,
             EntryPoint = "fftwf_plan_dft_c2r_2d",
             ExactSpelling = true,
             CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr fftwf_plan_dft_c2r_2d(int nx, int ny, IntPtr input, IntPtr output, Options flags);

        /// <summary>
        /// Creates a plan for a 3-dimensional complex-to-real DFT
        /// </summary>
        /// <param name="nx">Number of REAL (output) elements in the transform along the first dimension.</param>
        /// <param name="ny">Number of REAL (output) elements in the transform along the second dimension.</param>
        /// <param name="nz">Number of REAL (output) elements in the transform along the third dimension.</param>
        /// <param name="input">Pointer to an array of 8-byte complex numbers.</param>
        /// <param name="output">Pointer to an array of 4-byte real numbers.</param>
        /// <param name="flags">Flags that specify the behavior of the planner.</param>
        [DllImport(Library,
             EntryPoint = "fftwf_plan_dft_c2r_3d",
             ExactSpelling = true,
             CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr fftwf_plan_dft_c2r_3d(int nx, int ny, int nz, IntPtr input, IntPtr output, Options flags);

        /// <summary>
        /// Creates a plan for an n-dimensional complex-to-real DFT
        /// </summary>
        /// <param name="rank">Number of dimensions.</param>
        /// <param name="n">Array containing the number of REAL (output) elements along each dimension.</param>
        /// <param name="input">Pointer to an array of 8-byte complex numbers.</param>
        /// <param name="output">Pointer to an array of 4-byte real numbers.</param>
        /// <param name="flags">Flags that specify the behavior of the planner.</param>
        [DllImport(Library,
             EntryPoint = "fftwf_plan_dft_c2r",
             ExactSpelling = true,
             CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr fftwf_plan_dft_c2r(int rank, int[] n, IntPtr input, IntPtr output, Options flags);

        /// <summary>
        /// Creates a plan for a 1-dimensional real-to-real DFT
        /// </summary>
        /// <param name="n">Number of elements in the transform.</param>
        /// <param name="input">Pointer to an array of 4-byte real numbers.</param>
        /// <param name="output">Pointer to an array of 4-byte real numbers.</param>
        /// <param name="kind">The kind of real-to-real transform to compute.</param>
        /// <param name="flags">Flags that specify the behavior of the planner.</param>
        [DllImport(Library,
             EntryPoint = "fftwf_plan_r2r_1d",
             ExactSpelling = true,
             CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr fftwf_plan_r2r_1d(int n, IntPtr input, IntPtr output, Transform kind, Options flags);

        /// <summary>
        /// Creates a plan for a 2-dimensional real-to-real DFT
        /// </summary>
        /// <param name="nx">Number of elements in the transform along the first dimension.</param>
        /// <param name="ny">Number of elements in the transform along the second dimension.</param>
        /// <param name="input">Pointer to an array of 4-byte real numbers.</param>
        /// <param name="output">Pointer to an array of 4-byte real numbers.</param>
        /// <param name="kindx">The kind of real-to-real transform to compute along the first dimension.</param>
        /// <param name="kindy">The kind of real-to-real transform to compute along the second dimension.</param>
        /// <param name="flags">Flags that specify the behavior of the planner.</param>
        [DllImport(Library,
             EntryPoint = "fftwf_plan_r2r_2d",
             ExactSpelling = true,
             CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr fftwf_plan_r2r_2d(int nx, int ny, IntPtr input, IntPtr output,
            Transform kindx, Transform kindy, Options flags);

        /// <summary>
        /// Creates a plan for a 3-dimensional real-to-real DFT
        /// </summary>
        /// <param name="nx">Number of elements in the transform along the first dimension.</param>
        /// <param name="ny">Number of elements in the transform along the second dimension.</param>
        /// <param name="nz">Number of elements in the transform along the third dimension.</param>
        /// <param name="input">Pointer to an array of 4-byte real numbers.</param>
        /// <param name="output">Pointer to an array of 4-byte real numbers.</param>
        /// <param name="kindx">The kind of real-to-real transform to compute along the first dimension.</param>
        /// <param name="kindy">The kind of real-to-real transform to compute along the second dimension.</param>
        /// <param name="kindz">The kind of real-to-real transform to compute along the third dimension.</param>
        /// <param name="flags">Flags that specify the behavior of the planner.</param>
        [DllImport(Library,
             EntryPoint = "fftwf_plan_r2r_3d",
             ExactSpelling = true,
             CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr fftwf_plan_r2r_3d(int nx, int ny, int nz, IntPtr input, IntPtr output,
            Transform kindx, Transform kindy, Transform kindz, Options flags);

        /// <summary>
        /// Creates a plan for an n-dimensional real-to-real DFT
        /// </summary>
        /// <param name="rank">Number of dimensions.</param>
        /// <param name="n">Array containing the number of elements in the transform along each dimension.</param>
        /// <param name="input">Pointer to an array of 4-byte real numbers.</param>
        /// <param name="output">Pointer to an array of 4-byte real numbers.</param>
        /// <param name="kind">An array containing the kind of real-to-real transform to compute along each dimension.</param>
        /// <param name="flags">Flags that specify the behavior of the planner.</param>
        [DllImport(Library,
             EntryPoint = "fftwf_plan_r2r",
             ExactSpelling = true,
             CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr fftwf_plan_r2r(int rank, int[] n, IntPtr input, IntPtr output,
            Transform[] kind, Options flags);

#if USE_THREADS
        /// <summary>
        /// Perform any one-time initialization required to use threads.
        /// </summary>
        /// <returns>Returns zero if there was some error and a non-zero value otherwise.</returns>
        [DllImport(Library,
             EntryPoint = "fftwf_init_threads",
             ExactSpelling = true,
             CallingConvention = CallingConvention.Cdecl)]
        public static extern int fftwf_init_threads();

        /// <summary>
        /// Sets number of threads for FFTW to use.
        /// </summary>
        /// <param name="nthreads">The number of threads.</param>
        [DllImport(Library,
             EntryPoint = "fftwf_plan_with_nthreads",
             ExactSpelling = true,
             CallingConvention = CallingConvention.Cdecl)]
        public static extern void fftwf_plan_with_nthreads(int nthreads);

        /// <summary>
        /// Determines the current number of threads that the planner can use.
        /// </summary>
        /// <returns>Number of threads that the planner can use.</returns>
        [DllImport(Library,
             EntryPoint = "fftwf_planner_nthreads",
             ExactSpelling = true,
             CallingConvention = CallingConvention.Cdecl)]
        public static extern int fftwf_planner_nthreads();

        /// <summary>
        /// Cleanup all memory and other resources allocated internally by FFTW.
        /// </summary>
        /// <remarks>
        /// Much like the fftw_cleanup() function except that it also gets rid of threads-related data.
        /// You must not execute any previously created plans after calling this function.
        /// </remarks>
        [DllImport(Library,
             EntryPoint = "fftwf_cleanup_threads",
             ExactSpelling = true,
             CallingConvention = CallingConvention.Cdecl)]
        public static extern void fftwf_cleanup_threads();

        /// <summary>
        /// See http://www.fftw.org/fftw3_doc/Thread-safety.html
        /// </summary>
        [DllImport(Library,
             EntryPoint = "fftwf_make_planner_thread_safe",
             ExactSpelling = true,
             CallingConvention = CallingConvention.Cdecl)]
        public static extern void fftwf_make_planner_thread_safe();
#endif

        /// <summary>
        /// Returns (approximately) the number of flops used by a certain plan.
        /// </summary>
        /// <param name="plan">The plan to measure.</param>
        /// <param name="add">Reference to double to hold number of adds.</param>
        /// <param name="mul">Reference to double to hold number of muls.</param>
        /// <param name="fma">Reference to double to hold number of fmas (fused multiply-add).</param>
        /// <remarks>Total flops ~= add+mul+2*fma or add+mul+fma if fma is supported</remarks>
        [DllImport(Library,
             EntryPoint = "fftwf_flops",
             ExactSpelling = true,
             CallingConvention = CallingConvention.Cdecl)]
        public static extern void fftwf_flops(IntPtr plan, out double add, out double mul, out double fma);

        /// <summary>
        /// Estimate cost of given plan.
        /// </summary>
        /// <param name="plan">Pointer to the plan.</param>
        /// <returns></returns>
        [DllImport(Library,
             EntryPoint = "fftwf_estimate_cost",
             ExactSpelling = true,
             CallingConvention = CallingConvention.Cdecl)]
        public static extern double estimate_cost(IntPtr plan);

        /// <summary>
        /// Get cost of given plan.
        /// </summary>
        /// <param name="plan">Pointer to the plan.</param>
        /// <returns></returns>
        [DllImport(Library,
             EntryPoint = "fftwf_cost",
             ExactSpelling = true,
             CallingConvention = CallingConvention.Cdecl)]
        public static extern double cost(IntPtr plan);

        /// <summary>
        /// Outputs a "nerd-readable" version of the specified plan to stdout.
        /// </summary>
        /// <param name="plan">The plan to output.</param>
        [DllImport(Library,
             EntryPoint = "fftwf_print_plan",
             ExactSpelling = true,
             CallingConvention = CallingConvention.Cdecl)]
        public static extern void fftwf_print_plan(IntPtr plan);

        /// <summary>
        /// Outputs a "nerd-readable" version of the specified plan.
        /// </summary>
        /// <param name="plan">The plan to output.</param>
        [DllImport(Library,
             EntryPoint = "fftwf_sprint_plan",
             ExactSpelling = true,
             CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr fftwf_sprint_plan(IntPtr plan);

        /// <summary>
        /// Exports the accumulated Wisdom to the provided filename.
        /// </summary>
        /// <param name="filename">The target filename.</param>
        [DllImport(Library,
             EntryPoint = "fftwf_export_wisdom_to_filename",
             ExactSpelling = true,
             CallingConvention = CallingConvention.Cdecl)]
        public static extern int fftwf_export_wisdom_to_filename(string filename);

        /// <summary>
        /// Imports Wisdom from provided filename.
        /// </summary>
        /// <param name="filename">The filename to read from.</param>
        [DllImport(Library,
             EntryPoint = "fftwf_import_wisdom_from_filename",
             ExactSpelling = true,
             CallingConvention = CallingConvention.Cdecl)]
        public static extern int fftwf_import_wisdom_from_filename(string filename);
    }
}
