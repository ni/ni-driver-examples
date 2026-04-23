# Microsoft Developer Studio Generated NMAKE File, Based on ArbitraryWaveform.dsp
!IF "$(CFG)" == ""
CFG=ArbitraryWaveform - Win32 Debug
!MESSAGE No configuration specified. Defaulting to ArbitraryWaveform - Win32 Debug.
!ENDIF 

!IF "$(CFG)" != "ArbitraryWaveform - Win32 Release" && "$(CFG)" != "ArbitraryWaveform - Win32 Debug"
!MESSAGE Invalid configuration "$(CFG)" specified.
!MESSAGE You can specify a configuration when running NMAKE
!MESSAGE by defining the macro CFG on the command line. For example:
!MESSAGE 
!MESSAGE NMAKE /f "ArbitraryWaveform.mak" CFG="ArbitraryWaveform - Win32 Debug"
!MESSAGE 
!MESSAGE Possible choices for configuration are:
!MESSAGE 
!MESSAGE "ArbitraryWaveform - Win32 Release" (based on "Win32 (x86) Console Application")
!MESSAGE "ArbitraryWaveform - Win32 Debug" (based on "Win32 (x86) Console Application")
!MESSAGE 
!ERROR An invalid configuration is specified.
!ENDIF 

!IF "$(OS)" == "Windows_NT"
NULL=
!ELSE 
NULL=nul
!ENDIF 

!IF  "$(CFG)" == "ArbitraryWaveform - Win32 Release"

OUTDIR=.\Release
INTDIR=.\Release
# Begin Custom Macros
OutDir=.\Release
# End Custom Macros

ALL : "$(OUTDIR)\ArbitraryWaveform.exe"


CLEAN :
	-@erase "$(INTDIR)\ArbitraryWaveformExample.obj"
	-@erase "$(INTDIR)\vc60.idb"
	-@erase "$(OUTDIR)\ArbitraryWaveform.exe"

"$(OUTDIR)" :
    @(if not exist "$(OUTDIR)/$(NULL)" mkdir "$(OUTDIR)")

CPP=cl.exe
CPP_PROJ=/nologo /MT /W3 /EHsc /O2 /I "$(NIIVIPATH)\Include" /I "$(VXIPNPPATH)\winnt\include" /D "WIN32" /D "NDEBUG" /D "_CONSOLE" /D "_MBCS" /D "_CRT_SECURE_NO_DEPRECATE" /Fp"$(INTDIR)\ArbitraryWaveform.pch" /Fo"$(INTDIR)\\" /Fd"$(INTDIR)\\" /FD /c 

.c{$(INTDIR)}.obj::
   $(CPP) @<<
   $(CPP_PROJ) $< 
<<

.cpp{$(INTDIR)}.obj::
   $(CPP) @<<
   $(CPP_PROJ) $< 
<<

.cxx{$(INTDIR)}.obj::
   $(CPP) @<<
   $(CPP_PROJ) $< 
<<

.c{$(INTDIR)}.sbr::
   $(CPP) @<<
   $(CPP_PROJ) $< 
<<

.cpp{$(INTDIR)}.sbr::
   $(CPP) @<<
   $(CPP_PROJ) $< 
<<

.cxx{$(INTDIR)}.sbr::
   $(CPP) @<<
   $(CPP_PROJ) $< 
<<

RSC=rc.exe
BSC32=bscmake.exe
BSC32_FLAGS=/nologo /o"$(OUTDIR)\ArbitraryWaveform.bsc" 
BSC32_SBRS= \
	
LINK32=link.exe
LINK32_FLAGS=nifgen.lib /nologo /subsystem:console /pdb:"$(OUTDIR)\ArbitraryWaveform.pdb" /machine:I386 /out:"$(OUTDIR)\ArbitraryWaveform.exe" /libpath:"$(NIIVIPATH)\Lib\msc" /libpath:"$(VXIPNPPATH)\winnt\lib\msc" 
LINK32_OBJS= \
	"$(INTDIR)\ArbitraryWaveformExample.obj"

