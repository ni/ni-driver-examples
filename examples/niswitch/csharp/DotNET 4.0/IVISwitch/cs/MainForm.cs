using System;
using System.Windows.Forms;
using Ivi.Swtch;

namespace NationalInstruments.Examples.IviSwitch
{
    public partial class MainForm : Form
    {
        IIviSwtch switchSession;

        public MainForm()
        {
            InitializeComponent();
        }
        #region Program Properties
        private string ResourceName
        {
            get
            {
                return this.resourceNameTextBox.Text;
            }
        }

        private bool IDQuery
        {
            get
            {
                return this.idQueryCheckBox.Checked;
            }
        }
        private bool ResetDevice
        {
            get
            {
                return this.resetDeviceCheckBox.Checked;
            }
        }
        #endregion Program Properties

        #region FormEvents

       
        private void initializeButton_Click(object sender, EventArgs e)
        {
            try
            {

                switchSession = IviSwtch.Create(ResourceName, IDQuery, ResetDevice);
                
                ConfigureIviSwitch X = new ConfigureIviSwitch(switchSession);
                switchSession.Path.CanConnect("ab0", "ch0");
                X.Show();
            }
            catch (Exception ex)
            {
                ShowError(ex.Message);
                CloseSession();
            }
          


        }
        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            CloseSession();
        }
        #endregion FormEvents

        private void CloseSession()
        {
            if (switchSession != null)
            {
                try
                {
                    switchSession.Close();
                    switchSession = null;
                }
                catch (System.Exception ex)
                {
                    ShowError("Unable to Close Session, Reset the device.\n" + "Error : " + ex.Message);
                    Application.Exit();
                }
            }
        }
       
        private static void ShowError(string message)
        {
            if (string.IsNullOrEmpty(message))
                message = "Unexpected Error";
            MessageBox.Show(message, "Error"); ;
        }
    }
}
