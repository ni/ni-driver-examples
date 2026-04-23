Imports System
Imports NationalInstruments.DAQmx
Imports System.Diagnostics

Namespace NationalInstruments.Examples

    Public Enum WaveformType
        SineWave = 0
    End Enum 'WaveformType

    Public Class FunctionGenerator

        Public Sub New(ByVal desiredFrequency As String, ByVal samplesPerBuffer As String, ByVal cyclesPerBuffer As String, ByVal type As String, ByVal amplitude As String)
            Dim t As New WaveformType

            If type = "Sine" Then
                t = WaveformType.SineWave
            Else
                Debug.Assert(False, "Invalid Waveform Type")
            End If
            Init([Double].Parse(desiredFrequency), [Double].Parse(samplesPerBuffer), [Double].Parse(cyclesPerBuffer), t, [Double].Parse(amplitude))
        End Sub 'New


        Public Sub New(ByVal desiredFrequency As Double, ByVal samplesPerBuffer As Double, ByVal cyclesPerBuffer As Double, ByVal type As WaveformType, ByVal amplitude As Double)
            Init(desiredFrequency, samplesPerBuffer, cyclesPerBuffer, type, amplitude)
        End Sub 'New


        Private Sub Init(ByVal desiredFrequency As Double, ByVal samplesPerBuffer As Double, ByVal cyclesPerBuffer As Double, ByVal type As WaveformType, ByVal amplitude As Double)
            If desiredFrequency <= 0 Then
                Throw New ArgumentOutOfRangeException("desiredFrequency", desiredFrequency, "This parameter must be a positive number")
            End If
            If samplesPerBuffer <= 0 Then
                Throw New ArgumentOutOfRangeException("samplesPerBuffer", samplesPerBuffer, "This parameter must be a positive number")
            End If
            If cyclesPerBuffer <= 0 Then
                Throw New ArgumentOutOfRangeException("cyclesPerBuffer", cyclesPerBuffer, "This parameter must be a positive number")
            End If
            _resultingSampleClockRate = desiredFrequency * samplesPerBuffer / cyclesPerBuffer
            _samplesPerCycle = samplesPerBuffer / cyclesPerBuffer

            ' Determine the actual sample clock rate
            _resultingFrequency = _resultingSampleClockRate / (samplesPerBuffer / cyclesPerBuffer)

            Select Case type
                Case WaveformType.SineWave
                    _data = GenerateSineWave(_resultingFrequency, amplitude, _resultingSampleClockRate, samplesPerBuffer)
                Case Else
                    ' Invalid type value
                    Debug.Assert(False)
            End Select
        End Sub 'Init


        Public ReadOnly Property Data() As Double()
            Get
                Return _data
            End Get
        End Property


        Public Shared Function GenerateSineWave(ByVal frequency As Double, ByVal amplitude As Double, ByVal sampleClockRate As Double, ByVal samplesPerBuffer As Double) As Double()
            Dim deltaT As Double = 1 / sampleClockRate ' sec./samp
            Dim intSamplesPerBuffer As Integer = CInt(samplesPerBuffer)

            Dim rVal(intSamplesPerBuffer - 1) As Double

            Dim i As Integer
            For i = 0 To intSamplesPerBuffer - 1
                rVal(i) = amplitude * Math.Sin((2.0 * Math.PI * frequency * (i * deltaT)))
            Next i
            Return rVal
        End Function 'GenerateSineWave

        Private _data() As Double
        Private _resultingSampleClockRate As Double
        Private _resultingFrequency As Double
        Private _samplesPerCycle As Double
    End Class 'FunctionGenerator
End Namespace
