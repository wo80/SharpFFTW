
namespace SharpFFTW
{
    /// <summary>
    /// Kinds of real-to-real transforms
    /// </summary>
    public enum Transform : uint
    {
        /// <summary>
        /// Corresponds to an r2c DFT, but with "halfcomplex" format output.
        /// </summary>
        R2HC = 0,
        /// <summary>
        /// Corresponds to an c2r (inverse) DFT, but with "halfcomplex" format input.
        /// </summary>
        HC2R = 1,
        /// <summary>
        /// Discrete Hartley transform.
        /// </summary>
        DHT = 2,
        /// <summary>
        /// DCT even around j=0 and even around j=n-1 (DCT-I).
        /// </summary>
        REDFT00 = 3,
        /// <summary>
        /// DCT even around j=0 and odd around j=n (DCT-III, the IDCT).
        /// </summary>
        REDFT01 = 4,
        /// <summary>
        /// DCT even around j=-0.5 and even around j=n-0.5 (DCT-II, the DCT).
        /// </summary>
        REDFT10 = 5,
        /// <summary>
        /// DCT even around j=-0.5 and odd around j=n-0.5 (DCT-IV).
        /// </summary>
        REDFT11 = 6,
        /// <summary>
        /// DST odd around j=-1 and odd around j=n (DST-I).
        /// </summary>
        RODFT00 = 7,
        /// <summary>
        /// DST odd around j=-1 and even around j=n-1 (DST-III).
        /// </summary>
        RODFT01 = 8,
        /// <summary>
        /// DST odd around j=-0.5 and odd around j=n-0.5 (DST-II).
        /// </summary>
        RODFT10 = 9,
        /// <summary>
        /// DST odd around j=-0.5 and even around j=n-0.5 (DST-IV). 
        /// </summary>
        RODFT11 = 10
    }
}
