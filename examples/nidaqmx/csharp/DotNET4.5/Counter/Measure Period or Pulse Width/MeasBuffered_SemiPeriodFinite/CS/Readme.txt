Example Title:           MeasBuffered_SemiPeriodFinite

Example Filename:        MeasBuffered_SemiPeriodFinite.sln

Category:                CI

Description:             This example demonstrates how to measure semi-periods on
                         a counter input channel. The 
                         minimum value, maximum value, sample mode, and samples per
                         channel are all configurable.This example 
                         shows how to measure semi-period on the counter's default
                         input terminal (see I/O Conections Overview 
                         below for more information), but can easily be expanded to
                         measure semi-period on any PFI, RTSI, or internal 
                         signal by setting the properties on the CIChannel
                         object.Semi-period measurement differs from pulse width 
                         measurement in that it measures both the high and the low
                         pulses of a given signal.  So for every period, 
                         two data points will be returned.

Software Group:          Measurement Studio

Required Software:       Visual Studio .NET

Language:                Visual C#

Language Version:        8.0

Driver Name:             DAQmx
