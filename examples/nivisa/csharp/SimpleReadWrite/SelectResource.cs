using System.Globalization;
using System.Windows.Forms;
using NationalInstruments.Visa;

namespace NationalInstruments.Examples.SimpleReadWrite
{
    /// <summary>
    /// Summary description for SelectResource.
    /// </summary>
    public class SelectResource : System.Windows.Forms.Form
    {
        private System.Windows.Forms.ListBox _availableResourcesListBox;
        private System.Windows.Forms.Button _okButton;
        private System.Windows.Forms.Button _closeButton;
        private System.Windows.Forms.TextBox _visaResourceNameTextBox;
        private System.Windows.Forms.Label _availableResourcesLabel;
        private System.Windows.Forms.Label _resourceStringLabel;
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.Container _components = null;

        public SelectResource()
        {
            //
            // Required for Windows Form Designer support
            //
            InitializeComponent();
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
                if (_availableResourcesListBox != null)
                {
                    _availableResourcesListBox.Dispose();
                }
                if (_okButton != null)
                {
                    _okButton.Dispose();
                }
                if (_closeButton != null)
                {
                    _closeButton.Dispose();
                }
                if (_visaResourceNameTextBox != null)
                {
                    _visaResourceNameTextBox.Dispose();
                }
                if (_availableResourcesLabel != null)
                {
                    _availableResourcesLabel.Dispose();
                }
                if (_resourceStringLabel != null)
                {
                    _resourceStringLabel.Dispose();
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
            System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(SelectResource));
            this._availableResourcesListBox = new System.Windows.Forms.ListBox();
            this._okButton = new System.Windows.Forms.Button();
            this._closeButton = new System.Windows.Forms.Button();
            this._visaResourceNameTextBox = new System.Windows.Forms.TextBox();
            this._availableResourcesLabel = new System.Windows.Forms.Label();
            this._resourceStringLabel = new System.Windows.Forms.Label();
            this.SuspendLayout();
            //
            // _availableResourcesListBox
            //
            this._availableResourcesListBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                | System.Windows.Forms.AnchorStyles.Left)
                | System.Windows.Forms.AnchorStyles.Right)));
            this._availableResourcesListBox.Location = new System.Drawing.Point(5, 18);
            this._availableResourcesListBox.Name = "_availableResourcesListBox";
            this._availableResourcesListBox.Size = new System.Drawing.Size(282, 108);
            this._availableResourcesListBox.TabIndex = 0;
            this._availableResourcesListBox.DoubleClick += new System.EventHandler(this.AvailableResourcesListBox_DoubleClick);
            this._availableResourcesListBox.SelectedIndexChanged += new System.EventHandler(this.AvailableResourcesListBox_SelectedIndexChanged);
            //
            // _okButton
            //
            this._okButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this._okButton.DialogResult = System.Windows.Forms.DialogResult.OK;
            this._okButton.Location = new System.Drawing.Point(5, 187);
            this._okButton.Name = "_okButton";
            this._okButton.Size = new System.Drawing.Size(77, 25);
            this._okButton.TabIndex = 2;
            this._okButton.Text = "OK";
            //
            // _closeButton
            //
            this._closeButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this._closeButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this._closeButton.Location = new System.Drawing.Point(82, 187);
            this._closeButton.Name = "_closeButton";
            this._closeButton.Size = new System.Drawing.Size(77, 25);
            this._closeButton.TabIndex = 3;
            this._closeButton.Text = "Cancel";
            //
            // _visaResourceNameTextBox
            //
            this._visaResourceNameTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
                | System.Windows.Forms.AnchorStyles.Right)));
            this._visaResourceNameTextBox.Location = new System.Drawing.Point(5, 157);
            this._visaResourceNameTextBox.Name = "_visaResourceNameTextBox";
            this._visaResourceNameTextBox.Size = new System.Drawing.Size(282, 20);
            this._visaResourceNameTextBox.TabIndex = 4;
            this._visaResourceNameTextBox.Text = "GPIB0::2::INSTR";
            //
            // _availableResourcesLabel
            //
            this._availableResourcesLabel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                | System.Windows.Forms.AnchorStyles.Right)));
            this._availableResourcesLabel.Location = new System.Drawing.Point(5, 5);
            this._availableResourcesLabel.Name = "_availableResourcesLabel";
            this._availableResourcesLabel.Size = new System.Drawing.Size(279, 12);
            this._availableResourcesLabel.TabIndex = 5;
            this._availableResourcesLabel.Text = "Available Resources:";
            //
            // _resourceStringLabel
            //
            this._resourceStringLabel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
                | System.Windows.Forms.AnchorStyles.Right)));
            this._resourceStringLabel.Location = new System.Drawing.Point(5, 141);
            this._resourceStringLabel.Name = "_resourceStringLabel";
            this._resourceStringLabel.Size = new System.Drawing.Size(279, 13);
            this._resourceStringLabel.TabIndex = 6;
            this._resourceStringLabel.Text = "Resource String:";
            //
            // SelectResource
            //
            this.AcceptButton = this._okButton;
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.CancelButton = this._closeButton;
            this.ClientSize = new System.Drawing.Size(292, 220);
            this.Controls.Add(this._resourceStringLabel);
            this.Controls.Add(this._availableResourcesLabel);
            this.Controls.Add(this._visaResourceNameTextBox);
            this.Controls.Add(this._closeButton);
            this.Controls.Add(this._okButton);
            this.Controls.Add(this._availableResourcesListBox);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon", CultureInfo.CurrentCulture)));
            this.KeyPreview = true;
            this.MaximizeBox = false;
            this.MinimumSize = new System.Drawing.Size(177, 247);
            this.Name = "SelectResource";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Select Resource";
            this.Load += new System.EventHandler(this.OnLoad);
            this.ResumeLayout(false);
        }
        #endregion

        private void OnLoad(object sender, System.EventArgs e)
        {
            // This example uses an instance of the NationalInstruments.Visa.ResourceManager class to find resources on the system.
            // Alternatively, static methods provided by the Ivi.Visa.ResourceManager class may be used when an application
            // requires additional VISA .NET implementations.
            using (var rmSession = new ResourceManager())
            {
                var resources = rmSession.Find("(ASRL|GPIB|TCPIP|USB)?*");
                foreach (string s in resources)
                {
                    _availableResourcesListBox.Items.Add(s);
                }
            }
        }

        private void AvailableResourcesListBox_DoubleClick(object sender, System.EventArgs e)
        {
            string selectedString = (string)_availableResourcesListBox.SelectedItem;
            ResourceName = selectedString;
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void AvailableResourcesListBox_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            string selectedString = (string)_availableResourcesListBox.SelectedItem;
            ResourceName = selectedString;
        }

        public string ResourceName
        {
            get
            {
                return _visaResourceNameTextBox.Text;
            }
            set
            {
                _visaResourceNameTextBox.Text = value;
            }
        }
    }
}