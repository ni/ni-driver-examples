Example: Sample Width

Recommended Input Signal: 100 kHz, 8 Vpp, sine wave

Devices Supported: NI 5114, NI 5122, NI 5124, NI 5142, NI 5160, NI 5162,
NI 5922 (Some devices may require configuring input impedance,
vertical coupling and vertical range.)

Console C Description:

This example demonstrates lowering the resolution of a device. The driver supports
returning data in 8, 16, or 32 binary format. Normally the NISCOPE_ATTR_BINARY_SAMPLE_WIDTH
attribute can be queried to find out how many bytes are returned per sample stored.
For the 5114 for example, which is an 8 bit device, the native binary sample width
is 8 bits (1 byte). For the 5122 and 5124, which are 14 and 12 bit devices, the
native binary sample width is 16 bits (2 bytes).

If the resolution is not the most important factor in an application, setting the
NISCOPE_ATTR_BINARY_SAMPLE_WIDTH attribute to a lower value than the native will
actually change the resolution of the device. You can set the 5122 to be an 8 bit
device for example. In this case the stored samples will be only 1 byte wide, which
will save onboard memory. Also, you can use the 8 bit binary fetch improving performance
when transferring data to host memory.

Note that some devices support only the native width; it cannot be changed. Other devices
like the 5922 support lowering the resolution to 16 bits, but not to 8. An improvement
in memory and fetch performance will be noticeable even when fetching scaled data since
internally the device will store less bytes per sample and the driver will have to transfer
less data to be scaled.

For more information on binary fetching refer to the Binary Acquisition example.

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
