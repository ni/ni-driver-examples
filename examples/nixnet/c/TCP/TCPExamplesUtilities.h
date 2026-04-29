#ifndef TCP_EXAMPLES_UTILITIES_H
#define TCP_EXAMPLES_UTILITIES_H

#include <nixnet.h>                    // Include file for NI-XNET functions and constants
#include <nxsocket.h>                  // Various socket and IP stack functions
#include <stdio.h>                     // printf, getchar
#include <stdlib.h>                    // Include file for various C functions
#include <string.h>                    // strlen, strerror for Linux

#if defined _WIN64 || defined _WIN32

#include <winsock2.h>                  // Includes Windows-specific socket functions
#include <ws2tcpip.h>                  // Various Windows socket data types
#include <windows.h>                   // Include file on Windows for Sleep

#pragma comment(lib, "ws2_32.lib")     // To link against winsock

#elif defined __linux__

#include <sys/socket.h>                // Include Linux-specific socket functions
#include <errno.h>                     // For errno
#include <unistd.h>                    // Include file on Linux for sleep
#include <arpa/inet.h>                 // inet_pton
#include <netinet/in.h>                // IPPROTO_TCP, sockaddr_in

#endif

//=============================================================================
// Forward Declarations
//=============================================================================                              
void DisplayErrorAndExit(nxStatus_t Status, char* Source);

//=============================================================================
// Type Definitions
//=============================================================================      
typedef struct _result
{
   int value;
   int error;
} result_t;

//=============================================================================
// Defines
//=============================================================================  
#define TCP_MAX_SIZE 65536

#define CLIENT_STACK_CONFIG "{ \
                                 \"schema\":\"file:///NIXNET_Documentation/xnetIpStackSchema-07.json\", \
                                 \"xnetInterfaces\": \
                                 [{ \
                                    \"name\":\"%s\", \
                                    \"MACs\": \
                                    [{ \
                                       \"address\":\"generated\", \
                                       \"VLANs\": \
                                       [{ \
                                          \"name\":\"\", \
                                          \"isTagged\":false, \
                                          \"id\":0, \
                                          \"priority\":0, \
                                          \"IPv4\": \
                                          { \
                                             \"mode\":\"static\", \
                                             \"staticAddresses\": \
                                             [{ \
                                                \"address\":\"10.0.1.5\", \
                                                \"subnetMask\":\"255.255.255.0\" \
                                             }] \
                                          }, \
                                          \"IPv6\": \
                                          { \
                                             \"mode\":\"disabled\" \
                                          }, \
                                          \"staticArpTable\":[] \
                                       }] \
                                    }], \
                                    \"staticArpTable\":[] \
                                 }], \
                                 \"proxyServers\":[] \
                              }"

#define SERVER_STACK_CONFIG "{ \
                                 \"schema\":\"file:///NIXNET_Documentation/xnetIpStackSchema-07.json\", \
                                 \"xnetInterfaces\": \
                                 [{ \
                                    \"name\":\"%s\", \
                                    \"MACs\": \
                                    [{ \
                                       \"address\":\"generated\", \
                                       \"VLANs\": \
                                       [{ \
                                          \"name\":\"\", \
                                          \"isTagged\":false, \
                                          \"id\":0, \
                                          \"priority\":0, \
                                          \"IPv4\": \
                                          { \
                                             \"mode\":\"static\", \
                                             \"staticAddresses\": \
                                             [{ \
                                                \"address\":\"10.0.1.4\", \
                                                \"subnetMask\":\"255.255.255.0\" \
                                             }] \
                                          }, \
                                          \"IPv6\": \
                                          { \
                                             \"mode\":\"disabled\" \
                                          }, \
                                          \"staticArpTable\":[] \
                                       }] \
                                    }], \
                                    \"staticArpTable\":[] \
                                 }], \
                                 \"proxyServers\":[] \
                              }"

