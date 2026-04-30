// ==================================================================================================
//
// Title      : MainForm.cs
// Purpose    : This application shows the user how to use ResourceManager to
//              find all of the available resources on their system. In the example,
//              they can select between several filters to narrow the list. Public
//              property ResourceName contains the resource name selected in tvwResourceTree
//
// ==================================================================================================

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Windows.Forms;
using Ivi.Visa;
using NationalInstruments.Visa;

namespace NationalInstruments.Examples.FindResources
{
    /// <summary>
    /// This application shows the user how to use ResourceManager to
    /// find all of the available resources on their system.  In the
    /// example, they can select between several filters to narrow the
    /// list.
    /// </summary>
    public class MainForm : System.Windows.Forms.Form
    {
        private TreeNode _ndGPIB;
        private TreeNode _ndVXI;
        private TreeNode _ndSerial;
        private TreeNode _ndPXI;
        private TreeNode _ndTCPIP;
        private TreeNode _ndUSB;

        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.Container _components = null;

        private string _filter;
        private System.Windows.Forms.Button _useCustomStringButton;
        private System.Windows.Forms.Label _filterStringLabel;
        private System.Windows.Forms.Button _findResourcesButton;
        private System.Windows.Forms.Button _clearButton;
        private System.Windows.Forms.Label _availableResourcesLabel;
        private System.Windows.Forms.ListBox _filterStringsListBox;
        private System.Windows.Forms.TreeView _resourceTreeView;

