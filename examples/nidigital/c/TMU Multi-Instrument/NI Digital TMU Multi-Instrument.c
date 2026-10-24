/**
*   Example Program:
*      NI Digital TMU Multi-Instrument.c
*
*   Description:
*      Demonstrates how to use the NI-Digital Pattern Driver API to work with
*      TMU resources in a multi-instrument session. This example shows:
*        - TMU naming conventions: each device has its own set of TMUs,
*          and in multi-instrument sessions, TMU contexts are device-qualified
*        - How GetDisabledTMUContexts returns device-qualified TMU context
*          strings so you can identify which device each TMU belongs to
*        - Using TMU resources across multiple devices in the same session
*        - TMU is per-device: each device has independent TMU hardware that
*          can be configured and operated separately
*
*      Note: In simulate mode, the driver returns simulated measurement
*      values. With real hardware, each device's TMU operates independently
*      on its own DIO channels.
*
*/

/* Includes */
#include <conio.h>
#include <stdio.h>
#include <stdlib.h>
#include <string.h>
#include <niDigital.h>


/*
* Helper function: Discovers available TMU contexts in a multi-instrument
* session and returns one TMU context per device. Parses device prefixes
* (the portion before "/tmu") to ensure the two returned contexts belong
* to different instruments.
* Returns VI_SUCCESS if two contexts from different devices were found.
*/
static ViStatus getTmuContextsPerDevice(
   ViSession vi,
   ViChar tmuContext1[], ViInt32 context1Size,
   ViChar tmuContext2[], ViInt32 context2Size)
{
   ViInt32 bufferSize = niDigital_TMU_GetDisabledTMUContexts(vi, 0, VI_NULL);
   if (bufferSize == 0)
   {
      printf("No TMU contexts available. Ensure the instruments support TMU.\n");
      return IVI_ERROR_RESOURCE_UNKNOWN;
   }

   ViChar* disabledContexts = (ViChar*)malloc(bufferSize * sizeof(ViChar));
   ViStatus status = niDigital_TMU_GetDisabledTMUContexts(vi, bufferSize, disabledContexts);
   if (status < VI_SUCCESS)
   {
      free(disabledContexts);
      return status;
   }

   printf("TMU contexts in multi-instrument session (all initially disabled):\n");
   printf("  %s\n\n", disabledContexts);

   /* Parse the comma-separated list, selecting one TMU per device */
   printf("TMU resources per device:\n");
   ViChar contextsCopy[1024];
   strncpy_s(contextsCopy, sizeof(contextsCopy), disabledContexts, _TRUNCATE);
   ViChar* nextTmuContext = VI_NULL;
   ViChar* tmuContext = strtok_s(contextsCopy, ",", &nextTmuContext);
   ViChar device1Prefix[256] = "";

   while (tmuContext != VI_NULL)
   {
      printf("  %s\n", tmuContext);
      ViChar* slash = strstr(tmuContext, "/tmu");
      if (tmuContext1[0] == '\0')
      {
         strncpy_s(tmuContext1, context1Size, tmuContext, _TRUNCATE);
         if (slash != VI_NULL)
         {
            size_t prefixLen = slash - tmuContext;
            strncpy_s(device1Prefix, sizeof(device1Prefix), tmuContext, prefixLen);
         }
      }
      else if (tmuContext2[0] == '\0' && slash != VI_NULL)
      {
         size_t prefixLen = slash - tmuContext;
         if (prefixLen != strlen(device1Prefix) ||
            strncmp(tmuContext, device1Prefix, prefixLen) != 0)
         {
            strncpy_s(tmuContext2, context2Size, tmuContext, _TRUNCATE);
         }
      }
      tmuContext = strtok_s(VI_NULL, ",", &nextTmuContext);
   }

   printf("\n");
   printf("Using TMU context 1: %s\n", tmuContext1);
   printf("Using TMU context 2: %s\n\n", tmuContext2);
   free(disabledContexts);
   return VI_SUCCESS;
}


