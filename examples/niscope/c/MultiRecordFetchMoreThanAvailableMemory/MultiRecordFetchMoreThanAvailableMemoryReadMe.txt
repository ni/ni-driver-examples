Example: Multi Record Fetch More Than Available Memory

Recommended Input Signal: 1 kHz, 1.8 Vpp, sine wave

Devices Supported: NI 5105, NI 5114, NI 5122, NI 5124,
NI 5142, NI 5152, NI 5153, NI 5154, NI 5160, NI 5162, NI 5185, NI 5186,
NI 5622, NI 5922 (Some devices may require configuring input impedance,
vertical coupling and vertical range.)

Console C Description:

This example demonstrates the multi-record and continuous acquisition
capabilities of National Instruments digitizers.  In a multi-record
acquisition, each record is one waveform with at least "min record length"
points as specified with the Configure Horizontal Timing function.
Furthermore, each record is triggered, so when the trigger arrives for the
first record, the hardware quickly rearms for the next record.  Refer the to
Multi Record example for more information.

In this example, the Fetch function is used with the Fetch Record Number and
Fetch Number of Records attributes to fetch each record individually.  Using
the Fetch function with a positive timeout only waits for the requested
waveform to be done - rather than waiting for all the records.  In this
example, the fetch function only waits for the next record, and then it
retrieves the record while it is acquiring other records.

The Allow More Records Than Memory attribute is a Boolean value that allows
you to configure more records than fit in the onboard memory at one time.
When this is enabled, you can specify any number of records.  The records are
treated circularly in the onboard memory, so you must fetch them before they
are overwritten.  If you attempt to fetch a record that has been overwritten
in the digitizer's memory, an error is returned.

Warning!  Windows NT and 2000 limit the amount of page locked memory at any
given time, and they will crash without warning if you exceed this limit.
Every record that you configure requires a small amount of page locked memory,
so be careful when configuring a large number of records!

See the NI High-Speed Digitizers Help for more information about continuous,
multiple-record acquisitions.

This example allows you to disable plotting.  Fetching will be much faster
if plotting is not enabled.

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
