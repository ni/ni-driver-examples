'''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
'
' Example program:
'   Meas2EdgeSeparation
'
' Category:
'   CI
'
' Description:
'   This example demonstrates how to measure two edge separation on a counter
'   input channel. The first edge, second edge, minimum value, and maximum value
'   are all configurable. This example measures two edge separation on the
'   counter's default input terminals (see I/O Connections Overview below for
'   more information), but could easily be expanded to measure two edge
'   separation on any PFI, RTSI, or internal signal. Refer to your device
'   documentation to see if your device supports two edge separation
'   measurements.
'
' Instructions for running:
'   1.  Select the physical channel which corresponds to the counter on the DAQ
'       device you want to perform a two edge separation measurement on.
'   2.  Enter the first edge and second edge corresponding to the two edges you
'       want the counter to 
'       measure.  Enter the maximum and minimum value to specify the range of your
'       unknown two edge separation.  
'       Additionally, you can change the first and second edge input terminals
'       using the channel property node.Note: 
'       It is important to set the maximum and minimum values of your unknown two
'       edge separation as accurately 
'       as possible so the best internal timebase can be chosen to minimize
'       measurement error.  The default values 
'       specify a range that can be measured by the counter using the 20MHz
'       timebase.
'
' Steps:
'   1.  Create a Task.
'   2.  Create a CIChannel object by using the CreateTwoEdgeSeparationChannel
'       method.
'   3.  Create a CounterReader object and use the ReadSingleSampleDouble method
'       to initiate the measurement and return the data.
'   4.  Dispose the Task object to clean-up any resources associated with the
'       task.
'   5.  Handle any DaqExceptions, if they occur.
'
' I/O Connections Overview:
'   This example will perform a measurement on the default terminal(s) of the
'   counter specified. The default counter terminal(s) depend on the type of
'   measurement being taken.  In this example the two edge separation will be
'   measured on the default input terminals on ctr0. For more information on the
'   default counter input and output terminals for your device, open the
'   NI-DAQmx Help, and refer to Counter Signal Connections found under the
'   Device Considerations book in the table of contents.
'
' Microsoft Windows Vista User Account Control
'   Running certain applications on Microsoft Windows Vista requires
'   administrator privileges, 
'   because the application name contains keywords such as setup, update, or
'   install. To avoid this problem, 
'   you must add an additional manifest to the application that specifies the
'   privileges required to run 
'   the application. Some Measurement Studio NI-DAQmx examples for Visual Studio
'   include these keywords. 
'   Therefore, all examples for Visual Studio are shipped with an additional
'   manifest file that you must 
'   embed in the example executable. The manifest file is named
'   [ExampleName].exe.manifest, where [ExampleName] 
'   is the NI-provided example name. For information on how to embed the manifest
'   file, refer to http://msdn2.microsoft.com/en-us/library/bb756929.aspx.Note: 
'   The manifest file is not provided with examples for Visual Studio .NET 2003.
'
'''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

Imports NationalInstruments.DAQmx

Public Class MainFrom
    Inherits System.Windows.Forms.Form

    Private firstEdge As CITwoEdgeSeparationFirstEdge
    Private secondEdge As CITwoEdgeSeparationSecondEdge
    Private myTask As Task
    Private counterInReader As counterReader

