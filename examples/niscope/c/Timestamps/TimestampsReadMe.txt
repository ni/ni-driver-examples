Example: Timestamps

Recommended Input Signal: 5 kHz, 1 Vpp, square wave

Devices Supported: NI 5105, NI 5114, NI 5122, NI 5124,
NI 5142, NI 5152, NI 5153, NI 5154, NI 5160, NI 5162, NI 5185,
NI 5186, NI 5622, NI 5922 (Some devices may require configuring
input impedance, vertical coupling and vertical range.)

Console C Description:

This example demonstrates the timestamping ability of some National
Instruments digitizers by creating a histogram of the time between
triggers in a multi-record acquisition.  It includes code to create
a histogram of the time between triggers.

A multi-record example is configured, but the only information fetched
from each record is the absolute timestamp of the first point in the
waveform and the timestamp of the first point relative to the trigger.
 The absolute trigger time is computed as:

   absolute trigger time = absolute initial x - relative initial x

The fetch function is used to wait for the trigger to occur and
retrieve the timestamps in the niScope_wfmInfo structure by specifying
a positive timeout when the numSamples parameter is zero.  Since the
example does not fetch the waveform, the waveform pointer passed to
the fetch function may be NULL.

This example is made possible by using a free running, 48 bit, timer
on the digitizer.  The timer is reset only when the computer is powered
down, the Reset or Abort functions are called, or the "reset"
parameter on the Init function is set to true.  Therefore, a multi-record
acquisition is not necessary for this example to operate -- a program that
reconfigures the digitizer and initiates a single record acquisition could
also make use of the absolute timestamps to find an extremely accurate
time between triggers.  However with a multi-record acquisition, the "dead
time", or the time when the digitizer is not acquiring data between records,
is a few microseconds, while initiating a new acquisition with software may
take several milliseconds.

The recommended input signal of this example is extremely slow (5 kHz).
This allows the digitizer to trigger on every cycle of the waveform.
Therefore, the time between triggers will be the period of the input
waveform, as displayed in the example.

Alternatively, if you apply a high frequency waveform (5 MHz), the
digitizer will trigger as fast as possible.  While the histogram may
not look as pretty, this allows you to measure the minimum time necessary
between triggers.

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
