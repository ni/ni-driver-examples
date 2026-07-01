using System;
using System.Collections.Generic;
using System.Collections;
using System.Windows.Forms;
using NationalInstruments.ModularInstruments.NIRfsg;
using NationalInstruments.RFToolkits.BT.Generation;


namespace NationalInstruments.Examples.BTLoadConfigurationAndGenerate
{
   public partial class MainForm : Form
   {
      NIRfsg rfsgSession;
      niBTSG btsgSession;
      string resourceName, referenceClockSource, exportClock, waveformName, script;
      string filename;
      string model;
      double powerLevel, carrierFrequency, upConverterCenterFrequency, upConverterCenterFrequencyOffset, actualHeadroom, externalAttenuation, iCommonModeOffset, qCommonModeOffset, iOffset, qOffset;

      int channelNumber, standard, outputPort, terminalConfiguration;

      public MainForm()
      {
         InitializeComponent();
         ConfigureNumericUpDown();
         ConfigureClkOutTerminalComboBox();
         ConfigureRefSourceComboBox();
         ConfigureStandardComboBox();
         ConfigureOutputPortComboBox();
         ConfigureTerminalConfigurationComboBox();
      }

      #region UI Initial Value Config Section
      private void ConfigureNumericUpDown()
      {
         chnNumberNumeric.Minimum = 0;
         chnNumberNumeric.Maximum = 79;
         chnNumberNumeric.Increment = 1;
         chnNumberNumeric.Value = 3;
         chnNumberNumeric.DecimalPlaces = 0;

         powerLevelNumeric.Minimum = decimal.MinValue;
         powerLevelNumeric.Maximum = decimal.MaxValue;
         powerLevelNumeric.Increment = 1;
         powerLevelNumeric.Value = 0;
         powerLevelNumeric.DecimalPlaces = 2;

         externalAttnNumeric.Minimum = decimal.MinValue;
         externalAttnNumeric.Maximum = decimal.MaxValue;
         externalAttnNumeric.Increment = 0.1M;
         externalAttnNumeric.Value = 0;
         externalAttnNumeric.DecimalPlaces = 2;
      }

      private void ConfigureOutputPortComboBox()
      {
         var opList = new List<DictionaryEntry>();
         opList.Add(new DictionaryEntry("RF Out", RfsgOutputPort.RFOut));
         opList.Add(new DictionaryEntry("IQ Out", RfsgOutputPort.IQOut));
         outputPortComboBox.DataSource = opList;
         outputPortComboBox.DisplayMember = "Key";
         outputPortComboBox.ValueMember = "Value";
         outputPortComboBox.SelectedIndex = 0;
      }

      private void ConfigureTerminalConfigurationComboBox()
      {
         var tcList = new List<DictionaryEntry>();
         tcList.Add(new DictionaryEntry("Differential", RfsgTerminalConfiguration.Differential));
         tcList.Add(new DictionaryEntry("SingleEnded", RfsgTerminalConfiguration.SingleEnded));
         terminalConfigurationComboBox.DataSource = tcList;
         terminalConfigurationComboBox.DisplayMember = "Key";
         terminalConfigurationComboBox.ValueMember = "Value";
         terminalConfigurationComboBox.SelectedIndex = 0;
      }


      private void ConfigureClkOutTerminalComboBox()
      {
         var clkOutTerminalValueList = new List<DictionaryEntry>();
         clkOutTerminalValueList.Add(new DictionaryEntry("Do not export clock", RfsgOutputTerminal.DoNotExport));
         clkOutTerminalValueList.Add(new DictionaryEntry("RefOut", RfsgOutputTerminal.ReferenceOut));
         clkOutTerminalValueList.Add(new DictionaryEntry("RefOut2", RfsgOutputTerminal.ReferenceOut2));
         clkOutTerminalValueList.Add(new DictionaryEntry("ClkOut", RfsgOutputTerminal.ClockOut));
         clkOutTerminalComboBox.DataSource = clkOutTerminalValueList;
         clkOutTerminalComboBox.DisplayMember = "Key";
         clkOutTerminalComboBox.ValueMember = "Value";
         clkOutTerminalComboBox.SelectedIndex = 0;
      }

      private void ConfigureStandardComboBox()
      {
         var standardValueList = new List<DictionaryEntry>();
         standardValueList.Add(new DictionaryEntry("Basic+EDR", niBTSGConstants.StandardBasicEDR));
         standardValueList.Add(new DictionaryEntry("LE", niBTSGConstants.StandardLE));
         standardComboBox.DataSource = standardValueList;
         standardComboBox.DisplayMember = "Key";
         standardComboBox.ValueMember = "Value";
         standardComboBox.SelectedIndex = 0;
      }


