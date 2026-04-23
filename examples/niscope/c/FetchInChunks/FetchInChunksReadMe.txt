Example: Fetch in Chunks

Recommended Input Signal: 20 Hz, 1.8 Vpp, sine wave

Devices Supported: NI 5105, NI 5114, NI 5122, NI 5124, NI 5132,
NI 5133, NI 5142, NI 5152, NI 5153, NI 5154, NI 5160, NI 5162, NI 5185,
NI 5186, NI 5622, NI 5922 (Some devices may require configuring input
impedance, vertical coupling and vertical range.)

Console C Description:

This example demonstrates fetching subsets of data.  It configures one triggered
acquisition and fetches subsets of data starting on the first pretrigger point.
Using absolute timestamps, the data is pieced together and the absolute trigger
time is displayed on the graph.

The configuration for this acquisition is the same as any other
acquisition -- the vertical, timing, and triggering properties are set.
Furthermore, the same Fetch function is used for fetching as is for any
other mode.  In this case, the "fetch offset" parameter is set to the zero
at first.  This means that data fetching starts at the first pretrigger point.
Since the relative to property has not been set, pretrigger is used.
Every loop we increment the fetch offset by the actual number of points returned.
Therefore, the loop around the Fetch VI retrieves all the data that the scope acquires.

The absolute initial x time is an absolute time derived from a free running counter
on the digitizer.  Since this time scale is the same for each chunk of data, it can
be used to reconstruct the entire waveform.  As shown in the example, each waveform
of data fetched from the digitizer is plotted with respect to the absolute initial x.
The pieces line up in time to produce the entire waveform.

The waveform info structure also contains the relative initial x value, which is the
time from the first point fetched to the trigger.  This value is not valid until the
trigger occurs.  However, after the trigger occurs, this value allows you to locate
the trigger time, which can be calculated much more accurately than the sample period
using a time-to-digital conversion circuit.  In this example, the graph's cursor is
positioned at absolute trigger time, where the absolute trigger time is calculated as:

   absolute trigger time = absolute initial x - relative initial x

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
