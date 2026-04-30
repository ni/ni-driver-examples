//Steps:
//1. Open a new RFmx session.
//2. Configure the basic instrument properties (Clock Source and Clock Frequency).
//3. Configure Selected Ports.
//4. Configure the basic signal properties (Center Frequency, Reference Level and External Attenuation).
//5. Configure Trigger Type and Trigger Parameters.
//6. Configure PAVT measurement and enable the traces.
//7. Configure Measurement Location Type.

//8. Follow these steps depending upon Measurement Location Type :
// When Measurement Location Type is Time, configure
//8.1. Segment Start Time by :
//8.1.1. Configuring Number of Segments, Segment0 Start Time(s) and Segment Interval(s).
//8.2. Segment Start Time by :
//8.2.1. Configuring Segment Start Time(s).
//8.2.2. Configuring Number of Segments.
// When Measurement Location Type is Trigger, configure
//8.3. Number of Segments.

//9. Configure Measurement Bandwidth.
//10. Configure Measurement Interval.
//11. Initiate Measurement.
//12. Fetch PAVT Traces and Measurements.
//13. Close the RFmx Session.

using System;
using NationalInstruments.RFmx.InstrMX;
using NationalInstruments.RFmx.SpecAnMX;

namespace NationalInstruments.Examples.RFmxSpecAnPavt
{
   public enum MeasurementStartTimeType
   {
      Step = 0,
      List = 1
   }

   public class RFmxSpecAnPavt
   {
      RFmxInstrMX instrSession;
      RFmxSpecAnMX specAn;

      string resourceName;
      string selectedPorts;
      double centerFrequency;
      double referenceLevel;
      double externalAttenuation;

      string frequencyReferenceSource;
      double frequencyReference;

      bool enableTrigger;
      string digitalEdgeSource;
      RFmxSpecAnMXDigitalEdgeTriggerEdge digitalEdge;
      double triggerDelay;

      RFmxSpecAnMXPavtMeasurementLocationType measurementLocationType;
      const int NumberOfSegments = 1;

      double segment0StartTime;
      double segmentInterval;

      const int segmentStartTimeArraySize = 1;
      double[] segmentStartTime = new double[segmentStartTimeArraySize];

      MeasurementStartTimeType measurementStartTimeType;

      double measurementBandwidth;

      double measurementOffset;
      double measurementLength;

      double timeout;

      double[] meanRelativePhase = new double[NumberOfSegments];                          /* (deg) */
      double[] meanRelativeAmplitude = new double[NumberOfSegments];                      /* (dB) */
      double[] meanAbsolutePhase = new double[NumberOfSegments];                          /* (deg) */
      double[] meanAbsoluteAmplitude = new double[NumberOfSegments];                      /* (dBm) */

      AnalogWaveform<float>[] amplitude = new AnalogWaveform<float>[NumberOfSegments];    /* (dBm) */
      AnalogWaveform<float>[] phase = new AnalogWaveform<float>[NumberOfSegments];        /* (deg) */

      public void Run()
      {
         try
         {
            InitializeVariables();
            InitializeInstr();
            ConfigureSpecAn();
            RetrieveResults();
            PrintResults();
         }
         catch (Exception ex)
         {
            DisplayError(ex);
         }
         finally
         {
            /* Close session */
            CloseSession();
            Console.WriteLine("Press any key to exit");
            Console.ReadKey();
         }
      }

      private void InitializeVariables()
      {
         resourceName = "RFSA";

         selectedPorts = "";
         centerFrequency = 1.0e+9;                                         /* (Hz) */
         referenceLevel = 0.0;                                             /* (dBm) */
         externalAttenuation = 0.0;                                        /* (dB) */

         frequencyReferenceSource = RFmxInstrMXConstants.OnboardClock;
         frequencyReference = 10.0e6;                                      /* (Hz) */

         enableTrigger = true;
         digitalEdgeSource = RFmxSpecAnMXConstants.PxiTriggerLine0;
         digitalEdge = RFmxSpecAnMXDigitalEdgeTriggerEdge.Rising;
         triggerDelay = 0.0;                                               /* (s) */

         measurementLocationType = RFmxSpecAnMXPavtMeasurementLocationType.Time;

         /* Segment Step */
         segment0StartTime = 0.0;                                          /* (s) */
         segmentInterval = 1.0e-3;                                         /* (s) */

         /* Segment List */
         segmentStartTime[0] = 0.0;                                        /* (s) */

         measurementStartTimeType = MeasurementStartTimeType.Step;

         measurementBandwidth = 10.0e6;                                    /* (Hz) */

         measurementOffset = 0.0;                                          /* (s) */
         measurementLength = 1.0e-3;                                       /* (s) */

         timeout = 10.0;                                                   /* (s) */
      }

