/* Steps:
1. Open RFSG session.
2. Configure RFSG frequency reference.
3. Configure marker0 to be generated from RFSG on the specified output terminal.
4. Configure frequency and power level of RF output signal.
5. Set RFSG External Gain. #4 and #5 ensure that the average power of the signal at
   the input of the DUT matches the user configured DUT Average Input Power.
6. a. Read waveform from file and download Waveform from file to RFSG
   b. Set Waveform Runtime Scaling to the negative of the desired Pre-filter Gain.
   c. Read waveform sample rate, multiply by 0.8 and set the result to the signal bandwidth.
   d. Write script to generate the waveform specified in the script.
   e. This script is programmed to generate waveform continuously, with marker0 aligned to sample index 0.
7. Initiate RFSG generation as per the selected script.
8. Open RFmx session.
9. Configure frequency reference of the analyser.
10. Configure Selected Ports.
11. Configure trigger to use as reference for signal acquisition.
12. Configure center frequency and external attenuation.
13. Select AMPM measurement, configure the reference waveform and power of this signal at the input of the DUT.
14. Set the measurement sample rate and the measurement interval to use for analysis.
15. Set threshold.
16. Configure Reference Power Type.
17. Set Reference Level or perform Auto Level to compute an approximate reference level to use by the analyser.
18. Initiate and fetch AMPM results.
19. Close RFmx session.
20. Close RFSG session.
It is recommended to clear the waveform before closing RFSG session.
*/

using System;
using NationalInstruments.RFmx.InstrMX;
using NationalInstruments.RFmx.SpecAnMX;
using NationalInstruments.DataInfrastructure;
using NationalInstruments.ModularInstruments.NIRfsg;
using NationalInstruments.ModularInstruments.NIRfsgPlayback;
using System.IO;

namespace NationalInstruments.Examples.RFmxSpecAnAmpm
{
   public class RFmxSpecAnAmpm
   {
      RFmxInstrMX instrSession;
      RFmxSpecAnMX specAn;
      NIRfsg rfsgSession;
      IntPtr instrumentHandle;

      string rfsaResourceName = "RFSA";
      string rfsgResourceName = "RFSG";

      string frequencyReferenceSource = RFmxInstrMXConstants.OnboardClock;
      double frequencyReferenceFrequency = 10e6;      /* Hz */
      RFmxSpecAnMXTriggerType triggerType = RFmxSpecAnMXTriggerType.DigitalEdge;
      double triggerDelay = 0;                        /* seconds */
      bool enableTrigger = true;
      string digitalEdgeTriggerSource = RFmxSpecAnMXConstants.PxiTriggerLine0;
      string markerEventExportedOutputTerminal = RfsgMarkerEventExportedOutputTerminal.PxiTriggerLine0;
      RFmxSpecAnMXDigitalEdgeTriggerEdge digitalEdgeTriggerEdge = RFmxSpecAnMXDigitalEdgeTriggerEdge.Rising;
      string iqPowerEdgeTriggerSource = "0";
      double iqPowerEdgeTriggerLevel = -20;
      RFmxSpecAnMXIQPowerEdgeTriggerSlope iqPowerEdgeSlope = RFmxSpecAnMXIQPowerEdgeTriggerSlope.Rising;
      RFmxSpecAnMXTriggerMinimumQuietTimeMode minimumQuietTimeMode = RFmxSpecAnMXTriggerMinimumQuietTimeMode.Manual;
      double minimumQuietTimeDuration = 0;            /* seconds */
      string selectedPorts = "";
      double centerFrequency = 1e+9;                  /* Hz */
      double rfsaExternalAttenuation = 0.00;          /* dB */
      double rfsgExternalAttenuation = 0.00;          /* dB */
      bool autoLevel = true;
      double referenceLevel = -14.00;                 /* dBm */
      double signalBandwidth = 20e6;                  /* Hz */
      double autoLevelMeasurementInterval = 100e-6;   /* seconds */
      double autoLevelReferenceLevel;
      double preFilterGain = -4.00;                   /* dB */
      double runtimeScaling;
      double dutAverageInputPower = -20;              /* dBm */
      RFmxSpecAnMXAmpmMeasurementSampleRateMode sampleRateMode =
                                                RFmxSpecAnMXAmpmMeasurementSampleRateMode.ReferenceWaveform;
      double sampleRate = 120e6;                      /* S/s */
      double measurementInterval = 100e-6;            /* seconds */
      RFmxSpecAnMXAmpmThresholdEnabled thresholdEnabled = RFmxSpecAnMXAmpmThresholdEnabled.True;
      double thresholdLevel = -20;                    /* dB or dBm */
      RFmxSpecAnMXAmpmThresholdType thresholdType = RFmxSpecAnMXAmpmThresholdType.Relative;
      RFmxSpecAnMXAmpmReferencePowerType referencePowerType = RFmxSpecAnMXAmpmReferencePowerType.Input;

