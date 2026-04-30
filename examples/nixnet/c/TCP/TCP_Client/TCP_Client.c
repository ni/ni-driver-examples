/*
   This example creates a client that attempts to connect to an existing server using the TCP protocol.
   This example assumes that the port is connected in loopback on the same
   module. This is required in order for the client to find the server address.
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
   char* config = malloc(2048 + sizeof(input_client_intf));
   snprintf(config, 2048 + sizeof(input_client_intf), CLIENT_STACK_CONFIG, input_client_intf);
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

   // This function creates a TCP socket using the XNET IP Stack instance.
   nxSOCKET client_socket = nxsocket(client_ref, nxAF_INET, nxSOCK_STREAM, nxIPPROTO_TCP);

   if (client_socket == nxINVALID_SOCKET) {
      nxIpStackClear(client_ref);
      DisplaySocketErrorAndExit("Socket creation");
   } else {
      printf("Socket created. Connecting to server...\n");
   }

   // This code block specifies the server address we want to connect to.
   // In this case, 10.0.1.2. The address and port number must be in Big-
   // Endian format, which is done inside StringToIPv4Addr and hostToBig16.
   // We then attempt to connect to the server with the nxconnect function.
   struct nxsockaddr_in server_addr;
   server_addr.sin_family = nxAF_INET;
   server_addr.sin_addr = StringToIPv4Addr(client_ref, "10.0.1.4");
   server_addr.sin_port = hostToBig16(61000);
   nxsocklen_t addrlen = (nxsocklen_t)(sizeof(server_addr));
   status = nxconnect(client_socket, (struct nxsockaddr*) &server_addr, addrlen);

   if (status == nxSuccess) {
      printf("Connection created! Sending data...\n");
         
   } else {
      nxIpStackClear(client_ref);
      DisplaySocketErrorAndExit("Connect");
   }

   // This function sends a simple string over to the server. It
   // will report how many bytes are sent accross the wire.
   char* data_to_send = "The example is working!";
   int32_t send_len = nxsend(client_socket, data_to_send, (int32_t) strlen(data_to_send), 0);

   if (send_len == -1) {
      nxIpStackClear(client_ref);
      DisplaySocketErrorAndExit("Send");
   } else {
      printf("Sent %d bytes to the server. Awaiting close from server...\n", send_len);
   }

   // The nxrecv function is used to receive any incoming data
   // from the server. This function will block if no data is 
   // transmitted. However, it will unblock if the connection
   // is reset or a FIN packet is sent, which will read as
   // 0 bytes read. In this case, we can close our side of the
   // connection safely.
   char recv_buf[TCP_MAX_SIZE];
   int32_t read_len = nxrecv(client_socket, recv_buf, sizeof(recv_buf), 0);

   if (read_len == 0) {
      // This function will send a FIN packet to the server
      // and begin the teardown process of the connection.
      // This function is good practice, but not required,
      // to close a socket.
      status = nxshutdown(client_socket, nxSHUT_RDWR);
   }

   if (status == nxSuccess) {
      printf("Connection now closed. Closing sockets...\n");
   }
   else {
      nxIpStackClear(client_ref);
      DisplaySocketErrorAndExit("Shutdown");
   }

   // This function will finally close our socket. Should this
   // be called without nxshutdown, a RST packet will be sent.
   status = nxclose(client_socket);

   if (status == nxSuccess) {
      printf("Socket is now closed. Exiting successfully\n");
   }
   else {
      nxIpStackClear(client_ref);
      DisplaySocketErrorAndExit("Close");
   }

   // This function will tear down the XNET IP stack
   // instance to prevent leakage.
   nxIpStackClear(client_ref);

   return 0;
}
