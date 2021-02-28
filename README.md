SharpFFTW
===========

Basic C# wrapper for FFTW.

The native libraries are no longer distributed with this project. Go to the [FFTW downloads](http://www.fftw.org/install/windows.html) to get precompiled DLLs for windows. At the moment (Feb 2021), an outdated version 3.3.5 is available for download. To get an up-to-date version, try [conda](https://anaconda.org/conda-forge/fftw/files). Be aware that the DLLs provided in the conda package must be renamed to `libfftw3-3.dll` and `libfftw3f-3.dll` respectively (or alternatively, the library name in both `NativeMethods.cs` files must be adapted).

Features
============

* Unmanaged function calls to main FFTW functions for both single and double precision
* Basic managed wrappers for FFTW plans and unmanaged arrays
* Test program that demonstrates basic functionality
