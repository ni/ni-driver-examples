/* Steps:
1. Open RFSG session.
2. Configure RFSG frequency reference and Generation mode to Script.
3. Configure marker0 to be generated from RFSG on the specified output terminal.
4. Configure frequency and power level of RF output signal.
5. Set RFSG External Gain. #4 and #5 ensure that the average power of the signal at the input of the DUT
   matches the user configured DUT Average Input Power.
   Configure RFSG Power Level Type and Upconverter Frequency Offset Mode
6. Read waveform from file and download Waveform from file to RFSG.
   Configure RFSG IQ Rate and Pre-filter Gain.
   Read waveform sample rate, multiply by 0.8 and set the result to the signal bandwidth.
   Write script to generate the waveform specified in the script. This script is programmed
to generate waveform continuously, with marker0 aligned to sample index 0.
7. Initiate generation.
8. Open RFmx session.
9. Configure frequency reference of the analyser.
10. Configure Selected Ports.
11. Configure trigger to use as reference for signal acquisition.
12. Configure center frequency and external attenuation.
13. Select IDPD measurement, configure the reference waveform.
14. Configure the equalizer coefficients for 'Hold' mode of Equalizer.
15. Set power of configured reference signal at the input of the DUT.
      Set the measurement sample rate and the measurement interval to use for analysis.
      Set equalizer mode.
      Configure averaging and EVM Enabled with EVM unit.
      Set start and stop for impairment estimation and synchronization estimation.
      Configure gain expansion (dB) and power linearity tradeoff (%).
16. Perform Auto Level to compute an approximate reference level to use by the analyzer and adjust
      the reference level to account for PAPR changes after applying DPD.
17. Configure predistorted waveform obtained from previous iteration.  
18. Initiates IDPD measurement.
19. Fetch predistorted waveform and scale from -1 to 1 as RFSG power level type is peak power and
      fetch RMS EVM.
20. Abort RFSG generation and write a new Predistorted Waveform.
      Set Waveform Runtime Scaling to the desired Pre-filter Gain.
      Set the sample rate computed from Predistorted Waveform.
      Set the final PAPR to the sum of the actual PAPR and the Power Offset as computed from
      Predistorted Waveform.
      Set the Signal Bandwidth.
      Initiate RFSG generation using the script that was selected earlier
21. Configure appropriate trigger delay for AMPM measurement based on burst location in the Waveform.
22. Perform Auto Level to compute an approximate reference level while generating the predistorted waveform.
23. Select and configure AMPM measurement in RFmx after IDPD measurement is complete.
      AMPM measurement is used to measure the AM-AM and AM-PM response of the DUT.
24. Initiate and fetch AMPM results.
25. Close RFmx session.
26. Close RFSG session.
It is recommended to clear the waveform before closing RFSG session.*/

using System;
using NationalInstruments.RFmx.InstrMX;
using NationalInstruments.RFmx.SpecAnMX;
using NationalInstruments.ModularInstruments.NIRfsg;
using NationalInstruments.ModularInstruments.NIRfsgPlayback;

namespace NationalInstruments.Examples.RFmxSpecAnIdpd
{
   public class RFmxSpecAnIdpd
   {
      RFmxInstrMX instrSession;
      RFmxSpecAnMX specAn;
      NIRfsg rfsgSession;

      string rfsaResourceName = "RFSA";
      string rfsgResourceName = "RFSG";

      string referenceWaveformFile = @"LTE20MHz Waveform (Two Subframes).tdms";

      RfsgFrequencyReferenceSource referenceClockSource = RfsgFrequencyReferenceSource.OnboardClock;
      double referenceClockRate = 10e6;
      double triggerDelay = 0;                        /* seconds */
      double dutAverageInputPower = -20;              /* dBm */
      string selectedPorts = "";
      double centerFrequency = 1e+9;                  /* Hz */
      double referenceLevel = 0.00;                   /* dBm */
      double rfsaExternalAttenuation = 0.00;          /* dB */
      double rfsgExternalAttenuation = 0.00;          /* dB */
      double preFilterGain = -1.5;                    /* dB */
      double runtimeScaling;
      double papr = 0.0;

      string frequencyReferenceSource = RFmxInstrMXConstants.OnboardClock;
      double frequencyReferenceFrequency = 10e6;      /* Hz */
      bool enableTrigger = true;
      RFmxSpecAnMXTriggerType triggerType = RFmxSpecAnMXTriggerType.DigitalEdge;

