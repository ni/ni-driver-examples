using System.Collections.Generic;
using System.Collections;
using System.Windows.Forms;
using NationalInstruments.ModularInstruments.NIRfsg;
using NationalInstruments.RFToolkits.BT.Generation;


namespace NationalInstruments.Examples.BTReadWaveformFromFile
{
   public partial class MainForm : Form
   {
      NIRfsg rfsgSession;
      niBTSG btsgSession;

      string resourceName, referenceClockSource, exportClock, waveformName, script;
      string filepath;
      double powerLevel, iQRate, carrierFrequency, upConverterCenterFrequency, upConverterCenterFrequencyOffset, actualHeadroom, externalAttenuation, iCommonModeOffset, qCommonModeOffset, iOffset, qOffset;
      int eof, waveformSize, actwaveformSize, standard, isnewSession;
      int channelNumber, outputPort, terminalConfiguration;
      ComplexDouble[] dataWaveForm;
      string model;


      public MainForm()
      {
         InitializeComponent();
         ConfigureNumericUpDown();
         ConfigureClkOutTerminalComboBox();
         ConfigureRefSourceComboBox();
         ConfigurestopButton();
         ConfigureOutputPortComboBox();
         ConfigureTerminalConfigurationComboBox();
         ConfigureStandardComboBox();
      }

      #region UI Initial Value Config Section
      private void ConfigureNumericUpDown()
      {
         chnNumberNumeric.Minimum = 0;
         chnNumberNumeric.Maximum = 79;
         chnNumberNumeric.Increment = 1;
         chnNumberNumeric.Value = 3;
         chnNumberNumeric.DecimalPlaces = 0;

         powerLevelNumeric.Minimum = -179;
         powerLevelNumeric.Maximum = 25;
         powerLevelNumeric.Increment = 1;
         powerLevelNumeric.Value = 0;
         powerLevelNumeric.DecimalPlaces = 2;

         externalAttnNumeric.Minimum = decimal.MinValue;
         externalAttnNumeric.Maximum = decimal.MaxValue;
         externalAttnNumeric.Increment = 0.1M;
         externalAttnNumeric.Value = 0;
         externalAttnNumeric.DecimalPlaces = 2;
      }

      private void ConfigureStandardComboBox()
      {
         var standardlValueList = new List<DictionaryEntry>();
         standardlValueList.Add(new DictionaryEntry("StandardBasicEDR", niBTSGConstants.StandardBasicEDR));
         standardlValueList.Add(new DictionaryEntry("StandardLE", niBTSGConstants.StandardLE));
         standardComboBox.DataSource = standardlValueList;
         standardComboBox.DisplayMember = "Key";
         standardComboBox.ValueMember = "Value";
      }

      private void ConfigureClkOutTerminalComboBox()
      {
         var clkOutTerminalValueList = new List<DictionaryEntry>();
         clkOutTerminalValueList.Add(new DictionaryEntry("Do not export clock", RfsgOutputTerminal.DoNotExport));
         clkOutTerminalValueList.Add(new DictionaryEntry("RefOut", RfsgOutputTerminal.ReferenceOut));
         clkOutTerminalValueList.Add(new DictionaryEntry("RefOut2", RfsgOutputTerminal.ReferenceOut2));
         clkOutTerminalValueList.Add(new DictionaryEntry("ClkOut", RfsgOutputTerminal.ClockOut));
         clkOutTerminalComboBox.DataSource = clkOutTerminalValueList;
         clkOutTerminalComboBox.DataSource = clkOutTerminalValueList;
         clkOutTerminalComboBox.DisplayMember = "Key";
         clkOutTerminalComboBox.ValueMember = "Value";
         clkOutTerminalComboBox.SelectedValue = RfsgOutputTerminal.DoNotExport;
      }