"$(OUTDIR)\ArbitraryWaveform.exe" : "$(OUTDIR)" $(DEF_FILE) $(LINK32_OBJS)
    $(LINK32) @<<
  $(LINK32_FLAGS) $(LINK32_OBJS)
<<

!ELSEIF  "$(CFG)" == "ArbitraryWaveform - Win32 Debug"

OUTDIR=.\Debug
INTDIR=.\Debug
# Begin Custom Macros
OutDir=.\Debug
# End Custom Macros

ALL : "$(OUTDIR)\ArbitraryWaveform.exe"


CLEAN :
	-@erase "$(INTDIR)\ArbitraryWaveformExample.obj"
	-@erase "$(INTDIR)\vc60.idb"
	-@erase "$(INTDIR)\vc60.pdb"
	-@erase "$(OUTDIR)\ArbitraryWaveform.exe"
	-@erase "$(OUTDIR)\ArbitraryWaveform.ilk"
	-@erase "$(OUTDIR)\ArbitraryWaveform.pdb"

"$(OUTDIR)" :
    @(if not exist "$(OUTDIR)/$(NULL)" mkdir "$(OUTDIR)")

CPP=cl.exe
CPP_PROJ=/nologo /MTd /W3 /Gm /EHsc /ZI /Od /I "$(NIIVIPATH)\Include" /I "$(VXIPNPPATH)\winnt\include" /D "WIN32" /D "_DEBUG" /D "_CONSOLE" /D "_MBCS" /D "_CRT_SECURE_NO_DEPRECATE" /Fp"$(INTDIR)\ArbitraryWaveform.pch" /Fo"$(INTDIR)\\" /Fd"$(INTDIR)\\" /FD /RTC1 /c 

.c{$(INTDIR)}.obj::
   $(CPP) @<<
   $(CPP_PROJ) $< 
<<

.cpp{$(INTDIR)}.obj::
   $(CPP) @<<
   $(CPP_PROJ) $< 
<<

.cxx{$(INTDIR)}.obj::
   $(CPP) @<<
   $(CPP_PROJ) $< 
<<

.c{$(INTDIR)}.sbr::
   $(CPP) @<<
   $(CPP_PROJ) $< 
<<

.cpp{$(INTDIR)}.sbr::
   $(CPP) @<<
   $(CPP_PROJ) $< 
<<

.cxx{$(INTDIR)}.sbr::
   $(CPP) @<<
   $(CPP_PROJ) $< 
<<

RSC=rc.exe
BSC32=bscmake.exe
BSC32_FLAGS=/nologo /o"$(OUTDIR)\ArbitraryWaveform.bsc" 
BSC32_SBRS= \
	
LINK32=link.exe
LINK32_FLAGS=nifgen.lib /nologo /subsystem:console /incremental:yes /pdb:"$(OUTDIR)\ArbitraryWaveform.pdb" /debug /machine:I386 /out:"$(OUTDIR)\ArbitraryWaveform.exe" /libpath:"$(NIIVIPATH)\Lib\msc" /libpath:"$(VXIPNPPATH)\winnt\lib\msc" 
LINK32_OBJS= \
	"$(INTDIR)\ArbitraryWaveformExample.obj"

"$(OUTDIR)\ArbitraryWaveform.exe" : "$(OUTDIR)" $(DEF_FILE) $(LINK32_OBJS)
    $(LINK32) @<<
  $(LINK32_FLAGS) $(LINK32_OBJS)
<<

!ENDIF 


!IF "$(NO_EXTERNAL_DEPS)" != "1"
!IF EXISTS("ArbitraryWaveform.dep")
!INCLUDE "ArbitraryWaveform.dep"
!ENDIF 
!ENDIF 


!IF "$(CFG)" == "ArbitraryWaveform - Win32 Release" || "$(CFG)" == "ArbitraryWaveform - Win32 Debug"
SOURCE=.\ArbitraryWaveformExample.c

"$(INTDIR)\ArbitraryWaveformExample.obj" : $(SOURCE) "$(INTDIR)"



!ENDIF 

