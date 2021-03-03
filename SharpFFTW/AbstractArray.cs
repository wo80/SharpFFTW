
namespace SharpFFTW
{
    using System;
    using System.Runtime.InteropServices;

    /// <summary>
    /// Abstract base class for native FFTW arrays.
    /// </summary>
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
        /// Creates a new array of complex numbers.
        /// </summary>
        /// <param name="length">Logical length of the array.</param>
        public AbstractArray(int length)
        {
            this.Length = length;
        }

        /// <summary>
        /// Copy contents of given array to native memory.
        /// </summary>
        /// <param name="source">The data to copy.</param>
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

        protected bool hasDisposed = false;

        /// <inheritdoc />
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public abstract void Dispose(bool disposing);

        ~AbstractArray()
        {
            Dispose(true);
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
