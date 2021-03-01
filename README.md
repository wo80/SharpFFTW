SharpFFTW
===========

A lightweight C# wrapper for native FFTW.

The native libraries are no longer distributed with this project. Up-to-date versions can be obtained on [conda](https://anaconda.org/conda-forge/fftw/files). The current conda package (version 3.3.9, Feb 2021) has no SIMD features enabled. You can download a custom package with SSE2 enabled [here](http://wo80.bplaced.net/math/packages-fftw.html). For an official (but outdated) package go to the [FFTW downloads](http://www.fftw.org/install/windows.html). Be aware that the DLLs provided in the official package must be renamed to `fftw3.dll` and `fftw3f.dll` respectively (or alternatively, the library names in both `NativeMethods.cs` files can be adapted).

Features
============

* Unmanaged function calls to main FFTW functions for both single and double precision
* Basic managed wrappers for FFTW plans and unmanaged arrays
* Test program that demonstrates basic functionality
