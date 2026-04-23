Example: Flexible Resolution

Recommended Input Signal: 100 kHz, 8 Vpp, sine wave

Devices Supported: NI 5922 (Some devices may require configuring
input impedance, vertical coupling and vertical range.)

Console C Description:

The NI 5922 supports flexible resolution, an advanced averaging technique
that increases resolution at lower sampling rates.  The
NI 5922 always has flexible resolution.  Flexible resolution is often used
for spectrum analysis applications, so this example includes the FFT Amplitude
Spectrum of the acquired signal.

This example configures a flexible resolution acquisition with the Configure
Acquisition function.  The vertical and horizontal parameters are configured
as always.  The sampling rate used in this example will be coerced to a
sampling rate supported in flexible resolution.  This example also displays
the effective number of bits at each sampling rate.

The InitiateAcquisition and Fetch functions are used to acquire a waveform.
The increased resolution available during flexible resolution mode is
often used for spectral measurements.  Therefore, in this example the
NI-SCOPE measurement library is used to perform a windowed FFT measurement.
The Hanning window is added as a processing step.  This measurement is
computed before any other measurements, so when the Array Measurement
function is called a Hanning window is applied to the waveform followed by
the specified FFT measurement.  For more information about the analysis
available in NI-SCOPE, see the Advanced Measurement Library example or the
NI High-Speed Digitizers Help.

File Locations and Responsibilities:

Each NI-SCOPE example for Console C includes a directory of
files specific to that example.  In addition, each example project
includes a generic .c and .h file located in the
common directory.  These generic files
include all the NI-SCOPE specific programming, and they are used in the
Measurement Studio, CVI, and console C examples.  Many of the examples
also use the asciiPlot.c and asciiPlot.h files located in the common
directory.

The generic files call some externally defined C functions that are
specific for the development environment, such as the PlotWaveform
function that displays the acquired waveform.  For console C programs,
these functions are very simple.  They either prompt the user for input
or hard code certain values.  However, each example should provide
enough feedback to illustrate that the example is working properly.

Each example includes both a Visual C++ 6.0 project file and an
auto-generated make file for building the example.
