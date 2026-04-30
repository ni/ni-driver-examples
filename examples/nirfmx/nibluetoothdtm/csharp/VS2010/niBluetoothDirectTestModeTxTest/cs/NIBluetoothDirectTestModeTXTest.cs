
using System;
using NationalInstruments.BluetoothDtm;

namespace NationalInstruments.Examples.NIBluetoothDtmTXTest
{
    public class NIBluetoothDtmTXTest
    {
        NIBluetoothDtm dtmSession;
        UInt16 dataBits;
        UInt32 baudRate;
        BluetoothDtmFlowControl flowControl;
        BluetoothDtmStopBits stopBits;
        BluetoothDtmParity parity;
        int status;
        int channelNumber;
        int payloadLength;
        int packetCount;
        BluetoothDtmLEPatternType lePatternType;
        string visaResourceName;

        public void Run()
        {
            try
            {
                InitializeVariables();
                InitializeDtmSession();
                ConfigureBlueToothDTM();
                //Add your Generation code here
                RetrieveResults();
                PrintResults();
            }
            catch (Exception e)
            {
                DisplayError(e.Message);
            }
            finally
            {
                CloseSession();
                Console.WriteLine("Press any key to exit");
                Console.ReadKey();
            }
        }
        private void InitializeVariables()
        {
            /* Initialize input variables */
            visaResourceName = "Com1";
            dataBits = 8;
            baudRate = 115200;
            flowControl = BluetoothDtmFlowControl.RtsCts;
            stopBits = BluetoothDtmStopBits.StopBits1_0;
            parity = BluetoothDtmParity.None;
            status = 10;
            channelNumber = 3;
            packetCount = 0;
            payloadLength = 37;
            lePatternType = BluetoothDtmLEPatternType.Type_Prbs9;
        }

        private void InitializeDtmSession()
        {
            /* Create a new Bluetooth DTM Session */
            dtmSession = new NIBluetoothDtm(visaResourceName);
        }

        private void ConfigureBlueToothDTM()
        {
            dtmSession.ConfigureVisaSerialSettings(dataBits, baudRate, flowControl, stopBits, parity);
            dtmSession.SetVisaTimeout(2000);
            dtmSession.HciReset(out status);
            dtmSession.HciLETransmitterTest(channelNumber, payloadLength, lePatternType, out status);
        }

        private void RetrieveResults()
        {
            dtmSession.HciLETestEnd(out packetCount, out status);
            dtmSession.HciReset(out status);      
        }

        private void PrintResults()
        {
            /* Display PacketCount*/
            Console.WriteLine("Number of packet count:  {0}\n", packetCount);
        }

        void CloseSession()
        {
            if (dtmSession != null)
            {
                try
                {
                    dtmSession.Close();
                    dtmSession = null;
                }
                catch (Exception e)
                {
                    DisplayError(e.Message);
                }

            }
        }

        void DisplayError(string errorMessage)
        {
            Console.WriteLine("ERROR: {0}", errorMessage);
        }


    }
}
