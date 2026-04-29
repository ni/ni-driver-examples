/*==============================================================================*/
/*                                                                              */
/* Title:       NI-XNET-Helper.c                                                */
/* Purpose:     This file is designed to provide some helper functions          */
/*              for the CVI XNET API                                            */
/*                                                                              */
/*==============================================================================*/

#define _CRT_SECURE_NO_WARNINGS

/*==============================================================================*/
/* Include files                                                                */
/*==============================================================================*/

#include "NI-XNET-Helper.h"
#include <stdlib.h>     /* Include file for various functions */
#include <string.h>

// Helper macros (only valid for XNET Status variables)
#define IsError(Status) (Status < nxSuccess)
#define IsNoError(Status) (Status >= nxSuccess)

/*=============================================================================*/
/* Initializes a fixed frame with a payload of 8 bytes                         */
/*=============================================================================*/
nxStatus_t InitFixedCANFrame(nxFrameCAN_t *frame, u32 Identifier, u8 Type,
   u8 Flags, u8 Info, u8 *Payload)
{
   unsigned int i = 0;
   frame->Timestamp = 0;
   frame->Identifier = Identifier;
   frame->Type = Type;
   frame->Flags = Flags;
   frame->Info = Info;
   frame->PayloadLength = 8;

   for (i = 0; i < frame->PayloadLength; ++i)
   {
      frame->Payload[i] = Payload[i];
   }

   return nxSuccess;
}


/*=============================================================================*/
/* Initializes a variable frame (payload can be longer than 8 bytes)           */
/* nxFrameFixed_t(len) must be used to create the target variable in memory.   */
/*=============================================================================*/
nxStatus_t InitVariableCANFrame(nxFrameVar_t *frame, u32 Identifier, u8 Type,
   u8 Flags, u8 Info, u32 PayloadLength, u8 *Payload)
{
   unsigned int i = 0;
   frame->Timestamp = 0;
   frame->Identifier = Identifier;
   frame->Type = Type;
   frame->Flags = Flags;
   frame->Info = Info;
   /* Use this function to set the length in a way compatible with J1939 */
   nxFrameSetPayloadLength(frame, PayloadLength);

   for (i = 0; i < PayloadLength; ++i)
   {
      frame->Payload[i] = Payload[i];
   }

   return nxSuccess;
}


/*=============================================================================*/
/* Wrapper for local databases                                                 */
/*=============================================================================*/
nxStatus_t nxExampleGetDBAlias(u32 *pNumberOfDatabases, char ***pDatabaseAliasArray)
{
   return nxExampleGetDBAliasWithIP("", pNumberOfDatabases, pDatabaseAliasArray);
}


/*=============================================================================*/
/* Returns a list of all Database aliases currently on the system              */
/*=============================================================================*/
nxStatus_t nxExampleGetDBAliasWithIP(char *IPAddress, u32 *pNumberOfDatabases,
   char ***pDatabaseAliasArray)
{
   unsigned int i = 0;
   u32 l_AliasListSize = 0;
   u32 l_FilepathListSize = 0;
   u32 l_NumDatabases = 0;
   char *l_pAliasList = NULL;
   char *l_pFilepathList = NULL;
   char *l_pTempPtr = NULL; /* Temporary pointer used to iterate through full string */
   char **l_pAliasArray = NULL;
   size_t tempLen = 0; /* To find the length of the filepath */
   nxStatus_t l_Status = 0;

   /* Get the sizes of the strings */
   l_Status = nxdbGetDatabaseListSizes(IPAddress, &l_AliasListSize, &l_FilepathListSize);
   if (IsNoError(l_Status) && (l_AliasListSize > 0) && (l_FilepathListSize > 0))
   {
      /* Allocate memory for the aliases and filepaths */
      l_pAliasList = (char *)malloc(l_AliasListSize);
      if (NULL == l_pAliasList) {CriticalErrorHandler();}

      l_pFilepathList = (char *)malloc(l_FilepathListSize);
      if (NULL == l_pFilepathList) {CriticalErrorHandler();}

      /* Get the data of the known databases */
      l_Status = nxdbGetDatabaseList(IPAddress, l_AliasListSize, l_pAliasList,
         l_FilepathListSize, l_pFilepathList, &l_NumDatabases);
      if ((IsNoError(l_Status)) && (l_NumDatabases > 0))
      {
         l_pAliasArray = (char **)malloc(l_NumDatabases * sizeof(char *));
         if (NULL == l_pAliasArray) {CriticalErrorHandler();}

         /* Since the DB buffer contains the names delimited by ",", */
         /* extract the names and add them to the array. */
         l_pTempPtr = l_pAliasList;
         for (i = 0; i < l_NumDatabases; ++i)
         {
            /* Get the string length until the next "," without null termination */
            tempLen = strcspn(l_pTempPtr, ",");

            /* Allocate memory for this alias name */
            l_pAliasArray[i] = (char *)malloc(tempLen + 1);
            if (NULL == l_pAliasArray[i]) {CriticalErrorHandler();}

            /* Copy the string and add termination */
            strncpy(l_pAliasArray[i], l_pTempPtr, tempLen);
            l_pAliasArray[i][tempLen] = (char)'\0';

            /* Move the pointer to the next token*/
            l_pTempPtr += tempLen + 1;
         }
      }
      else
      {
         l_NumDatabases = 0;
      }
      free(l_pAliasList);
      free(l_pFilepathList);
   }

   *pNumberOfDatabases = l_NumDatabases;
   *pDatabaseAliasArray = l_pAliasArray;
   return l_Status;
}


/*=============================================================================*/
/* Returns a list of all Database filepaths currently on the system            */
/*=============================================================================*/
nxStatus_t nxExampleGetDBFilepaths(u32 *pNumberOfDatabases,
   char ***pDatabaseFilepathArray)
{
   return nxExampleGetDBFilepathsWithIP("", pNumberOfDatabases,
      pDatabaseFilepathArray);
}


