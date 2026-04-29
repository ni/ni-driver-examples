//Steps:
//1. Open a new RFmx session
//2. Configure the basic instrument properties (Clock Source, Clock Frequency)
//3. Configure Selected Ports
//4. Configure the basic signal properties  (Center Frequency, Reference Level and External Attenuation)
//5. Select SEM measurement and enable the traces
//6. Configure SEM Sweep Time
//7. Configure SEM Power Units and Reference Type
//8. Configure SEM Averaging
//9. Configure SEM FFT
//10. Configure SEM Number of Carrier Channel
//11. Configure SEM Carrier Offset, Integration BW, RBW Filter for all the Carriers using Selector String
//12. Configure SEM Number of Offsets
//13. Configure SEM Offset Frequency, RBW filter, Limit Fail Mask, Absolute Limit
//for all offsets using Selector String
//14. Initiate Measurement
//15. Fetch SEM Lower Offset Power, Lower Offset Margin for all the offsets
//16. Fetch SEM Upper Offset Power, Upper Offset Margin for all the offsets
//17. Fetch SEM Carrier Measurement for all the Carriers
//18. Fetch SEM Traces
//19. Close the RFmx session

using System;
using NationalInstruments;
using NationalInstruments.RFmx.InstrMX;
using NationalInstruments.RFmx.SpecAnMX;

namespace NationalInstruments.Examples.RFmxSpecAnSemAdvanced
{
   public class RFmxSpecAnSemAdvanced
   {
      RFmxInstrMX instrSession;
      RFmxSpecAnMX specAn;
      String resourceName, frequencySource;
      string selectedPorts;
      double centerFrequency, referenceLevel, externalAttenuation, frequency, timeout;
      int averagingCount;
      RFmxSpecAnMXSemReferenceType referenceType;
      RFmxSpecAnMXSemPowerUnits powerUnits;
      RFmxSpecAnMXSemSweepTimeAuto sweepTimeAuto;
      RFmxSpecAnMXSemAveragingEnabled averagingEnabled;
      RFmxSpecAnMXSemAveragingType averagingType;
      RFmxSpecAnMXSemFftWindow fftWindow;
      double sweepTimeInterval, fftPadding;

      const int NumberOfCarriers = 1;
      const int NumberOfOffsets = 2;

      struct CarrierChannel
      {
         public double carrierFrequency, integrationBandwidth, channelBandwidth, rbw, rrcFilterAlpha;
         public RFmxSpecAnMXSemCarrierRbwAutoBandwidth rbwAuto;
         public RFmxSpecAnMXSemCarrierRbwFilterType rbwFilterType;
         public RFmxSpecAnMXSemCarrierRrcFilterEnabled rrcFilterEnabled;
      };

      struct OffsetMeasurementPower
      {
         public double[] offsetSegmentAbsolutepower;
         public double[] offsetSegmentTotalRelativePower;
         public double[] offsetSegmentPeakAbsolutePower;
         public double[] offsetSegmentPeakFrequency;
         public double[] offsetSegmentPeakRelativeFrequency;
         public OffsetMeasurementPower(Int32 numOfOffsets)
         {
            offsetSegmentAbsolutepower = new double[numOfOffsets];
            offsetSegmentTotalRelativePower = new double[numOfOffsets];
            offsetSegmentPeakAbsolutePower = new double[numOfOffsets];
            offsetSegmentPeakFrequency = new double[numOfOffsets];
            offsetSegmentPeakRelativeFrequency = new double[numOfOffsets];
         }
      };

      struct OffsetMeasurementMargin
      {
         public double[] offsetSegmentMargin;
         public double[] offsetSegmentMarginAbsolutePower;
         public double[] offsetSegmentMarginRelativePower;
         public double[] offsetSegmentMarginFrequency;
         public OffsetMeasurementMargin(Int32 numOfOffsets)
         {
            offsetSegmentMargin = new double[numOfOffsets];
            offsetSegmentMarginAbsolutePower = new double[numOfOffsets];
            offsetSegmentMarginRelativePower = new double[numOfOffsets];
            offsetSegmentMarginFrequency = new double[numOfOffsets];
         }
      };

