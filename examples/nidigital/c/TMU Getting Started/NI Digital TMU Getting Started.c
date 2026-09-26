/**
*   Example Program:
*      NI Digital TMU Getting Started.c
*
*   Description:
*      Demonstrates the minimal workflow to configure and perform a Time
*      Measurement Unit (TMU) measurement using the NI-Digital Pattern
*      Driver API. This example walks through every required step:
*        1. Initialize the instrument session
*        2. Load a pin map
*        3. Configure voltage levels (VOL/VOH define TMU thresholds)
*        4. Construct a TMU context string
*        5. Enable the TMU
*        6. Configure a period measurement with Immediate arm
*        7. Initiate the TMU acquisition
*        8. Fetch the averaged measurement
*        9. Abort the TMU and clean up
*
*      This is the simplest possible TMU example. For arm type
*      comparison, multi-measurement workflows, debounce filtering,
*      or multi-instrument sessions, see the other TMU examples.
*
*      Note: The DUT must be generating signals on the configured channel
*      for the TMU measurement to complete. In simulate mode, the driver
*      returns simulated measurement values.
*
*/

/* Includes */
#include <conio.h>
#include <stdio.h>
#include <stdlib.h>
#include <string.h>
#include <niDigital.h>


int main(void)
{
   /* Initial Device Values */
   ViRsrc resourceName = "PXI1Slot5";
   ViConstString dutPin1 = "DUTPin1";

   /* IVI Driver Variables */
   ViSession vi = VI_NULL;
   ViStatus error = VI_SUCCESS;
   ViChar errDesc[IVI_MAX_MESSAGE_BUF_SIZE];

   /*
   * Step 1: Initialize the digital pattern instrument session.
   * The reset input is TRUE to ensure the instrument starts in a known
   * state. All channels are in a high-impedance state, and the I/O
   * switches are open.
   */
   checkErr(niDigital_InitWithOptions(resourceName, VI_TRUE, VI_TRUE, "Simulate=1,DriverSetup=Model:6571", &vi));

   /*
   * Step 2: Load the pin map. This allows referencing pin names (e.g.,
   * "DUTPin1") in the channel list of driver functions instead of raw
   * channel numbers.
   */
   checkErr(niDigital_LoadPinMap(vi, "PinMap.pinmap"));

   /*
   * Step 3: Configure voltage levels for all channels. The VOL and VOH
   * levels define the thresholds used by the TMU for
   * SOURCE_EVENT_VOL and SOURCE_EVENT_VOH events respectively.
   */
   ViReal64 vil = 0.0;
   ViReal64 vih = 5.0;
   ViReal64 vol = 0.5;
   ViReal64 voh = 2.4;
   ViReal64 vterm = 0.0;
   checkErr(niDigital_ConfigureVoltageLevels(vi, "", vil, vih, vol, voh, vterm));

   /*
   * Step 4: Construct the TMU context string. The context string
   * identifies which TMU resource to use. It is formed by combining
   * the device resource name with the TMU identifier, separated by a
   * forward slash: "<resourceName>/<tmuId>"
   *
   * Each device has multiple TMU resources (e.g., tmu0, tmu1, ...).
   * NIDIGITAL_VAL_TMU0 is the first TMU on the device.
   */
   ViConstString tmuId = NIDIGITAL_VAL_TMU0;
   ViChar tmuContext[256];
   sprintf_s(tmuContext, sizeof(tmuContext), "%s/%s", resourceName, tmuId);

   /*
   * Step 5: Enable the TMU. TMUs are disabled by default and must be
   * explicitly enabled before use.
   */
   checkErr(niDigital_SetAttributeViBoolean(vi, tmuContext, NIDIGITAL_ATTR_TMU_ENABLED, VI_TRUE));

   /*
   * Step 6: Configure a period measurement with Immediate arm.
   *
   * Arm Type: Immediate means the TMU begins looking for start/stop
   * events as soon as niDigital_TMU_Initiate is called. No explicit
   * arm event is required.
   *
   * Period measurement: Both start and stop sources are set to the
   * same channel with the same event and polarity. This example measures
   * the time between two consecutive rising-edge VOL threshold
   * crossings, which is the signal period.
   */
   checkErr(niDigital_SetAttributeViInt32(vi, tmuContext, NIDIGITAL_ATTR_TMU_ARM_TYPE, NIDIGITAL_VAL_IMMEDIATE));

   /* Start source: DUTPin1, VOL threshold, Rising Edge */
   checkErr(niDigital_SetAttributeViString(vi, tmuContext, NIDIGITAL_ATTR_TMU_START_SOURCE, dutPin1));
   checkErr(niDigital_SetAttributeViInt32(vi, tmuContext, NIDIGITAL_ATTR_TMU_START_SOURCE_EVENT, NIDIGITAL_VAL_TMU_SOURCE_EVENT_VOL));
   checkErr(niDigital_SetAttributeViInt32(vi, tmuContext, NIDIGITAL_ATTR_TMU_START_SOURCE_EVENT_POLARITY, NIDIGITAL_VAL_RISING_EDGE));

   /* Stop source: same channel, same event, same polarity (period) */
   checkErr(niDigital_SetAttributeViString(vi, tmuContext, NIDIGITAL_ATTR_TMU_STOP_SOURCE, dutPin1));
   checkErr(niDigital_SetAttributeViInt32(vi, tmuContext, NIDIGITAL_ATTR_TMU_STOP_SOURCE_EVENT, NIDIGITAL_VAL_TMU_SOURCE_EVENT_VOL));
   checkErr(niDigital_SetAttributeViInt32(vi, tmuContext, NIDIGITAL_ATTR_TMU_STOP_SOURCE_EVENT_POLARITY, NIDIGITAL_VAL_RISING_EDGE));

   /*
   * Configure the number of samples to acquire and average. A higher
   * sample count produces a more stable averaged measurement but takes
   * longer to complete.
   */
   checkErr(niDigital_SetAttributeViInt64(vi, tmuContext, NIDIGITAL_ATTR_TMU_SAMPLES_TO_ACQUIRE, 100));

   /*
   * Configure the per-sample timeout. If a single sample is not
   * acquired within this time, the measurement fails.
   */
   checkErr(niDigital_SetAttributeViReal64(vi, tmuContext, NIDIGITAL_ATTR_TMU_SAMPLE_TIMEOUT, 10.0));

   /*
   * Step 7: Initiate the TMU acquisition (non-blocking). The TMU
   * begins looking for start/stop events immediately.
   */
   checkErr(niDigital_TMU_Initiate(vi, tmuContext));

   /*
   * Step 8: Fetch the averaged measurement. This blocks until all
   * configured samples have been acquired and averaged, or the timeout
   * elapses. The result is in seconds.
   */
   ViReal64 periodMeasurement = 0;
   checkErr(niDigital_TMU_FetchAveragedMeasurement(vi, tmuContext, 10.0, &periodMeasurement));

   printf("Period measurement: %.6e seconds\n", periodMeasurement);

   /*
   * Step 9: Abort the TMU acquisition and clean up.
   */
   checkErr(niDigital_TMU_Abort(vi, tmuContext));

   /* Disconnect all channels using programmable onboard switching */
   checkErr(niDigital_SelectFunction(vi, "", NIDIGITAL_VAL_DISCONNECT));

   /* Print Result */
   printf("\nDone without error.\n");

Error:

   if (error != VI_SUCCESS) /* Error Occurred */
   {
      /* Get error description and print */
      niDigital_GetError(vi, &error, sizeof(errDesc) / sizeof(ViChar), errDesc);
      printf("\nError encountered\n===================\n%s\n", errDesc);
   }

   /* Close the session to the instrument */
   niDigital_close(vi);

   /* Prompt to exit (for pop-up console windows) */
   printf("\nHit <Enter> to continue...\n");
   _getch();

   return error;
}
