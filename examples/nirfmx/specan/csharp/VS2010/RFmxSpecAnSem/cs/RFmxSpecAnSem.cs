//Steps:
//1. Open a new RFmx session
//2. Configure the basic instrument properties (Clock Source, Clock Frequency)
//3. Configure Selected Ports
//4. Configure the basic signal properties  (Center Frequency, Reference Level and External Attenuation)
//5. Select SEM measurement and enable the traces
//6. Configure SEM Power Units and Reference Type
//7. Configure SEM Averaging
//8. Configure SEM Integration BW
//9. Configure SEM RBW Filter
//10. Configure SEM RRC Filter
//11. Configure SEM Number of Offsets
//12. Configure SEM Offset Frequency
//Use Array API's to configure all offset parameters as an array
//13. Configure SEM Offset Absolute Limit
//14. Configure SEM Offset Relative Limit
//15. Configure Offset RBW Filter
//16. Configure Offset Limit Fail Mask
//17. Initiate Measurement
//18. Fetch SEM Measurements and Traces
//19. Close the RFmx Session

using System;
using NationalInstruments;
using NationalInstruments.RFmx.InstrMX;
using NationalInstruments.RFmx.SpecAnMX;

namespace NationalInstruments.Examples.RFmxSpecAnSem
{
   public class RFmxSpecAnSem
   {
      RFmxInstrMX instrSession;
      RFmxSpecAnMX specAn;
      String resourceName, frequencySource;
      string selectedPorts;
      double centerFrequency, referenceLevel, externalAttenuation, frequency,
             integrationBandwidth, rbw, rrcFilterAlpha;
      RFmxSpecAnMXSemCarrierRbwAutoBandwidth rbwAuto;
      RFmxSpecAnMXSemCarrierRbwFilterType rbwFilterType;
      RFmxSpecAnMXSemCarrierRrcFilterEnabled rrcFilterEnabled;
      RFmxSpecAnMXSemReferenceType referenceType;
      RFmxSpecAnMXSemPowerUnits powerUnits;
      int averagingCount;
      RFmxSpecAnMXSemAveragingEnabled averagingEnabled;
      RFmxSpecAnMXSemAveragingType averagingType;
      RFmxSpecAnMXSemOffsetLimitFailMask limitFailMask;
      double timeout = 10.0;

      const int NumberOfOffsets = 2;

      RFmxSpecAnMXSemOffsetEnabled[] offsetEnabled = new RFmxSpecAnMXSemOffsetEnabled[NumberOfOffsets];
      RFmxSpecAnMXSemOffsetSideband[] offsetFrequencySideband = new RFmxSpecAnMXSemOffsetSideband[NumberOfOffsets];
      RFmxSpecAnMXSemOffsetRbwAutoBandwidth[] offsetRbwAuto = new RFmxSpecAnMXSemOffsetRbwAutoBandwidth[NumberOfOffsets];
      RFmxSpecAnMXSemOffsetRbwFilterType[] offsetRbwFilterType = new RFmxSpecAnMXSemOffsetRbwFilterType[NumberOfOffsets];
      RFmxSpecAnMXSemOffsetAbsoluteLimitMode[] offsetAbsoluteLimitMode = new RFmxSpecAnMXSemOffsetAbsoluteLimitMode[NumberOfOffsets];
      RFmxSpecAnMXSemOffsetRelativeLimitMode[] offsetRelativeLimitMode = new RFmxSpecAnMXSemOffsetRelativeLimitMode[NumberOfOffsets];
      double[] offsetStartFrequency = new double[NumberOfOffsets];
      double[] offsetStopFrequecny = new double[NumberOfOffsets];
      double[] offsetRbw = new double[NumberOfOffsets];
      double[] offsetAbsoluteLimitStart = new double[NumberOfOffsets];
      double[] offsetAbsoluteLimitStop = new double[NumberOfOffsets];
      double[] offsetRelativeLimitStart = new double[NumberOfOffsets];
      double[] offsetRelativeLimitStop = new double[NumberOfOffsets];

      //Output values
      RFmxSpecAnMXSemCompositeMeasurementStatus compositeMeasurementStatus;
      double absolutePower;
      double peakAbsolutePower;
      double peakFrequency;
      double totalRelativePower;

