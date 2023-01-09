# SharpFFTW

A lightweight C# wrapper for native FFTW.

## Features

* Unmanaged function calls to main FFTW functions for both single and double precision
* Basic managed wrappers for FFTW plans and unmanaged arrays
* Test program that demonstrates basic functionality

The binaries provided for download on the official website https://www.fftw.org/install/windows.html are outdated (version 3.3.5, last checked January 2023). If you are looking for up-to-date FFTW windows binaries, try [conda](https://anaconda.org/conda-forge/fftw/files) or my personal [package site](http://wo80.bplaced.net/packages/#package:fftw).

## FFT benchmark

The FFT benchmark `fftbench` compares a selection of managed libraries ([Accord](http://accord-framework.net/), [Exocortex](http://www.exocortex.org/dsp/), [Lomont](https://www.lomont.org/software/), [MathNet](https://numerics.mathdotnet.com/), [NAudio](https://github.com/naudio/NAudio), [NWaves](https://github.com/ar1st0crat/NWaves)) with FFTW. The benchmark was originally written for a CodeProject article [Comparison of FFT Implementations for .NET](https://www.codeproject.com/Articles/1095473/Comparison-of-FFT-Implementations-for-NET). Here's an example output. The first column displays the relative speedup (slowdown) compared to `Exocortex`:

```
FFT size: 2^12 (4096)
  Repeat: 80

[13/13] Done

    FFTWF (real):  0.2  [min:   13.07, max:   23.09, mean:   13.91, stddev:    1.58]
     FFTW (real):  0.3  [min:   22.45, max:   29.72, mean:   23.13, stddev:    1.16]
            FFTW:  0.6  [min:   44.38, max:   54.68, mean:   46.08, stddev:    2.33]
Exocortex (real):  1.0  [min:   73.41, max:   87.08, mean:   74.40, stddev:    1.71]
   Lomont (real):  1.2  [min:   83.59, max:  145.93, mean:   91.67, stddev:    8.83]
   NWaves (real):  1.6  [min:  108.90, max:  134.05, mean:  121.49, stddev:    5.93]
          NWaves:  1.8  [min:  131.83, max:  160.96, mean:  133.78, stddev:    3.88]
          Lomont:  2.2  [min:  161.74, max:  179.24, mean:  165.22, stddev:    3.96]
          NAudio:  2.3  [min:  165.75, max:  302.79, mean:  169.82, stddev:   15.28]
       Exocortex:  2.4  [min:  171.52, max:  193.15, mean:  175.00, stddev:    4.60]
          AForge:  3.1  [min:  224.57, max:  264.78, mean:  228.89, stddev:    5.33]
          Accord:  4.0  [min:  286.49, max:  352.00, mean:  298.92, stddev:   12.66]
        Math.NET:  4.9  [min:  331.12, max:  441.35, mean:  360.87, stddev:   17.33]

Timing in microseconds.
```

## Exocortex.DSP

One of the earliest .NET open-source FFT projects and - as the benchmark confirms - still one of the best performing. The copy provided here was updated to .NET standard 2.0 and uses the `System.Numerics.Complex` type. `Exocortex.DSP` is distributed under the BSD license.