int main(void)
{
   /*
   * Multi-instrument session: specify multiple device resource names
   * separated by commas. Each device contributes its own TMU resources
   * to the session.
   */
   ViRsrc resourceNames = "PXI1Slot5,PXI1Slot6";

   /* IVI Driver Variables */
   ViSession vi = VI_NULL;
   ViStatus error = VI_SUCCESS;
   ViChar errDesc[IVI_MAX_MESSAGE_BUF_SIZE];
   ViChar tmuContext1[256] = "";
   ViChar tmuContext2[256] = "";
   ViChar* remainingContexts = VI_NULL;

   /*
   * Initialize the digital pattern instrument session with multiple
   * devices. The session aggregates all devices and their TMU resources.
   */
   checkErr(niDigital_InitWithOptions(resourceNames, VI_TRUE, VI_TRUE, "Simulate=1,DriverSetup=Model:6571", &vi));

   /*
   * Load the pin map for the multi-instrument configuration. The pin
   * map defines how pins are mapped to channels across all instruments.
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
   * Discover TMU Resources
   * ===================================================================
   * GetDisabledTMUContexts returns device-qualified TMU context strings.
   * Each TMU context includes the device resource name as a prefix,
   * e.g.: "PXI1Slot5/tmu0"
   *
   * This tells you:
   *   - Which device each TMU belongs to
   *   - How many TMUs are available per device
   *   - The full context string needed to address each TMU
   *
   * TMU is per-device: each PXIe-6570/6571 instrument has its own TMU
   * hardware. TMUs on different devices operate independently and
   * can measure different channels simultaneously.
   */
   checkErr(getTmuContextsPerDevice(vi,
      tmuContext1, sizeof(tmuContext1),
      tmuContext2, sizeof(tmuContext2)));

   /*
   * ===================================================================
   * Configure TMUs on Different Devices
   * ===================================================================
   * In a multi-instrument session, you must use device-qualified TMU
   * context strings when setting attributes and calling TMU functions.
   *
   * Here we configure one TMU on each device to perform independent
   * measurements. Each device's TMU can only measure channels connected
   * to that same device.
   */

   /*
   * --- Device 1: Period measurement on DUTPin1 ---
   * DUTPin1 is connected to the first device's channel 0 (per the pin map).
   * We use the first TMU context from GetDisabledTMUContexts.
   */
   printf("--- Device 1 (%s): Period on DUTPin1 ---\n", tmuContext1);

   checkErr(niDigital_SetAttributeViBoolean(vi, tmuContext1, NIDIGITAL_ATTR_TMU_ENABLED, VI_TRUE));
   checkErr(niDigital_SetAttributeViInt32(vi, tmuContext1, NIDIGITAL_ATTR_TMU_ARM_TYPE, NIDIGITAL_VAL_IMMEDIATE));

   checkErr(niDigital_SetAttributeViString(vi, tmuContext1, NIDIGITAL_ATTR_TMU_START_SOURCE, "DUTPin1"));
   checkErr(niDigital_SetAttributeViInt32(vi, tmuContext1, NIDIGITAL_ATTR_TMU_START_SOURCE_EVENT, NIDIGITAL_VAL_TMU_SOURCE_EVENT_VOL));
   checkErr(niDigital_SetAttributeViInt32(vi, tmuContext1, NIDIGITAL_ATTR_TMU_START_SOURCE_EVENT_POLARITY, NIDIGITAL_VAL_RISING_EDGE));

   checkErr(niDigital_SetAttributeViString(vi, tmuContext1, NIDIGITAL_ATTR_TMU_STOP_SOURCE, "DUTPin1"));
   checkErr(niDigital_SetAttributeViInt32(vi, tmuContext1, NIDIGITAL_ATTR_TMU_STOP_SOURCE_EVENT, NIDIGITAL_VAL_TMU_SOURCE_EVENT_VOL));
   checkErr(niDigital_SetAttributeViInt32(vi, tmuContext1, NIDIGITAL_ATTR_TMU_STOP_SOURCE_EVENT_POLARITY, NIDIGITAL_VAL_RISING_EDGE));

   checkErr(niDigital_SetAttributeViInt64(vi, tmuContext1, NIDIGITAL_ATTR_TMU_SAMPLES_TO_ACQUIRE, 100));
   checkErr(niDigital_SetAttributeViReal64(vi, tmuContext1, NIDIGITAL_ATTR_TMU_SAMPLE_TIMEOUT, 10.0));

   /*
   * --- Device 2: Period measurement on DUTPin3 ---
   * DUTPin3 is connected to the second device's channel (per the pin map).
   * We use the second TMU context from GetDisabledTMUContexts. Each
   * device's TMU operates independently, so both measurements can be
   * configured and run in the same session.
   */
   printf("--- Device 2 (%s): Period on DUTPin3 ---\n", tmuContext2);

   checkErr(niDigital_SetAttributeViBoolean(vi, tmuContext2, NIDIGITAL_ATTR_TMU_ENABLED, VI_TRUE));
   checkErr(niDigital_SetAttributeViInt32(vi, tmuContext2, NIDIGITAL_ATTR_TMU_ARM_TYPE, NIDIGITAL_VAL_IMMEDIATE));

   checkErr(niDigital_SetAttributeViString(vi, tmuContext2, NIDIGITAL_ATTR_TMU_START_SOURCE, "DUTPin3"));
   checkErr(niDigital_SetAttributeViInt32(vi, tmuContext2, NIDIGITAL_ATTR_TMU_START_SOURCE_EVENT, NIDIGITAL_VAL_TMU_SOURCE_EVENT_VOL));
   checkErr(niDigital_SetAttributeViInt32(vi, tmuContext2, NIDIGITAL_ATTR_TMU_START_SOURCE_EVENT_POLARITY, NIDIGITAL_VAL_RISING_EDGE));

   checkErr(niDigital_SetAttributeViString(vi, tmuContext2, NIDIGITAL_ATTR_TMU_STOP_SOURCE, "DUTPin3"));
   checkErr(niDigital_SetAttributeViInt32(vi, tmuContext2, NIDIGITAL_ATTR_TMU_STOP_SOURCE_EVENT, NIDIGITAL_VAL_TMU_SOURCE_EVENT_VOL));
   checkErr(niDigital_SetAttributeViInt32(vi, tmuContext2, NIDIGITAL_ATTR_TMU_STOP_SOURCE_EVENT_POLARITY, NIDIGITAL_VAL_RISING_EDGE));

   checkErr(niDigital_SetAttributeViInt64(vi, tmuContext2, NIDIGITAL_ATTR_TMU_SAMPLES_TO_ACQUIRE, 100));
   checkErr(niDigital_SetAttributeViReal64(vi, tmuContext2, NIDIGITAL_ATTR_TMU_SAMPLE_TIMEOUT, 10.0));

   /*
   * ===================================================================
   * Verify Enabled TMUs with GetDisabledTMUContexts
   * ===================================================================
   * This code snippet demonstrates that GetDisabledTMUContexts returns
	* only the context strings for TMUs that have not been enabled. Because
	* two TMUs have been enabled, one on each instrument, GetDisabledTMUContexts
	* returns two fewer context strings.
   *
   * Note: This call triggers attribute verification for all TMUs. If
   * any TMU has an invalid configuration, it will appear as disabled.
   */
   printf("\n--- Checking disabled TMUs after configuration ---\n");
   ViInt32 verifyBufferSize = niDigital_TMU_GetDisabledTMUContexts(vi, 0, VI_NULL);
   if (verifyBufferSize > 0)
   {
      remainingContexts = (ViChar*)malloc(verifyBufferSize * sizeof(ViChar));
      checkErr(niDigital_TMU_GetDisabledTMUContexts(vi, verifyBufferSize, remainingContexts));

      if (strlen(remainingContexts) > 0)
         printf("Still disabled TMU contexts:\n  %s\n", remainingContexts);
      else
         printf("All TMUs are enabled.\n");
      free(remainingContexts);
      remainingContexts = VI_NULL;
   }

   /*
   * ===================================================================
   * Initiate and Fetch Measurements on Both Devices
   * ===================================================================
   * Each device's TMU can be initiated independently. The TMU hardware
   * on each device operates in parallel.
   */
   printf("\n--- Initiating measurements on both devices ---\n");

   /* Initiate TMU on device 1 */
   checkErr(niDigital_TMU_Initiate(vi, tmuContext1));

   /* Initiate TMU on device 2 */
   checkErr(niDigital_TMU_Initiate(vi, tmuContext2));

   /* Fetch measurement from device 1 */
   ViReal64 measurement1 = 0;
   checkErr(niDigital_TMU_FetchAveragedMeasurement(vi, tmuContext1, 10.0, &measurement1));
   printf("%s Period: %.6e seconds\n", tmuContext1, measurement1);

   /* Fetch measurement from device 2 */
   ViReal64 measurement2 = 0;
   checkErr(niDigital_TMU_FetchAveragedMeasurement(vi, tmuContext2, 10.0, &measurement2));
   printf("%s Period: %.6e seconds\n", tmuContext2, measurement2);

   /* Abort both TMU acquisitions */
   checkErr(niDigital_TMU_Abort(vi, tmuContext1));
   checkErr(niDigital_TMU_Abort(vi, tmuContext2));

   /* Disconnect all channels using programmable onboard switching */
   checkErr(niDigital_SelectFunction(vi, "", NIDIGITAL_VAL_DISCONNECT));

   /* Print Result */
   printf("\nDone without error.\n");

Error:

   if (remainingContexts != VI_NULL)
      free(remainingContexts);

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
