/*==============================================================================*/
/*                                                                              */
/* Title:       NI-XNET-Helper.h                                                */
/* Purpose:     Header for the NI-XNET-Helper.c file                            */
/*                                                                              */
/*==============================================================================*/

#ifndef NI_XNET_HELPER_H
#define NI_XNET_HELPER_H

#ifdef __cplusplus
    extern "C" {
#endif

/*==============================================================================*/
/* Include files */
#include <nixnet.h>
#include <stdio.h>

/*==============================================================================*/
/* Constants */

/*==============================================================================*/
/* Types */

/*==============================================================================*/
/* External variables */

/*==============================================================================*/
/* Global functions */

nxStatus_t InitFixedCANFrame(nxFrameCAN_t *frame, u32 Identifier, u8 Type,
   u8 Flags, u8 Info, u8 *Payload);

nxStatus_t InitVariableCANFrame(nxFrameVar_t *frame, u32 Identifier, u8 Type,
   u8 Flags, u8 Info, u32 PayloadLength, u8 *Payload);

nxStatus_t nxExampleGetDBAlias(u32 *pNumberOfDatabases, char ***pDatabaseAliasArray);

nxStatus_t nxExampleGetDBFilepaths(u32 *pNumberOfDatabases,
   char ***pDatabaseFilepathArray);

nxStatus_t GetClusterNamesByProtocol(char *DatabaseName, u32 *NumberOfClusters,
   char ***ClusterNames, u32 Protocol);

nxStatus_t GetClusterNames(char *DatabaseName, u32 *NumberOfClusters,
   char ***ClusterNames);

nxStatus_t GetCANClusterNames(char *DatabaseName, u32 *NumberOfClusters,
   char ***ClusterNames);

nxStatus_t GetLINClusterNames(char *DatabaseName, u32 *NumberOfClusters,
   char ***ClusterNames);

nxStatus_t GetFlexRayClusterNames(char *DatabaseName, u32 *NumberOfClusters,
   char ***ClusterNames);

nxStatus_t GetLINScheduleNames(char *DatabaseName, char *ClusterName,
   u32 *NumberOfSchedules, char ***ScheduleNames);

nxStatus_t GetSignalsNames(char *DatabaseName, char *ClusterName,
   u32 *NumberSignals, char ***SignalNames);

nxStatus_t GetFrameNames(char *DatabaseName, char *ClusterName, u32 *NumberFrames,
   char ***FrameNames);

nxStatus_t GetPDUNames(char *DatabaseName, char *ClusterName, u32 *NumberPDUs,
   char ***PDUNames);

nxStatus_t GetFramePayloadLength(char *DatabaseName, char *ClusterName,
   char *FrameName,u32 *PayloadLength);

nxStatus_t GetPDUPayloadLength(char *DatabaseName, char *ClusterName,
   char *PDUName, u32 *PayloadLength);

nxStatus_t nxExampleGetDBFilepathsWithIP(char *IPAddress, u32 *pNumberOfDatabases,
   char ***pDatabaseFilepathArray);

nxStatus_t nxExampleGetDBAliasWithIP(char *IPAddress, u32 *pNumberOfDatabases,
   char ***pDatabaseAliasArray);

void AbsTimeToString(nxTimestamp_t *time, char *TimeString);

nxStatus_t GetAllInterfacesByProtocol(u32 *numberOfInterfaces,
   char ***InterfacesNames, u32 Protocol);

nxStatus_t GetAllInterfaces(u32 *numberOfInterfaces,
   char ***InterfacesNames);

nxStatus_t GetAllCANInterfaces(u32 *numberOfInterfaces,
   char ***InterfacesNames);

nxStatus_t GetAllFlexRayInterfaces(u32 *numberOfInterfaces,
   char ***InterfacesNames);

nxStatus_t GetAllLINInterfaces(u32 *numberOfInterfaces,
   char ***InterfacesNames);

nxStatus_t GetSignalsNamesOfFrame(char *DatabaseName, char *ClusterName, char *FrameName,
   u32 *NumberSignals, char ***SignalNames);

nxStatus_t GetSignalsNamesOfPDU(char *DatabaseName, char *ClusterName, char *PduName,
   u32 *NumberSignals, char ***SignalNames);

void CriticalErrorHandler(void);

#ifdef __cplusplus
    }
#endif

#endif  /* ifndef NI_XNET_HELPER_H */

