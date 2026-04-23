Example: TClk Synchronized Configured Acquisition

Recommended Input Signal: 100 kHz, 4 Vpp, sine wave

Devices Supported: NI 5105, NI 5114, NI 5122, NI 5124, NI 5142, NI 5152,
NI 5153, NI 5154, NI 5160, NI 5162, NI 5185, NI 5186, NI 5922

Console C Description:

This example configures all the digitizer's vertical, horizontal, and triggering
properties before every acquisition.  It allows you to experiment with numerous
configurations, including acquisition types and triggering modes, since it
supports nearly the entire functionality of NI-SCOPE.

This example also demonstrates how to easily synchronize an arbitrary number of
digitizers with niTClk.  The niTClk Configure for Homogeneous Triggers, niTClk
Synchronize, and niTClk Initiate VIs make this very simple to accomplish.  niTClk
is useful when synchronized sampling and subsample trigger position are desired.
Please refer to the niTClk documentation for more information.

Keep in mind that not all digitizers support all the features in this example.
The NI High Speed Digitizers Help includes a table of supported features for each device.

This example includes an auto-generated make file for building the example.
