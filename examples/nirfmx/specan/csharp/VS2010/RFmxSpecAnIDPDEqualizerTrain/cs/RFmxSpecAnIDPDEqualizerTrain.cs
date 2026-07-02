/* Steps:
1. Open RFSG session.
2. Configure RFSG frequency reference.
3. Configure marker0 to be generated from RFSG on the specified output terminal.
4. Configure frequency and power level of RF output signal.
5. Set RFSG External Gain. #4 and #5 ensure that the average power of the signal at the input of the DUT
   matches the user configured DUT Average Input Power.
6. Open RFmx session.
7. Configure frequency reference of the analyser.
8. Configure Selected Ports.
9. Configure trigger to use as reference for signal acquisition.
10. Configure center frequency and external attenuation.
11. Select IDPD measurement, configure the reference waveform.
12. Equalizer Mode is fixed to Train Mode.
13. Configure power of reference signal at the input of the DUT.
      Set Equalizer mode and filter length.
      Set Sample rate and it's mode.
      Set Averaging Mode and count. 
      Set Start and Stop for Impairment estimation. 
14. Commit IDPD Measurement to get Equalizer Training waveform. Equalizer training waveform is 
      multitone waveform with same spectral occupancy as the reference waveform. This is because 
      equalizer training waveform is multitone wavaform to ensure that obtained equalizer coefficients 
      can be used for wider set of test waveform configurations.
15. Get Equalizer Training waveform and  scale from -1 to 1 as RFSG power level type is peak power.
16. Write Equalizer Training waveform to RFSG
      Set Waveform Runtime Scaling to the desired Pre-filter Gain.
      Read waveform sample rate, multiply by 0.8 and set the result to the signal bandwidth.
      Write script to generate the waveform specified in the script. This script is programmed 
      to generate waveform continuously, with marker0 aligned to sample index 0.
      Initiate RFSG generation. 
17. Perform Auto Level to compute an approximate reference level to be used by the analyser.
18. Initiates IDPD measurement.
19. Fetch Trained Equalizer Coefficients.
20. Close RFmx session.
21. Close RFSG session. 
It is recommended to clear the waveform before closing RFSG session.*/

using System;
using NationalInstruments.RFmx.InstrMX;
using NationalInstruments.RFmx.SpecAnMX;
using NationalInstruments.ModularInstruments.NIRfsg;
using NationalInstruments.ModularInstruments.NIRfsgPlayback;

namespace NationalInstruments.Examples.RFmxSpecAnIdpdEqualizerTrain
{
   public class RFmxSpecAnIdpdEqualizerTrain
   {
      RFmxInstrMX instrSession;
      RFmxSpecAnMX specAn;
      NIRfsg rfsgSession;
      IntPtr instrumentHandle;

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

      RFmxSpecAnMXIdpdEqualizerMode equalizerMode = RFmxSpecAnMXIdpdEqualizerMode.Train;

      double signalBandwidth = 20e6;                  /* Hz */
      double autoLevelMeasurementInterval = 100e-6;   /* seconds */
      double autoLevelReferenceLevel;

      double timeout = 10;                            /* seconds */

      string scriptName = "IDPDScript";
      string waveformName = "Wfm";
      int waveformSize;
      double rfsgIqRate;
      int markerNumber = 0;
      string waveformScript;

      double[] idpdMeanRmsEvm;
      ComplexWaveform<ComplexSingle> equalizerWaveform, normalizedEqualizerWaveform;
      ComplexWaveform<ComplexSingle> referenceWaveformComplexSingle;
      ComplexWaveform<ComplexSingle> equalizerCoefficients;
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
         rfsgSession.FrequencyReference.Configure(referenceClockSource, referenceClockRate);
         rfsgSession.DeviceEvents.MarkerEvents[markerNumber].ExportedOutputTerminal =
                                 RfsgMarkerEventExportedOutputTerminal.PxiTriggerLine0;
         rfsgSession.RF.Configure(centerFrequency, dutAverageInputPower);
         rfsgSession.RF.PowerLevelType = RfsgRFPowerLevelType.PeakPower;
         rfsgSession.RF.ExternalGain = -rfsgExternalAttenuation;
         rfsgSession.RF.Upconverter.FrequencyOffsetMode = UpconverterFrequencyOffsetMode.Auto;

