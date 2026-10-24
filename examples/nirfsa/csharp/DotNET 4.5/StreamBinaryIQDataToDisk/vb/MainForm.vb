'******************************************************************************
'*
'* Example program:
'*   Stream Binary IQ Data To Disk
'*
'* Category:
'*   NI-RFSA
'*
'* Description:
'*   Use this example to learn how to acquire I/Q data using the RF vector
'	signal analyzer and then stream the data to a file. This example shows how
'	to configure NI-RFSA for a continuous I/Q acquisition, how to set the
'	carrier frequency and the I/Q rate, and how to fetch I/Q data in blocks.
'   The data is then stored in the file that you select. This example also
'	demonstrates how to read previously acquired data from the file.
'                         
'
'* Instructions for running:
'*   1. Configure both RFSA device in the MAX for the program to run. 
'*
'*	 2. Configure the Reference Level, Carrier Frequency and the IQ Rate in the UI.
'*   
'*   3. Configure the Number of Samples Per Block and Total Number of Samples in UI.
'*
'*   4. Select the Stream to Disk Button in the UI to start the acquisition and put all the data in the file mentioned in the UI.
'*   
'*   5. The data is displayed in the DataGrid.
'*
'*   6. Select the Stream From Disk Button in the UI to read the data from the file on the disk. 
'*
'* I/O Connections Overview:
'*   Make sure your signal input terminals match the Physical Channel I/O
'*   Controls.  If you have a PXI chassis, ensure that it has been properly
'*   identified in MAX.  
'*
'******************************************************************************
Imports System.Windows.Forms
Imports NationalInstruments.ModularInstruments.NIRfsa
Imports System.IO
Imports System.Runtime.Serialization.Formatters.Binary
Imports NationalInstruments.ModularInstruments.SystemServices.DeviceServices

