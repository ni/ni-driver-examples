// Steps:
// 1. Open NI - RFSG session.
// 2. Configure RFSG Selected Ports.
// 3. Configure RFSG frequency reference.
// 4. Configure RFSG configuration settled event to the device scriptTrigger0, to make sure generation starts only after
//      RFSG configuration settled.
// 5. Configure RFSG to advance upon receipt of RFSA Ready for Advance event through PXI trigger line
// 6. Configure frequency and external gain of RF output signal. 
// 7. Get terminal name for marker0 and assign to the RFSA Reference Trigger Digital Edge source 
// 8. Create a Configuration List. Pass Power Level in the Configuration List Properties parameter to be able to configure 
//     Power Level in each step that we create. The Set As Active List parameter in this VI defaults to true, this will set the    
//     Active Configuration List property to the name of the created configuration list. Once the Active Configuration List 
//     is set, using a property node to access Power Level will modify the property for this configuration list.
// 9. Create a Configuration List Step. The Set As Active Step parameter in this VI defaults to true, this will set the Active 
//     Configuration List Step property to the created configuration list step index. Once the Active Configuration List 
//     Step is set, using a property node to access Power Level will modify the property for this configuration list step in 
//     the configuration list indicated by the Active Configuration List property.
// 10. Configure the Power Level for the Active Configuration List Step in the Active Configuration List.
// 11. Read waveform from file and download Waveform from file to RFSG.
// 12. Retrieve waveform PAPR and add the same to the RFSA reference level while configuring to every list step
// 13. Set Automatic SG SA Shared LO to Enabled.
// 14. Set LO Offset Mode to Auto while performing an in-band ModAcc measurement. This causes the RFSG LO to be 
//       placed outside the signal, if signal bandwidth is less than half of the device instantaneous bandwidth; otherwise,
//       the LO is placed at the center of the signal.
// 15. Write script to generate the waveform specified in the script. This script is programmed to generate waveform 
//       continuously and generate a marker at the start of the waveform (sample 0).
// 16. Open a new RFmx Session.
// 17. Configure the Frequency Reference properties (Clock Source and Clock Frequency).
// 18.Export RFSA Ready for Advance event to PXI trigger line. 
// 19. Create RFmx LTE List.
// 20. Create RFmx LTE List Step.
// 21. Configure the Reference Level for the Specified List Step in the LTE List.
// 22. Configure personality and measurement parameters for a List Step.
// 23  Configure Center Frequency, Selected Ports, External Attenuation, Trigger Type and Trigger Parameters for all List Steps.
// 24. Configure Link Direction, CC Spacing Type, CC bandwidth, Band, Duplex Scheme, In-Band Emission Mask Type, EVM Unit
//        and Auto DMRS Detection Enabled for all List Steps.
// 25. Set LO Leakage Avoidance Enabled to True and Automatic SG SA Shared LO to Enabled. Enabling LO Leakage Avoidance causes RFmx 
//        to place the SA LO outside the measurement bandwidth, if the measurement bandwidth is less than half of the device instantaneous 
//        bandwidth; otherwise, the LO is placed at the center of the signal.
// 26.  Select ModAcc measurement and disable traces for all List Steps.
// 27.  Initiate ModAcc measurement for RFmx LTE List.
// 28.  Initiate signal generation.
// 29.  Wait for Acquisition to complete.
// 30. Stop signal generation.
// 31. Delete the Configuration List to avoid committing the Configuration List to the hardware with subsequent 
//       calls to Commit. Deleting the list will reset the Active Configuration List.
// 32. Fetch ModAcc measurement Results for all List Steps one by one.
// 33. Delete RFmx LTE List
// 34. Close the RFmx Session.
// 35. Close the RFSG session. 
//      It is recommended to clear the waveform before closing RFSG session.
using System;
using NationalInstruments.RFmx.InstrMX;
using NationalInstruments.RFmx.LteMX;
using NationalInstruments.ModularInstruments.NIRfsg;
using NationalInstruments.ModularInstruments.NIRfsgPlayback;

namespace NationalInstruments.Examples.RFmxLteListModeWithSGSAHandshaking
{
 public class RFmxLteListModeWithSGSAHandshaking
 {
  RFmxInstrMX instrSession;
  RFmxLteMXList LteList;
  NIRfsg rfsgSession;
  IntPtr instrumentHandle;

  double centerFrequency;                    

  string rfsgResourceName;
  string rfsgSelectedPorts;
  string waveformFilePath;
  string waveformName;

  double rfsgExternalAttenuation;            

  RfsgFrequencyReferenceSource rfsgFrequencyReferenceSource;
  double rfsgFrequency;                      

  string rfsaResourceName;
  string rfsaSelectedPorts;
  double rfsaExternalAttenuation;            

  string rfsaFrequencyReferenceSource;
  double rfsaFrequency;                      

