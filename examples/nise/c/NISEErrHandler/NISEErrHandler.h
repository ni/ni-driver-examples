#ifndef _NISEERRHANDLER_H_
 #define _NISEERRHANDLER_H_


#include "nise.h"

#define niseCheckErr(fnCall)    if( status = (fnCall), status<NISE_ERROR_NONE){goto Error;}  else


void HandleError(NISESession session, NISEStatus status);
void ReportStatus(NISEConstString title, NISEConstString message);
#ifdef _MSC_VER
 void myScanf(NISEBuffer* buffer);
#endif

#endif // #ifndef _NISEERRHANDLER_H_