      RFmxSpecAnMXIdpdMeasurementSampleRateMode sampleRateMode = RFmxSpecAnMXIdpdMeasurementSampleRateMode.ReferenceWaveform;
      double sampleRate = 120e6;                      /* S/s */
      RFmxSpecAnMXIdpdReferenceWaveformIdleDurationPresent idleDurationPresent = RFmxSpecAnMXIdpdReferenceWaveformIdleDurationPresent.False;
      RFmxSpecAnMXIdpdSignalType signalType = RFmxSpecAnMXIdpdSignalType.Modulated;
      int numberOfIterations = 10;
      double gainExpansion = 3.00;                    /* dB */
      double powerLinearityTradeoff = 50.00;          /* % */
      RFmxSpecAnMXIdpdEqualizerMode equalizerMode = RFmxSpecAnMXIdpdEqualizerMode.Off;
      ComplexSingle[] equalizerCoefficients;
      double startTime = 0.00;                        /* seconds */
      double stopTime = 100e-6;                       /* seconds */
      RFmxSpecAnMXIdpdEvmEnabled idpdEvmEnabled = RFmxSpecAnMXIdpdEvmEnabled.True;
      RFmxSpecAnMXIdpdEvmUnit evmUnit = RFmxSpecAnMXIdpdEvmUnit.dB;
      double targetGain = 20;                         /* dB */
      double gain = 0;

      double signalBandwidth = 20e6;                  /* Hz */
      double autoLevelMeasurementInterval = 100e-6;   /* seconds */
      double autoLevelReferenceLevel;

      RFmxSpecAnMXAmpmReferenceWaveformIdleDurationPresent ampmIdleDurationPresent = RFmxSpecAnMXAmpmReferenceWaveformIdleDurationPresent.False;
      RFmxSpecAnMXAmpmSignalType ampmSignalType = RFmxSpecAnMXAmpmSignalType.Modulated;
      RFmxSpecAnMXAmpmReferencePowerType referencePowerType = RFmxSpecAnMXAmpmReferencePowerType.Input;
      RFmxSpecAnMXAmpmThresholdEnabled thresholdEnabled = RFmxSpecAnMXAmpmThresholdEnabled.True;

      double timeout = 10;                            /* seconds */

      string scriptName = "IDPDScript";
      string waveformName = "Wfm";
      int waveformSize;
      double rfsgIqRate;
      int markerNumber = 0;
      string waveformScript;

      double[] idpdMeanRmsEvm;
      ComplexWaveform<ComplexSingle> predistortedWaveform, normalizePredistortedWaveform;
      ComplexWaveform<ComplexSingle> referenceWaveformComplexSingle;

      double meanLinearGain, onedBCompressionPoint, meanRmsEvm,
            gainErrorRange, phaseErrorRange, meanPhaseError,
            amToAMResidual, amToPMResidual, powerOffset;

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
            ReadWaveFormSizeFromTdmsFile();
            OpenSession();
            ConfigureRfsg();
            ConfigureRFmx();
            RetrieveResults();
            DisplayResults();
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

      private void ReadWaveformFromTdmsFile()
      {
         NIRfsgPlayback.ReadWaveformFromFileComplex(referenceWaveformFile, ref referenceWaveformComplexSingle);
      }

      private void ReadWaveFormSizeFromTdmsFile()
      {
         NIRfsgPlayback.ReadWaveformSizeFromFile(referenceWaveformFile, 0, out waveformSize);
      }

      private void OpenSession()
      {
         instrSession = new RFmxInstrMX(rfsaResourceName, "");
         specAn = instrSession.GetSpecAnSignalConfiguration();
      }

      private void ConfigureRfsg()
      {
         rfsgSession = new NIRfsg(rfsgResourceName, false, true);
         rfsgSession.Arb.GenerationMode = RfsgWaveformGenerationMode.Script;
         rfsgSession.RF.PowerLevelType = RfsgRFPowerLevelType.PeakPower;
         rfsgSession.FrequencyReference.Configure(referenceClockSource, referenceClockRate);
         rfsgSession.DeviceEvents.MarkerEvents[markerNumber].ExportedOutputTerminal =
                                 RfsgMarkerEventExportedOutputTerminal.PxiTriggerLine0;
         rfsgSession.RF.Configure(centerFrequency, dutAverageInputPower);
         rfsgSession.RF.Upconverter.FrequencyOffsetMode = UpconverterFrequencyOffsetMode.Auto;
         rfsgSession.RF.ExternalGain = -rfsgExternalAttenuation;
         rfsgSession.Arb.ReadAndDownloadWaveformFromFileTdms(waveformName, referenceWaveformFile, 0);
         rfsgIqRate = rfsgSession.Arb.Waveforms[waveformName].IQRate;
         runtimeScaling = preFilterGain;
         rfsgSession.Arb.PreFilterGain = runtimeScaling;
         rfsgSession.Arb.SignalBandwidth = 0.8 * rfsgIqRate;
         waveformScript = String.Format("script {0}{1}repeat forever{1}generate {2} marker{3}(0){1}end repeat{1}end script",
            scriptName, Environment.NewLine, waveformName, markerNumber);
         rfsgSession.Arb.Scripting.WriteScript(waveformScript);
         rfsgSession.Initiate();
      }

