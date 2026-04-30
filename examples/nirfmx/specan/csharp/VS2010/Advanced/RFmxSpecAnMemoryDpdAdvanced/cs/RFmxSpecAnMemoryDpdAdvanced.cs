/* Steps:
1. Open RFSG session.
2. Configure RFSG frequency reference.
3. Configure marker0 to be generated from RFSG on the specified output terminal.
4. Configure frequency and power level of RF output signal.
5. Configure power level type.
6. Set RFSG External Gain. #4 and #5 ensure that the average power of the signal at the input of the DUT
   matches the user configured DUT Average Input Power.
7. a. Read waveform from.
   b. Write input waveform on RFSG device.
      Set the waveform sample rate.
      Store waveform PAPR.
      Set Waveform Runtime Scaling to the desired Pre-filter Gain.
      Read waveform sample rate, multiply by 0.8 and set the result to the signal bandwidth.
      Write script to generate the waveform specified in the script. This script is programmed
      to generate waveform continuously, with marker0 aligned to sample index 0.
8. Initiate generation.
9. Open RFmx session.
10. Configure frequency reference of the analyser.
11. Configure Selected Ports.
12. Configure trigger to use as reference for signal acquisition.
13. Configure center frequency and external attenuation.
14. Select DPD measurement.
15. Configure pre-DPD CFR.
16. Configure waveform settings for pre-DPD CFR with filtering.
17. Apply pre-DPD CFR.
18. Read PAPR from file.
19. configure the reference waveform.
20. Configure power of the signal at the input of the DUT. Select and configure the Memory
    polynomial or Generalized memory polynomial model and its parameters to estimate the predistotor.
21. Set the measurement sample rate and the measurement interval to use for analysis.
22. Enable iterative DPD.
23. Configure DPD NMSE Enabled.
24. Configure the Memory models Correction type.
25. Configure apply DPD CFR settings before calling RFmx initiate.
    This is because these settings are used by measurement when performing iterative DPD.
26. Perform Auto Level to compute an approximate reference level to use by the analyser.
27. Set the previous iteration polynomial, in case DPD is measured iteratively.
28. Initiates DPD measurement and then configure Apply Digital Predistortion to remove the
    effects of memory and nonlinearity introduced by the DUT.
29. a. Fetch DPD Polynomial.
    b. Fetch NMSE (dB).
30. Abort RFSG generation and write a new waveform that is predistorted by applying momory polynomial coefficients.
    Set Waveform Runtime Scaling to desired Pre-filter Gain.
    Set the sample rate computed from Apply Digital Predistortion.
    Set the final PAPR to the sum of the actual PAPR and the Power Offset as computed
    by Apply Digital Predistortion.
    Set the Signal Bandwidth.
    Initiate RFSG generation using the script that was selected earlier.
31. Perform Auto Level to compute an approximate reference level while generating the predistorted waveform.
32. Select and configure AMPM measurement in RFmx after DPD measurement is complete.
    AMPM measurement is used to inspect the measure the AM-AM and AM-PM response of the DUT.
33. Initiate and fetch AMPM results.
34. Close RFmx session.
35. Close RFSG session.
It is recommended to clear the waveform before closing RFSG session.*/

using System;
using NationalInstruments.RFmx.InstrMX;
using NationalInstruments.RFmx.SpecAnMX;
using NationalInstruments.DataInfrastructure;
using NationalInstruments.ModularInstruments.NIRfsg;
using NationalInstruments.ModularInstruments.NIRfsgPlayback;
using System.IO;

namespace NationalInstruments.Examples.RFmxSpecAnMemoryDpdAdvanced
{
   public class RFmxSpecAnMemoryDpdAdvanced
   {
      RFmxInstrMX instrSession;
      RFmxSpecAnMX specAn;
      NIRfsg rfsgSession;
      IntPtr instrumentHandle;

