/***************************************************************************
   Header file for NI-IMAQ example using Windows SDK
 ***************************************************************************/

/***************************************************************************
   constant definitions
 ***************************************************************************/

#define PB_QUIT     101       /* id for quit application push button */
#define PB_SNAP     102       /* id for execute AI_VRead push button */

#define MAXSTRINGLENGTH 80    /* maximum length of character string */


/***************************************************************************
   function prototypes
 ***************************************************************************/

#ifndef WINVER
   #define  WINAPI            far PASCAL
   typedef  unsigned int      UINT;
   typedef  UINT              WPARAM;            
   typedef  LONG              LPARAM;            
#endif


