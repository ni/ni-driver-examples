Example: Getting Started

Recommended Input Signal: 100 kHz, 1.8 Vpp, sine wave

Devices Supported: NI 5105, NI 5114, NI 5122, NI 5124, NI 5132,
NI 5133, NI 5142, NI 5152, NI 5153, NI 5154, NI 5160, NI 5162, NI 5185,
NI 5186, NI 5622, NI 5922 (Some devices may require configuring input
impedance, vertical coupling and vertical range.)

Console C Description:

This example opens a session to the NI-SCOPE driver by passing in the
device's resource name to the Init function.  The resource name is a
unique identifier for your National Instruments' hardware product.  It
can be found by running Measurement & Automation Explorer (MAX) on Windows
or the lsni utility on Linux.

The session handle returned from the Init function is a parameter to all
functions in the NI-SCOPE driver.  Only one session may be opened at a
time for your digitizer, and every session should be closed by calling
the Close function.  The session should be closed even when an error occurs.

The Auto Setup function takes several acquisitions to measure the amplitude
and frequency of the input signal.  This allows it to optimally set
properties such as the sample rate, vertical range, and minimum record
length.  The Read function, initiates an acquisition, waits for it to finish,
and fetches the data from the digitizer.  The voltage waveform is returned
with a wfmInfo structure.  The structure contains the relative initial x
which is the time of the first point in the waveform relative to the trigger
and the x increment which is the time between two points in the waveform.
These values can be used for plotting the waveform versus time, where time
equals zero corresponds to the trigger position.

The handleErr macro is used with the "error" and "errorSource" variables to
check for an error condition (error < 0).  If an error occurs, it stores the
name of the offending function in the errorSource string and it executes the
statement "goto Error".  All freeing of memory should be done after the
"Error" label to avoid a memory leak.   See niScope.h for the macro code.
The errorHandler function translates an error code into a text message.

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
