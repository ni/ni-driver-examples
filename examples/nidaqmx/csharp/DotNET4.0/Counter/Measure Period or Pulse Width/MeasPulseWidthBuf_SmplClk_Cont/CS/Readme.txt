Example Title:           MeasPulseWidthBuf_SmplClk_Cont

Example Filename:        MeasPulseWidthBuf_SmplClk_Cont.sln

Category:                CI

Description:             This example demonstrates how to continually measure
                         pulsewidths on a Counter Input Channel 
                         using an external sampleclock. The Maximum and Minimum
                         Values, Sample Clock Source, andSamples per Channel 
                         are all configurable.This example shows how to measure
                         pulse width on the counter'sdefault input terminal 
                         (refer to section IV, I/O ConnectionsOverview, below for
                         more information), but could easily beexpanded 
                         to measure pulse width on any PFI, RTSI, or
                         internalsignal.Note: For sample clock measurements, an
                         external 
                         sample clock isnecessary to signal when the counter should
                         measure asample. This is set by the Sample 
                         Clock Source control.

Software Group:          Measurement Studio

Required Software:       Visual Studio .NET

Language:                Visual C#

Language Version:        8.0

Driver Name:             DAQmx
