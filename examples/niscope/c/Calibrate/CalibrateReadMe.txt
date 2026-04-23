Example: Calibrate

Recommended Input Signal: None

Devices Supported: NI 5105, NI 5114, NI 5122, NI 5124, NI 5132, NI 5133,
NI 5142, NI 5152, NI 5153, NI 5154, NI 5622, NI 5185, NI 5186, NI 5160, NI 5162,
NI 5922

Console C Description:

This example performs a self-calibration of your digitizer.  Disconnect or
disable any AC input signals before starting self-calibration.  AC or varying
signals can, in some cases, cause self-calibration to fail or compromise the
accuracy of the calibration.

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
