//[1] Steps:
//1. Open an NI-RFSG session.
//2. Configure RFSG frequency reference  & waveform to script mode.
//3. Configure frequency and power level of RF output signal. 
//4. Set RFSG External Gain, Power Level Type and Pre-filter Gain.
//5. Export the Marker Event marker0 to the to the terminal specified by the user, which is also used as the source for the digital edge trigger on RFSA. 
//6. Configure RFSG LO Source to Automatic SG SA Shared.
//7. Read waveform from file and download it to RFSG.

//[2] Steps to perform ModAcc measurement:
//8.  Retrieve the waveform PAPR, Signal Bandwidth and IQ rate. Add the waveform PAPR to the RFSA reference level while configuring to every list step. 
//    Configure RFSG Signal Bandwidth, IQ rate, and PAPR. With the signal bandwidth configured and the Upconverter Frequency Offset Mode set to 
//    Automatic by default, the RFSG LO is placed outside the signal if the signal bandwidth is less than half of the device instantaneous bandwidth;
//    otherwise, the LO is placed at the center of the signal.
//9.  Write script to generate the waveform specified in the script. This script is programmed to generate waveform continuously. The marker0 is configured at sample0.
//10. Initiate signal generation.
//---------------------------------------------------------------------------------------------------------------------------------------------------
//11.Open a new RFmx session.
//12. Configure the Frequency Reference properties (Clock Source and Clock Frequency).
//13.Configure the basic signal properties  (Center Frequency, Reference Level and External Attenuation).
//14. Configure Digital Edge Trigger properties (Digital Edge Source, Digital Edge, Trigger Delay).
//15. Configure Standard and Channel Bandwidth properties.
//16. Set LO Source to Automatic_SG_SA_Shared.
//17. Set LO Leakage Avoidance Enabled to True. This causes RFmx to place the SA LO outside the measurement bandwidth, if the measurement bandwidth is less than half of the device instantaneous bandwidth; otherwise, the LO is placed at the center of the signal.
//18. Select OFDMModAcc measurement and disable traces.
//19. Configure OFDMModAcc Averaging properties (Averaging Enabled, Averaging Count, Averaging Type, Vector Averaging Time Alignment Enabled, Vector Averaging Phase Alignment Enabled).
//20. Initiate OFDMModAcc measurement.
//21. Fetch OFDMModAcc measurements.

//[3] Steps to perform SEM measurement:
//22. Stop signal generation.
//23. Initiate signal generation.
//---------------------------------------------------------------------------------------------------------------------------------------------------
//24. Select SEM measurement and disable traces. 
//25. Configure SEM Averaging properties (Averaging Enabled, Averaging Count, Averaging Type)
//26. Initiate SEM measurement.
//27. Fetch SEM measurements.

//[4] Steps:
//28. Close the RFmx Session.
//29. Close the RFSG session. 
//It is recommended to clear the waveform before closing RFSG session.

using NationalInstruments.ModularInstruments.NIRfsg;
using NationalInstruments.RFmx.InstrMX;
using NationalInstruments.RFmx.WlanMX;
using System;

namespace NationalInstruments.Examples.RFmxWlanFemTestWithAutomaticSGSASharedLO
{
   public class RFmxWlanFemTestWithAutomaticSGSASharedLO
   {
      RFmxInstrMX instrSession;
      RFmxWlanMX wlan;
      NIRfsg rfsgSession;

      double centerFrequency;

      string rfsgResourceName;
      string waveformFilePath;
      string waveformName;

      double powerLevel;
      double rfsgExternalAttenuation;

      RfsgFrequencyReferenceSource rfsgFrequencyReferenceSource;
      double rfsgFrequency;

      string rfsaResourceName;
      double referenceLevel;
      double rfsaExternalAttenuation;

      string rfsaFrequencyReferenceSource;
      double rfsaFrequency;