      private void ConfigurestopButton()
      {
         //Deactivate the 'Stop' button
         stopButton.Enabled = false;
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

      private void ConfigureOutputPortComboBox()
      {
         var opList = new List<DictionaryEntry>();
         opList.Add(new DictionaryEntry("RF Out", RfsgOutputPort.RFOut));
         opList.Add(new DictionaryEntry("IQ Out", RfsgOutputPort.IQOut));
         opList.Add(new DictionaryEntry("Cal Out", RfsgOutputPort.CalOut));
         opList.Add(new DictionaryEntry("I Only", RfsgOutputPort.IOnly));
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

      #endregion

      void ProcessTimerEvent(object sender, System.EventArgs e)
      {
         CheckGeneration();
      }

      // --------------------------------------------------------------------------
      // Open and configure BT and RFSA sessions	
      // --------------------------------------------------------------------------
      void StartGeneration()
      {
         /*BT session*/
         if (btsgSession == null)
         {
            btsgSession = new niBTSG("BTSG", niBTSGConstants.ToolkitCompatibilityVersion020000, out isnewSession);
         }

         //Initialise RFSG
         if (rfsgSession == null)
         {
            rfsgSession = new NIRfsg(resourceName, true, true);
         }

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

         /*TDMS File Operation*/
         double t0, dt;
         t0 = 0; dt = 0;
         double t01, dt1, iqrate1, headroom1;
         int eof1;
         //checkWarn(niBTSG_ReadWaveformFromFile(fileName, NULL, 0, -1, NULL, NULL, NULL, 0, &waveformSize, NULL, NULL, NULL));		
         btsgSession.ReadWaveformFromFile(filepath, null, 0, -1, out t01, out dt1, null, 0, out waveformSize, out iqrate1, out headroom1, out eof1);
         dataWaveForm = new ComplexDouble[waveformSize];
         btsgSession.ReadWaveformFromFile(filepath, null, 0, -1, out t0, out dt, dataWaveForm, waveformSize, out actwaveformSize, out iQRate, out actualHeadroom, out eof);

         waveformName = waveNameTextBox.Text;
         rfsgSession.Arb.WriteWaveform(waveformName, dataWaveForm);

         niBTSG.RFSGStoreIQRate(rfsgSession.GetInstrumentHandle().DangerousGetHandle(), null, waveformName, iQRate);
         niBTSG.RFSGStoreHeadroom(rfsgSession.GetInstrumentHandle().DangerousGetHandle(), null, waveformName, actualHeadroom);

         actualHeadroomTextBox.Text = actualHeadroom.ToString();

         /*Configure Script*/
         niBTSG.RFSGConfigureScript(rfsgSession.GetInstrumentHandle().DangerousGetHandle(), null, script, powerLevel);
         rfsgSession.RF.OutputEnabled = true;
         rfsgSession.Initiate();

         /*Start the status checking timer */
         timer.Enabled = true;
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

      private void dataWaveformFileBrowseButton_Click(object sender, System.EventArgs e)
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
            filepath = (string)filePathTextBox.Text;
            resourceName = (string)rfsgResourceTextBox.Text;
            referenceClockSource = refSourceComboBox.SelectedValue.ToString();
            exportClock = clkOutTerminalComboBox.SelectedValue.ToString();
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
            terminalConfiguration = (int)terminalConfigurationComboBox.SelectedValue;
            standard = (int)standardComboBox.SelectedValue;
            // Open the BT Generation Session
            StartGeneration();

            // Start the status checking timer
            timer.Enabled = true;
         }
         catch (System.Exception exception)
         {
            ShowError("StartGeneration()", exception);
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

         // Deactivate 'Stop' button
         stopButton.Enabled = false;

         // Activate 'Start' button
         generateButton.Enabled = true;
      }

      void CloseSession()
      {
         // Close the RFSG session
         try
         {
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
         catch (System.Exception e)
         {
            ShowError("CloseSession()", e);
         }
      }

      void MainFormClosing(object sender, FormClosingEventArgs e)
      {
         StopGeneration();
         CloseSession();
      }

      private void stopButton_Click(object sender, System.EventArgs e)
      {
         //Stop the status checking timer
         timer.Enabled = false;
         if (rfsgSession != null)
         {
            rfsgSession.RF.OutputEnabled = false;
            rfsgSession.Utility.Commit();
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