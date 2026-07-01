using System;
using NationalInstruments.RFToolkits.Wlan.Generation;
using NationalInstruments.ModularInstruments.NIRfsg;
using NationalInstruments.ModularInstruments.NIRfsg.Internal;
using System.Runtime.InteropServices;

namespace _80211GenerateWaveformMultiUser
{
   class Program
   {
      static void Main(string[] args)
      {
         GenerateWaveformMultipleRFSGExample example = new GenerateWaveformMultipleRFSGExample();
         example.Run();
      }
   }

   class GenerateWaveformMultipleRFSGExample
   {
      const int triggerFrameSize = 100;
      int triggerFrameguardIntervalType;
      int triggerFrameAPTxPower, triggerFrameSTBCAllStreamsEnabled, triggerFrameNumberOfLtfSymbols;
      int triggerFrameRUSize, triggerFrameRUOffset, triggerFrameMCSIndex, triggerFramenumberOfSpaceTimeStream;
      int triggerFrameDCMEnabled, triggerFrameFECCodingType, triggerFramePayloadDataLength, triggerFrameTargetRSSI;
      int triggerFrameSTAID;
      int csRequired, ltfSize;
      int[] triggerFrameMSDUBits = new int[triggerFrameSize];
      double triggerFrameChannelBandwidth;
      double channelBandwidth;
      int standard, MCSIndex, MACHeaderFrameControl, MACHeaderDuration, macHeaderRA, macHeaderTA;
      int payloadDataLength, ofdmDataRate, frameType, macPaddingDuration, triggerFrameNumberOfUsers, midamblePeriodicity;
      int lSigLength, preFecPaddingFactor, peDisambiguity, ldpcExtraSymbolSegment;

      NIRfsg rfsgSession;
      NIRfsg externalLOSession = null;
      String rfsgResourceName = "RIO0";
      double powerLevel;
      double externalAttenuation;
      double carrierFrequency;
      niWLANG wlanSession = null;
      niWLANG triggerFrameSession = null;
      String rfsgClockSource = RfsgFrequencyReferenceSource.OnboardClock;

      String waveformName = "Wlan";
      String channelString;
      int[] triggerLines = { 0, 1 };

      public void Run()
      {
         try
         {
            initGlobalVaribales();
            ConfigureWlanGenerationSession();
            ConfigureRfsgSession();
            CreateAndDownloadWaveform();
            StopGeneration();
         }
         catch (Exception e)
         {
            Console.WriteLine("Error : " + e.ToString());
            Console.WriteLine("Press any key exit");
            Console.ReadKey();
         }
         finally
         {
            if (wlanSession != null)
               wlanSession.Close();
            if (rfsgSession != null)
               rfsgSession.Close();
         }
      }

      private void initGlobalVaribales()
      {
         triggerFrameChannelBandwidth = 20e+6;
         triggerFrameguardIntervalType = niWLANGConstants.GuardIntervalTypeOneByFour;
         triggerFrameAPTxPower = 0;
         triggerFrameSTBCAllStreamsEnabled = niWLANGConstants.False;
         triggerFrameNumberOfLtfSymbols = -1;
         triggerFrameRUSize = niWLANGConstants.RuSize26;
         triggerFrameRUOffset = 0;
         triggerFrameMCSIndex = 0;
         triggerFramenumberOfSpaceTimeStream = 1;
         triggerFrameDCMEnabled = niWLANGConstants.False;
         triggerFrameFECCodingType = niWLANGConstants.FecCodingTypeLdpc;
         triggerFramePayloadDataLength = 100;
         triggerFrameTargetRSSI = 78;
         triggerFrameSTAID = 0;
         triggerFrameNumberOfUsers = 1;
         midamblePeriodicity = niWLANGConstants.MidamblePeriodicityNone;

         standard = niWLANGConstants.Standard80211agOfdm;
         channelBandwidth = 20e+6;
         MCSIndex = 0;
         csRequired = 0;
         ltfSize = niWLANGConstants.HeLtfSizeAuto;
         ofdmDataRate = niWLANGConstants.OfdmDataRate6;
         frameType = niWLANGConstants.PayloadFrameTypeTriggerFrame;
         macPaddingDuration = niWLANGConstants.MaximumPaddingDuration0us;
         MACHeaderFrameControl = 0x0024;
         MACHeaderDuration = 0x0000;
         macHeaderRA = 0x000000000000;
         macHeaderTA = 0x000000000000;

         lSigLength = -1;
         preFecPaddingFactor = -1;
         peDisambiguity = -1;
         ldpcExtraSymbolSegment = -1;

         powerLevel = -10;
         externalAttenuation = 0;
         carrierFrequency = 5.18e+9;
      }