/*=============================================================================*/
/* Returns a list of all Database filepaths currently on the system            */
/*=============================================================================*/
nxStatus_t nxExampleGetDBFilepathsWithIP(char *IPAddress, u32 *pNumberOfDatabases,
   char ***pDatabaseFilepathArray)
{
   unsigned int i = 0;
   u32 l_AliasListSize = 0;
   u32 l_FilepathListSize = 0;
   u32 l_NumDatabases = 0;
   char *l_pAliasList = NULL;
   char *l_pFilepathList = NULL;
   char *l_pTempPtr = NULL; /* Temporary pointer used to iterate through full string */
   char **l_pFilepathArray = NULL;
   size_t tempLen = 0; /* To find the length of the filepath */
   nxStatus_t l_Status = 0; /* Error status */

   /* Get the sizes and allocate memory for the strings */
   l_Status = nxdbGetDatabaseListSizes(IPAddress, &l_AliasListSize, &l_FilepathListSize);
   if (IsNoError(l_Status) && (l_AliasListSize > 0) && (l_FilepathListSize > 0))
   {
      /* Allocate memory for the aliases and filepaths */
      l_pAliasList = (char *)malloc(l_AliasListSize);
      if (NULL == l_pAliasList) {CriticalErrorHandler();}

      l_pFilepathList = (char *)malloc(l_FilepathListSize);
      if (NULL == l_pFilepathList) {CriticalErrorHandler();}

      /* Get the data of known databases */
      l_Status = nxdbGetDatabaseList(IPAddress, l_AliasListSize, l_pAliasList,
         l_FilepathListSize, l_pFilepathList, &l_NumDatabases);
      if ((IsNoError(l_Status)) && (l_NumDatabases > 0))
      {
         l_pFilepathArray = (char **)malloc(l_NumDatabases * sizeof(char *));
         if (NULL == l_pFilepathArray) {CriticalErrorHandler();}

         /* Since the DB buffer contains the names delimited by ",", */
         /* extract the names and add them to the array. */
         l_pTempPtr = l_pFilepathList;
         for (i = 0; i < l_NumDatabases; ++i)
         {
            /* Get the string length until the next "," without null termination */
            tempLen = strcspn(l_pTempPtr, ",");

            l_pFilepathArray[i] = (char *)malloc(tempLen + 1);
            if (NULL == l_pFilepathArray[i]) {CriticalErrorHandler();}

            /* Copy the string and add termination */
            strncpy(l_pFilepathArray[i], l_pTempPtr, tempLen);
            l_pFilepathArray[i][tempLen] = (char)'\0';

            /* Move the pointer to the next token*/
            l_pTempPtr += tempLen + 1;
         }
      }
      else
      {
         l_NumDatabases = 0;
      }
      free(l_pAliasList);
      free(l_pFilepathList);
   }

   *pNumberOfDatabases = l_NumDatabases;
   *pDatabaseFilepathArray = l_pFilepathArray;
   return l_Status;
}


/*=============================================================================*/
/* Returns a list of all clusters in a given database                          */
/*=============================================================================*/
nxStatus_t GetClusterNames(char *DatabaseName, u32 *NumberOfClusters,
   char ***ClusterNames)
{
   return GetClusterNamesByProtocol(DatabaseName, NumberOfClusters,
      ClusterNames, nxProtocol_Unknown);
}


/*=============================================================================*/
/* Returns a list of all CAN clusters in a given database                      */
/*=============================================================================*/
nxStatus_t GetCANClusterNames(char *DatabaseName, u32 *NumberOfClusters,
   char ***ClusterNames)
{
   return GetClusterNamesByProtocol(DatabaseName, NumberOfClusters,
      ClusterNames, nxProtocol_CAN);
}


/*=============================================================================*/
/* Returns a list of all FlexRay clusters in a given database                  */
/*=============================================================================*/
nxStatus_t GetFlexRayClusterNames(char *DatabaseName, u32 *NumberOfClusters,
   char ***ClusterNames)
{
   return GetClusterNamesByProtocol(DatabaseName, NumberOfClusters,
      ClusterNames, nxProtocol_FlexRay);
}


/*=============================================================================*/
/* Returns a list of all LIN clusters in a given database                      */
/*=============================================================================*/
nxStatus_t GetLINClusterNames(char *DatabaseName, u32 *NumberOfClusters,
   char ***ClusterNames)
{
   return GetClusterNamesByProtocol(DatabaseName, NumberOfClusters,
      ClusterNames, nxProtocol_LIN);
}