      ComplexWaveform<ComplexSingle> referenceWaveformSingle;

      string waveformFileName = "LTE20MHz Waveform (Two Subframes).tdms";
      RFmxSpecAnMXAmpmReferenceWaveformIdleDurationPresent idleDurationPresent =
                                                           RFmxSpecAnMXAmpmReferenceWaveformIdleDurationPresent.False;
      RFmxSpecAnMXAmpmSignalType signalType = RFmxSpecAnMXAmpmSignalType.Modulated;
      double timeout = 10;                            /* seconds */

      RfsgFrequencyReferenceSource referenceClockSource = RfsgFrequencyReferenceSource.OnboardClock;
      double referenceClockRate = 10e6;
      string scriptName = "AMPMScript";
      string waveformName = "Wfm";
      int markerNumber = 0;
      string waveformScript;

      double meanLinearGain, onedBCompressionPoint, meanRmsEvm,
             gainErrorRange, phaseErrorRange, meanPhaseError,
             amToAMResidual, amToPMResidual;

      float[] referencePowersAMToAM;
      float[] measuredAMToAM;
      float[] curveFitAMToAM;
      float[] referencePowersAMToPM;
      float[] measuredAMToPM;
      float[] curveFitAMToPM;

      internal void Run()
      {
         try
         {
            ReadWaveformFromTdmsFile();
            ConfigureRfsg();
            ConfigureRFmx();
            RetrieveResults();
            PrintResults();
         }
         catch (Exception ex)
         {
            DisplayError(ex);
         }
         finally
         {
            CloseSessions();
            Console.WriteLine("Press any key to exit.....");
            Console.ReadKey();
         }
      }

      private void ConfigureRfsg()
      {
         /* Configure RFSG */
         rfsgSession = new NIRfsg(rfsgResourceName, true, true);
         rfsgSession.FrequencyReference.Configure(referenceClockSource, referenceClockRate);
         rfsgSession.DeviceEvents.MarkerEvents[markerNumber].ExportedOutputTerminal = markerEventExportedOutputTerminal;
         rfsgSession.RF.Configure(centerFrequency, dutAverageInputPower);
         waveformScript =
                 "script " + scriptName + "\n" +
                 "      Repeat forever\n" +
                 "         Generate " + waveformName + " marker" + markerNumber + "(0)\n" +
                 "      end repeat\n" +
                 "end script";
         rfsgSession.RF.ExternalGain = -rfsgExternalAttenuation;
         instrumentHandle = rfsgSession.GetInstrumentHandle().DangerousGetHandle();
         NIRfsgPlayback.ReadAndDownloadWaveformFromFile(instrumentHandle, waveformFileName, waveformName);
         runtimeScaling = preFilterGain;
         NIRfsgPlayback.StoreWaveformRuntimeScaling(instrumentHandle, waveformName, runtimeScaling);
         NIRfsgPlayback.RetrieveWaveformSampleRate(instrumentHandle, waveformName, out sampleRate);
         NIRfsgPlayback.StoreWaveformSignalBandwidth(instrumentHandle, waveformName, 0.8 * sampleRate);
         NIRfsgPlayback.SetScriptToGenerateSingleRfsg(instrumentHandle, waveformScript);
         rfsgSession.Initiate();
      }

      private void ConfigureRFmx()
      {
         /* Configure RFmx */
         instrSession = new RFmxInstrMX(rfsaResourceName, "");
         specAn = instrSession.GetSpecAnSignalConfiguration();
         instrSession.ConfigureFrequencyReference("", frequencyReferenceSource, frequencyReferenceFrequency);
         specAn.SetSelectedPorts("", selectedPorts);

         if (triggerType.Equals(RFmxSpecAnMXTriggerType.DigitalEdge))
         {
            specAn.ConfigureDigitalEdgeTrigger("", digitalEdgeTriggerSource, digitalEdgeTriggerEdge,
                                               triggerDelay, enableTrigger);
         }
         else if (triggerType.Equals(RFmxSpecAnMXTriggerType.IQPowerEdge))
         {
            specAn.ConfigureIQPowerEdgeTrigger("", iqPowerEdgeTriggerSource, iqPowerEdgeTriggerLevel,
                                               iqPowerEdgeSlope, triggerDelay, minimumQuietTimeMode,
                                               minimumQuietTimeDuration, enableTrigger);
         }

         specAn.ConfigureRF("", centerFrequency, referenceLevel, rfsaExternalAttenuation);

         specAn.SelectMeasurements("", RFmxSpecAnMXMeasurementTypes.Ampm, true);
         specAn.Ampm.Configuration.ConfigureDutAverageInputPower("", dutAverageInputPower);
         specAn.Ampm.Configuration.ConfigureReferenceWaveform("", referenceWaveformSingle, idleDurationPresent,
                                                              signalType);
         specAn.Ampm.Configuration.ConfigureMeasurementSampleRate("", sampleRateMode, sampleRate);
         specAn.Ampm.Configuration.ConfigureMeasurementInterval("", measurementInterval);
         specAn.Ampm.Configuration.ConfigureThreshold("", thresholdEnabled, thresholdLevel, thresholdType);
         specAn.Ampm.Configuration.ConfigureReferencePowerType("", referencePowerType);

         if (autoLevel)
         {
            specAn.AutoLevel("", signalBandwidth, autoLevelMeasurementInterval, out autoLevelReferenceLevel);
            Console.WriteLine("Reference Level(dBm): {0}\n", autoLevelReferenceLevel);
         }
         else
         {
            specAn.ConfigureReferenceLevel("", referenceLevel);
         }

         specAn.Initiate("", "");
      }

