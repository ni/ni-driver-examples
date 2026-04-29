// ==================================================================================================
//
// Title      : MainForm.cs
// Purpose    : This example demonstrates how to use the NI-VISA .NET Library to
//              perform simple synchronous I/O operations.  You can use this example
//              to send commands, such as "*IDN?\n", to your instrument.
//
// ==================================================================================================

using System;
using System.Globalization;
using System.Windows.Forms;
using NationalInstruments.Visa;

namespace NationalInstruments.Examples.SimpleReadWrite
{
    /// <summary>
    /// Summary description for Form1.
    /// </summary>
    public class MainForm : System.Windows.Forms.Form
    {
        private MessageBasedSession _mbSession;
        private string _lastResourceString = null;
        private System.Windows.Forms.TextBox _writeTextBox;
        private System.Windows.Forms.TextBox _readTextBox;
        private System.Windows.Forms.Button _queryButton;
        private System.Windows.Forms.Button _writeButton;
        private System.Windows.Forms.Button _readButton;
        private System.Windows.Forms.Button _openSessionButton;
        private System.Windows.Forms.Button _clearButton;
        private System.Windows.Forms.Button _closeSessionButton;
        private System.Windows.Forms.Label _stringToWriteLabel;
        private System.Windows.Forms.Label _stringToReadLabel;
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
                if (_queryButton != null)
                {
                    _queryButton.Dispose();
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
            this._queryButton = new System.Windows.Forms.Button();
            this._writeButton = new System.Windows.Forms.Button();
            this._readButton = new System.Windows.Forms.Button();
            this._openSessionButton = new System.Windows.Forms.Button();
            this._writeTextBox = new System.Windows.Forms.TextBox();
            this._readTextBox = new System.Windows.Forms.TextBox();
            this._clearButton = new System.Windows.Forms.Button();
            this._closeSessionButton = new System.Windows.Forms.Button();
            this._stringToWriteLabel = new System.Windows.Forms.Label();
            this._stringToReadLabel = new System.Windows.Forms.Label();
            this.SuspendLayout();
            //
            // _queryButton
            //
            this._queryButton.Location = new System.Drawing.Point(5, 83);
            this._queryButton.Name = "_queryButton";
            this._queryButton.Size = new System.Drawing.Size(74, 23);
            this._queryButton.TabIndex = 3;
            this._queryButton.Text = "Query";
            this._queryButton.Click += new System.EventHandler(this.Query_Click);
            //
            // _writeButton
            //
            this._writeButton.Location = new System.Drawing.Point(79, 83);
            this._writeButton.Name = "_writeButton";
            this._writeButton.Size = new System.Drawing.Size(74, 23);
            this._writeButton.TabIndex = 4;
            this._writeButton.Text = "Write";
            this._writeButton.Click += new System.EventHandler(this.Write_Click);
            //
            // _readButton
            //
            this._readButton.Location = new System.Drawing.Point(153, 83);
            this._readButton.Name = "_readButton";
            this._readButton.Size = new System.Drawing.Size(74, 23);
            this._readButton.TabIndex = 5;
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
            this._readTextBox.Size = new System.Drawing.Size(275, 119);
            this._readTextBox.TabIndex = 6;
            this._readTextBox.TabStop = false;
            //
            // _clearButton
            //
            this._clearButton.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this._clearButton.Location = new System.Drawing.Point(6, 257);
            this._clearButton.Name = "_clearButton";
            this._clearButton.Size = new System.Drawing.Size(275, 24);
            this._clearButton.TabIndex = 7;
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
            this._stringToWriteLabel.Location = new System.Drawing.Point(5, 34);
            this._stringToWriteLabel.Name = "_stringToWriteLabel";
            this._stringToWriteLabel.Size = new System.Drawing.Size(91, 14);
            this._stringToWriteLabel.TabIndex = 8;
            this._stringToWriteLabel.Text = "String to Write:";
            //
            // _stringToReadLabel
            //
            this._stringToReadLabel.Location = new System.Drawing.Point(5, 117);
            this._stringToReadLabel.Name = "_stringToReadLabel";
            this._stringToReadLabel.Size = new System.Drawing.Size(101, 15);
            this._stringToReadLabel.TabIndex = 9;
            this._stringToReadLabel.Text = "String Read:";
            //
            // MainForm
            //
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.ClientSize = new System.Drawing.Size(287, 289);
            this.Controls.Add(this._stringToReadLabel);
            this.Controls.Add(this._stringToWriteLabel);
            this.Controls.Add(this._closeSessionButton);
            this.Controls.Add(this._clearButton);
            this.Controls.Add(this._readTextBox);
            this.Controls.Add(this._writeTextBox);
            this.Controls.Add(this._openSessionButton);
            this.Controls.Add(this._readButton);
            this.Controls.Add(this._writeButton);
            this.Controls.Add(this._queryButton);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon", CultureInfo.CurrentCulture)));
            this.MaximizeBox = false;
            this.MinimumSize = new System.Drawing.Size(295, 316);
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Simple Read/Write";
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
                    using (var rmSession = new ResourceManager())
                    {
                        try
                        {
                            _mbSession = (MessageBasedSession)rmSession.Open(sr.ResourceName);
                            SetupControlState(true);
                        }
                        catch (InvalidCastException)
                        {
                            MessageBox.Show("Resource selected must be a message-based session");
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
        }

        private void CloseSession_Click(object sender, System.EventArgs e)
        {
            SetupControlState(false);
            _mbSession.Dispose();
        }

        private void Query_Click(object sender, System.EventArgs e)
        {
            _readTextBox.Text = string.Empty;
            Cursor.Current = Cursors.WaitCursor;
            try
            {
                string textToWrite = ReplaceCommonEscapeSequences(_writeTextBox.Text);
                _mbSession.RawIO.Write(textToWrite);
                _readTextBox.Text = InsertCommonEscapeSequences(_mbSession.RawIO.ReadString());
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

        private void Write_Click(object sender, System.EventArgs e)
        {
            try
            {
                string textToWrite = ReplaceCommonEscapeSequences(_writeTextBox.Text);
                _mbSession.RawIO.Write(textToWrite);
            }
            catch (Exception exp)
            {
                MessageBox.Show(exp.Message);
            }
        }

        private void Read_Click(object sender, System.EventArgs e)
        {
            _readTextBox.Text = string.Empty;
            Cursor.Current = Cursors.WaitCursor;
            try
            {
                _readTextBox.Text = InsertCommonEscapeSequences(_mbSession.RawIO.ReadString());
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

        private void Clear_Click(object sender, System.EventArgs e)
        {
            _readTextBox.Text = string.Empty;
        }

        private void SetupControlState(bool isSessionOpen)
        {
            _openSessionButton.Enabled = !isSessionOpen;
            _closeSessionButton.Enabled = isSessionOpen;
            _queryButton.Enabled = isSessionOpen;
            _writeButton.Enabled = isSessionOpen;
            _readButton.Enabled = isSessionOpen;
            _writeTextBox.Enabled = isSessionOpen;
            _clearButton.Enabled = isSessionOpen;
            if (isSessionOpen)
            {
                _readTextBox.Text = string.Empty;
                _writeTextBox.Focus();
            }
        }

        private string ReplaceCommonEscapeSequences(string s)
        {
            return s.Replace("\\n", "\n").Replace("\\r", "\r");
        }

        private string InsertCommonEscapeSequences(string s)
        {
            return s.Replace("\n", "\\n").Replace("\r", "\\r");
        }
    }
}