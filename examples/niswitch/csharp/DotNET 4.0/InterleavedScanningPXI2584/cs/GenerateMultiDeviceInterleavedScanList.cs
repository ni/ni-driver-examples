using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NationalInstruments.Examples.InterleavedScanningPxi2584
{
    public class GenerateMultiDeviceInterleavedScanList
    {
        InterleavedLookupTable[] InterleavedEndChannel2584LookupTable;
        string StartEndRelationError, ValidChannelValuesError;
        private int interleavedStartChannel, interleavedEndChannel;

        public void InitializeLookupTable()
        {
            string[] lookUpStrings = { "ch0->com0", "ch6->com1", "ch1->com0", "ch7->com1", "ch2->com0", "ch8->com1", "ch3->com0", "ch9->com1", "ch4->com0", "ch10->com1", "ch5->com0", "ch11->com1" };
            InterleavedEndChannel2584LookupTable = new InterleavedLookupTable[11];

            InterleavedEndChannel2584LookupTable[0].FirstConnection = lookUpStrings[0];
            InterleavedEndChannel2584LookupTable[0].SecondConnection = lookUpStrings[1];

            InterleavedEndChannel2584LookupTable[1].FirstConnection = lookUpStrings[1];
            InterleavedEndChannel2584LookupTable[1].SecondConnection = lookUpStrings[2];

            InterleavedEndChannel2584LookupTable[2].FirstConnection = lookUpStrings[2];
            InterleavedEndChannel2584LookupTable[2].SecondConnection = lookUpStrings[3];

            InterleavedEndChannel2584LookupTable[3].FirstConnection = lookUpStrings[3];
            InterleavedEndChannel2584LookupTable[3].SecondConnection = lookUpStrings[4];

            InterleavedEndChannel2584LookupTable[4].FirstConnection = lookUpStrings[4];
            InterleavedEndChannel2584LookupTable[4].SecondConnection = lookUpStrings[5];

            InterleavedEndChannel2584LookupTable[5].FirstConnection = lookUpStrings[5];
            InterleavedEndChannel2584LookupTable[5].SecondConnection = lookUpStrings[6];

            InterleavedEndChannel2584LookupTable[6].FirstConnection = lookUpStrings[6];
            InterleavedEndChannel2584LookupTable[6].SecondConnection = lookUpStrings[7];

            InterleavedEndChannel2584LookupTable[7].FirstConnection = lookUpStrings[7];
            InterleavedEndChannel2584LookupTable[7].SecondConnection = lookUpStrings[8];

            InterleavedEndChannel2584LookupTable[8].FirstConnection = lookUpStrings[8];
            InterleavedEndChannel2584LookupTable[8].SecondConnection = lookUpStrings[9];

            InterleavedEndChannel2584LookupTable[9].FirstConnection = lookUpStrings[9];
            InterleavedEndChannel2584LookupTable[9].SecondConnection = lookUpStrings[10];

            InterleavedEndChannel2584LookupTable[10].FirstConnection = lookUpStrings[10];
            InterleavedEndChannel2584LookupTable[10].SecondConnection = lookUpStrings[11];
        }

        public string Generate(int startChannel, int endChannel)
        {
            interleavedStartChannel = startChannel;
            interleavedEndChannel = endChannel;
            StartEndRelationError = "Interleaved start channel must be smaller or equal to interleaved end channel on all devices.";
            ValidChannelValuesError = "Valid channel numbers for the PXI-2584 interleaved scanning are between 0 and 10.";
           

            if (IsValidValue())
            {
                StringBuilder interleavedScanList = new StringBuilder();
                CreateFirstEntry(interleavedScanList);
                if (endChannel > startChannel)
                {
                    int i = 0;
                    int j;
                    int k = startChannel;
                    do
                    {
                        j = startChannel + i + 1;
                        CreateIntermediateEntry(interleavedScanList, j, k);
                        k = j;
                        i++;
                    } while (j != endChannel);
                }
                CreateLastEntry(interleavedScanList);
                return (interleavedScanList.ToString());
            }
            else
            {   
                if (!CheckStartChannelEndChannelRelation())
                   return StartEndRelationError;  
                else if (!CheckValidChannelValues())
                   return ValidChannelValuesError;
                else
                    return null;
            }
        }

        private bool IsValidValue()
        {
            return ( CheckStartChannelEndChannelRelation() && CheckValidChannelValues());
        }

        private bool CheckValidChannelValues()
        {
            return ((interleavedEndChannel <= 10) && (interleavedStartChannel >= 0));
        }

        private bool CheckStartChannelEndChannelRelation()
        {
            return (interleavedStartChannel <= interleavedEndChannel);
        }

        private void CreateIntermediateEntry(StringBuilder interleavedScanList, int j, int k)
        {
            interleavedScanList.Append("~");
            interleavedScanList.Append(InterleavedEndChannel2584LookupTable[k].FirstConnection);
            interleavedScanList.Append(" && ");
            interleavedScanList.Append(InterleavedEndChannel2584LookupTable[j].SecondConnection);
            interleavedScanList.Append(";");
        }

        private void CreateLastEntry(StringBuilder interleavedScanList)
        {
            interleavedScanList.Append(" ~");
            interleavedScanList.Append(InterleavedEndChannel2584LookupTable[interleavedEndChannel].FirstConnection);
            interleavedScanList.Append(" &");
            interleavedScanList.Append(" ~");
            interleavedScanList.Append(InterleavedEndChannel2584LookupTable[interleavedEndChannel].SecondConnection);
            interleavedScanList.Append(" && ");
        }

        private void CreateFirstEntry(StringBuilder interleavedScanList)
        {
            interleavedScanList.Append(InterleavedEndChannel2584LookupTable[interleavedStartChannel].FirstConnection);
            interleavedScanList.Append(" & ");
            interleavedScanList.Append(InterleavedEndChannel2584LookupTable[interleavedStartChannel].SecondConnection);
            interleavedScanList.Append(";");
        }
    }

    public struct InterleavedLookupTable
    {
        public string FirstConnection { get; set; }
        public string SecondConnection { get; set; }
    }
}
