// Code to implement decently performing FFT for complex and real valued
// signals. See www.lomont.org for a derivation of the relevant algorithms 
// from first principles. Copyright Chris Lomont 2010-2012.
// This code and any ports are free for all to use for any reason as long 
// as this header is left in place.
// Version 1.1, Sept 2011
using System;

/* History:
 * Sep 2011 - v1.1 - added parameters to support various sign conventions 
 *                   set via properties A and B. 
 *                 - Removed dependencies on LINQ and generic collections. 
 *                 - Added unit tests for the new properties. 
 *                 - Switched UnitTest to static.
 * Jan 2010 - v1.0 - Initial release
 */
namespace Lomont
{
    /// <summary>
    /// Represent a class that performs real or complex valued Fast Fourier 
    /// Transforms. Instantiate it and use the FFT or TableFFT methods to 
    /// compute complex to complex FFTs. Use FFTReal for real to complex 
    /// FFTs which are much faster than standard complex to complex FFTs.
    /// Properties A and B allow selecting various FFT sign and scaling 
    /// conventions.
    /// </summary>
    public class LomontFFT
    {
        /// <summary>
        /// Compute the forward or inverse Fourier Transform of data, with 
        /// data containing complex valued data as alternating real and 
        /// imaginary parts. The length must be a power of 2. The data is 
        /// modified in place.
        /// </summary>
        /// <param name="data">The complex data stored as alternating real 
        /// and imaginary parts</param>
        /// <param name="forward">true for a forward transform, false for 
        /// inverse transform</param>
        public void FFT(double[] data, bool forward)
        {
            var n = data.Length;
            // checks n is a power of 2 in 2's complement format
            if ((n & (n - 1)) != 0) 
                throw new ArgumentException(
                    "data length " + n + " in FFT is not a power of 2");
            n /= 2;    // n is the number of samples

            Reverse(data, n); // bit index data reversal

            // do transform: so single point transforms, then doubles, etc.
            double sign = forward ? B : -B;
            var mmax = 1;
            while (n > mmax)
            {
                var istep = 2 * mmax;
                var theta = sign * Math.PI / mmax;
                double wr = 1, wi = 0;
                var wpr = Math.Cos(theta);
                var wpi = Math.Sin(theta);
                for (var m = 0; m < istep; m += 2)
                {
                    for (var k = m; k < 2 * n; k += 2 * istep)
                    {
                        var j = k + istep;
                        var tempr = wr * data[j] - wi * data[j + 1];
                        var tempi = wi * data[j] + wr * data[j + 1];
                        data[j] = data[k] - tempr;
                        data[j + 1] = data[k + 1] - tempi;
                        data[k] = data[k] + tempr;
                        data[k + 1] = data[k + 1] + tempi;
                    }
                    var t = wr; // trig recurrence
                    wr = wr * wpr - wi * wpi;
                    wi = wi * wpr + t * wpi;
                }
                mmax = istep;
            }

            // perform data scaling as needed
            Scale(data,n, forward);
        }

        /// <summary>
        /// Compute the forward or inverse Fourier Transform of data, with data
        /// containing complex valued data as alternating real and imaginary 
        /// parts. The length must be a power of 2. This method caches values 
        /// and should be slightly faster on than the FFT method for repeated uses. 
        /// It is also slightly more accurate. Data is transformed in place.
        /// </summary>
        /// <param name="data">The complex data stored as alternating real 
        /// and imaginary parts</param>
        /// <param name="forward">true for a forward transform, false for 
        /// inverse transform</param>
        public void TableFFT(double[] data, bool forward)
        {
            var n = data.Length;
            // checks n is a power of 2 in 2's complement format
            if ((n & (n - 1)) != 0) 
                throw new ArgumentException(
                    "data length " + n + " in FFT is not a power of 2"
                    );
            n /= 2;    // n is the number of samples

            Reverse(data, n); // bit index data reversal

            // make table if needed
            if ((cosTable == null) || (cosTable.Length != n))
                Initialize(n);

            // do transform: so single point transforms, then doubles, etc.
            double sign = forward ? B : -B;
            var mmax = 1;
            var tptr = 0;
            while (n > mmax)
            {
                var istep = 2 * mmax;
                for (var m = 0; m < istep; m += 2)
                {
                    var wr = cosTable[tptr];
                    var wi = sign * sinTable[tptr++];
                    for (var k = m; k < 2 * n; k += 2 * istep)
                    {
                        var j = k + istep;
                        var tempr = wr * data[j] - wi * data[j + 1];
                        var tempi = wi * data[j] + wr * data[j + 1];
                        data[j] = data[k] - tempr;
                        data[j + 1] = data[k + 1] - tempi;
                        data[k] = data[k] + tempr;
                        data[k + 1] = data[k + 1] + tempi;
                    }
                }
                mmax = istep;
            }


            // perform data scaling as needed
            Scale(data, n, forward);
        }

