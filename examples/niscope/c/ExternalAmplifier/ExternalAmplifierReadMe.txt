Example: External Amplifier

Recommended Input Signal: 100 kHz, 4 Vpp, sine wave

Devices Supported: NI 5900, NI 5922 (Some devices may
require configuring input impedance, vertical coupling and vertical range.)

Console C Description:

This example demonstrates how to configure the oscilloscope to use an external
accessory.  The driver will automatically return fully scaled data, taking
into account both the oscilloscope and accessory scaling coefficients. The
example is set up to configure the vertical, horizontal, channel, and
triggering properties before every acquisition. When the accessory is
configured as part of the oscilloscope session the driver works with the
oscilloscope-accessory pair as a single device, applying the optimal settings
for both devices.

For example you can configure the NI-5922 Flex Resolution Digitizer with the
NI-5900 Amplifer which provides 4x attenuation.  The NI-5922 has two vertical
ranges, 2V and 10V.  The pair allows for measurements up to 40V pk-pk on the
high range and 8V pk-pk on the low range.  If you specify a range below 8V the
driver will intelligently configure the NI-5900 to use the 2V range to achieve
maximum vertical resolution.

For more information on using High Speed Digitizer/Oscilloscope Accessories
such as the NI-5900, please refer to the High Speed Digitizers Help.

Keep in mind that not all digitizers/oscilloscopes support all the features in
this example.  The NI High-Speed Digitizers Help includes a table of supported
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
