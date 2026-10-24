Imports System.Collections.Generic
Imports System.ComponentModel
Imports System.Drawing
Imports System.Linq
Imports System.Text
Imports System.Windows.Forms
Imports NationalInstruments

Partial Public Class MultiDataGridView
    Inherits UserControl

    Private dataSource As ComplexDouble(,)
    Private currentIndex As Integer
    Private updateData As Boolean

    Public Sub New()
        InitializeComponent()
    End Sub

    Private Sub BindData(ByVal currentIndex As Integer)

        Me.dataGridView.DataSource = Nothing
        Me.dataGridView.Columns.Clear()
        Dim data As ComplexDouble() = New ComplexDouble(dataSource.GetLength(1) - 1) {}
        For ii As Integer = 0 To dataSource.GetLength(1) - 1
            data(ii) = dataSource(currentIndex, ii)
        Next

        Me.dataGridView.DataSource = data
        Me.dataGridView.Refresh()

    End Sub


    Public Sub SetData(ByVal data As ComplexDouble(,))
        dataSource = data
        currentIndex = 0

        updateData = False
        Me.recordLabel2.Text = [String].Format("of {0}", dataSource.GetLength(0))
        Me.recordNumberNumericUpDown.Enabled = True
        Me.recordNumberNumericUpDown.Value = currentIndex + 1
        Me.recordNumberNumericUpDown.Minimum = currentIndex + 1
        Me.recordNumberNumericUpDown.Maximum = dataSource.GetLength(0)

        updateData = True
        BindData(0)
    End Sub

    Private Sub recordNumberNumericUpDown_ValueChanged(ByVal sender As Object, ByVal e As EventArgs) Handles recordNumberNumericUpDown.ValueChanged
        If updateData Then
            Dim value As Integer = Decimal.ToInt32(recordNumberNumericUpDown.Value)

            If value < 1 OrElse currentIndex > dataSource.GetLength(0) Then
                currentIndex = 1
            Else
                currentIndex = value - 1
            End If

            BindData(currentIndex)
        End If
    End Sub
End Class