        /// <summary>
        /// Compute the forward or inverse Fourier Transform of data, with 
        /// data containing real valued data only. The output is complex 
        /// valued after the first two entries, stored in alternating real 
        /// and imaginary parts. The first two returned entries are the real 
        /// parts of the first and last value from the conjugate symmetric 
        /// output, which are necessarily real. The length must be a power 
        /// of 2.
        /// </summary>
        /// <param name="data">The complex data stored as alternating real 
        /// and imaginary parts</param>
        /// <param name="forward">true for a forward transform, false for 
        /// inverse transform</param>
        public void RealFFT(double[] data, bool forward)
        {
            
            var n = data.Length; // # of real inputs, 1/2 the complex length
            // checks n is a power of 2 in 2's complement format
            if ((n & (n - 1)) != 0)
                throw new ArgumentException(
                    "data length " + n + " in FFT is not a power of 2"
                    );

            var sign = -1.0; // assume inverse FFT, this controls how algebra below works
            if (forward)
            { // do packed FFT. This can be changed to FFT to save memory
                TableFFT(data, true);
                sign = 1.0;
                // scaling - divide by scaling for N/2, then mult by scaling for N
                if (A != 1)
                {
                    var scale = Math.Pow(2.0, (A - 1) / 2.0);
                    for (var i = 0; i < data.Length; ++i)
                        data[i] *= scale;
                }
            }

            var theta = B * sign * 2 * Math.PI / n;
            var wpr = Math.Cos(theta);
            var wpi = Math.Sin(theta);
            var wjr = wpr;
            var wji = wpi;

            for (var j = 1; j <= n/4; ++j)
            {
                var k = n / 2 - j;
                var tkr = data[2 * k];    // real and imaginary parts of t_k  = t_(n/2 - j)
                var tki = data[2 * k + 1];
                var tjr = data[2 * j];    // real and imaginary parts of t_j
                var tji = data[2 * j + 1];

                var a = (tjr - tkr) * wji;
                var b = (tji + tki) * wjr;
                var c = (tjr - tkr) * wjr;
                var d = (tji + tki) * wji;
                var e = (tjr + tkr);
                var f = (tji - tki);

                // compute entry y[j]
                data[2 * j] = 0.5 * (e + sign * (a + b));
                data[2 * j + 1] = 0.5 * (f + sign * (d - c));

                // compute entry y[k]
                data[2 * k] = 0.5 * (e - sign * (b + a));
                data[2 * k + 1] = 0.5 * (sign * (d - c) - f);

                var temp = wjr;
                // todo - allow more accurate version here? make option?
                wjr = wjr * wpr - wji * wpi;  
                wji = temp * wpi + wji * wpr;
            }

            if (forward)
            {
                // compute final y0 and y_{N/2}, store in data[0], data[1]
                var temp = data[0];
                data[0] += data[1];
                data[1] = temp - data[1];
            }
            else
            {
                var temp = data[0]; // unpack the y0 and y_{N/2}, then invert FFT
                data[0] = 0.5 * (temp + data[1]);
                data[1] = 0.5 * (temp - data[1]);
                // do packed inverse (table based) FFT. This can be changed to regular inverse FFT to save memory
                TableFFT(data, false);
                // scaling - divide by scaling for N, then mult by scaling for N/2
                //if (A != -1) // todo - off by factor of 2? this works, but something seems weird
                {
                    var scale = Math.Pow(2.0, -(A + 1) / 2.0)*2; 
                    for (var i = 0; i < data.Length; ++i)
                        data[i] *= scale;
                }
            }
        }

        /// <summary>
        /// Determine how scaling works on the forward and inverse transforms.
        /// For size N=2^n transforms, the forward transform gets divided by 
        /// N^((1-a)/2) and the inverse gets divided by N^((1+a)/2). Common 
        /// values for (A,B) are 
        ///     ( 0, 1)  - default
        ///     (-1, 1)  - data processing
        ///     ( 1,-1)  - signal processing
        /// Usual values for A are 1, 0, or -1
        /// </summary>
        public int A { get; set; }

