Example: OSP Baseband Decimation

Recommended Input Signal: 100 kHz, 1 Vpp, sine wave

Devices Supported: NI 5142, NI 5622 (Some devices may require
configuring input impedance, vertical coupling and vertical range.)

Console C Description:

This example shows how to program a baseband decimation acquisition.
This example shows how the digitizer can perform alais protected decimation
on input signals from channel 0 and channel 1.   The output of the DDC
is complex data where the real component is the data from channel 0 and
the imaginary component is the data from channel 1.

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