      double[] lowerOffsetTotalAbsolutePower;
      double[] lowerOffsetTotalRelativePower;
      double[] lowerOffsetPeakAbsolutePower;
      double[] lowerOffsetPeakFrequency;
      double[] lowerOffsetPeakRelativePower;
      double[] lowerOffsetMargin;
      double[] lowerOffsetMarginAbsolutePower;
      double[] lowerOffsetMarginRelativePower;
      double[] lowerOffsetMarginFrequency;
      RFmxSpecAnMXSemLowerOffsetMeasurementStatus[] lowerOffsetMeasurementStatus;

      double[] upperOffsetTotalAbsolutePower;
      double[] upperOffsetTotalRelativePower;
      double[] upperOffsetPeakAbsolutePower;
      double[] upperOffsetPeakFrequency;
      double[] upperOffsetPeakRelativePower;
      double[] upperOffsetMargin;
      double[] upperOffsetMarginAbsolutePower;
      double[] upperOffsetMarginRelativePower;
      double[] upperOffsetMarginFrequency;
      RFmxSpecAnMXSemUpperOffsetMeasurementStatus[] upperOffsetMeasurementStatus;

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
         centerFrequency = 1e+9;                 /* Hz */
         referenceLevel = 0.00;                  /* dBm */
         externalAttenuation = 0.00;             /* dB */

         frequencySource = RFmxInstrMXConstants.OnboardClock;
         frequency = 10.0e+6;                    /* Hz */

         integrationBandwidth = 2.0e+6;          /* Hz */
         rbwAuto = RFmxSpecAnMXSemCarrierRbwAutoBandwidth.False;
         rbwFilterType = RFmxSpecAnMXSemCarrierRbwFilterType.Gaussian;
         rbw = 10.0e+3;

         rrcFilterEnabled = RFmxSpecAnMXSemCarrierRrcFilterEnabled.False;
         rrcFilterAlpha = 0.220;

         referenceType = RFmxSpecAnMXSemReferenceType.Integration;
         powerUnits = RFmxSpecAnMXSemPowerUnits.dBm;

         averagingEnabled = RFmxSpecAnMXSemAveragingEnabled.False;
         averagingCount = 10;
         averagingType = RFmxSpecAnMXSemAveragingType.Rms;

         limitFailMask = RFmxSpecAnMXSemOffsetLimitFailMask.Absolute;

