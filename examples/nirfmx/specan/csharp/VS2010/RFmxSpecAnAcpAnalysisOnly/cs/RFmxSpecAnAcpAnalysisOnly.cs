//Steps:
//1. Open a new RFmx session.
//2. Configure Center Frequency.
//3. Select ACP measurement and enable the traces.
//4. Configure RBW filter parameters.
//5. Configure Sweep Time.
//6. Configure Integration BW of the Carrier and Offset Channels, Number of Offset Channels and Channel Spacing between the Offset channels. This method configures a Carrier Channel with Offset Channels as specified by the Number of Offsets. Refer to method help for more information.
//7. Configure Averaging Parameters.
//8. Commit the settings on the RFmx session and read the acquisition parameters (Span and Number of Records)from the RFmxInstr Property node.

//9. Open a new NI-RFSA session.
//10. Configure the NI-RFSA device reference clock.
//11. Configure the Center Frequency of the RFSA hardware for the 2 possible Acquisition types - IQ and Spectrum acquisition.
//12. Configure the External gain (in dB) of a device or cable connected before the RF IN connector of the NI-RFSA.
//13. Configure the Reference Level in dBm.
//14. Read the maximum instantaneous bandwidth of the RFSA device and compare with the Span requested by the measurement to decide the type of acquisition to be performed by RFSA.

//15. Configure the Acquisition Type to Spectrum.
//16. Read the Spectrum Acquisition parameters (Resolution Bandwidth and FFT Window Type) from the RFmx session and pass the same settings to the NI-RFSA session.
//17. Read the Power Spectrum from RFSA and pass it to RFmx AnalyzeSpectrum method for performing the measurement.
//    Note: NI-RFSA returns the power spectrum centered at the configured Center Frequency, but RFmx AnalyzeSpectrum method expects the spectrum to be at baseband i.e f0 is relative to 0Hz.

//18. Configure the Acquisition Type to IQ.
//19. Read the IQ Acquisition parameters (Sampling Rate and Acquisition Time) from the RFmx session and pass the same settings to the NI-RFSA session.
//20. Initiate the acquisition.
//21. Read the IQ Waveform from RFSA and pass it to RFmx AnalyzeIQ method for performing the measurement.
//    Note: In a multi-record acquisition case, the 'reset' parameter of the RFmxSpecAn AnalyzeIQ method is true for the first iteration and false for the remaining iterations.

//22. Fetch ACP Measurements and Traces.
//23. Close the RFmx and NI-RFSA Session.


using System;
using NationalInstruments.RFmx.InstrMX;
using NationalInstruments.RFmx.SpecAnMX;
using NationalInstruments.ModularInstruments.NIRfsa;

namespace NationalInstruments.Examples.RFmxSpecAnAcpAnalysisOnly
{
   public class RFmxSpecAnAcpAnalysisOnly
   {
      NIRfsa rfsaSession;
      RFmxInstrMX instrSession;
      RFmxSpecAnMX specAn;

      const int NumberOfOffsets = 1;

      string rfsaResourceName = "RFSA";
      double centerFrequency = 1e+9;         /* Hz */
      double referenceLevel = 0.00;          /* dBm */
      double externalAttenuation = 0.00;     /* dB */

      RfsaReferenceClockSource referenceClockSource = RfsaReferenceClockSource.OnboardClock;
      double referenceClockRate = 10e6;      /* Hz */

      double integrationBandwidth = 1.0e+6;  /* Hz */
      double channelSpacing = 1.0e+6;        /* Hz */

      RFmxSpecAnMXAcpRbwAutoBandwidth rbwAuto = RFmxSpecAnMXAcpRbwAutoBandwidth.True;
      double rbw = 10e3;                     /* Hz */
      RFmxSpecAnMXAcpRbwFilterType rbwFilterType = RFmxSpecAnMXAcpRbwFilterType.Gaussian;

      RFmxSpecAnMXAcpSweepTimeAuto sweepTimeAuto = RFmxSpecAnMXAcpSweepTimeAuto.True;
      double sweepTimeInterval = 1e6;        /* s */

      int averagingCount = 10;
      RFmxSpecAnMXAcpAveragingEnabled averagingEnabled = RFmxSpecAnMXAcpAveragingEnabled.False;
      RFmxSpecAnMXAcpAveragingType averagingType = RFmxSpecAnMXAcpAveragingType.Rms;

      double timeout = 10;                   /* s */
      int numberOfRecords;
      private long numberOfSamples;
      double maxDeviceInstantaneousBandwidth, spectralAcquisitionSpan;
      bool reset;
      long reserved = 0;

      double absolutePower;
      double[] lowerRelativePower;
      double[] upperRelativePower;
      double[] lowerAbsolutePower;
      double[] upperAbsolutePower;

      internal void Run()
      {
         try
         {
            ConfigureRfsaAndRFmx();
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
            Console.WriteLine("Press any key to exit.....");
            Console.ReadKey();
         }
      }

