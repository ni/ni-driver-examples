Example: Configured Acquisition

Recommended Input Signal: 100 kHz, 4 Vpp, sine wave

Devices Supported: NI 5105, NI 5114, NI 5122, NI 5124, NI 5132,
NI 5133, NI 5142, NI 5152, NI 5153, NI 5154, NI 5622, NI 5185,
NI 5186, NI 5160, NI 5162, NI 5922 (Some devices may require configuring
input impedance, vertical coupling and vertical range.)

Console C Description:

This example configures all the digitizer's vertical, horizontal,
and triggering properties before every acquisition.  It allows you
to experiment with numerous configurations, including acquisition
types and triggering modes, since it supports nearly the entire
functionality of NI-SCOPE.  However, it may be more instructive
to consult other examples about specific topics such as FlexRes,
Random Interleaved Sampling, and Multi-Record to learn more about
these features.

This example displays the "actual sample rate" and "actual record
length".  The corresponding "min sample rate" and "min record length"
are specified with the Configure Horizontal Timing function.  NI-SCOPE
maintains a constant time per record for each acquisition.  So when
you specify a min sample rate and a min record length, this determines
the amount of time the acquisition should take:

 time per record = min record length / min sampling rate.

However, digitizers support only a certain number of defined sampling
rates, so if the value chosen is not a valid rate, it is rounded to
the next higher rate, called the actual sample rate.  The actual record
length is then calculated in order to maintain the time per record.
Both of these values are returned in this example so you can investigate
the effects of parameter rounding with your digitizer.  For more
information about parameter coercions, see the NI-High Speed Digitizers
Help.

The enforce real-time property allows this example to use Random
Interleaved Sampling.   See the Random Interleaved Sampling example for
more details.

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
