// ==================================================================================================
//
// Title      : MainForm.cs
// Purpose    : This application illustrates how to use the service request event and
//              the service request status byte to determine when generated data is ready
//              and how to read it.
//
// ==================================================================================================

using System;
using System.Globalization;
using System.Windows.Forms;
using Ivi.Visa;
using NationalInstruments.Visa;

namespace NationalInstruments.Examples.ServiceRequest
{
    /// <summary>
    /// Summary description for Form1.
    /// </summary>
    public class MainForm : System.Windows.Forms.Form
    {
        private IMessageBasedSession _mbSession;
        private System.Windows.Forms.ToolTip _toolTip;
        private System.Windows.Forms.Label _selectResourceLabel;
        private System.Windows.Forms.GroupBox _configuringGroupBox;
        private System.Windows.Forms.GroupBox _writingGroupBox;
        private System.Windows.Forms.GroupBox _readingGroupBox;
        private System.Windows.Forms.Button _clearButton;
        private System.Windows.Forms.Label _resourceNameLabel;
        private System.Windows.Forms.Button _openButton;
        private System.Windows.Forms.Button _closeButton;
        private System.Windows.Forms.TextBox _commandTextBox;
        private System.Windows.Forms.Label _commandLabel;
        private System.Windows.Forms.Button _enableSRQButton;
        private System.Windows.Forms.TextBox _writeTextBox;
        private System.Windows.Forms.Button _writeButton;
        private System.Windows.Forms.TextBox _readTextBox;
        private System.Windows.Forms.ComboBox _resourceNameComboBox;
        private System.ComponentModel.IContainer _components;