      private void InitializeInstr()
      {
         /* Create a new RFmx Session */
         instrSession = new RFmxInstrMX(resourceName, "");
      }

      private void ConfigureSpecAn()
      {
         /* Get SpecAn signal */
         specAn = instrSession.GetSpecAnSignalConfiguration();

         /* Configure measurement */
         instrSession.ConfigureFrequencyReference("", frequencyReferenceSource, frequencyReference);
         specAn.SetSelectedPorts("", selectedPorts);
         specAn.ConfigureRF("", centerFrequency, referenceLevel, externalAttenuation);
         specAn.ConfigureDigitalEdgeTrigger("", digitalEdgeSource, digitalEdge, triggerDelay, enableTrigger);
         specAn.SelectMeasurements("", RFmxSpecAnMXMeasurementTypes.Pavt, true);
         specAn.Pavt.Configuration.ConfigureMeasurementLocationType("", measurementLocationType);
         if (measurementLocationType == RFmxSpecAnMXPavtMeasurementLocationType.Time)
         {
            if (measurementStartTimeType == MeasurementStartTimeType.Step)
            {
               specAn.Pavt.Configuration.ConfigureSegmentStartTimeStep("", NumberOfSegments,
                  segment0StartTime, segmentInterval);
            }
            else
            {
               specAn.Pavt.Configuration.ConfigureNumberOfSegments("", segmentStartTimeArraySize);
               specAn.Pavt.Configuration.ConfigureSegmentStartTimeList("", segmentStartTime);
            }
         }
         else
         {
            specAn.Pavt.Configuration.ConfigureNumberOfSegments("", NumberOfSegments);
         }
         specAn.Pavt.Configuration.ConfigureMeasurementBandwidth("", measurementBandwidth);
         specAn.Pavt.Configuration.ConfigureMeasurementInterval("", measurementOffset, measurementLength);
         specAn.Initiate("", "");
      }

      private void RetrieveResults()
      {
         specAn.Pavt.Results.FetchPhaseAndAmplitudeArray("", timeout, ref meanRelativePhase,
            ref meanRelativeAmplitude, ref meanAbsolutePhase, ref meanAbsoluteAmplitude);

         for (int i = 0; i < NumberOfSegments; i++)
         {
            specAn.Pavt.Results.FetchPhaseTrace("", timeout, i, ref phase[i]);
            specAn.Pavt.Results.FetchAmplitudeTrace("", timeout, i, ref amplitude[i]);
         }
      }

      private void PrintResults()
      {
         Console.WriteLine("Segment0 Mean Absolute Phase (deg)       : {0}", meanAbsolutePhase[0]);
         Console.WriteLine("Segment0 Mean Absolute Amplitude (dBm)   : {0}\n", meanAbsoluteAmplitude[0]);
         Console.WriteLine("Segment Measurements");
         for (int i = 0; i < NumberOfSegments; i++)
         {
            Console.WriteLine("Segment  :  {0}", i);
            Console.WriteLine("Mean Relative Phase (deg)                : {0}", meanRelativePhase[i]);
            Console.WriteLine("Mean Relative Amplitude (dB)             : {0}", meanRelativeAmplitude[i]);
            Console.WriteLine("-------------------------------------------------\n");
         }
      }

      private void CloseSession()
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
         }
         catch (Exception ex)
         {
            DisplayError(ex);
         }
      }

      private void DisplayError(Exception ex)
      {
         Console.WriteLine("ERROR:\n" + ex.GetType() + ": " + ex.Message);
      }

   }
}