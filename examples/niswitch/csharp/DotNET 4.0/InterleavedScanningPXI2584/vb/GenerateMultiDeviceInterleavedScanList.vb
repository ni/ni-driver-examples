Imports System.Collections.Generic
Imports System.Linq
Imports System.Text


Public Class GenerateMultiDeviceInterleavedScanList

    Private InterleavedEndChannel2584LookupTable As InterleavedLookupTable()
    Private StartEndRelationError As String, ValidChannelValuesError As String
    Private interleavedStartChannel As Integer, interleavedEndChannel As Integer

    Public Sub InitializeLookupTable()
        Dim lookUpStrings As String() = {"ch0->com0", "ch6->com1", "ch1->com0", "ch7->com1", "ch2->com0", "ch8->com1", _
         "ch3->com0", "ch9->com1", "ch4->com0", "ch10->com1", "ch5->com0", "ch11->com1"}
        InterleavedEndChannel2584LookupTable = New InterleavedLookupTable(10) {}

        InterleavedEndChannel2584LookupTable(0).FirstConnection = lookUpStrings(0)
        InterleavedEndChannel2584LookupTable(0).SecondConnection = lookUpStrings(1)

        InterleavedEndChannel2584LookupTable(1).FirstConnection = lookUpStrings(1)
        InterleavedEndChannel2584LookupTable(1).SecondConnection = lookUpStrings(2)

        InterleavedEndChannel2584LookupTable(2).FirstConnection = lookUpStrings(2)
        InterleavedEndChannel2584LookupTable(2).SecondConnection = lookUpStrings(3)

        InterleavedEndChannel2584LookupTable(3).FirstConnection = lookUpStrings(3)
        InterleavedEndChannel2584LookupTable(3).SecondConnection = lookUpStrings(4)

        InterleavedEndChannel2584LookupTable(4).FirstConnection = lookUpStrings(4)
        InterleavedEndChannel2584LookupTable(4).SecondConnection = lookUpStrings(5)

        InterleavedEndChannel2584LookupTable(5).FirstConnection = lookUpStrings(5)
        InterleavedEndChannel2584LookupTable(5).SecondConnection = lookUpStrings(6)

        InterleavedEndChannel2584LookupTable(6).FirstConnection = lookUpStrings(6)
        InterleavedEndChannel2584LookupTable(6).SecondConnection = lookUpStrings(7)

        InterleavedEndChannel2584LookupTable(7).FirstConnection = lookUpStrings(7)
        InterleavedEndChannel2584LookupTable(7).SecondConnection = lookUpStrings(8)

        InterleavedEndChannel2584LookupTable(8).FirstConnection = lookUpStrings(8)
        InterleavedEndChannel2584LookupTable(8).SecondConnection = lookUpStrings(9)

        InterleavedEndChannel2584LookupTable(9).FirstConnection = lookUpStrings(9)
        InterleavedEndChannel2584LookupTable(9).SecondConnection = lookUpStrings(10)

        InterleavedEndChannel2584LookupTable(10).FirstConnection = lookUpStrings(10)
        InterleavedEndChannel2584LookupTable(10).SecondConnection = lookUpStrings(11)
    End Sub

    Public Function Generate(ByVal startChannel As Integer, ByVal endChannel As Integer) As String
        interleavedStartChannel = startChannel
        interleavedEndChannel = endChannel
        StartEndRelationError = "Interleaved start channel must be smaller or equal to interleaved end channel on all devices."
        ValidChannelValuesError = "Valid channel numbers for the PXI-2584 interleaved scanning are between 0 and 10."

        If IsValidValue() Then
            Dim interleavedScanList As New StringBuilder()
            CreateFirstEntry(interleavedScanList)
            If endChannel > startChannel Then
                Dim i As Integer = 0
                Dim j As Integer
                Dim k As Integer = startChannel
                Do
                    j = startChannel + i + 1
                    CreateIntermediateEntry(interleavedScanList, j, k)
                    k = j
                    i += 1
                Loop While j <> endChannel
            End If
            CreateLastEntry(interleavedScanList)
            Return (interleavedScanList.ToString())
        Else
            If Not CheckStartChannelEndChannelRelation() Then
                Return StartEndRelationError
            ElseIf Not CheckValidChannelValues() Then
                Return ValidChannelValuesError
            Else
                Return Nothing
            End If
        End If
    End Function

    Private Function IsValidValue() As Boolean
        Return (CheckStartChannelEndChannelRelation() AndAlso CheckValidChannelValues())
    End Function
    Private Function CheckValidChannelValues() As Boolean
        Return ((interleavedEndChannel <= 10) AndAlso (interleavedStartChannel >= 0))
    End Function

    Private Function CheckStartChannelEndChannelRelation() As Boolean
        Return (interleavedStartChannel <= interleavedEndChannel)
    End Function


    Private Sub CreateIntermediateEntry(ByVal interleavedScanList As StringBuilder, ByVal j As Integer, ByVal k As Integer)
        interleavedScanList.Append("~")
        interleavedScanList.Append(InterleavedEndChannel2584LookupTable(k).FirstConnection)
        interleavedScanList.Append(" && ")
        interleavedScanList.Append(InterleavedEndChannel2584LookupTable(j).SecondConnection)
        interleavedScanList.Append(";")
    End Sub

    Private Sub CreateLastEntry(ByVal interleavedScanList As StringBuilder)
        interleavedScanList.Append(" ~")
        interleavedScanList.Append(InterleavedEndChannel2584LookupTable(interleavedEndChannel).FirstConnection)
        interleavedScanList.Append(" &")
        interleavedScanList.Append(" ~")
        interleavedScanList.Append(InterleavedEndChannel2584LookupTable(interleavedEndChannel).SecondConnection)
        interleavedScanList.Append(" && ")
    End Sub

    Private Sub CreateFirstEntry(ByVal interleavedScanList As StringBuilder)
        interleavedScanList.Append(InterleavedEndChannel2584LookupTable(interleavedStartChannel).FirstConnection)
        interleavedScanList.Append(" & ")
        interleavedScanList.Append(InterleavedEndChannel2584LookupTable(interleavedStartChannel).SecondConnection)
        interleavedScanList.Append(";")
    End Sub
End Class

Public Structure InterleavedLookupTable
    Private firstConnectionValue As String
    Private secondConnectionValue As String

    Public Property FirstConnection() As String
        Get
            Return firstConnectionValue
        End Get
        Set(ByVal value As String)
            firstConnectionValue = value
        End Set
    End Property

    Public Property SecondConnection() As String
        Get
            Return secondConnectionValue
        End Get
        Set(ByVal value As String)
            secondConnectionValue = value
        End Set
    End Property

End Structure

