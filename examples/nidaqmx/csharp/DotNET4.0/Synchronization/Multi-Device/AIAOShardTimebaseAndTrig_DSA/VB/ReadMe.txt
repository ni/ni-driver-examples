Example Title:           AIAOShardTimebaseAndTrig_DSA

Example Filename:        MultiDeviceSync_AIAOShardTimebaseAndTrig_DSA.sln

Category:                Synchronization

Description:             This example synchronizes the clocks and trigger on two
                         Dynamic Signal Acquistion (DSA) 
                         devices and performs continuous analog input and output. 
                         NOTE: This example is intended to show low 
                         level synchronization of various devices. DSA and S Series
                         devices now support including channels from 
                         multiple devices in a single task. DAQmx automatically
                         synchronizes the devices in such a task. See the 
                         DAQmx Help>>NI-DAQmx Device
                         Considerations>>Multidevice Tasks section for
                         further details.NOTE: 
                         If you are using PXI DSA devices along with sample clock
                         timebase synchronization, the master device 
                         must reside in PXI slot 2.NOTE: This code will not run
                         "as-is" on a multifunction (MIO) DAQ device.

Software Group:          Measurement Studio

Required Software:       Visual Studio .NET

Language:                Visual Basic .NET

Language Version:        8.0

Driver Name:             DAQmx

Driver Version:          26.8
