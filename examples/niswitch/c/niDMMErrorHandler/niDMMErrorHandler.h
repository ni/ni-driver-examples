#include "nidmm.h"

#define niDMMCheckErr(fnCall)    if( dmmError = (fnCall), dmmError<VI_SUCCESS){goto Error;}  else

void niDMM_ErrorHandler(ViSession session, ViStatus error);