/*=============================================================================*/
/* Returns a list of all clusters with a specific protocol in a given database */
/* Using an unknown protocol ID will return all clusters from the database     */
/*=============================================================================*/
nxStatus_t GetClusterNamesByProtocol(char *DatabaseName, u32 *NumberOfClusters,
   char ***ClusterNames, u32 Protocol)
{
   unsigned int i = 0;
   u32 l_ArrayPos = 0;
   unsigned int l_NumberOfClusters = 0;
   u32 l_PropertySize = 0;
   char **l_pClusterNames = NULL;
   nxDatabaseRef_t l_DatabaseRef = 0;
   nxDatabaseRef_t *l_pClusterRefs = NULL;
   unsigned int l_Filtering = 0;
   unsigned int l_CorrectProtocol = 0;
   u32 l_Protocol = 0;
   nxStatus_t l_Status = 0;

   if ((Protocol == nxProtocol_CAN) ||
       (Protocol == nxProtocol_LIN) ||
       (Protocol == nxProtocol_FlexRay))
   {
      l_Filtering = 1;
   }

   /* Open the selected database name */
   l_Status = nxdbOpenDatabase(DatabaseName, &l_DatabaseRef);
   if (IsNoError(l_Status))
   {
      /* Get the size of the cluster reference list */
      l_Status = nxdbGetPropertySize(l_DatabaseRef, nxPropDatabase_ClstRefs,
         &l_PropertySize);
      if (IsNoError(l_Status))
      {
         /* Each cluster reference has a length of 4 bytes - calculate the cluster count */
         l_NumberOfClusters = (unsigned int)(l_PropertySize / sizeof(nxDatabaseRef_t));

         /* Check the number - the function does not return an error if there are no clusters */
         if (0 < l_NumberOfClusters)
         {
            /* Allocate memory for the cluster references */
            l_pClusterRefs = (nxDatabaseRef_t *)malloc(l_PropertySize);
            if (NULL == l_pClusterRefs) {CriticalErrorHandler();}

            /* Read the cluster references into the buffer */
            l_Status = nxdbGetProperty(l_DatabaseRef, nxPropDatabase_ClstRefs,
               l_PropertySize, l_pClusterRefs);
            if (IsNoError(l_Status))
            {
               /* Allocate memory for the cluster names */
               l_pClusterNames = (char **)malloc(l_NumberOfClusters * sizeof(char *));
               if (NULL == l_pClusterNames) {CriticalErrorHandler();}

               for (i = 0; i < l_NumberOfClusters; ++i)
               {
                  if (l_Filtering == 0)
                  {
                     l_CorrectProtocol = 1;
                  }
                  else
                  {
                     /* Check if the protocol is correct */
                     l_Status = nxdbGetProperty(l_pClusterRefs[i],
                        nxPropClst_Protocol, (u32)sizeof(l_Protocol), &l_Protocol);
                     if ((IsNoError(l_Status)) && (l_Protocol == Protocol))
                     {
                        l_CorrectProtocol = 1;
                     }
                     else
                     {
                        l_CorrectProtocol = 0;
                     }
                  }

                  /* Add the cluster name to the array if the protocol is correct */
                  if (l_CorrectProtocol != 0)
                  {
                     /* Get the size of the cluster name */
                     l_Status = nxdbGetPropertySize(l_pClusterRefs[i],
                        nxPropClst_Name, &l_PropertySize);
                     if (IsNoError(l_Status) && (l_PropertySize > 0))
                     {
                        /* Allocate memory for the cluster name */
                        l_pClusterNames[l_ArrayPos] = (char *)malloc(l_PropertySize);
                        if (NULL == l_pClusterNames[l_ArrayPos]) {CriticalErrorHandler();}

                        /* Read the cluster name into the buffer */
                        l_Status = nxdbGetProperty(l_pClusterRefs[i],
                           nxPropClst_Name, l_PropertySize, l_pClusterNames[l_ArrayPos]);
                        if (IsNoError(l_Status))
                        {
                           /* Move on to the next array entry */
                           ++l_ArrayPos;
                        }
                        else
                        {
                           /* Error - Delete the string from the list and keep the position */
                           free(l_pClusterNames[l_ArrayPos]);
                        }
                     }
                  }
               }
            }
            free(l_pClusterRefs);
         }
      }
   }
   /* Close the database */
   if (0 != l_DatabaseRef)
   {
      l_Status = nxdbCloseDatabase(l_DatabaseRef, 1);
   }

   *NumberOfClusters = l_ArrayPos;
   *ClusterNames = l_pClusterNames;
   return l_Status;
}


/*=============================================================================*/
/* Returns a list of all LIN schedules in a given database cluster             */
/*=============================================================================*/
nxStatus_t GetLINScheduleNames(char *DatabaseName, char *ClusterName,
   u32 *NumberOfSchedules, char ***ScheduleNames)
{
   unsigned int i = 0;
   u32 l_ArrayPos = 0;
   unsigned int l_TotalNumberOfSchedules = 0;
   u32 l_PropertySize = 0;
   char **l_pScheduleNames = NULL;
   nxDatabaseRef_t l_ClusterRef = 0;
   nxDatabaseRef_t l_DatabaseRef = 0;
   nxDatabaseRef_t *l_pScheduleRefs = NULL;
   nxStatus_t l_Status = 0;

   /* Open the selected database name */
   l_Status = nxdbOpenDatabase(DatabaseName, &l_DatabaseRef);
   if (IsNoError(l_Status))
   {
      /* Find the cluster object in the DB */
      l_Status = nxdbFindObject(l_DatabaseRef, nxClass_Cluster, ClusterName,
         &l_ClusterRef);
      if (IsNoError(l_Status))
      {
         /* Get the size of the cluster's LIN schedule reference array */
         l_Status = nxdbGetPropertySize(l_ClusterRef, nxPropClst_LINSchedules,
            &l_PropertySize);
         if (IsNoError(l_Status))
         {
            /* Each schedule reference has length of 4 bytes */
            l_TotalNumberOfSchedules = (unsigned int)(l_PropertySize / sizeof(nxDatabaseRef_t));

            /* Check the number - the function does not return an error if there are no schedules */
            if (0 < l_TotalNumberOfSchedules)
            {
               /* Allocate memory for the schedule references */
               l_pScheduleRefs = (nxDatabaseRef_t *)malloc(l_PropertySize);
               if (NULL == l_pScheduleRefs) {CriticalErrorHandler();}

               /* Read the schedule references into the buffer */
               l_Status = nxdbGetProperty(l_ClusterRef, nxPropClst_LINSchedules,
                  l_PropertySize, l_pScheduleRefs);
               if (IsNoError(l_Status))
               {
                  /* Allocate memory for the schedule names */
                  l_pScheduleNames = (char **)malloc(l_TotalNumberOfSchedules * sizeof(char *));
                  if (NULL == l_pScheduleNames) {CriticalErrorHandler();}

                  /* Add the schedule names to the array */
                  for (i = 0; i < l_TotalNumberOfSchedules; ++i)
                  {
                     /* Get the size of the schedule name */
                     l_Status = nxdbGetPropertySize(l_pScheduleRefs[i],
                        nxPropLINSched_Name, &l_PropertySize);
                     if (IsNoError(l_Status) && (l_PropertySize > 0))
                     {
                        /* Allocate memory for the schedule name */
                        l_pScheduleNames[l_ArrayPos] = (char *)malloc(l_PropertySize);
                        if (NULL == l_pScheduleNames[l_ArrayPos]) {CriticalErrorHandler();}

                        /* Read the schedule name into the buffer */
                        l_Status = nxdbGetProperty(l_pScheduleRefs[i],
                           nxPropLINSched_Name, l_PropertySize,
                           l_pScheduleNames[l_ArrayPos]);
                        if (IsNoError(l_Status))
                        {
                           /* Move on to the next array entry */
                           ++l_ArrayPos;
                        }
                        else
                        {
                           /* Error - Delete the string from the list and keep the position */
                           free(l_pScheduleNames[l_ArrayPos]);
                        }
                     }
                  }
               }
               free(l_pScheduleRefs);
            }
         }
      }
   }
   /* Close the database */
   if (0 != l_DatabaseRef)
   {
      l_Status = nxdbCloseDatabase(l_DatabaseRef, 1);
   }

   *NumberOfSchedules = l_ArrayPos;
   *ScheduleNames = l_pScheduleNames;
   return l_Status;
}