        public MainForm()
        {
            //
            // Required for Windows Form Designer support
            //
            InitializeComponent();
            InitializeUI();
            _toolTip.SetToolTip(_enableSRQButton, "Enable the instrument's SRQ event on MAV by sending the following command (varies by instrument):");
            _toolTip.SetToolTip(_writeButton, "Send string to device");
            _toolTip.SetToolTip(_closeButton, "Causes the control to release its handle to the device");
            _toolTip.SetToolTip(_openButton, "The resource name of the device is set and the control attempts to connect to the device");

            try
            {
               // This example uses an instance of the NationalInstruments.Visa.ResourceManager class to find resources on the system.
               // Alternatively, static methods provided by the Ivi.Visa.ResourceManager class may be used when an application
               // requires additional VISA .NET implementations.
               using (var rmSession = new ResourceManager())
                {
                    var validResources = rmSession.Find("(GPIB|TCPIP|USB)?*INSTR");
                    foreach (var resource in validResources)
                    {
                        _resourceNameComboBox.Items.Add(resource);
                    }
                }
            }
            catch (Exception)
            {
                _resourceNameComboBox.Items.Add("No 488.2 INSTR resource found on the system");
                UpdateResourceNameControls(false);
                _closeButton.Enabled = false;
            }
            _resourceNameComboBox.SelectedIndex = 0;
        }

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (_components != null)
                {
                    _components.Dispose();
                }
                if (_mbSession != null)
                {
                    _mbSession.Dispose();
                }
                if (_writingGroupBox != null)
                {
                    _writingGroupBox.Dispose();
                }
                if (_selectResourceLabel != null)
                {
                    _selectResourceLabel.Dispose();
                }
                if (_configuringGroupBox != null)
                {
                    _configuringGroupBox.Dispose();
                }
                if (_closeButton != null)
                {
                    _closeButton.Dispose();
                }
                if (_readingGroupBox != null)
                {
                    _readingGroupBox.Dispose();
                }
                if (_resourceNameComboBox != null)
                {
                    _resourceNameComboBox.Dispose();
                }
                if (_readTextBox != null)
                {
                    _readTextBox.Dispose();
                }
                if (_writeTextBox != null)
                {
                    _writeTextBox.Dispose();
                }
                if (_resourceNameLabel != null)
                {
                    _resourceNameLabel.Dispose();
                }
                if (_openButton != null)
                {
                    _openButton.Dispose();
                }
                if (_clearButton != null)
                {
                    _clearButton.Dispose();
                }
                if (_writeButton != null)
                {
                    _writeButton.Dispose();
                }
                if (_enableSRQButton != null)
                {
                    _enableSRQButton.Dispose();
                }
                if (_commandTextBox != null)
                {
                    _commandTextBox.Dispose();
                }
                if (_toolTip != null)
                {
                    _toolTip.Dispose();
                }
                if (_commandLabel != null)
                {
                    _commandLabel.Dispose();
                }
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code
        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this._components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this._resourceNameLabel = new System.Windows.Forms.Label();
            this._openButton = new System.Windows.Forms.Button();
            this._closeButton = new System.Windows.Forms.Button();
            this._commandLabel = new System.Windows.Forms.Label();
            this._commandTextBox = new System.Windows.Forms.TextBox();
            this._enableSRQButton = new System.Windows.Forms.Button();
            this._configuringGroupBox = new System.Windows.Forms.GroupBox();
            this._resourceNameComboBox = new System.Windows.Forms.ComboBox();
            this._selectResourceLabel = new System.Windows.Forms.Label();
            this._writingGroupBox = new System.Windows.Forms.GroupBox();
            this._writeTextBox = new System.Windows.Forms.TextBox();
            this._writeButton = new System.Windows.Forms.Button();
            this._readingGroupBox = new System.Windows.Forms.GroupBox();
            this._clearButton = new System.Windows.Forms.Button();
            this._readTextBox = new System.Windows.Forms.TextBox();
            this._toolTip = new System.Windows.Forms.ToolTip(this._components);
            this._configuringGroupBox.SuspendLayout();
            this._writingGroupBox.SuspendLayout();
            this._readingGroupBox.SuspendLayout();
            this.SuspendLayout();
            //
            // _resourceNameLabel
            //
            this._resourceNameLabel.Location = new System.Drawing.Point(16, 80);
            this._resourceNameLabel.Name = "_resourceNameLabel";
            this._resourceNameLabel.Size = new System.Drawing.Size(112, 16);
            this._resourceNameLabel.TabIndex = 1;
            this._resourceNameLabel.Text = "Resource Name:";
            //
            // _openButton
            //
            this._openButton.Location = new System.Drawing.Point(16, 128);
            this._openButton.Name = "_openButton";
            this._openButton.Size = new System.Drawing.Size(104, 23);
            this._openButton.TabIndex = 2;
            this._openButton.Text = "Open Session";
            this._openButton.Click += new System.EventHandler(this.OpenButton_Click);
            //
            // _closeButton
            //
            this._closeButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this._closeButton.Location = new System.Drawing.Point(160, 64);
            this._closeButton.Name = "_closeButton";
            this._closeButton.Size = new System.Drawing.Size(104, 23);
            this._closeButton.TabIndex = 3;
            this._closeButton.Text = "Close Session";
            this._closeButton.Click += new System.EventHandler(this.CloseButton_Click);
            //
            // _commandLabel
            //
            this._commandLabel.Location = new System.Drawing.Point(16, 160);
            this._commandLabel.Name = "_commandLabel";
            this._commandLabel.Size = new System.Drawing.Size(256, 32);
            this._commandLabel.TabIndex = 4;
            this._commandLabel.Text = "Type the command to enable the instrument\'s SRQ event on MAV:";
            //
            // _commandTextBox
            //
            this._commandTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this._commandTextBox.Location = new System.Drawing.Point(16, 200);
            this._commandTextBox.Name = "_commandTextBox";
            this._commandTextBox.Size = new System.Drawing.Size(152, 20);
            this._commandTextBox.TabIndex = 5;
            this._commandTextBox.Text = "*SRE 16";
            //
            // _enableSRQButton
            //
            this._enableSRQButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this._enableSRQButton.Location = new System.Drawing.Point(168, 200);
            this._enableSRQButton.Name = "_enableSRQButton";
            this._enableSRQButton.Size = new System.Drawing.Size(104, 24);
            this._enableSRQButton.TabIndex = 6;
            this._enableSRQButton.Text = "Enable SRQ";
            this._enableSRQButton.Click += new System.EventHandler(this.EnableSRQButton_Click);
            //
            // _configuringGroupBox
            //
            this._configuringGroupBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this._configuringGroupBox.Controls.Add(this._resourceNameComboBox);
            this._configuringGroupBox.Controls.Add(this._closeButton);
            this._configuringGroupBox.Location = new System.Drawing.Point(8, 64);
            this._configuringGroupBox.Name = "_configuringGroupBox";
            this._configuringGroupBox.Size = new System.Drawing.Size(272, 168);
            this._configuringGroupBox.TabIndex = 7;
            this._configuringGroupBox.TabStop = false;
            this._configuringGroupBox.Text = "Configuring";
            //
            // _resourceNameComboBox
            //
            this._resourceNameComboBox.FormattingEnabled = true;
            this._resourceNameComboBox.Location = new System.Drawing.Point(11, 35);
            this._resourceNameComboBox.Name = "_resourceNameComboBox";
            this._resourceNameComboBox.Size = new System.Drawing.Size(255, 21);
            this._resourceNameComboBox.TabIndex = 4;
            //
            // _selectResourceLabel
            //
            this._selectResourceLabel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this._selectResourceLabel.Location = new System.Drawing.Point(8, 8);
            this._selectResourceLabel.Name = "_selectResourceLabel";
            this._selectResourceLabel.Size = new System.Drawing.Size(272, 56);
            this._selectResourceLabel.TabIndex = 8;
            this._selectResourceLabel.Text = "Select the Resource Name associated with your device and press the Configure Devi" +
    "ce button. Then enter the command string that enables SRQ and click the Enable S" +
    "RQ button.";
            //
            // _writingGroupBox
            //
            this._writingGroupBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this._writingGroupBox.Controls.Add(this._writeTextBox);
            this._writingGroupBox.Controls.Add(this._writeButton);
            this._writingGroupBox.Location = new System.Drawing.Point(8, 240);
            this._writingGroupBox.Name = "_writingGroupBox";
            this._writingGroupBox.Size = new System.Drawing.Size(272, 56);
            this._writingGroupBox.TabIndex = 9;
            this._writingGroupBox.TabStop = false;
            this._writingGroupBox.Text = "Writing";
            //
            // _writeTextBox
            //
            this._writeTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this._writeTextBox.Location = new System.Drawing.Point(8, 24);
            this._writeTextBox.Name = "_writeTextBox";
            this._writeTextBox.Size = new System.Drawing.Size(152, 20);
            this._writeTextBox.TabIndex = 2;
            this._writeTextBox.Text = "*IDN?\\n";
            //
            // _writeButton
            //
            this._writeButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this._writeButton.Location = new System.Drawing.Point(160, 24);
            this._writeButton.Name = "_writeButton";
            this._writeButton.Size = new System.Drawing.Size(104, 23);
            this._writeButton.TabIndex = 1;
            this._writeButton.Text = "Write";
            this._writeButton.Click += new System.EventHandler(this.WriteButton_Click);
            //
            // _readingGroupBox
            //
            this._readingGroupBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
            | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this._readingGroupBox.Controls.Add(this._clearButton);
            this._readingGroupBox.Controls.Add(this._readTextBox);
            this._readingGroupBox.Location = new System.Drawing.Point(8, 312);
            this._readingGroupBox.Name = "_readingGroupBox";
            this._readingGroupBox.Size = new System.Drawing.Size(272, 120);
            this._readingGroupBox.TabIndex = 10;
            this._readingGroupBox.TabStop = false;
            this._readingGroupBox.Text = "Reading";
            //
            // _clearButton
            //
            this._clearButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this._clearButton.Location = new System.Drawing.Point(8, 88);
            this._clearButton.Name = "_clearButton";
            this._clearButton.Size = new System.Drawing.Size(104, 23);
            this._clearButton.TabIndex = 1;
            this._clearButton.Text = "Clear";
            this._clearButton.Click += new System.EventHandler(this.ClearButton_Click);
            //
            // _readTextBox
            //
            this._readTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
            | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this._readTextBox.Location = new System.Drawing.Point(8, 24);
            this._readTextBox.Multiline = true;
            this._readTextBox.Name = "_readTextBox";
            this._readTextBox.ReadOnly = true;
            this._readTextBox.Size = new System.Drawing.Size(256, 56);
            this._readTextBox.TabIndex = 0;
            //
            // MainForm
            //
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.ClientSize = new System.Drawing.Size(288, 448);
            this.Controls.Add(this._readingGroupBox);
            this.Controls.Add(this._writingGroupBox);
            this.Controls.Add(this._selectResourceLabel);
            this.Controls.Add(this._enableSRQButton);
            this.Controls.Add(this._commandTextBox);
            this.Controls.Add(this._commandLabel);
            this.Controls.Add(this._openButton);
            this.Controls.Add(this._resourceNameLabel);
            this.Controls.Add(this._configuringGroupBox);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon", CultureInfo.CurrentCulture)));
            this.MaximizeBox = false;
            this.MinimumSize = new System.Drawing.Size(296, 482);
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Service Request";
            this._configuringGroupBox.ResumeLayout(false);
            this._writingGroupBox.ResumeLayout(false);
            this._writingGroupBox.PerformLayout();
            this._readingGroupBox.ResumeLayout(false);
            this._readingGroupBox.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();
        }
        #endregion

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        public static void Main()
        {
            Application.Run(new MainForm());
        }

