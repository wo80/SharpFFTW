
namespace FFTWSharp
{
    using System;
    using System.Threading;

    /// <summary>
    /// Abstract base class for FFTW plans.
    /// </summary>
    public abstract class AbstractPlan : IDisposable
    {
        protected IntPtr handle;

        protected AbstractPlan(IntPtr handle)
        {
            this.handle = handle;
        }

        /// <summary>
        /// Gets the handle to the FFTW plan.
        /// </summary>
        public IntPtr Handle
        {
            get { return handle; }
        }

        /// <summary>
        /// Executes a FFTW plan.
        /// </summary>
        public abstract void Execute();

        #region IDisposable implementation

        protected bool hasDisposed = false;

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