      struct LowerOffsetMeasurement
      {
         public OffsetMeasurementPower power;
         public OffsetMeasurementMargin margin;
         public RFmxSpecAnMXSemLowerOffsetMeasurementStatus[] status;
         public LowerOffsetMeasurement(Int32 numOfOffSets)
         {
            power = new OffsetMeasurementPower(numOfOffSets);
            margin = new OffsetMeasurementMargin(numOfOffSets);
            status = new RFmxSpecAnMXSemLowerOffsetMeasurementStatus[NumberOfOffsets];
         }
      };

      struct UpperOffsetMeasurement
      {
         public OffsetMeasurementPower power;
         public OffsetMeasurementMargin margin;
         public RFmxSpecAnMXSemUpperOffsetMeasurementStatus[] status;
         public UpperOffsetMeasurement(Int32 numOfOffSets)
         {
            power = new OffsetMeasurementPower(numOfOffSets);
            margin = new OffsetMeasurementMargin(numOfOffSets);
            status = new RFmxSpecAnMXSemUpperOffsetMeasurementStatus[NumberOfOffsets];
         }
      };

      struct CarrierMeasurement
      {
         public double absolutePower;
         public double peakAbsolutePower;
         public double peakFrequency;
         public double totalRelativePower;
      };

      //Carrier channels and Offset segments inputs
      CarrierChannel[] carrierChannel = new CarrierChannel[NumberOfCarriers];

      //Carrier channels and Offset segment measurement results        
      CarrierMeasurement[] carrierMeasurement = new CarrierMeasurement[NumberOfCarriers];
      LowerOffsetMeasurement lowerOffset = new LowerOffsetMeasurement(NumberOfOffsets);
      UpperOffsetMeasurement upperOffset = new UpperOffsetMeasurement(NumberOfOffsets);

      double totalCarrierPower;
      RFmxSpecAnMXSemCompositeMeasurementStatus compositeMeasurementStatus;

