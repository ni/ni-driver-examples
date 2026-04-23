#include "niswitch.h"

#define niSwitchCheckErr(fnCall)    if( switchError = (fnCall), switchError<VI_SUCCESS){goto Error;}  else

void niSwitch_ErrorHandler(ViSession session, ViStatus error);