        public MainForm()
        {
            //
            // Required for Windows Form Designer support
            //
            InitializeComponent();

            _ndGPIB = new TreeNode("GPIB");
            _ndVXI = new TreeNode("VXI");
            _ndSerial = new TreeNode("Serial");
            _ndPXI = new TreeNode("PXI");
            _ndTCPIP = new TreeNode("TCP/IP");
            _ndUSB = new TreeNode("USB");
            CleanResourceNodes();

            PopulateFilterList();
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
                if (_resourceTreeView != null)
                {
                    _resourceTreeView.Dispose();
                }
                if (_filterStringLabel != null)
                {
                    _filterStringLabel.Dispose();
                }
                if (_filterStringsListBox != null)
                {
                    _filterStringsListBox.Dispose();
                }
                if (_availableResourcesLabel != null)
                {
                    _availableResourcesLabel.Dispose();
                }
                if (_clearButton != null)
                {
                    _clearButton.Dispose();
                }
                if (_useCustomStringButton != null)
                {
                    _useCustomStringButton.Dispose();
                }
                if (_findResourcesButton != null)
                {
                    _findResourcesButton.Dispose();
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
            this._availableResourcesLabel = new System.Windows.Forms.Label();
            this._resourceTreeView = new System.Windows.Forms.TreeView();
            this._findResourcesButton = new System.Windows.Forms.Button();
            this._filterStringsListBox = new System.Windows.Forms.ListBox();
            this._filterStringLabel = new System.Windows.Forms.Label();
            this._clearButton = new System.Windows.Forms.Button();
            this._useCustomStringButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            //
            // _availableResourcesLabel
            //
            this._availableResourcesLabel.Location = new System.Drawing.Point(16, 213);
            this._availableResourcesLabel.Name = "_availableResourcesLabel";
            this._availableResourcesLabel.Size = new System.Drawing.Size(152, 16);
            this._availableResourcesLabel.TabIndex = 0;
            this._availableResourcesLabel.Text = "Available Resources Found:";
            //
            // _resourceTreeView
            //
            this._resourceTreeView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
            | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this._resourceTreeView.Location = new System.Drawing.Point(16, 232);
            this._resourceTreeView.Name = "_resourceTreeView";
            this._resourceTreeView.Size = new System.Drawing.Size(248, 136);
            this._resourceTreeView.TabIndex = 5;
            //
            // _findResourcesButton
            //
            this._findResourcesButton.Location = new System.Drawing.Point(16, 168);
            this._findResourcesButton.Name = "_findResourcesButton";
            this._findResourcesButton.Size = new System.Drawing.Size(130, 23);
            this._findResourcesButton.TabIndex = 8;
            this._findResourcesButton.Text = "Find Resources";
            this._findResourcesButton.Click += new System.EventHandler(this.FindResourcesButton_Click);
            //
            // _filterStringsListBox
            //
            this._filterStringsListBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this._filterStringsListBox.Location = new System.Drawing.Point(16, 40);
            this._filterStringsListBox.Name = "_filterStringsListBox";
            this._filterStringsListBox.Size = new System.Drawing.Size(248, 121);
            this._filterStringsListBox.TabIndex = 9;
            //
            // _filterStringLabel
            //
            this._filterStringLabel.Location = new System.Drawing.Point(16, 24);
            this._filterStringLabel.Name = "_filterStringLabel";
            this._filterStringLabel.Size = new System.Drawing.Size(72, 16);
            this._filterStringLabel.TabIndex = 10;
            this._filterStringLabel.Text = "Filter String:";
            //
            // _clearButton
            //
            this._clearButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this._clearButton.Location = new System.Drawing.Point(152, 168);
            this._clearButton.Name = "_clearButton";
            this._clearButton.Size = new System.Drawing.Size(112, 24);
            this._clearButton.TabIndex = 11;
            this._clearButton.Text = "Clear";
            this._clearButton.Click += new System.EventHandler(this.ClearButton_Click);
            //
            // _useCustomStringButton
            //
            this._useCustomStringButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this._useCustomStringButton.Location = new System.Drawing.Point(152, 8);
            this._useCustomStringButton.Name = "_useCustomStringButton";
            this._useCustomStringButton.Size = new System.Drawing.Size(112, 24);
            this._useCustomStringButton.TabIndex = 12;
            this._useCustomStringButton.Text = "Use Custom String";
            this._useCustomStringButton.Click += new System.EventHandler(this.UseCustomStringButton_Click);
            //
            // MainForm
            //
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.ClientSize = new System.Drawing.Size(280, 373);
            this.Controls.Add(this._useCustomStringButton);
            this.Controls.Add(this._clearButton);
            this.Controls.Add(this._filterStringLabel);
            this.Controls.Add(this._filterStringsListBox);
            this.Controls.Add(this._findResourcesButton);
            this.Controls.Add(this._resourceTreeView);
            this.Controls.Add(this._availableResourcesLabel);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon", CultureInfo.CurrentCulture)));
            this.MaximizeBox = false;
            this.MinimumSize = new System.Drawing.Size(288, 400);
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Available Resouces List";
            this.ResumeLayout(false);
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

        private void PopulateFilterList()
        {
            _filterStringsListBox.Items.Clear();
            _filterStringsListBox.Items.Add("?*");
            _filterStringsListBox.Items.Add("ASRL?*INSTR");
            _filterStringsListBox.Items.Add("GPIB?*");
            _filterStringsListBox.Items.Add("GPIB?*INSTR");
            _filterStringsListBox.Items.Add("GPIB?*INTFC");
            _filterStringsListBox.Items.Add("PXI?*");
            _filterStringsListBox.Items.Add("PXI?*BACKPLANE");
            _filterStringsListBox.Items.Add("PXI?*INSTR");
            _filterStringsListBox.Items.Add("TCPIP?*");
            _filterStringsListBox.Items.Add("TCPIP?*INSTR");
            _filterStringsListBox.Items.Add("TCPIP?*SOCKET");
            _filterStringsListBox.Items.Add("USB?*");
            _filterStringsListBox.Items.Add("USB?*INSTR");
            _filterStringsListBox.Items.Add("USB?*RAW");
            _filterStringsListBox.Items.Add("VXI?*");
            _filterStringsListBox.Items.Add("VXI?*BACKPLANE");
            _filterStringsListBox.Items.Add("VXI?*INSTR");
            _filterStringsListBox.SelectedIndex = 0;
        }

        private void AddToResourceTree()
        {
            if (_ndGPIB.Nodes.Count != 0)
            {
                _resourceTreeView.Nodes.Add(_ndGPIB);
            }
            if (_ndVXI.Nodes.Count != 0)
            {
                _resourceTreeView.Nodes.Add(_ndVXI);
            }
            if (_ndSerial.Nodes.Count != 0)
            {
                _resourceTreeView.Nodes.Add(_ndSerial);
            }
            if (_ndPXI.Nodes.Count != 0)
            {
                _resourceTreeView.Nodes.Add(_ndPXI);
            }
            if (_ndTCPIP.Nodes.Count != 0)
            {
                _resourceTreeView.Nodes.Add(_ndTCPIP);
            }
            if (_ndUSB.Nodes.Count != 0)
            {
                _resourceTreeView.Nodes.Add(_ndUSB);
            }
        }

        private void AddToResourceNode(string resourceName, HardwareInterfaceType intType)
        {
            switch (intType)
            {
                case HardwareInterfaceType.Gpib:
                    _ndGPIB.Nodes.Add(new TreeNode(resourceName));
                    break;
                case HardwareInterfaceType.Vxi:
                    _ndVXI.Nodes.Add(new TreeNode(resourceName));
                    break;
                case HardwareInterfaceType.Serial:
                    _ndSerial.Nodes.Add(new TreeNode(resourceName));
                    break;
                case HardwareInterfaceType.Pxi:
                    _ndPXI.Nodes.Add(new TreeNode(resourceName));
                    break;
                case HardwareInterfaceType.Tcp:
                    _ndTCPIP.Nodes.Add(new TreeNode(resourceName));
                    break;
                case HardwareInterfaceType.Usb:
                    _ndUSB.Nodes.Add(new TreeNode(resourceName));
                    break;
                default:
                    break;
            }
        }

        private void FindResources()
        {
            // This example uses an instance of the NationalInstruments.Visa.ResourceManager class to find resources on the system.
            // Alternatively, static methods provided by the Ivi.Visa.ResourceManager class may be used when an application
            // requires additional VISA .NET implementations.
            using (var rm = new ResourceManager())
            {
                try
                {
                    IEnumerable<string> resources = rm.Find(_filter);
                    foreach (string s in resources)
                    {
                        ParseResult parseResult = rm.Parse(s);
                        AddToResourceNode(s, parseResult.InterfaceType);
                    }
                    AddToResourceTree();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        private void CleanResourceNodes()
        {
            _ndGPIB.Nodes.Clear();
            _ndVXI.Nodes.Clear();
            _ndSerial.Nodes.Clear();
            _ndPXI.Nodes.Clear();
            _ndTCPIP.Nodes.Clear();
            _ndUSB.Nodes.Clear();
        }

        private void FindResourcesButton_Click(object sender, System.EventArgs e)
        {
            _filter = _filterStringsListBox.Text;
            DisplayResources();
        }

        private string GetCustomFilter()
        {
            CustomFilterForm customFilterForm = new CustomFilterForm();
            customFilterForm.ShowDialog();
            return customFilterForm.CustomFilter;
        }

        private void ClearButton_Click(object sender, System.EventArgs e)
        {
            _resourceTreeView.Nodes.Clear();
            CleanResourceNodes();
        }

        private void UseCustomStringButton_Click(object sender, System.EventArgs e)
        {
            _filter = GetCustomFilter();
            DisplayResources();
        }

        private void DisplayResources()
        {
            _resourceTreeView.Nodes.Clear();
            CleanResourceNodes();
            FindResources();
            _resourceTreeView.ExpandAll();
        }

        public string ResourceName
        {
            get
            {
                try
                {
                    return _resourceTreeView.SelectedNode.Text;
                }
                catch (Exception)
                {
                    return string.Empty;
                }
            }
        }
    }
}