         for (int i = 0; i < NumberOfOffsets; i++)
         {
            if (i == 0)
            {
               offsetStartFrequency[i] = 1.0e+6;       /* Hz */
               offsetStopFrequecny[i] = 2.0e+6;        /* Hz */
               offsetRelativeLimitMode[i] = RFmxSpecAnMXSemOffsetRelativeLimitMode.Manual;
               offsetRelativeLimitStart[i] = -10.00;
               offsetRelativeLimitStop[i] = -30.00;
            }

            else if (i == 1)
            {
               offsetStartFrequency[i] = 2.0e+6;       /* Hz */
               offsetStopFrequecny[i] = 3.0e+6;        /* Hz */
               offsetRelativeLimitMode[i] = RFmxSpecAnMXSemOffsetRelativeLimitMode.Couple;
               offsetRelativeLimitStart[i] = -30.00;
               offsetRelativeLimitStop[i] = -30.00;
            }

            offsetRbwAuto[i] = RFmxSpecAnMXSemOffsetRbwAutoBandwidth.True;
            offsetRbwFilterType[i] = RFmxSpecAnMXSemOffsetRbwFilterType.Gaussian;
            offsetRbw[i] = 10.0e+3;                     /* Hz */
            offsetAbsoluteLimitMode[i] = RFmxSpecAnMXSemOffsetAbsoluteLimitMode.Couple;
            offsetAbsoluteLimitStart[i] = -10.00;
            offsetAbsoluteLimitStop[i] = -10.00;
            offsetEnabled[i] = RFmxSpecAnMXSemOffsetEnabled.True;
            offsetFrequencySideband[i] = RFmxSpecAnMXSemOffsetSideband.Both;
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
         specAn.ConfigureRF("", centerFrequency, referenceLevel, externalAttenuation);

         specAn.SelectMeasurements("", RFmxSpecAnMXMeasurementTypes.Sem, true);
         specAn.Sem.Configuration.ConfigurePowerUnits("", powerUnits);
         specAn.Sem.Configuration.ConfigureReferenceType("", referenceType);
         specAn.Sem.Configuration.ConfigureAveraging("", averagingEnabled, averagingCount,
                                                     averagingType);
         specAn.Sem.Configuration.ConfigureCarrierIntegrationBandwidth("", integrationBandwidth);
         specAn.Sem.Configuration.ConfigureCarrierRbwFilter("", rbwAuto, rbw, rbwFilterType);
         specAn.Sem.Configuration.ConfigureCarrierRrcFilter("", rrcFilterEnabled, rrcFilterAlpha);
         specAn.Sem.Configuration.ConfigureNumberOfOffsets("", NumberOfOffsets);
         specAn.Sem.Configuration.ConfigureOffsetFrequencyArray("", offsetStartFrequency,
                                                                offsetStopFrequecny, offsetEnabled,
                                                                offsetFrequencySideband);
         specAn.Sem.Configuration.ConfigureOffsetAbsoluteLimitArray("", offsetAbsoluteLimitMode,
                                                                    offsetAbsoluteLimitStart,
                                                                    offsetAbsoluteLimitStop);
         specAn.Sem.Configuration.ConfigureOffsetRelativeLimitArray("", offsetRelativeLimitMode,
                                                                    offsetRelativeLimitStart,
                                                                    offsetRelativeLimitStop);
         specAn.Sem.Configuration.ConfigureOffsetRbwFilterArray("", offsetRbwAuto,
                                                                offsetRbw,
                                                                offsetRbwFilterType);
         specAn.Sem.Configuration.ConfigureOffsetLimitFailMask("offset::all", limitFailMask);

         specAn.Initiate("", "");
      }

      private void RetrieveResults()
      {
         /* Retrieve results */

         Spectrum<float> absoluteMaskTrace = null;
         Spectrum<float> relativeMaskTrace = null;
         Spectrum<float> spectrum = null;

         specAn.Sem.Results.FetchLowerOffsetPowerArray("", timeout, ref lowerOffsetTotalAbsolutePower,
                                                       ref lowerOffsetTotalRelativePower,
                                                       ref lowerOffsetPeakAbsolutePower,
                                                       ref lowerOffsetPeakFrequency,
                                                       ref lowerOffsetPeakRelativePower);

         specAn.Sem.Results.FetchLowerOffsetMarginArray("", timeout, ref lowerOffsetMeasurementStatus,
                                                        ref lowerOffsetMargin,
                                                        ref lowerOffsetMarginFrequency,
                                                        ref lowerOffsetMarginAbsolutePower,
                                                        ref lowerOffsetMarginRelativePower);

         specAn.Sem.Results.FetchUpperOffsetPowerArray("", timeout, ref upperOffsetTotalAbsolutePower,
                                                       ref upperOffsetTotalRelativePower,
                                                       ref upperOffsetPeakAbsolutePower,
                                                       ref upperOffsetPeakFrequency,
                                                       ref upperOffsetPeakRelativePower);

         specAn.Sem.Results.FetchUpperOffsetMarginArray("", timeout, ref upperOffsetMeasurementStatus,
                                                        ref upperOffsetMargin,
                                                        ref upperOffsetMarginFrequency,
                                                        ref upperOffsetMarginAbsolutePower,
                                                        ref upperOffsetMarginRelativePower);

         specAn.Sem.Results.FetchCarrierMeasurement("", timeout, out absolutePower,
                                                    out peakAbsolutePower, out peakFrequency,
                                                    out totalRelativePower);

         specAn.Sem.Results.FetchAbsoluteMaskTrace("", timeout, ref absoluteMaskTrace);

         specAn.Sem.Results.FetchRelativeMaskTrace("", timeout, ref relativeMaskTrace);

         specAn.Sem.Results.FetchSpectrum("", timeout, ref spectrum);

         specAn.Sem.Results.FetchCompositeMeasurementStatus("", timeout,
                                                            out compositeMeasurementStatus);
      }

