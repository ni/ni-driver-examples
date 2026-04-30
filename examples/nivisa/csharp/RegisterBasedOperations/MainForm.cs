// ==================================================================================================
//
// Title      : MainForm.cs
// Purpose    : This application illustrates how to use In/Out/MoveIn/MoveOut on register-based
//              sessions.
//
// ==================================================================================================

using System;
using System.Globalization;
using System.Windows.Forms;
using Ivi.Visa;
using NationalInstruments.Visa;

namespace NationalInstruments.Examples.RegisterBasedOperations
{
    /// <summary>
    /// Summary description for Form1.
    /// </summary>
    public class MainForm : System.Windows.Forms.Form
    {
        private PxiSession _session;
        private System.Windows.Forms.Button _closeButton;
        private System.Windows.Forms.Button _openButton;
        private System.Windows.Forms.NumericUpDown[] _numberOfOutputArray;
        private System.Windows.Forms.Label _resourceNameLabel;
        private System.Windows.Forms.ComboBox _resourceNameComboBox;
        private System.Windows.Forms.Label _resultLabel;
        private System.Windows.Forms.TextBox _resultTextBox;
        private System.Windows.Forms.Button _clearButton;
        private System.Windows.Forms.Button _moveInButton;
        private System.Windows.Forms.Button _inButton;
        private System.Windows.Forms.Label _spaceLabel;
        private System.Windows.Forms.Label _offsetLabel;
        private System.Windows.Forms.Label _widthLabel;
        private System.Windows.Forms.ComboBox _spaceComboBox;
        private System.Windows.Forms.NumericUpDown _offsetNumericUpDown;
        private System.Windows.Forms.ComboBox _widthComboBox;
        private System.Windows.Forms.NumericUpDown _numberOfElementsNumericUpDown;
        private System.Windows.Forms.Label _numberOfElementsLabel;
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
            SetupUI(false);
            PopulateComboBoxes();
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
                if (_session != null)
                {
                    _session.Dispose();
                }
                if (_inButton != null)
                {
                    _inButton.Dispose();
                }
                if (_widthLabel != null)
                {
                    _widthLabel.Dispose();
                }
                if (_clearButton != null)
                {
                    _clearButton.Dispose();
                }
                if (_resultTextBox != null)
                {
                    _resultTextBox.Dispose();
                }
                if (_offsetNumericUpDown != null)
                {
                    _offsetNumericUpDown.Dispose();
                }
                if (_numberOfElementsNumericUpDown != null)
                {
                    _numberOfElementsNumericUpDown.Dispose();
                }
                if (_resultLabel != null)
                {
                    _resultLabel.Dispose();
                }
                if (_openButton != null)
                {
                    _openButton.Dispose();
                }
                if (_widthComboBox != null)
                {
                    _widthComboBox.Dispose();
                }
                if (_moveInButton != null)
                {
                    _moveInButton.Dispose();
                }
                if (_spaceComboBox != null)
                {
                    _spaceComboBox.Dispose();
                }
                if (_resourceNameComboBox != null)
                {
                    _resourceNameComboBox.Dispose();
                }
                if (_numberOfElementsLabel != null)
                {
                    _numberOfElementsLabel.Dispose();
                }
                if (_offsetLabel != null)
                {
                    _offsetLabel.Dispose();
                }
                if (_resourceNameLabel != null)
                {
                    _resourceNameLabel.Dispose();
                }
                if (_closeButton != null)
                {
                    _closeButton.Dispose();
                }
                if (_spaceLabel != null)
                {
                    _spaceLabel.Dispose();
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
            this._resourceNameLabel = new System.Windows.Forms.Label();
            this._offsetNumericUpDown = new System.Windows.Forms.NumericUpDown();
            this._numberOfOutputArray = new System.Windows.Forms.NumericUpDown[4];
            this._numberOfElementsNumericUpDown = new System.Windows.Forms.NumericUpDown();
            this._offsetLabel = new System.Windows.Forms.Label();
            this._numberOfElementsLabel = new System.Windows.Forms.Label();
            this._moveInButton = new System.Windows.Forms.Button();
            this._inButton = new System.Windows.Forms.Button();
            this._resultTextBox = new System.Windows.Forms.TextBox();
            this._resultLabel = new System.Windows.Forms.Label();
            this._clearButton = new System.Windows.Forms.Button();
            this._spaceComboBox = new System.Windows.Forms.ComboBox();
            this._spaceLabel = new System.Windows.Forms.Label();
            this._widthLabel = new System.Windows.Forms.Label();
            this._widthComboBox = new System.Windows.Forms.ComboBox();
            this._resourceNameComboBox = new System.Windows.Forms.ComboBox();
            this._closeButton = new System.Windows.Forms.Button();
            this._openButton = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this._offsetNumericUpDown)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this._numberOfElementsNumericUpDown)).BeginInit();
            this.SuspendLayout();
            //
            // _resourceNameLabel
            //
            this._resourceNameLabel.Location = new System.Drawing.Point(20, 8);
            this._resourceNameLabel.Name = "_resourceNameLabel";
            this._resourceNameLabel.Size = new System.Drawing.Size(96, 16);
            this._resourceNameLabel.TabIndex = 0;
            this._resourceNameLabel.Text = "Resource Name:";
            //
            // _offsetNumericUpDown
            //
            this._offsetNumericUpDown.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this._offsetNumericUpDown.Hexadecimal = true;
            this._offsetNumericUpDown.Location = new System.Drawing.Point(20, 169);
            this._offsetNumericUpDown.Maximum = new decimal(new int[]
            {
            65535,
            0,
            0,
            0
            });
            this._offsetNumericUpDown.Name = "_offsetNumericUpDown";
            this._offsetNumericUpDown.Size = new System.Drawing.Size(137, 20);
            this._offsetNumericUpDown.TabIndex = 8;
            //
            // _numberOfElementsNumericUpDown
            //
            this._numberOfElementsNumericUpDown.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this._numberOfElementsNumericUpDown.Location = new System.Drawing.Point(174, 169);
            this._numberOfElementsNumericUpDown.Name = "_numberOfElementsNumericUpDown";
            this._numberOfElementsNumericUpDown.Size = new System.Drawing.Size(137, 20);
            this._numberOfElementsNumericUpDown.TabIndex = 8;
            this._numberOfElementsNumericUpDown.Value = new decimal(new int[]
            {
            1,
            0,
            0,
            0
            });
            //
            // _offsetLabel
            //
            this._offsetLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this._offsetLabel.Location = new System.Drawing.Point(21, 150);
            this._offsetLabel.Name = "_offsetLabel";
            this._offsetLabel.Size = new System.Drawing.Size(72, 16);
            this._offsetLabel.TabIndex = 9;
            this._offsetLabel.Text = "Offset:";
            //
            // _numberOfElementsLabel
            //
            this._numberOfElementsLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this._numberOfElementsLabel.Location = new System.Drawing.Point(171, 150);
            this._numberOfElementsLabel.Name = "_numberOfElementsLabel";
            this._numberOfElementsLabel.Size = new System.Drawing.Size(112, 16);
            this._numberOfElementsLabel.TabIndex = 10;
            this._numberOfElementsLabel.Text = "Number of Elements:";
            //
            // _moveInButton
            //
            this._moveInButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this._moveInButton.Location = new System.Drawing.Point(172, 219);
            this._moveInButton.Name = "_moveInButton";
            this._moveInButton.Size = new System.Drawing.Size(139, 24);
            this._moveInButton.TabIndex = 11;
            this._moveInButton.Text = "Move In";
            this._moveInButton.Click += new System.EventHandler(this.MoveInButton_Click);
            //
            // _inButton
            //
            this._inButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this._inButton.Location = new System.Drawing.Point(20, 219);
            this._inButton.Name = "_inButton";
            this._inButton.Size = new System.Drawing.Size(128, 24);
            this._inButton.TabIndex = 13;
            this._inButton.Text = "In";
            this._inButton.Click += new System.EventHandler(this.InButton_Click);
            //
            // _resultTextBox
            //
            this._resultTextBox.AcceptsReturn = true;
            this._resultTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
            | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this._resultTextBox.Location = new System.Drawing.Point(23, 276);
            this._resultTextBox.Multiline = true;
            this._resultTextBox.Name = "_resultTextBox";
            this._resultTextBox.ReadOnly = true;
            this._resultTextBox.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this._resultTextBox.Size = new System.Drawing.Size(292, 242);
            this._resultTextBox.TabIndex = 17;
            //
            // _resultLabel
            //
            this._resultLabel.Location = new System.Drawing.Point(20, 257);
            this._resultLabel.Name = "_resultLabel";
            this._resultLabel.Size = new System.Drawing.Size(64, 16);
            this._resultLabel.TabIndex = 18;
            this._resultLabel.Text = "Result:";
            //
            // _clearButton
            //
            this._clearButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this._clearButton.Location = new System.Drawing.Point(235, 522);
            this._clearButton.Name = "_clearButton";
            this._clearButton.Size = new System.Drawing.Size(80, 24);
            this._clearButton.TabIndex = 24;
            this._clearButton.Text = "Clear";
            this._clearButton.Click += new System.EventHandler(this.ClearButton_Click);
            //
            // _spaceComboBox
            //
            this._spaceComboBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this._spaceComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this._spaceComboBox.Location = new System.Drawing.Point(20, 121);
            this._spaceComboBox.Name = "_spaceComboBox";
            this._spaceComboBox.Size = new System.Drawing.Size(137, 21);
            this._spaceComboBox.TabIndex = 25;
            //
            // _spaceLabel
            //
            this._spaceLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this._spaceLabel.Location = new System.Drawing.Point(20, 104);
            this._spaceLabel.Name = "_spaceLabel";
            this._spaceLabel.Size = new System.Drawing.Size(52, 14);
            this._spaceLabel.TabIndex = 26;
            this._spaceLabel.Text = "Space:";
            //
            // _widthLabel
            //
            this._widthLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this._widthLabel.Location = new System.Drawing.Point(171, 104);
            this._widthLabel.Name = "_widthLabel";
            this._widthLabel.Size = new System.Drawing.Size(40, 16);
            this._widthLabel.TabIndex = 27;
            this._widthLabel.Text = "Width:";
            //
            // _widthComboBox
            //
            this._widthComboBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this._widthComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this._widthComboBox.Location = new System.Drawing.Point(174, 121);
            this._widthComboBox.Name = "_widthComboBox";
            this._widthComboBox.Size = new System.Drawing.Size(137, 21);
            this._widthComboBox.TabIndex = 28;
            //
            // _resourceNameComboBox
            //
            this._resourceNameComboBox.FormattingEnabled = true;
            this._resourceNameComboBox.Location = new System.Drawing.Point(20, 24);
            this._resourceNameComboBox.Name = "_resourceNameComboBox";
            this._resourceNameComboBox.Size = new System.Drawing.Size(291, 21);
            this._resourceNameComboBox.TabIndex = 29;
            //
            // _closeButton
            //
            this._closeButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this._closeButton.Location = new System.Drawing.Point(173, 55);
            this._closeButton.Name = "_closeButton";
            this._closeButton.Size = new System.Drawing.Size(137, 24);
            this._closeButton.TabIndex = 30;
            this._closeButton.Text = "Close Session";
            this._closeButton.Click += new System.EventHandler(this.CloseButton_Click);
            //
            // _openButton
            //
            this._openButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this._openButton.Location = new System.Drawing.Point(20, 55);
            this._openButton.Name = "_openButton";
            this._openButton.Size = new System.Drawing.Size(137, 24);
            this._openButton.TabIndex = 30;
            this._openButton.Text = "Open Session";
            this._openButton.Click += new System.EventHandler(this.OpenButton_Click);
            //
            // MainForm
            //
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.ClientSize = new System.Drawing.Size(327, 558);
            this.Controls.Add(this._openButton);
            this.Controls.Add(this._closeButton);
            this.Controls.Add(this._resourceNameComboBox);
            this.Controls.Add(this._widthComboBox);
            this.Controls.Add(this._widthLabel);
            this.Controls.Add(this._spaceLabel);
            this.Controls.Add(this._spaceComboBox);
            this.Controls.Add(this._clearButton);
            this.Controls.Add(this._resultLabel);
            this.Controls.Add(this._resultTextBox);
            this.Controls.Add(this._inButton);
            this.Controls.Add(this._moveInButton);
            this.Controls.Add(this._numberOfElementsLabel);
            this.Controls.Add(this._offsetLabel);
            this.Controls.Add(this._offsetNumericUpDown);
            this.Controls.Add(this._resourceNameLabel);
            this.Controls.Add(this._numberOfElementsNumericUpDown);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon", CultureInfo.CurrentCulture)));
            this.MaximizeBox = false;
            this.MinimumSize = new System.Drawing.Size(300, 440);
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Register-Based Operations";
            ((System.ComponentModel.ISupportInitialize)(this._offsetNumericUpDown)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this._numberOfElementsNumericUpDown)).EndInit();
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

        private void OpenButton_Click(object sender, System.EventArgs e)
        {
            if (_session != null)
            {
                _session.Dispose();
            }

            try
            {
                using (var rmSession = new ResourceManager())
                {
                    _session = (PxiSession)rmSession.Open(_resourceNameComboBox.Text);
                    SetupUI(true);
                }
            }
            catch (Exception exp)
            {
                MessageBox.Show(exp.Message);
            }
        }

        private void CloseButton_Click(object sender, System.EventArgs e)
        {
            try
            {
                if (_session != null)
                {
                    _session.Dispose();
                    SetupUI(false);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void ClearButton_Click(object sender, System.EventArgs e)
        {
            _resultTextBox.Clear();
        }

        // Performs an "InXX" operation
        private void InButton_Click(object sender, System.EventArgs e)
        {
            try
            {
                AddressSpace space = (AddressSpace)_spaceComboBox.SelectedItem;
                int offset = (int)_offsetNumericUpDown.Value;
                DataWidth width = (DataWidth)_widthComboBox.SelectedItem;

                switch (width)
                {
                    case DataWidth.Width8:
                        _resultTextBox.AppendText(GetOperationText("In8"));
                        byte data8 = _session.In8(space, offset);
                        _resultTextBox.AppendText(GetDataText(data8.ToString("x", CultureInfo.InvariantCulture)));
                        break;

                    case DataWidth.Width16:
                        _resultTextBox.AppendText(GetOperationText("In16"));
                        short data16 = _session.In16(space, offset);
                        _resultTextBox.AppendText(GetDataText(data16.ToString("x", CultureInfo.InvariantCulture)));
                        break;

                    case DataWidth.Width32:
                        _resultTextBox.AppendText(GetOperationText("In32"));
                        int data32 = _session.In32(space, offset);
                        _resultTextBox.AppendText(GetDataText(data32.ToString("x", CultureInfo.InvariantCulture)));
                        break;

                    case DataWidth.Width64:
                        _resultTextBox.AppendText(GetOperationText("In64"));
                        long data64 = _session.In64(space, offset);
                        _resultTextBox.AppendText(GetDataText(data64.ToString("x", CultureInfo.InvariantCulture)));
                        break;
                }
            }
            catch (Exception ex)
            {
                _resultTextBox.AppendText(GetOperationText(ex.Message));
            }
            ScrollToBottomOfResultTextBox();
        }

        // Perform a "MoveInXX" operation.
        private void MoveInButton_Click(object sender, System.EventArgs e)
        {
            try
            {
                AddressSpace space = (AddressSpace)_spaceComboBox.SelectedItem;
                int offset = (int)_offsetNumericUpDown.Value;
                DataWidth width = (DataWidth)_widthComboBox.SelectedItem;
                int length = (int)_numberOfElementsNumericUpDown.Value;

                switch (width)
                {
                    case DataWidth.Width8:
                        _resultTextBox.AppendText(GetOperationText("MoveIn8"));
                        byte[] data8 = _session.MoveIn8(space, offset, length);
                        ShowArray(data8);
                        break;

                    case DataWidth.Width16:
                        _resultTextBox.AppendText(GetOperationText("MoveIn16"));
                        short[] data16 = _session.MoveIn16(space, offset, length);
                        ShowArray(data16);
                        break;

                    case DataWidth.Width32:
                        _resultTextBox.AppendText(GetOperationText("MoveIn32"));
                        int[] data32 = _session.MoveIn32(space, offset, length);
                        ShowArray(data32);
                        break;

                    case DataWidth.Width64:
                        _resultTextBox.AppendText(GetOperationText("MoveIn64"));
                        long[] data64 = _session.MoveIn64(space, offset, length);
                        ShowArray(data64);
                        break;
                }
            }
            catch (Exception ex)
            {
                _resultTextBox.AppendText(GetOperationText(ex.Message));
            }
            ScrollToBottomOfResultTextBox();
        }

        private void PopulateComboBoxes()
        {
            try
            {
                // This example uses an instance of the NationalInstruments.Visa.ResourceManager class to find resources on the system.
                // Alternatively, static methods provided by the Ivi.Visa.ResourceManager class may be used when an application
                // requires additional VISA .NET implementations.
                using (var rmSession = new ResourceManager())
                {
                    var pXIResources = rmSession.Find("PXI?*INSTR");
                    foreach (var resource in pXIResources)
                    {
                        _resourceNameComboBox.Items.Add(resource);
                    }
                    // Add PXI specific address spaces only
                    for (AddressSpace space = AddressSpace.PxiConfiguration; space <= AddressSpace.PxiBar5; ++space)
                    {
                        _spaceComboBox.Items.Add(space);
                    }
                    _spaceComboBox.SelectedIndex = 0;

                    foreach (DataWidth width in Enum.GetValues(typeof(DataWidth)))
                    {
                        _widthComboBox.Items.Add(width);
                    }
                    _widthComboBox.SelectedIndex = 0;
                }
            }
            catch (Exception)
            {
                _resourceNameComboBox.Items.Add("No PXI Resource found on the system");
                _resourceNameComboBox.Enabled = false;
                _openButton.Enabled = false;
            }
            _resourceNameComboBox.SelectedIndex = 0;
        }

        private void SetupUI(bool sessionActive)
        {
            _resourceNameComboBox.Enabled = !sessionActive;
            _openButton.Enabled = !sessionActive;
            _closeButton.Enabled = sessionActive;
            _moveInButton.Enabled = sessionActive;
            _inButton.Enabled = sessionActive;
            _spaceLabel.Enabled = sessionActive;
            _spaceComboBox.Enabled = sessionActive;
            _widthLabel.Enabled = sessionActive;
            _widthComboBox.Enabled = sessionActive;
            _offsetLabel.Enabled = sessionActive;
            _offsetNumericUpDown.Enabled = sessionActive;
            _numberOfElementsLabel.Enabled = sessionActive;
            _numberOfElementsNumericUpDown.Enabled = sessionActive;
        }

        private void ScrollToBottomOfResultTextBox()
        {
            _resultTextBox.SelectAll();
        }

        private string GetOperationText(string operation)
        {
            return operation + Environment.NewLine;
        }

        private string GetDataText(string data)
        {
            return (string.Format(CultureInfo.InvariantCulture, "Data = {0}", data) + Environment.NewLine);
        }

        private void ShowArray(Array data)
        {
            int i = 0;
            foreach (object o in data)
            {
                string formattedValue = string.Empty;
                if (o is byte)
                {
                    formattedValue = ((byte)o).ToString("x", CultureInfo.InvariantCulture);
                }
                else if (o is short)
                {
                    formattedValue = ((short)o).ToString("x", CultureInfo.InvariantCulture);
                }
                else if (o is int)
                {
                    formattedValue = ((int)o).ToString("x", CultureInfo.InvariantCulture);
                }
                else if (o is long)
                {
                    formattedValue = ((long)o).ToString("x", CultureInfo.InvariantCulture);
                }
                _resultTextBox.AppendText(string.Format(CultureInfo.InvariantCulture, "Data({0} = {1})", i++, formattedValue) + Environment.NewLine);
            }
        }

        private byte[] BuildByteOutputData()
        {
            int numberOfElements = (int)_numberOfElementsNumericUpDown.Value;
            byte[] od = new byte[numberOfElements];
            for (int i = 0; i < numberOfElements; i++)
            {
                od[i] = (byte)_numberOfOutputArray[i].Value;
            }
            return od;
        }

        private short[] BuildShortOutputData()
        {
            int numberOfElements = (int)_numberOfElementsNumericUpDown.Value;
            short[] od = new short[numberOfElements];
            for (int i = 0; i < numberOfElements; i++)
            {
                od[i] = (short)_numberOfOutputArray[i].Value;
            }
            return od;
        }

        private int[] BuildIntOutputData()
        {
            int numberOfElements = (int)_numberOfElementsNumericUpDown.Value;
            int[] od = new int[numberOfElements];
            for (int i = 0; i < numberOfElements; i++)
            {
                od[i] = (int)_numberOfOutputArray[i].Value;
            }
            return od;
        }
    }
}