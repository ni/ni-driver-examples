/*********************************************************************/
/* This example demonstrates how you may query the USB RAW devices   */
/* and list the descriptors associated with those devices.           */
/*                                                                   */
/* The general flow of the code is                                   */
/*      Open Resource Manager                                        */
/*      Use viFindRsrc() to query available USB RAW instrument       */
/*      Open a session to the device found                           */
/*      Display the descriptors for this device                      */
/*      Repeat process with the next instrument using viFindNext()   */
/*      Close all VISA Sessions                                      */
/*********************************************************************/

#include <stdio.h>
#include <stdlib.h>
#include <string.h>

#include "visa.h"

/* Defines */
#define USB_REQUESTTYPE_GET_DESCRIPTOR      0x80
#define USB_REQUEST_GET_DESCRIPTOR          6
#define USB_DESCRIPTOR_TYPE_DEVICE          0x01
#define USB_DESCRIPTOR_TYPE_CONFIG          0x02
#define USB_DESCRIPTOR_TYPE_STRING          0x03
#define USB_DESCRIPTOR_TYPE_INTERFACE       0x04
#define USB_DESCRIPTOR_TYPE_ENDPOINT        0x05
#define USB_DESCRIPTOR_LANGUAGE_ENGLISH     0x0409

/* Typedefs */
typedef struct {
   ViUInt8 bLength;
   ViUInt8 bDescriptorType;
   ViUInt16 bcdUSB;
   ViUInt8 bDeviceClass;
   ViUInt8 bDeviceSubClass;
   ViUInt8 bDeviceProtocol;
   ViUInt8 bMaxPacketSize0;
   ViUInt16 idVendor;
   ViUInt16 idProduct;
   ViUInt16 bcdDevice;
   ViUInt8 iManufacturer;
   ViUInt8 iProduct;
   ViUInt8 iSerialNumber;
   ViUInt8 bNumConfigurations;
} tUsbDeviceDesc;

typedef struct {
   ViUInt8 bLength;
   ViUInt8 bDescriptorType;
   ViUInt16 wTotalLength;
   ViUInt8 bNumInterfaces;
   ViUInt8 bConfigurationValue;
   ViUInt8 iConfiguration;
   ViUInt8 bmAttributes;
   ViUInt8 MaxPower;
} tUsbConfigDesc;

typedef struct {
   ViUInt8 bLength;
   ViUInt8 bDescriptorType;
   ViUInt8 bInterfaceNumber;
   ViUInt8 bAlternateSetting;
   ViUInt8 bNumEndpoints;
   ViUInt8 bInterfaceClass;
   ViUInt8 bInterfaceSubClass;
   ViUInt8 bInterfaceProtocol;
   ViUInt8 iInterface;
} tUsbInterfaceDesc;

typedef struct {
   ViUInt8 bLength;
   ViUInt8 bDescriptorType;
   ViUInt8 bEndpointAddress;
   ViUInt8 bmAttributes;
   ViUInt16 wMaxPacketSize;
   ViUInt8 bInterval;
} tUsbEndpointDesc ;

typedef struct {
   ViUInt8 bLength;
   ViUInt8 bDescriptorType;
} tUsbCommonDesc;

/* Prototypes */
void GetStringDescriptor(ViUInt8 stringIndex, ViString stringDesc);
void DisplayEndPointTransferType(ViUInt8 bmAttr);
void DisplayDeviceDescriptor(ViSession instrHandle);
void DisplayConfigDescriptor(ViSession instrHandle);
void TraverseConfigDescriptor(ViChar* configDesc);

/* Variables */
static char instrDescriptor[VI_FIND_BUFLEN];
static ViUInt32 numInstrs;
static ViFindList findList;
static ViSession defaultRM, instr;
static ViStatus status;

