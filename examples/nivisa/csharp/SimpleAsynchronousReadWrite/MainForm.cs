// ==================================================================================================
//
// Title      : MainForm.cs
// Purpose    : This example demonstrates how to use the NI-VISA .NET Library to
//              perform simple asynchronous operations.  With asynchronous
//              operations, you first call a method to start the operation. You then
//              call a second method to complete the operation.  The first method
//              returns a handle that you can use to check the progress of the operation.
//
// ==================================================================================================

using System;
using System.Globalization;
using System.Windows.Forms;
using Ivi.Visa;
using NationalInstruments.Visa;

namespace NationalInstruments.Examples.SimpleAsynchronousReadWrite
{
    /// <summary>
    /// Summary description for Form1.
    /// </summary>
    public class MainForm : System.Windows.Forms.Form
    {
        private MessageBasedSession _mbSession;
        private string _lastResourceString = null;
        private IVisaAsyncResult _asyncHandle = null;
        private System.Windows.Forms.TextBox _writeTextBox;
        private System.Windows.Forms.TextBox _readTextBox;
        private System.Windows.Forms.Button _writeButton;
        private System.Windows.Forms.Button _readButton;
        private System.Windows.Forms.Button _openSessionButton;
        private System.Windows.Forms.Button _clearButton;
        private System.Windows.Forms.Button _closeSessionButton;
        private System.Windows.Forms.Label _stringToWriteLabel;
        private System.Windows.Forms.Label _stringToReadLabel;
        private System.Windows.Forms.Button _terminateButton;
        private System.Windows.Forms.Label _elementsTransferredLabel;
        private System.Windows.Forms.TextBox _elementsTransferredTextBox;
        private System.Windows.Forms.TextBox _lastIOStatusTextBox;
        private System.Windows.Forms.Label _lastIOStatusLabel;
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.Container _components = null;