      private void ConfigureRFmx()
      {
         instrSession.ConfigureFrequencyReference("", frequencyReferenceSource, frequencyReferenceFrequency);
         specAn.SetSelectedPorts("", selectedPorts);
         if (triggerType == RFmxSpecAnMXTriggerType.DigitalEdge)
         {
            specAn.ConfigureDigitalEdgeTrigger("", RFmxInstrMXConstants.PxiTriggerLine0,
               RFmxSpecAnMXDigitalEdgeTriggerEdge.Rising, triggerDelay, enableTrigger);
         }
         else if (triggerType == RFmxSpecAnMXTriggerType.IQPowerEdge)
         {
            specAn.ConfigureIQPowerEdgeTrigger("", "0", -20.0,
               RFmxSpecAnMXIQPowerEdgeTriggerSlope.Rising, triggerDelay,
               RFmxSpecAnMXTriggerMinimumQuietTimeMode.Manual, 0.0, enableTrigger);
         }
         specAn.ConfigureRF("", centerFrequency, referenceLevel, rfsaExternalAttenuation);
         specAn.SelectMeasurements("", RFmxSpecAnMXMeasurementTypes.Idpd, true);
         specAn.Idpd.Configuration.ConfigureReferenceWaveform("", referenceWaveformComplexSingle, idleDurationPresent,
                                                            signalType);
         specAn.Idpd.Configuration.ConfigureEqualizerCoefficients("", 0.00E+0, 0.00E+0, equalizerCoefficients);
         specAn.Idpd.Configuration.SetDutAverageInputPower("", dutAverageInputPower);
         specAn.Idpd.Configuration.SetMeasurementSampleRateMode("", sampleRateMode);
         specAn.Idpd.Configuration.SetMeasurementSampleRate("", sampleRate);
         specAn.Idpd.Configuration.SetEqualizerMode("", equalizerMode);
         specAn.Idpd.Configuration.SetEvmEnabled("", idpdEvmEnabled);
         specAn.Idpd.Configuration.SetEvmUnit("", evmUnit);
         specAn.Idpd.Configuration.SetGainExpansion("", gainExpansion);
         specAn.Idpd.Configuration.SetPowerLinearityTradeoff("", powerLinearityTradeoff);
         specAn.Idpd.Configuration.SetImpairmentEstimationStart("", startTime);
         specAn.Idpd.Configuration.SetSynchronizationEstimationStart("", startTime);
         specAn.Idpd.Configuration.SetImpairmentEstimationStop("", stopTime);
         specAn.Idpd.Configuration.SetSynchronizationEstimationStop("", stopTime);

         specAn.AutoLevel("", signalBandwidth, autoLevelMeasurementInterval, out autoLevelReferenceLevel);
      }