  RFmxLteMXDigitalEdgeTriggerEdge digitalEdge;
  double triggerDelay;                       

  int numberOfSteps;

  double startReferenceLevel;                
  double stopReferenceLevel;                 

  double carrierBandwidth;                   
  int band;
  RFmxLteMXDuplexScheme duplexScheme;
  RFmxLteMXModAccInBandEmissionMaskType inBandEmissionMaskType;
  RFmxLteMXModAccEvmUnit evmUnit;

  double[] rampPattern;
  double timeout;                            
  string script;
  double papr;
  RFmxLteMX[] step;
  string configurationSettledEvenTerminalName;
  string markerEventTerminalName;

  double[] meanRmsCompositeEvm;                                          /*(% or dB)*/
  double[] maximumPeakCompositeEvm;                                      /*(% or dB)*/

  public void Run()
  {
   try
   {
    InitializeVariables();
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
    CloseSession();
    Console.WriteLine("Press any key to exit");
    Console.ReadKey();
   }
  }

  void InitializeVariables()
  {
   centerFrequency = 1950000000.0;                                        /* (Hz) */

   rfsgResourceName = "RFSG";
   rfsgSelectedPorts = "";
   waveformFilePath = "LTE_UL_FDD_CC-1_BW-10MHz_PUSCH-QPSK.tdms";
   waveformName = "Wfm";

   rfsgExternalAttenuation = 0.0;                                         /* (dB ) */ 

   rfsgFrequencyReferenceSource = RfsgFrequencyReferenceSource.OnboardClock;
   rfsgFrequency = 10000000.0;                                            /* (Hz) */

   rfsaResourceName = "RFSA";
   rfsaSelectedPorts = "";
   rfsaExternalAttenuation = 0.0;                                         /* (dB ) */ 

   rfsaFrequencyReferenceSource = RFmxInstrMXConstants.OnboardClock;
   rfsaFrequency = 10000000.0;                                            /* (Hz) */

   digitalEdge = RFmxLteMXDigitalEdgeTriggerEdge.Rising;
   triggerDelay = 0.0;                                                    /* (s) */

   numberOfSteps = 10;

   startReferenceLevel = -20.0;                                           /* (dBm) */
   stopReferenceLevel = 0.0;                                              /* (dBm) */

   carrierBandwidth = 10000000.0;                                         /* (Hz) */

   band = 1;
   duplexScheme = RFmxLteMXDuplexScheme.Fdd;
   evmUnit = RFmxLteMXModAccEvmUnit.Percentage;
   rampPattern = new double[numberOfSteps];
   LinearRampPattern(startReferenceLevel, stopReferenceLevel, numberOfSteps, false, ref rampPattern);

   timeout = 10.0;                                                       /*(s) */

   script = string.Format("script GenerateWaveform\n  repeat forever\n    generate {0} marker0(0)\n" +
         "  wait until scripttrigger0\n   end repeat\n  end script", waveformName);

   step = new RFmxLteMX[numberOfSteps];
   meanRmsCompositeEvm = new double[numberOfSteps];                      
   maximumPeakCompositeEvm = new double[numberOfSteps];                  
  }

  void ConfigureRfsg()
  {
   rfsgSession = new NIRfsg(rfsgResourceName, true, false);
   rfsgSession.SignalPath.SelectedPorts = rfsgSelectedPorts;
   rfsgSession.FrequencyReference.Configure(rfsgFrequencyReferenceSource, rfsgFrequency);
   configurationSettledEvenTerminalName = rfsgSession.DeviceEvents.ConfigurationSettledEvent.TerminalName;
   rfsgSession.Triggers.ScriptTriggers[0].DigitalEdge.Configure(configurationSettledEvenTerminalName,
       RfsgTriggerEdge.RisingEdge);
   rfsgSession.Triggers.ConfigurationListStepTrigger.DigitalEdge.Configure(
       RfsgDigitalEdgeConfigurationListStepTriggerSource.PxiTriggerLine0, RfsgTriggerEdge.RisingEdge);
   rfsgSession.RF.ExternalGain = -1 * rfsgExternalAttenuation;
   rfsgSession.RF.Frequency = centerFrequency;
   markerEventTerminalName = rfsgSession.DeviceEvents.MarkerEvents[0].TerminalName;
   RfsgConfigurationListProperties[] properties = new RfsgConfigurationListProperties[1] {
       RfsgConfigurationListProperties.PowerLevel };
   rfsgSession.BasicConfigurationList.CreateConfigurationList("PowerLevelList", properties, true);
   for (int i = 0; i < numberOfSteps; i++)
   {
    rfsgSession.BasicConfigurationList.CreateStep(true);
    rfsgSession.RF.PowerLevel = rampPattern[i];
   }
   instrumentHandle = rfsgSession.GetInstrumentHandle().DangerousGetHandle();
   NIRfsgPlayback.ReadAndDownloadWaveformFromFile(instrumentHandle, waveformFilePath, waveformName);
   NIRfsgPlayback.RetrieveWaveformPapr(instrumentHandle, waveformName, out papr);
   NIRfsgPlayback.StoreAutomaticSGSASharedLO(instrumentHandle, "", RfsgPlaybackAutomaticSGSASharedLO.Enabled);
   NIRfsgPlayback.StoreWaveformLOOffsetMode(instrumentHandle, waveformName, NIRfsgPlaybackLOOffsetMode.Auto);
   NIRfsgPlayback.SetScriptToGenerateSingleRfsg(instrumentHandle, script);
  }