      void ConfigureRfsaAndRFmx()
      {
         /* Create a new RFSA Session */
         rfsaSession = new NIRfsa(rfsaResourceName, true, false);

         rfsaSession.Configuration.ReferenceClock.Configure(referenceClockSource, referenceClockRate);
         rfsaSession.Configuration.IQ.CarrierFrequency = centerFrequency;
         rfsaSession.Configuration.Spectrum.CenterFrequency = centerFrequency;
         rfsaSession.Configuration.Vertical.Advanced.ExternalGain = -externalAttenuation;
         rfsaSession.Configuration.Vertical.ReferenceLevel = referenceLevel;

         /* Create a new RFmx Session */
         instrSession = new RFmxInstrMX("", "AnalysisOnly=1");

         /* Get SpecAn signal */
         specAn = instrSession.GetSpecAnSignalConfiguration();

         /* Configure measurement */
         specAn.ConfigureFrequency("", centerFrequency);
         specAn.SelectMeasurements("", RFmxSpecAnMXMeasurementTypes.Acp, true);
         specAn.Acp.Configuration.ConfigureRbwFilter("", rbwAuto, rbw, rbwFilterType);
         specAn.Acp.Configuration.ConfigureSweepTime("", sweepTimeAuto, sweepTimeInterval);
         specAn.Acp.Configuration.ConfigureCarrierAndOffsets("", integrationBandwidth, NumberOfOffsets,
             channelSpacing);
         specAn.Acp.Configuration.ConfigureAveraging("", averagingEnabled, averagingCount, averagingType);
         specAn.Commit("");

         maxDeviceInstantaneousBandwidth = rfsaSession.DeviceCharacteristics.MaxInstantaneousBandwidth;
         // maxDeviceInstantaneousBandwidth = -0.1;
         instrSession.GetRecommendedNumberOfRecords("", out numberOfRecords);
         instrSession.GetRecommendedSpectralAcquisitionSpan("", out spectralAcquisitionSpan);

         if (maxDeviceInstantaneousBandwidth >= spectralAcquisitionSpan)
         {
            // IQ Acquisition.
            rfsaSession.Configuration.AcquisitionType = RfsaAcquisitionType.IQ;

            double acquisitionTime, minimumSampleRate;
            instrSession.GetRecommendedIQAcquisitionTime("", out acquisitionTime);
            instrSession.GetRecommendedIQMinimumSampleRate("", out minimumSampleRate);

            numberOfSamples = (long)(Math.Ceiling(minimumSampleRate * acquisitionTime));
            rfsaSession.Configuration.IQ.IQRate = minimumSampleRate;
            rfsaSession.Configuration.IQ.NumberOfSamples = numberOfSamples;
            rfsaSession.Configuration.IQ.NumberOfRecordsIsFinite = true;
            rfsaSession.Configuration.IQ.NumberOfRecords = numberOfRecords;
            rfsaSession.Acquisition.IQ.Initiate();

            for (int i = 0; i < numberOfRecords; i++)
            {
               reset = (i == 0) ? true : false;
               RfsaWaveformInfo iqInfo;
               ComplexDouble[] iqData = rfsaSession.Acquisition.IQ.FetchIQSingleRecordComplex<ComplexDouble>(i, numberOfSamples, new PrecisionTimeSpan(timeout), out iqInfo);
               ComplexWaveform<ComplexSingle> iq = CreateWaveform(iqData, iqInfo);
               specAn.AnalyzeIQ1Waveform("", "", iq, reset, reserved);
            }
         }
         else
         {
            // Spectral Acquisition.
            rfsaSession.Configuration.AcquisitionType = RfsaAcquisitionType.Spectrum;

            RFmxInstrMXRecommendedSpectralFftWindow recommendedFftWindow;
            RfsaFftWindowType fftWindowType;
            double resolutionBandwidth;

            instrSession.GetRecommendedSpectralFftWindow("", out recommendedFftWindow);
            instrSession.GetRecommendedSpectralResolutionBandwidth("", out resolutionBandwidth);
            switch (recommendedFftWindow)
            {
               case RFmxInstrMXRecommendedSpectralFftWindow.FlatTop:
                  fftWindowType = RfsaFftWindowType.FlatTop;
                  break;
               case RFmxInstrMXRecommendedSpectralFftWindow.Hanning:
                  fftWindowType = RfsaFftWindowType.Hanning;
                  break;
               case RFmxInstrMXRecommendedSpectralFftWindow.Hamming:
                  fftWindowType = RfsaFftWindowType.Hamming;
                  break;
               case RFmxInstrMXRecommendedSpectralFftWindow.Gaussian:
                  fftWindowType = RfsaFftWindowType.Gaussian;
                  break;
               case RFmxInstrMXRecommendedSpectralFftWindow.Blackman:
                  fftWindowType = RfsaFftWindowType.Blackman;
                  break;
               case RFmxInstrMXRecommendedSpectralFftWindow.BlackmanHarris:
                  fftWindowType = RfsaFftWindowType.BlackmanHarris;
                  break;
               case RFmxInstrMXRecommendedSpectralFftWindow.KaiserBessel:
                  fftWindowType = RfsaFftWindowType.KaiserBessel;
                  break;
               default:
                  fftWindowType = RfsaFftWindowType.Uniform;
                  break;
            }

            double updatedCenterFrequency = rfsaSession.Configuration.Spectrum.CenterFrequency;
            rfsaSession.Configuration.Spectrum.ResolutionBandwidth = resolutionBandwidth;
            rfsaSession.Configuration.Spectrum.FftWindowType = fftWindowType;
            rfsaSession.Configuration.Spectrum.ResolutionBandwidthType = RfsaResolutionBandwidthType.RbwBinWidth;
            rfsaSession.Configuration.Spectrum.PowerSpectrumUnits = RfsaPowerSpectrumUnits.dBm;
            rfsaSession.Configuration.Spectrum.Span = spectralAcquisitionSpan;

            for (int i = 0; i < numberOfRecords; i++)
            {
               reset = (i == 0) ? true : false;
               RfsaSpectrumInfo spectrumInfo;
               double[] spectrumData = rfsaSession.Acquisition.Spectrum.ReadPowerSpectrum(new PrecisionTimeSpan(timeout), out spectrumInfo);
               Spectrum<float> powerSpectrum = CreateSpectrum(spectrumData, spectrumInfo);
               powerSpectrum.StartFrequency = powerSpectrum.StartFrequency - updatedCenterFrequency;
               specAn.AnalyzeSpectrum1Waveform("", "", powerSpectrum, reset, reserved);
            }
         }
      }

