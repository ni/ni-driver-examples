Example: Random Interleaved Sampling

Recommended Input Signal: 10 MHz, 8 Vpp, sine wave

Devices Supported: NI 5114, NI 5122, NI 5124, NI 5142,
NI 5152, NI 5153, NI 5154, NI 5160, NI 5162, NI 5185, NI 5186 (Some
devices may require configuring input impedance, vertical coupling and
vertical range.)

Console C Description:

This example demonstrates random interleaved sampling (RIS) acquisitions.
RIS is a method of reconstructing a periodic waveform by combining multiple,
triggered acquisitions. Refer to the NI High-Speed Digitizers Help for more
information about RIS.

This example does two acquisitions: one at the maximum real-time sampling rate
of the digitizer and the other at a higher rate using RIS. Both waveforms are
plotted using the asciiPlot function.

The example performs the same coercion that NI-SCOPE would perform to
determine the actual RIS sample rate, and it displays the oversampling factor.
The oversampling factor is the minimum number of real-time waveforms needed to
construct the RIS waveform.  It follows the formula:

  actual RIS sample rate = oversampling factor * maximum real-time sample rate

To keep the duration of the two acquisitions equal, the number of samples are
adjusted (using the oversampling factor) for the real-time acquisition.

This example prompts for the number of averages for the RIS acquisition.  The
NI High-Speed Digitizers Help discusses the importance of averaging RIS
acquisitions since coercing the time of samples to create an evenly spaced
waveform may add noise to the signal.

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
