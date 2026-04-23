Imports System
Imports NationalInstruments.DAQmx
Imports System.Diagnostics

Public Enum WaveformType
    SineWave = 0
End Enum

Public Class FunctionGenerator
    Public Sub New( _
        ByVal timingSubobject As Timing, _
        ByVal desiredFrequency As String, _
        ByVal samplesPerBuffer As String, _
        ByVal cyclesPerBuffer As String, _
        ByVal type As String, _
        ByVal amplitude As String)

        Dim t As New WaveformType
        t = WaveformType.SineWave
        If (type = "Sine Wave") Then
            t = WaveformType.SineWave
        Else
            Debug.Assert(False, "Invalid Waveform Type")
        End If

        Init( _
            timingSubobject, _
            Double.Parse(desiredFrequency), _
            Double.Parse(samplesPerBuffer), _
            Double.Parse(cyclesPerBuffer), _
            t, _
            Double.Parse(amplitude))
    End Sub

    Public Sub New( _
        ByVal timingSubobject As Timing, _
        ByVal desiredFrequency As Double, _
        ByVal samplesPerBuffer As Double, _
        ByVal cyclesPerBuffer As Double, _
        ByVal type As WaveformType, _
        ByVal amplitude As Double)

        Init( _
            timingSubobject, _
            desiredFrequency, _
            samplesPerBuffer, _
            cyclesPerBuffer, _
            type, _
            amplitude)
    End Sub

    Private Sub Init( _
        ByVal timingSubobject As Timing, _
        ByVal desiredFrequency As Double, _
        ByVal samplesPerBuffer As Double, _
        ByVal cyclesPerBuffer As Double, _
        ByVal type As WaveformType, _
        ByVal amplitude As Double)

        If (desiredFrequency <= 0) Then
            Throw New ArgumentOutOfRangeException("desiredFrequency", desiredFrequency, "This parameter must be a positive number")
        ElseIf (samplesPerBuffer <= 0) Then
            Throw New ArgumentOutOfRangeException("samplesPerBuffer", samplesPerBuffer, "This parameter must be a positive number")
        ElseIf (cyclesPerBuffer <= 0) Then
            Throw New ArgumentOutOfRangeException("cyclesPerBuffer", cyclesPerBuffer, "This parameter must be a positive number")
        End If

        ' First configure the Task timing parameters
        If (timingSubobject.SampleTimingType = SampleTimingType.OnDemand) Then
            timingSubobject.SampleTimingType = SampleTimingType.SampleClock
        End If

        _desiredSampleClockRate = (desiredFrequency * samplesPerBuffer) / cyclesPerBuffer
        _samplesPerCycle = samplesPerBuffer / cyclesPerBuffer

        ' Determine the actual sample clock rate
        timingSubobject.SampleClockRate = _desiredSampleClockRate
        _resultingSampleClockRate = timingSubobject.SampleClockRate

        _resultingFrequency = _resultingSampleClockRate / (samplesPerBuffer / cyclesPerBuffer)

        If (type = WaveformType.SineWave) Then
            _data = GenerateSineWave(_resultingFrequency, amplitude, _resultingSampleClockRate, samplesPerBuffer)
        End If

    End Sub

    Public ReadOnly Property Data() As Double()
        Get
            Return _data
        End Get
    End Property

    Public ReadOnly Property ResultingSampleClockRate() As Double
        Get
            Return _resultingSampleClockRate
        End Get
    End Property

    Public Shared Function GenerateSineWave( _
        ByVal frequency As Double, _
        ByVal amplitude As Double, _
        ByVal sampleClockRate As Double, _
        ByVal samplesPerBuffer As Double) As Double()

        Dim deltaT As Double
        Dim intSamplesPerBuffer As Integer

        deltaT = 1 / sampleClockRate ' sec./samp
        intSamplesPerBuffer = CInt(samplesPerBuffer) - 1

        Dim rVal(intSamplesPerBuffer - 1) As Double

        For i As Integer = 0 To intSamplesPerBuffer - 1
            rVal(i) = amplitude * Math.Sin((2.0 * Math.PI) * frequency * (i * deltaT))
        Next

        Return rVal
    End Function

    Public Shared Sub InitComboBox(ByVal box As System.Windows.Forms.ComboBox)
        Dim obj(0) As Object
        obj(0) = "Sine Wave"

        box.Items.Clear()
        box.Items.AddRange(obj)
        box.Sorted = False
        box.DropDownStyle = ComboBoxStyle.DropDownList
        box.Text = "Sine Wave"
    End Sub

    Private _data As Double()
    Private _resultingSampleClockRate As Double
    Private _resultingFrequency As Double
    Private _desiredSampleClockRate As Double
    Private _samplesPerCycle As Double

End Class