        public MainForm()
        {
            //
            // Required for Windows Form Designer support
            //
            InitializeComponent();
            SetupControlState(false);
        }

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (_mbSession != null)
                {
                    _mbSession.Dispose();
                }
                if (_components != null)
                {
                    _components.Dispose();
                }
                if (_writeTextBox != null)
                {
                    _writeTextBox.Dispose();
                }
                if (_readTextBox != null)
                {
                    _readTextBox.Dispose();
                }
                if (_writeButton != null)
                {
                    _writeButton.Dispose();
                }
                if (_readButton != null)
                {
                    _readButton.Dispose();
                }
                if (_openSessionButton != null)
                {
                    _openSessionButton.Dispose();
                }
                if (_clearButton != null)
                {
                    _clearButton.Dispose();
                }
                if (_closeSessionButton != null)
                {
                    _closeSessionButton.Dispose();
                }
                if (_stringToWriteLabel != null)
                {
                    _stringToWriteLabel.Dispose();
                }
                if (_stringToReadLabel != null)
                {
                    _stringToReadLabel.Dispose();
                }
                if (_terminateButton != null)
                {
                    _terminateButton.Dispose();
                }
                if (_elementsTransferredLabel != null)
                {
                    _elementsTransferredLabel.Dispose();
                }
                if (_elementsTransferredTextBox != null)
                {
                    _elementsTransferredTextBox.Dispose();
                }
                if (_lastIOStatusTextBox != null)
                {
                    _lastIOStatusTextBox.Dispose();
                }
                if (_lastIOStatusLabel != null)
                {
                    _lastIOStatusLabel.Dispose();
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this._writeButton = new System.Windows.Forms.Button();
            this._readButton = new System.Windows.Forms.Button();
            this._openSessionButton = new System.Windows.Forms.Button();
            this._writeTextBox = new System.Windows.Forms.TextBox();
            this._readTextBox = new System.Windows.Forms.TextBox();
            this._clearButton = new System.Windows.Forms.Button();
            this._closeSessionButton = new System.Windows.Forms.Button();
            this._stringToWriteLabel = new System.Windows.Forms.Label();
            this._stringToReadLabel = new System.Windows.Forms.Label();
            this._terminateButton = new System.Windows.Forms.Button();
            this._elementsTransferredLabel = new System.Windows.Forms.Label();
            this._elementsTransferredTextBox = new System.Windows.Forms.TextBox();
            this._lastIOStatusTextBox = new System.Windows.Forms.TextBox();
            this._lastIOStatusLabel = new System.Windows.Forms.Label();
            this.SuspendLayout();
            //
            // _writeButton
            //
            this._writeButton.Location = new System.Drawing.Point(5, 83);
            this._writeButton.Name = "_writeButton";
            this._writeButton.Size = new System.Drawing.Size(74, 23);
            this._writeButton.TabIndex = 3;
            this._writeButton.Text = "Write";
            this._writeButton.Click += new System.EventHandler(this.Write_Click);
            //
            // _readButton
            //
            this._readButton.Location = new System.Drawing.Point(79, 83);
            this._readButton.Name = "_readButton";
            this._readButton.Size = new System.Drawing.Size(74, 23);
            this._readButton.TabIndex = 4;
            this._readButton.Text = "Read";
            this._readButton.Click += new System.EventHandler(this.Read_Click);
            //
            // _openSessionButton
            //
            this._openSessionButton.Location = new System.Drawing.Point(5, 5);
            this._openSessionButton.Name = "_openSessionButton";
            this._openSessionButton.Size = new System.Drawing.Size(92, 22);
            this._openSessionButton.TabIndex = 0;
            this._openSessionButton.Text = "Open Session";
            this._openSessionButton.Click += new System.EventHandler(this.OpenSession_Click);
            //
            // _writeTextBox
            //
            this._writeTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this._writeTextBox.Location = new System.Drawing.Point(5, 54);
            this._writeTextBox.Name = "_writeTextBox";
            this._writeTextBox.Size = new System.Drawing.Size(275, 20);
            this._writeTextBox.TabIndex = 2;
            this._writeTextBox.Text = "*IDN?\\n";
            //
            // _readTextBox
            //
            this._readTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
            | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this._readTextBox.Location = new System.Drawing.Point(5, 136);
            this._readTextBox.Multiline = true;
            this._readTextBox.Name = "_readTextBox";
            this._readTextBox.ReadOnly = true;
            this._readTextBox.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this._readTextBox.Size = new System.Drawing.Size(275, 158);
            this._readTextBox.TabIndex = 6;
            this._readTextBox.TabStop = false;
            //
            // _clearButton
            //
            this._clearButton.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this._clearButton.Location = new System.Drawing.Point(6, 344);
            this._clearButton.Name = "_clearButton";
            this._clearButton.Size = new System.Drawing.Size(275, 24);
            this._clearButton.TabIndex = 6;
            this._clearButton.Text = "Clear";
            this._clearButton.Click += new System.EventHandler(this.Clear_Click);
            //
            // _closeSessionButton
            //
            this._closeSessionButton.Location = new System.Drawing.Point(97, 5);
            this._closeSessionButton.Name = "_closeSessionButton";
            this._closeSessionButton.Size = new System.Drawing.Size(92, 22);
            this._closeSessionButton.TabIndex = 1;
            this._closeSessionButton.Text = "Close Session";
            this._closeSessionButton.Click += new System.EventHandler(this.CloseSession_Click);
            //
            // _stringToWriteLabel
            //
            this._stringToWriteLabel.Location = new System.Drawing.Point(5, 40);
            this._stringToWriteLabel.Name = "_stringToWriteLabel";
            this._stringToWriteLabel.Size = new System.Drawing.Size(91, 14);
            this._stringToWriteLabel.TabIndex = 8;
            this._stringToWriteLabel.Text = "String to Write:";
            //
            // _stringToReadLabel
            //
            this._stringToReadLabel.Location = new System.Drawing.Point(5, 122);
            this._stringToReadLabel.Name = "_stringToReadLabel";
            this._stringToReadLabel.Size = new System.Drawing.Size(101, 14);
            this._stringToReadLabel.TabIndex = 9;
            this._stringToReadLabel.Text = "String Read:";
            //
            // _terminateButton
            //
            this._terminateButton.Enabled = false;
            this._terminateButton.Location = new System.Drawing.Point(205, 83);
            this._terminateButton.Name = "_terminateButton";
            this._terminateButton.Size = new System.Drawing.Size(74, 23);
            this._terminateButton.TabIndex = 5;
            this._terminateButton.Text = "Terminate";
            this._terminateButton.Click += new System.EventHandler(this.Terminate_Click);
            //
            // _elementsTransferredLabel
            //
            this._elementsTransferredLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this._elementsTransferredLabel.Location = new System.Drawing.Point(5, 308);
            this._elementsTransferredLabel.Name = "_elementsTransferredLabel";
            this._elementsTransferredLabel.Size = new System.Drawing.Size(116, 11);
            this._elementsTransferredLabel.TabIndex = 11;
            this._elementsTransferredLabel.Text = "Elements Transferred:";
            //
            // _elementsTransferredTextBox
            //
            this._elementsTransferredTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this._elementsTransferredTextBox.Location = new System.Drawing.Point(5, 321);
            this._elementsTransferredTextBox.Name = "_elementsTransferredTextBox";
            this._elementsTransferredTextBox.ReadOnly = true;
            this._elementsTransferredTextBox.Size = new System.Drawing.Size(104, 20);
            this._elementsTransferredTextBox.TabIndex = 12;
            this._elementsTransferredTextBox.TabStop = false;
            //
            // _lastIOStatusTextBox
            //
            this._lastIOStatusTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this._lastIOStatusTextBox.Location = new System.Drawing.Point(113, 321);
            this._lastIOStatusTextBox.Name = "_lastIOStatusTextBox";
            this._lastIOStatusTextBox.ReadOnly = true;
            this._lastIOStatusTextBox.Size = new System.Drawing.Size(168, 20);
            this._lastIOStatusTextBox.TabIndex = 14;
            this._lastIOStatusTextBox.TabStop = false;
            //
            // _lastIOStatusLabel
            //
            this._lastIOStatusLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this._lastIOStatusLabel.Location = new System.Drawing.Point(113, 308);
            this._lastIOStatusLabel.Name = "_lastIOStatusLabel";
            this._lastIOStatusLabel.Size = new System.Drawing.Size(116, 11);
            this._lastIOStatusLabel.TabIndex = 13;
            this._lastIOStatusLabel.Text = "Last I/O Status:";
            //
            // MainForm
            //
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.ClientSize = new System.Drawing.Size(287, 376);
            this.Controls.Add(this._lastIOStatusTextBox);
            this.Controls.Add(this._elementsTransferredTextBox);
            this.Controls.Add(this._readTextBox);
            this.Controls.Add(this._writeTextBox);
            this.Controls.Add(this._lastIOStatusLabel);
            this.Controls.Add(this._elementsTransferredLabel);
            this.Controls.Add(this._terminateButton);
            this.Controls.Add(this._stringToReadLabel);
            this.Controls.Add(this._stringToWriteLabel);
            this.Controls.Add(this._closeSessionButton);
            this.Controls.Add(this._clearButton);
            this.Controls.Add(this._openSessionButton);
            this.Controls.Add(this._readButton);
            this.Controls.Add(this._writeButton);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon", CultureInfo.CurrentCulture)));
            this.MaximizeBox = false;
            this.MinimumSize = new System.Drawing.Size(295, 316);
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Simple Asynchronous Read/Write";
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

        private void OpenSession_Click(object sender, System.EventArgs e)
        {
            using (SelectResource sr = new SelectResource())
            {
                if (_lastResourceString != null)
                {
                    sr.ResourceName = _lastResourceString;
                }
                DialogResult result = sr.ShowDialog(this);
                if (result == DialogResult.OK)
                {
                    _lastResourceString = sr.ResourceName;
                    Cursor.Current = Cursors.WaitCursor;
                    try
                    {
                        using (var rmSession = new ResourceManager())
                        {
                            _mbSession = (MessageBasedSession)rmSession.Open(sr.ResourceName);
                            // Use SynchronizeCallbacks to specify that the object marshals callbacks across threads appropriately.
                            _mbSession.SynchronizeCallbacks = true;
                            SetupControlState(true);
                        }
                    }
                    catch (Exception exp)
                    {
                        MessageBox.Show(exp.Message);
                    }
                    finally
                    {
                        Cursor.Current = Cursors.Default;
                    }
                }
            }
        }

        private void CloseSession_Click(object sender, System.EventArgs e)
        {
            SetupControlState(false);
            _mbSession.Dispose();
        }

        private void Write_Click(object sender, System.EventArgs e)
        {
            try
            {
                SetupWaitingControlState(true);
                string textToWrite = ReplaceCommonEscapeSequences(_writeTextBox.Text);
                _asyncHandle = _mbSession.RawIO.BeginWrite(
                    textToWrite,
                    new VisaAsyncCallback(OnWriteComplete),
                    (object)textToWrite.Length);
            }
            catch (Exception exp)
            {
                MessageBox.Show(exp.Message);
            }
        }

        private void Read_Click(object sender, System.EventArgs e)
        {
            try
            {
                SetupWaitingControlState(true);
                _asyncHandle = _mbSession.RawIO.BeginRead(
                    1024,
                    new VisaAsyncCallback(OnReadComplete),
                    null);
            }
            catch (Exception exp)
            {
                MessageBox.Show(exp.Message);
            }
        }

        private void Clear_Click(object sender, System.EventArgs e)
        {
            ClearControls();
        }

        private void Terminate_Click(object sender, System.EventArgs e)
        {
            SetupWaitingControlState(false);
            try
            {
                _mbSession.RawIO.AbortAsyncOperation(_asyncHandle);
            }
            catch (Exception exp)
            {
                MessageBox.Show(exp.Message);
            }
        }

        private void OnWriteComplete(IVisaAsyncResult result)
        {
            try
            {
                SetupWaitingControlState(false);
                _mbSession.RawIO.EndWrite(result);
                _lastIOStatusTextBox.Text = "Success";
            }
            catch (Exception exp)
            {
                _lastIOStatusTextBox.Text = exp.Message;
            }
            _elementsTransferredTextBox.Text = ((int)result.Count).ToString(CultureInfo.InvariantCulture);
        }

        private void OnReadComplete(IVisaAsyncResult result)
        {
            try
            {
                SetupWaitingControlState(false);
                string responseString = _mbSession.RawIO.EndReadString(result);
                _readTextBox.Text = InsertCommonEscapeSequences(responseString);
                _lastIOStatusTextBox.Text = "Success";
            }
            catch (Exception exp)
            {
                _lastIOStatusTextBox.Text = exp.Message;
            }
            _elementsTransferredTextBox.Text = ((int)result.Count).ToString(CultureInfo.InvariantCulture);
        }

        private void SetupControlState(bool isSessionOpen)
        {
            _openSessionButton.Enabled = !isSessionOpen;
            _closeSessionButton.Enabled = isSessionOpen;
            _writeButton.Enabled = isSessionOpen;
            _readButton.Enabled = isSessionOpen;
            _writeTextBox.Enabled = isSessionOpen;
            _clearButton.Enabled = isSessionOpen;
            if (isSessionOpen)
            {
                ClearControls();
                _writeTextBox.Focus();
            }
        }

        private void SetupWaitingControlState(bool operationIsInProgress)
        {
            if (operationIsInProgress)
            {
                _readTextBox.Text = string.Empty;
                _elementsTransferredTextBox.Text = string.Empty;
                _lastIOStatusTextBox.Text = string.Empty;
            }
            _terminateButton.Enabled = operationIsInProgress;
            _writeButton.Enabled = !operationIsInProgress;
            _readButton.Enabled = !operationIsInProgress;
        }

        private string ReplaceCommonEscapeSequences(string s)
        {
            return (s != null) ? s.Replace("\\n", "\n").Replace("\\r", "\r") : s;
        }

        private string InsertCommonEscapeSequences(string s)
        {
            return (s != null) ? s.Replace("\n", "\\n").Replace("\r", "\\r") : s;
        }

        private void ClearControls()
        {
            _readTextBox.Text = string.Empty;
            _lastIOStatusTextBox.Text = string.Empty;
            _elementsTransferredTextBox.Text = string.Empty;
        }
    }
}