      string rfsaResourceName = "RFSA";
      string rfsgResourceName = "RFSG";
      bool enableTrigger = true;
      string digitalEdgeSource = RFmxInstrMXConstants.PxiTriggerLine0;
      RFmxSpecAnMXDigitalEdgeTriggerEdge digitalEdgeTriggerEdge = RFmxSpecAnMXDigitalEdgeTriggerEdge.Rising;
      RFmxSpecAnMXIQPowerEdgeTriggerSlope iqPowerEdgeTriggerSlope = RFmxSpecAnMXIQPowerEdgeTriggerSlope.Rising;
      RFmxSpecAnMXTriggerMinimumQuietTimeMode triggerMinimumQuietTimeMode =
         RFmxSpecAnMXTriggerMinimumQuietTimeMode.Manual;
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
      double autoLevelMeasurementInterval = 100e-6;   /* seconds */
      double autoLevelReferenceLevel;
      double dutAverageInputPower = -20;              /* dBm */
      RfsgRFPowerLevelType powerLevelType = RfsgRFPowerLevelType.PeakPower;
      RFmxSpecAnMXDpdMeasurementSampleRateMode sampleRateMode =
         RFmxSpecAnMXDpdMeasurementSampleRateMode.ReferenceWaveform;
      double sampleRate = 120e6;                      /* S/s */
      double measurementInterval = 100e-6;            /* seconds */

      double thresholdLevel = -20;                    /* dB or dBm */
      ComplexWaveform<ComplexSingle> referenceWaveformComplexSingle, waveformWithDpdComplexSingle;
      ComplexWaveform<ComplexSingle> preDpdWaveformWithComplexSingle;

      string referenceWaveformFile = @"LTE20MHz Waveform (Two Subframes).tdms";
      RFmxSpecAnMXDpdReferenceWaveformIdleDurationPresent idleDurationPresent =
         RFmxSpecAnMXDpdReferenceWaveformIdleDurationPresent.False;
      RFmxSpecAnMXDpdApplyDpdIdleDurationPresent dpdApplyDpdIdleDurationPresent =
      RFmxSpecAnMXDpdApplyDpdIdleDurationPresent.False;
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
      int crossTermsLeadOrder = 2, crossTermsLagOrder = 2, crossTermsLeadMemoryDepth = 2,
          crossTermsLagMemoryDepth = 2, crossTermsMaximumLead = 2, crossTermsMaximumLag = 2;
      RFmxSpecAnMXDpdNmseEnabled nmseEnabled = RFmxSpecAnMXDpdNmseEnabled.False;
      RFmxSpecAnMXAmpmReferencePowerType referencePowerType = RFmxSpecAnMXAmpmReferencePowerType.Input;

      RFmxSpecAnMXDpdPreDpdCfrEnabled preDpdCfrEnabled = RFmxSpecAnMXDpdPreDpdCfrEnabled.False;
      RFmxSpecAnMXDpdPreDpdCfrMethod preDpdCfrMethod = RFmxSpecAnMXDpdPreDpdCfrMethod.Clipping;
      int preDpdCfrMaximumIterations = 10;
      double preDpdCfrTargetPapr = 8.0;                /* (dB) */
      RFmxSpecAnMXDpdPreDpdCfrWindowType preDpdCfrWindowType = RFmxSpecAnMXDpdPreDpdCfrWindowType.KaiserBessel;
      int preDpdCfrWindowLength = 10;
      double preDpdCfrShapingFactor = 5.0;
      double preDpdCfrShapingThreshold = -5.0;         /* (dB) */
      RFmxSpecAnMXDpdPreDpdCfrFilterEnabled preDpdCfrFilterEnabled = RFmxSpecAnMXDpdPreDpdCfrFilterEnabled.False;
      const int NumberOfCarriers = 1;
      double[] preDpdCarrierOffsets = new double[NumberOfCarriers] { 0.0 };                                /* (Hz) */
      double[] preDpdCarrierBandwidths = new double[NumberOfCarriers] { 20e6 };                            /* (Hz) */

      RFmxSpecAnMXDpdApplyDpdCfrEnabled applyDpdCfrEnabled = RFmxSpecAnMXDpdApplyDpdCfrEnabled.False;
      RFmxSpecAnMXDpdApplyDpdCfrMethod applyDpdCfrMethod = RFmxSpecAnMXDpdApplyDpdCfrMethod.Clipping;
      int applyDpdCfrMaximumIterations = 10;
      RFmxSpecAnMXDpdApplyDpdCfrTargetPaprType applyDpdCfrTargetPaprType =
         RFmxSpecAnMXDpdApplyDpdCfrTargetPaprType.InputPapr;
      double applyDpdCfrTargetPapr = 8.0;               /* (dB) */
      RFmxSpecAnMXDpdApplyDpdCfrWindowType applyDpdCfrWindowType = RFmxSpecAnMXDpdApplyDpdCfrWindowType.KaiserBessel;
      int applyDpdCfrWindowLength = 10;
      double applyDpdCfrShapingFactor = 5.0;
      double applyDpdCfrShapingThreshold = -5.0;        /* (dB) */