      private void ConfigureRefSourceComboBox()
      {
         var refSourceValueList = new List<DictionaryEntry>();
         refSourceValueList.Add(new DictionaryEntry("OnBoardClock", RfsgFrequencyReferenceSource.OnboardClock));
         refSourceValueList.Add(new DictionaryEntry("RefIn", RfsgFrequencyReferenceSource.ReferenceIn));
         refSourceValueList.Add(new DictionaryEntry("PXI_CLK", RfsgFrequencyReferenceSource.PxiClock));
         refSourceValueList.Add(new DictionaryEntry("ClkIn", RfsgFrequencyReferenceSource.ClockIn));
         refSourceComboBox.DataSource = refSourceValueList;
         refSourceComboBox.DisplayMember = "Key";
         refSourceComboBox.ValueMember = "Value";
         refSourceComboBox.SelectedValue = RfsgFrequencyReferenceSource.PxiClock;
      }

      #endregion

      void ProcessTimerEvent(object sender, System.EventArgs e)
      {
         CheckGeneration();
      }

      void CheckGeneration()
      {
         try
         {
            RfsgGenerationStatus status = RfsgGenerationStatus.InProgress;
            if (rfsgSession != null)
               status = rfsgSession.CheckGenerationStatus();
         }
         catch (System.Exception exception)
         {
            ShowError("CheckGeneration()", exception);
         }
      }

      void ShowError(string functionName, System.Exception exception)
      {
         StopGeneration();
         CloseSession();

         // Display error to the user
         string exceptionMessage = string.IsNullOrEmpty(exception.Message) ? "Undefined Error." : exception.Message;
         errorTextBox.Text = "Error in " + functionName + System.Environment.NewLine + exceptionMessage;
         MessageBox.Show(exceptionMessage, functionName, MessageBoxButtons.OK, MessageBoxIcon.Error); ;
      }

      void StopGeneration()
      {
         //Stop the status checking timer
         timer.Enabled = false;
         if (rfsgSession != null)
         {
            rfsgSession.Abort();
            rfsgSession.RF.OutputEnabled = false;
            rfsgSession.Utility.Commit();
            try
            {
               rfsgSession.Abort();
               niBTSG.RFSGClearDatabase(rfsgSession.GetInstrumentHandle().DangerousGetHandle(), null, waveformName);
               rfsgSession.Dispose();
            }
            catch
            {
               // ignoring the exception here - the RFSGClearDatabase API throws an exception
               // if called more than once on the same waveformName
            }
         }

         // Deactivate 'Stop' button
         stopButton.Enabled = false;

         // Activate 'Start' button
         generateButton.Enabled = true;
      }

      void MainFormClosing(object sender, FormClosingEventArgs e)
      {
         StopGeneration();
         CloseSession();
      }

      void CloseSession()
      {
         // Close the RFSG session
         if (rfsgSession != null)
         {
            rfsgSession.Abort();
            niBTSG.RFSGClearDatabase(rfsgSession.GetInstrumentHandle().DangerousGetHandle(), null, "");
            rfsgSession.Dispose();
            rfsgSession = null;
         }

         // Close the BT Session
         if (btsgSession != null)
         {
            btsgSession.CloseSession();
            btsgSession = null;
         }
      }

