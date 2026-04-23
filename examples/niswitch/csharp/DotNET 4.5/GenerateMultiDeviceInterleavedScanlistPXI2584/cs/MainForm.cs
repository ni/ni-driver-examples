//==================================================================================================
// Title        :  	Generate Multi Device Interleaved Scanlist
// Description  : This example explains how to generate interleaved scan list for the device NI 2584. The example first checks if the start and end channels specified by the user is valid. The first entry of the scan list intermediate entry and last entry for the scan list are made in order to get the final interleaved scan list.
//==================================================================================================
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using System.Collections;
using System.Text;

namespace NationalInstruments.Examples.GenerateMultiDeviceInterleavedScanlistPxi2584
{
    public partial class MainForm : Form
    {
        InterleavedLookupTable[] InterleavedEndChannel2584LookupTable;
        public MainForm()
        {
            InitializeComponent();
            InitializeLookupTable();
        }

        private struct InterleavedLookupTable
        {
            private string firstConnection;
            private string secondConnection;
            public string FirstConnection
            {
                get
                {
                    return firstConnection;
                }
                set
                {
                    firstConnection = value;
                }
            }
            public string SecondConnection
            {
                get
                {
                    return secondConnection;
                }
                set
                {
                    secondConnection = value;
                }
            }

        }

        private void InitializeLookupTable()
        {
            string[] lookupStrings = { "ch0->com0", "ch6->com1", "ch1->com0", "ch7->com1", "ch2->com0", "ch8->com1", "ch3->com0", "ch9->com1", "ch4->com0", "ch10->com1", "ch5->com0", "ch11->com1" };
            InterleavedEndChannel2584LookupTable = new InterleavedLookupTable[11];

            InterleavedEndChannel2584LookupTable[0].FirstConnection = lookupStrings[0];
            InterleavedEndChannel2584LookupTable[0].SecondConnection = lookupStrings[1];

            InterleavedEndChannel2584LookupTable[1].FirstConnection = lookupStrings[1];
            InterleavedEndChannel2584LookupTable[1].SecondConnection = lookupStrings[2];

            InterleavedEndChannel2584LookupTable[2].FirstConnection = lookupStrings[2];
            InterleavedEndChannel2584LookupTable[2].SecondConnection = lookupStrings[3];

            InterleavedEndChannel2584LookupTable[3].FirstConnection = lookupStrings[3];
            InterleavedEndChannel2584LookupTable[3].SecondConnection = lookupStrings[4];

            InterleavedEndChannel2584LookupTable[4].FirstConnection = lookupStrings[4];
            InterleavedEndChannel2584LookupTable[4].SecondConnection = lookupStrings[5];

            InterleavedEndChannel2584LookupTable[5].FirstConnection = lookupStrings[5];
            InterleavedEndChannel2584LookupTable[5].SecondConnection = lookupStrings[6];

            InterleavedEndChannel2584LookupTable[6].FirstConnection = lookupStrings[6];
            InterleavedEndChannel2584LookupTable[6].SecondConnection = lookupStrings[7];

            InterleavedEndChannel2584LookupTable[7].FirstConnection = lookupStrings[7];
            InterleavedEndChannel2584LookupTable[7].SecondConnection = lookupStrings[8];

            InterleavedEndChannel2584LookupTable[8].FirstConnection = lookupStrings[8];
            InterleavedEndChannel2584LookupTable[8].SecondConnection = lookupStrings[9];

            InterleavedEndChannel2584LookupTable[9].FirstConnection = lookupStrings[9];
            InterleavedEndChannel2584LookupTable[9].SecondConnection = lookupStrings[10];

            InterleavedEndChannel2584LookupTable[10].FirstConnection = lookupStrings[10];
            InterleavedEndChannel2584LookupTable[10].SecondConnection = lookupStrings[11];
        }

        #region Program Properties
        private int InterleavedStartChannel
        {
            get
            {
                return (int)this.interleavedStartChannelNumericUpDown.Value;
            }
        }

