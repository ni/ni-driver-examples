Example Title:           AIFiniteAcquisition

Example Filename:        AIFiniteAcquisition.sln

Category:                Synchronization

Description:             This example demonstrates how to acquire a finite amount
                         of analog input data using two 
                         DAQ devices' internal clocks.  It also synchronizes these
                         devices depending on the device family (E Series, 
                         M Series, or DSA) to simultaneously acquire the data.NOTE:
                         This example is intended to show low level 
                         synchronization of various devices. DSA and S Series
                         devices now support including channels from multiple 
                         devices in a single task. DAQmx automatically synchronizes
                         the devices in such a task. See the DAQmx 
                         Help>>NI-DAQmx Device
                         Considerations>>Multidevice Tasks section for
                         further details.NOTE: 
                         PXI 6115 and 6120 (S Series) devices don't require sharing
                         of master timebase, because they auto-lock 
                         to Clock 10.  For those devices sharing a start trigger is
                         adequate.NOTE: For the PCI-6154 S Series device 
                         use the M Series (PCI) synchronization type to synchronize
                         using the reference clock.

Software Group:          Measurement Studio

Required Software:       Visual Studio .NET

Language:                Visual Basic .NET

Language Version:        8.0

Driver Name:             DAQmx

Driver Version:          26.5
