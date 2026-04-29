//[1] Steps:
//1. Open an NI - RFSG session.
//2. Configure RFSG Selected Ports.
//3. Configure RFSG frequency reference.
//4. Configure frequency and power level of RF output signal.
//5. Set RFSG External Gain. #4 and #5 ensure that the average power of the signal at the input of the DUT
//   matches the user configured DUT Average Input Power.
//6. Read waveform from file and download Waveform from file to RFSG.
//7. Set Automatic SG SA Shared LO to Enabled.

//[2] Steps to perform ModAcc measurement:
//8. Set LO Offset Mode to Auto while performing an in - band ModAcc measurement.
//   This causes the RFSG LO to be placed outside the signal, if signal bandwidth is less than
//   half of the device instantaneous bandwidth; otherwise, the LO is placed at the center of the signal.
//9. Write script to generate the waveform specified in the script. This script is programmed to generate waveform continuously.
//10. Initiate signal generation.
//-------------------------------------------------------------------------------------------------------------------------------
//11. Open a new RFmx Session.
//12. Configure the Frequency Reference properties (Clock Source and Clock Frequency).
//13. Configure Selected Ports.
//14. Configure Automatic SG SA Shared LO to Enabled.
//15. Configure basic signal properties (Center Frequency, Reference Level and External Attenuation).
//16. Configure Trigger Type and Trigger Parameters.
//17. Configure Link Direction, Frequency Range, CC bandwidth, Cell ID, Band and BWP Subcarrier Spacing.
//18. Set LO Leakage Avoidance Enabled to True. This causes RFmx to place the SA LO outside the measurement bandwidth,
//    if the measurement bandwidth is less than half of the device instantaneous bandwidth;
//    otherwise, the LO is placed at the center of the signal.
//19. Select ModAcc measurement and disable Traces.
//20. Initiate ModAcc measurement.
//21. Fetch ModAcc measurements.

//[3] Steps to perform SEM measurement:
//22. Stop signal generation.
//23. Configure LO Offset Mode for SEM measurement.
//    Set LO Offset Mode to Auto. This causes the RFSG LO to be placed outside the signal, if signal bandwidth is less than
//    half of the device instantaneous bandwidth; otherwise, the LO is placed at the center of the signal.
//    Set LO Offset Mode to No Offset when you see significant difference in the SEM upper and lower offset margin results.
//    This causes the RFSG LO to be placed at the center of the signal and avoids RFSG LO impacting the SEM offset results.
//24. Write script to generate the waveform specified in the script. This script is programmed to generate waveform continuously.
//25. Initiate signal generation.
//-------------------------------------------------------------------------------------------------------------------------------
//26. Select SEM measurement and disable the traces.
//27. Initiate SEM measurement.
//28. Fetch SEM measurements.

//[4] Steps:
//29. Close the RFmx Session.
//30. Close the RFSG session.
//It is recommended to clear the waveform before closing RFSG session.

using System;
using NationalInstruments.RFmx.InstrMX;
using NationalInstruments.RFmx.NRMX;
using NationalInstruments.ModularInstruments.NIRfsg;
using NationalInstruments.ModularInstruments.NIRfsgPlayback;

namespace NationalInstruments.Examples.RFmxNRFemTestWithAutomaticSGSASharedLO
{
   public class RFmxNRFemTestWithAutomaticSGSASharedLO
   {
      RFmxInstrMX instrSession;
      RFmxNRMX NR;
      NIRfsg rfsgSession;
      IntPtr instrumentHandle;

      double centerFrequency;

      string rfsgResourceName;
      string rfsgSelectedPorts;
      string waveformFilePath;
      string waveformName;

      double powerLevel;
      double rfsgExternalAttenuation;

      RfsgFrequencyReferenceSource rfsgFrequencyReferenceSource;
      double rfsgFrequency;

      string rfsaResourceName;
      string rfsaSelectedPorts;
      double referenceLevel;
      double rfsaExternalAttenuation;

      string rfsaFrequencyReferenceSource;
      double rfsaFrequency;

      bool enableTrigger;
      string digitalEdgeSource;
      RFmxNRMXDigitalEdgeTriggerEdge digitalEdge;
      double triggerDelay;

      RFmxNRMXFrequencyRange frequencyRange;
      double carrierBandwidth;
      double subcarrierSpacing;
      int band;
      int cellID;

      NIRfsgPlaybackLOOffsetMode rfsgLOOffsetMode;

      double timeout;
      string script;

      double compositeRmsEvmMean;                                             /* (%) */
      double compositePeakEvmMaximum;                                         /* (%) */

      RFmxNRMXSemMeasurementStatus measurementStatus;

      double absolutePower;                                                   /* (dBm) */
      double relativePower;
      double peakFrequency;
      double peakAbsolutePower;