/*=============================================================================*/
/* Returns a list (string) of all frames in a given cluster                    */
/*=============================================================================*/
nxStatus_t GetFrameNames(char *DatabaseName, char *ClusterName, u32 *NumberFrames,
   char ***FrameNames)
{
   unsigned int i = 0;
   u32 l_ArrayPos = 0;
   u32 l_PropertySize = 0;
   unsigned int l_NumberOfFrames = 0;
   char **l_pFrameNames = NULL;
   nxDatabaseRef_t l_DatabaseRef = 0;
   nxDatabaseRef_t l_ClusterRef = 0;
   nxDatabaseRef_t *l_pFrameRefs = NULL;
   nxStatus_t l_Status = 0;

   /* Open the selected database name */
   l_Status = nxdbOpenDatabase(DatabaseName, &l_DatabaseRef);
   if (IsNoError(l_Status))
   {
      /* Find the cluster object in the DB */
      l_Status = nxdbFindObject(l_DatabaseRef, nxClass_Cluster, ClusterName,
         &l_ClusterRef);
      if (IsNoError(l_Status))
      {
         /* Get the size of the cluster's frame reference array */
         l_Status = nxdbGetPropertySize(l_ClusterRef, nxPropClst_FrmRefs,
            &l_PropertySize);
         if (IsNoError(l_Status))
         {
            /* Each frame reference has 4 bytes */
            l_NumberOfFrames = (unsigned int)(l_PropertySize / sizeof(nxDatabaseRef_t));

            /* Check the number - the function does not return an error if there are no frames */
            if (0 < l_NumberOfFrames)
            {
               /* Allocate memory for the frame references */
               l_pFrameRefs = (nxDatabaseRef_t *)malloc(l_PropertySize);
               if (NULL == l_pFrameRefs) {CriticalErrorHandler();}

               /* Read the frame references into the buffer */
               l_Status = nxdbGetProperty(l_ClusterRef, nxPropClst_FrmRefs,
                  l_PropertySize, l_pFrameRefs);
               if (IsNoError(l_Status))
               {
                  /* Allocate memory for the frame names */
                  l_pFrameNames = (char **)malloc(l_NumberOfFrames * sizeof(char *));
                  if (NULL == l_pFrameNames) {CriticalErrorHandler();}

                  /* Add the frame names to the array */
                  for (i = 0; i < l_NumberOfFrames; ++i)
                  {
                     /* Get the size of the frame name */
                     l_Status = nxdbGetPropertySize(l_pFrameRefs[i],
                        nxPropFrm_Name, &l_PropertySize);
                     if (IsNoError(l_Status) && (l_PropertySize > 0))
                     {
                        /* Allocate memory for the frame name */
                        l_pFrameNames[l_ArrayPos] = (char *)malloc(l_PropertySize);
                        if (NULL == l_pFrameNames[l_ArrayPos]) {CriticalErrorHandler();}

                        /* Read the frame name into the buffer */
                        l_Status = nxdbGetProperty(l_pFrameRefs[i],
                           nxPropFrm_Name, l_PropertySize,
                           l_pFrameNames[l_ArrayPos]);
                        if (IsNoError(l_Status))
                        {
                           /* Move on to the next array entry */
                           ++l_ArrayPos;
                        }
                        else
                        {
                           /* Error - Delete the string from the list and keep the position */
                           free(l_pFrameNames[l_ArrayPos]);
                        }
                     }
                  }
               }
               free(l_pFrameRefs);
            }
         }
      }
   }
   /* Close the database */
   if (0 != l_DatabaseRef)
   {
      l_Status = nxdbCloseDatabase(l_DatabaseRef, 1);
   }

   *NumberFrames = l_ArrayPos;
   *FrameNames = l_pFrameNames;
   return l_Status;
}


/*=============================================================================*/
/* Returns a list (string) of all PDUs in a given cluster                      */
/*=============================================================================*/
nxStatus_t GetPDUNames(char *DatabaseName, char *ClusterName, u32 *NumberPDUs,
   char ***PDUNames)
{
   unsigned int i = 0;
   u32 l_ArrayPos = 0;
   u32 l_PropertySize = 0;
   unsigned int l_NumberOfPdus = 0;
   char **l_pPduNames = NULL;
   nxDatabaseRef_t l_DatabaseRef = 0;
   nxDatabaseRef_t l_ClusterRef = 0;
   nxDatabaseRef_t *l_pPduRefs = NULL;
   nxStatus_t l_Status = 0;

   /* Open the selected database name */
   l_Status = nxdbOpenDatabase(DatabaseName, &l_DatabaseRef);
   if (IsNoError(l_Status))
   {
      /* Find the cluster object in the DB */
      l_Status = nxdbFindObject(l_DatabaseRef, nxClass_Cluster, ClusterName,
         &l_ClusterRef);
      if (IsNoError(l_Status))
      {
         /* Get the size of the cluster's PDU reference array */
         l_Status = nxdbGetPropertySize(l_ClusterRef, nxPropClst_PDURefs,
            &l_PropertySize);
         if (IsNoError(l_Status))
         {
            /* Each PDU reference has 4 bytes */
            l_NumberOfPdus = (unsigned int)(l_PropertySize / sizeof(nxDatabaseRef_t));

            /* Check the number - the function does not return an error if there are no PDUs */
            if (0 < l_NumberOfPdus)
            {
               /* Allocate memory for the PDU references */
               l_pPduRefs = (nxDatabaseRef_t *)malloc(l_PropertySize);
               if (NULL == l_pPduRefs) {CriticalErrorHandler();}

               /* Read the PDU references into the buffer */
               l_Status = nxdbGetProperty(l_ClusterRef, nxPropClst_PDURefs,
                  l_PropertySize, l_pPduRefs);
               if (IsNoError(l_Status))
               {
                  /* Allocate memory for the PDU names */
                  l_pPduNames = (char **)malloc(l_NumberOfPdus * sizeof(char *));
                  if (NULL == l_pPduNames) {CriticalErrorHandler();}

                  /* Add the PDU names to the array */
                  for (i = 0; i < l_NumberOfPdus; ++i)
                  {
                     /* Get the size of the PDU name */
                     l_Status = nxdbGetPropertySize(l_pPduRefs[i],
                        nxPropPDU_Name, &l_PropertySize);
                     if (IsNoError(l_Status) && (l_PropertySize > 0))
                     {
                        /* Allocate memory for the PDU name */
                        l_pPduNames[l_ArrayPos] = (char *)malloc(l_PropertySize);
                        if (NULL == l_pPduNames[l_ArrayPos]) {CriticalErrorHandler();}

                        /* Read the PDU name into the buffer */
                        l_Status = nxdbGetProperty(l_pPduRefs[i],
                           nxPropPDU_Name, l_PropertySize,
                           l_pPduNames[l_ArrayPos]);
                        if (IsNoError(l_Status))
                        {
                           /* Move on to the next array entry */
                           ++l_ArrayPos;
                        }
                        else
                        {
                           /* Error - Delete the string from the list and keep the position */
                           free(l_pPduNames[l_ArrayPos]);
                        }
                     }
                  }
               }
               free(l_pPduRefs);
            }
         }
      }
   }
   /* Close the database */
   if (0 != l_DatabaseRef)
   {
      l_Status = nxdbCloseDatabase(l_DatabaseRef, 1);
   }

   *NumberPDUs = l_ArrayPos;
   *PDUNames = l_pPduNames;
   return l_Status;
}


