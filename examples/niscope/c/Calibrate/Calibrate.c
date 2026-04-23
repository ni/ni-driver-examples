/********************************************************************************
 * Calibrate Example, uses GenericCalibrate.h generic file
 *******************************************************************************/

#include <locale.h>
#include <stdio.h>
#include "GenericCalibrate.h"

int main()
{
   setlocale(LC_ALL, "");
   // Call the generic function to perform calibration on the device
   niScope_GenericCalibrate ();
   // Wait and exit
   printf("Press <RETURN> to exit.\n");
   getc(stdin); getc(stdin);
   return 0;
}

// Obtain the resource name of the device from the user
int GetResourceNameFromGUI (ViRsrc resourceName)
{
   //Get the device name from the user
   printf("Type the device name (e.g., Dev1, PXI1Slot2, Digitizer1, ...): ");
   scanf("%s", resourceName);
   return 0;
}

// Obtain the necessary parameters
int GetParametersFromGUI (ViInt32 *option)
{
   // Get the option of the calibration -- Only 1 character
   printf("Option (0-Self Calibrate All Channels, 1-Restore External Calibration): ");
   scanf("%d", option);
   return 0;
}

// Display message - do a printf
int DisplayErrorMessageInGUI (ViInt32 error,
                              ViConstString errorMessage)
{

   printf("%s\n",errorMessage);
   return 0;
}

/*************************************************************************************\

                              End of example

\*************************************************************************************/
