/*
// This example sends two Ethernet frames out every 100 ms and demonstrates a frame
// stream output session. For more information about this type of session, please
// consult the NI-XNET manual.
*/

#include "../../example_support.h"        // Include file for Sleep, _getch, _kbhit, and PrintTimestamp
#include "../EthernetExamplesUtilities.h" // Include struct for ethernet header plus other constants
#include <stdlib.h>                       // Include file for various functions
#include <ctype.h>                        // Include file for tolower
#include <string.h>                       // memcpy
#include <stdio.h>                        // printf, getchar

//=============================================================================
// Static global variables
//=============================================================================
static nxSessionRef_t m_SessionRef = 0;

//=============================================================================
// Global functions declarations
//=============================================================================
void DisplayErrorAndExit(nxStatus_t Status, char *Source);
u16 ChangeByteOrderU16(u16 value);
u32 ChangeByteOrderU32(u32 value);
void AssignVlanId(u32 *pVlan, u16 vlanId);
void AssignPriorityCodePoint(u32 *pVlan , u8 pcp);
u32 GenerateVlanTag(u16 vlanId);
void EncodeEnetFrameHeader(nxFrameEnet_t *pFrame, bool hasVlan, u16 vlanId);
void EncodeEnetFrame(nxFrameEnet_t *pFrame, u16 frameSize, bool hasVlan, u16 vlanId);

//=============================================================================
// Main function
//=============================================================================
int main(void)
{
   // Declare all variables for the function
   char *pSelectedInterface = "ENET1";
   nxStatus_t status = 0;
   const u16 kEthernetPayloadSize = 150;
   const u8 kNumberOfFrames = 2;
   u32 bufferSize = kNumberOfFrames * (kMaxEnetFrameSize + sizeof(nxFrameEnet_t) + kFcsSize);
   u8 * pBuffer = NULL;
   nxFrameEnet_t *pFrame = NULL;
   u32 numberOfBytesForFrames = 0;
   u64 frameCounter = 0;
   int typedChar = 0;

   printf("\nPlease press enter to start the program: ");
   getchar();

   // Display parameters that will be used for the example
   printf("Interface: %s\n", pSelectedInterface);

   // Create an XNET session in FrameOutStream mode
   status = nxCreateSession(NULL, NULL, NULL, pSelectedInterface,
      nxMode_FrameOutStream, &m_SessionRef);

   if (nxSuccess == status)
   {
      printf("Session created successfully.\n");
   }
   else
   {
      DisplayErrorAndExit(status, "nxCreateSession");
   }

   // Buffer must be exact size for nxWriteSession, so dynamically allocate the
   // maximum buffer size and later reallocate it to the proper size based off the
   // total amount of bytes actually written in the buffer.
   pBuffer =  (u8*)malloc(bufferSize);

   // Initialize frame pointer to first element of empty buffer
   pFrame = (nxFrameEnet_t *)pBuffer;

   // Construct a non-VLAN-tagged ethernet frame
   EncodeEnetFrame(pFrame, kEthernetPayloadSize, FALSE /*hasVlan*/, 0 /*vlanId (unused)*/);

   // The nxFrameIterateEthernetWrite macro can be used to iterate to the next
   // frame location in the buffer. This macro is especially useful to
   // traverse variable-length frames. The macro is available in nixnet.h.
   pFrame = nxFrameIterateEthernetWrite(pFrame);

   // Construct a VLAN-tagged frame
   EncodeEnetFrame(pFrame, kEthernetPayloadSize, TRUE /*hasVlan*/, kVlanId);

   // Iterate one more time in order to reach the end of the 2nd frame.
   pFrame = nxFrameIterateEthernetWrite(pFrame);

   // Calculate the number of bytes used using pointer arithmetic.
   // This gives us the total number of bytes used for the frames
   numberOfBytesForFrames = (u32)((u8 *)pFrame - (u8 *)pBuffer);

   // Reallocate buffer as required for nxWriteFrame; the buffer should be the
   // exact size required and not any larger.
   pBuffer = realloc(pBuffer, numberOfBytesForFrames);

   printf("Press q to quit.\n");
   do
   {
      // We constructed two frames in our buffer; thus, each iteration sends two frames
      frameCounter += kNumberOfFrames;
      // Write the frame buffer
      status = nxWriteFrame(m_SessionRef, pBuffer, numberOfBytesForFrames, 10.0 /* Timeout (s)*/);
      if (nxSuccess != status)
      {
         DisplayErrorAndExit(status, "nxWriteFrame");
      }
      printf("\rTotal Frames Sent: ");
      printf("%llu", frameCounter);

      Sleep(100); // Wait 100 ms

      if (_kbhit())
      {
         typedChar = _getch();
      }
   } while ('q' != tolower(typedChar));

   // Clear the XNET session
   status = nxClear(m_SessionRef);
   if (nxSuccess == status)
   {
      printf("\nSession cleared successfully!\n");
   }
   else
   {
      DisplayErrorAndExit(status, "nxClear");
   }
   // Free dynamically allocated memory for buffer
   free(pBuffer);
   return 0;
}

