Example: Binary Acquisition

Recommended Input Signal: 100 kHz, 8 Vpp, sine wave

Devices Supported: NI 5105, NI 5114, NI 5122, NI 5124, NI 5132,
NI 5133, NI 5142, NI 5152, NI 5153, NI 5154, NI 5160, NI 5162, NI 5185,
NI 5186, NI 5622, NI 5922 (Some devices may require configuring input
impedance, vertical coupling and vertical range.)

Console C Description:

This example demonstrates fetching binary data instead of scaled data.
The driver always fetches binary data from the digitizer using DMA.
Typically you will fetch scaled data, which is a double (8 byte) floating
point number in units of volts.  However, it is faster to fetch the binary
data, which is a signed 8-, 16-, or 32- bit integer.  Then you can scale
the data to voltage using the gain and offset scaling parameters returned
by the Fetch function in the wfmInfo structure.

Finally, notice that the binary data size parameter changes which Fetch
function is used.  The only difference is the size of the binary data
returned from the driver.  For 8-bit devices such as the NI 5114 and NI
5152, there is rarely a reason to fetch anything but 8-bit data with these devices.
If you fetch 16- or 32-bit data from these devices, the data is shifted left and
the least order 8 or 16 bits are always zero.  This makes the min and max
data values constant for each fetch type, regardless of the device resolution,
as follows:

8 bit fetch = -128 to 127
16 bit fetch = -32,768 to 32,767
32 bit fetch = -2,147,484,468 to 2,147,484,467

For devices like the NI 5922 with up to 24 bits of resolution, you will need
to fetch 32-bit data. Fetching 8-bit data would be throwing away resolution! For
devices like the NI 5122 or NI 5105 with resolution of 14 bits and 12 bits,
respectively, you will need to fetch 16-bit data.

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
