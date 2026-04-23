#if defined _MSC_VER
   #define _CRT_SECURE_NO_WARNINGS
#endif

#include <stdlib.h>
#include <stdio.h>
#include "niDMMErrorHandler.h"

#if defined _CVI_
   #include <userint.h>
   void niDMM_ReportStatus(ViConstString title, ViConstString message)
   {
      MessagePopup(title, message);
   }

#elif defined _MSC_VER

   #include <conio.h>
   #include <ctype.h>
   #include <string.h>
   void niDMM_ReportStatus(ViConstString title, ViConstString message)
   {
      printf("\n%s:\n%s\n\nPress any key to continue...\n", title, message);
      while(!_kbhit());
      _getch();
   }
#else
   void niDMM_ReportStatus(ViConstString title, ViConstString message)
   {
      printf("\n%s:\n%s\n", title, message);
   }

#endif

void niDMM_ErrorHandler(ViSession session, ViStatus status)
{
   ViChar *buffer = VI_NULL;
   ViChar *pos = VI_NULL;
   ViStatus errorNumber = VI_SUCCESS;
   ViInt32 bufferSize = 0;
   bufferSize = niDMM_GetError(session, &errorNumber, 0, buffer);
   //  the additional 256 bytes are for the static text before the description.
   pos = buffer = malloc(bufferSize + 256);
   pos += sprintf(pos, "NI-DMM Error\n\nError:%d (0x%x)\n", status, status);
   niDMM_GetError(session, &errorNumber, bufferSize, pos);
   niDMM_ReportStatus("Error!", buffer);
   free(buffer);
}