        /// <summary>
        /// Determine how phase works on the forward and inverse transforms.
        /// For size N=2^n transforms, the forward transform uses an 
        /// exp(B*2*pi/N) term and the inverse uses an exp(-B*2*pi/N) term.
        /// Common values for (A,B) are 
        ///     ( 0, 1)  - default
        ///     (-1, 1)  - data processing
        ///     ( 1,-1)  - signal processing
        /// Abs(B) should be relatively prime to N.
        /// Setting B=-1 effectively corresponds to conjugating both input and 
        /// output data.
        /// Usual values for B are 1 or -1.
        /// </summary>
        public int B { get; set; }

        public LomontFFT()
        {
            A = 0;
            B = 1;
        }

        #region Internals

        /// <summary>
        /// Scale data using n samples for forward and inverse transforms as needed 
        /// </summary>
        /// <param name="data"></param>
        /// <param name="n"></param>
        /// <param name="forward"></param>
        void Scale(double[] data, int n, bool forward)
        {
            // forward scaling if needed
            if ((forward) && (A != 1))
            {
                var scale = Math.Pow(n, (A - 1) / 2.0);
                for (var i = 0; i < data.Length; ++i)
                    data[i] *= scale;
            }

            // inverse scaling if needed
            if ((!forward) && (A != -1))
            {
                var scale = Math.Pow(n, -(A + 1) / 2.0);
                for (var i = 0; i < data.Length; ++i)
                    data[i] *= scale;
            }
        }

        /// <summary>
        /// Call this with the size before using the TableFFT version
        /// Fills in tables for speed. Done automatically in TableFFT
        /// </summary>
        /// <param name="size">The size of the FFT in samples</param>
        void Initialize(int size)
        {
            // NOTE: if you port to non garbage collected languages 
            // like C# or Java be sure to free these correctly
            cosTable = new double[size];
            sinTable = new double[size];

            // forward pass
            var n = size;
            int mmax = 1, pos = 0;
            while (n > mmax)
            {
                var istep = 2 * mmax;
                var theta = Math.PI / mmax;
                double wr = 1, wi = 0;
                var wpi = Math.Sin(theta);
                // compute in a slightly slower yet more accurate manner
                var wpr = Math.Sin(theta / 2);
                wpr = -2 * wpr * wpr; 
                for (var m = 0; m < istep; m += 2)
                {
                    cosTable[pos] = wr;
                    sinTable[pos++] = wi;
                    var t = wr;
                    wr = wr * wpr - wi * wpi + wr;
                    wi = wi * wpr + t * wpi + wi;
                }
                mmax = istep;
            }
        }

        /// <summary>
        /// Swap data indices whenever index i has binary 
        /// digits reversed from index j, where data is
        /// two doubles per index.
        /// </summary>
        /// <param name="data"></param>
        /// <param name="n"></param>
        static void Reverse(double [] data, int n)
        {
            // bit reverse the indices. This is exercise 5 in section 
            // 7.2.1.1 of Knuth's TAOCP the idea is a binary counter 
            // in k and one with bits reversed in j
            int j = 0, k = 0; // Knuth R1: initialize
            var top = n / 2;  // this is Knuth's 2^(n-1)
            while (true)
            {
                // Knuth R2: swap - swap j+1 and k+2^(n-1), 2 entries each
                var t = data[j + 2]; 
                data[j + 2] = data[k + n]; 
                data[k + n] = t;
                t = data[j + 3]; 
                data[j + 3] = data[k + n + 1]; 
                data[k + n + 1] = t;
                if (j > k)
                { // swap two more
                    // j and k
                    t = data[j]; 
                    data[j] = data[k]; 
                    data[k] = t;
                    t = data[j + 1]; 
                    data[j + 1] = data[k + 1]; 
                    data[k + 1] = t;
                    // j + top + 1 and k+top + 1
                    t = data[j + n + 2]; 
                    data[j + n + 2] = data[k + n + 2]; 
                    data[k + n + 2] = t;
                    t = data[j + n + 3]; 
                    data[j + n + 3] = data[k + n + 3]; 
                    data[k + n + 3] = t;
                }
                // Knuth R3: advance k
                k += 4;
                if (k >= n)
                    break;
                // Knuth R4: advance j
                var h = top;
                while (j >= h)
                {
                    j -= h;
                    h /= 2;
                }
                j += h;
            } // bit reverse loop
        }