#define PROXY_CLIENT_STACK_CONFIG "{ \
                                 \"schema\":\"file:///NIXNET_Documentation/xnetIpStackSchema-07.json\", \
                                 \"xnetInterfaces\": \
                                 [{ \
                                    \"name\":\"%s\", \
                                    \"MACs\": \
                                    [{ \
                                       \"address\":\"generated\", \
                                       \"VLANs\": \
                                       [{ \
                                          \"name\":\"\", \
                                          \"isTagged\":false, \
                                          \"id\":0, \
                                          \"priority\":0, \
                                          \"IPv4\": \
                                          { \
                                             \"mode\":\"static\", \
                                             \"staticAddresses\": \
                                             [{ \
                                                \"address\":\"10.0.1.5\", \
                                                \"subnetMask\":\"255.255.255.0\" \
                                             }] \
                                          }, \
                                          \"IPv6\": \
                                          { \
                                             \"mode\":\"disabled\" \
                                          }, \
                                          \"staticArpTable\":[] \
                                       }] \
                                    }], \
                                    \"staticArpTable\":[] \
                                 }], \
                                 \"proxyServers\": \
                                 [{ \
                                    \"protocol\":\"tcp\", \
                                    \"direction\":\"outbound\", \
                                    \"listenAddress\":\"\", \
                                    \"listenPort\":60002, \
                                    \"forwardingDestinationAddress\":\"10.0.1.4\", \
                                    \"forwardingDestinationPort\":60003, \
                                    \"forwardingSourceAddress\":\"\", \
                                    \"forwardingSourcePort\":0 \
                                 }] \
                              }"

#define PROXY_SERVER_STACK_CONFIG "{ \
                                 \"schema\":\"file:///NIXNET_Documentation/xnetIpStackSchema-07.json\", \
                                 \"xnetInterfaces\": \
                                 [{ \
                                    \"name\":\"%s\", \
                                    \"MACs\": \
                                    [{ \
                                       \"address\":\"generated\", \
                                       \"VLANs\": \
                                       [{ \
                                          \"name\":\"\", \
                                          \"isTagged\":false, \
                                          \"id\":0, \
                                          \"priority\":0, \
                                          \"IPv4\": \
                                          { \
                                             \"mode\":\"static\", \
                                             \"staticAddresses\": \
                                             [{ \
                                                \"address\":\"10.0.1.4\", \
                                                \"subnetMask\":\"255.255.255.0\" \
                                             }] \
                                          }, \
                                          \"IPv6\": \
                                          { \
                                             \"mode\":\"disabled\" \
                                          }, \
                                          \"staticArpTable\":[] \
                                       }] \
                                    }], \
                                    \"staticArpTable\":[] \
                                 }], \
                                 \"proxyServers\": \
                                 [{ \
                                    \"protocol\":\"tcp\", \
                                    \"direction\":\"inbound\", \
                                    \"listenAddress\":\"\", \
                                    \"listenPort\":60003, \
                                    \"forwardingDestinationAddress\":\"127.0.0.1\", \
                                    \"forwardingDestinationPort\":60004, \
                                    \"forwardingSourceAddress\":\"\", \
                                    \"forwardingSourcePort\":0 \
                                 }] \
                              }"

// Change this value to 1 if your architecture uses Big endian bitness
#ifndef NIXNET_BIG_ENDIAN
   #define NIXNET_BIG_ENDIAN 0
#endif


//=============================================================================
// Helper Functions
//=============================================================================
struct nxin_addr StringToIPv4Addr(nxIpStackRef_t stack_ref, const char* addr)
{
   struct nxin_addr ipv4;
   int32_t ret = nxinet_pton(stack_ref, nxAF_INET, addr, &ipv4.addr);
   if (ret < 1) {
      DisplayErrorAndExit(ret, "StringToIPv4Addr");
   }
   return ipv4;
}

uint32_t osGetError()
{
#if defined _WIN64 || defined _WIN32
   return (uint32_t) WSAGetLastError();
#elif defined __linux__
   return (uint32_t) errno;
#endif
}

void DisplayErrorAndExit(nxStatus_t Status, char* Source)
{
   char l_StatusString[1024];
   nxStatusToString(Status, sizeof(l_StatusString), l_StatusString);

   printf("\n\nERROR at %s!\n%s\n", Source, l_StatusString);
   printf("\nExecution stopped.\nPress any key to quit\n");

   getchar();
   exit(1);
}

void DisplaySocketErrorAndExit(char* Source) 
{
   char errstr[256];
   int32_t errnum = nxgetlasterrornum();
   nxstrerr_r(errnum, errstr, sizeof(errstr));
   printf("%s failed with error:  %s", Source, errstr);
   printf("\nExecution stopped.\nPress any key to quit\n");

   getchar();
   exit(1);
}

void DisplayOSErrorAndExit(char* Source)
{

   uint32_t error = osGetError(); 
   
#if defined _WIN64 || defined _WIN32
   char* err_str = NULL;
   FormatMessage(FORMAT_MESSAGE_ALLOCATE_BUFFER | FORMAT_MESSAGE_FROM_SYSTEM | FORMAT_MESSAGE_IGNORE_INSERTS, NULL, error, LANG_SYSTEM_DEFAULT, err_str, 0, NULL);
   printf("Error performing OS operation %s: Code %d, %s\nExiting.", Source, error, err_str);
   LocalFree(err_str);
#elif defined __linux__
   char* err_str = strerror(error);
   printf("Error performing OS operation %s: Code %d, %s\nExiting.", Source, error, err_str);
#endif
   printf("\nExecution stopped.\nPress any key to quit\n");

   getchar();
   exit(1);
}