      /*offset segment inputs*/
      public RFmxSpecAnMXSemOffsetEnabled[] enabled = new RFmxSpecAnMXSemOffsetEnabled[NumberOfOffsets];
      public RFmxSpecAnMXSemOffsetSideband[] frequencySideband = new RFmxSpecAnMXSemOffsetSideband[NumberOfOffsets];
      public RFmxSpecAnMXSemOffsetRbwAutoBandwidth[] rbwAuto = new RFmxSpecAnMXSemOffsetRbwAutoBandwidth[NumberOfOffsets];
      public RFmxSpecAnMXSemOffsetRbwFilterType[] rbwFilterType = new RFmxSpecAnMXSemOffsetRbwFilterType[NumberOfOffsets];
      public RFmxSpecAnMXSemOffsetLimitFailMask[] limitFailMask = new RFmxSpecAnMXSemOffsetLimitFailMask[NumberOfOffsets];
      public RFmxSpecAnMXSemOffsetAbsoluteLimitMode[] absoluteLimitMode = new RFmxSpecAnMXSemOffsetAbsoluteLimitMode[NumberOfOffsets];
      public RFmxSpecAnMXSemOffsetRelativeLimitMode[] relativeLimitMode = new RFmxSpecAnMXSemOffsetRelativeLimitMode[NumberOfOffsets];
      public double[] startFrequency = new double[NumberOfOffsets];
      public double[] stopFrequecny = new double[NumberOfOffsets];
      public double[] rbw = new double[NumberOfOffsets];
      public double[] absoluteStartLimit = new double[NumberOfOffsets];
      public double[] absoluteStopLimit = new double[NumberOfOffsets];
      public double[] relativeStartLimit = new double[NumberOfOffsets];
      public double[] relativeStopLimit = new double[NumberOfOffsets];
      public RFmxSpecAnMXSemOffsetFrequencyDefinition[] frequencyDefinition = new RFmxSpecAnMXSemOffsetFrequencyDefinition[NumberOfOffsets];

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
            Console.WriteLine("Press any key to exit.....");
            Console.ReadKey();
         }
      }

      private void InitializeVariables()
      {
         resourceName = "RFSA";

         selectedPorts = "";
         centerFrequency = 1e+9;         /* Hz */
         referenceLevel = 0.00;          /* dBm */
         externalAttenuation = 0.00;     /* dB */

         frequencySource = RFmxInstrMXConstants.OnboardClock;
         frequency = 10.0e+6;            /* Hz */

         timeout = 10.0;                 /* seconds */

         referenceType = RFmxSpecAnMXSemReferenceType.Integration;
         powerUnits = RFmxSpecAnMXSemPowerUnits.dBm;

         //Sweep Time
         sweepTimeAuto = RFmxSpecAnMXSemSweepTimeAuto.True;
         sweepTimeInterval = 1.0e-3;

         //Averaging
         averagingEnabled = RFmxSpecAnMXSemAveragingEnabled.False;
         averagingCount = 10;
         averagingType = RFmxSpecAnMXSemAveragingType.Rms;

         //FFT Window
         fftWindow = RFmxSpecAnMXSemFftWindow.FlatTop;
         fftPadding = -1.00;

         //Set up the carrier channel inputs
         for (int i = 0; i < NumberOfCarriers; i++)
         {
            carrierChannel[i].carrierFrequency = 0.00;
            carrierChannel[i].integrationBandwidth = 2.0e+6;
            carrierChannel[i].channelBandwidth = 2.0e+6;
            carrierChannel[i].rbwAuto = RFmxSpecAnMXSemCarrierRbwAutoBandwidth.True;
            carrierChannel[i].rbwFilterType = RFmxSpecAnMXSemCarrierRbwFilterType.Gaussian;
            carrierChannel[i].rbw = 10.0e+3;
            carrierChannel[i].rrcFilterEnabled = RFmxSpecAnMXSemCarrierRrcFilterEnabled.False;
            carrierChannel[i].rrcFilterAlpha = 0.220;
         }

         //Set up the offset segment inputs
         for (int i = 0; i < NumberOfOffsets; i++)
         {
            if (i == 0)
            {
               startFrequency[i] = 1.0e+6;
               stopFrequecny[i] = 2.0e+6;
               relativeLimitMode[i] = RFmxSpecAnMXSemOffsetRelativeLimitMode.Manual;
               relativeStartLimit[i] = -10.00;
               relativeStopLimit[i] = -30.00;
            }
            else
            {
               startFrequency[i] = 2.0e+6;
               stopFrequecny[i] = 3.0e+6;
               relativeLimitMode[i] = RFmxSpecAnMXSemOffsetRelativeLimitMode.Couple;
               relativeStartLimit[i] = -30.00;
               relativeStopLimit[i] = -30.00;
            }

            enabled[i] = RFmxSpecAnMXSemOffsetEnabled.True;
            frequencySideband[i] = RFmxSpecAnMXSemOffsetSideband.Both;
            rbwAuto[i] = RFmxSpecAnMXSemOffsetRbwAutoBandwidth.True;
            rbwFilterType[i] = RFmxSpecAnMXSemOffsetRbwFilterType.Gaussian;
            rbw[i] = 10.0e+3;
            limitFailMask[i] = RFmxSpecAnMXSemOffsetLimitFailMask.Absolute;
            absoluteLimitMode[i] = RFmxSpecAnMXSemOffsetAbsoluteLimitMode.Couple;
            absoluteStartLimit[i] = -10.00;
            absoluteStopLimit[i] = -10.00;
            frequencyDefinition[i] = RFmxSpecAnMXSemOffsetFrequencyDefinition.CarrierCenterToMeasurementBandwidthCenter;
         }
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
         instrSession.ConfigureFrequencyReference("", frequencySource, frequency);
         specAn.SetSelectedPorts("", selectedPorts);
         specAn.ConfigureFrequency("", centerFrequency);
         specAn.ConfigureReferenceLevel("", referenceLevel);
         specAn.ConfigureExternalAttenuation("", externalAttenuation);
         specAn.SelectMeasurements("", RFmxSpecAnMXMeasurementTypes.Sem, true);
         specAn.Sem.Configuration.ConfigureSweepTime("", sweepTimeAuto, sweepTimeInterval);
         specAn.Sem.Configuration.ConfigurePowerUnits("", powerUnits);
         specAn.Sem.Configuration.ConfigureReferenceType("", referenceType);
         specAn.Sem.Configuration.ConfigureAveraging("", averagingEnabled, averagingCount, averagingType);
         specAn.Sem.Configuration.ConfigureFft("", fftWindow, fftPadding);

         string carrierString, offsetString;
         specAn.Sem.Configuration.ConfigureNumberOfCarriers("", NumberOfCarriers);
         for (int i = 0; i < NumberOfCarriers; i++)
         {
            carrierString = RFmxSpecAnMX.BuildCarrierString2("", i);
            specAn.Sem.Configuration.ConfigureCarrierFrequency(carrierString,
                                                            carrierChannel[i].carrierFrequency);
            specAn.Sem.Configuration.ConfigureCarrierIntegrationBandwidth(carrierString,
                                                                          carrierChannel[i].integrationBandwidth);
            specAn.Sem.Configuration.ConfigureCarrierRbwFilter(carrierString,
                                                               carrierChannel[i].rbwAuto,
                                                               carrierChannel[i].rbw,
                                                               carrierChannel[i].rbwFilterType);
            specAn.Sem.Configuration.ConfigureCarrierRrcFilter(carrierString,
                                                               carrierChannel[i].rrcFilterEnabled,
                                                               carrierChannel[i].rrcFilterAlpha);
            specAn.Sem.Configuration.ConfigureCarrierChannelBandwidth(carrierString,
                                                                      carrierChannel[i].channelBandwidth);
         }

         specAn.Sem.Configuration.ConfigureNumberOfOffsets("", NumberOfOffsets);
         for (int i = 0; i < NumberOfOffsets; i++)
         {
            offsetString = RFmxSpecAnMX.BuildOffsetString2("", i);

            specAn.Sem.Configuration.ConfigureOffsetLimitFailMask(offsetString,
                                                                  limitFailMask[i]);

            specAn.Sem.Configuration.ConfigureOffsetFrequencyDefinition(offsetString,
                                                                        frequencyDefinition[i]);
         }
         specAn.Sem.Configuration.ConfigureOffsetFrequencyArray("",
                                                               startFrequency,
                                                               stopFrequecny,
                                                               enabled,
                                                               frequencySideband);
         specAn.Sem.Configuration.ConfigureOffsetRbwFilterArray("",
                                                           rbwAuto,
                                                           rbw,
                                                           rbwFilterType);
         specAn.Sem.Configuration.ConfigureOffsetAbsoluteLimitArray("",
                                                                   absoluteLimitMode,
                                                                   absoluteStartLimit,
                                                                   absoluteStopLimit);
         specAn.Sem.Configuration.ConfigureOffsetRelativeLimitArray("",
                                                               relativeLimitMode,
                                                               relativeStartLimit,
                                                               relativeStopLimit);

         specAn.Initiate("", "");
      }

      private void RetrieveResults()
      {
         /* Retrieve results */

         string carrierString;

         specAn.Sem.Results.FetchLowerOffsetPowerArray("", timeout,
                                                      ref lowerOffset.power.offsetSegmentAbsolutepower,
                                                      ref lowerOffset.power.offsetSegmentTotalRelativePower,
                                                      ref lowerOffset.power.offsetSegmentPeakAbsolutePower,
                                                      ref lowerOffset.power.offsetSegmentPeakFrequency,
                                                      ref lowerOffset.power.offsetSegmentPeakRelativeFrequency);

         specAn.Sem.Results.FetchLowerOffsetMarginArray("", timeout,
                                                   ref lowerOffset.status,
                                                   ref lowerOffset.margin.offsetSegmentMargin,
                                                   ref lowerOffset.margin.offsetSegmentMarginFrequency,
                                                   ref lowerOffset.margin.offsetSegmentMarginAbsolutePower,
                                                   ref lowerOffset.margin.offsetSegmentMarginRelativePower);

         specAn.Sem.Results.FetchUpperOffsetPowerArray("", timeout,
                                                  ref upperOffset.power.offsetSegmentAbsolutepower,
                                                  ref upperOffset.power.offsetSegmentTotalRelativePower,
                                                  ref upperOffset.power.offsetSegmentPeakAbsolutePower,
                                                  ref upperOffset.power.offsetSegmentPeakFrequency,
                                                  ref upperOffset.power.offsetSegmentPeakRelativeFrequency);

         specAn.Sem.Results.FetchUpperOffsetMarginArray("", timeout,
                                                   ref upperOffset.status,
                                                   ref upperOffset.margin.offsetSegmentMargin,
                                                   ref upperOffset.margin.offsetSegmentMarginFrequency,
                                                   ref upperOffset.margin.offsetSegmentMarginAbsolutePower,
                                                   ref upperOffset.margin.offsetSegmentMarginRelativePower);

         for (int i = 0; i < NumberOfCarriers; i++)
         {
            carrierString = RFmxSpecAnMX.BuildCarrierString2("", i);
            specAn.Sem.Results.FetchCarrierMeasurement(carrierString, timeout,
                                                       out carrierMeasurement[i].absolutePower,
                                                       out carrierMeasurement[i].peakAbsolutePower,
                                                       out carrierMeasurement[i].peakFrequency,
                                                       out carrierMeasurement[i].totalRelativePower);
         }

         Spectrum<float> absoluteMaskTrace = null;
         specAn.Sem.Results.FetchAbsoluteMaskTrace("", timeout, ref absoluteMaskTrace);

         Spectrum<float> relativeMaskTrace = null;
         specAn.Sem.Results.FetchRelativeMaskTrace("", timeout, ref relativeMaskTrace);

         Spectrum<float> spectrum = null;
         specAn.Sem.Results.FetchSpectrum("", timeout, ref spectrum);

         specAn.Sem.Results.FetchTotalCarrierPower("", timeout, out totalCarrierPower);

         specAn.Sem.Results.FetchCompositeMeasurementStatus("", timeout, out compositeMeasurementStatus);
      }

      private void PrintResults()
      {
         string status = "Fail";
         if (compositeMeasurementStatus == RFmxSpecAnMXSemCompositeMeasurementStatus.Pass)
            status = "Pass";

         Console.WriteLine("Composite measurement status:         : {0}", status);
         Console.WriteLine("Total Carrier Power (dBm or dBm/Hz)   : {0}\n", totalCarrierPower);

         Console.WriteLine("--------------Carrier Measurements-----------------------------\n");
         for (int i = 0; i < NumberOfCarriers; i++)
         {
            Console.WriteLine("*** Carrier {0} ***\n", i);
            Console.WriteLine("Absolute Power (dBm or dBm/Hz)      : {0}",
                              carrierMeasurement[i].absolutePower);
            Console.WriteLine("Total Relative Power(dB)            : {0}",
                              carrierMeasurement[i].totalRelativePower);
            Console.WriteLine("Peak Absolute Power (dBm or dbm/Hz) : {0}",
                              carrierMeasurement[i].peakAbsolutePower);
            Console.WriteLine("Peak Frequency                      : {0}",
                              carrierMeasurement[i].peakFrequency);
            Console.WriteLine("-----------------------------------------------------------\n");
         }

         Console.WriteLine("--------------Offset segment measurements ---------------------\n");
         for (int i = 0; i < NumberOfOffsets; i++)
         {
            Console.WriteLine("*** Offset {0} ***\n", i);
            Console.WriteLine("Lower offset : Total Absolute Power (dBm or dBm/Hz)  : {0}",
                              lowerOffset.power.offsetSegmentAbsolutepower[i]);
            Console.WriteLine("Lower offset : Total Relative Power (dB)             : {0}",
                              lowerOffset.power.offsetSegmentTotalRelativePower[i]);
            Console.WriteLine("Lower Offset : Peak Absolute Power (dBm or dBm/Hz)   : {0}",
                              lowerOffset.power.offsetSegmentPeakAbsolutePower[i]);
            Console.WriteLine("Lower offset : Peak Frequency (Hz)                   : {0}",
                              lowerOffset.power.offsetSegmentPeakFrequency[i]);
            Console.WriteLine("Lower offset : Peak Relative Power (dB)              : {0}",
                              lowerOffset.power.offsetSegmentPeakRelativeFrequency[i]);
            Console.WriteLine("Lower Offset : Margin (dB)                           : {0}",
                              lowerOffset.margin.offsetSegmentMargin[i]);
            Console.WriteLine("Lower offset : Margin Absolute Power (dBm or dBm/Hz) : {0}",
                              lowerOffset.margin.offsetSegmentMarginAbsolutePower[i]);
            Console.WriteLine("Lower offset : Margin Relative Power (dB)            : {0}",
                              lowerOffset.margin.offsetSegmentMarginRelativePower[i]);
            Console.WriteLine("Lower offset : Margin Frequency (Hz)                 : {0}",
                              lowerOffset.margin.offsetSegmentMarginFrequency[i]);

            status = "Fail";
            if (lowerOffset.status[i] == RFmxSpecAnMXSemLowerOffsetMeasurementStatus.Pass)
               status = "Pass";
            Console.WriteLine("Lower offset : Measurement Status                    : {0}\n", status);

            Console.WriteLine("\nUpper offset : Total Absolute Power (dBm or dBm/Hz)  : {0}",
                              upperOffset.power.offsetSegmentAbsolutepower[i]);
            Console.WriteLine("Upper offset : Total Relative Power (dB)             : {0}",
                              upperOffset.power.offsetSegmentTotalRelativePower[i]);
            Console.WriteLine("Upper Offset : Peak Absolute Power (dBm or dBm/Hz)   : {0}",
                              upperOffset.power.offsetSegmentPeakAbsolutePower[i]);
            Console.WriteLine("Upper offset : Peak Frequency (Hz)                   : {0}",
                              upperOffset.power.offsetSegmentPeakFrequency[i]);
            Console.WriteLine("Upper offset : Peak Relative Power (dB)              : {0}",
                              upperOffset.power.offsetSegmentPeakRelativeFrequency[i]);
            Console.WriteLine("Upper Offset : Margin (dB)                           : {0}",
                              upperOffset.margin.offsetSegmentMargin[i]);
            Console.WriteLine("Upper offset : Margin Absolute Power (dBm or dBm/Hz) : {0}",
                               upperOffset.margin.offsetSegmentMarginAbsolutePower[i]);
            Console.WriteLine("Upper offset : Margin Relative Power (dB)            : {0}",
                              upperOffset.margin.offsetSegmentMarginRelativePower[i]);
            Console.WriteLine("Upper offset : Margin Frequency (Hz)                 : {0}",
                              upperOffset.margin.offsetSegmentMarginFrequency[i]);

            status = "Fail";
            if (upperOffset.status[i] == RFmxSpecAnMXSemUpperOffsetMeasurementStatus.Pass)
               status = String.Copy("Pass");
            Console.WriteLine("Upper offset : Measurement Status                    : {0}", status);
            Console.WriteLine("-----------------------------------------------------------\n");
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

      static private void DisplayError(Exception ex)
      {
         Console.WriteLine("ERROR:\n" + ex.GetType() + ": " + ex.Message);
      }
   }
}
