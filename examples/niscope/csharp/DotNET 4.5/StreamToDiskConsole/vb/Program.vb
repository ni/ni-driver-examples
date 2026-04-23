'====================================================================================================
'
' Title:
'        Stream To Disk Console
'
' Description:
'      The Application demonstrates the programmatic usage of .Net API for NI-Scope.
'      The application acquires data from a channel and saves it onto a file in the local disk.
'      The waveform stored in the file can be loaded by another shipping example titled "Stream To Disk".
'         
'=================================================================================================

Imports System.IO
Imports System.Runtime.Serialization
Imports System.Runtime.Serialization.Formatters.Binary
Imports System.Security
Imports NationalInstruments.ModularInstruments.NIScope

Class Program
    Shared scopeSession As NIScope

    Friend Shared Sub Main()

        Dim deviceName As String = Nothing, channelList As String = Nothing
        Dim range As Double, sampleRateMin As Double
        Dim recordLengthMin As Long
        Dim outputFileStream As FileStream = Nothing
        Dim waveforms As AnalogWaveformCollection(Of Double) = Nothing
        Dim thrownException As Exception = Nothing

        GetInputParameters(deviceName, channelList, range, sampleRateMin, recordLengthMin, outputFileStream)

        Try
            InitializeSession(deviceName)

            scopeSession.Channels(channelList).Enabled = True
            scopeSession.Channels(channelList).Range = range
            scopeSession.Acquisition.SampleRateMin = sampleRateMin
            scopeSession.Acquisition.NumberOfPointsMin = recordLengthMin

            scopeSession.Measurement.Initiate()
            waveforms = scopeSession.Channels(channelList).Measurement.FetchDouble(PrecisionTimeSpan.Zero, recordLengthMin, waveforms)

            Dim formatter As New BinaryFormatter
            formatter.Serialize(outputFileStream, waveforms)

        Catch ex As SecurityException
            Console.WriteLine("Unable to serialize the data: ")
            thrownException = ex
        Catch ex As SerializationException
            Console.WriteLine("Unable to serialize the data: ")
            thrownException = ex
        Catch ex As Exception
            Console.WriteLine("Error occured in NI-Scope: ")
            thrownException = ex
        Finally
            If scopeSession IsNot Nothing Then
                scopeSession.Close()
                scopeSession = Nothing
            End If
            If outputFileStream IsNot Nothing Then
                outputFileStream.Close()
            End If
            If thrownException IsNot Nothing Then
                Console.WriteLine(thrownException.Message)
            Else
                Console.WriteLine("Successfully saved acquired data to """ & outputFileStream.Name & """")
            End If
            Console.WriteLine("Press any key to exit...")
            Console.ReadKey()
        End Try
    End Sub

    Private Shared Sub InitializeSession(deviceName As String)
        scopeSession = New NIScope(deviceName, False, False)
        AddHandler scopeSession.DriverOperation.Warning, New EventHandler(Of ScopeWarningEventArgs)(AddressOf DriverOperation_Warning)
    End Sub

    Private Shared Sub DriverOperation_Warning(sender As Object, e As ScopeWarningEventArgs)
        Console.WriteLine(e.Text)
    End Sub

    Private Shared Sub GetInputParameters(ByRef deviceName As String, ByRef channelList As String, ByRef range As Double, ByRef SampleRateMin As Double, ByRef recordLengthMin As Long, ByRef outputFileStream As FileStream)
        outputFileStream = Nothing

        Console.WriteLine("Provide input parameter values for the acquisition:")
        Console.WriteLine("Press Enter to accept the default values for the parameters.")
        Console.WriteLine()

        deviceName = GetInputString("Device Name", "Dev1")
        channelList = GetInputString("Channel List", "0")

        While Not Double.TryParse(GetInputString("Range", "10"), range)
            Console.WriteLine("The entered value is not in the correct format. Please try again:")
        End While
        While Not Double.TryParse(GetInputString("Minimum Sample Rate", "1e6"), SampleRateMin)
            Console.WriteLine("The entered value is not in the correct format. Please try again:")
        End While
        While Not Long.TryParse(GetInputString("Minimum Record Length", "1000"), recordLengthMin)
            Console.WriteLine("The entered value is not in the correct format. Please try again:")
        End While
        While outputFileStream Is Nothing
            Try
                ' Folder location.
                Dim directoryPath As String = "C:\waveform"
                If Not Directory.Exists(directoryPath) Then
                    Directory.CreateDirectory(directoryPath)
                End If

                Dim fileName As New FileInfo(Path.Combine(directoryPath, "waveform1.txt"))
                If Not fileName.Exists Then
                    fileName.Create().Close()
                Else
                    File.WriteAllText(fileName.ToString(), [String].Empty)
                End If
                outputFileStream = New FileStream(fileName.ToString(), FileMode.Create, FileAccess.ReadWrite)
            Catch e As Exception
                Console.WriteLine("Unable to create a file stream with the given path:")
                Console.WriteLine(e.Message)
            End Try
        End While
        Console.WriteLine()
        Console.WriteLine()
    End Sub

    Private Shared Function GetInputString(prompt As String, defaultValue As String) As String
        Console.Write(prompt & " [" & defaultValue & "]:")
        Dim inputString As String = Console.ReadLine()

        If inputString = "" Then
            Return defaultValue
        Else
            Return inputString
        End If
    End Function
End Class