void sleepForMs(uint32_t ms) {
#if defined _WIN64 || defined _WIN32
   Sleep(ms);
#elif defined __linux__
   sleep(ms);
#endif
}


/*
   osSocket()
   Creates a socket using the OS IP stack, either Linux or Windows

   Arguments: N/A
   Returns: Unsigned 64-bit int handle/FD

   On error: Returns ~0, in binary, 111....111, which is the only value not used by either OS for valid sockets

*/
uintptr_t osSocket() 
{
#if defined _WIN64 || defined _WIN32
   uintptr_t socket_ = socket(AF_INET, SOCK_STREAM, IPPROTO_TCP);
#elif defined __linux__
   int32_t socket_ = socket(AF_INET, SOCK_STREAM, IPPROTO_TCP);
#endif

   if(socket_ < 0 || socket_ == ~0) {
      return (uintptr_t) ~0;
   }

   return socket_;
}

/*
   osBind()
   Binds an OS socket

   Arguments: Socket handle, socket address to use
   Returns: Unsigned 32-bit status value, 0 if success

   On error: Returns a nonnegative error code for the respective OS
   
*/
result_t osBind(uintptr_t socket, struct sockaddr_in* address)
{
   result_t result = {0};

   socklen_t addr_len = sizeof(struct sockaddr_in);

   result.value = bind(socket, (struct sockaddr*) address, addr_len);

   if (result.value < 0) {
      result.error = osGetError();
   }

   return result;
}

/*
   osListen()
   Listens for incoming connections on a bound OS socket

   Arguments: Socket handle, packet backlog
   Returns: Unsigned 32-bit status value, 0 if success

   On error: Returns a nonnegative error code for the respective OS
   
*/
result_t osListen(uintptr_t socket, int32_t backlog)
{
   result_t result = {0};

   result.value = listen(socket, backlog);

   if (result.value < 0) {
      result.error = osGetError();
   }

   return result;
}

/*
   osAccept()
   Creates a socket for an incoming connection request

   Arguments: Handle for the listening socket
   Returns: Unsigned 64-bit int handle/FD

   On error: Returns ~0, in binary, 111....111, which is the only value not used by either OS for valid sockets

*/
uintptr_t osAccept(uintptr_t socket)
{
#if defined _WIN64 || defined _WIN32
   uintptr_t new_socket = accept(socket, NULL, NULL);
#elif defined __linux__
   int32_t new_socket = accept(socket, NULL, NULL);
#endif

   if (new_socket < 0 || new_socket == ~0) {
      return (uintptr_t) ~0;
   }
   return new_socket;
}

/*
   osConnect()
   Attempt to send a connection request to a listening server socket

   Arguments: Socket handle, server address
   Returns: Unsigned 32-bit status value, 0 if success

   On error: Returns a nonnegative error code for the respective OS
   
*/
result_t osConnect(uintptr_t socket, struct sockaddr_in* address)
{
   result_t result = {0};

   socklen_t addr_len = sizeof(struct sockaddr_in);
   result.value = connect(socket, (struct sockaddr*) address, addr_len);

   if (result.value < 0) {
      result.error = osGetError();
   }

   return result;
}

/*
   osSend()
   Send packets over the given socket

   Arguments: Socket handle, the data to send (as a buffer), the length of the data (in bytes), call flags
   Returns: Signed 32-bit int, number of bytes sent over the socket if success

   On error: Returns the NEGATIVE error code for the respective OS
   Note: Cannot use normal nonnegative codes due to ambiguity of success/failure cases
   
*/
result_t osSend(uintptr_t socket, const char* buff, size_t len, int32_t flags)
{
   result_t result = {0};

   result.value = send(socket, buff, (int) len, flags);

   if (result.value < 0) {
      result.error = osGetError();
   }

   return result;
}

/*
   osRecv()
   Receive packets from the given socket

   Arguments: Socket handle, the buffer to receive the data in, the length of the buffer (in bytes), call flags
   Returns: Signed 32-bit int, number of bytes received from the socket if success

   On error: Returns the NEGATIVE error code for the respective OS
   Note: Cannot use normal nonnegative codes due to ambiguity of success/failure cases
   
*/
result_t osRecv(uintptr_t socket, char* buff, size_t len, int32_t flags)
{

   result_t result = {0};
   
   result.value = recv(socket, buff, (int) len, flags);

   if (result.value < 0) {
      result.error = osGetError();
   }

   return result;
}

