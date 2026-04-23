Example: Video Triggering

Recommended Input Signal: Standard Video Signal, NTSC format, Negative polarity

Devices Supported: NI 5114, NI 5122, NI 5124, NI 5142 (Some devices may require
configuring input impedance, vertical coupling and vertical range.)

Note that these devices only support Negative Polarity and trigger events: Any Line,
Line Number, Field1, and Field2.

Console C Description:

This example configures the digitizer's vertical, horizontal, and video
triggering properties before every acquisition.  The signal format indicates the
number of lines in each frame.  This allows the digitizer to trigger on a specific
line of a video signal.  If the digitizer is configured to trigger from a specific
line and the TV event is set to "Line Number" then the example will fetch
"Actual Record Length" of points for the line specified.  The example also
has a DC restore option.  Since video signals typically contain significant
levels of DC offset, it is necessary to AC couple the signal.  However, DC
coupling the signal moves the average value of the signal to zero.  DC
restore finds the zero level reference of the signal for each line and restores
the reference to the correct level.

Keep in mind that not all digitizers support all the features in this
example.  The NI High-Speed Digitizers Help includes a table of supported
features for each device.

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
