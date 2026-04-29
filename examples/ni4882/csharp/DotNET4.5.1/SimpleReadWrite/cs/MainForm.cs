using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;
using NationalInstruments.NI4882;

namespace NationalInstruments.Examples.SimpleReadWrite
{
    /// <summary>
    /// Summary description for Form1.
    /// </summary>
    public class MainForm : System.Windows.Forms.Form
    {
        private System.Windows.Forms.Button openButton;
        private System.Windows.Forms.Button closeButton;
        private System.Windows.Forms.TextBox stringToWriteTextBox;
        private System.Windows.Forms.Button writeButton;
        private System.Windows.Forms.Label stringToWriteLabel;
        private System.Windows.Forms.Button readButton;
        private System.Windows.Forms.Label stringReadLabel;
        private Device device;
        private System.Windows.Forms.NumericUpDown boardIdNumericUpDown;
        private System.Windows.Forms.Label boardIdLabel;
        private System.Windows.Forms.NumericUpDown primaryAddressNumericUpDown;
        private System.Windows.Forms.Label primaryAddressLabel;
        private System.Windows.Forms.Label secondaryAddressLabel;
        private System.Windows.Forms.TextBox stringReadTextBox;
		private System.Windows.Forms.ComboBox secondaryAddressComboBox;
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.Container components = null;

        public MainForm()
        {
            //
            // Required for Windows Form Designer support
            //
            InitializeComponent();
			// Initialize the Combo Box with the Secondary Address possible values
			secondaryAddressComboBox.Items.Add("None");
			for (int i=96;i<=126;i++)
			{
				secondaryAddressComboBox.Items.Add(i);
			}
			secondaryAddressComboBox.SelectedIndex = 0;
		}

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        protected override void Dispose( bool disposing )
        {
            if( disposing )
            {
                if (device != null)
                {
                    device.Dispose();
                }
                if (components != null) 
                {
                    components.Dispose();
                }
            }
            base.Dispose( disposing );
        }