      private void ConfigureWlanGenerationSession()
      {
         int isNewSession, actualArraySize, arraySize, generationDone;
         double MSDUBitSize;
         if (triggerFrameSession == null)
            triggerFrameSession = new niWLANG("WLANGT", niWLANGConstants.CompatibilityVersion060000, out isNewSession);

         triggerFrameSession.SetStandard(null, niWLANGConstants.Standard80211AxMimoOfdm);
         triggerFrameSession.SetChannelBandwidth(null, triggerFrameChannelBandwidth);
         triggerFrameSession.SetOFDMGuardIntervalType(null, triggerFrameguardIntervalType);
         triggerFrameSession.SetPPDUType(null, niWLANGConstants.PpduTypeTriggerBasedPpdu);
         triggerFrameSession.SetAPTXPower(null, triggerFrameAPTxPower);
         triggerFrameSession.SetAmpduEnabled(null, niWLANGConstants.False);
         triggerFrameSession.SetStbcAllStreamsEnabled(null, triggerFrameSTBCAllStreamsEnabled);
         triggerFrameSession.SetNumberOfLtfSymbols(null, triggerFrameNumberOfLtfSymbols);
         triggerFrameSession.SetNumberOfUsers(null, triggerFrameNumberOfUsers);
         triggerFrameSession.SetTriggerFrameCSRequired(channelString, csRequired);
         triggerFrameSession.SetLtfSize(channelString, ltfSize);
         triggerFrameSession.SetOfdmMidamblePeriodicity(null, midamblePeriodicity);

         channelString = "user0";
         triggerFrameSession.SetRUSize(channelString, triggerFrameRUSize);
         triggerFrameSession.SetRUOffsetMruIndex(channelString, triggerFrameRUOffset);
         triggerFrameSession.SetMcsIndex(channelString, triggerFrameMCSIndex);
         triggerFrameSession.SetNumberOfSpaceTimeStreams(channelString, triggerFramenumberOfSpaceTimeStream);
         triggerFrameSession.SetDualCarrierModulationEnabled(channelString, triggerFrameDCMEnabled);
         triggerFrameSession.SetFecCodingType(channelString, triggerFrameFECCodingType);
         triggerFrameSession.SetPayloadDataLength(channelString, triggerFramePayloadDataLength);
         triggerFrameSession.SetTargetRSSI(channelString, triggerFrameTargetRSSI);
         triggerFrameSession.SetSTAID(channelString, triggerFrameSTAID);

         if (lSigLength == -1)
         {
            triggerFrameSession.SetMacFcsEnabled(channelString, niWLANGConstants.False);
            triggerFrameSession.SetMacHeaderEnabled(channelString, niWLANGConstants.False);
            triggerFrameSession.SetPayloadDataLength(channelString, triggerFramePayloadDataLength);
         }
         else
         {
            triggerFrameSession.SetLSIGLength(null, lSigLength);
            triggerFrameSession.SetPreFECPaddingFactor(null, preFecPaddingFactor);
            triggerFrameSession.SetPEDisambiguity(null, peDisambiguity);
            triggerFrameSession.SetLdpcExtraSymbolSegment(null, ldpcExtraSymbolSegment);
            triggerFrameSession.SetNumberOfMPDUs(null, niWLANGConstants.True);
            triggerFrameSession.SetAutoPayloadDataLengthMode(null, niWLANGConstants.True);
         }

         triggerFrameSession.CreateTriggerFrameMSDU(null, out generationDone, null, out actualArraySize);
         if (actualArraySize > 0)
         {
            triggerFrameMSDUBits = new int[actualArraySize];
            triggerFrameSession.CreateTriggerFrameMSDU(null, out generationDone, triggerFrameMSDUBits, out arraySize);

         }
         if (wlanSession == null)
            wlanSession = new niWLANG("WLANG", niWLANGConstants.CompatibilityVersion050000, out isNewSession);

         wlanSession.SetStandard(null, standard);
         wlanSession.SetChannelBandwidth(null, channelBandwidth);
         wlanSession.SetOFDMDataRate(null, ofdmDataRate);
         wlanSession.SetMcsIndex(null, MCSIndex);

         wlanSession.SetRFBlankingEnabled(null, niWLANGConstants.True);

         wlanSession.SetPayloadMacFrameType(null, frameType);
         wlanSession.SetMacFrameControl(null, MACHeaderFrameControl);
         wlanSession.SetMacDurationOrId(null, MACHeaderDuration);
         wlanSession.SetMacAddress1(null, macHeaderRA);
         wlanSession.SetMacAddress2(null, macHeaderTA);
         wlanSession.SetTriggerFrameMaximumMacPaddingDuration(null, macPaddingDuration);

         wlanSession.SetPayloadDataType(null, niWLANGConstants.UserDefined);
         wlanSession.SetPayloadUserDefinedBits(null, triggerFrameMSDUBits, actualArraySize);
         MSDUBitSize = actualArraySize / 8;
         payloadDataLength = (int)Math.Ceiling(MSDUBitSize);
         wlanSession.SetPayloadDataLength(null, payloadDataLength);
      }