      bool digitalTriggerEnabled;
      RFmxWlanMXDigitalEdgeTriggerEdge digitalEdgeTriggerEdge;
      String digitalEdgeSource;
      double triggerDelay;

      RFmxWlanMXStandard standard;
      double channelBandwidth;

      RFmxWlanMXOfdmModAccAveragingEnabled ofdmModAccAveragingEnabled;
      Int32 ofdmModAccAveragingCount;
      RFmxWlanMXOfdmModAccAveragingType ofdmModAccAveragingType;
      RFmxWlanMXOfdmModAccVectorAveragingTimeAlignmentEnabled vectorAveragingTimeAlignmentEnabled;
      RFmxWlanMXOfdmModAccVectorAveragingPhaseAlignmentEnabled vectorAveragingPhaseAlignmentEnabled;

      RFmxWlanMXSemAveragingEnabled semAveragingEnabled;
      Int32 semAveragingCount;
      RFmxWlanMXSemAveragingType semAveragingType;

      double timeout;

      string script;
      int markerNumber;

      double compositeRmsEvmMean;                                             /* (dB) */
      double compositeDataRmsEvmMean;                                         /* (dB) */
      double compositePilotRmsEvmMean;                                        /* (dB) */

      RFmxWlanMXSemMeasurementStatus measurementStatus;

      double absolutePower;                                                   /* (dBm) */
      double relativePower;

      RFmxWlanMXSemLowerOffsetMeasurementStatus[] lowerOffsetMeasurementStatus;
      double[] lowerOffsetMargin;                                             /* (dB) */
      double[] lowerOffsetMarginFrequency;                                    /* (Hz) */
      double[] lowerOffsetMarginAbsolutePower;                                /* (dBm) */
      double[] lowerOffsetMarginRelativePower;

      RFmxWlanMXSemUpperOffsetMeasurementStatus[] upperOffsetMeasurementStatus;
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
         centerFrequency = 2.412e9;                                           /* (Hz) */

         rfsgResourceName = "RFSG";
         waveformFilePath = "WLAN_80211ac_BW-80MHz_SISO.tdms";
         waveformName = "Wfm";

         powerLevel = -10.0;                                                  /* (dBm) */
         rfsgExternalAttenuation = 0.0;                                       /* (dB) */

         rfsgFrequencyReferenceSource = RfsgFrequencyReferenceSource.OnboardClock;
         rfsgFrequency = 10.0e6;                                              /* (Hz) */

         rfsaResourceName = "RFSA";
         referenceLevel = 0.0;                                                /* (dBm) */
         rfsaExternalAttenuation = 0.0;                                       /* (dB) */

         rfsaFrequencyReferenceSource = RFmxInstrMXConstants.OnboardClock;
         rfsaFrequency = 10.0e6;                                              /* (Hz) */

         digitalTriggerEnabled = true;
         triggerDelay = 0.0;                                                  /* (s) */
         digitalEdgeTriggerEdge = RFmxWlanMXDigitalEdgeTriggerEdge.Rising;
         digitalEdgeSource = "PFI0";

         standard = RFmxWlanMXStandard.Standard802_11ac;
         channelBandwidth = 80e6;                                             /* (Hz) */

         ofdmModAccAveragingEnabled = RFmxWlanMXOfdmModAccAveragingEnabled.False;
         ofdmModAccAveragingCount = 10;
         ofdmModAccAveragingType = RFmxWlanMXOfdmModAccAveragingType.Rms;
         vectorAveragingTimeAlignmentEnabled = 
            RFmxWlanMXOfdmModAccVectorAveragingTimeAlignmentEnabled.True;
         vectorAveragingPhaseAlignmentEnabled = 
            RFmxWlanMXOfdmModAccVectorAveragingPhaseAlignmentEnabled.True;

         semAveragingEnabled = RFmxWlanMXSemAveragingEnabled.False;
         semAveragingCount = 10;
         semAveragingType = RFmxWlanMXSemAveragingType.Rms;

         timeout = 10.0;                                                      /* (s) */

