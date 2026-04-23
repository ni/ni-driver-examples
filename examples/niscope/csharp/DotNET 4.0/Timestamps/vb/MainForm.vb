'==================================================================================================
'
' Title:
'      Timestamps
'
' Description:
'      This example demonstrates the timestamping ability of some National Instruments
'      digitizers by creating a histogram of the time between triggers in a multi-record
'      acquisition. It includes code to create a histogram of the time between triggers.
'
'==================================================================================================

Imports System.Windows.Forms
Imports NationalInstruments.ModularInstruments.NIScope
Imports NationalInstruments.ModularInstruments.SystemServices.DeviceServices

Public Partial Class MainForm
	Inherits Form
	Private scopeSession As NIScope

	Public Sub New()
		InitializeComponent()
		ConfiguretriggertypeComboBox()
		ConfiguretriggersourceComboBox()
		LoadScopeDeviceNames()
		ChangeControlState(True)
	End Sub

	#Region "MainForm initial configuration"
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

	Private Sub ConfiguretriggertypeComboBox()
		triggerTypeComboBox.Items.Add(ScopeTriggerType.Edge)
		triggerTypeComboBox.Items.Add(ScopeTriggerType.DigitalEdge)
		triggerTypeComboBox.SelectedIndex = 0
	End Sub

	Private Sub ConfiguretriggersourceComboBox()
		triggerSourceComboBox.Items.Add(ScopeTriggerSource.Channel0)
		triggerSourceComboBox.Items.Add(ScopeTriggerSource.Channel1)
		triggerSourceComboBox.Items.Add(ScopeTriggerSource.Channel2)
		triggerSourceComboBox.Items.Add(ScopeTriggerSource.Channel3)
		triggerSourceComboBox.Items.Add(ScopeTriggerSource.Channel4)
		triggerSourceComboBox.Items.Add(ScopeTriggerSource.Channel5)
		triggerSourceComboBox.Items.Add(ScopeTriggerSource.Channel6)
		triggerSourceComboBox.Items.Add(ScopeTriggerSource.Channel7)
		triggerSourceComboBox.Items.Add(ScopeTriggerSource.External)
		triggerSourceComboBox.Items.Add(ScopeTriggerSource.Rtsi0)
		triggerSourceComboBox.Items.Add(ScopeTriggerSource.Rtsi1)
		triggerSourceComboBox.Items.Add(ScopeTriggerSource.Rtsi2)
		triggerSourceComboBox.Items.Add(ScopeTriggerSource.Rtsi3)
		triggerSourceComboBox.Items.Add(ScopeTriggerSource.Rtsi4)
		triggerSourceComboBox.Items.Add(ScopeTriggerSource.Rtsi5)
		triggerSourceComboBox.Items.Add(ScopeTriggerSource.Rtsi6)
		triggerSourceComboBox.Items.Add(ScopeTriggerSource.Pfi0)
		triggerSourceComboBox.Items.Add(ScopeTriggerSource.Pfi1)
		triggerSourceComboBox.Items.Add(ScopeTriggerSource.Pfi2)
		triggerSourceComboBox.Items.Add(ScopeTriggerSource.PxiStar)
		triggerSourceComboBox.SelectedIndex = 0
	End Sub
	#End Region

	#Region "Mainform configuration values"
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

	Private ReadOnly Property SampleRateMin() As Double
		Get
			Return Decimal.ToDouble(Me.sampleRateMinNumeric.Value)
		End Get
	End Property

	Private ReadOnly Property VerticalRange() As Double
		Get
			Return Decimal.ToDouble(Me.verticalRangeNumeric.Value)
		End Get
	End Property

	Private ReadOnly Property NumberOfRecords() As Integer
		Get
			Return Decimal.ToInt32(Me.numberOfRecordsNumeric.Value)
		End Get
	End Property

	Private ReadOnly Property TriggerType() As ScopeTriggerType
		Get
			Return CType(Me.triggerTypeComboBox.SelectedItem, ScopeTriggerType)
		End Get
	End Property

	Private ReadOnly Property TriggerSource() As ScopeTriggerSource
		Get
			Return DirectCast(Me.triggerSourceComboBox.SelectedItem, ScopeTriggerSource)
		End Get
	End Property

	Private ReadOnly Property HoldOff() As PrecisionTimeSpan
		Get
			Return PrecisionTimeSpan.FromSeconds(Decimal.ToDouble(Me.triggerHoldoffNumeric.Value))
		End Get
	End Property

	Private WriteOnly Property MeanTime() As String
		Set
			Me.meanTimeTextBox.Text = value
		End Set
	End Property

	Private WriteOnly Property StandardDeviation() As String
		Set
			Me.standardDeviationTextBox.Text = value
		End Set
	End Property

	Private WriteOnly Property MeanFrequency() As String
		Set
			Me.meanfrequencyTextBox.Text = value
		End Set
	End Property
	#End Region

	Private Sub acquireButton_Click(sender As Object, e As EventArgs)
		Timestamp()
	End Sub

	Private Sub mainForm_Closing(sender As Object, e As FormClosingEventArgs)
		CloseSession()
	End Sub

	Private Sub InitializeSession()
		scopeSession = New NIScope(ResourceName, False, False)
		AddHandler scopeSession.DriverOperation.Warning, New EventHandler(Of ScopeWarningEventArgs)(AddressOf DriverOperation_Warning)
	End Sub

	Private Sub DriverOperation_Warning(sender As Object, e As ScopeWarningEventArgs)
		MessageBox.Show(e.Text, "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning)
	End Sub

	Private Sub Timestamp()
		ChangeControlState(False)

		Try
			InitializeSession()

			' Configure the vertical parameters.
			Dim verticalOffset As Double = 0.0
            Dim verticalCoupling As ScopeVerticalCoupling = ScopeVerticalCoupling.DC
			Dim probeAttenuation As Double = 1.0
			scopeSession.Channels(ChannelName).Configure(VerticalRange, verticalOffset, verticalCoupling, probeAttenuation, True)

			' Configure the horizontal parameters, with the specified number of records and 1 point only.
			Dim recordLengthMin As Integer = 1
			Dim referencePosition As Double = 50.0
			Dim enforceRealtime As Boolean = True
			scopeSession.Timing.ConfigureTiming(SampleRateMin, recordLengthMin, referencePosition, NumberOfRecords, enforceRealtime)

            ' Configure the trigger.
			Dim triggerLevel As Double = 0.0
			Dim triggerSlope As ScopeTriggerSlope = ScopeTriggerSlope.Positive
            Dim triggerDelay As PrecisionTimeSpan = PrecisionTimeSpan.Zero
            Dim triggerCoupling As ScopeTriggerCoupling = ScopeTriggerCoupling.DC
			Select Case TriggerType
				Case ScopeTriggerType.Edge
                    scopeSession.Trigger.EdgeTrigger.Configure(TriggerSource, triggerLevel, triggerSlope, triggerCoupling, HoldOff, triggerDelay)
					Exit Select

				Case ScopeTriggerType.DigitalEdge
					scopeSession.Trigger.ConfigureTriggerDigital(TriggerSource, triggerSlope, HoldOff, triggerDelay)
					Exit Select
			End Select

			' Fetch only one record at a time.
			scopeSession.Acquisition.NumberOfRecordsToFetch = 1

			scopeSession.Measurement.Initiate()

            Dim waveformInfo As ScopeWaveformInfo() = Nothing
			Dim timeOut As New PrecisionTimeSpan(5.0)
			Dim triggerIntervals As Double() = New Double(NumberOfRecords - 1) {}
			Dim previousTriggerTime As Double = 0.0
			For recordNumber As Integer = 0 To NumberOfRecords - 1
				' Fetch only one record at a time.
				scopeSession.Acquisition.RecordNumberToFetch = recordNumber

				' Fetch the timestamps, no data.
                scopeSession.Channels(ChannelName).Measurement.FetchInt32(timeOut, 0, Nothing, waveformInfo)

				Dim triggerTime As Double = waveformInfo(0).AbsoluteInitialX - waveformInfo(0).RelativeInitialX
				triggerIntervals(recordNumber) = triggerTime - previousTriggerTime
				previousTriggerTime = triggerTime
			Next

			Dim numberOfBins As Integer = 100
			Dim mean As Double, standardDeviation As Double
            Dim histogramXValue As Double() = Nothing
            Dim histogramYValue As Integer() = Nothing
			Histogram(triggerIntervals, numberOfBins, mean, standardDeviation, histogramXValue, histogramYValue)

			DisplayOutput(numberOfBins, histogramXValue, histogramYValue, mean, standardDeviation)
		Catch ex As Exception
			ShowError(ex)
		Finally
			CloseSession()
			ChangeControlState(True)
		End Try
	End Sub

	Private Sub DisplayOutput(numberOfBins As Integer, histogramXValue As Double(), histogramYValue As Integer(), mean As Double, standardDeviation__1 As Double)
		histogramDataGridView.ColumnHeadersVisible = True
		For rowIndex As Integer = 0 To numberOfBins - 1
			histogramDataGridView.Rows.Add()
			histogramDataGridView.Rows(rowIndex).Cells(0).Value = (rowIndex + 1).ToString()
			histogramDataGridView.Rows(rowIndex).Cells(1).Value = histogramXValue(rowIndex).ToString("E")
			histogramDataGridView.Rows(rowIndex).Cells(2).Value = histogramYValue(rowIndex).ToString()
		Next
		MeanTime = mean.ToString("E")
		StandardDeviation = standardDeviation__1.ToString("E")
		MeanFrequency = (1 / mean).ToString("E")
	End Sub

	Private Sub ChangeControlState(isEnabled As Boolean)
		generalGroupBox.Enabled = isEnabled
		acquireButton.Enabled = isEnabled
		triggerGroupBox.Enabled = isEnabled
		If Not isEnabled Then
			histogramDataGridView.Rows.Clear()
			histogramDataGridView.ColumnHeadersVisible = False
		End If
		Me.Refresh()
	End Sub

	Private Sub CloseSession()
		If scopeSession IsNot Nothing Then
			Try
				scopeSession.Close()
				scopeSession = Nothing
			Catch ex As Exception
				ShowError(ex)
				Application.[Exit]()
			End Try
		End If
	End Sub

	Private Sub Histogram(triggerIntervals As Double(), numberOfBins As Integer, ByRef mean As Double, ByRef standardDeviation As Double, ByRef histogramXValue As Double(), ByRef histogramYValue As Integer())
		Dim max As Double = triggerIntervals(1)
		Dim min As Double = triggerIntervals(1)

		Dim counterMean As Double = 0.0
		Dim counterStandardDeviation As Double = 0.0

        ' We ignore the first timestamp because it is not a valid reference.
        For i As Integer = 1 To triggerIntervals.Length - 1
			max = Math.Max(triggerIntervals(i), max)
			min = Math.Min(triggerIntervals(i), min)
			counterMean += triggerIntervals(i)
		Next

		' Calculate the mean -- sum{X[i]}/n
		mean = counterMean / (triggerIntervals.Length - 1)

		' Ignore timestamp 0.
		For i As Integer = 1 To triggerIntervals.Length - 1
			counterStandardDeviation += Math.Pow(triggerIntervals(i) - mean, 2)
		Next

		' Calculate the stdev -- sqrt(sum{ (X[i] - mean)^2 }/n )
		standardDeviation = Math.Sqrt(counterStandardDeviation / (triggerIntervals.Length - 1))

		' Get the width of each bin.
		Dim dx As Double = (max - min) / numberOfBins

		histogramXValue = New Double(numberOfBins - 1) {}
		histogramYValue = New Integer(numberOfBins - 1) {}

        ' Add the counts to each bin according to the timestamps.
        Dim y As Integer
		For i As Integer = 1 To NumberOfRecords - 1
			y = CInt(Math.Truncate((triggerIntervals(i) - min) / dx))
			If y = numberOfBins Then
				y -= 1
			End If
			histogramYValue(y) += 1
		Next

        ' Form a corresponding x axis.
        For i As Integer = 0 To numberOfBins - 1
			histogramXValue(i) = i * dx + min
		Next
	End Sub

	Private Sub ShowError(ex As Exception)
		MessageBox.Show(ex.Message, ex.[GetType]().ToString(), MessageBoxButtons.OK, MessageBoxIcon.[Error])
	End Sub
End Class
