using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;
using NationalInstruments.NI4882;

namespace NationalInstruments.Examples.Notify
{
	public class MainForm : System.Windows.Forms.Form
	{
        private Device testDevice;
        private System.Windows.Forms.TextBox stringToWriteTextBox;
        private System.Windows.Forms.Label stringToWriteLabel;
        private System.Windows.Forms.TextBox stringReadTextBox;
        private System.Windows.Forms.Label stringReadLabel;
        private System.Windows.Forms.Button notifyOnDSRButton;
        private System.Windows.Forms.ComboBox reenableMaskComboBox;
        private System.Windows.Forms.Label reenableMaskLabel;
        private System.Windows.Forms.TextBox notifyStatusTextBox;
        private System.Windows.Forms.TextBox notifyCountTextBox;
        private System.Windows.Forms.Label notifyStatusLabel;
        private System.Windows.Forms.Label notifyCountLabel;
        private System.Windows.Forms.GroupBox deviceGroupBox;
        private System.Windows.Forms.Button closeButton;
        private System.Windows.Forms.Button openButton;
        private System.Windows.Forms.NumericUpDown padNumericUpDown;
        private System.Windows.Forms.NumericUpDown boardIDNumericUpDown;
        private System.Windows.Forms.Label sadLabel;
        private System.Windows.Forms.Label padLabel;
        private System.Windows.Forms.Label boardIDLabel;
        private System.Windows.Forms.GroupBox notifyDataGroupBox;
        private System.Windows.Forms.GroupBox communicationGroupBox;
        private System.Windows.Forms.GroupBox notifyControlGroupBox;
        private System.Windows.Forms.Button writeButton;
        private System.Windows.Forms.Button clearOutputButton;
        private System.Windows.Forms.ComboBox sadComboBox;

		private delegate string NotifyUpdateStatusDelegate(string readText, string status, string count);
		private NotifyUpdateStatusDelegate notifyUpdateStatusHandler;

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

            sadComboBox.Items.Add("None");
            
            for (int i= 96; i <= 126; ++i)
            {
                sadComboBox.Items.Add(i);
            }
            sadComboBox.SelectedIndex = 0;
            if(reenableMaskComboBox.Items.Count > 0)
            {
                reenableMaskComboBox.SelectedIndex = 0;
            }

            SetupControlState(false);