/*=============================================================================*/
/* Returns a list (string) of all signals in a given cluster                   */
/*=============================================================================*/
nxStatus_t GetSignalsNames(char *DatabaseName, char *ClusterName,
   u32 *NumberSignals, char ***SignalNames)
{
   unsigned int i = 0;
   u32 l_ArrayPos = 0;
   u32 l_PropertySize = 0;
   unsigned int l_NumberOfSignals = 0;
   char **l_pSignalNames = NULL;
   nxDatabaseRef_t l_DatabaseRef = 0;
   nxDatabaseRef_t l_ClusterRef = 0;
   nxDatabaseRef_t *l_pSignalRefs = NULL;
   nxStatus_t l_Status = 0;

   /* Open the selected database name */
   l_Status = nxdbOpenDatabase(DatabaseName, &l_DatabaseRef);
   if (IsNoError(l_Status))
   {
      /* Find the cluster object in the DB */
      l_Status = nxdbFindObject(l_DatabaseRef, nxClass_Cluster, ClusterName,
         &l_ClusterRef);
      if (IsNoError(l_Status))
      {
         /* Get the size of the cluster's signal reference array */
         l_Status = nxdbGetPropertySize(l_ClusterRef, nxPropClst_SigRefs,
            &l_PropertySize);
         if (IsNoError(l_Status))
         {
            /* Each signal reference has 4 bytes */
            l_NumberOfSignals = (unsigned int)(l_PropertySize / sizeof(nxDatabaseRef_t));

            /* Check the number - the function does not return an error if there are no signals */
            if (0 < l_NumberOfSignals)
            {
               /* Allocate memory for the signal references */
               l_pSignalRefs = (nxDatabaseRef_t *)malloc(l_PropertySize);
               if (NULL == l_pSignalRefs) {CriticalErrorHandler();}

               /* Read the signal references into the buffer */
               l_Status = nxdbGetProperty(l_ClusterRef, nxPropClst_SigRefs,
                  l_PropertySize, l_pSignalRefs);
               if (IsNoError(l_Status))
               {
                  /* Allocate memory for the signal names */
                  l_pSignalNames = (char **)malloc(l_NumberOfSignals * sizeof(char*));
                  if (NULL == l_pSignalNames) {CriticalErrorHandler();}

                  /* Add the signal names to the array */
                  for (i = 0; i < l_NumberOfSignals; ++i)
                  {
                     /* Get the size of the signal name */
                     l_Status = nxdbGetPropertySize(l_pSignalRefs[i],
                        nxPropSig_NameUniqueToCluster, &l_PropertySize);
                     if (IsNoError(l_Status) && (l_PropertySize > 0))
                     {
                        /* Allocate memory for the signal name */
                        l_pSignalNames[l_ArrayPos] = (char *)malloc(l_PropertySize);
                        if (NULL == l_pSignalNames[l_ArrayPos]) {CriticalErrorHandler();}

                        /* Read the signal name into the buffer */
                        l_Status = nxdbGetProperty(l_pSignalRefs[i],
                           nxPropSig_NameUniqueToCluster, l_PropertySize,
                           l_pSignalNames[l_ArrayPos]);
                        if (IsNoError(l_Status))
                        {
                           /* Move on to the next array entry */
                           ++l_ArrayPos;
                        }
                        else
                        {
                           /* Error - Delete the string from the list and keep the position */
                           free(l_pSignalNames[l_ArrayPos]);
                        }
                     }
                  }
               }
               free(l_pSignalRefs);
            }
         }
      }
   }
   /* Close the database */
   if (0 != l_DatabaseRef)
   {
      l_Status = nxdbCloseDatabase(l_DatabaseRef, 1);
   }

   *NumberSignals = l_ArrayPos;
   *SignalNames = l_pSignalNames;
   return l_Status;
}


