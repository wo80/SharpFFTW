
namespace SharpFFTW
{
    using System;
    using System.Threading;

    /// <summary>
    /// Abstract base class for native FFTW arrays.
    /// </summary>
    /// <typeparam name="T">Supported types are <see cref="float"/> and <see cref="double"/>.</typeparam>
    public abstract class AbstractArray<T> : IDisposable
        where T : struct, IEquatable<T>, IFormattable
    {
        /// <summary>
        /// Gets the handle to the native memory.
        /// </summary>
        public IntPtr Handle { get; protected set; }

        /// <summary>
        /// Gets the logical size of this array.
        /// </summary>
        public int Length { get; private set; }

        // Temporary data used for copying between native and managed.
        private T[] data;

        /// <summary>
        /// Creates a new <see cref="AbstractArray{T}"/>.
        /// </summary>
        /// <param name="length">Logical length of the array.</param>
        public AbstractArray(int length)
        {
            Length = length;
        }

        /// <summary>
        /// Copy <see cref="Length"/> items from the given <paramref name="source"/> array
        /// to native memory.
        /// </summary>
        /// <param name="source">The data to copy.</param>
        /// <remarks>
        /// For real valued data the <paramref name="source"/> array length must be at
        /// least <see cref="Length"/>,  for complex valued data the length must be at
        /// least <c>2 * Length</c>.
        /// </remarks>
        public abstract void Set(T[] source);

        /// <summary>
        /// Set the native memory to zeros.
        /// </summary>
        public abstract void Clear();

        /// <summary>
        /// Copy native memory to given array.
        /// </summary>
        /// <param name="target">The target array.</param>
        public abstract void CopyTo(T[] target);

        /// <summary>
        /// Returns a managed array containing a copy of the native data.
        /// </summary>
        /// <returns>Managed array.</returns>
        public abstract T[] ToArray();

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

        ~AbstractArray()
        {
            Dispose(false);
        }

        #endregion

        /// <summary>
        /// Return temporary array of given size.
        /// </summary>
        /// <param name="size">Size of the array.</param>
        /// <returns></returns>
        protected T[] GetTemporaryData(int size)
        {
            if (data == null)
            {
                data = new T[size];
            }

            return data;
        }
    }
}
