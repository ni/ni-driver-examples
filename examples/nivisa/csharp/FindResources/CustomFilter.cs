using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Globalization;
using System.Windows.Forms;

namespace NationalInstruments.Examples.FindResources
{
    /// <summary>
    /// CustomFilterForm lets the user enter the custom filter string to
    /// find the available resources with. Public property CustomFilter
    /// returns the custom filter string.
    /// </summary>
    public class CustomFilterForm : System.Windows.Forms.Form
    {
        private System.Windows.Forms.Label _customFilterLabel;
        private System.Windows.Forms.TextBox _customFilterTextBox;
        private System.Windows.Forms.Button _okButton;
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.Container _components = null;

        public CustomFilterForm()
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
                if (_customFilterLabel != null)
                {
                    _customFilterLabel.Dispose();
                }
                if (_customFilterTextBox != null)
                {
                    _customFilterTextBox.Dispose();
                }
                if (_okButton != null)
                {
                    _okButton.Dispose();
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
            System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(CustomFilterForm));
            this._customFilterTextBox = new System.Windows.Forms.TextBox();
            this._customFilterLabel = new System.Windows.Forms.Label();
            this._okButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            //
            // _customFilterTextBox
            //
            this._customFilterTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                | System.Windows.Forms.AnchorStyles.Right)));
            this._customFilterTextBox.Location = new System.Drawing.Point(16, 24);
            this._customFilterTextBox.Name = "_customFilterTextBox";
            this._customFilterTextBox.Size = new System.Drawing.Size(152, 20);
            this._customFilterTextBox.TabIndex = 0;
            this._customFilterTextBox.Text = "?*";
            //
            // _customFilterLabel
            //
            this._customFilterLabel.Location = new System.Drawing.Point(16, 8);
            this._customFilterLabel.Name = "_customFilterLabel";
            this._customFilterLabel.Size = new System.Drawing.Size(144, 16);
            this._customFilterLabel.TabIndex = 1;
            this._customFilterLabel.Text = "Enter Custom Filter String:";
            //
            // _okButton
            //
            this._okButton.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this._okButton.Location = new System.Drawing.Point(56, 56);
            this._okButton.Name = "_okButton";
            this._okButton.TabIndex = 2;
            this._okButton.Text = "OK";
            this._okButton.Click += new System.EventHandler(this.OkButton_Click);
            //
            // CustomFilterForm
            //
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.ClientSize = new System.Drawing.Size(184, 78);
            this.Controls.Add(this._okButton);
            this.Controls.Add(this._customFilterLabel);
            this.Controls.Add(this._customFilterTextBox);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon", CultureInfo.CurrentCulture)));
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(384, 112);
            this.MinimumSize = new System.Drawing.Size(192, 112);
            this.Name = "CustomFilterForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Custom Filter";
            this.ResumeLayout(false);
        }
        #endregion

        private void OkButton_Click(object sender, System.EventArgs e)
        {
            this.Close();
        }

        public string CustomFilter
        {
            get
            {
                return _customFilterTextBox.Text;
            }
        }
    }
}