//=============================================================================
// Display Error Function
//=============================================================================
void DisplayErrorAndExit(nxStatus_t Status, char *Source)
{
   char l_StatusString[1024];
   nxStatusToString(Status, sizeof(l_StatusString), l_StatusString);

   printf("\n\nERROR at %s!\n%s\n", Source, l_StatusString);
   printf("\nExecution stopped.\nPress any key to quit\n");

   nxClear(m_SessionRef);

   getchar();
   exit(1);
}

// Performs endianness swapping on u16
u16 ChangeByteOrderU16(u16 value)
{
   return BigToHostOrder16(value);
}

// Performs endianness swapping on u32
u32 ChangeByteOrderU32(u32 value)
{
   return (value & 0x000000FF) << 24 |
          (value & 0x0000FF00) << 8  |
          (value & 0x00FF0000) >> 8  |
          (value & 0xFF000000) >> 24;
}

void AssignVlanId(u32 *pVlan , u16 vlanId)
{
   u16 vidMask = 0x0FFF;
   *pVlan = ((*pVlan) & ~(vidMask));
   *pVlan = ((*pVlan) | (vlanId & vidMask));
}

void AssignPriorityCodePoint(u32 *pVlan , u8 pcp)
{
   u16 pcpMask = 0xE000;
   u8 pcpLsb = 13;
   *pVlan = ((*pVlan) & ~(pcpMask));
   *pVlan = ((*pVlan) | (((u16)pcp << pcpLsb) & pcpMask));
}

u32 GenerateVlanTag(u16 vlanId)
{
   // Initialize vlan tag with the TPID
   u32 vlanTag = 0x81000000;
   AssignVlanId(&vlanTag, vlanId);
   AssignPriorityCodePoint(&vlanTag, kPcp);
   return vlanTag;
}

void EncodeEnetFrameHeader(nxFrameEnet_t *pFrame, bool hasVlan, u16 vlanId)
{
   struct EthernetHeader EnetHead =
   {
      {0xFF,0xFF,0xFF,0xFF,0xFF,0xFF},
      {0xFF,0xFF,0xFF,0xFF,0xFF,0xFF},
      0x88B5 /* Local Experimental Ethertype */
   };
   // The FrameData field is big-endian, so fields that are wider than
   // a byte (i.e. EtherType and VLAN tag) must be converted.
   u16 ethertypeBigEndian = ChangeByteOrderU16(EnetHead.EtherType);
   u32 vlanTagBigEndian = ChangeByteOrderU32(GenerateVlanTag(vlanId));

   memcpy(pFrame->FrameData + kEnetFrameOffsetDstMacAddr, &EnetHead.DestinationMacAddress, kEnetMacAddrSize);
   memcpy(pFrame->FrameData + kEnetFrameOffsetSrcMacAddr, &EnetHead.SourceMacAddress, kEnetMacAddrSize);

   if (hasVlan)
   {
      memcpy(pFrame->FrameData + kEnetFrameOffsetVlanTag, &vlanTagBigEndian, sizeof(vlanTagBigEndian) );
      memcpy(pFrame->FrameData + kEnetFrameOffsetVlanEtherType, &ethertypeBigEndian, sizeof(EthernetHeader.EtherType));
   }
   else
   {
      memcpy(pFrame->FrameData + kEnetFrameOffsetEtherType, &ethertypeBigEndian, sizeof(EthernetHeader.EtherType));
   }
}

void EncodeEnetFrame(nxFrameEnet_t *pFrame, u16 payloadSize, bool hasVlan, u16 vlanId)
{
   u32 payloadOffset = 0;
   u16 frameSize = 0;
   u16 i = 0;

   EncodeEnetFrameHeader(pFrame, hasVlan, vlanId);
   pFrame->DeviceTimestamp = 0;
   pFrame->NetworkTimestamp = 0;
   pFrame->Flags = 0;
   pFrame->Type = nxEnetFrameType_Data;

   if (hasVlan)
   {
      payloadOffset = kEnetFrameVlanTaggedHeaderSize;
   }
   else
   {
      payloadOffset = kEnetFrameHeaderSize;
   }
   // Subtract one from the size of nxFrameEnet_t because the FrameData field
   // is already defined as a one-byte array.
   frameSize = (u16)(sizeof(nxFrameEnet_t) - 1 + payloadOffset + payloadSize + kFcsSize);

   // The Length field is big-endian
   pFrame->Length = ChangeByteOrderU16(frameSize);

   // The payload is constructed with incrementing values
   for (i = 0; i < payloadSize; i++)
   {
      pFrame->FrameData[payloadOffset + i] = (u8)i;
   }
}