int main(void)
{
   /* First we will need to open the default resource manager. */
   status = viOpenDefaultRM (&defaultRM);
   if (status < VI_SUCCESS)
   {
      printf("Could not open a session to the VISA Resource Manager!\n");
      exit (EXIT_FAILURE);
   }  

   /* Find all the RAW USB VISA resources in our system and store the  */
   /* number of resources in the system in numInstrs.                  */
   status = viFindRsrc (defaultRM, "USB?*RAW", &findList, &numInstrs, instrDescriptor);
   
   if (status < VI_SUCCESS)
   {
      printf ("An error occurred while finding resources.\nHit enter to continue.");
      fflush(stdin);
      getchar();
      viClose (defaultRM);
      return status;
   }

   printf("%d USB RAW instruments found:\n\n",numInstrs);
   printf("%s \n\n",instrDescriptor);

   /* Now we will open a session to the instrument we just found. */
   status = viOpen (defaultRM, instrDescriptor, VI_NULL, VI_NULL, &instr);
   if (status < VI_SUCCESS)
   {
      printf ("An error occurred opening a session to %s\n",instrDescriptor);
   }
   else
   {
      DisplayDeviceDescriptor(instr);
      DisplayConfigDescriptor(instr);
      viClose (instr);
   }
        
   while (--numInstrs)
   {
      /* stay in this loop until we find all instruments */
      status = viFindNext (findList, instrDescriptor);  /* find next desriptor */
      if (status < VI_SUCCESS) 
      {   /* did we find the next resource? */
         printf ("An error occurred finding the next resource.\nHit enter to continue.");
         fflush(stdin);
         getchar();
         viClose (defaultRM);
         return status;
      } 
      printf("%s \n",instrDescriptor);
    
      /* Now we will open a session to the instrument we just found */
      status = viOpen (defaultRM, instrDescriptor, VI_NULL, VI_NULL, &instr);
      if (status < VI_SUCCESS)
      {
         printf ("An error occurred opening a session to %s\n",instrDescriptor);
      }
      else
      {
         DisplayDeviceDescriptor(instr);
         DisplayConfigDescriptor(instr);
         viClose (instr);
      }
   }    /* end while */

   status = viClose(findList);
   status = viClose(defaultRM);
   printf ("\nHit enter to continue.");
   fflush(stdin);
   getchar();

   return 0;  
}

/*********************************************************************/
/* Retrieve the string descriptor for the device                     */
/* stringIndex: input parameter containing the index for the string  */
/*              descriptor                                           */
/* stringDesc: output buffer to hold the string descriptor           */
/*********************************************************************/

void GetStringDescriptor(ViUInt8 stringIndex, ViString stringDesc)
{
   ViUInt16 inBuffer[VI_FIND_BUFLEN];
   ViUInt16 stringDescLen;   
   ViUInt16 retCount;
   ViUInt16 i;
   tUsbCommonDesc inBufferHeader;
   ViStatus status;
   
   /* Query the length of the string package */
   status = viUsbControlIn (instr, USB_REQUESTTYPE_GET_DESCRIPTOR, USB_REQUEST_GET_DESCRIPTOR, 
    USB_DESCRIPTOR_TYPE_STRING << 8 | stringIndex, USB_DESCRIPTOR_LANGUAGE_ENGLISH, 2, (ViPBuf)&inBufferHeader, &retCount);
   if (status < VI_SUCCESS)
   {
      printf("An error occured when retrieving string descriptor header.\n");
   }

   if (inBufferHeader.bLength > sizeof(inBuffer))
   {
      stringDesc[0] = '\0';
      printf ("String descriptor size %hu greater than maximum supported size %hu\n",inBufferHeader.bLength,sizeof(inBuffer));
      return;
   }
   
   /* Query the String Descriptor */
   status = viUsbControlIn (instr, USB_REQUESTTYPE_GET_DESCRIPTOR, USB_REQUEST_GET_DESCRIPTOR, 
    USB_DESCRIPTOR_TYPE_STRING << 8 | stringIndex, USB_DESCRIPTOR_LANGUAGE_ENGLISH, inBufferHeader.bLength, 
    (ViPBuf)inBuffer, &retCount);
   if (status < VI_SUCCESS)
   {
      printf("An error occured when retrieving string descriptor.\n");
   }

   /* Reformat the string */
   stringDescLen = (inBufferHeader.bLength / 2) - 1;

   for (i = 0; i < stringDescLen; ++i)
   {
      stringDesc[i] = (ViChar)inBuffer[i+1];
   }

   /* Append End of the String */
   stringDesc[stringDescLen]='\0';
}

/*********************************************************************/
/* Print the Endpoint Transfer Type to console output                */
/* bmAttr: bmAttribute value for the Endpoint                        */
/*********************************************************************/
void DisplayEndPointTransferType(ViUInt8 bmAttr)
{
   switch (bmAttr & 0x03)
   {
      case 0:
         printf("Control");
         break;
      case 1:
         printf("Isochronous");
         break;
      case 2:
         printf("Bulk");
         break;
      case 3:
         printf("Interrupt");
         break;
      default:
         printf("Unrecognized Type");
   }
   printf("\n");
}


