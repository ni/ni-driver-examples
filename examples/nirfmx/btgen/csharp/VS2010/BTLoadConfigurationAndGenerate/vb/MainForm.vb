Imports System
Imports System.Collections.Generic
Imports System.Collections
Imports System.Windows.Forms
Imports NationalInstruments.ModularInstruments.NIRfsg
Imports NationalInstruments.RFToolkits.BT.Generation

Partial Public Class MainForm
   Inherits Form

   Private rfsgSession As NIRfsg
   Private btsgSession As niBTSG
   Private resourceName As String, referenceClockSource As String, exportClock As String, waveformName As String, script As String
   Private filename As String
   Private model As String
   Private powerLevel As Double, carrierFrequency As Double, upConverterCenterFrequency As Double, upConverterCenterFrequencyOffset As Double, actualHeadroom As Double, externalAttenuation As Double,
      iCommonModeOffset As Double, qCommonModeOffset As Double, iOffset As Double, qOffset As Double
   Private channelNumber As Integer, standard As Integer, outputPort As Integer, terminalConfiguration As Integer
   Private generationStatus As RfsgGenerationStatus


   Public Sub New()
      InitializeComponent()
      ConfigureNumericUpDown()
      ConfigureClkOutTerminalComboBox()
      ConfigureRefSourceComboBox()
      ConfigureStandardComboBox()
      ConfigureOutputPortComboBox()
      ConfigureTerminalConfigurationComboBox()
   End Sub

#Region "UI Initial Value Config Section"
   Private Sub ConfigureNumericUpDown()
      chnNumberNumeric.Minimum = 0
      chnNumberNumeric.Maximum = 79
      chnNumberNumeric.Increment = 1
      chnNumberNumeric.Value = 3
      chnNumberNumeric.DecimalPlaces = 0

      powerLevelNumeric.Minimum = Decimal.MinValue
      powerLevelNumeric.Maximum = Decimal.MaxValue
      powerLevelNumeric.Increment = 1
      powerLevelNumeric.Value = 0
      powerLevelNumeric.DecimalPlaces = 2

      externalAttnNumeric.Minimum = Decimal.MinValue
      externalAttnNumeric.Maximum = Decimal.MaxValue
      externalAttnNumeric.Increment = 0.1D
      externalAttnNumeric.Value = 0
      externalAttnNumeric.DecimalPlaces = 2
   End Sub

   Private Sub ConfigureOutputPortComboBox()
      Dim opList As New List(Of DictionaryEntry)()
      opList.Add(New DictionaryEntry("RF Out", RfsgOutputPort.RFOut))
      opList.Add(New DictionaryEntry("IQ Out", RfsgOutputPort.IQOut))
      outputPortComboBox.DataSource = opList
      outputPortComboBox.DisplayMember = "Key"
      outputPortComboBox.ValueMember = "Value"
      outputPortComboBox.SelectedIndex = 0
   End Sub

   Private Sub ConfigureTerminalConfigurationComboBox()
      Dim tcList As New List(Of DictionaryEntry)()
      tcList.Add(New DictionaryEntry("Differential", RfsgTerminalConfiguration.Differential))
      tcList.Add(New DictionaryEntry("SingleEnded", RfsgTerminalConfiguration.SingleEnded))
      terminalConfigurationComboBox.DataSource = tcList
      terminalConfigurationComboBox.DisplayMember = "Key"
      terminalConfigurationComboBox.ValueMember = "Value"
      terminalConfigurationComboBox.SelectedIndex = 0
   End Sub


   Private Sub ConfigureClkOutTerminalComboBox()
      Dim clkOutTerminalValueList As New List(Of DictionaryEntry)()
      clkOutTerminalValueList.Add(New DictionaryEntry("Do not export clock", RfsgOutputTerminal.DoNotExport))
      clkOutTerminalValueList.Add(New DictionaryEntry("RefOut", RfsgOutputTerminal.ReferenceOut))
      clkOutTerminalValueList.Add(New DictionaryEntry("RefOut2", RfsgOutputTerminal.ReferenceOut2))
      clkOutTerminalValueList.Add(New DictionaryEntry("ClkOut", RfsgOutputTerminal.ClockOut))
      clkOutTerminalComboBox.DataSource = clkOutTerminalValueList
      clkOutTerminalComboBox.DisplayMember = "Key"
      clkOutTerminalComboBox.ValueMember = "Value"
      clkOutTerminalComboBox.SelectedIndex = 0
   End Sub

   Private Sub ConfigureStandardComboBox()
      Dim standardValueList As New List(Of DictionaryEntry)()
      standardValueList.Add(New DictionaryEntry("Basic+EDR", niBTSGConstants.StandardBasicEDR))
      standardValueList.Add(New DictionaryEntry("LE", niBTSGConstants.StandardLE))
      standardComboBox.DataSource = standardValueList
      standardComboBox.DisplayMember = "Key"
      standardComboBox.ValueMember = "Value"
      standardComboBox.SelectedIndex = 0
   End Sub


   Private Sub ConfigureRefSourceComboBox()
      Dim refSourceValueList As New List(Of DictionaryEntry)()
      refSourceValueList.Add(New DictionaryEntry("OnBoardClock", RfsgFrequencyReferenceSource.OnboardClock))
      refSourceValueList.Add(New DictionaryEntry("RefIn", RfsgFrequencyReferenceSource.ReferenceIn))
      refSourceValueList.Add(New DictionaryEntry("PXI_CLK", RfsgFrequencyReferenceSource.PxiClock))
      refSourceValueList.Add(New DictionaryEntry("ClkIn", RfsgFrequencyReferenceSource.ClockIn))
      refSourceComboBox.DataSource = refSourceValueList
      refSourceComboBox.DisplayMember = "Key"
      refSourceComboBox.ValueMember = "Value"
      refSourceComboBox.SelectedValue = RfsgFrequencyReferenceSource.PxiClock
   End Sub

