/**
*   Example Program:
*      NI Digital TMU Measurement Types.c
*
*   Description:
*      Demonstrates how to perform common TMU measurement types using the
*      NI-Digital Pattern Driver API. This example shows three simple measurement
*      configurations:
*        - Period: time between two consecutive identical events on the
*          same channel (rising-edge to rising-edge)
*        - Duty Cycle High: time the signal spends above a threshold on a
*          single channel (rising-edge to falling-edge)
*        - Skew: time difference between the same event on two different
*          channels (inter-channel timing)
*      The TMU is not limited by these measurement configurations.
*
*      Each measurement uses Edge arm to guarantee deterministic results.
*      To use Immediate arm instead, remove the edge arm configuration
*      and change the arm type to NIDIGITAL_VAL_IMMEDIATE.
*
*      Note: The DUT must be generating signals on the configured channels
*      for TMU measurements to complete. In simulate mode, the driver
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
   ViConstString dutPin2 = "DUTPin2";

   /* IVI Driver Variables */
   ViSession vi = VI_NULL;
   ViStatus error = VI_SUCCESS;
   ViChar errDesc[IVI_MAX_MESSAGE_BUF_SIZE];

   /*
   * Initialize the digital pattern instrument session. The reset input is
   * TRUE to ensure the instrument starts in a known state.
   */
   checkErr(niDigital_InitWithOptions(resourceName, VI_TRUE, VI_TRUE, "Simulate=1,DriverSetup=Model:6571", &vi));

   /*
   * Load the pin map for the instrument to allow referencing of pin names
   * in the channel list of select driver functions
   */
   checkErr(niDigital_LoadPinMap(vi, "PinMap.pinmap"));

   /*
   * Configure voltage levels for all channels. The VOL and VOH levels
   * define the thresholds used by the TMU for SOURCE_EVENT_VOL and
   * SOURCE_EVENT_VOH events respectively.
   */
   ViReal64 vil = 0.0;
   ViReal64 vih = 5.0;
   ViReal64 vol = 0.5;
   ViReal64 voh = 2.4;
   ViReal64 vterm = 0.0;
   checkErr(niDigital_ConfigureVoltageLevels(vi, "", vil, vih, vol, voh, vterm));

   /*
   * Construct the TMU context string.
   */
   ViConstString tmuId = NIDIGITAL_VAL_TMU0;
   ViChar tmuContext[256];
   sprintf_s(tmuContext, sizeof(tmuContext), "%s/%s", resourceName, tmuId);

   /* Enable the TMU */
   checkErr(niDigital_SetAttributeViBoolean(vi, tmuContext, NIDIGITAL_ATTR_TMU_ENABLED, VI_TRUE));

   /* Configure sample count and timeout (shared by all measurements) */
   checkErr(niDigital_SetAttributeViInt64(vi, tmuContext, NIDIGITAL_ATTR_TMU_SAMPLES_TO_ACQUIRE, 100));
   checkErr(niDigital_SetAttributeViReal64(vi, tmuContext, NIDIGITAL_ATTR_TMU_SAMPLE_TIMEOUT, 10.0));

   /*
   * ===================================================================
   * Measurement 1: Period
   * ===================================================================
   * Measures the time between two consecutive identical events on the
   * same channel. Both start and stop sources are configured to the same
   * channel with the same event and polarity.
   *
   * Configuration:
   *   Start: DUTPin1, VOL threshold, Rising Edge
   *   Stop:  DUTPin1, VOL threshold, Rising Edge (same as start)
   *   Arm:   matches start/stop (Rising Edge at VOL on DUTPin1)
   *
   * The TMU captures the first rising-edge VOL crossing as the start,
   * then the next one as the stop. The difference is one full period.
   */
   printf("--- Measurement 1: Period ---\n");

   /* Configure Edge arm */
   checkErr(niDigital_SetAttributeViInt32(vi, tmuContext, NIDIGITAL_ATTR_TMU_ARM_TYPE, NIDIGITAL_VAL_EDGE));

   /* Start source: DUTPin1, VOL threshold, Rising Edge */
   checkErr(niDigital_SetAttributeViString(vi, tmuContext, NIDIGITAL_ATTR_TMU_START_SOURCE, dutPin1));
   checkErr(niDigital_SetAttributeViInt32(vi, tmuContext, NIDIGITAL_ATTR_TMU_START_SOURCE_EVENT, NIDIGITAL_VAL_TMU_SOURCE_EVENT_VOL));
   checkErr(niDigital_SetAttributeViInt32(vi, tmuContext, NIDIGITAL_ATTR_TMU_START_SOURCE_EVENT_POLARITY, NIDIGITAL_VAL_RISING_EDGE));

   /* Stop source: same channel, same event/polarity for Period */
   checkErr(niDigital_SetAttributeViString(vi, tmuContext, NIDIGITAL_ATTR_TMU_STOP_SOURCE, dutPin1));
   checkErr(niDigital_SetAttributeViInt32(vi, tmuContext, NIDIGITAL_ATTR_TMU_STOP_SOURCE_EVENT, NIDIGITAL_VAL_TMU_SOURCE_EVENT_VOL));
   checkErr(niDigital_SetAttributeViInt32(vi, tmuContext, NIDIGITAL_ATTR_TMU_STOP_SOURCE_EVENT_POLARITY, NIDIGITAL_VAL_RISING_EDGE));

   /* Edge arm matches start/stop */
   checkErr(niDigital_SetAttributeViString(vi, tmuContext, NIDIGITAL_ATTR_TMU_EDGE_ARM_SOURCE, dutPin1));
   checkErr(niDigital_SetAttributeViInt32(vi, tmuContext, NIDIGITAL_ATTR_TMU_EDGE_ARM_SOURCE_EVENT, NIDIGITAL_VAL_TMU_SOURCE_EVENT_VOL));
   checkErr(niDigital_SetAttributeViInt32(vi, tmuContext, NIDIGITAL_ATTR_TMU_EDGE_ARM_POLARITY, NIDIGITAL_VAL_RISING_EDGE));

   /* Initiate and fetch */
   checkErr(niDigital_TMU_Initiate(vi, tmuContext));

   ViReal64 periodMeasurement = 0;
   checkErr(niDigital_TMU_FetchAveragedMeasurement(vi, tmuContext, 10.0, &periodMeasurement));

   printf("Period: %.6e seconds\n", periodMeasurement);

   checkErr(niDigital_TMU_Abort(vi, tmuContext));

   /*
   * ===================================================================
   * Measurement 2: Duty Cycle High
   * ===================================================================
   * Measures the high portion of a duty cycle on a single channel. The
   * start event is the signal rising above a threshold, and the stop
   * event is the signal falling below the same threshold.
   *
   * Configuration:
   *   Start: DUTPin1, VOH threshold, Rising Edge
   *   Stop:  DUTPin1, VOH threshold, Falling Edge
   *   Arm:   matches start (Rising Edge at VOH on DUTPin1)
   *
   * The arm event guarantees the rising edge is captured first, so
   * the measurement is always the time the signal spends above VOH.
   */
   printf("\n--- Measurement 2: Duty Cycle High ---\n");

   /* Start source: DUTPin1, VOH threshold, Rising Edge */
   checkErr(niDigital_SetAttributeViString(vi, tmuContext, NIDIGITAL_ATTR_TMU_START_SOURCE, dutPin1));
   checkErr(niDigital_SetAttributeViInt32(vi, tmuContext, NIDIGITAL_ATTR_TMU_START_SOURCE_EVENT, NIDIGITAL_VAL_TMU_SOURCE_EVENT_VOH));
   checkErr(niDigital_SetAttributeViInt32(vi, tmuContext, NIDIGITAL_ATTR_TMU_START_SOURCE_EVENT_POLARITY, NIDIGITAL_VAL_RISING_EDGE));

   /* Stop source: same channel, VOH threshold, Falling Edge */
   checkErr(niDigital_SetAttributeViString(vi, tmuContext, NIDIGITAL_ATTR_TMU_STOP_SOURCE, dutPin1));
   checkErr(niDigital_SetAttributeViInt32(vi, tmuContext, NIDIGITAL_ATTR_TMU_STOP_SOURCE_EVENT, NIDIGITAL_VAL_TMU_SOURCE_EVENT_VOH));
   checkErr(niDigital_SetAttributeViInt32(vi, tmuContext, NIDIGITAL_ATTR_TMU_STOP_SOURCE_EVENT_POLARITY, NIDIGITAL_VAL_FALLING_EDGE));

   /* Edge arm matches start source */
   checkErr(niDigital_SetAttributeViString(vi, tmuContext, NIDIGITAL_ATTR_TMU_EDGE_ARM_SOURCE, dutPin1));
   checkErr(niDigital_SetAttributeViInt32(vi, tmuContext, NIDIGITAL_ATTR_TMU_EDGE_ARM_SOURCE_EVENT, NIDIGITAL_VAL_TMU_SOURCE_EVENT_VOH));
   checkErr(niDigital_SetAttributeViInt32(vi, tmuContext, NIDIGITAL_ATTR_TMU_EDGE_ARM_POLARITY, NIDIGITAL_VAL_RISING_EDGE));

   /* Initiate and fetch */
   checkErr(niDigital_TMU_Initiate(vi, tmuContext));

   ViReal64 dutyCycleMeasurement = 0;
   checkErr(niDigital_TMU_FetchAveragedMeasurement(vi, tmuContext, 10.0, &dutyCycleMeasurement));

   printf("Duty Cycle High: %.6e seconds\n", dutyCycleMeasurement);

   checkErr(niDigital_TMU_Abort(vi, tmuContext));

   /*
   * ===================================================================
   * Measurement 3: Skew
   * ===================================================================
   * Measures the time difference between the same event on two different
   * channels. The start source is on one channel and the stop source is
   * on a different channel. The result represents the inter-channel
   * timing skew.
   *
   * Configuration:
   *   Start: DUTPin1, VOL threshold, Rising Edge
   *   Stop:  DUTPin2, VOL threshold, Rising Edge
   *   Arm:   matches start (Rising Edge at VOL on DUTPin1)
   *
   * The arm event on DUTPin1 guarantees the start event on DUTPin1 is
   * captured first, then the TMU waits for the corresponding event on
   * DUTPin2 as the stop.
   */
   printf("\n--- Measurement 3: Skew ---\n");

   /* Start source: DUTPin1, VOL threshold, Rising Edge */
   checkErr(niDigital_SetAttributeViString(vi, tmuContext, NIDIGITAL_ATTR_TMU_START_SOURCE, dutPin1));
   checkErr(niDigital_SetAttributeViInt32(vi, tmuContext, NIDIGITAL_ATTR_TMU_START_SOURCE_EVENT, NIDIGITAL_VAL_TMU_SOURCE_EVENT_VOL));
   checkErr(niDigital_SetAttributeViInt32(vi, tmuContext, NIDIGITAL_ATTR_TMU_START_SOURCE_EVENT_POLARITY, NIDIGITAL_VAL_RISING_EDGE));

   /* Stop source: DUTPin2 (different channel), VOL threshold, Rising Edge */
   checkErr(niDigital_SetAttributeViString(vi, tmuContext, NIDIGITAL_ATTR_TMU_STOP_SOURCE, dutPin2));
   checkErr(niDigital_SetAttributeViInt32(vi, tmuContext, NIDIGITAL_ATTR_TMU_STOP_SOURCE_EVENT, NIDIGITAL_VAL_TMU_SOURCE_EVENT_VOL));
   checkErr(niDigital_SetAttributeViInt32(vi, tmuContext, NIDIGITAL_ATTR_TMU_STOP_SOURCE_EVENT_POLARITY, NIDIGITAL_VAL_RISING_EDGE));

   /* Edge arm matches start source */
   checkErr(niDigital_SetAttributeViString(vi, tmuContext, NIDIGITAL_ATTR_TMU_EDGE_ARM_SOURCE, dutPin1));
   checkErr(niDigital_SetAttributeViInt32(vi, tmuContext, NIDIGITAL_ATTR_TMU_EDGE_ARM_SOURCE_EVENT, NIDIGITAL_VAL_TMU_SOURCE_EVENT_VOL));
   checkErr(niDigital_SetAttributeViInt32(vi, tmuContext, NIDIGITAL_ATTR_TMU_EDGE_ARM_POLARITY, NIDIGITAL_VAL_RISING_EDGE));

   /* Initiate and fetch */
   checkErr(niDigital_TMU_Initiate(vi, tmuContext));

   ViReal64 skewMeasurement = 0;
   checkErr(niDigital_TMU_FetchAveragedMeasurement(vi, tmuContext, 10.0, &skewMeasurement));

   printf("Skew: %.6e seconds\n", skewMeasurement);

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
