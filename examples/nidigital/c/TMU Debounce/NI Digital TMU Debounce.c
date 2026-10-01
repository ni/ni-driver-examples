/**
*   Example Program:
*      NI Digital TMU Debounce.c
*
*   Description:
*      Demonstrates how to use the NI-Digital Pattern Driver API to configure
*      TMU input debounce filtering. This example shows:
*        - Discovering available TMU resources using GetDisabledTMUContexts
*        - Configuring debounce times for start and stop inputs
*        - The constraint that when the start source and stop source are the
*          same channel, the start and stop debounce times must be equal
*        - When start and stop sources are different channels, the debounce
*          times can be configured independently
*
*      Debounce filtering helps reject noise and glitches on the TMU input
*      signals by requiring the signal to remain stable for the configured
*      debounce time before a threshold crossing is recognized as a valid
*      event.
*
*      Note: In simulate mode, the driver returns simulated measurement
*      values.
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
   * ===================================================================
   * Create TMU Context String
   * ===================================================================
   */
   ViConstString tmuId = NIDIGITAL_VAL_TMU0;
   ViChar tmuContext[256];
   sprintf_s(tmuContext, sizeof(tmuContext), "%s/%s", resourceName, tmuId);

   /*
   * ===================================================================
   * Example 1: Same Channel - Debounce Times Must Be Equal
   * ===================================================================
   * When the TMU start source and stop source are configured to the
   * same channel, the start input debounce time and stop input debounce
   * time MUST be set to the same value. If they differ, the driver
   * returns an error.
   *
   * This constraint exists because the same physical input path is used
   * for both start and stop events when they share a channel, so only
   * one debounce filter setting can be applied.
   */
   printf("--- Example 1: Same Channel Debounce (Equal Times) ---\n");

   /* Enable a TMU to use for measurements */
   checkErr(niDigital_SetAttributeViBoolean(vi, tmuContext, NIDIGITAL_ATTR_TMU_ENABLED, VI_TRUE));

   /* Configure arm type to Immediate */
   checkErr(niDigital_SetAttributeViInt32(vi, tmuContext, NIDIGITAL_ATTR_TMU_ARM_TYPE, NIDIGITAL_VAL_IMMEDIATE));

   /*
   * Configure start and stop to the SAME channel (DUTPin1).
   * This is a Period measurement configuration.
   */
   checkErr(niDigital_SetAttributeViString(vi, tmuContext, NIDIGITAL_ATTR_TMU_START_SOURCE, dutPin1));
   checkErr(niDigital_SetAttributeViInt32(vi, tmuContext, NIDIGITAL_ATTR_TMU_START_SOURCE_EVENT, NIDIGITAL_VAL_TMU_SOURCE_EVENT_VOL));
   checkErr(niDigital_SetAttributeViInt32(vi, tmuContext, NIDIGITAL_ATTR_TMU_START_SOURCE_EVENT_POLARITY, NIDIGITAL_VAL_RISING_EDGE));

   checkErr(niDigital_SetAttributeViString(vi, tmuContext, NIDIGITAL_ATTR_TMU_STOP_SOURCE, dutPin1));
   checkErr(niDigital_SetAttributeViInt32(vi, tmuContext, NIDIGITAL_ATTR_TMU_STOP_SOURCE_EVENT, NIDIGITAL_VAL_TMU_SOURCE_EVENT_VOL));
   checkErr(niDigital_SetAttributeViInt32(vi, tmuContext, NIDIGITAL_ATTR_TMU_STOP_SOURCE_EVENT_POLARITY, NIDIGITAL_VAL_RISING_EDGE));

   /* Configure sample count and timeout */
   checkErr(niDigital_SetAttributeViInt64(vi, tmuContext, NIDIGITAL_ATTR_TMU_SAMPLES_TO_ACQUIRE, 100));
   checkErr(niDigital_SetAttributeViReal64(vi, tmuContext, NIDIGITAL_ATTR_TMU_SAMPLE_TIMEOUT, 10.0));

   /*
   * Set debounce times for both start and stop inputs to the SAME value.
   * Since start and stop sources are on the same channel, these must be
   * equal. Setting different values would cause an error.
   *
   * A debounce time of 200ns filters out glitches shorter than 200ns.
   */
   ViReal64 debounceTime = 200.0e-9; /* 200 ns */
   checkErr(niDigital_SetAttributeViReal64(vi, tmuContext, NIDIGITAL_ATTR_TMU_START_INPUT_DEBOUNCE_TIME, debounceTime));
   checkErr(niDigital_SetAttributeViReal64(vi, tmuContext, NIDIGITAL_ATTR_TMU_STOP_INPUT_DEBOUNCE_TIME, debounceTime));

   printf("Same channel (%s): start debounce = %.0f ns, stop debounce = %.0f ns\n",
      dutPin1, debounceTime * 1.0e9, debounceTime * 1.0e9);

   /* Initiate the TMU acquisition */
   checkErr(niDigital_TMU_Initiate(vi, tmuContext));

   /* Fetch the averaged measurement */
   ViReal64 measurement1 = 0;
   checkErr(niDigital_TMU_FetchAveragedMeasurement(vi, tmuContext, 10.0, &measurement1));

   printf("Period measurement with debounce: %.6e seconds\n", measurement1);

   /* Abort the TMU */
   checkErr(niDigital_TMU_Abort(vi, tmuContext));

   /*
   * ===================================================================
   * Example 2: Different Channels - Independent Debounce Times
   * ===================================================================
   * When the TMU start source and stop source are configured to
   * different channels, the start and stop debounce times can be
   * configured independently. Each channel has its own physical input
   * path, so each can have a different debounce filter setting.
   */
   printf("\n--- Example 2: Different Channels Debounce (Independent Times) ---\n");

   /* Enable a TMU to use for measurements */
   checkErr(niDigital_SetAttributeViBoolean(vi, tmuContext, NIDIGITAL_ATTR_TMU_ENABLED, VI_TRUE));

   /* Configure arm type to Immediate */
   checkErr(niDigital_SetAttributeViInt32(vi, tmuContext, NIDIGITAL_ATTR_TMU_ARM_TYPE, NIDIGITAL_VAL_IMMEDIATE));

   /*
   * Configure start and stop to DIFFERENT channels.
   * This is a Skew measurement configuration.
   */
   checkErr(niDigital_SetAttributeViString(vi, tmuContext, NIDIGITAL_ATTR_TMU_START_SOURCE, dutPin1));
   checkErr(niDigital_SetAttributeViInt32(vi, tmuContext, NIDIGITAL_ATTR_TMU_START_SOURCE_EVENT, NIDIGITAL_VAL_TMU_SOURCE_EVENT_VOL));
   checkErr(niDigital_SetAttributeViInt32(vi, tmuContext, NIDIGITAL_ATTR_TMU_START_SOURCE_EVENT_POLARITY, NIDIGITAL_VAL_RISING_EDGE));

   checkErr(niDigital_SetAttributeViString(vi, tmuContext, NIDIGITAL_ATTR_TMU_STOP_SOURCE, dutPin2));
   checkErr(niDigital_SetAttributeViInt32(vi, tmuContext, NIDIGITAL_ATTR_TMU_STOP_SOURCE_EVENT, NIDIGITAL_VAL_TMU_SOURCE_EVENT_VOL));
   checkErr(niDigital_SetAttributeViInt32(vi, tmuContext, NIDIGITAL_ATTR_TMU_STOP_SOURCE_EVENT_POLARITY, NIDIGITAL_VAL_RISING_EDGE));

   /* Configure sample count and timeout */
   checkErr(niDigital_SetAttributeViInt64(vi, tmuContext, NIDIGITAL_ATTR_TMU_SAMPLES_TO_ACQUIRE, 100));
   checkErr(niDigital_SetAttributeViReal64(vi, tmuContext, NIDIGITAL_ATTR_TMU_SAMPLE_TIMEOUT, 10.0));

   /*
   * Set DIFFERENT debounce times for start and stop inputs. This is
   * allowed because the start and stop sources are on different channels.
   * Each channel's input path has its own independent debounce filter.
   *
   * Start debounce: 200ns - signal on DUTPin1 must be stable for 200ns
   * Stop debounce:  500ns - signal on DUTPin2 must be stable for 500ns
   */
   ViReal64 startDebounce = 200.0e-9; /* 200 ns */
   ViReal64 stopDebounce = 500.0e-9;  /* 500 ns */
   checkErr(niDigital_SetAttributeViReal64(vi, tmuContext, NIDIGITAL_ATTR_TMU_START_INPUT_DEBOUNCE_TIME, startDebounce));
   checkErr(niDigital_SetAttributeViReal64(vi, tmuContext, NIDIGITAL_ATTR_TMU_STOP_INPUT_DEBOUNCE_TIME, stopDebounce));

   printf("Different channels: start (%s) debounce = %.0f ns, "
      "stop (%s) debounce = %.0f ns\n",
      dutPin1, startDebounce * 1.0e9, dutPin2, stopDebounce * 1.0e9);

   /* Initiate the TMU acquisition */
   checkErr(niDigital_TMU_Initiate(vi, tmuContext));

   /* Fetch the averaged measurement */
   ViReal64 measurement2 = 0;
   checkErr(niDigital_TMU_FetchAveragedMeasurement(vi, tmuContext, 10.0, &measurement2));

   printf("Skew measurement with independent debounce: %.6e seconds\n", measurement2);

   /* Abort the TMU */
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