         script = String.Format("script GenerateWaveform\n  repeat forever\n    generate {0} marker0(0)\n   end repeat\n  end script", waveformName);
         markerNumber = 0;

      }

      void ConfigureRfsg()
      {
         rfsgSession = new NIRfsg(rfsgResourceName, true, false);
         rfsgSession.FrequencyReference.Configure(rfsgFrequencyReferenceSource, rfsgFrequency);
         rfsgSession.Arb.GenerationMode = RfsgWaveformGenerationMode.Script;
         rfsgSession.RF.Configure(centerFrequency, powerLevel);
         rfsgSession.RF.PowerLevelType = RfsgRFPowerLevelType.PeakPower;
         rfsgSession.Arb.PreFilterGain = -1.5;
         rfsgSession.RF.ExternalGain = -1 * rfsgExternalAttenuation;
         rfsgSession.DeviceEvents.MarkerEvents[markerNumber].ExportedOutputTerminal =
                                  RfsgMarkerEventExportedOutputTerminal.Pfi0;
         rfsgSession.RF.LocalOscillator.Source = RfsgLocalOscillatorSource.AutomaticSGSAShared;
         rfsgSession.RF.Upconverter.FrequencyOffsetMode = UpconverterFrequencyOffsetMode.Auto;
         rfsgSession.Arb.ReadAndDownloadWaveformFromFileTdms(waveformName, waveformFilePath, 0);
         double waveformPapr = rfsgSession.Arb.Waveforms[waveformName].Papr;
         double waveformSignalBandwidth = rfsgSession.Arb.Waveforms[waveformName].SignalBandwidth;
         double waveformIqRate = rfsgSession.Arb.Waveforms[waveformName].IQRate;
         rfsgSession.Arb.SignalBandwidth = waveformSignalBandwidth;
         rfsgSession.Arb.IQRate = waveformIqRate;
         rfsgSession.RF.PeakPowerAdjustment = waveformPapr;
         rfsgSession.Arb.Scripting.WriteScript(script);
         rfsgSession.Initiate();
      }

      void ConfigureRFmxAndRetrieveResults()
      {
         instrSession = new RFmxInstrMX(rfsaResourceName, "");
         wlan = instrSession.GetWlanSignalConfiguration();
         instrSession.ConfigureFrequencyReference("", rfsaFrequencyReferenceSource, rfsaFrequency);
         wlan.ConfigureFrequency("", centerFrequency);
         wlan.ConfigureReferenceLevel("", referenceLevel);
         wlan.ConfigureExternalAttenuation("", rfsaExternalAttenuation);
         wlan.ConfigureDigitalEdgeTrigger("", digitalEdgeSource, digitalEdgeTriggerEdge, triggerDelay, digitalTriggerEnabled);
         wlan.ConfigureStandard("", standard);
         wlan.ConfigureChannelBandwidth("", channelBandwidth);
         instrSession.SetLOSource("", RFmxInstrMXConstants.LOSourceAutomaticSGSAShared);
         instrSession.SetLOLeakageAvoidanceEnabled("", RFmxInstrMXLOLeakageAvoidanceEnabled.True);
         wlan.SelectMeasurements("", RFmxWlanMXMeasurementTypes.OfdmModAcc, false);
         wlan.OfdmModAcc.Configuration.ConfigureAveraging("", ofdmModAccAveragingEnabled, ofdmModAccAveragingCount);
         wlan.OfdmModAcc.Configuration.SetAveragingType("", ofdmModAccAveragingType);
         wlan.OfdmModAcc.Configuration.SetVectorAveragingTimeAlignmentEnabled("", vectorAveragingTimeAlignmentEnabled);
         wlan.OfdmModAcc.Configuration.SetVectorAveragingPhaseAlignmentEnabled("", vectorAveragingPhaseAlignmentEnabled);
         wlan.Initiate("", "");

         RetrieveOfdmModAccResults();

         rfsgSession.Abort();
         rfsgSession.Initiate();

         wlan.SelectMeasurements("", RFmxWlanMXMeasurementTypes.Sem, false);
         wlan.Sem.Configuration.ConfigureAveraging("", semAveragingEnabled, semAveragingCount, semAveragingType);
         wlan.Initiate("", "");

         RetrieveSemResults();
      }

