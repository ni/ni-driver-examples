/* Steps:
1. Open RFSG session.
2. Configure RFSG frequency reference and Generation mode to Script.
3. Configure marker0 to be generated from RFSG on the specified output terminal.
4. Configure frequency and power level of RF output signal.
5. Set RFSG External Gain and Power Level Type. #4 and #5 ensure that the average power of the signal at the input of the DUT
   matches the user configured DUT Average Input Power.
6. Read waveform from file and download Waveform from file to RFSG.
   Set RFSG IQ Rate and Pre-filter Gain.
   Read waveform sample rate, multiply by 0.8 and set the result to the signal bandwidth.
   Write script to generate the waveform specified in the script. This script is programmed
to generate waveform continuously, with marker0 aligned to sample index 0.
7. Initiate generation.
8. Open RFmx session.
9. Configure frequency reference of the analyser.
10. Configure Selected Ports.
11. Configure trigger to use as reference for signal acquisition.
12. Configure center frequency and external attenuation.
13. Select DPD measurement, configure the reference waveform
    and power of this signal at the input of the DUT.
14. Select and configure the Memory polynomial or Generalized memory polynomial model
    to estimate the predistotor.
15. Set the measurement sample rate and the measurement interval to use for analysis.
16. Enable iterative DPD.
17. Perform Auto Level to compute an approximate reference level to use by the analyser.
18. Configure the Memory models Correction type.
19. Initiates DPD measurement and then applies the DPD polynomial to
    remove the effects of memory and nonlinearity introduced by the DUT.
20. Set the previous iteration polynomial, in case DPD is measured iteratively.
21. Fetch DPD Polynomial.
22. Abort RFSG generation and write a new waveform that is predistorted by applying LUT.
    Set Waveform Runtime Scaling to the negative of the desired Pre-filter Gain.
    Set the sample rate computed from Apply Digital Predistortion.
    Set the final PAPR to the sum of the actual PAPR and the Power Offset as computed
    by Apply Digital Predistortion.
    Set the Signal Bandwidth.
    Initiate RFSG generation using the script that was selected earlier.
23. Perform Auto Level to compute an approximate reference level while generating the predistorted waveform.
24. Select and configure AMPM measurement in RFmx after DPD measurement is complete.
    AMPM measurement is used to inspect the measure the AM-AM and AM-PM response of the DUT.
25. Initiate and fetch AMPM results.
26. Close RFmx session.
27. Close RFSG session.
It is recommended to clear the waveform before closing RFSG session.*/

using System;
using NationalInstruments.RFmx.InstrMX;
using NationalInstruments.RFmx.SpecAnMX;
using NationalInstruments.DataInfrastructure;
using NationalInstruments.ModularInstruments.NIRfsg;
using NationalInstruments.ModularInstruments.NIRfsgPlayback;
using System.IO;

namespace NationalInstruments.Examples.RFmxSpecAnMemoryDpd
{
   public class RFmxSpecAnMemoryDpd
   {
      RFmxInstrMX instrSession;
      RFmxSpecAnMX specAn;
      NIRfsg rfsgSession;

      string rfsaResourceName = "RFSA";
      string rfsgResourceName = "RFSG";
      bool enableTrigger = true;
      string frequencyReferenceSource = RFmxInstrMXConstants.OnboardClock;
      double frequencyReferenceFrequency = 10e6;      /* Hz */

      RFmxSpecAnMXTriggerType triggerType = RFmxSpecAnMXTriggerType.DigitalEdge;
      double triggerDelay = 0;                        /* seconds */
      string selectedPorts = "";
      double centerFrequency = 1e+9;                  /* Hz */
      double referenceLevel = 0.00;                   /* dBm */
      double rfsaExternalAttenuation = 0.00;          /* dB */
      double rfsgExternalAttenuation = 0.00;          /* dB */
      double preFilterGain = -4.00;                   /* dB */
      double runtimeScaling;
      double papr = 0.0;
      double signalBandwidth = 20e6;                  /* Hz */
      double autoLevelMeasurementInterval = 100e-6;        /* seconds */
      double autoLevelReferenceLevel;
      double dutAverageInputPower = -20;              /* dBm */
      RFmxSpecAnMXDpdMeasurementSampleRateMode sampleRateMode = RFmxSpecAnMXDpdMeasurementSampleRateMode.ReferenceWaveform;
      double sampleRate = 120e6;                      /* S/s */
      double measurementInterval = 100e-6;            /* seconds */