/*
   osShutdown()
   Disable sending/receiving packets on this socket

   Arguments: Socket handle
   Returns: Unsigned 32-bit status value, 0 if success

   On error: Returns a nonnegative error code for the respective OS
   
*/
result_t osShutdown(uintptr_t socket)
{
   result_t result = {0};

   // 2 = SD_BOTH = SHUT_RDWR
   result.value = shutdown(socket, 2);

   if (result.value < 0) {
      result.error = osGetError();
   }

   return result;
}

/*
   osShutdown()
   Disable sending/receiving packets on this socket

   Arguments: Socket handle
   Returns: Unsigned 32-bit status value, 0 if success

   On error: Returns a nonnegative error code for the respective OS
   
*/
result_t osClose(uintptr_t socket)
{
   result_t result = {0};

#if defined _WIN64 || defined _WIN32
   result.value = closesocket(socket);
#elif defined __linux__
   result.value = close(socket);
#endif

   if (result.value < 0) {
      result.error = osGetError();
   }

   return result;
}

/*! changeEndianness16()
   do an endian conversion on 16-bit data
   /param data              the data to byte swap
   /return the swapped data
*/
uint16_t changeEndianness16 (const uint16_t data)
{
   return (((data << 8) & 0xff00) | ((data >> 8) & 0x00ff));
}

/*! changeEndianness32()
   do an endian conversion on 32-bit data
   /param data              the data to byte swap
   /return the swapped data
*/
uint32_t changeEndianness32 (const uint32_t data)
{
   return (((data << 24) & 0xff000000) | ((data << 8) & 0x00ff0000) | \
           ((data >> 8) & 0x0000ff00) | ((data >> 24) & 0x000000ff));
}

/*! changeEndianness64()
   do an endian conversion on 64-bit data
   /param data              the data to byte swap
   /return the swapped data
*/
uint64_t changeEndianness64 (const uint64_t data)
{
   return (
           ((data << 56) & 0xff00000000000000LL) | 
           ((data << 40) & 0x00ff000000000000LL) |
           ((data << 24) & 0x0000ff0000000000LL) | 
           ((data <<  8) & 0x000000ff00000000LL) |
           ((data >>  8) & 0x00000000ff000000LL) | 
           ((data >> 24) & 0x0000000000ff0000LL) |
           ((data >> 40) & 0x000000000000ff00LL) |
           ((data >> 56) & 0x00000000000000ffLL)
           );
}

#if NIXNET_BIG_ENDIAN
   uint16_t hostToBig16 (const uint16_t data) {return data;}
   uint32_t hostToBig32 (const uint32_t data) {return data;}
   uint64_t hostToBig64 (const uint64_t data) {return data;}
   uint16_t bigToHost16 (const uint16_t data) {return data;}
   uint32_t bigToHost32 (const uint32_t data) {return data;}
   uint64_t bigToHost64 (const uint64_t data) {return data;}

   uint16_t hostToLittle16 (const uint16_t data) {return changeEndianness16(data);}
   uint32_t hostToLittle32 (const uint32_t data) {return changeEndianness32(data);}
   uint64_t hostToLittle64 (const uint64_t data) {return changeEndianness64(data);}
   uint16_t littleToHost16 (const uint16_t data) {return changeEndianness16(data);}
   uint32_t littleToHost32 (const uint32_t data) {return changeEndianness32(data);}
   uint64_t littleToHost64 (const uint64_t data) {return changeEndianness64(data);}
#else  // !NIXNET_BIG_ENDIAN
   uint16_t hostToBig16 (const uint16_t data) {return changeEndianness16(data);}
   uint32_t hostToBig32 (const uint32_t data) {return changeEndianness32(data);}
   uint64_t hostToBig64 (const uint64_t data) {return changeEndianness64(data);}
   uint16_t bigToHost16 (const uint16_t data) {return changeEndianness16(data);}
   uint32_t bigToHost32 (const uint32_t data) {return changeEndianness32(data);}
   uint64_t bigToHost64 (const uint64_t data) {return changeEndianness64(data);}

   uint16_t hostToLittle16 (const uint16_t data) {return data;}
   uint32_t hostToLittle32 (const uint32_t data) {return data;}
   uint64_t hostToLittle64 (const uint64_t data) {return data;}
   uint16_t littleToHost16 (const uint16_t data) {return data;}
   uint32_t littleToHost32 (const uint32_t data) {return data;}
   uint64_t littleToHost64 (const uint64_t data) {return data;}
#endif // NIXNET_BIG_ENDIAN

#endif // TCP_EXAMPLES_UTILITIES_H