      private void RetrieveResults()
      {
         idpdMeanRmsEvm = new double[numberOfIterations];
         for (int i = 0; i < numberOfIterations; i++)
         {
            specAn.Idpd.Configuration.ConfigurePredistortedWaveform("", predistortedWaveform, targetGain);
            specAn.Initiate("", "");
            specAn.Idpd.Results.FetchPredistortedWaveform("", timeout, ref predistortedWaveform, out papr, out powerOffset, out gain);
            specAn.Idpd.Results.GetMeanRmsEvm("", out idpdMeanRmsEvm[i]);

            /* Storing the predistorted waveform again to normalize it */
            double x0 = predistortedWaveform.PrecisionTiming.TimeOffset.TotalSeconds;
            double dx = predistortedWaveform.PrecisionTiming.SampleInterval.TotalSeconds;
            normalizePredistortedWaveform = new ComplexWaveform<ComplexSingle>(0);
            normalizePredistortedWaveform.Append(predistortedWaveform.GetRawData());
            PrecisionWaveformTiming precisionTiming = PrecisionWaveformTiming.CreateWithRegularInterval(new PrecisionTimeSpan(dx), new PrecisionTimeSpan(x0));
            normalizePredistortedWaveform.PrecisionTiming = precisionTiming;

            /*Normalization*/
            ComplexSingle[] predistortedWaveformRawData = normalizePredistortedWaveform.GetRawData();
            float maxElement = predistortedWaveformRawData[0].Magnitude;

            for (int k = 1; k < predistortedWaveformRawData.Length; k++)
            {
               if (predistortedWaveformRawData[k].Magnitude > maxElement)
               {
                  maxElement = predistortedWaveformRawData[k].Magnitude;
               }
            }

            for (int j = 0; j < predistortedWaveformRawData.Length; j++)
            {
               predistortedWaveformRawData[j] /= new ComplexSingle(maxElement, 0);
            }

            normalizePredistortedWaveform = new ComplexWaveform<ComplexSingle>(0);
            normalizePredistortedWaveform.Append(predistortedWaveformRawData);
            normalizePredistortedWaveform.PrecisionTiming = precisionTiming;

            targetGain = gain;

            rfsgSession.Abort();
            rfsgSession.Arb.ClearWaveform(waveformName);
            rfsgIqRate = 1 / normalizePredistortedWaveform.PrecisionTiming.SampleInterval.TotalSeconds;
            rfsgSession.Arb.WriteWaveform(waveformName, normalizePredistortedWaveform);
            rfsgSession.Arb.PreFilterGain = runtimeScaling;
            rfsgSession.Arb.IQRate = rfsgIqRate;
            rfsgSession.Arb.Waveforms[waveformName].Papr = (papr + powerOffset);
            rfsgSession.Arb.SignalBandwidth = 0.8 * rfsgIqRate;
            rfsgSession.Arb.Scripting.WriteScript(waveformScript);
            rfsgSession.Initiate();
         }

         specAn.SetTriggerDelay("", triggerDelay);
         specAn.AutoLevel("", signalBandwidth, autoLevelMeasurementInterval, out autoLevelReferenceLevel);
         specAn.SelectMeasurements("", RFmxSpecAnMXMeasurementTypes.Ampm, true);
         specAn.Ampm.Configuration.ConfigureReferenceWaveform("", referenceWaveformComplexSingle, ampmIdleDurationPresent, ampmSignalType);
         specAn.Ampm.Configuration.SetDutAverageInputPower("", dutAverageInputPower);
         specAn.Ampm.Configuration.SetMeasurementSampleRateMode("", RFmxSpecAnMXAmpmMeasurementSampleRateMode.ReferenceWaveform);
         specAn.Ampm.Configuration.SetMeasurementSampleRate("", sampleRate);
         specAn.Ampm.Configuration.SetMeasurementInterval("", stopTime - startTime);
         specAn.Ampm.Configuration.SetReferencePowerType("", referencePowerType);
         specAn.Ampm.Configuration.SetThresholdEnabled("", thresholdEnabled);

         specAn.Initiate("", "");

         specAn.Ampm.Results.FetchDutCharacteristics("", timeout, out meanLinearGain, out onedBCompressionPoint, out meanRmsEvm);
         specAn.Ampm.Results.FetchError("", timeout, out gainErrorRange, out phaseErrorRange, out meanPhaseError);
         specAn.Ampm.Results.FetchCurveFitResidual("", timeout, out amToAMResidual, out amToPMResidual);
         specAn.Ampm.Results.FetchAMToAMTrace("", timeout, ref referencePowersAMToAM, ref measuredAMToAM, ref curveFitAMToAM);
         specAn.Ampm.Results.FetchAMToPMTrace("", timeout, ref referencePowersAMToPM, ref measuredAMToPM, ref curveFitAMToPM);

         rfsgSession.Abort();
         rfsgSession.Arb.ClearWaveform(waveformName);
      }

      private void DisplayResults()
      {
         Console.WriteLine("-----------------AMPM Measurement-----------------\n");
         Console.WriteLine("Mean Linear Gain (dB)            {0}", meanLinearGain);
         Console.WriteLine("Mean Phase Error (deg)           {0}", meanPhaseError);
         Console.WriteLine("Mean RMS EVM (%)                 {0}", meanRmsEvm);
         Console.WriteLine("AM to AM Residual (dB)           {0}", amToAMResidual);
         Console.WriteLine("AM to PM Residual (deg)          {0}", amToPMResidual);
         Console.WriteLine("Gain Error Range (dB)            {0}", gainErrorRange);
         Console.WriteLine("Phase Error Range (deg)          {0}", phaseErrorRange);
         Console.WriteLine("1 dB Compression Point (dBm)     {0}", onedBCompressionPoint);
         Console.WriteLine("\n-----------------IDPD Measurement-----------------\n");
         Console.WriteLine("IDPD RMS EVM Mean (% or dB) per iteration:");
         for (int i = 0; i < idpdMeanRmsEvm.Length; i++)
         {
            Console.WriteLine(idpdMeanRmsEvm[i]);
         }
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
   }
}
