#ifndef _ASCII_PLOT
#define _ASCII_PLOT

#include "niScope.h"

#if defined(__cplusplus) || defined(__cplusplus__)
extern "C" {
#endif

#define COL 80   // 1 more than number of columns for plot
#define ROW 20   // number of rows for plot

void _VI_FUNC asciiPlot(ViReal64 *waveform, ViInt32 size);

#if defined(__cplusplus) || defined(__cplusplus__)
}
#endif

#endif /* _ASCII_PLOT   */
