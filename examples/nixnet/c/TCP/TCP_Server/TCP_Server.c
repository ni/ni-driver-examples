/*
   This example creates a server and listens for an incoming client using the TCP protocol.
   This example assumes that the port is connected in loopback on the same
   module. This is required in order for the server to accept the client
   connection.
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
      int bytes_wrote = snprintf(input_server_intf, sizeof(input_server_intf), "%s", argv[1]);
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
   char* config = malloc(2048 + sizeof(input_server_intf));
   snprintf(config, 2048 + sizeof(input_server_intf), SERVER_STACK_CONFIG, input_server_intf);
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

   // This function creates a TCP socket using the XNET IP Stack instance.
   // We will be listening on this socket for incoming connection requests.
   nxSOCKET server_socket = nxsocket(server_ref, nxAF_INET, nxSOCK_STREAM, nxIPPROTO_TCP);

   if (server_socket == nxINVALID_SOCKET) {
      nxIpStackClear(server_ref);
      DisplaySocketErrorAndExit("Socket Creation");
   } else {
      printf("Socket created. Binding to interface...\n");
   }

   // This code block specifies the IP address and port we will be listening on.
   // In this case, 10.0.1.2. The address and port number must be in Big-
   // Endian format, which is done inside StringToIPv4Addr and hostToBig16.
   // We then bind the socket.
   struct nxsockaddr_in bindAddress;
   bindAddress.sin_family = nxAF_INET;
   bindAddress.sin_addr = StringToIPv4Addr(server_ref, "10.0.1.4");
   bindAddress.sin_port = hostToBig16(61000);
   nxsocklen_t addrlen = (nxsocklen_t)(sizeof(bindAddress));

   status = nxbind(server_socket, (struct nxsockaddr*) &bindAddress, addrlen);

   if (status == nxSuccess) {
      printf("Socket has been bound to IP-Port interface. Making socket listen...\n");
   }
   else {
      nxIpStackClear(server_ref);
      DisplaySocketErrorAndExit("Bind");
   }
   
   // This function will listen on our created socket.
   status = nxlisten(server_socket, 0);

   if (status == nxSuccess) {
      printf("Socket now listening. Waiting for incoming connections...\n");
   }
   else {
      nxIpStackClear(server_ref);
      DisplaySocketErrorAndExit("Listen");
   }

   // This function will block the server until an incoming connection
   // is detected, at which point the server will create a second
   // socket to use for the connection, leaving the original socket
   // for other incoming connections.
   struct nxsockaddr_in peer_addr;
   nxsocklen_t peer_len = (nxsocklen_t) sizeof(peer_addr);
   nxSOCKET server_cxn = nxaccept(server_socket, (struct nxsockaddr*) &(peer_addr), &peer_len);

   if (server_cxn == nxINVALID_SOCKET) {
      nxIpStackClear(server_ref);
     DisplaySocketErrorAndExit("Accept");
   } else {
      printf("Connection accepted! Receiving data...\n");
   }

   // The nxrecv function is used to receive any incoming data
   // from the client. This function will block if no data is 
   // transmitted. However, it will unblock if the connection
   // is reset or a FIN packet is sent, which will read as
   // 0 bytes read.
   char recv_buf[TCP_MAX_SIZE];
   int32_t read_len = nxrecv(server_cxn, recv_buf, sizeof(recv_buf), 0);

   if (read_len == -1) {
      nxIpStackClear(server_ref);
      DisplaySocketErrorAndExit("Receive");
   } else {
      printf("Read %d bytes from client.\nRead: %s\nClosing connection...\n", read_len, recv_buf);
   }

   // This function will send a FIN packet to the client
   // and begin the teardown process of the connection.
   // This function is good practice, but not required,
   // to close a socket.
   status = nxshutdown(server_cxn, nxSHUT_RDWR);

   if (status == nxSuccess) {
      printf("Connection now closed. Closing sockets...\n");
   }
   else {
      nxIpStackClear(server_ref);
      DisplaySocketErrorAndExit("Shutdown");
   }

   // This function will finally close our socket. Should this
   // be called without nxshutdown, a RST packet will be sent.
   status = nxclose(server_cxn);

   if(status != nxSuccess) {
      nxIpStackClear(server_ref);
      DisplaySocketErrorAndExit("Close(server_cxn)");
   }

   status = nxclose(server_socket);

   if (status == nxSuccess) {
      printf("Both sockets now closed. Exiting successfully\n");
   }
   else {
      nxIpStackClear(server_ref);
      DisplaySocketErrorAndExit("Close(server_socket)");
   }

   // This function will tear down the XNET IP stack
   // instance to prevent leakage.
   nxIpStackClear(server_ref);

   return 0;
}