Partial Public Class MainForm
    Inherits Form
    Private rfsaSession As NIRfsa
    Const headerFileName As String = "header.bin"

    Public Sub New()
        InitializeComponent()
        LoadRfsaDeviceNames()
    End Sub

    Private Sub LoadRfsaDeviceNames()
        Dim modularInstrumentsSystem As New ModularInstrumentsSystem("NI-RFSA")
        For Each device As DeviceInfo In modularInstrumentsSystem.DeviceCollection
            resourceNameComboBox.Items.Add(device.Name)
        Next
        If modularInstrumentsSystem.DeviceCollection.Count > 0 Then
            resourceNameComboBox.SelectedIndex = 0
        End If
    End Sub

    Private ReadOnly Property RFSAResourceName() As String
        Get
            Return Me.resourceNameComboBox.Text
        End Get
    End Property

    Private ReadOnly Property ReferenceLevel() As Double
        Get
            Return Decimal.ToDouble(Me.referenceLevelNumeric.Value)
        End Get
    End Property

    Private ReadOnly Property CarrierFrequency() As Double
        Get
            Return Decimal.ToDouble(Me.carrierFrequencyNumeric.Value)
        End Get
    End Property

    Private ReadOnly Property IQRate() As Double
        Get
            Return Decimal.ToDouble(Me.iqRateNumeric.Value)
        End Get
    End Property

    Private ReadOnly Property NumberOfSamples() As Integer
        Get
            Return Decimal.ToInt32(Me.samplesPerBlockNumeric.Value)
        End Get
    End Property

    Private ReadOnly Property FileName() As String
        Get
            Return fileNameTextBox.Text
        End Get
    End Property

    Private ReadOnly Property MaxSamples() As Integer
        Get
            Return Decimal.ToInt32(Me.maxSamplesNumeric.Value)
        End Get
    End Property

    Private Sub InitializeRfsaSession()
        CloseSession()
        rfsaSession = New NIRfsa(RFSAResourceName, True, False)
        AddHandler rfsaSession.DriverOperation.Warning, New EventHandler(Of RfsaWarningEventArgs)(AddressOf Me.DriverOperationWarning)
    End Sub

    Private Sub DriverOperationWarning(ByVal sender As Object, ByVal e As RfsaWarningEventArgs)
        MessageBox.Show(e.Warning.ToString(), "Warning")
    End Sub

    Private Sub ConfigureForIQ()
        rfsaSession.Configuration.Vertical.ReferenceLevel = ReferenceLevel
        rfsaSession.Configuration.AcquisitionType = RfsaAcquisitionType.IQ
        rfsaSession.Configuration.IQ.CarrierFrequency = CarrierFrequency
        rfsaSession.Configuration.IQ.NumberOfSamples = 1000
        rfsaSession.Configuration.IQ.NumberOfSamplesIsFinite = False
        rfsaSession.Configuration.IQ.IQRate = IQRate
    End Sub

    Private Sub StreamToDisk()
        Dim samplesToRead As Integer = NumberOfSamples
        Dim maxSamplesToSave As Integer = MaxSamples
        Dim sampleSaved As Integer = 0
        Using writestream As New FileStream(FileName, FileMode.Create)
            Using bw As New BinaryWriter(writestream)
                Dim wfInfo As RfsaWaveformInfo
                Dim rawData As ComplexInt16()
                Do
                    If sampleSaved + samplesToRead > maxSamplesToSave Then
                        samplesToRead = maxSamplesToSave - sampleSaved
                    End If

                    Dim IQData As ComplexWaveform(Of ComplexInt16) = FetchBinaryIQdata(samplesToRead, wfInfo)
                    If sampleSaved = 0 Then
                        StreamHeaderToDisk(wfInfo)
                    End If
                    SaveToFile(bw, sampleSaved, IQData, samplesToRead, wfInfo)

                    rawData = IQData.GetRawData()
                    Me.dataGridViewResults.DataSource = rawData
                    Me.samplesSoFarTextBox.Text = sampleSaved.ToString()

                    Me.Refresh()
                Loop While sampleSaved < maxSamplesToSave
            End Using
        End Using
    End Sub

    Private Sub StreamHeaderToDisk(ByVal wfInfo As RfsaWaveformInfo)
        Using writestream As New FileStream(headerFileName, FileMode.Create)
            Using bw As New BinaryWriter(writestream)
                bw.Write(wfInfo.XIncrement)
                bw.Write(wfInfo.Gain)
                bw.Write(wfInfo.Offset)

                Me.dtTextBox.Text = wfInfo.XIncrement.ToString()
                Me.gainTextBox.Text = wfInfo.Gain.ToString()
                Me.offsetTextBox.Text = wfInfo.Offset.ToString()
            End Using
        End Using
    End Sub

    Private Sub InitiateAcquisition()
        rfsaSession.Acquisition.IQ.Initiate()

    End Sub

    Private Function FetchBinaryIQdata(ByVal samplesToRead As Integer, ByRef wfInfo As RfsaWaveformInfo) As ComplexWaveform(Of ComplexInt16)
        Dim timespan As New PrecisionTimeSpan(10.0)
        Return rfsaSession.Acquisition.IQ.FetchIQSingleRecordComplexWaveform(Of ComplexInt16)(0, samplesToRead, timespan, wfInfo)
    End Function

    Private Sub SaveToFile(ByVal bw As BinaryWriter, ByRef sampleSaved As Integer, ByVal IQData As ComplexWaveform(Of ComplexInt16), ByVal sampleToRead As Integer, ByVal wfInfo As RfsaWaveformInfo)
        Dim data As ComplexInt16() = IQData.GetRawData()

        For Each d As ComplexInt16 In data
            bw.Write(d.Real)
            bw.Write(d.Imaginary)
        Next

        sampleSaved += sampleToRead
    End Sub

    Private Sub streamFromDisk()
        Dim samplesPerBlock As Integer = NumberOfSamples
        Dim readstream As FileStream = Nothing
        Dim rb As BinaryReader = Nothing
        Try
            readstream = New FileStream(FileName, FileMode.Open)
            rb = New BinaryReader(readstream)
            ReadWaveformInfo()
            Dim data As ComplexInt16()
            Dim totalRead As Integer = 0
            Dim samplesToRead As Integer = 0

            Dim totalLength As Long = readstream.Length
            Dim totalSamples As Long = totalLength \ 4
            Do
                If totalSamples - totalRead > samplesPerBlock Then
                    samplesToRead = samplesPerBlock
                Else
                    samplesToRead = CInt(totalSamples - totalRead)
                End If

                If totalRead >= totalSamples Then
                    Exit Do
                End If

                data = New ComplexInt16(samplesToRead - 1) {}

                For i As Integer = 0 To samplesToRead - 1
                    data(i).Real = rb.ReadInt16()
                    data(i).Imaginary = rb.ReadInt16()
                Next

                totalRead += samplesToRead
                Me.dataGridViewResults.DataSource = data
                Me.Refresh()
                Me.samplesSoFarTextBox.Text = totalRead.ToString()
            Loop While samplesToRead = samplesPerBlock
        Catch e As Exception
            ShowError(e.Message)
            Return
        Finally
            If rb IsNot Nothing Then
                rb.Close()
            End If
            If readstream IsNot Nothing Then
                readstream.Close()
            End If
        End Try
    End Sub

    Private Sub ReadWaveformInfo()
        Using readstream As New FileStream(headerFileName, FileMode.Open)
            Using rb As New BinaryReader(readstream)
                Me.dtTextBox.Text = rb.ReadDouble().ToString()
                Me.gainTextBox.Text = rb.ReadDouble().ToString()
                Me.offsetTextBox.Text = rb.ReadDouble().ToString()
            End Using
        End Using
    End Sub

    Private Sub CloseSession()
        If rfsaSession IsNot Nothing Then
            Try
                rfsaSession.Close()
                rfsaSession = Nothing
            Catch ex As Exception
                ShowError(("Unable to Close Session, Reset the device.\n Error : ") + ex.Message)
                Application.[Exit]()
            End Try
        End If
    End Sub

    Private Sub streamToDiskButton_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles streamToDiskButton.Click
        Try
            ChangeControlState(False)
            InitializeRfsaSession()
            ConfigureForIQ()
            InitiateAcquisition()
            StreamToDisk()
            CloseSession()
        Catch ex As Exception
            ShowError(ex.Message)
            CloseSession()
        Finally
            ChangeControlState(True)
        End Try
    End Sub

    Private Sub readFromDiskButton_Click(ByVal sender As Object, ByVal e As EventArgs) Handles readFromDiskButton.Click
        ChangeControlState(False)
        streamFromDisk()
        ChangeControlState(True)
    End Sub

    Private Sub ChangeControlState(ByVal isEnabled As Boolean)
        Me.readFromDiskButton.Enabled = isEnabled
        Me.streamToDiskButton.Enabled = isEnabled
        Me.resourceNameComboBox.Enabled = isEnabled
        Me.referenceLevelNumeric.Enabled = isEnabled
        Me.carrierFrequencyNumeric.Enabled = isEnabled
        Me.iqRateNumeric.Enabled = isEnabled
        Me.maxSamplesNumeric.Enabled = isEnabled
        Me.samplesPerBlockNumeric.Enabled = isEnabled
        Me.fileNameTextBox.Enabled = isEnabled
    End Sub

    Private Shared Sub ShowError(ByVal message As String)
        If String.IsNullOrEmpty(message) Then
            message = "Unexpected Error"
        End If
        MessageBox.Show(message, "Error")


    End Sub
End Class