			notifyUpdateStatusHandler += new NotifyUpdateStatusDelegate(NotifyUpdateStatus);
		}

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
            if( disposing )
			{
                if(testDevice != null)
                {
                    testDevice.Dispose();
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
            this.stringToWriteTextBox = new System.Windows.Forms.TextBox();
            this.stringToWriteLabel = new System.Windows.Forms.Label();
            this.writeButton = new System.Windows.Forms.Button();
            this.stringReadTextBox = new System.Windows.Forms.TextBox();
            this.stringReadLabel = new System.Windows.Forms.Label();
            this.notifyOnDSRButton = new System.Windows.Forms.Button();
            this.reenableMaskComboBox = new System.Windows.Forms.ComboBox();
            this.reenableMaskLabel = new System.Windows.Forms.Label();
            this.notifyStatusTextBox = new System.Windows.Forms.TextBox();
            this.notifyCountTextBox = new System.Windows.Forms.TextBox();
            this.notifyStatusLabel = new System.Windows.Forms.Label();
            this.notifyCountLabel = new System.Windows.Forms.Label();
            this.deviceGroupBox = new System.Windows.Forms.GroupBox();
            this.sadComboBox = new System.Windows.Forms.ComboBox();
            this.closeButton = new System.Windows.Forms.Button();
            this.openButton = new System.Windows.Forms.Button();
            this.padNumericUpDown = new System.Windows.Forms.NumericUpDown();
            this.boardIDNumericUpDown = new System.Windows.Forms.NumericUpDown();
            this.sadLabel = new System.Windows.Forms.Label();
            this.padLabel = new System.Windows.Forms.Label();
            this.boardIDLabel = new System.Windows.Forms.Label();
            this.notifyDataGroupBox = new System.Windows.Forms.GroupBox();
            this.communicationGroupBox = new System.Windows.Forms.GroupBox();
            this.clearOutputButton = new System.Windows.Forms.Button();
            this.notifyControlGroupBox = new System.Windows.Forms.GroupBox();
            this.deviceGroupBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.padNumericUpDown)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.boardIDNumericUpDown)).BeginInit();
            this.notifyDataGroupBox.SuspendLayout();
            this.communicationGroupBox.SuspendLayout();
            this.notifyControlGroupBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // stringToWriteTextBox
            // 
            this.stringToWriteTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
                | System.Windows.Forms.AnchorStyles.Right)));
            this.stringToWriteTextBox.Location = new System.Drawing.Point(16, 40);
            this.stringToWriteTextBox.Name = "stringToWriteTextBox";
            this.stringToWriteTextBox.Size = new System.Drawing.Size(296, 20);
            this.stringToWriteTextBox.TabIndex = 8;
            this.stringToWriteTextBox.Text = "*IDN?\\n";
            // 
            // stringToWriteLabel
            // 
            this.stringToWriteLabel.Location = new System.Drawing.Point(16, 24);
            this.stringToWriteLabel.Name = "stringToWriteLabel";
            this.stringToWriteLabel.Size = new System.Drawing.Size(88, 16);
            this.stringToWriteLabel.TabIndex = 9;
            this.stringToWriteLabel.Text = "String To Write:";
            // 
            // writeButton
            // 
            this.writeButton.Location = new System.Drawing.Point(16, 64);
            this.writeButton.Name = "writeButton";
            this.writeButton.Size = new System.Drawing.Size(72, 24);
            this.writeButton.TabIndex = 10;
            this.writeButton.Text = "Write";
            this.writeButton.Click += new System.EventHandler(this.writeButton_Click);
            // 
            // stringReadTextBox
            // 
            this.stringReadTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
                | System.Windows.Forms.AnchorStyles.Left) 
                | System.Windows.Forms.AnchorStyles.Right)));
            this.stringReadTextBox.Location = new System.Drawing.Point(16, 120);
            this.stringReadTextBox.Multiline = true;
            this.stringReadTextBox.Name = "stringReadTextBox";
            this.stringReadTextBox.ReadOnly = true;
            this.stringReadTextBox.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.stringReadTextBox.Size = new System.Drawing.Size(296, 80);
            this.stringReadTextBox.TabIndex = 11;
            this.stringReadTextBox.Text = "";
            // 
            // stringReadLabel
            // 
            this.stringReadLabel.Location = new System.Drawing.Point(16, 104);
            this.stringReadLabel.Name = "stringReadLabel";
            this.stringReadLabel.Size = new System.Drawing.Size(216, 16);
            this.stringReadLabel.TabIndex = 12;
            this.stringReadLabel.Text = "String Read After DeviceServiceRequest:";
            // 
            // notifyOnDSRButton
            // 
            this.notifyOnDSRButton.Location = new System.Drawing.Point(16, 24);
            this.notifyOnDSRButton.Name = "notifyOnDSRButton";
            this.notifyOnDSRButton.Size = new System.Drawing.Size(184, 24);
            this.notifyOnDSRButton.TabIndex = 13;
            this.notifyOnDSRButton.Text = "Notify on DeviceServiceRequest";
            this.notifyOnDSRButton.Click += new System.EventHandler(this.notifyOnDSRButton_Click);
            // 
            // reenableMaskComboBox
            // 
            this.reenableMaskComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.reenableMaskComboBox.Items.AddRange(new object[] {
                                                                      "DeviceServiceRequest",
                                                                      "None"});
            this.reenableMaskComboBox.Location = new System.Drawing.Point(16, 72);
            this.reenableMaskComboBox.Name = "reenableMaskComboBox";
            this.reenableMaskComboBox.Size = new System.Drawing.Size(184, 21);
            this.reenableMaskComboBox.TabIndex = 14;
            // 
            // reenableMaskLabel
            // 
            this.reenableMaskLabel.Location = new System.Drawing.Point(16, 56);
            this.reenableMaskLabel.Name = "reenableMaskLabel";
            this.reenableMaskLabel.Size = new System.Drawing.Size(136, 16);
            this.reenableMaskLabel.TabIndex = 15;
            this.reenableMaskLabel.Text = "Reenable Notify Mask:";
            // 
            // notifyStatusTextBox
            // 
            this.notifyStatusTextBox.Location = new System.Drawing.Point(16, 40);
            this.notifyStatusTextBox.Name = "notifyStatusTextBox";
            this.notifyStatusTextBox.ReadOnly = true;
            this.notifyStatusTextBox.Size = new System.Drawing.Size(120, 20);
            this.notifyStatusTextBox.TabIndex = 16;
            this.notifyStatusTextBox.Text = "";
            // 
            // notifyCountTextBox
            // 
            this.notifyCountTextBox.Location = new System.Drawing.Point(16, 88);
            this.notifyCountTextBox.Name = "notifyCountTextBox";
            this.notifyCountTextBox.ReadOnly = true;
            this.notifyCountTextBox.Size = new System.Drawing.Size(120, 20);
            this.notifyCountTextBox.TabIndex = 18;
            this.notifyCountTextBox.Text = "";
            // 
            // notifyStatusLabel
            // 
            this.notifyStatusLabel.Location = new System.Drawing.Point(16, 24);
            this.notifyStatusLabel.Name = "notifyStatusLabel";
            this.notifyStatusLabel.Size = new System.Drawing.Size(80, 16);
            this.notifyStatusLabel.TabIndex = 20;
            this.notifyStatusLabel.Text = "Status:";
            // 
            // notifyCountLabel
            // 
            this.notifyCountLabel.Location = new System.Drawing.Point(16, 72);
            this.notifyCountLabel.Name = "notifyCountLabel";
            this.notifyCountLabel.Size = new System.Drawing.Size(72, 16);
            this.notifyCountLabel.TabIndex = 22;
            this.notifyCountLabel.Text = "Count:";
            // 
            // deviceGroupBox
            // 
            this.deviceGroupBox.Controls.Add(this.sadComboBox);
            this.deviceGroupBox.Controls.Add(this.closeButton);
            this.deviceGroupBox.Controls.Add(this.openButton);
            this.deviceGroupBox.Controls.Add(this.padNumericUpDown);
            this.deviceGroupBox.Controls.Add(this.boardIDNumericUpDown);
            this.deviceGroupBox.Controls.Add(this.sadLabel);
            this.deviceGroupBox.Controls.Add(this.padLabel);
            this.deviceGroupBox.Controls.Add(this.boardIDLabel);
            this.deviceGroupBox.Location = new System.Drawing.Point(8, 8);
            this.deviceGroupBox.Name = "deviceGroupBox";
            this.deviceGroupBox.Size = new System.Drawing.Size(194, 144);
            this.deviceGroupBox.TabIndex = 23;
            this.deviceGroupBox.TabStop = false;
            this.deviceGroupBox.Text = "Device";
            // 
            // sadComboBox
            // 
            this.sadComboBox.Location = new System.Drawing.Point(128, 64);
            this.sadComboBox.Name = "sadComboBox";
            this.sadComboBox.Size = new System.Drawing.Size(60, 21);
            this.sadComboBox.TabIndex = 16;
            // 
            // closeButton
            // 
            this.closeButton.Location = new System.Drawing.Point(96, 104);
            this.closeButton.Name = "closeButton";
            this.closeButton.Size = new System.Drawing.Size(72, 24);
            this.closeButton.TabIndex = 15;
            this.closeButton.Text = "Close";
            this.closeButton.Click += new System.EventHandler(this.closeButton_Click);
            // 
            // openButton
            // 
            this.openButton.Location = new System.Drawing.Point(16, 104);
            this.openButton.Name = "openButton";
            this.openButton.Size = new System.Drawing.Size(72, 24);
            this.openButton.TabIndex = 14;
            this.openButton.Text = "Open";
            this.openButton.Click += new System.EventHandler(this.openButton_Click);
            // 
            // padNumericUpDown
            // 
            this.padNumericUpDown.Location = new System.Drawing.Point(128, 48);
            this.padNumericUpDown.Maximum = new System.Decimal(new int[] {
                                                                       31,
                                                                       0,
                                                                       0,
                                                                       0});
            this.padNumericUpDown.Name = "padNumericUpDown";
            this.padNumericUpDown.Size = new System.Drawing.Size(60, 20);
            this.padNumericUpDown.TabIndex = 12;
            this.padNumericUpDown.Value = new System.Decimal(new int[] {
                                                                     2,
                                                                     0,
                                                                     0,
                                                                     0});
            // 
            // boardIDNumericUpDown
            // 
            this.boardIDNumericUpDown.Location = new System.Drawing.Point(128, 24);
            this.boardIDNumericUpDown.Maximum = new System.Decimal(new int[] {
                                                                           99,
                                                                           0,
                                                                           0,
                                                                           0});
            this.boardIDNumericUpDown.Name = "boardIDNumericUpDown";
            this.boardIDNumericUpDown.Size = new System.Drawing.Size(60, 20);
            this.boardIDNumericUpDown.TabIndex = 11;
            // 
            // sadLabel
            // 
            this.sadLabel.Location = new System.Drawing.Point(16, 72);
            this.sadLabel.Name = "sadLabel";
            this.sadLabel.Size = new System.Drawing.Size(110, 16);
            this.sadLabel.TabIndex = 10;
            this.sadLabel.Text = "Secondary Address:";
            // 
            // padLabel
            // 
            this.padLabel.Location = new System.Drawing.Point(16, 48);
            this.padLabel.Name = "padLabel";
            this.padLabel.Size = new System.Drawing.Size(112, 16);
            this.padLabel.TabIndex = 9;
            this.padLabel.Text = "Primary Address:";
            // 
            // boardIDLabel
            // 
            this.boardIDLabel.Location = new System.Drawing.Point(16, 24);
            this.boardIDLabel.Name = "boardIDLabel";
            this.boardIDLabel.Size = new System.Drawing.Size(112, 16);
            this.boardIDLabel.TabIndex = 8;
            this.boardIDLabel.Text = "Board ID:";
            // 
            // notifyDataGroupBox
            // 
            this.notifyDataGroupBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
                | System.Windows.Forms.AnchorStyles.Right)));
            this.notifyDataGroupBox.Controls.Add(this.notifyCountLabel);
            this.notifyDataGroupBox.Controls.Add(this.notifyStatusTextBox);
            this.notifyDataGroupBox.Controls.Add(this.notifyStatusLabel);
            this.notifyDataGroupBox.Controls.Add(this.notifyCountTextBox);
            this.notifyDataGroupBox.Location = new System.Drawing.Point(344, 160);
            this.notifyDataGroupBox.Name = "notifyDataGroupBox";
            this.notifyDataGroupBox.Size = new System.Drawing.Size(152, 144);
            this.notifyDataGroupBox.TabIndex = 24;
            this.notifyDataGroupBox.TabStop = false;
            this.notifyDataGroupBox.Text = "Notify Data";
            // 
            // communicationGroupBox
            // 
            this.communicationGroupBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
                | System.Windows.Forms.AnchorStyles.Left) 
                | System.Windows.Forms.AnchorStyles.Right)));
            this.communicationGroupBox.Controls.Add(this.clearOutputButton);
            this.communicationGroupBox.Controls.Add(this.stringReadLabel);
            this.communicationGroupBox.Controls.Add(this.stringReadTextBox);
            this.communicationGroupBox.Controls.Add(this.stringToWriteLabel);
            this.communicationGroupBox.Controls.Add(this.writeButton);
            this.communicationGroupBox.Controls.Add(this.stringToWriteTextBox);
            this.communicationGroupBox.Location = new System.Drawing.Point(8, 160);
            this.communicationGroupBox.Name = "communicationGroupBox";
            this.communicationGroupBox.Size = new System.Drawing.Size(328, 216);
            this.communicationGroupBox.TabIndex = 25;
            this.communicationGroupBox.TabStop = false;
            this.communicationGroupBox.Text = "Communication";
            // 
            // clearOutputButton
            // 
            this.clearOutputButton.Location = new System.Drawing.Point(96, 64);
            this.clearOutputButton.Name = "clearOutputButton";
            this.clearOutputButton.Size = new System.Drawing.Size(80, 24);
            this.clearOutputButton.TabIndex = 13;
            this.clearOutputButton.Text = "Clear Output";
            this.clearOutputButton.Click += new System.EventHandler(this.clearOutputButton_Click);
            // 
            // notifyControlGroupBox
            // 
            this.notifyControlGroupBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
                | System.Windows.Forms.AnchorStyles.Right)));
            this.notifyControlGroupBox.Controls.Add(this.notifyOnDSRButton);
            this.notifyControlGroupBox.Controls.Add(this.reenableMaskComboBox);
            this.notifyControlGroupBox.Controls.Add(this.reenableMaskLabel);
            this.notifyControlGroupBox.Location = new System.Drawing.Point(210, 8);
            this.notifyControlGroupBox.Name = "notifyControlGroupBox";
            this.notifyControlGroupBox.Size = new System.Drawing.Size(286, 144);
            this.notifyControlGroupBox.TabIndex = 26;
            this.notifyControlGroupBox.TabStop = false;
            this.notifyControlGroupBox.Text = "Notify Control";
            // 
            // MainForm
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.ClientSize = new System.Drawing.Size(504, 397);
            this.Controls.Add(this.notifyControlGroupBox);
            this.Controls.Add(this.communicationGroupBox);
            this.Controls.Add(this.notifyDataGroupBox);
            this.Controls.Add(this.deviceGroupBox);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MinimumSize = new System.Drawing.Size(512, 424);
            this.Name = "MainForm";
            this.Text = "Using Notify";
            this.deviceGroupBox.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.padNumericUpDown)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.boardIDNumericUpDown)).EndInit();
            this.notifyDataGroupBox.ResumeLayout(false);
            this.communicationGroupBox.ResumeLayout(false);
            this.notifyControlGroupBox.ResumeLayout(false);
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

        private string ReplaceCommonEscapeSequences(string s)
        {
            return s.Replace("\\n", "\n").Replace("\\r", "\r");
        }

        private string InsertCommonEscapeSequences(string s)
        {
            return s.Replace("\n", "\\n").Replace("\r", "\\r");
        }

        private void SetupControlState(bool deviceOpen)
        {
            boardIDNumericUpDown.Enabled = !deviceOpen;
            padNumericUpDown.Enabled = !deviceOpen;
            sadComboBox.Enabled = !deviceOpen;
            openButton.Enabled = !deviceOpen;
            closeButton.Enabled = deviceOpen;
            notifyControlGroupBox.Enabled = deviceOpen;
            communicationGroupBox.Enabled = deviceOpen;
            notifyDataGroupBox.Enabled = deviceOpen;
        }

        private void ClearOutput()
        {
            stringReadTextBox.Text = string.Empty;
            notifyStatusTextBox.Text = string.Empty;            
            notifyCountTextBox.Text = string.Empty;
        }

        private void openButton_Click(object sender, System.EventArgs e)
        {
            try
            {
                int currentSecondaryAddress = 0;
                if (sadComboBox.SelectedIndex != 0)
                {
                    currentSecondaryAddress = (int)sadComboBox.SelectedItem;
                }
                testDevice = new Device((int)boardIDNumericUpDown.Value,
                    (byte)padNumericUpDown.Value,
                    (byte)currentSecondaryAddress);
                SetupControlState(true);
                ClearOutput();
            }
            catch(Exception exp)
            {
                MessageBox.Show(exp.Message);
            }
        }

        private void closeButton_Click(object sender, System.EventArgs e)
        {
            try
            {
                testDevice.Dispose();
                SetupControlState(false);
            }
            catch(Exception exp)
            {
                MessageBox.Show(exp.Message);
            }
        }

        private void writeButton_Click(object sender, System.EventArgs e)
        {
            try
            {
                testDevice.Write(ReplaceCommonEscapeSequences(stringToWriteTextBox.Text));
            }
            catch(Exception exp)
            {
                MessageBox.Show(exp.Message);
            }
        }

        private void notifyOnDSRButton_Click(object sender, System.EventArgs e)
        {
            try
            {
                testDevice.Notify(GpibStatusFlags.DeviceServiceRequest, new NotifyCallback(testDevice_Notify), "Sample user data");
            }
            catch(Exception exp)
            {
                MessageBox.Show(exp.Message);
            }
        }

        private void testDevice_Notify(object sender, NotifyData e)
        {
            try
            {
                testDevice.SerialPoll();
				string reenableMask = (string) Invoke(notifyUpdateStatusHandler, new object[] { InsertCommonEscapeSequences(testDevice.ReadString()), e.Status.ToString(), e.Count.ToString() });
                e.SetReenableMask((GpibStatusFlags)Enum.Parse(typeof(GpibStatusFlags), reenableMask));
            }
            catch(Exception exp)
            {
                MessageBox.Show(exp.Message);
            }
        }

		private string NotifyUpdateStatus(string readText, string status, string count)
		{
			stringReadTextBox.Text = readText;
			notifyStatusTextBox.Text = status;                
			notifyCountTextBox.Text = count;

			return reenableMaskComboBox.Text;
		}

        private void clearOutputButton_Click(object sender, System.EventArgs e)
        {
            ClearOutput();
        }
	}
}