  void ConfigureRFmx()
  {
   instrSession = new RFmxInstrMX(rfsaResourceName, "");
   instrSession.ConfigureFrequencyReference("", rfsaFrequencyReferenceSource, rfsaFrequency);
   instrSession.ExportSignal(RFmxInstrMXExportSignalSource.ReadyForAdvanceEvent,
       RFmxInstrMXConstants.PxiTriggerLine0);
   LteList = instrSession.GetLteList("ModAcc_List");
   for( int i = 0; i < numberOfSteps; i++)
   {
    step[i] = LteList.CreateListStep();
    step[i].SetReferenceLevel("", papr + rampPattern[i]);
   }
   RFmxLteMX stepAll = LteList.GetListStepAll();
   stepAll.ConfigureFrequency("", centerFrequency);
   stepAll.SetSelectedPorts("", rfsaSelectedPorts);
   stepAll.ConfigureDigitalEdgeTrigger("", markerEventTerminalName, digitalEdge, triggerDelay, true);
   stepAll.ConfigureExternalAttenuation("", rfsaExternalAttenuation);

   stepAll.SetLinkDirection("", RFmxLteMXLinkDirection.Uplink);
   stepAll.ComponentCarrier.ConfigureSpacing("", RFmxLteMXComponentCarrierSpacingType.Nominal, 0);
   stepAll.ComponentCarrier.SetBandwidth("", carrierBandwidth);
   stepAll.SetBand("", band);
   stepAll.SetDuplexScheme("", duplexScheme);
   stepAll.ModAcc.Configuration.ConfigureInBandEmissionMaskType("", inBandEmissionMaskType);
   stepAll.ModAcc.Configuration.ConfigureEvmUnit("", evmUnit);
   stepAll.ConfigureAutoDmrsDetectionEnabled("", RFmxLteMXAutoDmrsDetectionEnabled.True);

   instrSession.SetLOSource("", RFmxInstrMXConstants.LOSourceAutomaticSGSAShared);
   instrSession.SetLOLeakageAvoidanceEnabled("", RFmxInstrMXLOLeakageAvoidanceEnabled.True);

   stepAll.SelectMeasurements("", RFmxLteMXMeasurementTypes.ModAcc, false);
   LteList.Initiate("", "");
   rfsgSession.Initiate();
   instrSession.WaitForAcquisitionComplete(timeout);
  }

  void RetrieveResults()
  {
   for (int i = 0; i < numberOfSteps; i++)
   {
    step[i].ModAcc.Results.GetMeanRmsCompositeEvm("", out meanRmsCompositeEvm[i]);
    step[i].ModAcc.Results.GetMaximumPeakCompositeEvm("", out maximumPeakCompositeEvm[i]);
   }
  }

  void PrintResults()
  {
   Console.WriteLine("------------------Measurements------------------\n");
   Console.WriteLine("Mean RMS Composite EVM (% or dB)");
   for (int i = 0; i < numberOfSteps; i++)
    Console.WriteLine("Step{0}    : {1}", i, meanRmsCompositeEvm[i]);
   Console.WriteLine("Maximum Peak Composite EVM (% or dB)");
   for (int i = 0; i < numberOfSteps; i++)
    Console.WriteLine("Step{0}    : {1}", i, maximumPeakCompositeEvm[i]);
  }

  void CloseSession()
  {
   if (LteList != null)
   {
    LteList.Dispose();
    LteList = null;
   }
   if (instrSession != null)
   {
    instrSession.Close();
    instrSession = null;
   }
   if (rfsgSession != null)
   {
    rfsgSession.Abort();
    rfsgSession.BasicConfigurationList.DeleteConfigurationList("PowerLevelList");
    NIRfsgPlayback.ClearWaveform(instrumentHandle, waveformName);
    rfsgSession.Close();
    rfsgSession = null;
   }
  }

  static void DisplayError(Exception ex)
  {
   Console.WriteLine("ERROR:\n" + ex.GetType() + ": " + ex.Message);
  }

  void LinearRampPattern(double start, double end, int samples, bool includeEnd, ref double[] rampPattern)
  {
   int m = includeEnd ? samples : (samples - 1);
   double delta = (end - start) / m;
   for (int i = 0; i < samples; i++)
   {
    rampPattern[i] = start + (i * delta);
   }
  }

 }
}
