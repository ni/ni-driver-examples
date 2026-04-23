namespace NationalInstruments.Examples.NIFgen.OspBaseband
{
    using System;
    using ModularInstruments.NIFgen;

    /// <summary>
    /// Demonstrates the use of the NI-FGEN .NET API to generate real or complex data while applying
    /// baseband onboard signal processing.
    /// Note: Refer to the specifications document for your NI signal generator to determine whether the features and values used in this example are supported.
    /// </summary>
    /// <remarks>
    /// Using this example:
    /// 1. Expand the Settings and Configuration region and familiarize yourself with the settings.
    /// 2. Configure the onboard signal processing settings.
    /// 3. Configure the FIR filter settings.
    /// 4. Configure the output filter settings.
    /// 5. Set the OSP data processing mode, and create a real or complex waveform.
    /// 6. Build and run the example.
    /// </remarks>
    public sealed class Program
    {
        #region Settings and Configuration

        // IVI resource name of your signal generator in MAX.
        private const string ResourceName = "PXI1Slot2";

        // Keep this as channel 0. When writing your waveform to memory,
        // you must either use "I" or "I,Q" depending on whether you are
        // writing real, or complex data.
        private const string ChannelName = "0";

        // Onboard Signal Processing settings.
        private const double IQRate = 100000;
        private const double FrequencyShift = 0.0;
        private const double PrefilterGainI = 1.0;
        private const double PrefilterOffsetI = 0.0;
        private const double PrefilterGainQ = 1.0;
        private const double PrefilterOffsetQ = 0.0;

        // FIR filter settings.
        // When using OSP Baseband, set the FirFilterType to Flat, RaisedCosine, or RootRaisedCosine.
        private const OspFirFilterType FirFilterType = OspFirFilterType.RaisedCosine;
        private const double FlatPassband = 0.4;
        private const double RaisedCosineAlpha = 0.25;
        private const double RootRaisedCosineAlpha = 0.25;

        // Output filter settings.
        // Ensure your instrument supports AnalogFilterEnabled before enabling it.
        private const bool FlatnessCorrectionEnabled = true;
        private const bool AnalogFilterEnabled = false;

        // OSP data processing mode. Use double data when in Real mode and ComplexDouble data when in Complex mode.
        private const OspDataProcessingMode DataProcessingMode = OspDataProcessingMode.Complex;

        // Waveform to generate. Replace this with a double[] if using OspDataProcessingMode.Real.
        private static readonly ComplexDouble[] Waveform =
        {
            new ComplexDouble(0.707, 0.707),
            new ComplexDouble(-0.707, 0.707),
            new ComplexDouble(0.707, -0.707),
            new ComplexDouble(-0.707, -0.707),
        };

        #endregion // Settings and Configuration

        static void Main(string[] args)
        {
            try
            {
                Console.WriteLine("1. Initializing the signal generator session.");
                using (NIFgen session = new NIFgen(ResourceName, false, true))
                {
                    Console.WriteLine("2. Configuring session to use channel {0} for all channel-based properties.", ChannelName);
                    session.ConfigureChannels(ChannelName);

                    Console.WriteLine("3. Enabling onboard signal processing.");
                    session.Arbitrary.OnboardSignalProcessing.SetEnabled(ChannelName, true);

                    // Setting the trigger mode to Single will cause the signal generator
                    // to only generate once.
                    session.Trigger.SetTriggerMode(ChannelName, TriggerMode.Single);

                    // Set the output mode of the signal generator. Set this at the beginning of your 
                    // program to determine what the signal generator will output.
                    Console.WriteLine("4. Setting the signal generator's output mode to Arbitrary.");
                    session.Output.OutputMode = OutputMode.Arbitrary;

                    Console.WriteLine("5. Setting the onboard signal processing mode to Baseband.");
                    session.Arbitrary.OnboardSignalProcessing.SetMode(ChannelName, OspMode.Baseband);

                    Console.WriteLine("6. Configuring the onboard signal processing settings:");
                    Console.WriteLine("\t - IQ Rate = {0}", IQRate);
                    session.Arbitrary.OnboardSignalProcessing.SetIQRate(ChannelName, IQRate);

                    Console.WriteLine("\t - Frequency Shift = {0:F}", FrequencyShift);
                    session.Arbitrary.OnboardSignalProcessing.SetFrequencyShift(ChannelName, FrequencyShift);

                    Console.WriteLine("\t - Prefilter Gain I = {0:F}", PrefilterGainI);
                    session.Arbitrary.OnboardSignalProcessing.SetPrefilterGainI(ChannelName, PrefilterGainI);

                    Console.WriteLine("\t - Prefilter Offset I = {0:F}", PrefilterOffsetI);
                    session.Arbitrary.OnboardSignalProcessing.SetPrefilterOffsetI(ChannelName, PrefilterOffsetI);

                    Console.WriteLine("\t - Prefilter Gain Q = {0:F}", PrefilterGainQ);
                    session.Arbitrary.OnboardSignalProcessing.SetPrefilterGainQ(ChannelName, PrefilterGainQ);

                    Console.WriteLine("\t - Prefilter Offset Q = {0:F}", PrefilterOffsetQ);
                    session.Arbitrary.OnboardSignalProcessing.SetPrefilterOffsetQ(ChannelName, PrefilterOffsetQ);

                    Console.WriteLine("7. Enabling OSP overflow error reporting.");
                    session.Arbitrary.OnboardSignalProcessing.SetOverflowErrorReporting(ChannelName, OspOverflowErrorReporting.Error);

                    Console.WriteLine("8. Setting the FIR filter type to {0}.", FirFilterType);
                    session.Arbitrary.OnboardSignalProcessing.FirFilter.SetFilterType(ChannelName, FirFilterType);

                    OspFirFilterType firFilterType = session.Arbitrary.OnboardSignalProcessing.FirFilter.GetFilterType(ChannelName);

                    switch (firFilterType)
                    {
                        case OspFirFilterType.Flat:
                            Console.WriteLine("9. Setting the flat passband to {0}.", FlatPassband);
                            session.Arbitrary.OnboardSignalProcessing.FirFilter.SetFlatPassband(ChannelName, FlatPassband);
                            break;
                        case OspFirFilterType.RaisedCosine:
                            Console.WriteLine("9. Setting the raised cosine alpha to {0}.", RaisedCosineAlpha);
                            session.Arbitrary.OnboardSignalProcessing.FirFilter.SetRaisedCosineAlpha(ChannelName, RaisedCosineAlpha);
                            break;
                        case OspFirFilterType.RootRaisedCosine:
                            Console.WriteLine("9. Setting the root raised cosine alpha to {0}.", RootRaisedCosineAlpha);
                            session.Arbitrary.OnboardSignalProcessing.FirFilter.SetRootRaisedCosineAlpha(ChannelName, RootRaisedCosineAlpha);
                            break;
                        default:
                            Console.WriteLine("You must configure the OSP FirFilterType to Flat, RaisedCosine, or RootRaisedCosine when using OSP Baseband.");
                            return;
                    }

                    Console.WriteLine("10 {0} flatness correction.", FlatnessCorrectionEnabled ? "Enabling" : "Disabling");
                    session.Output.Filter.SetFlatnessCorrectionEnabled(ChannelName, FlatnessCorrectionEnabled);

                    Console.WriteLine("11. {0} the analog filter.", AnalogFilterEnabled ? "Enabling" : "Disabling");
                    session.Output.Filter.SetAnalogFilterEnabled(ChannelName, AnalogFilterEnabled);

                    Console.WriteLine("12. Setting the data processing mode to {0}.", DataProcessingMode);
                    session.Arbitrary.OnboardSignalProcessing.SetDataProcessingMode(ChannelName, DataProcessingMode);

                    Console.WriteLine("13. Allocating memory for the waveform data.");
                    int waveformHandle = session.Arbitrary.Waveform.Allocate(ChannelName, Waveform.Length);

                    // If the OSP data processing mode is Real, only write to "I".
                    // If the OSP data processing mode is Complex, write to both "I" and "Q".
                    Console.WriteLine("14. Writing the specified waveform in the signal generator's memory.");
                    session.Arbitrary.Waveform.Write("I,Q", waveformHandle, Waveform);
                    session.Arbitrary.Waveform.SetHandle(ChannelName, waveformHandle);

                    Console.WriteLine("15. Initiating generation.");
                    session.InitiateGeneration();

                    // Wait until the generation is complete. If the trigger mode is set
                    // to Continuous, then WaitUntilDone will time out.
                    try
                    {
                        session.WaitUntilDone(TimeSpan.FromSeconds(30));
                        Console.WriteLine("16. Generation complete.");
                    }
                    catch (TimeoutException)
                    {
                        session.AbortGeneration();
                        Console.WriteLine("16. Generation was aborted.");
                    }
   
                    OspOverflowStatus overflowStatus = session.Arbitrary.OnboardSignalProcessing.OverflowStatus;
                    Console.WriteLine("17. Overflow status:");
                    Console.WriteLine("\t Prefilter Gain I - {0}", overflowStatus.HasFlag(OspOverflowStatus.PrefilterGainI));
                    Console.WriteLine("\t Prefilter Gain Q - {0}", overflowStatus.HasFlag(OspOverflowStatus.PrefilterGainQ));
                    Console.WriteLine("\t Prefilter Offset I - {0}", overflowStatus.HasFlag(OspOverflowStatus.PrefilterOffsetI));
                    Console.WriteLine("\t Prefilter Offset Q - {0}", overflowStatus.HasFlag(OspOverflowStatus.PrefilterOffsetQ));
                    Console.WriteLine("\t PFIR Filter I - {0}", overflowStatus.HasFlag(OspOverflowStatus.FirFilterI));
                    Console.WriteLine("\t PFIR Filter Q - {0}", overflowStatus.HasFlag(OspOverflowStatus.FirFilterQ));
                    Console.WriteLine("\t CIC Filter I - {0}", overflowStatus.HasFlag(OspOverflowStatus.CicFilterI));
                    Console.WriteLine("\t CIC Filter Q - {0}", overflowStatus.HasFlag(OspOverflowStatus.CicFilterQ));
                    Console.WriteLine("\t Complex Data - {0}", overflowStatus.HasFlag(OspOverflowStatus.ComplexData));
                    Console.WriteLine("\t CFIR Filter I - {0}", overflowStatus.HasFlag(OspOverflowStatus.CFirFilterI));
                    Console.WriteLine("\t CFIR Filter Q - {0}", overflowStatus.HasFlag(OspOverflowStatus.CFirFilterQ));

                }
            }
            // Handle driver-specific exceptions before the general exception handler that follows.
            // An example of a driver-specific exception is Ivi.Driver.IviCDriverException.
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
            finally
            {
                Console.WriteLine("Program complete. Press any key to close.");
                Console.ReadKey();
            }
        }
    }
}