        /// <summary>
        /// Pre-computed sine/cosine tables for speed
        /// </summary>
        double [] cosTable;
        double [] sinTable;
        
        #endregion 

        #region UnitTest
        /// <summary>
        /// Return true if unit tests pass
        /// </summary>
        /// <returns>true if and only if the tests all passed</returns>
        public static bool UnitTest()
        {
            #region Test Data
            // some tests of various lengths. Answers are for (A,B) types (1,1), (0,1), (1,-1), and (-1,1)
            double[] t4 = { 0.9463910302640065255, 2.1719817340114636428, 0.5408022840459105995546, 1.5560308568160938805};
            double[] t8 = {-0.00585152907659816402, -0.38747973044006503981, -0.07599531911180304592, 0.45076971245423087174, 0.26739426495460068114, -0.26125937528189408420, 0.41416072137518122013, -0.01641988546867230817};
            double[] t16 = {1.315477046808896429519, 1.207210985661466858068, 0.121056790034505835908, -0.25379277220838383483, 0.913644534255553004335, 0.927677591320202188087, 2.235664368076910734991, -0.375686965597889518826, 2.301831908175541205636, 1.06141993445964001079, -0.06885421111166161113, 0.537848193477814512702, 1.776371630015752925985, -0.337293413841513571577, 2.476154181731257139688, -0.246423441467474887253};
            double[] t32 = { 0.43456805587961525780, -0.06380054971262695336, -0.488558284014200504051, -0.416304883965628375255, -0.469871270992736403590, -0.25166411067679778788, 0.40215502170455820014, -0.35830084570211135293, -0.05852687773965932269, 0.06837143745059148373, 0.39433540724645248849, -0.24284994799967130717, 0.35674139694999335405, -0.12888862317683722326, -0.483481846064623712881, 0.31470624594663715993, -0.36261847505420561595, 0.45600311160472028856, -0.32101981739601821164, -0.39660499028915120615, 0.12690816916754634235, -0.15432871988088497073, -0.20789746477910645318, 0.11641012713093763133, 0.15442520359906394183, 0.45398815306631095852, 0.26643905565987603855, -0.443374926998731886764, 0.34684336103208890521, 0.11453023518323556461, -0.24188480129115013822, -0.37645566690319011335};
            double[][] a4R = {new[]{5.215205905137474648, -2.240819276517640398, 0.405588746218095926, 0.615950877195369762}, new[]{2.607602952568737324, -1.120409638258820199,0.2027943731090479630, 0.307975438597684881}, new[]{5.215205905137474648, -2.240819276517640398,0.405588746218095926, -0.615950877195369762}, new[]{1.303801476284368662, -0.560204819129410100, 0.1013971865545239815, 0.153987719298842441}};
            double[][] a4C = {new[]{1.487193314309917125, 3.728012590827557523, 0.405588746218095926,0.615950877195369762}, new[]{1.051604477483838966, 2.636102983322995846,0.286794552823765311, 0.435543042142648332}, new[]{1.487193314309917125,3.728012590827557523, 0.405588746218095926,0.615950877195369762}, new[]{0.743596657154958563, 1.864006295413778762,0.2027943731090479630, 0.307975438597684881}};
            double[][] a8R = {new[]{0.38531885940498013,0.81409741687778125, -0.69284999587841737, -0.24905437675199995,-0.07662266638537566, -1.08308893270751769, 0.14635840781601968,0.73125770422196858}, new[]{0.13623078920216368,0.28782690201036541, -0.24495946521535021, -0.08805401934276420,-0.027090203496846828, -0.38292976447279300, 0.05174551132518686,0.25853864072513031}, new[]{0.38531885940498013,0.81409741687778125, -0.69284999587841737,0.24905437675199995, -0.07662266638537566, 1.08308893270751769,0.14635840781601968, -0.73125770422196858}, new[]{0.048164857425622516,0.101762177109722656, -0.086606249484802171, -0.031131797093999994,-0.009577833298171957, -0.135386116588439711, 0.018294800977002460,0.091407213027746072}};
            double[][] a8C = {new[]{0.5997081381413806913, -0.2143892787364005604,-0.7404353919541020251, -0.6163763956451552217,-0.0766226663853756571, -1.0830889327075176876, 0.1939438038917043347,0.3639356853288133104}, new[]{0.2998540690706903457,-0.1071946393682002802, -0.3702176959770510125,-0.3081881978225776108, -0.0383113331926878286,-0.5415444663537588438, 0.0969719019458521674,0.1819678426644066552}, new[]{0.5997081381413806913,-0.2143892787364005604, 0.1939438038917043347,0.3639356853288133104, -0.0766226663853756571,-1.0830889327075176876, -0.7404353919541020251,-0.6163763956451552217}, new[]{0.1499270345353451728,-0.05359731968410014011, -0.1851088479885255063,-0.1540940989112888054, -0.01915566659634391428,-0.2707722331768794219, 0.04848595097292608369,0.09098392133220332761}};
            double[][] t16R = {new[]{13.59230635979061742,8.55038613618289391, -1.21492956216715601, -0.45486697068671133,1.47323644084476175, -2.83216031656357654, 0.71462633901964782,0.66109635428761313, 1.54330399052473147,3.197070083395729214, -3.29601473130025480, -0.99282859461499410,0.38134914058150166, 6.48707162520707076, -0.14910149101881611,1.34211646345148113}, new[]{3.39807658994765436,2.137596534045723477, -0.303732390541789004, -0.113716742671677833,0.368309110211190438, -0.708040079140894134, 0.178656584754911956,0.165274088571903281, 0.385825997631182867,0.799267520848932303, -0.82400368282506370, -0.248207148653748524,0.095337285145375415, 1.62176790630176769, -0.037275372754704028,0.335529115862870283}, new[]{13.59230635979061742,8.55038613618289391, -1.21492956216715601, 0.45486697068671133,1.47323644084476175, 2.83216031656357654,0.71462633901964782, -0.66109635428761313,1.54330399052473147, -3.197070083395729214, -3.29601473130025480,0.99282859461499410,0.38134914058150166, -6.48707162520707076, -0.14910149101881611,-1.34211646345148113}, new[]{0.849519147486913589,0.534399133511430869, -0.075933097635447251, -0.028429185667919458,0.092077277552797609, -0.177010019785223534, 0.044664146188727989,0.041318522142975820, 0.096456499407795717,0.1998168802122330758, -0.206000920706265925, -0.062051787163437131,0.023834321286343854, 0.405441976575441923, -0.009318843188676007,0.083882278965717571}};
            double[][] t16C = {new[]{11.071346247986755665, 2.520960111803861757, -1.295808722180236886, -1.221072246496360113, 0.021126962378336621, -2.981369228242905398, 0.625454618595877207, 1.441125106282221185, 1.543303990524731466, 3.197070083395729214, -3.206843010876484186, -0.212799842620386036, 1.833458619047926789, 6.337862713527741902, -0.068222331005735240, 0.575911187641832353}, new[]{3.914312004407837382, 0.891293995078653829, -0.458137567287160258, -0.431714232908133866, 0.007469509181797447, -1.054073199255731085, 0.221131601066795241, 0.509514667595171310, 0.545640358566148453, 1.130334967948880571, -1.133790219595723673, -0.075236105876152529, 0.648225511276855981, 2.240772851482419665, -0.024120236441254322, 0.203615353071368931}, new[]{11.071346247986755665, 2.520960111803861757, -0.068222331005735240, 0.575911187641832353, 1.833458619047926789, 6.337862713527741902, -3.206843010876484186, -0.212799842620386036, 1.543303990524731466, 3.197070083395729214, 0.625454618595877207, 1.441125106282221185, 0.021126962378336621, -2.981369228242905398, -1.295808722180236886, -1.221072246496360113}, new[]{1.3839182809983444581, 0.3151200139754827196, -0.1619760902725296107, -0.1526340308120450141, 0.0026408702972920776, -0.3726711535303631747,0.0781818273244846509, 0.1801406382852776481, 0.1929129988155914333, 0.3996337604244661517, -0.4008553763595605232, -0.0265999803275482545, 0.2291823273809908486, 0.7922328391909677378, -0.0085277913757169050, 0.0719888984552290441}};
            double[][] a32R = {new[]{-1.46000712101570392, 1.15712078883069226, -0.56341032896397069, -0.97348179246515461, -1.56634631955235446, -2.03358236992303745, 0.10016779686778672, -0.28982229431706279, 1.59649489283936457, 0.48088940342020419, 0.61889994518995724, -0.94638900654944842, 1.44036681871883545, 0.56879809230684088, -0.40555236689846115, 1.76515791462870041, 1.208382291775918752, 2.296985822638620811, 0.63608517570432352, -0.54113478089643728, 1.89167325112076917, -0.74545011136225907, 2.66767101857698704, -0.02967504673446225, -1.98204239178352045, -0.28372150043265964, 1.51795039241265502, 1.43503462291130686, -1.86148873042323007, 0.83836086563695189, 1.80568061458128929, -0.76801487986712270}, new[]{-0.258095233962713142, 0.204551989108527394, -0.099597816050241796, -0.172088894203436537, -0.276893526060515187, -0.359489970968497496, 0.017707332105432151, -0.051233827412659624, 0.282223091214101301, 0.085010039539794926, 0.109407087029950338, -0.167299521042878729, 0.254623286228045786, 0.100550247049034749, -0.071692207190039153, 0.312038782824764749, 0.213613828195123352, 0.406053562869282277, 0.112445035288190932, -0.095660018276941854, 0.334403745914174673, -0.131778207195130101, 0.471582066802652984, -0.005245856694491492, -0.350378903957332773, -0.050155349231088902, 0.268338253994942301, 0.253680678274516299, -0.329067826096150775, 0.148202663293328177, 0.319202251806880595, -0.135767132401553529},new[] {-1.46000712101570392,1.15712078883069226, -0.56341032896397069, 0.97348179246515461, -1.56634631955235446, 2.03358236992303745, 0.10016779686778672, 0.28982229431706279, 1.59649489283936457, -0.48088940342020419, 0.61889994518995724, 0.94638900654944842, 1.44036681871883545, -0.56879809230684088, -0.40555236689846115, -1.76515791462870041, 1.208382291775918752, -2.296985822638620811, 0.63608517570432352, 0.54113478089643728, 1.89167325112076917, 0.74545011136225907, 2.66767101857698704, 0.02967504673446225, -1.98204239178352045, 0.28372150043265964, 1.51795039241265502, -1.43503462291130686, -1.86148873042323007, -0.83836086563695189, 1.80568061458128929, 0.76801487986712270}, new[]{-0.0456252225317407476, 0.0361600246509591330, -0.017606572780124084, -0.030421306014536081, -0.048948322486011077, -0.063549449060094920, 0.003130243652118335, -0.009056946697408212, 0.0498904654012301429, 0.0150277938568813810, 0.019340623287186164, -0.029574656454670263, 0.045011463084963608, 0.017774940384588778, -0.012673511465576911, 0.055161184832146888, 0.0377619466179974610, 0.0717808069574569003, 0.019877661740760110, -0.016910461903013665, 0.059114789097524036, -0.023295315980070596, 0.083364719330530845, -0.000927345210451945, -0.0619388247432350139, -0.0088662968885206138, 0.047435949762895469, 0.044844831965978339, -0.058171522825725940, 0.026198777051154746, 0.056427519205665290, -0.024000464995847585}};
            double[][] a32C = {new[]{-0.151443166092505834, -1.308563954923198090, 1.244058936325143007, -1.434392792279571653, -1.105324127259872307, -1.528329335338557865, -0.060884434278954178, -1.13372709101066608, 1.002720860253378163, 1.717218822886712110, 1.062676103603080428, -1.433258891213431473, 1.49134454751956007, 0.48916776214349496, -0.51494251390467968, 1.651791582176155218, 1.208382291775918752, 2.296985822638620811, 0.745475322710542044, -0.654501113348982475, 1.840695522320044552, -0.825080441525604990, 2.223894860163863861, -0.51654493139844530, -1.388268359197534037, 0.952607919033848279, 1.679002623559395916, 0.591129826217703575, -2.32251092271571222, 1.34361390022143147, -0.00178865070782441, -1.228925879681539749}, new[]{-0.037860791523126458, -0.327140988730799523, 0.311014734081285752, -0.358598198069892913, -0.276331031814968077, -0.382082333834639466, -0.015221108569738544, -0.283431772752666519, 0.250680215063344541, 0.429304705721678028, 0.265669025900770107, -0.358314722803357868, 0.372836136879890017, 0.122291940535873739, -0.128735628476169919, 0.412947895544038804, 0.302095572943979688, 0.574246455659655203, 0.186368830677635511, -0.163625278337245619, 0.460173880580011138, -0.206270110381401247, 0.555973715040965965, -0.129136232849611326, -0.347067089799383509, 0.238151979758462070, 0.419750655889848979, 0.147782456554425894, -0.580627730678928055, 0.335903475055357867, -0.000447162676956103, -0.307231469920384937}, new[]{-0.151443166092505834, -1.308563954923198090, -0.001788650707824411, -1.228925879681539749, -2.322510922715712221, 1.343613900221431469, 1.679002623559395916, 0.591129826217703575, -1.388268359197534037, 0.952607919033848279, 2.223894860163863861, -0.516544931398445303, 1.840695522320044552, -0.825080441525604990, 0.745475322710542044, -0.654501113348982475, 1.208382291775918752, 2.296985822638620811, -0.514942513904679676, 1.651791582176155218, 1.491344547519560067, 0.489167762143494958, 1.062676103603080428, -1.433258891213431473, 1.002720860253378163, 1.717218822886712110, -0.060884434278954178, -1.133727091010666075, -1.105324127259872307, -1.528329335338557865, 1.244058936325143007, -1.434392792279571653}, new[]{-0.0094651978807816146, -0.0817852471826998806, 0.0777536835203214379, -0.0896495495174732283, -0.0690827579537420192, -0.0955205834586598666, -0.0038052771424346361, -0.0708579431881666297, 0.0626700537658361352,0.1073261764304195069, 0.0664172564751925267, -0.0895786807008394671, 0.0932090342199725042, 0.0305729851339684349, -0.0321839071190424798, 0.1032369738860097011, 0.0755238932359949220, 0.1435616139149138007,0.0465922076694088777, -0.0409063195843114047, 0.1150434701450027845, -0.0515675275953503119, 0.1389934287602414913, -0.0322840582124028314, -0.0867667724498458773, 0.0595379949396155175, 0.1049376639724622447, 0.0369456141386064734, -0.1451569326697320138, 0.0839758687638394668, -0.0001117906692390257, -0.0768078674800962343}};

            double[][] tests =  { t4,  t8,  t16,  t32};
            double[][][] ansr = { a4R, a8R, t16R, a32R};
            double[][][] ansc = { a4C, a8C, t16C, a32C};
            #endregion

            var fftObject = new LomontFFT();

            int[] ab = {1,1, 0,1, 1,-1, -1,1}; // A,B parameter pairs

            var success = true;

            // single test action makes debugging easier
            Action<int,int> testAction = (testIndex, paramIndex) =>
            {
                var test = tests[testIndex];
                fftObject.A = ab[2 * paramIndex];
                fftObject.B = ab[2 * paramIndex + 1];
                var answerReal = ansr[testIndex][paramIndex];
                var answerComplex = ansc[testIndex][paramIndex];
                success &= Test(fftObject.RealFFT, test, answerReal);
                success &= Test(fftObject.FFT, test, answerComplex);
                success &= Test(fftObject.TableFFT, test, answerComplex);
            };

            for (var testIndex = 0; testIndex < tests.Length; testIndex ++)
                for (var t = 0; t < ab.Length/2; t++)
                    testAction(testIndex, t);
            return success;
        }

        /// <summary>
        /// Test the given function on the given data and see if the result is the given answer.
        /// </summary>
        /// <returns>true if matches</returns>
        static bool Test(Action<double[], bool> fftFunction, double[] test, double[] answer)
        {
            var success = true;
            var copy = new double[test.Length];
            Array.Copy(test, copy, test.Length); // make a copy
            fftFunction(copy, true);             // forward transform
            success &= Compare(copy, answer);    // check it
            fftFunction(copy, false);            // backward transform
            success &= Compare(copy, test);      // check it
            return success;
        }

        /// <summary>
        /// Compare two arrays of doubles for "equality"
        /// up to a small tolerance
        /// </summary>
        /// <param name="arr1"></param>
        /// <param name="arr2"></param>
        /// <returns></returns>
        static bool Compare(double [] arr1, double [] arr2)
        {
            if (arr1.Length != arr2.Length)
                return false;
            for (var i = 0; i < arr1.Length; ++i)
                if ((Math.Abs(arr1[i] - arr2[i]) > 1e-12))
                    return false;
            return true;
        }

        #endregion
    }
}
// end of file