      double thresholdLevel = -20;                    /* dB or dBm */
      RFmxSpecAnMXAmpmReferencePowerType referencePowerType = RFmxSpecAnMXAmpmReferencePowerType.Input;
      ComplexWaveform<ComplexSingle> referenceWaveformComplexSingle, waveformWithDpdComplexSingle;

      string referenceWaveformFile = @"LTE20MHz Waveform (Two Subframes).tdms";
      RFmxSpecAnMXDpdReferenceWaveformIdleDurationPresent idleDurationPresent =
                                            RFmxSpecAnMXDpdReferenceWaveformIdleDurationPresent.False;
      RFmxSpecAnMXDpdSignalType signalType = RFmxSpecAnMXDpdSignalType.Modulated;
      double timeout = 10;                            /* seconds */
      int numberOfIterations = 3;
      RFmxSpecAnMXDpdApplyDpdMemoryModelCorrectionType memoryModelCorrectionType =
         RFmxSpecAnMXDpdApplyDpdMemoryModelCorrectionType.MagnitudeAndPhase;
      RFmxSpecAnMXDpdIterativeDpdEnabled iterativeDpdEnabled = RFmxSpecAnMXDpdIterativeDpdEnabled.False;
      RfsgFrequencyReferenceSource referenceClockSource = RfsgFrequencyReferenceSource.OnboardClock;
      RFmxSpecAnMXDpdModel dpdModel = RFmxSpecAnMXDpdModel.MemoryPolynomial;
      double referenceClockRate = 10e6;
      string scriptName = "DPDScript";
      string waveformName = "Wfm";
      double rfsgIqRate;
      int markerNumber = 0;
      string waveformScript;
      int memoryPolynomialOrder = 3, memoryPolynomialDepth = 2;
      int memoryPolynomialLeadOrder = 2, memoryPolynomialLagOrder = 2, memoryPolynomialLeadMemoryDepth = 2,
          memoryPolynomialLagMemoryDepth = 2, memoryPolynomialMaxLead = 2, memoryPolynomialMaxLag = 2;
      double meanLinearGain, onedBCompressionPoint, meanRmsEvm,
             gainErrorRange, phaseErrorRange, meanPhaseError,
             amToAMResidual, amToPMResidual, powerOffset;

      float[] referencePowersAMToAM;
      float[] measuredAMToAM;
      float[] curveFitAMToAM;
      float[] referencePowersAMToPM;
      float[] measuredAMToPM;
      float[] curveFitAMToPM;
      ComplexSingle[] dpdPolynomial;