        #region Windows Form Designer generated code
        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
			System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(MainForm));
			this.openButton = new System.Windows.Forms.Button();
			this.closeButton = new System.Windows.Forms.Button();
			this.stringToWriteTextBox = new System.Windows.Forms.TextBox();
			this.stringReadTextBox = new System.Windows.Forms.TextBox();
			this.writeButton = new System.Windows.Forms.Button();
			this.stringToWriteLabel = new System.Windows.Forms.Label();
			this.readButton = new System.Windows.Forms.Button();
			this.stringReadLabel = new System.Windows.Forms.Label();
			this.boardIdNumericUpDown = new System.Windows.Forms.NumericUpDown();
			this.primaryAddressNumericUpDown = new System.Windows.Forms.NumericUpDown();
			this.boardIdLabel = new System.Windows.Forms.Label();
			this.primaryAddressLabel = new System.Windows.Forms.Label();
			this.secondaryAddressLabel = new System.Windows.Forms.Label();
			this.secondaryAddressComboBox = new System.Windows.Forms.ComboBox();
			((System.ComponentModel.ISupportInitialize)(this.boardIdNumericUpDown)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.primaryAddressNumericUpDown)).BeginInit();
			this.SuspendLayout();
			// 
			// openButton
			// 
			this.openButton.Location = new System.Drawing.Point(8, 96);
			this.openButton.Name = "openButton";
			this.openButton.TabIndex = 2;
			this.openButton.Text = "&Open";
			this.openButton.Click += new System.EventHandler(this.openButton_Click);
			// 
			// closeButton
			// 
			this.closeButton.Location = new System.Drawing.Point(88, 96);
			this.closeButton.Name = "closeButton";
			this.closeButton.TabIndex = 3;
			this.closeButton.Text = "&Close";
			this.closeButton.Click += new System.EventHandler(this.closeButton_Click);
			// 
			// stringToWriteTextBox
			// 
			this.stringToWriteTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
				| System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.stringToWriteTextBox.Location = new System.Drawing.Point(8, 152);
			this.stringToWriteTextBox.Name = "stringToWriteTextBox";
			this.stringToWriteTextBox.Size = new System.Drawing.Size(400, 20);
			this.stringToWriteTextBox.TabIndex = 4;
			this.stringToWriteTextBox.Text = "*idn?\\n";
			// 
			// stringReadTextBox
			// 
			this.stringReadTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
				| System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.stringReadTextBox.Location = new System.Drawing.Point(8, 248);
			this.stringReadTextBox.Multiline = true;
			this.stringReadTextBox.Name = "stringReadTextBox";
			this.stringReadTextBox.ReadOnly = true;
			this.stringReadTextBox.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
			this.stringReadTextBox.Size = new System.Drawing.Size(400, 88);
			this.stringReadTextBox.TabIndex = 6;
			this.stringReadTextBox.Text = "";
			// 
			// writeButton
			// 
			this.writeButton.Location = new System.Drawing.Point(8, 184);
			this.writeButton.Name = "writeButton";
			this.writeButton.TabIndex = 7;
			this.writeButton.Text = "&Write";
			this.writeButton.Click += new System.EventHandler(this.writeButton_Click);
			// 
			// stringToWriteLabel
			// 
			this.stringToWriteLabel.Location = new System.Drawing.Point(8, 128);
			this.stringToWriteLabel.Name = "stringToWriteLabel";
			this.stringToWriteLabel.TabIndex = 8;
			this.stringToWriteLabel.Text = "String to Write:";
			// 
			// readButton
			// 
			this.readButton.Location = new System.Drawing.Point(88, 184);
			this.readButton.Name = "readButton";
			this.readButton.TabIndex = 9;
			this.readButton.Text = "&Read";
			this.readButton.Click += new System.EventHandler(this.readButton_Click);
			// 
			// stringReadLabel
			// 
			this.stringReadLabel.Location = new System.Drawing.Point(8, 216);
			this.stringReadLabel.Name = "stringReadLabel";
			this.stringReadLabel.TabIndex = 10;
			this.stringReadLabel.Text = "String Read:";
			// 
			// boardIdNumericUpDown
			// 
			this.boardIdNumericUpDown.Location = new System.Drawing.Point(128, 16);
			this.boardIdNumericUpDown.Name = "boardIdNumericUpDown";
			this.boardIdNumericUpDown.Size = new System.Drawing.Size(40, 20);
			this.boardIdNumericUpDown.TabIndex = 11;
			// 
			// primaryAddressNumericUpDown
			// 
			this.primaryAddressNumericUpDown.Location = new System.Drawing.Point(128, 40);
			this.primaryAddressNumericUpDown.Name = "primaryAddressNumericUpDown";
			this.primaryAddressNumericUpDown.Size = new System.Drawing.Size(40, 20);
			this.primaryAddressNumericUpDown.TabIndex = 12;
			this.primaryAddressNumericUpDown.Value = new System.Decimal(new int[] {
																		 2,
																		 0,
																		 0,
																		 0});
			// 
			// boardIdLabel
			// 
			this.boardIdLabel.Location = new System.Drawing.Point(8, 16);
			this.boardIdLabel.Name = "boardIdLabel";
			this.boardIdLabel.Size = new System.Drawing.Size(72, 16);
			this.boardIdLabel.TabIndex = 14;
			this.boardIdLabel.Text = "Board ID:";
			// 
			// primaryAddressLabel
			// 
			this.primaryAddressLabel.Location = new System.Drawing.Point(8, 40);
			this.primaryAddressLabel.Name = "primaryAddressLabel";
			this.primaryAddressLabel.Size = new System.Drawing.Size(100, 16);
			this.primaryAddressLabel.TabIndex = 15;
			this.primaryAddressLabel.Text = "Primary Address:";
			// 
			// secondaryAddressLabel
			// 
			this.secondaryAddressLabel.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.secondaryAddressLabel.Location = new System.Drawing.Point(8, 64);
			this.secondaryAddressLabel.Name = "secondaryAddressLabel";
			this.secondaryAddressLabel.Size = new System.Drawing.Size(112, 16);
			this.secondaryAddressLabel.TabIndex = 16;
			this.secondaryAddressLabel.Text = "Secondary Address:";
			// 
			// secondaryAddressComboBox
			// 
			this.secondaryAddressComboBox.Location = new System.Drawing.Point(128, 64);
			this.secondaryAddressComboBox.Name = "secondaryAddressComboBox";
			this.secondaryAddressComboBox.Size = new System.Drawing.Size(56, 21);
			this.secondaryAddressComboBox.TabIndex = 17;
			this.secondaryAddressComboBox.Text = "secondaryAddressComboBox";
			// 
			// MainForm
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(416, 341);
			this.Controls.Add(this.secondaryAddressComboBox);
			this.Controls.Add(this.secondaryAddressLabel);
			this.Controls.Add(this.primaryAddressLabel);
			this.Controls.Add(this.boardIdLabel);
			this.Controls.Add(this.primaryAddressNumericUpDown);
			this.Controls.Add(this.boardIdNumericUpDown);
			this.Controls.Add(this.stringReadLabel);
			this.Controls.Add(this.readButton);
			this.Controls.Add(this.stringToWriteLabel);
			this.Controls.Add(this.writeButton);
			this.Controls.Add(this.stringReadTextBox);
			this.Controls.Add(this.stringToWriteTextBox);
			this.Controls.Add(this.closeButton);
			this.Controls.Add(this.openButton);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MinimumSize = new System.Drawing.Size(184, 304);
			this.Name = "MainForm";
			this.Text = "NI-488.2 Simple Read/Write";
			this.Load += new System.EventHandler(this.MainForm_Load);
			((System.ComponentModel.ISupportInitialize)(this.boardIdNumericUpDown)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.primaryAddressNumericUpDown)).EndInit();
			this.ResumeLayout(false);

		}
        #endregion

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main() 
        {
            Application.Run(new MainForm());
        }

        private void MainForm_Load(object sender, System.EventArgs e)
        {
            SetupControlState(false);
        }

        private void SetupControlState(bool isSessionOpen)
        {
            boardIdNumericUpDown.Enabled = !isSessionOpen;
            primaryAddressNumericUpDown.Enabled = !isSessionOpen;
            secondaryAddressComboBox.Enabled = !isSessionOpen;
            openButton.Enabled = !isSessionOpen;
            closeButton.Enabled = isSessionOpen;
            stringToWriteTextBox.Enabled = isSessionOpen;
            writeButton.Enabled = isSessionOpen;
            readButton.Enabled = isSessionOpen;
            stringReadTextBox.Enabled = isSessionOpen;
        }

        private void openButton_Click(object sender, System.EventArgs e)
        {
            try
            {
                Cursor.Current = Cursors.WaitCursor;
				// Convert the Secondary Address Combo Text into a number.
				int currentSecondaryAddress = 0;
				if (secondaryAddressComboBox.SelectedIndex != 0)
				{
					currentSecondaryAddress = (int)secondaryAddressComboBox.SelectedItem;
				}
                // Intialize the device
				device = new Device((int)boardIdNumericUpDown.Value,(byte)primaryAddressNumericUpDown.Value,(byte)currentSecondaryAddress);
                SetupControlState(true);
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                Cursor.Current = Cursors.Default;
            }
        }

        private void closeButton_Click(object sender, System.EventArgs e)
        {
            try
            {
                device.Dispose();
                SetupControlState(false);
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void writeButton_Click(object sender, System.EventArgs e)
        {
            try
            {
                device.Write(ReplaceCommonEscapeSequences(stringToWriteTextBox.Text));
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void readButton_Click(object sender, System.EventArgs e)
        {
            try
            {
                Cursor.Current = Cursors.WaitCursor;
                stringReadTextBox.Text = InsertCommonEscapeSequences(device.ReadString());
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                Cursor.Current = Cursors.Default;
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