/*=============================================================================*/
/* Returns a list (string) of all signals in a given cluster's frame           */
/*=============================================================================*/
nxStatus_t GetSignalsNamesOfFrame(char *DatabaseName, char *ClusterName, char *FrameName,
   u32 *NumberSignals, char ***SignalNames)
{
   unsigned int i = 0;
   u32 l_ArrayPos = 0;
   u32 l_PropertySize = 0;
   unsigned int l_NumberOfSignals = 0;
   char **l_pSignalNames = NULL;
   nxDatabaseRef_t l_DatabaseRef = 0;
   nxDatabaseRef_t l_ClusterRef = 0;
   nxDatabaseRef_t l_FrameRef = 0;
   nxDatabaseRef_t *l_pSignalRefs = NULL;
   nxStatus_t l_Status = 0;

   /* Open the selected database name */
   l_Status = nxdbOpenDatabase(DatabaseName, &l_DatabaseRef);
   if (IsNoError(l_Status))
   {
      /* Find the cluster object in the DB */
      l_Status = nxdbFindObject(l_DatabaseRef, nxClass_Cluster, ClusterName,
         &l_ClusterRef);
      if (IsNoError(l_Status))
      {
         /* Find the frame object in the DB */
         l_Status = nxdbFindObject(l_ClusterRef, nxClass_Frame, FrameName,
            &l_FrameRef);
         if (IsNoError(l_Status))
         {
            /* Get the size of the frame's signal reference array */
            l_Status = nxdbGetPropertySize(l_FrameRef, nxPropFrm_SigRefs,
               &l_PropertySize);
            if (IsNoError(l_Status))
            {
               /* Each signal reference has 4 bytes */
               l_NumberOfSignals = (unsigned int)(l_PropertySize / sizeof(nxDatabaseRef_t));

               /* Check the number - the function does not return an error if there are no signals */
               if (0 < l_NumberOfSignals)
               {
                  /* Allocate memory for the signal references */
                  l_pSignalRefs = (nxDatabaseRef_t *)malloc(l_PropertySize);
                  if (NULL == l_pSignalRefs) {CriticalErrorHandler();}

                  /* Read the signal references into the buffer */
                  l_Status = nxdbGetProperty(l_FrameRef, nxPropFrm_SigRefs,
                     l_PropertySize, l_pSignalRefs);
                  if (IsNoError(l_Status))
                  {
                     /* Allocate memory for the signal names */
                     l_pSignalNames = (char **)malloc(l_NumberOfSignals * sizeof(char *));
                     if (NULL == l_pSignalNames) {CriticalErrorHandler();}

                     /* Add the signal names to the array */
                     for (i = 0; i < l_NumberOfSignals; ++i)
                     {
                        /* Get the size of the signal name */
                        l_Status = nxdbGetPropertySize(l_pSignalRefs[i],
                           nxPropSig_Name, &l_PropertySize);
                        if (IsNoError(l_Status) && (l_PropertySize > 0))
                        {
                           /* Allocate memory for the signal name */
                           l_pSignalNames[l_ArrayPos] = (char *)malloc(l_PropertySize);
                           if (NULL == l_pSignalNames[l_ArrayPos]) {CriticalErrorHandler();}

                           /* Read the signal name into the buffer */
                           l_Status = nxdbGetProperty(l_pSignalRefs[i],
                              nxPropSig_Name, l_PropertySize,
                              l_pSignalNames[l_ArrayPos]);
                           if (IsNoError(l_Status))
                           {
                              /* Move on to the next array entry */
                              ++l_ArrayPos;
                           }
                           else
                           {
                              /* Error - Delete the string from the list and keep the position */
                              free(l_pSignalNames[l_ArrayPos]);
                           }
                        }
                     }
                  }
                  free(l_pSignalRefs);
               }
            }
         }
      }
   }
   /* Close the database */
   if (0 != l_DatabaseRef)
   {
      l_Status = nxdbCloseDatabase(l_DatabaseRef, 1);
   }

   *NumberSignals = l_ArrayPos;
   *SignalNames = l_pSignalNames;
   return l_Status;
}


/*=============================================================================*/
/* Returns a list (string) of all signals in a given cluster's PDU             */
/*=============================================================================*/
nxStatus_t GetSignalsNamesOfPDU(char *DatabaseName, char *ClusterName, char *PduName,
   u32 *NumberSignals, char ***SignalNames)
{
   unsigned int i = 0;
   u32 l_ArrayPos = 0;
   u32 l_PropertySize = 0;
   unsigned int l_NumberOfSignals = 0;
   char **l_pSignalNames = NULL;
   nxDatabaseRef_t l_DatabaseRef = 0;
   nxDatabaseRef_t l_ClusterRef = 0;
   nxDatabaseRef_t l_PduRef = 0;
   nxDatabaseRef_t *l_pSignalRefs = NULL;
   nxStatus_t l_Status = 0;

   /* Open the selected database name */
   l_Status = nxdbOpenDatabase(DatabaseName, &l_DatabaseRef);
   if (IsNoError(l_Status))
   {
      /* Find the cluster object in the DB */
      l_Status = nxdbFindObject(l_DatabaseRef, nxClass_Cluster, ClusterName,
         &l_ClusterRef);
      if (IsNoError(l_Status))
      {
         /* Find the PDU object in the DB */
         l_Status = nxdbFindObject(l_ClusterRef, nxClass_PDU, PduName,
            &l_PduRef);
         if (IsNoError(l_Status))
         {
            /* Get the size of the PDU's signal reference array */
            l_Status = nxdbGetPropertySize(l_PduRef, nxPropPDU_SigRefs,
               &l_PropertySize);
            if (IsNoError(l_Status))
            {
               /* Each signal reference has 4 bytes */
               l_NumberOfSignals = (unsigned int)(l_PropertySize / sizeof(nxDatabaseRef_t));

               /* Check the number - the function does not return an error if there are no signals */
               if (0 < l_NumberOfSignals)
               {
                  /* Allocate memory for the signal references */
                  l_pSignalRefs = (nxDatabaseRef_t *)malloc(l_PropertySize);
                  if (NULL == l_pSignalRefs) {CriticalErrorHandler();}

                  /* Read the signal references into the buffer */
                  l_Status = nxdbGetProperty(l_PduRef, nxPropPDU_SigRefs,
                     l_PropertySize, l_pSignalRefs);
                  if (IsNoError(l_Status))
                  {
                     /* Allocate memory for the signal names */
                     l_pSignalNames = (char **)malloc(l_NumberOfSignals * sizeof(char *));
                     if (NULL == l_pSignalNames) {CriticalErrorHandler();}

                     /* Add the signal names to the array */
                     for (i = 0; i < l_NumberOfSignals; ++i)
                     {
                        /* Get the size of the signal name */
                        l_Status = nxdbGetPropertySize(l_pSignalRefs[i],
                           nxPropSig_Name, &l_PropertySize);
                        if (IsNoError(l_Status) && (l_PropertySize > 0))
                        {
                           /* Allocate memory for the signal name */
                           l_pSignalNames[l_ArrayPos] = (char *)malloc(l_PropertySize);
                           if (NULL == l_pSignalNames[l_ArrayPos]) {CriticalErrorHandler();}

                           /* Read the signal name into the buffer */
                           l_Status = nxdbGetProperty(l_pSignalRefs[i],
                              nxPropSig_Name, l_PropertySize,
                              l_pSignalNames[l_ArrayPos]);
                           if (IsNoError(l_Status))
                           {
                              /* Move on to the next array entry */
                              ++l_ArrayPos;
                           }
                           else
                           {
                              /* Error - Delete the string from the list and keep the position */
                              free(l_pSignalNames[l_ArrayPos]);
                           }
                        }
                     }
                  }
                  free(l_pSignalRefs);
               }
            }
         }
      }
   }
   /* Close the database */
   if (0 != l_DatabaseRef)
   {
      l_Status = nxdbCloseDatabase(l_DatabaseRef, 1);
   }

   *NumberSignals = l_ArrayPos;
   *SignalNames = l_pSignalNames;
   return l_Status;
}


