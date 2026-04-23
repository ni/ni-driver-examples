Example: Differential Acquisition

Devices Supported: NI 5922. All other devices support native mode only.
(Some devices may require configuring input impedance, vertical coupling
and vertical range.)

Console C Description:

This example showcases the differential feature of the NI 5922. Differential
mode detects the difference between the two analog input channels, allowing any
signals in common to be discounted. Positive should be connected to channel 0
and negative to channel 1. Configuration for channel 0 will be used for both
channels. Only channel 0 should be fetched.

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
