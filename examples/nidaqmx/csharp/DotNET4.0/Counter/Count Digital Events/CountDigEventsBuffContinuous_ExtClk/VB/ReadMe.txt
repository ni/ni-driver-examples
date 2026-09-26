Example Title:           CountDigEventsBuffContinuous_ExtClk

Example Filename:        CountDigEventsBuffContinuous_ExtClk.sln

Category:                CI

Description:             This example demonstrates how to count buffered digital
                         events on a Counter Input channel.  
                         The initial count, count direction, edge, and sample clock
                         source are all configurable.  Edges are counted 
                         on the counter's default input terminal (see I/O
                         Connections Overview below for more information), but 
                         could easily be modified to count edges on a PFI or RTSI
                         line.Note: For buffered event counting, an external 
                         sample clock is necessary to signal when a sample should
                         be inserted into the buffer.  Specify the source 
                         terminal of the external clock in the clock source text
                         box when you run the example.

Software Group:          Measurement Studio

Required Software:       Visual Studio .NET

Language:                Visual Basic .NET

Language Version:        8.0

Driver Name:             DAQmx

Driver Version:          26.8