      internal void Run()
      {
         try
         {
            ReadWaveformFromTdmsFile();
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
         rfsgIqRate = 1 / referenceWaveformComplexSingle.PrecisionTiming.SampleInterval.TotalSeconds;
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
         rfsgSession.FrequencyReference.Configure(referenceClockSource, referenceClockRate);
         rfsgSession.DeviceEvents.MarkerEvents[markerNumber].ExportedOutputTerminal =
                                  RfsgMarkerEventExportedOutputTerminal.PxiTriggerLine0;
         rfsgSession.RF.Configure(centerFrequency, dutAverageInputPower);
         rfsgSession.RF.PowerLevelType = RfsgRFPowerLevelType.PeakPower;
         waveformScript = String.Format("script {0}{1}repeat forever{1}generate {2} marker{3}(0){1}end repeat{1}end script",
            scriptName, Environment.NewLine, waveformName, markerNumber);
         rfsgSession.RF.ExternalGain = -rfsgExternalAttenuation;
         rfsgSession.Arb.ReadAndDownloadWaveformFromFileTdms(waveformName, referenceWaveformFile, 0);
         sampleRate = rfsgSession.Arb.Waveforms[waveformName].IQRate;
         papr = rfsgSession.Arb.Waveforms[waveformName].Papr;
         runtimeScaling = preFilterGain;
         rfsgSession.Arb.PreFilterGain = runtimeScaling;
         rfsgSession.Arb.SignalBandwidth = 0.8 * sampleRate;
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
         specAn.SelectMeasurements("", RFmxSpecAnMXMeasurementTypes.Dpd, true);
         specAn.Dpd.Configuration.ConfigureReferenceWaveform("", referenceWaveformComplexSingle, idleDurationPresent,
                                                             signalType);
         specAn.Dpd.Configuration.ConfigureDutAverageInputPower("", dutAverageInputPower);
         specAn.Dpd.Configuration.ConfigureDpdModel("", dpdModel);
         specAn.Dpd.Configuration.ConfigureMemoryPolynomial("", memoryPolynomialOrder, memoryPolynomialDepth);
         specAn.Dpd.Configuration.ConfigureGeneralizedMemoryPolynomialCrossTerms("", memoryPolynomialLeadOrder,
             memoryPolynomialLagOrder, memoryPolynomialLeadMemoryDepth,
             memoryPolynomialLagMemoryDepth, memoryPolynomialMaxLead, memoryPolynomialMaxLag);
         specAn.Dpd.Configuration.ConfigureMeasurementSampleRate("", sampleRateMode, sampleRate);
         specAn.Dpd.Configuration.ConfigureMeasurementInterval("", measurementInterval);
         specAn.Dpd.Configuration.ConfigureIterativeDpdEnabled("", iterativeDpdEnabled);
         specAn.AutoLevel("", signalBandwidth, autoLevelMeasurementInterval, out autoLevelReferenceLevel);
         specAn.Dpd.ApplyDpd.ConfigureMemoryModelCorrectionType("", memoryModelCorrectionType);
      }

      private void RetrieveResults()
      {

         if (iterativeDpdEnabled == RFmxSpecAnMXDpdIterativeDpdEnabled.False)
         {
            numberOfIterations = 1;
         }
         else
         {
            numberOfIterations = 3;
         }
         for (int i = 0; i < numberOfIterations; i++)
         {
            specAn.Dpd.Configuration.ConfigurePreviousDpdPolynomial("", dpdPolynomial);
            specAn.Initiate("", "");
            specAn.Dpd.ApplyDpd.ApplyDigitalPredistortion("", referenceWaveformComplexSingle,
                  RFmxSpecAnMXDpdApplyDpdIdleDurationPresent.False, timeout, ref waveformWithDpdComplexSingle,
                  out papr, out powerOffset);
            rfsgSession.Abort();
            rfsgSession.Arb.ClearWaveform(waveformName);
            rfsgIqRate = 1 / waveformWithDpdComplexSingle.PrecisionTiming.SampleInterval.TotalSeconds;
            rfsgSession.Arb.WriteWaveform(waveformName, waveformWithDpdComplexSingle);
            rfsgSession.Arb.PreFilterGain = runtimeScaling;
            rfsgSession.Arb.IQRate = rfsgIqRate;
            rfsgSession.Arb.Waveforms[waveformName].Papr = (papr + powerOffset);
            rfsgSession.Arb.SignalBandwidth = 0.8 * rfsgIqRate;
            rfsgSession.Arb.Scripting.WriteScript(waveformScript);
            rfsgSession.Initiate();

            specAn.Dpd.Results.FetchDpdPolynomial("", timeout, ref dpdPolynomial);
         }

         specAn.AutoLevel("", signalBandwidth, autoLevelMeasurementInterval, out autoLevelReferenceLevel);
         specAn.SelectMeasurements("", RFmxSpecAnMXMeasurementTypes.Ampm, true);
         specAn.Ampm.Configuration.ConfigureMeasurementSampleRate("", RFmxSpecAnMXAmpmMeasurementSampleRateMode.ReferenceWaveform,
                                                                  sampleRate);
         specAn.Ampm.Configuration.ConfigureMeasurementInterval("", measurementInterval);
         specAn.Ampm.Configuration.ConfigureReferenceWaveform("", referenceWaveformComplexSingle,
             RFmxSpecAnMXAmpmReferenceWaveformIdleDurationPresent.False, RFmxSpecAnMXAmpmSignalType.Modulated);
         specAn.Ampm.Configuration.ConfigureDutAverageInputPower("", dutAverageInputPower);
         specAn.Ampm.Configuration.ConfigureThreshold("", RFmxSpecAnMXAmpmThresholdEnabled.True,
             thresholdLevel, RFmxSpecAnMXAmpmThresholdType.Relative);
         specAn.Ampm.Configuration.ConfigureReferencePowerType("", referencePowerType);

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
