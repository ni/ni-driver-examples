'==================================================================================================
' Title        : 	Generate Multi Device Interleaved Scanlist
' Description  : This example explains how to generate interleaved scan list for the device NI 2584. 
'The example first checks if the start and end channels specified by the user is valid. The first entry
'of the scan list intermediate entry and last entry for the scan list are made in order to get the final interleaved scan list.
'==================================================================================================
Imports System.Collections.Generic
Imports System.Drawing
Imports System.Windows.Forms
Imports System.Collections
Imports System.Text

Partial Public Class MainForm
    Inherits Form
    Private InterleavedEndChannel2584LookupTable As InterleavedLookupTable()
    Public Sub New()
        InitializeComponent()
        InitializeLookupTable()
    End Sub

    Private Structure InterleavedLookupTable
        Private firstConnection As String
        Private secondConnection As String
        Public Property FirstConnection1 As String
            Get
                Return firstConnection
            End Get
            Set(ByVal value As String)
                firstConnection = value
            End Set
        End Property
        Public Property SecondConnection2 As String
            Get
                Return secondConnection
            End Get
            Set(ByVal value As String)
                secondConnection = value
            End Set
        End Property
    End Structure

    Private Sub InitializeLookupTable()
        Dim lookupStrings As String() = {"ch0->com0", "ch6->com1", "ch1->com0", "ch7->com1", "ch2->com0", "ch8->com1", _
         "ch3->com0", "ch9->com1", "ch4->com0", "ch10->com1", "ch5->com0", "ch11->com1"}
        InterleavedEndChannel2584LookupTable = New InterleavedLookupTable(10) {}

        InterleavedEndChannel2584LookupTable(0).FirstConnection1 = lookupStrings(0)
        InterleavedEndChannel2584LookupTable(0).SecondConnection2 = lookupStrings(1)

        InterleavedEndChannel2584LookupTable(1).FirstConnection1 = lookupStrings(1)
        InterleavedEndChannel2584LookupTable(1).SecondConnection2 = lookupStrings(2)

        InterleavedEndChannel2584LookupTable(2).FirstConnection1 = lookupStrings(2)
        InterleavedEndChannel2584LookupTable(2).SecondConnection2 = lookupStrings(3)

        InterleavedEndChannel2584LookupTable(3).FirstConnection1 = lookupStrings(3)
        InterleavedEndChannel2584LookupTable(3).SecondConnection2 = lookupStrings(4)

        InterleavedEndChannel2584LookupTable(4).FirstConnection1 = lookupStrings(4)
        InterleavedEndChannel2584LookupTable(4).SecondConnection2 = lookupStrings(5)

        InterleavedEndChannel2584LookupTable(5).FirstConnection1 = lookupStrings(5)
        InterleavedEndChannel2584LookupTable(5).SecondConnection2 = lookupStrings(6)

        InterleavedEndChannel2584LookupTable(6).FirstConnection1 = lookupStrings(6)
        InterleavedEndChannel2584LookupTable(6).SecondConnection2 = lookupStrings(7)

        InterleavedEndChannel2584LookupTable(7).FirstConnection1 = lookupStrings(7)
        InterleavedEndChannel2584LookupTable(7).SecondConnection2 = lookupStrings(8)

        InterleavedEndChannel2584LookupTable(8).FirstConnection1 = lookupStrings(8)
        InterleavedEndChannel2584LookupTable(8).SecondConnection2 = lookupStrings(9)

        InterleavedEndChannel2584LookupTable(9).FirstConnection1 = lookupStrings(9)
        InterleavedEndChannel2584LookupTable(9).SecondConnection2 = lookupStrings(10)

        InterleavedEndChannel2584LookupTable(10).FirstConnection1 = lookupStrings(10)
        InterleavedEndChannel2584LookupTable(10).SecondConnection2 = lookupStrings(11)



    End Sub

#Region "Program Properties"
    Private ReadOnly Property InterleavedStartChannel() As Integer
        Get
            Return CInt(Me.interleavedStartChannelNumericUpDown.Value)
        End Get
    End Property

    Private ReadOnly Property InterleavedEndChannel() As Integer
        Get
            Return CInt(Me.interleavedEndChannelNumericUpDown.Value)
        End Get
    End Property



