
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

        protected int _disposeCount;

        /// <inheritdoc />
        public void Dispose()
        {
            if (Interlocked.Increment(ref _disposeCount) == 1)
            {
                Dispose(true);
                GC.SuppressFinalize(this);
            }
        }

        /// <summary>
        /// Performs application-defined tasks associated with disposing of resources.
        /// </summary>
        /// <param name="disposing">Indicates whether the method call comes from a Dispose method
        /// (value is <c>true</c>) or from a finalizer (value is <c>false</c>).</param>
        /// <remarks>
        /// Override this method in derived classes. Unmanaged resources should always be
        /// released when this method is called. Managed resources may only be disposed
        /// of if <paramref name="disposing"/> is true.
        /// </remarks>
        public abstract void Dispose(bool disposing);

        ~AbstractPlan()
        {
            Dispose(false);
        }

        #endregion
    }
}