/*=============================================================================*/
/* Returns the payload length for a frame                                      */
/*=============================================================================*/
nxStatus_t GetFramePayloadLength(char *DatabaseName, char *ClusterName,
   char *FrameName, u32 *PayloadLength)
{
   nxDatabaseRef_t l_DatabaseRef = 0;
   nxDatabaseRef_t l_ClusterRef = 0;
   nxDatabaseRef_t l_FrameRef = 0;
   nxStatus_t l_Status = 0;

   *PayloadLength = 0;

   /* Open the selected database name */
   l_Status = nxdbOpenDatabase(DatabaseName, &l_DatabaseRef);
   if (IsError(l_Status))
   {
      nxdbCloseDatabase(l_DatabaseRef, 1);
      return l_Status;
   }

   /* Find the cluster object in the DB */
   l_Status = nxdbFindObject(l_DatabaseRef, nxClass_Cluster, ClusterName,
      &l_ClusterRef);
   if (IsError(l_Status))
   {
      nxdbCloseDatabase(l_DatabaseRef, 1);
      return l_Status;
   }

   /* Find the frame object in the DB */
   l_Status = nxdbFindObject(l_ClusterRef, nxClass_Frame, FrameName,
      &l_FrameRef);
   if (IsError(l_Status))
   {
      nxdbCloseDatabase(l_DatabaseRef, 1);
      return l_Status;
   }

   /* Get the length of the payload */
   l_Status = nxdbGetProperty(l_FrameRef, nxPropFrm_PayloadLen,
      (u32)sizeof(*PayloadLength), PayloadLength);
   if (IsError(l_Status))
   {
      nxdbCloseDatabase(l_DatabaseRef, 1);
      return l_Status;
   }

   /* Close the database */
   if (0 != l_DatabaseRef)
   {
      l_Status = nxdbCloseDatabase(l_DatabaseRef, 1);
   }

   return l_Status;
}


/*=============================================================================*/
/* Returns the payload length for a PDU                                        */
/*=============================================================================*/
nxStatus_t GetPDUPayloadLength(char *DatabaseName, char *ClusterName,
   char *PDUName, u32 *PayloadLength)
{
   nxDatabaseRef_t l_DatabaseRef = 0;
   nxDatabaseRef_t l_ClusterRef = 0;
   nxDatabaseRef_t l_PduRef = 0;
   nxStatus_t l_Status = 0;

   *PayloadLength = 0;

   /* Open the selected database name */
   l_Status = nxdbOpenDatabase(DatabaseName, &l_DatabaseRef);
   if (IsError(l_Status))
   {
      nxdbCloseDatabase(l_DatabaseRef, 1);
      return l_Status;
   }

   /* Find the cluster object in the DB */
   l_Status = nxdbFindObject(l_DatabaseRef, nxClass_Cluster, ClusterName,
      &l_ClusterRef);
   if (IsError(l_Status))
   {
      nxdbCloseDatabase(l_DatabaseRef, 1);
      return l_Status;
   }

   /* Find the PDU object in the DB */
   l_Status = nxdbFindObject(l_ClusterRef, nxClass_PDU, PDUName,
      &l_PduRef);
   if (IsError(l_Status))
   {
      nxdbCloseDatabase(l_DatabaseRef, 1);
      return l_Status;
   }

   /* Get the length of the payload */
   l_Status = nxdbGetProperty(l_PduRef, nxPropPDU_PayloadLen,
      (u32)sizeof(*PayloadLength), PayloadLength);
   if (IsError(l_Status))
   {
      nxdbCloseDatabase(l_DatabaseRef, 1);
      return l_Status;
   }

   /* Close the database */
   if (0 != l_DatabaseRef)
   {
      l_Status = nxdbCloseDatabase(l_DatabaseRef, 1);
   }

   return l_Status;
}

#if defined _WIN64 || defined _WIN32

// Exclude rarely-used stuff from Windows headers
#ifndef WIN32_LEAN_AND_MEAN
#define WIN32_LEAN_AND_MEAN
#endif
#include <windows.h>    // Include file on Windows for time functions

/*=============================================================================*/
/* Change Absolute time to string                                              */
/*=============================================================================*/
void AbsTimeToString(nxTimestamp_t *time, char *TimeString)
{
   SYSTEMTIME stime;
   FILETIME localftime;

   FileTimeToLocalFileTime((FILETIME *)time, &localftime);
   FileTimeToSystemTime(&localftime, &stime);
   sprintf(TimeString, "%02d:%02d:%02d.%03d", stime.wHour, stime.wMinute,
      stime.wSecond, stime.wMilliseconds);
}

#elif defined __linux__

#include <time.h>

/*=============================================================================*/
/* Change Absolute time to string                                              */
/*=============================================================================*/
void AbsTimeToString(nxTimestamp_t *time, char *Timestring)
{
   // NI-XNET timestamps are a counter of 100 nanosecond ticks since Jan 1, 1601, but POSIX timestamps
   // are a counter of 1 second ticks since Jan 1, 1901, so we just need to convert units and subtract them.
   static const unsigned long long EPOCH_DIFFERENCE_IN_100NS_TICKS = 116444736000000000ull;
   const unsigned long long posix_timestamp_100ns_ticks = *time - EPOCH_DIFFERENCE_IN_100NS_TICKS;
   const time_t posix_timestamp = posix_timestamp_100ns_ticks / 10000000;
   const int milliseconds = (int)(posix_timestamp_100ns_ticks / 10000) % 1000;
   struct tm local_time = {0};
   char buffer[32] = {0};

   localtime_r(&posix_timestamp, &local_time);
   strftime(buffer, sizeof(buffer), "%T", &local_time);
   sprintf(Timestring, "%s.%03d", buffer, milliseconds);
}