        private void UpdateResourceNameControls(bool enable)
        {
            _resourceNameComboBox.Enabled = enable;
            _openButton.Enabled = enable;
            _closeButton.Enabled = !enable;
            if (enable)
            {
                _openButton.Focus();
            }
        }

        private void UpdateSRQControls(bool enable)
        {
            _commandTextBox.Enabled = enable;
            _enableSRQButton.Enabled = enable;
            if (enable)
            {
                _enableSRQButton.Focus();
            }
        }

        private void UpdateWriteControls(bool enable)
        {
            _writeTextBox.Enabled = enable;
            _writeButton.Enabled = enable;
            if (enable)
            {
                _writeButton.Focus();
            }
        }

        private void InitializeUI()
        {
            UpdateResourceNameControls(true);
            UpdateSRQControls(false);
            UpdateWriteControls(false);
        }

        // When the Open Session button is pressed, the resource name of the
        // device is set and the control attempts to connect to the device
        private void OpenButton_Click(object sender, System.EventArgs e)
        {
            try
            {
                using (var rmSession = new ResourceManager())
                {
                    _mbSession = (MessageBasedSession)rmSession.Open(_resourceNameComboBox.Text);
                    // Use SynchronizeCallbacks to specify that the object marshals callbacks across threads appropriately.
                    _mbSession.SynchronizeCallbacks = true;
                    UpdateResourceNameControls(false);
                    UpdateSRQControls(true);
                }
            }
            catch (Exception exp)
            {
                MessageBox.Show(exp.Message);
            }
        }