      private void CreateAndDownloadWaveform()
      {
         double iqRate, waveformDuration, actualHeadRoom;
         int iqWaveformSize;
         IntPtr externalLOHandle = new IntPtr();

         String script = @"script GenerateWlan
				                repeat forever
					                generate Wlan
				                end repeat
			                end script";

         if (externalLOSession != null)
            externalLOHandle = externalLOSession.GetInstrumentHandle().DangerousGetHandle();
         wlanSession.RFSGConfigureFrequencySingleLO(new IntPtr[] { rfsgSession.GetInstrumentHandle().DangerousGetHandle() }, niWLANGConstants.LOSourceOnboard, externalLOHandle, carrierFrequency,
                                                 niWLANGConstants.False, niWLANGConstants.False);

         if (standard == niWLANGConstants.Standard80211AcMimoOfdm || standard == niWLANGConstants.Standard80211nMimoOfdm ||
             standard == niWLANGConstants.Standard80211AxMimoOfdm || standard == niWLANGConstants.Standard80211beMimoOfdm || standard == niWLANGConstants.Standard80211bnMimoOfdm)
         {
            wlanSession.RFSGCreateAndDownloadMIMOWaveforms(new IntPtr[] { rfsgSession.GetInstrumentHandle().DangerousGetHandle() }, null, 1, waveformName);
         }
         else
         {
            wlanSession.RFSGCreateAndDownloadMIMOWaveforms(new IntPtr[] { rfsgSession.GetInstrumentHandle().DangerousGetHandle() }, null, 1, waveformName);
            channelString = "";
         }


         triggerFrameSession.GetIqRate(null, out iqRate);
         triggerFrameSession.GetIqWaveformSize(null, out iqWaveformSize);
         waveformDuration = iqWaveformSize / iqRate;

         Console.WriteLine("Waveform Duration : {0}", waveformDuration);
         channelString = "channel0";
         triggerFrameSession.GetActualHeadroom(channelString, out actualHeadRoom);

         Console.WriteLine("Actual HeadRoom :", actualHeadRoom);

         wlanSession.Close();

         niWLANG.WLANG_RFSGConfigureScript(rfsgSession.GetInstrumentHandle().DangerousGetHandle(), null, script, powerLevel);
         rfsgSession.Initiate();

         //Check for successful generation
         CheckGeneration();

         Console.WriteLine("Press any key to exit");
         Console.ReadKey();
      }

      private void ConfigureRfsgSession()
      {
         //Open all RFSG Sessions
         if (rfsgSession == null)
         {
            rfsgSession = new NIRfsg(rfsgResourceName, true, false);
         }
         rfsgSession.FrequencyReference.Configure(rfsgClockSource, 10.0e6);
         rfsgSession.RF.PowerLevelType = RfsgRFPowerLevelType.PeakPower;
         rfsgSession.Arb.GenerationMode = RfsgWaveformGenerationMode.Script;
         rfsgSession.RF.ExternalGain = -(externalAttenuation);
      }

      private bool CheckGeneration()
      {
         RfsgGenerationStatus status = rfsgSession.CheckGenerationStatus();
         return Convert.ToBoolean(status);
      }

      private void StopGeneration()
      {
         rfsgSession.Abort();
         rfsgSession.RF.OutputEnabled = false;
         rfsgSession.Utility.Commit();
         niWLANG.WLANG_RFSGClearDatabase(rfsgSession.GetInstrumentHandle().DangerousGetHandle(), "", waveformName);
      }
   }
}
