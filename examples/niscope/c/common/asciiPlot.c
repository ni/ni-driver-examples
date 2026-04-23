#include "asciiPlot.h"
#include <stdio.h>

void _VI_FUNC asciiPlot (ViReal64 *waveform, ViInt32 size)
{
   ViInt32   i, j, iWfm=0;
   ViReal64  min, max, sum;
   ViChar    plotArray[ROW][COL];
   ViInt32   pntsPerCol, iRow, iColumn;
   ViChar    dot = 'o';

   if (size <= 0) return;

   // Find the min and max voltage of the waveform to scale the plotArray.
   // Note, this could be done with the niScope_FetchWaveformMeasurement function
   // but we've chosen to implement it separately.
   min = max = waveform[0];
   for (i=1; i<size; i++)
   {
      if (waveform[i] > max) max = waveform[i];
      if (waveform[i] < min) min = waveform[i];
   }


   // Initialize the plotArray array
   for (i=0; i<ROW; i++)
   {
      plotArray[i][COL-1] = 0;  // End of string marker at the end of each line
      for (j=0; j<COL-1; j++)
         plotArray[i][j] = ' '; // Initialize 2D array as spaces
   }

   // Average data so each column has a dot representing the average of all the points in
   // that fall in that column.
   pntsPerCol = size / (COL-1); // Num. of waveform points averaged for each plotted point

   // If fewer points than columns in the plot, no averaging is necessary
   if (pntsPerCol == 0)
      pntsPerCol = 1;

   for (i=0; i<COL-1 && iWfm < size; i++)
   {
      sum = 0.0;
      for (j=0; j<pntsPerCol; j++)
         sum += waveform[iWfm++];

      // Find the row index for where the point should be plotted.
      iRow = (ViInt32)( (ROW-1) * ((sum / pntsPerCol) - min) / (max-min) );
      if (iRow > ROW-1) iRow = ROW-1;
      if (iRow < 0) iRow = 0;
      iRow = (ROW-1) - iRow;

      // If fewer points than columns in the plot, some columns are skipped
      // and don't get dots.  Otherwise, every column gets a dot.
      if (size < COL-1)
      {
         iColumn = (ViInt32)((ViReal64)(iWfm-1) * (COL-2) / (size-1));
         if (iColumn > COL-2) iColumn = COL-2;
         if (iColumn < 0) iColumn = 0;
      }
      else
         iColumn = i;

      plotArray[iRow][iColumn] = dot;
   }

   // Plot the array by printing each row's string.
   for (i=0; i<ROW; i++)
      printf("%s\n", plotArray[i]);
   printf ("Min Voltage: %f\tMax Voltage: %f\n", min, max);
}
