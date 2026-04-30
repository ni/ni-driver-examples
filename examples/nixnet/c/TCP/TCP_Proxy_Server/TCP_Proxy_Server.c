/*
   This example creates a TCP proxy server which then listens for incoming connections.
   This example assumes that the port the server listens on is connected in loopback to
   the same module. This is required for the server to accept the client connection.

   A TCP proxy server differs from an OS IP stack server in that the proxy server uses the XNET IP 
   stack to forward all received packets to a given destination. In this case, the server's OS
   socket will be forwarded any packets from its proxy server, which itself is receiving packets
   from the client's proxy server.
*/

#define _CRT_SECURE_NO_WARNINGS

#include <nxsocket.h>                     // various socket and IP stack functions
#include <stdio.h>                        // printf
#include "../TCPExamplesUtilities.h"      // helper functions and useful defines

//=============================================================================
// Main function
//=============================================================================
int main(int argc, char** argv)
{
   if(argc > 2) {
      printf("This program does not accept more than one argument");
      exit(1);
   }

   char input_server_intf[256];

   // Receive input from the command line or use
   // default interface name
   if(argc == 1) {
      strcpy(input_server_intf, "ENET4");
   } else {
      int32_t bytes_wrote = snprintf(input_server_intf, sizeof(input_server_intf), "%s", argv[1]);
      if(bytes_wrote == 255) {
         printf("Input interface name too long. Truncating to 255 characters.\n");
      }
   }

   // Declare a status variable to keep track of any errors.
   nxStatus_t status; 

   // This code block creates an instance of the XNET IP Stack.
   // This stack is configured to simulate a server
   // listening on a given IP address. See TCPExamplesUtilities.h
   // for the precise configuration.
   nxIpStackRef_t server_ref;
   char* config = malloc(4096 + sizeof(input_server_intf));
   snprintf(config, 4096 + sizeof(input_server_intf), PROXY_SERVER_STACK_CONFIG, input_server_intf);
   printf("Creating IP stack...\n");
   status = nxIpStackCreate("Server IP Stack", config, &server_ref);
   free(config);

   if (status == nxSuccess) {
      printf("IP Stack created. Waiting for interface...\n");
   } else {
      // This function will tear down the XNET IP stack
      // instance to prevent leakage.
      nxIpStackClear(server_ref);
      DisplayErrorAndExit(status, "nxIpStackCreate");
   }

   // This function will block the program until the IP stack
   // is able to set up fully. Failure to do so will result
   // in errors.
   status = nxIpStackWaitForInterface(server_ref, "", 5000);

   if (status == nxSuccess) {
      printf("Interface has come up. Creating socket...\n");
   } else {
      nxIpStackClear(server_ref);
      DisplayErrorAndExit(status, "nxIpStackWaitForInterface");
   }

   // See TCPExamplesUtilities.h for more details on OS functions 
   uintptr_t server_socket = osSocket();

   if (server_socket == ~0) {
      nxIpStackClear(server_ref);
      DisplayOSErrorAndExit("osSocket");
   } else {
      printf("Socket created. Binding to interface...\n");
   }

   // Bind the server socket to listen to this address/port pair
   // A properly configured proxy server will "listen" at this
   // location and will forward any connections to the actual OS socket
   struct sockaddr_in bindAddress;
   bindAddress.sin_family = AF_INET;
   inet_pton(AF_INET, "127.0.0.1", &bindAddress.sin_addr);
   bindAddress.sin_port = htons(60004);

   result_t result = osBind(server_socket, &bindAddress);

   if (result.error == 0) {
      printf("Socket has been bound to IP-Port interface. Making socket listen...\n");
   }
   else {
      nxIpStackClear(server_ref);
      DisplayOSErrorAndExit("osBind");
   }
   
   result = osListen(server_socket, 0);

   if (result.error == 0) {
      printf("Socket now listening. Waiting for incoming connections...\n");
   }
   else {
      nxIpStackClear(server_ref);
      DisplayOSErrorAndExit("osListen");
   }

   uintptr_t server_cxn = osAccept(server_socket);

   if (server_cxn == ~0) {
      nxIpStackClear(server_ref);
      DisplayOSErrorAndExit("osAccept");
   }
   else {
      printf("Connection accepted! Receiving data...\n");
   }

   char recv_buf[TCP_MAX_SIZE];
   result = osRecv(server_cxn, recv_buf, TCP_MAX_SIZE, 0); 

   if (result.error != 0) {
      nxIpStackClear(server_ref);
      DisplayOSErrorAndExit("osRecv");
   }
   else {
      printf("Read %d bytes from client.\nRead: %s\nClosing connection...\n", result.value, recv_buf);
   }

   result = osShutdown(server_cxn);

   if (result.error == 0) {
      printf("Connection now closed. Closing sockets...\n");
   }
   else {
      nxIpStackClear(server_ref);
      DisplayOSErrorAndExit("osShutdown");
   }

   result = osClose(server_cxn);

   if (result.error != 0) {
      nxIpStackClear(server_ref);
      DisplayOSErrorAndExit("osClose(server_cxn)");
   }

   result.error = result.error || osClose(server_socket).error;

   if (result.error == 0) {
      printf("Both sockets now closed. Exiting successfully\n");
   }
   else {
      nxIpStackClear(server_ref);
      DisplayOSErrorAndExit("osClose(server_socket)");
   }

   // This function will tear down the XNET IP stack
   // instance to prevent leakage.
   nxIpStackClear(server_ref);

   return 0;
}