#End Region

    Private Sub generateButton_Click(ByVal sender As Object, ByVal e As EventArgs) Handles generateButton.Click
        Try
            If IsValidValue() Then
                Dim interleavedScanlist As New StringBuilder()
                CreateFirstEntry(interleavedScanlist)
                If InterleavedEndChannel > InterleavedStartChannel Then
                    Dim i As Integer = 0
                    Dim j As Integer
                    Dim k As Integer = InterleavedStartChannel
                    Do
                        j = InterleavedStartChannel + i + 1
                        CreateIntermediateEntry(interleavedScanlist, j, k)
                        k = j
                        i += 1
                    Loop While j <> InterleavedEndChannel
                End If
                CreateLastEntry(interleavedScanlist)
                interleavedScanListRichTextBox.Text = interleavedScanlist.ToString()
            Else
                If Not CheckForNotNull() Then
                    ShowError("The input fields for Interleaved Start or End Channel should not be blank")
                ElseIf Not CheckStartChannelEndChannelRelation() Then
                    ShowError("Interleaved start channel must be smaller or equal to interleaved end channel on all devices.")
                ElseIf Not CheckValidChannelValues() Then
                    ShowError("Valid channel numbers for the PXI-2584 interleaved scanning are between 0 and 10.")
                End If
            End If
        Catch ex As System.Exception
            ShowError(ex.Message)
        End Try
    End Sub
    Private Shared Sub ShowError(ByVal message As String)
        If String.IsNullOrEmpty(message) Then
            message = "Unexpected Error"
        End If
        MessageBox.Show(message, "Error")


    End Sub
    Private Sub CreateIntermediateEntry(ByVal interleavedScanlist As StringBuilder, ByVal j As Integer, ByVal k As Integer)
        interleavedScanlist.Append("~")
        interleavedScanlist.Append(InterleavedEndChannel2584LookupTable(k).FirstConnection1)
        interleavedScanlist.Append(" && ")
        interleavedScanlist.Append(InterleavedEndChannel2584LookupTable(j).SecondConnection2)
        interleavedScanlist.Append(";")
    End Sub

    Private Sub CreateLastEntry(ByVal interleavedScanlist As StringBuilder)
        interleavedScanlist.Append(" ~")
        interleavedScanlist.Append(InterleavedEndChannel2584LookupTable(InterleavedEndChannel).FirstConnection1)
        interleavedScanlist.Append(" &")
        interleavedScanlist.Append(" ~")
        interleavedScanlist.Append(InterleavedEndChannel2584LookupTable(InterleavedEndChannel).SecondConnection2)
        interleavedScanlist.Append(" && ")
    End Sub

    Private Sub CreateFirstEntry(ByVal interleavedScanlist As StringBuilder)
        interleavedScanlist.Append(InterleavedEndChannel2584LookupTable(InterleavedStartChannel).FirstConnection1)
        interleavedScanlist.Append(" & ")
        interleavedScanlist.Append(InterleavedEndChannel2584LookupTable(InterleavedStartChannel).SecondConnection2)
        interleavedScanlist.Append(";")
    End Sub

    Private Function IsValidValue() As Boolean
        Return (CheckForNotNull() AndAlso CheckStartChannelEndChannelRelation() AndAlso CheckValidChannelValues())
    End Function
    Private Function CheckForNotNull() As Boolean

        If ((String.IsNullOrEmpty((Me.interleavedEndChannelNumericUpDown).Text)) Or (String.IsNullOrEmpty((Me.interleavedStartChannelNumericUpDown).Text))) Then

            Return False

        Else
            Return True
        End If

    End Function
    Private Function CheckValidChannelValues() As Boolean
        Return ((InterleavedEndChannel <= 10) AndAlso (InterleavedStartChannel >= 0))
    End Function

    Private Function CheckStartChannelEndChannelRelation() As Boolean
        Return (InterleavedStartChannel <= InterleavedEndChannel)
    End Function


End Class

