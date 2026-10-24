'==================================================================================================
' Title        : Arbitrary Waveform Streaming
' Description  : This example demonstrates how to generate an arbitrary streaming waveform.
'==================================================================================================

Imports System.Collections.Concurrent
Imports System.Drawing
Imports System.IO
Imports System.IO.MemoryMappedFiles
Imports System.Threading
Imports System.Windows.Forms
Imports NationalInstruments.ModularInstruments.NIRfsg
Imports NationalInstruments.ModularInstruments.SystemServices.DeviceServices

Partial Public Class MainForm
    Inherits Form

    ' Maximum elements in queue
    Const MaximumQueueSize As Integer = 100

    ' Time in milliseconds that the ProducerLoop waits for ConsumerLoop to catch up.
    ' Used when queue is full (MaximumQueueSize items).
    ' Note: If consumerLoop is faster than the producerLoop then streaming cannot be sustained and will result in an RFSG error.
    ' Hence we need to handle only the situation where ProducerLoop is faster.
    Const WaitForConsumerInterval As Integer = 100

    Const StreamingWaveformName As String = "streamingWaveform"
    Const RfsgIDQuery As Boolean = True
    Const RfsgReset As Boolean = False

    Private _rfsgSession As NIRfsg
    Private _readFromStart As Boolean
    Private _elementsRemaining As Integer, _blockSize As Integer, _elementsPerBlock As Integer, _elementsInFile As Integer

    ' Thread safe variables available from .Net 4.0 onwards
    Private _queueHandle As ConcurrentQueue(Of ComplexInt16())
    Dim _running As Boolean, _producerDone As Boolean, _consumerDone As Boolean

    Private _producerBuffer As ComplexInt16(), _consumerBuffer As ComplexInt16()
    Private _memoryMappedFileObject As MemoryMappedFile
    Private _memoryMappedAccessor As MemoryMappedViewAccessor
    Private _fileCurrentIndex As Integer

    ' ThreadSafe method wrapper for errorTextBox control
    Private Delegate Sub SetErrorText(ByVal functionName As String, ByVal exception As Exception)

    Public Sub New()
        InitializeComponent()

        LoadRfsgDeviceNames()
    End Sub

    Private Sub LoadRfsgDeviceNames()
        Dim modularInstrumentsSystem As New ModularInstrumentsSystem("NI-Rfsg")
        For Each device As DeviceInfo In modularInstrumentsSystem.DeviceCollection
            resourceNameComboBox.Items.Add(device.Name)
        Next
        If modularInstrumentsSystem.DeviceCollection.Count > 0 Then
            resourceNameComboBox.SelectedIndex = 0
        End If
    End Sub

