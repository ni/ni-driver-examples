Example: OSP Quadrature Downconversion

Recommended Input Signal:  10.1 MHz, 1 Vpp, sine wave

Devices Supported: NI 5142, NI 5622 (Some devices may require configuring
input impedance, vertical coupling and vertical range.)

Console C Description:

This example shows how to program a quadrature downconversion acquisition.
With an input signal of 10.1 MHz and default configurations you should see
two sine waves that are 90 degrees out of phase.

Digitizers support only a certain number of defined sample
rates with DDC processing enabled, so if the value chosen is not
a valid rate, it is rounded to the next higher rate, called the
actual sample rate.

File Locations and Responsibilities:

Each NI-SCOPE example for Console C includes a directory of
files specific to that example.  In addition, each example project
includes a generic .c and .h file located in the
niscope\examples\c\common directory.  These generic files
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

This example includes an auto-generated make file for building the example.