/*********************************************************************/
/* This function queries the device descriptor using the instrument  */
/* handle that is passed in. Then it print the device descriptor to  */
/* console output                                                    */
/*********************************************************************/
void DisplayDeviceDescriptor(ViSession instrHandle)
{
   ViUInt16 retCount;
   ViChar stringDesc[VI_FIND_BUFLEN];
   tUsbDeviceDesc deviceDesc;
   ViStatus status;
   
   /* Get Device Descriptor */
   status = viUsbControlIn (instrHandle, USB_REQUESTTYPE_GET_DESCRIPTOR, USB_REQUEST_GET_DESCRIPTOR, 
    USB_DESCRIPTOR_TYPE_DEVICE << 8, 0, sizeof(tUsbDeviceDesc), (ViPBuf)&deviceDesc, &retCount);
   if (status < VI_SUCCESS)
   {
      printf("An error occured when retrieving device descriptor.\n");
   }
   
   /* Display Device Descriptor */
   printf("*********************\n");
   printf("* Device Descriptor *\n");
   printf("*********************\n");
   printf("bcdUSB:\t\t\t 0x%04X\n",deviceDesc.bcdUSB);
   printf("bDeviceClass:\t\t 0x%02X\n",deviceDesc.bDeviceClass);
   printf("bDeviceSubClass:\t 0x%02X\n",deviceDesc.bDeviceSubClass);
   printf("bDeviceProtocol:\t 0x%02X\n", deviceDesc.bDeviceProtocol);
   printf("bMaxPacketSize0:\t 0x%02X\n", deviceDesc.bMaxPacketSize0);
   printf("idVendor:\t\t 0x%04X\n", deviceDesc.idVendor);
   printf("idProduct:\t\t 0x%04X\n", deviceDesc.idProduct);
   printf("bcdDevice:\t\t 0x%04X\n", deviceDesc.bcdDevice);
   
   /* A few Device Descriptor items also could include String Descriptors */
   printf("iManufacturer:\t\t 0x%02X\n", deviceDesc.iManufacturer);
   if (deviceDesc.iManufacturer != 0)
   {
      GetStringDescriptor(deviceDesc.iManufacturer,stringDesc);
      printf("\t\t\t \"%s\"\n",stringDesc);
   }

   printf("iProduct:\t\t 0x%02X\n", deviceDesc.iProduct);
   if (deviceDesc.iProduct != 0)
   {
      GetStringDescriptor(deviceDesc.iProduct,stringDesc);
      printf("\t\t\t \"%s\"\n",stringDesc);
   }

   printf("iSerialNumber:\t\t 0x%02X\n", deviceDesc.iSerialNumber);
   if (deviceDesc.iSerialNumber != 0)
   {
      GetStringDescriptor(deviceDesc.iSerialNumber,stringDesc);
      printf("\t\t\t \"%s\"\n",stringDesc);
   }

   printf("bNumConfigurations:\t 0x%02X\n", deviceDesc.bNumConfigurations);   

   printf("\n");
}

