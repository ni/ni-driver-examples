Example: Measurement Library

Recommended Input Signal: 100 kHz, 8 Vpp, sine wave

Devices Supported: NI 5105, NI 5114, NI 5122, NI 5124, NI 5132,
NI 5133, NI 5142, NI 5152, NI 5153, NI 5154, NI 5160, NI 5162, NI 5185,
NI 5186, NI 5622, NI 5922 (Some devices may require configuring input
impedance, vertical coupling and vertical range.)

Console C Description:

This example illustrates fetching scalar measurements from NI-SCOPE,
such as period and rise time calculations.

Some common horizontal and vertical parameters are configured for the
digitizer.  The SetAttribute function is used to set the reference level
units, and the low, middle and high reference levels.  The reference levels
are used for finding the time of various events in the waveform, and they
are generally set to 10, 50, and 90 percent.  For more information about
measurement library parameters, see the NI-SCOPE Function Reference Help file.

After the Fetch function is called to acquire a waveform, the Fetch Measurement
Stats function is used to perform the desired scalar measurement analysis on
the waveform.  NI-SCOPE keeps a history of each measurement's results, which
allows it to retrieve the statistics computed over multiple acquisitions.
These statistics include the mean, standard deviation, min, max and number of
acquisitions used in the statistics.  The statistics are updated once per
acquisition when the measurement is fetched.  The Clear Stats function clears
this history.

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
