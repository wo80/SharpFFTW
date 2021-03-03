
namespace SharpFFTW
{
    using System;
    using System.Threading;

    /// <summary>
    /// Abstract base class for FFTW plans.
    /// </summary>
    public abstract class AbstractPlan<T> : IDisposable
        where T : struct, IEquatable<T>, IFormattable
    {
        protected IntPtr handle;

        protected AbstractArray<T> input;
        protected AbstractArray<T> output;

        protected bool ownsArrays;

        protected AbstractPlan(IntPtr handle, AbstractArray<T> input,
            AbstractArray<T> output, bool ownsArrays)
        {
            this.handle = handle;

            this.input = input;
            this.output = output;

            this.ownsArrays = ownsArrays;
        }

        /// <summary>
        /// Gets the handle to the FFTW plan.
        /// </summary>
        public IntPtr Handle
        {
            get { return handle; }
        }

        /// <summary>
        /// Gets the input array associated with the FFTW plan.
        /// </summary>
        public AbstractArray<T> Input
        {
            get { return input; }
        }

        /// <summary>
        /// Gets the output array associated with the FFTW plan.
        /// </summary>
        public AbstractArray<T> Output
        {
            get { return output; }
        }

        /// <summary>
        /// Executes a FFTW plan.
        /// </summary>
        public abstract void Execute();

        #region IDisposable implementation

        protected bool hasDisposed = false;

        /// <inheritdoc />
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public abstract void Dispose(bool disposing);

        ~AbstractPlan()
        {
            Dispose(true);
        }

        #endregion
    }
}