      string carrierString;

      double meanLinearGain, onedBCompressionPoint, meanRmsEvm,
             gainErrorRange, phaseErrorRange, meanPhaseError,
             amToAMResidual, amToPMResidual, powerOffset, nmse;

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
            ConfigureRfsgAndRFmx();
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

      private void ConfigureRfsgAndRFmx()
      {
         rfsgSession = new NIRfsg(rfsgResourceName, false, true);
         rfsgSession.FrequencyReference.Configure(referenceClockSource, referenceClockRate);
         rfsgSession.DeviceEvents.MarkerEvents[markerNumber].ExportedOutputTerminal =
                                  RfsgMarkerEventExportedOutputTerminal.PxiTriggerLine0;
         rfsgSession.RF.Configure(centerFrequency, dutAverageInputPower);
         rfsgSession.RF.PowerLevelType = powerLevelType;
         waveformScript = String.Format("script {0}{1}repeat forever{1}generate {2} marker{3}(0){1}end repeat{1}end script",
            scriptName, Environment.NewLine, waveformName, markerNumber);
         rfsgSession.RF.ExternalGain = -rfsgExternalAttenuation;
         instrumentHandle = rfsgSession.GetInstrumentHandle().DangerousGetHandle();
         NIRfsgPlayback.ReadWaveformFromFileComplex(referenceWaveformFile, ref referenceWaveformComplexSingle);

         instrSession.ConfigureFrequencyReference("", frequencyReferenceSource, frequencyReferenceFrequency);
         specAn.SetSelectedPorts("", selectedPorts);

         if (triggerType == RFmxSpecAnMXTriggerType.IQPowerEdge)
            specAn.ConfigureIQPowerEdgeTrigger("", "0", -20.0, iqPowerEdgeTriggerSlope, triggerDelay,
               triggerMinimumQuietTimeMode, 0.0, enableTrigger);
         else
            specAn.ConfigureDigitalEdgeTrigger("", digitalEdgeSource, digitalEdgeTriggerEdge, triggerDelay,
               enableTrigger);

         specAn.ConfigureRF("", centerFrequency, referenceLevel, rfsaExternalAttenuation);
         specAn.SelectMeasurements("", RFmxSpecAnMXMeasurementTypes.Dpd, true);

         if (preDpdCfrEnabled == RFmxSpecAnMXDpdPreDpdCfrEnabled.True)
         {
            specAn.Dpd.PreDpd.SetCfrEnabled("", preDpdCfrEnabled);
            specAn.Dpd.PreDpd.SetCfrMethod("", preDpdCfrMethod);
            specAn.Dpd.PreDpd.SetCfrMaximumIterations("", preDpdCfrMaximumIterations);
            specAn.Dpd.PreDpd.SetCfrTargetPapr("", preDpdCfrTargetPapr);
            specAn.Dpd.PreDpd.SetCfrWindowType("", preDpdCfrWindowType);
            specAn.Dpd.PreDpd.SetCfrWindowLength("", preDpdCfrWindowLength);
            specAn.Dpd.PreDpd.SetCfrShapingFactor("", preDpdCfrShapingFactor);
            specAn.Dpd.PreDpd.SetCfrShapingThreshold("", preDpdCfrShapingThreshold);
            specAn.Dpd.PreDpd.SetCfrFilterEnabled("", preDpdCfrFilterEnabled);
            specAn.Dpd.PreDpd.SetCfrNumberOfCarriers("", NumberOfCarriers);
            for (int i = 0; i < NumberOfCarriers; i++)
            {
               carrierString = RFmxSpecAnMX.BuildCarrierString2("", i);
               specAn.Dpd.PreDpd.SetCarrierOffset(carrierString, preDpdCarrierOffsets[i]);
               specAn.Dpd.PreDpd.SetCarrierBandwidth(carrierString, preDpdCarrierBandwidths[i]);
            }
            specAn.Dpd.PreDpd.ApplyPreDpdSignalConditioning("", referenceWaveformComplexSingle,
               dpdApplyDpdIdleDurationPresent, ref preDpdWaveformWithComplexSingle, out papr);
         }
         else
            NIRfsgPlayback.ReadPaprFromFile(referenceWaveformFile, 0, out papr);

         if (preDpdCfrEnabled == RFmxSpecAnMXDpdPreDpdCfrEnabled.True)
         {
            rfsgSession.Arb.WriteWaveform(waveformName, preDpdWaveformWithComplexSingle);
            sampleRate = 1 / preDpdWaveformWithComplexSingle.PrecisionTiming.SampleInterval.TotalSeconds;

         }
         else
         {
            rfsgSession.Arb.WriteWaveform(waveformName, referenceWaveformComplexSingle);
            sampleRate = 1 / referenceWaveformComplexSingle.PrecisionTiming.SampleInterval.TotalSeconds;
         }
         NIRfsgPlayback.StoreWaveformSampleRate(instrumentHandle, waveformName, sampleRate);
         NIRfsgPlayback.StoreWaveformPapr(instrumentHandle, waveformName, papr);
         runtimeScaling = preFilterGain;
         NIRfsgPlayback.StoreWaveformRuntimeScaling(instrumentHandle, waveformName, runtimeScaling);
         NIRfsgPlayback.StoreWaveformSignalBandwidth(instrumentHandle, waveformName, 0.8 * sampleRate);
         NIRfsgPlayback.SetScriptToGenerateSingleRfsg(instrumentHandle, waveformScript);
         rfsgSession.Initiate();

         if (preDpdCfrEnabled == RFmxSpecAnMXDpdPreDpdCfrEnabled.True)
         {
            specAn.Dpd.Configuration.ConfigureReferenceWaveform("", preDpdWaveformWithComplexSingle,
               idleDurationPresent, signalType);

         }
         else
         {
            specAn.Dpd.Configuration.ConfigureReferenceWaveform("", referenceWaveformComplexSingle,
               idleDurationPresent, signalType);
         }
         specAn.Dpd.Configuration.ConfigureDutAverageInputPower("", dutAverageInputPower);
         specAn.Dpd.Configuration.ConfigureDpdModel("", dpdModel);
         specAn.Dpd.Configuration.ConfigureMemoryPolynomial("", memoryPolynomialOrder, memoryPolynomialDepth);
         specAn.Dpd.Configuration.ConfigureGeneralizedMemoryPolynomialCrossTerms("", crossTermsLeadOrder,
            crossTermsLagOrder, crossTermsLeadMemoryDepth,
            crossTermsLagMemoryDepth, crossTermsMaximumLead, crossTermsMaximumLag);
         specAn.Dpd.Configuration.ConfigureMeasurementSampleRate("", sampleRateMode, sampleRate);
         specAn.Dpd.Configuration.ConfigureMeasurementInterval("", measurementInterval);
         specAn.Dpd.Configuration.ConfigureIterativeDpdEnabled("", iterativeDpdEnabled);
         if (iterativeDpdEnabled == RFmxSpecAnMXDpdIterativeDpdEnabled.False)
            numberOfIterations = 1;
         specAn.Dpd.Configuration.SetNmseEnabled("", nmseEnabled);
         specAn.Dpd.ApplyDpd.ConfigureMemoryModelCorrectionType("", memoryModelCorrectionType);
         specAn.Dpd.ApplyDpd.SetCfrEnabled("", applyDpdCfrEnabled);
         specAn.Dpd.ApplyDpd.SetCfrMethod("", applyDpdCfrMethod);
         specAn.Dpd.ApplyDpd.SetCfrMaximumIterations("", applyDpdCfrMaximumIterations);
         specAn.Dpd.ApplyDpd.SetCfrTargetPaprType("", applyDpdCfrTargetPaprType);
         specAn.Dpd.ApplyDpd.SetCfrTargetPapr("", applyDpdCfrTargetPapr);
         specAn.Dpd.ApplyDpd.SetCfrWindowType("", applyDpdCfrWindowType);
         specAn.Dpd.ApplyDpd.SetCfrWindowLength("", applyDpdCfrWindowLength);
         specAn.Dpd.ApplyDpd.SetCfrShapingFactor("", applyDpdCfrShapingFactor);
         specAn.Dpd.ApplyDpd.SetCfrShapingThreshold("", applyDpdCfrShapingThreshold);
         specAn.AutoLevel("", signalBandwidth, autoLevelMeasurementInterval, out autoLevelReferenceLevel);
      }