#Region " Windows Form Designer generated code "

    Public Sub New()
        MyBase.New()
        Application.EnableVisualStyles()

        'This call is required by the Windows Form Designer.
        InitializeComponent()

        'Add any initialization after the InitializeComponent() call
        firstEdgeComboBox.SelectedIndex = 0
        secondEdgeComboBox.SelectedIndex = 1

        counterComboBox.Items.AddRange(DaqSystem.Local.GetPhysicalChannels(PhysicalChannelTypes.CI, PhysicalChannelAccess.External))
        If (counterComboBox.Items.Count > 0) Then
            counterComboBox.SelectedIndex = 0
        End If

    End Sub

    'Form overrides dispose to clean up the component list.
    Protected Overloads Overrides Sub Dispose(ByVal disposing As Boolean)
        If disposing Then
            If Not (components Is Nothing) Then
                components.Dispose()
            End If
        End If
        MyBase.Dispose(disposing)
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    Friend WithEvents channelParametersGroupBox As System.Windows.Forms.GroupBox
    Friend WithEvents secondEdgeLabel As System.Windows.Forms.Label
    Friend WithEvents secondEdgeComboBox As System.Windows.Forms.ComboBox
    Friend WithEvents firstEdgeLabel As System.Windows.Forms.Label
    Friend WithEvents maximumLabel As System.Windows.Forms.Label
    Friend WithEvents minimumLabel As System.Windows.Forms.Label
    Friend WithEvents physicalChannelLabel As System.Windows.Forms.Label
    Friend WithEvents maximumTextBox As System.Windows.Forms.TextBox
    Friend WithEvents minimumTextBox As System.Windows.Forms.TextBox
    Friend WithEvents firstEdgeComboBox As System.Windows.Forms.ComboBox
    Friend WithEvents acqResultGroupBox As System.Windows.Forms.GroupBox
    Friend WithEvents aquisitionDataTextBox As System.Windows.Forms.TextBox
    Friend WithEvents resultLabel As System.Windows.Forms.Label
    Friend WithEvents measureButton As System.Windows.Forms.Button
    Friend WithEvents counterComboBox As System.Windows.Forms.ComboBox
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Dim resources As System.Resources.ResourceManager = New System.Resources.ResourceManager(GetType(MainFrom))
        Me.channelParametersGroupBox = New System.Windows.Forms.GroupBox
        Me.counterComboBox = New System.Windows.Forms.ComboBox
        Me.secondEdgeLabel = New System.Windows.Forms.Label
        Me.secondEdgeComboBox = New System.Windows.Forms.ComboBox
        Me.firstEdgeLabel = New System.Windows.Forms.Label
        Me.maximumLabel = New System.Windows.Forms.Label
        Me.minimumLabel = New System.Windows.Forms.Label
        Me.physicalChannelLabel = New System.Windows.Forms.Label
        Me.maximumTextBox = New System.Windows.Forms.TextBox
        Me.minimumTextBox = New System.Windows.Forms.TextBox
        Me.firstEdgeComboBox = New System.Windows.Forms.ComboBox
        Me.acqResultGroupBox = New System.Windows.Forms.GroupBox
        Me.aquisitionDataTextBox = New System.Windows.Forms.TextBox
        Me.resultLabel = New System.Windows.Forms.Label
        Me.measureButton = New System.Windows.Forms.Button
        Me.channelParametersGroupBox.SuspendLayout()
        Me.acqResultGroupBox.SuspendLayout()
        Me.SuspendLayout()
        '
        'channelParametersGroupBox
        '
        Me.channelParametersGroupBox.Controls.Add(Me.counterComboBox)
        Me.channelParametersGroupBox.Controls.Add(Me.secondEdgeLabel)
        Me.channelParametersGroupBox.Controls.Add(Me.secondEdgeComboBox)
        Me.channelParametersGroupBox.Controls.Add(Me.firstEdgeLabel)
        Me.channelParametersGroupBox.Controls.Add(Me.maximumLabel)
        Me.channelParametersGroupBox.Controls.Add(Me.minimumLabel)
        Me.channelParametersGroupBox.Controls.Add(Me.physicalChannelLabel)
        Me.channelParametersGroupBox.Controls.Add(Me.maximumTextBox)
        Me.channelParametersGroupBox.Controls.Add(Me.minimumTextBox)
        Me.channelParametersGroupBox.Controls.Add(Me.firstEdgeComboBox)
        Me.channelParametersGroupBox.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.channelParametersGroupBox.Location = New System.Drawing.Point(13, 8)
        Me.channelParametersGroupBox.Name = "channelParametersGroupBox"
        Me.channelParametersGroupBox.Size = New System.Drawing.Size(248, 216)
        Me.channelParametersGroupBox.TabIndex = 1
        Me.channelParametersGroupBox.TabStop = False
        Me.channelParametersGroupBox.Text = "Channel Parameters:"
        '
        'counterComboBox
        '
        Me.counterComboBox.Location = New System.Drawing.Point(136, 24)
        Me.counterComboBox.Name = "counterComboBox"
        Me.counterComboBox.Size = New System.Drawing.Size(96, 21)
        Me.counterComboBox.TabIndex = 1
        Me.counterComboBox.Text = "Dev1/ctr0"
        '
        'secondEdgeLabel
        '
        Me.secondEdgeLabel.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.secondEdgeLabel.Location = New System.Drawing.Point(12, 176)
        Me.secondEdgeLabel.Name = "secondEdgeLabel"
        Me.secondEdgeLabel.Size = New System.Drawing.Size(112, 16)
        Me.secondEdgeLabel.TabIndex = 8
        Me.secondEdgeLabel.Text = "Second Edge:"
        '
        'secondEdgeComboBox
        '
        Me.secondEdgeComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.secondEdgeComboBox.Items.AddRange(New Object() {"Rising", "Falling"})
        Me.secondEdgeComboBox.Location = New System.Drawing.Point(136, 176)
        Me.secondEdgeComboBox.Name = "secondEdgeComboBox"
        Me.secondEdgeComboBox.Size = New System.Drawing.Size(96, 21)
        Me.secondEdgeComboBox.TabIndex = 9
        '
        'firstEdgeLabel
        '
        Me.firstEdgeLabel.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.firstEdgeLabel.Location = New System.Drawing.Point(12, 136)
        Me.firstEdgeLabel.Name = "firstEdgeLabel"
        Me.firstEdgeLabel.Size = New System.Drawing.Size(112, 16)
        Me.firstEdgeLabel.TabIndex = 6
        Me.firstEdgeLabel.Text = "First Edge:"
        '
        'maximumLabel
        '
        Me.maximumLabel.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.maximumLabel.Location = New System.Drawing.Point(12, 96)
        Me.maximumLabel.Name = "maximumLabel"
        Me.maximumLabel.Size = New System.Drawing.Size(120, 16)
        Me.maximumLabel.TabIndex = 4
        Me.maximumLabel.Text = "Maximum Value (sec):"
        '
        'minimumLabel
        '
        Me.minimumLabel.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.minimumLabel.Location = New System.Drawing.Point(12, 62)
        Me.minimumLabel.Name = "minimumLabel"
        Me.minimumLabel.Size = New System.Drawing.Size(120, 18)
        Me.minimumLabel.TabIndex = 2
        Me.minimumLabel.Text = "Minimum Value (sec):"
        '
        'physicalChannelLabel
        '
        Me.physicalChannelLabel.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.physicalChannelLabel.Location = New System.Drawing.Point(12, 26)
        Me.physicalChannelLabel.Name = "physicalChannelLabel"
        Me.physicalChannelLabel.Size = New System.Drawing.Size(96, 16)
        Me.physicalChannelLabel.TabIndex = 0
        Me.physicalChannelLabel.Text = "Physical Channel:"
        '
        'maximumTextBox
        '
        Me.maximumTextBox.Location = New System.Drawing.Point(136, 96)
        Me.maximumTextBox.Name = "maximumTextBox"
        Me.maximumTextBox.Size = New System.Drawing.Size(96, 20)
        Me.maximumTextBox.TabIndex = 5
        Me.maximumTextBox.Text = "0.838860750"
        '
        'minimumTextBox
        '
        Me.minimumTextBox.Location = New System.Drawing.Point(136, 60)
        Me.minimumTextBox.Name = "minimumTextBox"
        Me.minimumTextBox.Size = New System.Drawing.Size(96, 20)
        Me.minimumTextBox.TabIndex = 3
        Me.minimumTextBox.Text = "0.000000100"
        '
        'firstEdgeComboBox
        '
        Me.firstEdgeComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.firstEdgeComboBox.Items.AddRange(New Object() {"Rising", "Falling"})
        Me.firstEdgeComboBox.Location = New System.Drawing.Point(136, 136)
        Me.firstEdgeComboBox.Name = "firstEdgeComboBox"
        Me.firstEdgeComboBox.Size = New System.Drawing.Size(96, 21)
        Me.firstEdgeComboBox.TabIndex = 7
        '
        'acqResultGroupBox
        '
        Me.acqResultGroupBox.Controls.Add(Me.aquisitionDataTextBox)
        Me.acqResultGroupBox.Controls.Add(Me.resultLabel)
        Me.acqResultGroupBox.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.acqResultGroupBox.Location = New System.Drawing.Point(269, 8)
        Me.acqResultGroupBox.Name = "acqResultGroupBox"
        Me.acqResultGroupBox.Size = New System.Drawing.Size(152, 72)
        Me.acqResultGroupBox.TabIndex = 2
        Me.acqResultGroupBox.TabStop = False
        Me.acqResultGroupBox.Text = "Acquisition Results:"
        '
        'aquisitionDataTextBox
        '
        Me.aquisitionDataTextBox.Location = New System.Drawing.Point(16, 40)
        Me.aquisitionDataTextBox.Name = "aquisitionDataTextBox"
        Me.aquisitionDataTextBox.ReadOnly = True
        Me.aquisitionDataTextBox.Size = New System.Drawing.Size(120, 20)
        Me.aquisitionDataTextBox.TabIndex = 1
        Me.aquisitionDataTextBox.Text = "0.0"
        '
        'resultLabel
        '
        Me.resultLabel.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.resultLabel.Location = New System.Drawing.Point(8, 16)
        Me.resultLabel.Name = "resultLabel"
        Me.resultLabel.Size = New System.Drawing.Size(112, 16)
        Me.resultLabel.TabIndex = 0
        Me.resultLabel.Text = "Acquisition Data:"
        '
        'measureButton
        '
        Me.measureButton.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.measureButton.Location = New System.Drawing.Point(280, 184)
        Me.measureButton.Name = "measureButton"
        Me.measureButton.Size = New System.Drawing.Size(128, 32)
        Me.measureButton.TabIndex = 0
        Me.measureButton.Text = "Measure"
        '
        'MainFrom
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
        Me.ClientSize = New System.Drawing.Size(434, 232)
        Me.Controls.Add(Me.channelParametersGroupBox)
        Me.Controls.Add(Me.acqResultGroupBox)
        Me.Controls.Add(Me.measureButton)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MaximizeBox = False
        Me.Name = "MainFrom"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Measure Two Edge Separation"
        Me.channelParametersGroupBox.ResumeLayout(False)
        Me.acqResultGroupBox.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub

