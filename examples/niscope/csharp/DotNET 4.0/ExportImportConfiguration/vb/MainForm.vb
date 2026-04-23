'==================================================================================================
'
' Title:
'      NI-Scope Export/Import Configuration File
'
' Description:
'      This example demonstrates how to use the Export and Import Attribute Configuration File APIs.
'      Clicking Perform auto-setup will initialize a session; perform auto-setup; and acquire data once.
'
'      Clicking Export will prompt for a file to save the attribute configuration into; initialize a
'      session; configure those attributes to the session; and export those to file.
'
'      Clicking Import will prompt for a file to load the attribute configuration from; initialize a
'      session; and import the attribute configuration into the session.
'      
'      Clicking Acquire will initialize a session; configure that session with the attribute values
'      displayed on the UI; and acquire data once. 
'
'==================================================================================================

Imports System.IO
Imports System.Windows.Forms
Imports NationalInstruments.ModularInstruments.NIScope
Imports NationalInstruments.ModularInstruments.SystemServices.DeviceServices

Partial Public Class MainForm
    Inherits Form
    
    Public Sub New()
        InitializeComponent()
        LoadScopeDeviceNames()
        ConfigureVerticalCouplingComboBox()
        ConfigureInputImpedanceComboBox()
        ChangeControlState(True)
    End Sub

#Region "Mainform Configuration values"
    Private ReadOnly Property ResourceName() As String
        Get
            Return Me.resourceNameComboBox.Text
        End Get
    End Property

    Private ReadOnly Property ChannelName() As String
        Get
            Return Me.channelNameTextBox.Text
        End Get
    End Property

    Private WriteOnly Property RecordLength() As String
        Set(value As String)
            Me.actualRecordLengthTextBox.Text = value
        End Set
    End Property

    Private WriteOnly Property SampleRate() As String
        Set(value As String)
            Me.actualSampleRateTextBox.Text = value
        End Set
    End Property

    Private Property Range() As Double
        Get
            Return Convert.ToDouble(Me.verticalRangeTextBox.Text)
        End Get
        Set(value As Double)
            Me.verticalRangeTextBox.Text = value.ToString()
        End Set
    End Property

    Private Property Offset() As Double
        Get
            Return Convert.ToDouble(Me.verticalOffsetTextBox.Text)
        End Get
        Set(value As Double)
            Me.verticalOffsetTextBox.Text = value.ToString()
        End Set
    End Property

    Private Property Coupling() As ScopeVerticalCoupling
        Get
            Return DirectCast(Me.verticalCouplingComboBox.SelectedItem, ScopeVerticalCoupling)
        End Get
        Set(value As ScopeVerticalCoupling)
            Me.verticalCouplingComboBox.Text = value.ToString()
        End Set
    End Property

    Private Property InputImpedance() As Double
        Get
            Return Convert.ToDouble(Me.inputImpedanceComboBox.SelectedItem)
        End Get
        Set(value As Double)
            Me.inputImpedanceComboBox.Text = value.ToString()
        End Set
    End Property

    Private Property MinSampleRate() As Double
        Get
            Return Convert.ToDouble(Me.minSampleRateTextBox.Text)
        End Get
        Set(value As Double)
            Me.minSampleRateTextBox.Text = value.ToString()
        End Set
    End Property