      void RetrieveOfdmModAccResults()
      {
         wlan.OfdmModAcc.Results.FetchCompositeRmsEvm("", timeout, out compositeRmsEvmMean, out compositeDataRmsEvmMean,
            out compositePilotRmsEvmMean);
      }

      void RetrieveSemResults()
      {
         wlan.Sem.Results.FetchMeasurementStatus("", timeout, out measurementStatus);
         wlan.Sem.Results.FetchCarrierMeasurement("", timeout, out absolutePower, out relativePower);
         wlan.Sem.Results.FetchLowerOffsetMarginArray("", timeout, ref lowerOffsetMeasurementStatus,
            ref lowerOffsetMargin, ref lowerOffsetMarginFrequency, ref lowerOffsetMarginAbsolutePower,
            ref lowerOffsetMarginRelativePower);
         wlan.Sem.Results.FetchUpperOffsetMarginArray("", timeout, ref upperOffsetMeasurementStatus,
            ref upperOffsetMargin, ref upperOffsetMarginFrequency, ref upperOffsetMarginAbsolutePower,
            ref upperOffsetMarginRelativePower);
      }

      void PrintResults()
      {
         Console.WriteLine("------------------OFDMModAcc------------------\n");
         Console.WriteLine("------------------Composite EVM------------------");
         Console.WriteLine("RMS EVM Mean (dB)                  : {0}", compositeRmsEvmMean);
         Console.WriteLine("Data RMS EVM Mean (dB)             : {0}", compositeDataRmsEvmMean);
         Console.WriteLine("Pilot RMS EVM Mean (dB)            : {0}\n", compositePilotRmsEvmMean);

         Console.WriteLine("\n------------------SEM------------------\n");
         Console.WriteLine("Measurement Status                 :{0}", measurementStatus);
         Console.WriteLine("Carrier Absolute Power (dBm)       :{0}", absolutePower);
         Console.WriteLine("\n----------Lower Offset Measurements----------\n");
         for (int i = 0; i < lowerOffsetMargin.Length; i++)
         {
            Console.WriteLine("Offset {0}", i);
            Console.WriteLine("Measurement Status                 :{0}", lowerOffsetMeasurementStatus[i]);
            Console.WriteLine("Margin (dB)                        :{0}", lowerOffsetMargin[i]);
            Console.WriteLine("Margin Frequency (Hz)              :{0}", lowerOffsetMarginFrequency[i]);
            Console.WriteLine("Margin Absolute Power (dBm)        :{0}\n", lowerOffsetMarginAbsolutePower[i]);
         }
         Console.WriteLine("\n----------Upper Offset Measurements----------\n");
         for (int i = 0; i < upperOffsetMargin.Length; i++)
         {
            Console.WriteLine("Offset {0}", i);
            Console.WriteLine("Measurement Status                 :{0}", upperOffsetMeasurementStatus[i]);
            Console.WriteLine("Margin (dB)                        :{0}", upperOffsetMargin[i]);
            Console.WriteLine("Margin Frequency (Hz)              :{0}", upperOffsetMarginFrequency[i]);
            Console.WriteLine("Margin Absolute Power (dBm)        :{0}\n", upperOffsetMarginAbsolutePower[i]);
         }
      }

      void CloseSession()
      {
         if (wlan != null)
         {
            wlan.Dispose();
            wlan = null;
         }
         if (instrSession != null)
         {
            instrSession.Close();
            instrSession = null;
         }
         if (rfsgSession != null)
         {
            rfsgSession.Abort();
            rfsgSession.Arb.ClearWaveform(waveformName);
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
