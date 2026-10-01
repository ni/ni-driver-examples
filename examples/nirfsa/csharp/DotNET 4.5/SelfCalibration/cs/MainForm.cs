/*******************************************************************************
*
* Example program:
*   Rfsa Self Calibration
*
* Category:
*   NI-RFSA
*
* Description:
*   Use this example to learn how to do self calibration for the NI-RFSA.
                         

* Instructions for running:
*   1. Configure both RFSA device in the MAX for the program to run. 
*
*	2. Configure the Clock Source in the UI.
*   
*   3. Configure the Self Calibration Step Operation.
*
*   4. Select the Start Button in the UI to start the self-calibration.
*   
*   5. The data is displayed in the DataGrid.
*
* I/O Connections Overview:
*   Make sure your signal input terminals match the Physical Channel I/O
*   Controls.  If you have a PXI chassis, ensure that it has been properly
*   identified in MAX.  
*
*******************************************************************************/
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using NationalInstruments.ModularInstruments.NIRfsa;
using NationalInstruments.ModularInstruments.SystemServices.DeviceServices;

namespace SelfCalibration
{
    public partial class MainForm : Form
    {
        private NIRfsa rfsaSession;
        public MainForm()
        {
            InitializeComponent();
            LoadRfsaDeviceNames();
            ConfigureOutputTerminalComboBox();
            ConfigureSelfCalibrationStepOperation();
            indicatorLabel.BackColor = Color.Gray;
        }

        #region Initial Configuration

        private void LoadRfsaDeviceNames()
        {
            ModularInstrumentsSystem modularInstrumentsSystem = new ModularInstrumentsSystem("NI-RFSA");
            foreach (DeviceInfo device in modularInstrumentsSystem.DeviceCollection)
                resourceNameComboBox.Items.Add(device.Name);
            if (modularInstrumentsSystem.DeviceCollection.Count > 0)
                resourceNameComboBox.SelectedIndex = 0;
        }

        private void ConfigureOutputTerminalComboBox()
        {
            var clockSourceValueList = new List<KeyValuePair<string, RfsaReferenceClockSource>>();
            clockSourceValueList.Add(new KeyValuePair<string, RfsaReferenceClockSource>("OnboardClock", RfsaReferenceClockSource.OnboardClock));
            clockSourceValueList.Add(new KeyValuePair<string, RfsaReferenceClockSource>("RefIn", RfsaReferenceClockSource.ReferenceIn));
            clockSourceValueList.Add(new KeyValuePair<string, RfsaReferenceClockSource>("PXI_Clk", RfsaReferenceClockSource.PxiClock));
            clockSourceValueList.Add(new KeyValuePair<string, RfsaReferenceClockSource>("ClkIn", RfsaReferenceClockSource.ClockIn));
            clockSourceComboBox.DisplayMember = "Key";
            clockSourceComboBox.ValueMember = "Value";
            clockSourceComboBox.DataSource = clockSourceValueList;
            clockSourceComboBox.SelectedIndex = 0;
        }

        private void ConfigureSelfCalibrationStepOperation()
        {
            var selfCalibrationOperationValueList = new List<string>();
            selfCalibrationOperationValueList.Add("Perform All Self Calibration Step");
            selfCalibrationOperationValueList.Add("Perform Neccessary Self Calibration");
            selfCalibrationOperationValueList.Add("Omit IF Flatness Self Calibration");
            selfCalibrationComboBox.DataSource = selfCalibrationOperationValueList;
        }

        #endregion

        #region UI Gets

        private string ResourceName
        {
            get
            {
                return this.resourceNameComboBox.Text;
            }
        }

        public RfsaReferenceClockSource ReferenceClockSource
        {
            get
            {
                return this.clockSourceComboBox.SelectedValue as RfsaReferenceClockSource ?? RfsaReferenceClockSource.FromString(this.clockSourceComboBox.Text);
            }
        }

        private int SelfCalibrationOperation
        {
            get
            {
                return this.selfCalibrationComboBox.SelectedIndex;
            }
        }

        #endregion
        private void InitializeRfsaSession()
        {
            CloseSession();
            rfsaSession = new NIRfsa(ResourceName, true, false);
            rfsaSession.DriverOperation.Warning += new System.EventHandler<RfsaWarningEventArgs>(DriverOperationWarning);
        }

        private void DriverOperationWarning(object sender, RfsaWarningEventArgs e)
        {
            MessageBox.Show(e.Warning.ToString(), "Warning");
        }

        private void CloseSession()
        {
            if (rfsaSession != null)
            {
                try
                {
                    rfsaSession.Close();
                    rfsaSession = null;
                }
                catch (Exception ex)
                {
                    ShowError("Unable to Close Session, Reset the device.\n" + "Error : " + ex.Message);
                    Application.Exit();
                }
            }
        }

        private void ConfigureReferenceClock()
        {
            rfsaSession.Configuration.ReferenceClock.Source = ReferenceClockSource;
        }

        private void ConfigureSelfCalibration()
        {
            RfsaSelfCalibrationSteps validSteps;
            switch (SelfCalibrationOperation)
            {
                case 0:
                    //Passing an empty array will ensure NI-RFSA perform all Self Calibration steps.
                    rfsaSession.Calibration.Self.SelfCalibrate(RfsaSelfCalibrationSteps.None);
                    break;
                case 1:
                    // Queries NI-RFSA for the valid calibration steps.
                    // Passing this value to the niRFSA Self Cal VI ensures valid calibration steps are not repeated,
                    // thus only calibrating necessary steps
                    rfsaSession.Calibration.Self.IsSelfCalibrationValid(out validSteps);
                    rfsaSession.Calibration.Self.SelfCalibrate(validSteps);
                    break;
                case 2:
                    // IF Flatness Self Calibration can take up to 15 minutes.
                    rfsaSession.Calibration.Self.SelfCalibrate(RfsaSelfCalibrationSteps.IFFlatness);
                    break;
            }
        }

        private void ChangeControlState(bool state)
        {
            this.resourceNameComboBox.Enabled = state;
            this.clockSourceComboBox.Enabled = state;
            this.selfCalibrationComboBox.Enabled = state;
            this.startButton.Enabled = state;
        }

        private void startButton_Click(object sender, EventArgs e)
        {
            indicatorLabel.BackColor = Color.Gray;
            ChangeControlState(false);
            try
            {
                // Steps:
                //1. Open a new NI-RFSA session.
                //2. Configure the Reference clock.
                //3. Determine which self-calibration steps to omit.
                //4. Initiate self-calibration.
                //5. Close the NI-RFSA session.
                InitializeRfsaSession();
                ConfigureReferenceClock();
                ConfigureSelfCalibration();
                CloseSession();
                indicatorLabel.BackColor = Color.Green;
            }
            catch (Exception ex)
            {
                ShowError(ex.Message);
                CloseSession();
            }
            finally
            {
                ChangeControlState(true);
            }
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            CloseSession();
        }

        private static void ShowError(string message)
        {
            if (string.IsNullOrEmpty(message))
                message = "Unexpected Error";
            MessageBox.Show(message, "Error"); ;
        }

    }
}