        // The Enable SRQ button writes the string that tells the instrument to
        // enable the SRQ bit
        private void EnableSRQButton_Click(object sender, System.EventArgs e)
        {
            try
            { // Registering a handler for an event automatically enables that event.
                _mbSession.ServiceRequest += OnServiceRequest;
                WriteToSession(_commandTextBox.Text);
                UpdateSRQControls(false);
                UpdateWriteControls(true);
            }
            catch (Exception exp)
            {
                MessageBox.Show(exp.Message);
            }
        }

        // Pressing Close Session causes the control to release its handle to the device
        private void CloseButton_Click(object sender, System.EventArgs e)
        {
            _mbSession.ServiceRequest -= OnServiceRequest;
            _mbSession.Dispose();
            InitializeUI();
        }

        // Clicking the Write Button causes the Send String to be written to the device
        private void WriteButton_Click(object sender, System.EventArgs e)
        {
            WriteToSession(_writeTextBox.Text);
        }

        // Pressing the Clear button clears the read textbox
        private void ClearButton_Click(object sender, System.EventArgs e)
        {
            _readTextBox.Clear();
        }

        private string ReplaceCommonEscapeSequences(string s)
        {
            return s.Replace("\\n", "\n").Replace("\\r", "\r");
        }

        private string InsertCommonEscapeSequences(string s)
        {
            return s.Replace("\n", "\\n").Replace("\r", "\\r");
        }

        private void WriteToSession(string textWrite)
        {
            try
            {
                string textToWrite = ReplaceCommonEscapeSequences(textWrite);
                _mbSession.RawIO.Write(textToWrite);
            }
            catch (Exception exp)
            {
                MessageBox.Show(exp.Message);
            }
        }

        private void OnServiceRequest(object sender, VisaEventArgs e)
        {
            try
            {
                var messageBasedSession = (MessageBasedSession)sender;
                StatusByteFlags sb = messageBasedSession.ReadStatusByte();

                if ((sb & StatusByteFlags.MessageAvailable) != 0)
                {
                    string textRead = messageBasedSession.RawIO.ReadString();
                    _readTextBox.Text = InsertCommonEscapeSequences(textRead);
                }
                else
                {
                    MessageBox.Show("MAV in status register is not set, which means that message is not available. Make sure the command to enable SRQ is correct, and the instrument is 488.2 compatible.");
                }
            }
            catch (Exception exp)
            {
                MessageBox.Show(exp.Message);
            }
        }
    }
}