#End Region

    Private Sub LoadScopeDeviceNames()
        Using scopeDevices As New ModularInstrumentsSystem("NI-Scope")
            For Each device As DeviceInfo In scopeDevices.DeviceCollection
                resourceNameComboBox.Items.Add(device.Name)
            Next
        End Using
        If resourceNameComboBox.Items.Count > 0 Then
            resourceNameComboBox.SelectedIndex = 0
        End If
    End Sub

    Private Sub ConfigureInputImpedanceComboBox()
        inputImpedanceComboBox.Items.Add(50)
        inputImpedanceComboBox.Items.Add(1000000)
        inputImpedanceComboBox.SelectedIndex = 1
    End Sub

    Private Sub ConfigureVerticalCouplingComboBox()
        For Each value As ScopeVerticalCoupling In [Enum].GetValues(GetType(ScopeVerticalCoupling))
            verticalCouplingComboBox.Items.Add(value)
        Next
        verticalCouplingComboBox.SelectedIndex = 1
    End Sub

    Private Sub acquireButton_Click(ByVal sender As Object, ByVal e As EventArgs) Handles acquireButton.Click
        ConfigureAndAcquireData()
    End Sub

    Private Sub AutoSetup_Click(sender As Object, e As EventArgs) Handles performAutoSetupButton.Click
        ChangeControlState(False)

        Try
            Using scopeSession As New NIScope(ResourceName, False, False)
                AddHandler scopeSession.DriverOperation.Warning, New EventHandler(Of ScopeWarningEventArgs)(AddressOf DriverOperation_Warning)

                ' Perform auto-setup on the session
                scopeSession.Measurement.AutoSetup()

                ' Query the properties and display them on the UI
                Query(scopeSession)

                ' Acquire the data and display
                StartAcquisition(scopeSession)
            End Using
            ' Handle driver-specific exceptions before the general exception handler that follows.
            ' An example of a driver-specific exception is Ivi.Driver.IviCDriverException.
        Catch ex As Exception
            ShowError(ex)
        End Try

        ChangeControlState(True)
    End Sub

    Private Sub ConfigureAndAcquireData()
        ChangeControlState(False)

        Try
            Using scopeSession As New NIScope(ResourceName, False, False)
                AddHandler scopeSession.DriverOperation.Warning, New EventHandler(Of ScopeWarningEventArgs)(AddressOf DriverOperation_Warning)

                ' Configure the session
                Configure(scopeSession)

                ' Acquire the data and display the results
                StartAcquisition(scopeSession)
            End Using
            ' Handle driver-specific exceptions before the general exception handler that follows.
            ' An example of a driver-specific exception is Ivi.Driver.IviCDriverException.
        Catch ex As Exception
            ShowError(ex)
        End Try

        ChangeControlState(True)
    End Sub

    Private Sub DriverOperation_Warning(sender As Object, e As ScopeWarningEventArgs)
        MessageBox.Show(e.Text, "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning)
    End Sub

    Private Sub StartAcquisition(ByVal scopeSession As NIScope)
        Dim timeout As New PrecisionTimeSpan(5.0)
        Dim waveforms As AnalogWaveformCollection(Of Double) = Nothing

        Dim recordLength As Long = scopeSession.Acquisition.RecordLength
        Dim sampleRate As Double = scopeSession.Acquisition.SampleRate

        waveforms = scopeSession.Channels(ChannelName).Measurement.Read(timeout, recordLength, waveforms)

        DisplayResults(recordLength, sampleRate)
        PlotWaveforms(sampledDataGridView, waveforms)
    End Sub

    Private Sub ExportConfiguration(ByVal filePath As String)
        Try
            Using scopeSession As New NIScope(ResourceName, False, False)
                AddHandler scopeSession.DriverOperation.Warning, New EventHandler(Of ScopeWarningEventArgs)(AddressOf DriverOperation_Warning)

                ' Configure the session
                Configure(scopeSession)

                ' Export the configuration to a file
                scopeSession.Utility.ExportAttributeConfigurationFile(filePath)
            End Using
            ' Handle driver-specific exceptions before the general exception handler that follows.
            ' An example of a driver-specific exception is Ivi.Driver.IviCDriverException.
        Catch ex As Exception
            ShowError(ex)
        End Try
    End Sub

    Private Sub ImportConfiguration(ByVal filePath As String)
        Try
            Using scopeSession As New NIScope(ResourceName, False, False)
                AddHandler scopeSession.DriverOperation.Warning, New EventHandler(Of ScopeWarningEventArgs)(AddressOf DriverOperation_Warning)

                ' Import the configuration from a file
                scopeSession.Utility.ImportAttributeConfigurationFile(filePath)

                ' Query the properties and display them on the UI
                Query(scopeSession)
            End Using
            ' Handle driver-specific exceptions before the general exception handler that follows.
            ' An example of a driver-specific exception is Ivi.Driver.IviCDriverException.
        Catch ex As Exception
            ShowError(ex)
        End Try
    End Sub

    Private Sub ClearWaveforms()
        sampledDataGridView.Columns.Clear()
    End Sub

    Private Shared Sub PlotWaveforms(dgv As DataGridView, waveforms As AnalogWaveformCollection(Of Double))
        Dim rowIndex As Integer, columnIndex As Integer

        SetupDataGridView(dgv, waveforms.Count)
        For rowIndex = 0 To waveforms(0).SampleCount - 1
            columnIndex = 0
            dgv.Rows.Add()
            dgv.Rows(rowIndex).Cells(columnIndex).Value = (rowIndex + 1).ToString()
            columnIndex += 1
            For Each waveform As AnalogWaveform(Of Double) In waveforms
                dgv.Rows(rowIndex).Cells(columnIndex).Value = waveform.Samples(rowIndex).Value.ToString("E")
                columnIndex += 1
            Next
        Next
    End Sub

    Private Sub DisplayResults(length As Long, rate As Double)
        RecordLength = length.ToString()
        SampleRate = rate.ToString("E")
    End Sub

    Private Shared Sub SetupDataGridView(dgv As DataGridView, numberOfWaveforms As Integer)
        If dgv.ColumnCount > 0 Then
            Return
        End If

        Dim indexColumn As New DataGridViewTextBoxColumn()
        indexColumn.Width = 60
        indexColumn.HeaderText = "Index"
        dgv.Columns.Add(indexColumn)

        For waveformIndex As Integer = 0 To numberOfWaveforms - 1
            Dim waveformColumn As New DataGridViewTextBoxColumn()
            waveformColumn.Width = 125
            waveformColumn.HeaderText = "Waveform " & waveformIndex
            dgv.Columns.Add(waveformColumn)
        Next
    End Sub

    Private Sub ChangeControlState(isEnabled As Boolean)
        acquireButton.Enabled = isEnabled
        resourceNameComboBox.Enabled = isEnabled
        If Not isEnabled Then
            ClearWaveforms()
        End If
        Me.Refresh()
    End Sub

    Private Sub Configure(ByVal scopeSession As NIScope)
        scopeSession.Channels(ChannelName).Range = Range
        scopeSession.Channels(ChannelName).Coupling = Coupling
        scopeSession.Channels(ChannelName).Offset = Offset
        scopeSession.Channels(ChannelName).InputImpedance = InputImpedance
        scopeSession.Acquisition.SampleRateMin = MinSampleRate
    End Sub

    Private Sub Query(ByVal scopeSession As NIScope)
        Range = scopeSession.Channels(ChannelName).Range
        Coupling = scopeSession.Channels(ChannelName).Coupling
        Offset = scopeSession.Channels(ChannelName).Offset
        InputImpedance = scopeSession.Channels(ChannelName).InputImpedance
        MinSampleRate = scopeSession.Acquisition.SampleRateMin
    End Sub

    Private Shared Sub ShowError(ex As Exception)
        MessageBox.Show(ex.Message, ex.[GetType]().ToString(), MessageBoxButtons.OK, MessageBoxIcon.[Error])
    End Sub

    Private Sub Export_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles exportButton.Click
        ChangeControlState(False)

        Dim fileDialog As OpenFileDialog = GetConfigurationFileDialog()
        fileDialog.Title = "Select filename to export configuration to..."

        If fileDialog.ShowDialog() = DialogResult.OK Then
            ExportConfiguration(fileDialog.FileName)
        End If

        ChangeControlState(True)
    End Sub

    Private Sub Import_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles importButton.Click
        ChangeControlState(False)

        Dim fileDialog As OpenFileDialog = GetConfigurationFileDialog()
        fileDialog.Title = "Select filename to import configuration from..."

        If fileDialog.ShowDialog() = DialogResult.OK Then
            ImportConfiguration(fileDialog.FileName)
        End If

        ChangeControlState(True)
    End Sub

    Private Function GetConfigurationFileDialog() As OpenFileDialog
        Dim fileDialog As New OpenFileDialog()
        Dim userDir As String = Environment.GetEnvironmentVariable("userprofile")

        fileDialog.InitialDirectory = Path.Combine(userDir, "Desktop")
        fileDialog.Filter = "NI-Scope configuration files (*.niscopeconfig)|*.niscopeconfig|All files (*.*)|*.*"
        fileDialog.FilterIndex = 1
        fileDialog.RestoreDirectory = True
        fileDialog.CheckFileExists = False

        Return fileDialog
    End Function
End Class
