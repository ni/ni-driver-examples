/**
*   Example Program:
*      NI Digital TMU Arm Types.c
*
*   Description:
*      Demonstrates the difference between Immediate and Edge arm types
*      for TMU measurements using the NI-Digital Pattern Driver API.
*      This example shows:
*        - Immediate arm: the TMU begins looking for start/stop events as
*          soon as niDigital_TMU_Initiate is called. When start and stop
*          events differ (e.g., duty cycle), the detection order is not
*          guaranteed on a free-running signal, which can produce negative
*          measurements
*        - Edge arm: the TMU waits for a specific arm event before looking
*          for start/stop events, guaranteeing deterministic event ordering
*          and consistent (positive) measurements
*        - The constraint that edge arm source/event/polarity must match
*          either the start or stop source configuration
*
*      Both sections measure the same quantity (duty cycle high) on the
*      same channel so the behavioral difference between the two arm types
*      is clearly visible.
*
*      Note: The DUT must be generating signals on the configured channel
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
   * Construct the TMU context strings.
   * TMU0 will be used for the Immediate arm measurement.
   * TMU1 will be used for the Edge arm measurement.
   */
   ViChar tmuContext0[256];
   sprintf_s(tmuContext0, sizeof(tmuContext0), "%s/%s", resourceName, NIDIGITAL_VAL_TMU0);
   ViChar tmuContext1[256];
   sprintf_s(tmuContext1, sizeof(tmuContext1), "%s/%s", resourceName, NIDIGITAL_VAL_TMU1);

   /* Enable both TMUs */
   checkErr(niDigital_SetAttributeViBoolean(vi, tmuContext0, NIDIGITAL_ATTR_TMU_ENABLED, VI_TRUE));

   checkErr(niDigital_SetAttributeViBoolean(vi, tmuContext1, NIDIGITAL_ATTR_TMU_ENABLED, VI_TRUE));

   /* Configure sample count and timeout for both TMUs */
   checkErr(niDigital_SetAttributeViInt64(vi, tmuContext0, NIDIGITAL_ATTR_TMU_SAMPLES_TO_ACQUIRE, 100));
   checkErr(niDigital_SetAttributeViReal64(vi, tmuContext0, NIDIGITAL_ATTR_TMU_SAMPLE_TIMEOUT, 10.0));

   checkErr(niDigital_SetAttributeViInt64(vi, tmuContext1, NIDIGITAL_ATTR_TMU_SAMPLES_TO_ACQUIRE, 100));
   checkErr(niDigital_SetAttributeViReal64(vi, tmuContext1, NIDIGITAL_ATTR_TMU_SAMPLE_TIMEOUT, 10.0));

   /*
   * ===================================================================
   * Part 1: Duty Cycle High with Immediate Arm
   * ===================================================================
   * With Immediate arm, the TMU begins looking for start and stop events
   * as soon as niDigital_TMU_Initiate is called. Because the start event
   * (VOH Rising Edge) and stop event (VOH Falling Edge) are different,
   * the first event detected after initiation could be either one.
   *
   * If the stop event (Falling Edge) happens to be detected first, the
   * measurement result will be NEGATIVE, indicating the stop timestamp
   * preceded the start timestamp. This is the fundamental ambiguity of
   * Immediate arm with non-identical start/stop events on a free-running
   * periodic signal.
   */
   printf("=== Part 1: Duty Cycle High with IMMEDIATE Arm (TMU0) ===\n\n");

   /* Configure Immediate arm type on TMU0 */
   checkErr(niDigital_SetAttributeViInt32(vi, tmuContext0, NIDIGITAL_ATTR_TMU_ARM_TYPE, NIDIGITAL_VAL_IMMEDIATE));

   /* Start source: DUTPin1, VOH threshold, Rising Edge */
   checkErr(niDigital_SetAttributeViString(vi, tmuContext0, NIDIGITAL_ATTR_TMU_START_SOURCE, dutPin1));
   checkErr(niDigital_SetAttributeViInt32(vi, tmuContext0, NIDIGITAL_ATTR_TMU_START_SOURCE_EVENT, NIDIGITAL_VAL_TMU_SOURCE_EVENT_VOH));
   checkErr(niDigital_SetAttributeViInt32(vi, tmuContext0, NIDIGITAL_ATTR_TMU_START_SOURCE_EVENT_POLARITY, NIDIGITAL_VAL_RISING_EDGE));

   /* Stop source: same channel, VOH threshold, Falling Edge */
   checkErr(niDigital_SetAttributeViString(vi, tmuContext0, NIDIGITAL_ATTR_TMU_STOP_SOURCE, dutPin1));
   checkErr(niDigital_SetAttributeViInt32(vi, tmuContext0, NIDIGITAL_ATTR_TMU_STOP_SOURCE_EVENT, NIDIGITAL_VAL_TMU_SOURCE_EVENT_VOH));
   checkErr(niDigital_SetAttributeViInt32(vi, tmuContext0, NIDIGITAL_ATTR_TMU_STOP_SOURCE_EVENT_POLARITY, NIDIGITAL_VAL_FALLING_EDGE));

   /*
   * ===================================================================
   * Part 2: Duty Cycle High with Edge Arm (TMU1)
   * ===================================================================
   * With Edge arm, the TMU waits for a specific arm event before it
   * begins looking for start/stop events. This eliminates the ambiguity
   * of Immediate arm by establishing a known starting point in the
   * signal cycle.
   *
   * Here we configure the arm to match the start source (DUTPin1, VOH,
   * Rising Edge). The sequence is:
   *   1. TMU waits for arm event (Rising Edge at VOH on DUTPin1)
   *   2. Arm event is captured as the start event
   *   3. TMU then waits for the stop event (Falling Edge at VOH)
   *   4. Measurement = stop timestamp - start timestamp (always positive)
   *
   * IMPORTANT CONSTRAINT: The edge arm source, event, and polarity must
   * match either the start or stop source configuration. If they do not
   * match, the driver returns an error.
   */
   printf("\n=== Part 2: Duty Cycle High with EDGE Arm (TMU1) ===\n\n");

   /* Configure Edge arm type on TMU1 */
   checkErr(niDigital_SetAttributeViInt32(vi, tmuContext1, NIDIGITAL_ATTR_TMU_ARM_TYPE, NIDIGITAL_VAL_EDGE));

   /* Start source: same duty cycle configuration as Part 1 */
   checkErr(niDigital_SetAttributeViString(vi, tmuContext1, NIDIGITAL_ATTR_TMU_START_SOURCE, dutPin1));
   checkErr(niDigital_SetAttributeViInt32(vi, tmuContext1, NIDIGITAL_ATTR_TMU_START_SOURCE_EVENT, NIDIGITAL_VAL_TMU_SOURCE_EVENT_VOH));
   checkErr(niDigital_SetAttributeViInt32(vi, tmuContext1, NIDIGITAL_ATTR_TMU_START_SOURCE_EVENT_POLARITY, NIDIGITAL_VAL_RISING_EDGE));

   /* Stop source: same duty cycle configuration as Part 1 */
   checkErr(niDigital_SetAttributeViString(vi, tmuContext1, NIDIGITAL_ATTR_TMU_STOP_SOURCE, dutPin1));
   checkErr(niDigital_SetAttributeViInt32(vi, tmuContext1, NIDIGITAL_ATTR_TMU_STOP_SOURCE_EVENT, NIDIGITAL_VAL_TMU_SOURCE_EVENT_VOH));
   checkErr(niDigital_SetAttributeViInt32(vi, tmuContext1, NIDIGITAL_ATTR_TMU_STOP_SOURCE_EVENT_POLARITY, NIDIGITAL_VAL_FALLING_EDGE));

   /*
   * Configure the edge arm to match the start source. This guarantees
   * the rising edge (start) is always captured before the falling edge
   * (stop), producing a consistent positive measurement.
   */
   checkErr(niDigital_SetAttributeViString(vi, tmuContext1, NIDIGITAL_ATTR_TMU_EDGE_ARM_SOURCE, dutPin1));
   checkErr(niDigital_SetAttributeViInt32(vi, tmuContext1, NIDIGITAL_ATTR_TMU_EDGE_ARM_SOURCE_EVENT, NIDIGITAL_VAL_TMU_SOURCE_EVENT_VOH));
   checkErr(niDigital_SetAttributeViInt32(vi, tmuContext1, NIDIGITAL_ATTR_TMU_EDGE_ARM_POLARITY, NIDIGITAL_VAL_RISING_EDGE));

   /*
   * ===================================================================
   * Initiate both TMUs back-to-back
   * ===================================================================
   * By initiating both TMUs consecutively, they begin acquiring
   * measurements on the same signal nearly simultaneously.
   */
   checkErr(niDigital_TMU_Initiate(vi, tmuContext0));
   checkErr(niDigital_TMU_Initiate(vi, tmuContext1));

   /* Fetch results from both TMUs */
   ViReal64 immediateMeasurement = 0;
   checkErr(niDigital_TMU_FetchAveragedMeasurement(vi, tmuContext0, 10.0, &immediateMeasurement));

   ViReal64 edgeMeasurement = 0;
   checkErr(niDigital_TMU_FetchAveragedMeasurement(vi, tmuContext1, 10.0, &edgeMeasurement));

   /*
   * The Immediate arm result may be positive or negative. A negative
   * value means the falling edge (stop) was detected before the rising
   * edge (start). This is expected with Immediate arm on a free-running
   * signal when start and stop events differ.
   */
   printf("Immediate Arm (TMU0) - Duty Cycle High: %.6e seconds\n", immediateMeasurement);

   /*
   * With Edge arm, the result is always positive because the arm event
   * guarantees the start event is captured before the stop event.
   */
   printf("Edge Arm (TMU1) - Duty Cycle High: %.6e seconds\n", edgeMeasurement);

   /* Abort both TMUs */
   checkErr(niDigital_TMU_Abort(vi, tmuContext0));
   checkErr(niDigital_TMU_Abort(vi, tmuContext1));

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
