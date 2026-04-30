#ifndef ETHERNET_EXAMPLES_UTILITIES_H
#define ETHERNET_EXAMPLES_UTILITIES_H

#include <nixnet.h> // Include file for NI-XNET functions and constants

typedef u32 bool;
#ifndef TRUE
#define TRUE (1L)
#endif
#ifndef FALSE
#define FALSE (0L)
#endif

// To support older MSVC compilers, define some constants using #define
// when they are used to express array size.
#define FCS_SIZE 4
#define MAX_ENET_FRAME_SIZE 1518

static const u8 kEnetFrameOffsetEtherType = 12;
static const u8 kEnetFrameOffsetVlanEtherType = 16;
static const u8 kEnetFrameOffsetVlanTag = 12;
static const u8 kEnetFrameOffsetDstMacAddr = 0;
static const u8 kEnetFrameOffsetSrcMacAddr = 6;
static const u8 kPcp = 3;
static const u16 kVlanId = 2;
static const u32 kEnableVidAndPriority = 3;
static const u32 kEnetFrameHeaderSize = 14;
static const u32 kEnetFrameVlanTaggedHeaderSize = 18;
static const u32 kEnetMacAddrSize = 6;
static const u32 kFcsSize = FCS_SIZE;
static const u32 kMaxEnetFrameSize = MAX_ENET_FRAME_SIZE;

struct EthernetHeader
{
   u8 DestinationMacAddress[6];
   u8 SourceMacAddress[6];
   u16 EtherType;
} EthernetHeader;

#define CHOOSE_MONITOR_OR_ENDPOINT_TEXT "\nPress \'m\' followed by enter to configure the input stream session to use the monitor path"\
                                        " to monitor all network traffic else just the enter key to use the endpoint path which filters"\
                                        " traffic based on VLAN ID and priority.\n"

#endif // ETHERNET_EXAMPLES_UTILITIES_H