      private void RetrieveResults()
      {
         specAn.Ampm.Results.FetchDutCharacteristics("", timeout, out meanLinearGain, out onedBCompressionPoint,
                                                     out meanRmsEvm);
         specAn.Ampm.Results.FetchError("", timeout, out gainErrorRange, out phaseErrorRange, out meanPhaseError);
         specAn.Ampm.Results.FetchCurveFitResidual("", timeout, out amToAMResidual, out amToPMResidual);
         specAn.Ampm.Results.FetchAMToAMTrace("", timeout, ref referencePowersAMToAM, ref measuredAMToAM, ref curveFitAMToAM);
         specAn.Ampm.Results.FetchAMToPMTrace("", timeout, ref referencePowersAMToPM, ref measuredAMToPM, ref curveFitAMToPM);

         rfsgSession.Abort();
         NIRfsgPlayback.ClearWaveform(instrumentHandle, waveformName);
      }

      private void PrintResults()
      {
         Console.WriteLine("-----------------Measurement-----------------\n");
         Console.WriteLine("Mean Linear Gain (dB)            {0}", meanLinearGain);
         Console.WriteLine("Mean Phase Error (deg)           {0}", meanPhaseError);
         Console.WriteLine("Mean RMS EVM (%)                 {0}", meanRmsEvm);
         Console.WriteLine("AM to AM Residual (dB)           {0}", amToAMResidual);
         Console.WriteLine("AM to PM Residual (deg)          {0}", amToPMResidual);
         Console.WriteLine("Gain Error Range (dB)            {0}", gainErrorRange);
         Console.WriteLine("Phase Error Range (deg)          {0}", phaseErrorRange);
         Console.WriteLine("1 dB Compression Point (dBm)     {0}", onedBCompressionPoint);
      }

      private void ReadWaveformFromTdmsFile()
      {
         NIRfsgPlayback.ReadWaveformFromFileComplex(waveformFileName, ref referenceWaveformSingle);
      }

      private void CloseSessions()
      {
         try
         {
            if (specAn != null)
            {
               specAn.Dispose();
               specAn = null;
            }
            if (instrSession != null)
            {
               instrSession.Close();
               instrSession = null;
            }
            if (rfsgSession != null)
            {
               rfsgSession.Close();
               rfsgSession = null;
            }
         }
         catch (Exception ex)
         {
            DisplayError(ex);
         }
      }

      private static void DisplayError(Exception ex)
      {
         Console.WriteLine("ERROR:\n" + ex.GetType() + ": " + ex.Message);
      }

      private static ComplexWaveform<ComplexSingle> ComplexDoubleWaveformToComplexSingleWaveform(ComplexWaveform<ComplexDouble> waveform)
      {
         ComplexWaveform<ComplexSingle> retWaveForm;
         ComplexDouble[] tempArrayDouble = waveform.GetRawData();
         ComplexSingle[] tempArraySingle = new ComplexSingle[tempArrayDouble.Length];
         for (int i = 0; i < tempArraySingle.Length; i++)
         {
            tempArraySingle[i].Real = (float)tempArrayDouble[i].Real;
            tempArraySingle[i].Imaginary = (float)tempArrayDouble[i].Imaginary;
         }
         retWaveForm = ComplexWaveform<ComplexSingle>.FromArray1D(tempArraySingle);
         retWaveForm.PrecisionTiming = PrecisionWaveformTiming.CreateWithRegularInterval
                                            (new PrecisionTimeSpan(waveform.PrecisionTiming.SampleInterval.TotalSeconds),
                                            new PrecisionTimeSpan(waveform.PrecisionTiming.TimeOffset.TotalSeconds));

         return retWaveForm;
      }

   }
}