        private int InterleavedEndChannel
        {
            get
            {
                return (int)this.interleavedEndChannelNumericUpDown.Value;
            }
        }



        #endregion Program Properties

        private void generateButton_Click(object sender, EventArgs e)
        {
            try
            {
                if (IsValidValue())
                {
                    StringBuilder interleavedScanlist = new StringBuilder();
                    CreateFirstEntry(interleavedScanlist);
                    if (InterleavedEndChannel > InterleavedStartChannel)
                    {
                        int i = 0;
                        int j;
                        int k = InterleavedStartChannel;
                        do
                        {
                            j = InterleavedStartChannel + i + 1;
                            CreateIntermediateEntry(interleavedScanlist, j, k);
                            k = j;
                            i++;
                        } while (j != InterleavedEndChannel);
                    }
                    CreateLastEntry(interleavedScanlist);
                    interleavedScanListRichTextBox.Text = interleavedScanlist.ToString();
                }
                else
                {
                    if (!CheckForNotNull())
                        ShowError("The input fields for Interleaved Start or End Channel should not be blank");
                    else if (!CheckStartChannelEndChannelRelation())
                        ShowError("Interleaved start channel must be smaller or equal to interleaved end channel on all devices.");
                    else if (!CheckValidChannelValues())
                        ShowError("Valid channel numbers for the PXI-2584 interleaved scanning are between 0 and 10.");
                }
            }
            catch (System.Exception ex)
            {
                ShowError(ex.Message);
            }
        }
        private static void ShowError(string message)
        {
            if (string.IsNullOrEmpty(message))
                message = "Unexpected Error";
            MessageBox.Show(message, "Error"); ;
        }
        private void CreateIntermediateEntry(StringBuilder interleavedScanlist, int j, int k)
        {
            interleavedScanlist.Append("~");
            interleavedScanlist.Append(InterleavedEndChannel2584LookupTable[k].FirstConnection);
            interleavedScanlist.Append(" && ");
            interleavedScanlist.Append(InterleavedEndChannel2584LookupTable[j].SecondConnection);
            interleavedScanlist.Append(";");
        }

        private void CreateLastEntry(StringBuilder interleavedScanlist)
        {
            interleavedScanlist.Append(" ~");
            interleavedScanlist.Append(InterleavedEndChannel2584LookupTable[InterleavedEndChannel].FirstConnection);
            interleavedScanlist.Append(" &");
            interleavedScanlist.Append(" ~");
            interleavedScanlist.Append(InterleavedEndChannel2584LookupTable[InterleavedEndChannel].SecondConnection);
            interleavedScanlist.Append(" && ");
        }

        private void CreateFirstEntry(StringBuilder interleavedScanlist)
        {
            interleavedScanlist.Append(InterleavedEndChannel2584LookupTable[InterleavedStartChannel].FirstConnection);
           
            interleavedScanlist.Append(" & ");
            interleavedScanlist.Append(InterleavedEndChannel2584LookupTable[InterleavedStartChannel].SecondConnection);
           
            interleavedScanlist.Append(";");
        }

        private bool IsValidValue()
        {
            return (CheckForNotNull() && CheckStartChannelEndChannelRelation() && CheckValidChannelValues());
        }

        private bool CheckForNotNull()
        {
            if ((string.IsNullOrEmpty(((Control)this.interleavedEndChannelNumericUpDown).Text)) || (string.IsNullOrEmpty(((Control)this.interleavedStartChannelNumericUpDown).Text)))
            {
                return false;
            }
            else
                return true;
        }
        private bool CheckValidChannelValues()
        {
            return ((InterleavedEndChannel <= 10) && (InterleavedStartChannel >= 0));
        }

        private bool CheckStartChannelEndChannelRelation()
        {
            return (InterleavedStartChannel <= InterleavedEndChannel);
        }


    }


}