/*********************************************************************/
/* This function queries the config descriptor using the instrument  */
/* handle that is passed in. Then it print the config descriptor to  */
/* console output                                                    */
/*********************************************************************/
void DisplayConfigDescriptor(ViSession instrHandle)
{
   tUsbConfigDesc configDescOnly;
   ViStatus status;
   ViUInt16 retCount;
   ViChar configDescComplete[2048];
      
   /* Retrieve the config descriptor */
   status = viUsbControlIn(instrHandle, USB_REQUESTTYPE_GET_DESCRIPTOR, USB_REQUEST_GET_DESCRIPTOR, 
    USB_DESCRIPTOR_TYPE_CONFIG << 8, 0, sizeof(tUsbConfigDesc), 
    (ViPBuf)&configDescOnly, &retCount);
   if (status < VI_SUCCESS)
   {
      printf("An error occured when retrieving configuration descriptor.\n");
   }

   if (configDescOnly.wTotalLength > 2048)
   {
      printf("Actual size of complete configuration descriptor (%lu) is greater than maximum buffer size (%lu)\n", configDescOnly.wTotalLength, 2048);
      return;
   }
   /* Retrieve the entire config descriptors */   
   status = viUsbControlIn(instrHandle, USB_REQUESTTYPE_GET_DESCRIPTOR, USB_REQUEST_GET_DESCRIPTOR, 
    USB_DESCRIPTOR_TYPE_CONFIG << 8, 0, configDescOnly.wTotalLength, 
    (ViPBuf)configDescComplete, &retCount);
   if (status < VI_SUCCESS)
   {
      printf("An error occured when retrieving configuration descriptor.\n");
   }

   /* Traverse and Display the entire config descriptor */
   printf("****************************\n");
   printf("* Configuration Descriptor *\n");
   printf("****************************\n");
   printf("wTotalLength:\t\t 0x%04X\n", configDescOnly.wTotalLength);
   printf("bNumInterfaces:\t\t 0x%02X\n", configDescOnly.bNumInterfaces);
   printf("bConfigurationValue:\t 0x%02X\n", configDescOnly.bConfigurationValue);
   printf("iConfiguration:\t\t 0x%02X\n", configDescOnly.iConfiguration);
   printf("bmAttributes:\t\t 0x%02X\n", configDescOnly.bmAttributes);
   printf("MaxPower:\t\t 0x%02X\n\n", configDescOnly.MaxPower);
   
   TraverseConfigDescriptor(configDescComplete);
}

/*********************************************************************/
/* This function parses the complete config descriptor.              */
/* Then it print the device descriptor to console output.            */
/*********************************************************************/
void TraverseConfigDescriptor(ViChar* pConfigDesc)
{
   ViUInt16 currentOffset    = ((tUsbConfigDesc *)pConfigDesc)->bLength; /* Skip over the config descriptor */
   ViUInt16 totalSize        = ((tUsbConfigDesc *)pConfigDesc)->wTotalLength;
   tUsbInterfaceDesc *       pInterfaceDesc;
   tUsbEndpointDesc *        pEndpointDesc;   
   
   while (currentOffset < totalSize)       
   {
      /* Interface Descriptor */
      if (((tUsbCommonDesc *)(pConfigDesc + currentOffset))->bDescriptorType == USB_DESCRIPTOR_TYPE_INTERFACE)
      {                           
         pInterfaceDesc = (tUsbInterfaceDesc *)(pConfigDesc + currentOffset);
         printf("************************\n");
         printf("* Interface Descriptor *\n");
         printf("************************\n");
         printf("bInterfaceNumber:\t 0x%02X\n",pInterfaceDesc->bInterfaceNumber);
         printf("bAlternatedSetting:\t 0x%02X\n",pInterfaceDesc->bAlternateSetting);
         printf("bNumEndpoints:\t\t 0x%02X\n",pInterfaceDesc->bNumEndpoints);
         printf("bInterfaceClass:\t 0x%02X\n",pInterfaceDesc->bInterfaceClass);
         printf("bInterfaceSubClass:\t 0x%02X\n",pInterfaceDesc->bInterfaceSubClass);
         printf("bInterfaceProtocol:\t 0x%02X\n",pInterfaceDesc->bInterfaceProtocol);
         printf("bInterface:\t\t 0x%02X\n",pInterfaceDesc->iInterface);
         printf("\n");
      }
      /* Endpoint Descriptor */
      else if (((tUsbCommonDesc *)(pConfigDesc + currentOffset))->bDescriptorType == USB_DESCRIPTOR_TYPE_ENDPOINT)
      {   
         pEndpointDesc = (tUsbEndpointDesc *)(pConfigDesc + currentOffset);
         printf("***********************\n");
         printf("* Endpoint Descriptor *\n");
         printf("***********************\n");
         printf("bEndpointAddress:\t 0x%02X\n",pEndpointDesc->bEndpointAddress);
         printf("Transfer Type:\t\t ");
         DisplayEndPointTransferType(pEndpointDesc->bmAttributes);
         printf("wMaxPacketSize:\t\t 0x%02X\n",pEndpointDesc->wMaxPacketSize);
         printf("bInterval:\t\t 0x%02X\n",pEndpointDesc->bInterval);
         printf("\n");
      }
      /* update the iterator */
      currentOffset += ((tUsbCommonDesc *)(pConfigDesc + currentOffset))->bLength;      
   }
}