#End Region

   Private Sub ProcessTimerEvent(sender As Object, e As System.EventArgs)
      CheckGeneration()
   End Sub

   Private Sub CheckGeneration()
      Try
         generationStatus = RfsgGenerationStatus.InProgress
         ' Check Generation Status
         If rfsgSession IsNot Nothing Then
            generationStatus = rfsgSession.CheckGenerationStatus()
         End If
      Catch exception As System.Exception
         ShowError("CheckGeneration()", exception)
      End Try
   End Sub

   Private Sub ShowError(functionName As String, exception As System.Exception)
      StopGeneration()
      CloseSession()

      ' Display error to the user
      Dim exceptionMessage As String = If(String.IsNullOrEmpty(exception.Message), "Undefined Error.", exception.Message)
      errorTextBox.Text = "Error in " & functionName & System.Environment.NewLine & exceptionMessage
      MessageBox.Show(exceptionMessage, functionName, MessageBoxButtons.OK, MessageBoxIcon.[Error])
   End Sub

   Private Sub StopGeneration()
      'Stop the status checking timer
      timer.Enabled = False
      If rfsgSession IsNot Nothing Then
         rfsgSession.Abort()
         rfsgSession.RF.OutputEnabled = False
         rfsgSession.Utility.Commit()
         Try
            rfsgSession.Abort()
            niBTSG.RFSGClearDatabase(rfsgSession.GetInstrumentHandle().DangerousGetHandle(), Nothing, waveformName)
            ' ignoring the exception here - the RFSGClearDatabase API throws an exception
            ' if called more than once on the same waveformName
         Catch
         End Try
      End If

      ' Deactivate 'Stop' button
      stopButton.Enabled = False

      ' Activate 'Start' button
      generateButton.Enabled = True
   End Sub

   Private Sub MainFormClosing(sender As Object, e As FormClosingEventArgs)
      StopGeneration()
      CloseSession()
   End Sub

   Private Sub CloseSession()
      ' Close the RFSG session
      If rfsgSession IsNot Nothing Then
         rfsgSession.Abort()
         niBTSG.RFSGClearDatabase(rfsgSession.GetInstrumentHandle().DangerousGetHandle(), Nothing, "")
         rfsgSession.Dispose()
         rfsgSession = Nothing
      End If

      ' Close the BT Session
      If btsgSession IsNot Nothing Then
         btsgSession.CloseSession()
         btsgSession = Nothing
      End If
   End Sub

   Private Sub StartGeneration()
      'Bluetooth Session
      If btsgSession Is Nothing Then
         btsgSession = New niBTSG(niBTSGConstants.ToolkitCompatibilityVersion020000)
      End If

      'Bluetooth Properties
      btsgSession.LoadConfigurationFromFile(filename, niBTSGConstants.[True])
      niBTSG.ChannelNumberToCarrierFrequency(channelNumber, standard, carrierFrequency)
      carrierFreqTextBox.Text = carrierFrequency.ToString()
      'Rfsg Session

      If rfsgSession Is Nothing Then
         rfsgSession = New NIRfsg(resourceName, False, True)
      End If

      'get model
      model = rfsgSession.Identity.InstrumentModel

      'RFSG Properties
      rfsgSession.FrequencyReference.Configure(referenceClockSource, 10000000.0)
      rfsgSession.Utility.ExportSignal(RfsgSignalType.ReferenceClock, "", exportClock)
      If model.Equals("NI PXIe-5644R") OrElse model.Equals("NI PXIe-5645R") OrElse model.Equals("NI PXIe-5646R") Then
         If outputPort = RfsgOutputPort.IQOut Then
            rfsgSession.RF.PowerLevelType = RfsgRFPowerLevelType.PeakPower
            rfsgSession.Arb.GenerationMode = RfsgWaveformGenerationMode.Script
            rfsgSession.IQOutPort("I").TerminalConfiguration = CType(terminalConfiguration, RfsgTerminalConfiguration)
            rfsgSession.IQOutPort("I").CommonModeOffset = iCommonModeOffset
            rfsgSession.IQOutPort("I").Offset = iOffset
            rfsgSession.IQOutPort("Q").CommonModeOffset = qCommonModeOffset
            rfsgSession.IQOutPort("Q").Offset = qOffset
            carrierFrequency = 0
         Else
            rfsgSession.RF.Frequency = carrierFrequency
            rfsgSession.RF.PowerLevelType = RfsgRFPowerLevelType.PeakPower
            rfsgSession.Arb.GenerationMode = RfsgWaveformGenerationMode.Script
            upConverterCenterFrequencyOffset = 0
            upConverterCenterFrequency = carrierFrequency + upConverterCenterFrequencyOffset
            rfsgSession.RF.Upconverter.CenterFrequency = upConverterCenterFrequency
         End If
      Else
         rfsgSession.RF.Frequency = carrierFrequency
         rfsgSession.RF.PowerLevelType = RfsgRFPowerLevelType.PeakPower
         rfsgSession.Arb.GenerationMode = RfsgWaveformGenerationMode.Script
         upConverterCenterFrequencyOffset = 0
         upConverterCenterFrequency = carrierFrequency + upConverterCenterFrequencyOffset
         rfsgSession.RF.Upconverter.CenterFrequency = upConverterCenterFrequency
      End If
      rfsgSession.RF.ExternalGain = -(externalAttenuation)

      'Create & Download Waveform
      btsgSession.RFSGCreateAndDownloadWaveform(rfsgSession.GetInstrumentHandle().DangerousGetHandle(), String.Empty, waveformName)
      btsgSession.GetActualHeadroom(Nothing, actualHeadroom)
      actualHeadroomTextBox.Text = actualHeadroom.ToString()

      'Execute Script
      niBTSG.RFSGConfigureScript(rfsgSession.GetInstrumentHandle().DangerousGetHandle(), Nothing, script, powerLevel)
      rfsgSession.RF.OutputEnabled = True
      rfsgSession.Initiate()

      'Start the status checking timer 
      timer.Enabled = True
   End Sub

   Private Sub browseButton_Click(sender As Object, e As System.EventArgs)
      Dim ofg As New OpenFileDialog()
      ofg.Filter = "TDMS Files (*.tdms)|*.tdms|All files (*.*)|*.*"
      If ofg.ShowDialog() = DialogResult.OK Then
         filePathTextBox.Text = ofg.FileName
      End If
   End Sub

   Private Sub generateButton_Click(sender As Object, e As System.EventArgs)
      Try
         'Deactivate the 'Start' button
         generateButton.Enabled = False
         'Activate the 'Stop' button
         stopButton.Enabled = True

         'Reset the error message text box
         errorTextBox.Text = "No Error"
         'Read the Panel control values
         resourceName = DirectCast(rfsgResourceTextBox.Text, String)
         referenceClockSource = CType(refSourceComboBox.SelectedValue, RfsgFrequencyReferenceSource)
         exportClock = CType(clkOutTerminalComboBox.SelectedValue, RfsgOutputTerminal)
         filename = DirectCast(filePathTextBox.Text, String)
         waveformName = DirectCast(waveNameTextBox.Text, String)
         script = DirectCast(scriptTextBox.Text, String)
         carrierFrequency = Double.Parse(carrierFreqTextBox.Text)
         externalAttenuation = CDbl(externalAttnNumeric.Value)
         channelNumber = CInt(Math.Truncate(chnNumberNumeric.Value))
         powerLevel = CDbl(powerLevelNumeric.Value)
         actualHeadroom = System.Convert.ToDouble(actualHeadroomTextBox.Text)
         standard = CInt(standardComboBox.SelectedValue)
         iOffset = CDbl(iOffsetNumeric.Value)
         qOffset = CDbl(qOffsetNumeric.Value)
         iCommonModeOffset = CDbl(iCommonModeOffsetNumeric.Value)
         qCommonModeOffset = CDbl(qCommonModeOffsetNumeric.Value)
         outputPort = CInt(outputPortComboBox.SelectedValue)
         terminalConfiguration = CInt(terminalConfigurationComboBox.SelectedValue)
         ' BT Generation 
         StartGeneration()

         ' Start the status checking timer
         timer.Enabled = True
      Catch exception As System.Exception
         ShowError("StartGeneration()", exception)
      End Try
   End Sub

   Private Sub stopButton_Click(sender As Object, e As System.EventArgs)
      'Stop the status checking timer
      timer.Enabled = False
      If rfsgSession IsNot Nothing Then
         rfsgSession.RF.OutputEnabled = False
         Try
            niBTSG.RFSGClearDatabase(rfsgSession.GetInstrumentHandle().DangerousGetHandle(), Nothing, waveformName)
            ' ignoring the exception here - the RFSGClearDatabase API throws an exception
            ' if called more than once on the same waveformName
         Catch
         End Try
      End If
      ' Deactivate 'Stop' button
      stopButton.Enabled = False

      ' Activate 'Start' button
      generateButton.Enabled = True
   End Sub
End Class