      RFmxNRMXSemLowerOffsetMeasurementStatus[] lowerOffsetMeasurementStatus;
      double[] lowerOffsetMargin;                                             /* (dB) */
      double[] lowerOffsetMarginFrequency;                                    /* (Hz) */
      double[] lowerOffsetMarginAbsolutePower;                                /* (dBm) */
      double[] lowerOffsetMarginRelativePower;

      RFmxNRMXSemUpperOffsetMeasurementStatus[] upperOffsetMeasurementStatus;
      double[] upperOffsetMargin;                                             /* (dB) */
      double[] upperOffsetMarginFrequency;                                    /* (Hz) */
      double[] upperOffsetMarginAbsolutePower;                                /* (dBm) */
      double[] upperOffsetMarginRelativePower;

      public void Run()
      {
         try
         {
            InitializeVariables();
            ConfigureRfsg();
            ConfigureRFmxAndRetrieveResults();
            PrintResults();
         }
         catch (Exception ex)
         {
            DisplayError(ex);
         }
         finally
         {
            CloseSession();
            Console.WriteLine("Press any key to exit");
            Console.ReadKey();
         }
      }

      void InitializeVariables()
      {
         centerFrequency = 3.5e9;                                             /* (Hz) */

         rfsgResourceName = "RFSG";
         rfsgSelectedPorts = "";
         waveformFilePath = "NR_FR2_UL_SISO_CC-1_BW-50MHz_SCS-120kHz.tdms";
         waveformName = "Wfm";

         powerLevel = -10.0;                                                  /* (dBm) */
         rfsgExternalAttenuation = 0.0;                                       /* (dB) */

         rfsgFrequencyReferenceSource = RfsgFrequencyReferenceSource.OnboardClock;
         rfsgFrequency = 10.0e6;                                              /* (Hz) */

         rfsaResourceName = "RFSA";
         rfsaSelectedPorts = "";
         referenceLevel = 0.0;                                                /* (dBm) */
         rfsaExternalAttenuation = 0.0;                                       /* (dB) */

         rfsaFrequencyReferenceSource = RFmxInstrMXConstants.OnboardClock;
         rfsaFrequency = 10.0e6;                                              /* (Hz) */

         enableTrigger = false;
         digitalEdgeSource = RFmxNRMXConstants.PxiTriggerLine0;
         digitalEdge = RFmxNRMXDigitalEdgeTriggerEdge.Rising;
         triggerDelay = 0.0;                                                  /* (s) */

         frequencyRange = RFmxNRMXFrequencyRange.Range2;
         carrierBandwidth = 50e6;                                             /* (Hz) */
         subcarrierSpacing = 120e3;                                           /* (Hz) */
         band = 257;
         cellID = 0;

         rfsgLOOffsetMode = NIRfsgPlaybackLOOffsetMode.Auto;

         timeout = 10.0;                                                      /* (s) */

         script = "script GenerateWaveform\n  repeat forever\n    generate Wfm\n   end repeat\n  end script";
      }

      void ConfigureRfsg()
      {
         rfsgSession = new NIRfsg(rfsgResourceName, true, false);
         rfsgSession.SignalPath.SelectedPorts = rfsgSelectedPorts;
         rfsgSession.FrequencyReference.Configure(rfsgFrequencyReferenceSource, rfsgFrequency);
         rfsgSession.RF.Configure(centerFrequency, powerLevel);
         rfsgSession.RF.ExternalGain = -1 * rfsgExternalAttenuation;
         instrumentHandle = rfsgSession.GetInstrumentHandle().DangerousGetHandle();
         NIRfsgPlayback.ReadAndDownloadWaveformFromFile(instrumentHandle, waveformFilePath, waveformName);
         NIRfsgPlayback.StoreAutomaticSGSASharedLO(instrumentHandle, "", RfsgPlaybackAutomaticSGSASharedLO.Enabled);
         NIRfsgPlayback.StoreWaveformLOOffsetMode(instrumentHandle, waveformName, NIRfsgPlaybackLOOffsetMode.Auto);
         NIRfsgPlayback.SetScriptToGenerateSingleRfsg(instrumentHandle, script);
         rfsgSession.Initiate();
      }