         instrumentHandle = rfsgSession.GetInstrumentHandle().DangerousGetHandle();
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
         specAn.Idpd.Configuration.SetEqualizerMode("", equalizerMode);
         specAn.Idpd.Configuration.SetDutAverageInputPower("", dutAverageInputPower);
         specAn.Idpd.Configuration.SetMeasurementSampleRateMode("", sampleRateMode);
         specAn.Idpd.Configuration.SetMeasurementSampleRate("", sampleRate);
         specAn.Commit("");
      }

      private void RetrieveResults()
      {
         specAn.Idpd.Results.GetEqualizerReferenceWaveform("", ref equalizerWaveform, out papr);

         /* Storing the predistorted waveform again to normalize it */
         double x0 = equalizerWaveform.PrecisionTiming.TimeOffset.TotalSeconds;
         double dx = equalizerWaveform.PrecisionTiming.SampleInterval.TotalSeconds;
         normalizedEqualizerWaveform = new ComplexWaveform<ComplexSingle>(0);
         normalizedEqualizerWaveform.Append(equalizerWaveform.GetRawData());
         PrecisionWaveformTiming precisionTiming = PrecisionWaveformTiming.CreateWithRegularInterval(new PrecisionTimeSpan(dx), new PrecisionTimeSpan(x0));
         normalizedEqualizerWaveform.PrecisionTiming = precisionTiming;

         /*Normalization*/
         ComplexSingle[] equalizerWaveformRawData = normalizedEqualizerWaveform.GetRawData();
         float maxElement = equalizerWaveformRawData[0].Magnitude;

         for (int k = 1; k < equalizerWaveformRawData.Length; k++)
         {
            if (equalizerWaveformRawData[k].Magnitude > maxElement)
            {
               maxElement = equalizerWaveformRawData[k].Magnitude;
            }
         }

         for (int j = 0; j < equalizerWaveformRawData.Length; j++)
         {
            equalizerWaveformRawData[j] /= new ComplexSingle(maxElement, 0);
         }

         normalizedEqualizerWaveform = new ComplexWaveform<ComplexSingle>(0);
         normalizedEqualizerWaveform.Append(equalizerWaveformRawData);
         normalizedEqualizerWaveform.PrecisionTiming = precisionTiming;

         rfsgIqRate = 1 / normalizedEqualizerWaveform.PrecisionTiming.SampleInterval.TotalSeconds;
         rfsgSession.Arb.WriteWaveform(waveformName, normalizedEqualizerWaveform);
         NIRfsgPlayback.StoreWaveformRuntimeScaling(instrumentHandle, waveformName, runtimeScaling);
         NIRfsgPlayback.StoreWaveformSampleRate(instrumentHandle, waveformName, rfsgIqRate);
         NIRfsgPlayback.StoreWaveformPapr(instrumentHandle, waveformName, papr);
         NIRfsgPlayback.StoreWaveformSignalBandwidth(instrumentHandle, waveformName, 0.8 * rfsgIqRate);
         waveformScript = String.Format("script {0}{1}repeat forever{1}generate {2} marker{3}(0){1}end repeat{1}end script",
               scriptName, Environment.NewLine, waveformName, markerNumber);
         NIRfsgPlayback.SetScriptToGenerateSingleRfsg(instrumentHandle, waveformScript);
         rfsgSession.Initiate();

         specAn.AutoLevel("", signalBandwidth, autoLevelMeasurementInterval, out autoLevelReferenceLevel);
         specAn.Initiate("", "");
         specAn.Idpd.Results.FetchEqualizerCoefficients("", timeout, ref equalizerCoefficients);
      }

      private void DisplayResults()
      {
         Console.WriteLine("x0 : {0}", equalizerCoefficients.PrecisionTiming.TimeOffset.TotalSeconds);
         Console.WriteLine("dx : {0}", equalizerCoefficients.PrecisionTiming.SampleInterval.TotalSeconds);
         ComplexSingle[] coefficientArray = equalizerCoefficients.GetRawData();
         Console.WriteLine("Equalizer Coefficients: ");
         for (int i = 0; i < coefficientArray.Length; i++)
         {
            Console.WriteLine(coefficientArray[i]);
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
               rfsgSession.Abort();
               NIRfsgPlayback.ClearWaveform(instrumentHandle, waveformName);
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
