Example: Fetch Forever

Recommended Input Signal: 2 kHz, 1.8 Vpp, sine wave

Recommended Sample Rate: 50 kHz

Devices Supported:
NI 5105, NI 5114, NI 5122, NI 5124, NI 5142, NI 5152, NI 5153,
NI 5154, NI 5160, NI 5162, NI 5622, NI 5185, NI 5186,
NI 5922 (Some devices may require configuring input impedance, vertical
coupling and vertical range.)
You should also use a 16 bit fetch function for 5622 and 5122 devices.

Console C Description:

This example demonstrates using continuous acquisition to fetch an infinite
record of data (or at least until you press stop).   It configures and starts
one acquisition that is never triggered.  This is accomplished by configuring
a software trigger and never sending the software trigger.  Therefore, the
digitizer will continuously acquire data into its onboard memory until Abort
or Close is called.

The loop continuously fetches data from onboard memory into the computer's
memory, using DMA transfers. If the sampling rate is low, the computer can
fetch all the data from the digitizer, so data is never overwritten.
However, at faster sampling rates, the computer will not be able to fetch the
data before it is overwritten by new data.  This results in the "data
overwritten" error.

The input parameters to the Fetch function include the maximum waveform size
to fetch.  Since a timeout of zero is set, the digitizer will return however
many points are currently available rather than waiting for the requested
number of samples.

The "relative to" property determines which samples to fetch from memory.  In
this example, it is set to the read pointer, which starts at the beginning of
the acquisition and is incremented by the actual number of points during each
fetch.

This example plots the latest acquired waveform every second.  At slow sample
rates (~1 kHz), the digitizer will probably acquire only a few points every iteration,
so the plot will be very small.  At faster rates (~50 kHz), the digitizer will acquire
many points every iteration and the plot will look better.

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

Each example includes both a Visual C++ 6.0 project file and an
auto-generated make file for building the example.