      private void RetrieveResults()
      {
         for (int i = 0; i < numberOfIterations; i++)
         {
            specAn.Dpd.Configuration.ConfigurePreviousDpdPolynomial("", dpdPolynomial);
            specAn.Initiate("", "");

            if (preDpdCfrEnabled == RFmxSpecAnMXDpdPreDpdCfrEnabled.True)
               specAn.Dpd.ApplyDpd.ApplyDigitalPredistortion("", preDpdWaveformWithComplexSingle, dpdApplyDpdIdleDurationPresent,
                 timeout, ref waveformWithDpdComplexSingle, out papr, out powerOffset);
            else
               specAn.Dpd.ApplyDpd.ApplyDigitalPredistortion("", referenceWaveformComplexSingle, dpdApplyDpdIdleDurationPresent,
                  timeout, ref waveformWithDpdComplexSingle, out papr, out powerOffset);

            specAn.Dpd.Results.FetchDpdPolynomial("", timeout, ref dpdPolynomial);
            specAn.Dpd.Results.FetchNmse("", timeout, out nmse);
            Console.WriteLine("NMSE            {0}", nmse);

            rfsgSession.Abort();
            NIRfsgPlayback.ClearWaveform(instrumentHandle, waveformName);
            rfsgIqRate = 1 / waveformWithDpdComplexSingle.PrecisionTiming.SampleInterval.TotalSeconds;
            rfsgSession.Arb.WriteWaveform(waveformName, waveformWithDpdComplexSingle);
            NIRfsgPlayback.StoreWaveformRuntimeScaling(instrumentHandle, waveformName, runtimeScaling);
            NIRfsgPlayback.StoreWaveformSampleRate(instrumentHandle, waveformName, rfsgIqRate);
            NIRfsgPlayback.StoreWaveformPapr(instrumentHandle, waveformName, (papr + powerOffset));
            NIRfsgPlayback.StoreWaveformSignalBandwidth(instrumentHandle, waveformName, 0.8 * rfsgIqRate);
            NIRfsgPlayback.SetScriptToGenerateSingleRfsg(instrumentHandle, waveformScript);
            rfsgSession.Initiate();
         }

         specAn.AutoLevel("", signalBandwidth, autoLevelMeasurementInterval, out autoLevelReferenceLevel);
         specAn.SelectMeasurements("", RFmxSpecAnMXMeasurementTypes.Ampm, true);
         specAn.Ampm.Configuration.ConfigureMeasurementSampleRate("",
            RFmxSpecAnMXAmpmMeasurementSampleRateMode.ReferenceWaveform, sampleRate);
         specAn.Ampm.Configuration.ConfigureMeasurementInterval("", measurementInterval);

         if (preDpdCfrEnabled == RFmxSpecAnMXDpdPreDpdCfrEnabled.True)
            specAn.Ampm.Configuration.ConfigureReferenceWaveform("", preDpdWaveformWithComplexSingle,
             RFmxSpecAnMXAmpmReferenceWaveformIdleDurationPresent.False, RFmxSpecAnMXAmpmSignalType.Modulated);
         else
            specAn.Ampm.Configuration.ConfigureReferenceWaveform("", referenceWaveformComplexSingle,
               RFmxSpecAnMXAmpmReferenceWaveformIdleDurationPresent.False, RFmxSpecAnMXAmpmSignalType.Modulated);

         specAn.Ampm.Configuration.ConfigureDutAverageInputPower("", dutAverageInputPower);
         specAn.Ampm.Configuration.ConfigureThreshold("", RFmxSpecAnMXAmpmThresholdEnabled.True,
             thresholdLevel, RFmxSpecAnMXAmpmThresholdType.Relative);
         specAn.Ampm.Configuration.ConfigureReferencePowerType("", referencePowerType);
         specAn.Initiate("", "");

         specAn.Ampm.Results.FetchDutCharacteristics("", timeout, out meanLinearGain, out onedBCompressionPoint,
            out meanRmsEvm);
         specAn.Ampm.Results.FetchError("", timeout, out gainErrorRange, out phaseErrorRange, out meanPhaseError);
         specAn.Ampm.Results.FetchCurveFitResidual("", timeout, out amToAMResidual, out amToPMResidual);
         specAn.Ampm.Results.FetchAMToAMTrace("", timeout, ref referencePowersAMToAM, ref measuredAMToAM,
            ref curveFitAMToAM);
         specAn.Ampm.Results.FetchAMToPMTrace("", timeout, ref referencePowersAMToPM, ref measuredAMToPM,
            ref curveFitAMToPM);

         rfsgSession.Abort();
         NIRfsgPlayback.ClearWaveform(instrumentHandle, waveformName);
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