#Region "Program Functions"

    Private Sub StartGeneration()
        Dim resourceName As String
        Dim streamingSize As Integer, fileSize As Integer
        Dim frequency As Double, power As Double, iqRate As Double, preFilterGain As Double
        Dim i As Integer, numberOfBlocksInWaveform As Integer, elementsToWrite As Integer
        Dim fileName As String
        Dim fStream As FileStream = Nothing

        Try
            ' We are starting - set GUI ctrls
            generatingLed.BackColor = System.Drawing.SystemColors.Control
            errorTextBox.Text = "No error."
            Application.DoEvents()

            ' Read in all of the control values
            resourceName = resourceNameComboBox.Text
            frequency = CDbl(frequencyNumeric.Value)
            power = CDbl(powerLevelNumeric.Value)
            iqRate = CDbl(iqRateNumeric.Value)
            preFilterGain = CDbl(preGainNumeric.Value)
            streamingSize = CInt(Math.Truncate(streamingSizeNumeric.Value))
            fileName = pathTextBox.Text
            _blockSize = CInt(Math.Truncate(blockSizeNumeric.Value))

            ' Open a session to the device
            _rfsgSession = New NIRfsg(resourceName, RfsgIDQuery, RfsgReset)

            ' Subscribe to Rfsg warnings
            AddHandler _rfsgSession.DriverOperation.Warning, New EventHandler(Of RfsgWarningEventArgs)(AddressOf DriverOperation_Warning)

            ' Configure the generation mode to Arb Waveform
            _rfsgSession.Arb.GenerationMode = RfsgWaveformGenerationMode.ArbitraryWaveform

            ' Configure the frequency and output power level
            _rfsgSession.RF.Configure(frequency, power)

            ' Set the power level type to peak power
            _rfsgSession.RF.PowerLevelType = RfsgRFPowerLevelType.PeakPower

            ' Enable streaming and specify which waveform will be the streaming waveform
            _rfsgSession.Arb.DataTransfer.Streaming.StreamingWaveformName = StreamingWaveformName
            _rfsgSession.Arb.DataTransfer.Streaming.StreamingEnabled = True

            ' Set the IQ rate equal to the IQ rate for the waveform data
            _rfsgSession.Arb.IQRate = iqRate

            ' Set the pre-filter gain in the arb
            _rfsgSession.Arb.PreFilterGain = preFilterGain

            ' Configure the signal bandwidth
            iqRate = _rfsgSession.Arb.IQRate
            actualIQRateTextBox.Text = iqRate.ToString()
            _rfsgSession.Arb.SignalBandwidth = iqRate * 0.8

            ' Open the file that contains the waveform.
            ' The file contains signed binary interleaved IQ Data.
            ' Each IQ sample takes 4 bytes; the first two for the I and the second two for the Q.
            fStream = File.OpenRead(fileName)
            fileSize = CInt(fStream.Length)
            fStream.Close()

            _memoryMappedFileObject = MemoryMappedFile.CreateFromFile(fileName)
            _memoryMappedAccessor = _memoryMappedFileObject.CreateViewAccessor()

            ' Allocate a buffer on the device with the specificed size
            _rfsgSession.Arb.AllocateWaveform(StreamingWaveformName, streamingSize)

            numberOfBlocksInWaveform = CInt(Math.Truncate(Math.Floor(CDbl(streamingSize \ _blockSize))))
            _elementsPerBlock = _blockSize * 2
            _elementsInFile = fileSize \ 2
            _elementsRemaining = _elementsInFile
            _readFromStart = True
            _producerBuffer = New ComplexInt16(_blockSize - 1) {}

            ' Fill the initial buffer allocated buffer with data
            For i = 0 To numberOfBlocksInWaveform - 1
                If _readFromStart Then
                    _fileCurrentIndex = 0
                    _readFromStart = False
                End If

                elementsToWrite = Math.Min(_elementsPerBlock, _elementsRemaining)

                ' If we have reached the end of the file, start from the beginning
                If (_elementsRemaining - elementsToWrite) = 0 Then
                    _elementsRemaining = _elementsInFile
                    _readFromStart = True
                Else
                    _elementsRemaining -= elementsToWrite
                End If

                _memoryMappedAccessor.ReadArray(Of ComplexInt16)(_fileCurrentIndex, _producerBuffer, 0, elementsToWrite \ 2)
                _fileCurrentIndex += elementsToWrite * 2

                ' Write to the Rfsg Arb memory
                _rfsgSession.Arb.WriteWaveform(Of ComplexInt16)(StreamingWaveformName, _producerBuffer)
            Next

            ' Create the queue used by the producer and consumer
            _queueHandle = New ConcurrentQueue(Of ComplexInt16())()

            _consumerBuffer = New ComplexInt16(_blockSize - 1) {}

            _running = True
            _producerDone = False
            _consumerDone = False
            generatingLed.BackColor = Color.Lime

            ' Setup the thread that will handle the file I/O
            Dim sync As New Object()
            ThreadPool.QueueUserWorkItem(AddressOf ProducerLoop, sync)

            ' Wait till the producer populates the queue before invoking consumer
            While _queueHandle.Count < 90
                Thread.Sleep(50)
            End While

            _rfsgSession.Initiate()

            ' Setup the thread that will hand the data from the queue
            ThreadPool.QueueUserWorkItem(AddressOf ConsumerLoop, sync)

            ' Activate stop button
            EnableControls(False)
            stopButton.Focus()
        Catch ex As Exception
            ShowError("StartGeneration()", ex)
        Finally
            If fStream IsNot Nothing Then
                fStream.Close()
            End If
        End Try
    End Sub

    Private Sub ConsumerLoop(ByVal sync As Object)
        Try

            ' Unsubscribe from warning events before going into the loop. This is to make it work similar to CVI example.
            ' Otherwise the example will keep giving warning for a wrong configuration.
            RemoveHandler _rfsgSession.DriverOperation.Warning, AddressOf DriverOperation_Warning

            While (_running) AndAlso (Not _producerDone)
                SyncLock sync
                    ' If there are no items in the queue, wait till the producer populates the queue
                    While _queueHandle.IsEmpty
                        Monitor.Wait(sync)
                    End While
                    ' Write new data there is enough data. The Write function will block until there is enough space available on the AWG.
                    If _queueHandle.Count > 0 Then
                        If _queueHandle.TryDequeue(_consumerBuffer) Then
                            ' Now that the queue isnt full, let the producer run
                            If _queueHandle.Count < 100 Then
                                Monitor.Pulse(sync)
                            End If

                            ' Write the data retrieved from the queue
                            _rfsgSession.Arb.WriteWaveform(Of ComplexInt16)(StreamingWaveformName, _consumerBuffer)
                        End If
                    End If
                    _rfsgSession.CheckGenerationStatus()
                End SyncLock
            End While

            ' Re-Subscribe to Rfsg warnings.
            AddHandler _rfsgSession.DriverOperation.Warning, New EventHandler(Of RfsgWarningEventArgs)(AddressOf DriverOperation_Warning)

            _consumerDone = True
        Catch ex As Exception
            _consumerDone = True
            _running = False
            ShowErrorSafe("ConsumerLoop()", ex)
        End Try
    End Sub

    Private Sub ProducerLoop(ByVal sync As Object)
        Dim elementsToWrite As Integer

        While (_running) AndAlso (Not _consumerDone)
            SyncLock sync
                ' If the queue is getting full, wait for the consumer to dequeue some elements
                While _queueHandle.Count > MaximumQueueSize AndAlso _running AndAlso Not _consumerDone
                    Monitor.Wait(sync, WaitForConsumerInterval)
                End While
            End SyncLock

            If _readFromStart Then
                _fileCurrentIndex = 0
                _readFromStart = False
            End If

            elementsToWrite = Math.Min(_elementsPerBlock, _elementsRemaining)

            If (_elementsRemaining - elementsToWrite) = 0 Then
                _elementsRemaining = _elementsInFile
                _readFromStart = True
            Else
                _elementsRemaining -= elementsToWrite
            End If

            SyncLock sync
                _memoryMappedAccessor.ReadArray(Of ComplexInt16)(_fileCurrentIndex, _producerBuffer, 0, elementsToWrite \ 2)
                _fileCurrentIndex += elementsToWrite * 2
                _queueHandle.Enqueue(_producerBuffer)

                ' Now that queue has some elements, let the consumer read from it
                Monitor.Pulse(sync)
            End SyncLock
        End While
        _producerDone = True
    End Sub

    Private Sub StopGeneration()
        ' Tell the producer to stop
        _running = False

        ' Wait for the threads to finish before closing things out
        If _queueHandle IsNot Nothing Then
            While (Not _producerDone) OrElse (Not _consumerDone)
                Thread.Sleep(50)
            End While
        End If

        Try
            If _rfsgSession IsNot Nothing Then
                ' Abort generation
                _rfsgSession.Abort()

                ' Disable the output.  This sets the noise floor as low as possible.
                _rfsgSession.RF.OutputEnabled = False

                ' Unsubscribe from warning events
                RemoveHandler _rfsgSession.DriverOperation.Warning, AddressOf DriverOperation_Warning

                ' Close the RFSG NIRfsg session
                _rfsgSession.Close()
            End If
            _rfsgSession = Nothing
        Catch ex As Exception
            ShowErrorSafe("StopGeneration", ex)
        End Try

        ' Close the queue
        If _queueHandle IsNot Nothing Then
            _queueHandle = Nothing
        End If

        ' Close the file handle
        If _memoryMappedAccessor IsNot Nothing Then
            _memoryMappedAccessor.Dispose()
        End If
        If _memoryMappedFileObject IsNot Nothing Then
            _memoryMappedFileObject.Dispose()
        End If

        ' Activate all stopped controls
        EnableControls(True)
        generatingLed.BackColor = System.Drawing.SystemColors.Control
    End Sub

    Private Sub ShowErrorSafe(ByVal functionName As String, ByVal exception As Exception)
        If errorTextBox.InvokeRequired Then
            errorTextBox.Invoke(New SetErrorText(AddressOf ShowError), functionName, exception)
        Else
            ShowError(functionName, exception)
        End If
    End Sub

    Private Sub ShowError(ByVal functionName As String, ByVal exception As Exception)
        StopGeneration()

        errorTextBox.Text = "Error in " & functionName & ": " & exception.Message
        EnableControls(True)
    End Sub

