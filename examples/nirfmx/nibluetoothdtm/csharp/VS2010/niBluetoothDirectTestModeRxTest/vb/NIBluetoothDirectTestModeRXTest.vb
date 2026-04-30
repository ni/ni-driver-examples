
Imports NationalInstruments.BluetoothDtm

Public Class NIBluetoothDtmRXTest

    Private dtmSession As NIBluetoothDtm
    Private dataBits As UInt16
    Private baudRate As UInt32
    Private flowControl As BluetoothDtmFlowControl
    Private stopBits As BluetoothDtmStopBits
    Private parity As BluetoothDtmParity
    Private status As Integer
    Private channelNumber As Integer
    Private packetCount As Integer
    Private visaResourceName As String

    Public Sub Run()
        Try
            InitializeVariables()
            InitializeDtmSession()
            ConfigureBlueToothDTM()
			' Add your Generation code here
            RetrieveResults()
            PrintResults()
        Catch e As Exception
            DisplayError(e.Message)
        Finally
            CloseSession()
            Console.WriteLine("Press any key to exit")
            Console.ReadKey()
        End Try
    End Sub

    Private Sub InitializeVariables()
        ' Initialize input variables 

        visaResourceName = "Com1"
        dataBits = 8
        baudRate = 115200
        flowControl = BluetoothDtmFlowControl.RtsCts
        stopBits = BluetoothDtmStopBits.StopBits1_0
        parity = BluetoothDtmParity.None
        status = 10
        channelNumber = 3
        packetCount = 0

    End Sub

    Private Sub InitializeDtmSession()
        ' Create a new BluetoothDTM Session 

        dtmSession = New NIBluetoothDtm(visaResourceName)
    End Sub

    Private Sub ConfigureBlueToothDTM()
        dtmSession.ConfigureVisaSerialSettings(dataBits, baudRate, flowControl, stopBits, parity)
        dtmSession.SetVisaTimeout(2000)
        dtmSession.HciReset(status)
        dtmSession.HciLEReceiverTest(channelNumber, status)
    End Sub

    Private Sub RetrieveResults()
        dtmSession.HciLETestEnd(packetCount, status)
        dtmSession.HciReset(status)
    End Sub

    Private Sub PrintResults()
        ' Display PacketCount

        Console.WriteLine("Number of packet count:  {0}" & vbLf, packetCount)
    End Sub



    Private Sub CloseSession()
        If dtmSession IsNot Nothing Then
            Try
                dtmSession.Close()
                dtmSession = Nothing
            Catch e As Exception
                DisplayError(e.Message)
            End Try
        End If
    End Sub

    Private Sub DisplayError(errorMessage As String)
        Console.WriteLine("ERROR: {0}", errorMessage)

    End Sub

End Class