      private void PrintResults()
      {
         String status = "Fail";
         if (compositeMeasurementStatus == RFmxSpecAnMXSemCompositeMeasurementStatus.Pass)
            status = "Pass";

         Console.WriteLine("Composite measurement status         : {0}\n", status);

         Console.WriteLine("\n--------------Carrier Measurements----------------------------\n");
         Console.WriteLine("Absolute Power (dBm or dBm/Hz)       : {0}", absolutePower);
         Console.WriteLine("Peak Absolute Power (dBm or dbm/Hz)  : {0}", peakAbsolutePower);
         Console.WriteLine("Peak Frquency                        : {0}", peakFrequency);

         Console.WriteLine("\n--------------Offset segment measurements ---------------------------\n");
         for (int i = 0; i < NumberOfOffsets; i++)
         {
            Console.WriteLine("Offset {0}\n", i);

            Console.WriteLine("Lower offset : Total Absolute Power (dBm or dBm/Hz):  {0}",
                              lowerOffsetTotalAbsolutePower[i]);
            Console.WriteLine("Lower offset : Total Relative Power (dB):             {0}",
                                lowerOffsetTotalRelativePower[i]);
            Console.WriteLine("Lower Offset : Peak Absolute Power (dBm or dBm/Hz):   {0}",
                                lowerOffsetPeakAbsolutePower[i]);
            Console.WriteLine("Lower offset : Peak Frequency (Hz):                   {0}",
                                lowerOffsetPeakFrequency[i]);
            Console.WriteLine("Lower offset : Peak Relative Power (dB):              {0}",
                                lowerOffsetPeakRelativePower[i]);
            Console.WriteLine("Lower Offset : Margin (dB):                           {0}",
                                lowerOffsetMargin[i]);
            Console.WriteLine("Lower offset : Margin Absolute Power (dBm or dBm/Hz): {0}",
                                lowerOffsetMarginAbsolutePower[i]);
            Console.WriteLine("Lower offset : Margin Relative Power (dB):            {0}",
                                lowerOffsetMarginRelativePower[i]);
            Console.WriteLine("Lower offset : Margin Frequency (Hz):                 {0}",
                                lowerOffsetMarginFrequency[i]);

            status = "Fail";
            if (lowerOffsetMeasurementStatus[i].Equals(RFmxSpecAnMXSemLowerOffsetMeasurementStatus.Pass))
               status = "Pass";
            Console.WriteLine("Lower offset : Measurement Status : {0}\n", status);

            Console.WriteLine("\nUpper offset : Total Absolute Power (dBm or dBm/Hz)  : {0}",
                              upperOffsetTotalAbsolutePower[i]);
            Console.WriteLine("Upper offset : Total Relative Power (dB):              {0}",
                                upperOffsetTotalRelativePower[i]);
            Console.WriteLine("Upper Offset : Peak Absolute Power (dBm or dBm/Hz):    {0}",
                                upperOffsetPeakAbsolutePower[i]);
            Console.WriteLine("Upper offset : Peak Frequency (Hz):                    {0}",
                                upperOffsetPeakFrequency[i]);
            Console.WriteLine("Upper offset : Peak Relative Power (dB):               {0}",
                                upperOffsetPeakRelativePower[i]);
            Console.WriteLine("Upper Offset : Margin (dB):                            {0}",
                                upperOffsetMargin[i]);
            Console.WriteLine("Upper offset : Margin Absolute Power (dBm or dBm/Hz):  {0}",
                                upperOffsetMarginAbsolutePower[i]);
            Console.WriteLine("Upper offset : Margin Relative Power (dB):             {0}",
                                upperOffsetMarginRelativePower[i]);
            Console.WriteLine("Upper offset : Margin Frequency (Hz):                  {0}",
                                upperOffsetMarginFrequency[i]);

            status = "Fail";
            if (upperOffsetMeasurementStatus[i].Equals(RFmxSpecAnMXSemUpperOffsetMeasurementStatus.Pass))
               status = "Pass";
            Console.WriteLine("Upper offset : Measurement Status : {0}\n", status);
            Console.WriteLine("-----------------------------------------------------------------------\n");
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