#End Region

    Private Sub DriverOperation_Warning(ByVal sender As Object, ByVal e As RfsgWarningEventArgs)
        ' Display the rfsg warning
        errorTextBox.Text = e.Message
    End Sub

#Region "Form Events"

    Private Sub browseButton_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim openFileDialog As New OpenFileDialog()
        openFileDialog.InitialDirectory = Path.GetDirectoryName(Path.GetFullPath(pathTextBox.Text))
        openFileDialog.FileName = Path.GetFileName(pathTextBox.Text)
        openFileDialog.Filter = "Binary Files (*.bin)|*.bin"
        openFileDialog.Title = "Select a waveform file..."
        If openFileDialog.ShowDialog() = DialogResult.OK Then
            pathTextBox.Text = openFileDialog.FileName
        End If
        openFileDialog.Dispose()
    End Sub

    Private Sub startButton_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        StartGeneration()
    End Sub

    Private Sub stopButton_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        StopGeneration()
    End Sub

    Private Sub MainForm_FormClosing(ByVal sender As Object, ByVal e As FormClosingEventArgs)
        StopGeneration()
    End Sub

#End Region

    Private Sub EnableControls(ByVal enabled As Boolean)
        startButton.Enabled = enabled
        stopButton.Enabled = Not enabled
        resourceNameComboBox.Enabled = enabled
        pathTextBox.Enabled = enabled
        frequencyNumeric.Enabled = enabled
        powerLevelNumeric.Enabled = enabled
        iqRateNumeric.Enabled = enabled
        preGainNumeric.Enabled = enabled
        streamingSizeNumeric.Enabled = enabled
        blockSizeNumeric.Enabled = enabled

        Application.DoEvents()
    End Sub
End Class