      void ConfigureRFmxAndRetrieveResults()
      {
         instrSession = new RFmxInstrMX(rfsaResourceName, "");
         NR = instrSession.GetNRSignalConfiguration();
         instrSession.ConfigureFrequencyReference("", rfsaFrequencyReferenceSource, rfsaFrequency);
         NR.SetSelectedPorts("", rfsaSelectedPorts);
         instrSession.SetLOSource("", RFmxInstrMXConstants.LOSourceAutomaticSGSAShared);
         NR.ConfigureRF("", centerFrequency, referenceLevel, rfsaExternalAttenuation);
         NR.ConfigureDigitalEdgeTrigger("", digitalEdgeSource, digitalEdge, triggerDelay, enableTrigger);
         NR.SetLinkDirection("", RFmxNRMXLinkDirection.Uplink);
         NR.SetFrequencyRange("", frequencyRange);
         NR.ComponentCarrier.SetBandwidth("", carrierBandwidth);
         NR.ComponentCarrier.SetCellID("", cellID);
         NR.SetBand("", band);
         NR.ComponentCarrier.SetBandwidthPartSubcarrierSpacing("", subcarrierSpacing);
         instrSession.SetLOLeakageAvoidanceEnabled("", RFmxInstrMXLOLeakageAvoidanceEnabled.True);
         NR.SelectMeasurements("", RFmxNRMXMeasurementTypes.ModAcc, false);
         NR.Initiate("", "");

         RetrieveModAccResults();

         rfsgSession.Abort();
         NIRfsgPlayback.StoreWaveformLOOffsetMode(instrumentHandle, waveformName, rfsgLOOffsetMode);
         NIRfsgPlayback.SetScriptToGenerateSingleRfsg(instrumentHandle, script);
         rfsgSession.Initiate();

         NR.SelectMeasurements("", RFmxNRMXMeasurementTypes.Sem, false);
         NR.Initiate("", "");

         RetrieveSemResults();
      }

      void RetrieveModAccResults()
      {
         NR.ModAcc.Results.GetCompositeRmsEvmMean("", out compositeRmsEvmMean);
         NR.ModAcc.Results.GetCompositePeakEvmMaximum("", out compositePeakEvmMaximum);
      }

      void RetrieveSemResults()
      {
         NR.Sem.Results.FetchMeasurementStatus("", timeout, out measurementStatus);
         NR.Sem.Results.ComponentCarrier.FetchMeasurement("", timeout, out absolutePower, out peakAbsolutePower,
            out peakFrequency, out relativePower);
         NR.Sem.Results.FetchLowerOffsetMarginArray("", timeout, ref lowerOffsetMeasurementStatus, ref lowerOffsetMargin,
            ref lowerOffsetMarginFrequency, ref lowerOffsetMarginAbsolutePower, ref lowerOffsetMarginRelativePower);
         NR.Sem.Results.FetchUpperOffsetMarginArray("", timeout, ref upperOffsetMeasurementStatus, ref upperOffsetMargin,
            ref upperOffsetMarginFrequency, ref upperOffsetMarginAbsolutePower, ref upperOffsetMarginRelativePower);
      }

      void PrintResults()
      {
         Console.WriteLine("------------------ModAcc------------------\n");
         Console.WriteLine("------------------Measurement------------------\n");
         Console.WriteLine("Composite RMS EVM Mean (%)                     : {0}", compositeRmsEvmMean);
         Console.WriteLine("Composite Peak EVM Maximum (%)                 : {0}", compositePeakEvmMaximum);

         Console.WriteLine("\n------------------SEM------------------\n");
         Console.WriteLine("Measurement Status                       : {0}", measurementStatus);
         Console.WriteLine("Carrier Absolute Integrated Power (dBm)  : {0}", absolutePower);
         Console.WriteLine("\n----------Lower Offset Segment Measurements----------\n");
         for (int i = 0; i < lowerOffsetMargin.Length; i++)
         {
            Console.WriteLine("Offset {0}", i);
            Console.WriteLine("Measurement Status                 : {0}", lowerOffsetMeasurementStatus[i]);
            Console.WriteLine("Margin (dB)                        : {0}", lowerOffsetMargin[i]);
            Console.WriteLine("Margin Frequency (Hz)              : {0}", lowerOffsetMarginFrequency[i]);
            Console.WriteLine("Margin Absolute Power (dBm)        : {0}\n", lowerOffsetMarginAbsolutePower[i]);
         }
         Console.WriteLine("\n----------Upper Offset Segment Measurements----------\n");
         for (int i = 0; i < upperOffsetMargin.Length; i++)
         {
            Console.WriteLine("Offset {0}", i);
            Console.WriteLine("Measurement Status                 : {0}", upperOffsetMeasurementStatus[i]);
            Console.WriteLine("Margin (dB)                        : {0}", upperOffsetMargin[i]);
            Console.WriteLine("Margin Frequency (Hz)              : {0}", upperOffsetMarginFrequency[i]);
            Console.WriteLine("Margin Absolute Power (dBm)        : {0}\n", upperOffsetMarginAbsolutePower[i]);
         }
      }

      void CloseSession()
      {
         if (NR != null)
         {
            NR.Dispose();
            NR = null;
         }
         if (instrSession != null)
         {
            instrSession.Close();
            instrSession = null;
         }
         if (rfsgSession != null)
         {
            rfsgSession.Abort();
            NIRfsgPlayback.ClearWaveform(instrumentHandle, waveformName);
            rfsgSession.Close();
            rfsgSession = null;
         }
      }

      static void DisplayError(Exception ex)
      {
         Console.WriteLine("ERROR:\n" + ex.GetType() + ": " + ex.Message);
      }

   }
}
