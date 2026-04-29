/*
   This example creates a TCP proxy client that attempts to connect to an existing server.
   This example assumes that the port is connected in loopback on the same
   module. This is required in order for the client to find the server address.

   This example uses a TCP proxy server, which differs from an OS IP stack
   server in that the proxy server uses the XNET IP stack to forward all 
   received packets to a given destination. In this case, the client's OS socket
   will send all packets through its proxy server, which will then forward them
   to the server's proxy server.
*/

#define _CRT_SECURE_NO_WARNINGS

#include <nxsocket.h>                     // various socket and IP stack functions
#include <stdio.h>                        // printf
#include <string.h>                       // strlen
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

   char input_client_intf[256];

   // Receive input from the command line or use
   // default interface name
   if(argc == 1) {
      strcpy(input_client_intf, "ENET1");
   } else {
      snprintf(input_client_intf, sizeof(input_client_intf), "%s", argv[1]);
   }

   // Declare a status variable to keep track of any errors.
   nxStatus_t status; 

   // This code block creates an instance of the XNET IP Stack.
   // This stack is configured to simulate a client
   // coming from a separate IP address. See TCPExamplesUtilities.h
   // for the precise configuration.
   nxIpStackRef_t client_ref;
   char* config = malloc(4096 + sizeof(input_client_intf));
   snprintf(config, 4096 + sizeof(input_client_intf), PROXY_CLIENT_STACK_CONFIG, input_client_intf);
   printf("Creating IP stack...\n");
   status = nxIpStackCreate("Client IP Stack", config, &client_ref);
   free(config);

   if (status == nxSuccess) {
      printf("IP Stack created. Waiting for interface...\n");
   } else {
      // This function will tear down the XNET IP stack
      // instance to prevent leakage.
      nxIpStackClear(client_ref);
      DisplayErrorAndExit(status, "nxIpStackCreate");
   }

   // This function will block the program until the IP stack
   // is able to set up fully. Failure to do so will result
   // in errors.
   status = nxIpStackWaitForInterface(client_ref, "", 5000);

   if (status == nxSuccess) {
      printf("Interface has come up. Creating socket...\n");
   } else {
      nxIpStackClear(client_ref);
      DisplayErrorAndExit(status, "nxIpStackWaitForInterface");
   }

   uintptr_t client_socket = osSocket();

   if (client_socket == ~0) {
      nxIpStackClear(client_ref);
      DisplayOSErrorAndExit("osSocket");
   } else {
      printf("Socket created. Connecting to server...\n");
   }

   // Bind the client socket to explicitly indicate where
   // the connection is going to be coming from. Otherwise the
   // OS will pick an address-port pair for unbound client sockets
   // which not work with a proxy server
   struct sockaddr_in bindAddress;
   bindAddress.sin_family = AF_INET;
   inet_pton(AF_INET, "127.0.0.1", &bindAddress.sin_addr);
   bindAddress.sin_port = htons(60001);

   result_t result = osBind(client_socket, &bindAddress);

   if (result.error == 0) {
      printf("Socket has been bound to IP-Port interface. Making socket listen...\n");
   }
   else {
      nxIpStackClear(client_ref);
      DisplayOSErrorAndExit("osBind");
   }

   // Attempt to connect to the server
   // Recall that a properly configured proxy server "listens" 
   // on the created servers OS socket, so we use that address
   struct sockaddr_in server_addr;
   server_addr.sin_family = AF_INET;
   inet_pton(AF_INET, "127.0.0.1", &server_addr.sin_addr);
   server_addr.sin_port = htons(60002);

   result = osConnect(client_socket, &server_addr);

   if (result.error == 0) {
      printf("Connection created! Sending data...\n");
   } else {
      nxIpStackClear(client_ref);
      DisplayOSErrorAndExit("osConnect");
   }

   char* data_to_send = "The example is working!";
   result = osSend(client_socket, data_to_send, strlen(data_to_send), 0);

   if (result.error != 0) {
      nxIpStackClear(client_ref);
      DisplayOSErrorAndExit("osSend");
   } else {
      printf("Sent %d bytes to the server. Awaiting close from server...\n", result.value);
   }

   char recv_buf[TCP_MAX_SIZE];
   result = osRecv(client_socket, recv_buf, TCP_MAX_SIZE, 0); 
   
   if (result.value == 0) {
      result = osShutdown(client_socket);
   }

   if (result.error == 0) {
      printf("Connection now closed. Closing sockets...\n");
   }
   else {
      nxIpStackClear(client_ref);
      DisplayOSErrorAndExit("osShutdown");
   }

   result = osClose(client_socket);

   if (result.error == 0) {
      printf("Socket is now closed. Exiting successfully\n");
   }
   else {
      nxIpStackClear(client_ref);
      DisplayOSErrorAndExit("osClose");
   }

   // This function will tear down the XNET IP stack
   // instance to prevent leakage.
   nxIpStackClear(client_ref);

   return 0;
}