#End Region

    Private Sub measureButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles measureButton.Click

        ' This example uses the default source (or gate) terminal for 
        ' the counter of your device.  To determine what the default 
        ' counter pins for your device are or to set a different source 
        ' (or gate) pin, refer to the Connecting Counter Signals topic
        ' in the NI-DAQmx Help (search for "Connecting Counter Signals").

        Try

            measureButton.Enabled = False

            Select Case firstEdgeComboBox.SelectedIndex
                Case 0
                    firstEdge = CITwoEdgeSeparationFirstEdge.Rising
                Case 1
                    firstEdge = CITwoEdgeSeparationFirstEdge.Falling
            End Select

            Select Case secondEdgeComboBox.SelectedIndex
                Case 0
                    secondEdge = CITwoEdgeSeparationSecondEdge.Rising
                Case 1
                    secondEdge = CITwoEdgeSeparationFirstEdge.Falling
            End Select

            myTask = New Task()

            myTask.CIChannels.CreateTwoEdgeSeparationChannel(counterComboBox.Text, _
                "", Convert.ToDouble(minimumTextBox.Text), _
                Convert.ToDouble(maximumTextBox.Text), firstEdge, secondEdge, _
                CITwoEdgeSeparationUnits.Seconds)

            counterInReader = New CounterReader(myTask.Stream)

            Dim data As Double = counterInReader.ReadSingleSampleDouble()

            aquisitionDataTextBox.Text = data.ToString()

        Catch exception As DaqException

            MessageBox.Show(exception.Message)

        Finally

            myTask.Dispose()
            measureButton.Enabled = True

        End Try

    End Sub

End Class