      ComplexWaveform<ComplexSingle> CreateWaveform(ComplexDouble[] data, RfsaWaveformInfo info)
      {
         ComplexWaveform<ComplexSingle> target = new ComplexWaveform<ComplexSingle>(data.Length);
         var targetBuffer = target.GetWritableBuffer();
         for (int i = 0; i < data.Length; i++)
         {
            targetBuffer[i] = new ComplexSingle(Convert.ToSingle(data[i].Real), Convert.ToSingle(data[i].Imaginary));
         }
         target.PrecisionTiming = PrecisionWaveformTiming.CreateWithRegularInterval(
             new PrecisionTimeSpan(info.XIncrement),
             new PrecisionTimeSpan(info.AbsoluteInitialX));
         return target;
      }

      Spectrum<float> CreateSpectrum(double[] data, RfsaSpectrumInfo info)
      {
         Spectrum<float> target = new Spectrum<float>(data.Length);
         var targetBuffer = target.GetWritableBuffer();
         for (int i = 0; i < data.Length; i++)
         {
            targetBuffer[i] = Convert.ToSingle(data[i]);
         }
         target.StartFrequency = info.InitialFrequency;
         target.FrequencyIncrement = info.FrequencyIncrement;
         return target;
      }

      void RetrieveResults()
      {
         /* Retrieve results */

         Spectrum<float> spectrum = null;
         double totalRelativePower, carrierFrequency;

         specAn.Acp.Results.FetchOffsetMeasurementArray("", timeout, ref lowerRelativePower,
                                                        ref upperRelativePower,
                                                        ref lowerAbsolutePower,
                                                        ref upperAbsolutePower);

         specAn.Acp.Results.FetchCarrierMeasurement("", timeout, out absolutePower,
                                                    out totalRelativePower,
                                                    out carrierFrequency,
                                                    out integrationBandwidth);

         specAn.Acp.Results.FetchSpectrum("", timeout, ref spectrum);
      }

      void PrintResults()
      {
         Console.WriteLine("-----------------Carrier Measurements-----------------\n");
         Console.WriteLine("Absolute Power (dBm)       {0}", absolutePower);

         Console.WriteLine("\n--------------Offset Channel Measurements-------------\n");
         for (int i = 0; i < NumberOfOffsets; i++)
         {
            Console.WriteLine("----Offset {0}\n", i);
            Console.WriteLine("Lower Relative Power (dB)            {0}", lowerRelativePower[i]);
            Console.WriteLine("Upper Relative Power (dB)            {0}", upperRelativePower[i]);
            Console.WriteLine("Lower Absolute Power (dBm)           {0}", lowerAbsolutePower[i]);
            Console.WriteLine("Upper Absolute Power (dBm)           {0}", upperAbsolutePower[i]);
         }
         Console.WriteLine("-------------------------------------------------\n");
      }

      void CloseSession()
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
            if (rfsaSession != null)
            {
               rfsaSession.Dispose();
               rfsaSession = null;
            }
         }
         catch (Exception ex)
         {
            DisplayError(ex);
         }
      }

      void DisplayError(Exception ex)
      {
         Console.WriteLine("ERROR:\n" + ex.GetType() + ": " + ex.Message);
      }
   }
}