#else
   #error The NI-XNET Examples do not support this Operating System.
#endif

/*=============================================================================*/
/* Returns all interface names on the system                                   */
/*=============================================================================*/
nxStatus_t GetAllInterfaces(u32 *numberOfInterfaces, char ***InterfacesNames)
{
   return GetAllInterfacesByProtocol(numberOfInterfaces, InterfacesNames,
      nxProtocol_Unknown);
}


/*=============================================================================*/
/* Returns all CAN interface names on the system                               */
/*=============================================================================*/
nxStatus_t GetAllCANInterfaces(u32 *numberOfInterfaces,
   char ***InterfacesNames)
{
   return GetAllInterfacesByProtocol(numberOfInterfaces, InterfacesNames,
      nxProtocol_CAN);
}


/*=============================================================================*/
/* Returns all FlexRay interface names on the system                           */
/*=============================================================================*/
nxStatus_t GetAllFlexRayInterfaces(u32 *numberOfInterfaces,
   char ***InterfacesNames)
{
   return GetAllInterfacesByProtocol(numberOfInterfaces, InterfacesNames,
      nxProtocol_FlexRay);
}


/*=============================================================================*/
/* Returns all LIN interface names on the system                               */
/*=============================================================================*/
nxStatus_t GetAllLINInterfaces(u32 *numberOfInterfaces,
   char ***InterfacesNames)
{
   return GetAllInterfacesByProtocol(numberOfInterfaces, InterfacesNames,
      nxProtocol_LIN);
}


/*=============================================================================*/
/* Returns a list of all interfaces with a specific protocol in the system     */
/* Using an unknown protocol ID will return all interfaces                     */
/*=============================================================================*/
nxStatus_t GetAllInterfacesByProtocol(u32 *numberOfInterfaces,
   char ***InterfacesNames, u32 Protocol)
{
   unsigned int i = 0;
   u32 l_ArrayPos = 0;
   u32 l_PropertySize = 0;
   unsigned int l_NumberOfInterfaces = 0;
   u32 *l_pInterfaceRefs = NULL;
   char **l_pInterfaceNames = NULL;
   nxSessionRef_t l_SystemRef = 0;
   u32 l_ActualProperty = 0;
   nxStatus_t l_Status = 0;

   /* Open a system session. This can be used to get certain */
   /* properties from the NI-XNET driver. */
   l_Status = nxSystemOpen(&l_SystemRef);
   if (IsNoError(l_Status))
   {
      /* Select the appropriate property to use */
      switch (Protocol)
      {
      case nxProtocol_CAN:
         l_ActualProperty = nxPropSys_IntfRefsCAN;
         break;
      case nxProtocol_FlexRay:
         l_ActualProperty = nxPropSys_IntfRefsFlexRay;
         break;
      case nxProtocol_LIN:
         l_ActualProperty = nxPropSys_IntfRefsLIN;
         break;
      default:
         l_ActualProperty = nxPropSys_IntfRefs;
         break;
      }

      /* Get the size of the interface reference array */
      l_Status = nxGetPropertySize(l_SystemRef, l_ActualProperty,
         &l_PropertySize);
      if (IsNoError(l_Status))
      {
         /* Each interface reference has length of 4 bytes */
         l_NumberOfInterfaces = (unsigned int)(l_PropertySize / sizeof(u32));

         /* Check the number - the function does not return an error if there are no interfaces */
         if (0 < l_NumberOfInterfaces)
         {
            /* Allocate memory for the interface references */
            l_pInterfaceRefs = (u32 *)malloc(l_PropertySize);
            if (NULL == l_pInterfaceRefs) {CriticalErrorHandler();}

            /* Read the interface references into the buffer */
            l_Status = nxGetProperty(l_SystemRef, l_ActualProperty,
               l_PropertySize, l_pInterfaceRefs);
            if (IsNoError(l_Status))
            {
               /* Allocate memory for the interface names */
               l_pInterfaceNames = (char **)malloc(l_NumberOfInterfaces * sizeof(char *));
               if (NULL == l_pInterfaceNames) {CriticalErrorHandler();}

               /* Add the interface name to the array */
               for (i = 0; i < l_NumberOfInterfaces; ++i)
               {
                  /* Get the size of the interface name */
                  l_Status = nxGetPropertySize((nxSessionRef_t)l_pInterfaceRefs[i],
                     nxPropIntf_Name, &l_PropertySize);
                  if (IsNoError(l_Status) && (l_PropertySize > 0))
                  {
                     /* Allocate memory for the interface name */
                     l_pInterfaceNames[l_ArrayPos] = (char *)malloc(l_PropertySize);
                     if (NULL == l_pInterfaceNames[l_ArrayPos]) {CriticalErrorHandler();}

                     /* Read the interface name into the buffer */
                     l_Status = nxGetProperty((nxSessionRef_t)l_pInterfaceRefs[i],
                        nxPropIntf_Name, l_PropertySize, l_pInterfaceNames[l_ArrayPos]);
                     if (IsNoError(l_Status))
                     {
                        /* Move on to the next array entry */
                        ++l_ArrayPos;
                     }
                     else
                     {
                        /* Error - Delete the string from the list and keep the position */
                        free(l_pInterfaceNames[l_ArrayPos]);
                     }
                  }
               }
            }
            free(l_pInterfaceRefs);
         }
      }
   }
   /* Close the system reference */
   if (0 != l_SystemRef)
   {
      l_Status = nxSystemClose(l_SystemRef);
   }

   *numberOfInterfaces = l_ArrayPos;
   *InterfacesNames = l_pInterfaceNames;
   return l_Status;
}


/*=============================================================================*/
/* Exit in case of critical errors                                             */
/*=============================================================================*/
void CriticalErrorHandler(void)
{
   printf("\nCritical Error occured. Out of memory?\n");
   exit(1);
}