      void StartGeneration()
      {
         /*Bluetooth Session*/
         if (btsgSession == null)
         {
            btsgSession = new niBTSG(niBTSGConstants.ToolkitCompatibilityVersion020000);
         }
         /*Rfsg Session*/
         if (rfsgSession == null)
         {
            rfsgSession = new NIRfsg(resourceName, true, true);
         }

         /*Bluetooth Properties*/
         btsgSession.LoadConfigurationFromFile(filename, niBTSGConstants.True);
         niBTSG.ChannelNumberToCarrierFrequency(channelNumber, standard, out carrierFrequency);
         carrierFreqTextBox.Text = carrierFrequency.ToString();

         //get model
         model = rfsgSession.Identity.InstrumentModel;

         /*RFSG Properties*/
         rfsgSession.FrequencyReference.Configure((RfsgFrequencyReferenceSource)referenceClockSource, 10.0e6);
         rfsgSession.Utility.ExportSignal(RfsgSignalType.ReferenceClock, "", exportClock);
         if (model.Equals("NI PXIe-5644R") || model.Equals("NI PXIe-5645R") || model.Equals("NI PXIe-5646R"))
         {
            if (outputPort == (int)RfsgOutputPort.IQOut)
            {
               rfsgSession.RF.PowerLevelType = RfsgRFPowerLevelType.PeakPower;
               rfsgSession.Arb.GenerationMode = RfsgWaveformGenerationMode.Script;
               rfsgSession.IQOutPort["I"].TerminalConfiguration = (RfsgTerminalConfiguration)terminalConfiguration;
               rfsgSession.IQOutPort["I"].CommonModeOffset = iCommonModeOffset;
               rfsgSession.IQOutPort["I"].Offset = iOffset;
               rfsgSession.IQOutPort["Q"].CommonModeOffset = qCommonModeOffset;
               rfsgSession.IQOutPort["Q"].Offset = qOffset;
               carrierFrequency = 0;
            }
            else
            {
               rfsgSession.RF.Frequency = carrierFrequency;
               rfsgSession.RF.PowerLevelType = RfsgRFPowerLevelType.PeakPower;
               rfsgSession.Arb.GenerationMode = RfsgWaveformGenerationMode.Script;
               upConverterCenterFrequencyOffset = 0;
               upConverterCenterFrequency = carrierFrequency + upConverterCenterFrequencyOffset;
               rfsgSession.RF.Upconverter.CenterFrequency = upConverterCenterFrequency;
            }
         }
         else
         {
            rfsgSession.RF.Frequency = carrierFrequency;
            rfsgSession.RF.PowerLevelType = RfsgRFPowerLevelType.PeakPower;
            rfsgSession.Arb.GenerationMode = RfsgWaveformGenerationMode.Script;
            upConverterCenterFrequencyOffset = 0;
            upConverterCenterFrequency = carrierFrequency + upConverterCenterFrequencyOffset;
            rfsgSession.RF.Upconverter.CenterFrequency = upConverterCenterFrequency;
         }
         rfsgSession.RF.ExternalGain = -(externalAttenuation);

         /*Create & Download Waveform*/
         btsgSession.RFSGCreateAndDownloadWaveform(rfsgSession.GetInstrumentHandle().DangerousGetHandle(), string.Empty, waveformName);
         btsgSession.GetActualHeadroom(null, out actualHeadroom);
         actualHeadroomTextBox.Text = actualHeadroom.ToString();

         /*Execute Script*/
         niBTSG.RFSGConfigureScript(rfsgSession.GetInstrumentHandle().DangerousGetHandle(), null, script, powerLevel);
         rfsgSession.RF.OutputEnabled = true;
         rfsgSession.Initiate();

         /*Start the status checking timer */
         CheckGeneration();
         timer.Enabled = true;
      }

      private void browseButton_Click(object sender, System.EventArgs e)
      {
         OpenFileDialog ofg = new OpenFileDialog();
         ofg.Filter = "TDMS Files (*.tdms)|*.tdms|All files (*.*)|*.*";
         if (ofg.ShowDialog() == DialogResult.OK)
         {
            filePathTextBox.Text = ofg.FileName;
         }
      }

      private void generateButton_Click(object sender, System.EventArgs e)
      {
         try
         {
            //Deactivate the 'Start' button
            generateButton.Enabled = false;
            //Activate the 'Stop' button
            stopButton.Enabled = true;

            //Reset the error message text box
            errorTextBox.Text = "No Error";
            //Read the Panel control values
            resourceName = (string)rfsgResourceTextBox.Text;
            referenceClockSource = refSourceComboBox.SelectedValue.ToString();
            exportClock = clkOutTerminalComboBox.SelectedValue.ToString();
            filename = (string)filePathTextBox.Text;
            waveformName = (string)waveNameTextBox.Text;
            script = (string)scriptTextBox.Text;
            carrierFrequency = double.Parse(carrierFreqTextBox.Text);
            externalAttenuation = (double)externalAttnNumeric.Value;
            channelNumber = (int)chnNumberNumeric.Value;
            powerLevel = (double)powerLevelNumeric.Value;
            actualHeadroom = System.Convert.ToDouble(actualHeadroomTextBox.Text);
            iOffset = (double)iOffsetNumeric.Value;
            qOffset = (double)qOffsetNumeric.Value;
            iCommonModeOffset = (double)iCommonModeOffsetNumeric.Value;
            qCommonModeOffset = (double)qCommonModeOffsetNumeric.Value;
            outputPort = (int)outputPortComboBox.SelectedValue;
            standard = (int)standardComboBox.SelectedValue;
            terminalConfiguration = (int)terminalConfigurationComboBox.SelectedValue;
            //  BT Generation 
            StartGeneration();

            // Start the status checking timer
            timer.Enabled = true;
         }
         catch (System.Exception exception)
         {
            ShowError("StartGeneration()", exception);
         }
      }

      private void stopButton_Click(object sender, System.EventArgs e)
      {
         //Stop the status checking timer
         timer.Enabled = false;
         if (rfsgSession != null)
         {
            rfsgSession.RF.OutputEnabled = false;
            try
            {
               niBTSG.RFSGClearDatabase(rfsgSession.GetInstrumentHandle().DangerousGetHandle(), null, waveformName);
            }
            catch
            {
               // ignoring the exception here - the RFSGClearDatabase API throws an exception
               // if called more than once on the same waveformName
            }
         }
         // Deactivate 'Stop' button
         stopButton.Enabled = false;

         // Activate 'Start' button
         generateButton.Enabled = true;
      }
   }
}