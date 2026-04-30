#include <stdlib.h>
#include <stdio.h>
#include "nise.h"

#if defined _CVI_

 #include <userint.h>
 void ReportStatus(NISEConstString title, NISEConstString message)
 {
     MessagePopup(title, message);
 } 

#elif defined _MSC_VER
 #include <conio.h>
 #include <ctype.h>
 #include <string.h>
 void ReportStatus(NISEConstString title, NISEConstString message)
 {
     printf("\n%s:\n%s\n\nPress any key to continue...\n", title, message);
     while(!kbhit());
     getch();
 }
 void myScanf(NISEBuffer* buffer)
 {
     NISEBuffer* pos = buffer;
     NISEBuffer* defaultValue = malloc(strlen(buffer)+1);
     strcpy(defaultValue, buffer);
     //  Using getch() instead of scanf() in order to be able to assume 
     //  the default value when the user hits the <CR> key
     *pos = getch();
     if (*pos == '\r')
     {
         sprintf (pos, "%s", defaultValue);
         printf ("%s\n", pos);
     }
     else
     {
         printf ("%c", *pos++);
         while ((*pos = getch()) != '\r')
         {
             if (isprint(*pos))
             {
                 printf("%c", *pos++);
             }
             else if (*pos == '\b' && pos >= buffer)
             {
                 printf ("%c", *pos);
                 printf (" %c", *pos--);
             }
         }
         *pos = '\0';
     }
     free (defaultValue);
 }

#endif

void HandleError(NISESession session, NISEStatus status)
{
    NISEBuffer *buffer = NULL;
    NISEBuffer *pos;
    NISEStatus errorNumber, bufferSize = 0;
    niSE_GetError(session, &errorNumber, buffer, &bufferSize);
    //  the additional 256 bytes are for the static text before the description.
    pos = buffer = malloc(bufferSize + 256);
    pos += sprintf(pos, "Switch Executive Error\n\nError:%d (0x%x)\n", status, status);
    niSE_GetError(session, &errorNumber, pos, &bufferSize);
    ReportStatus("Error!", buffer);
    free(buffer);
}
