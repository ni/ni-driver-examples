# Microsoft Developer Studio Generated NMAKE File, Based on SynchronizeTClk.dsp
!IF "$(CFG)" == ""
CFG=SynchronizeTClk - Win32 Debug
!MESSAGE No configuration specified. Defaulting to SynchronizeTClk - Win32 Debug.
!ENDIF 

!IF "$(CFG)" != "SynchronizeTClk - Win32 Release" && "$(CFG)" != "SynchronizeTClk - Win32 Debug"
!MESSAGE Invalid configuration "$(CFG)" specified.
!MESSAGE You can specify a configuration when running NMAKE
!MESSAGE by defining the macro CFG on the command line. For example:
!MESSAGE 
!MESSAGE NMAKE /f "SynchronizeTClk.mak" CFG="SynchronizeTClk - Win32 Debug"
!MESSAGE 
!MESSAGE Possible choices for configuration are:
!MESSAGE 
!MESSAGE "SynchronizeTClk - Win32 Release" (based on "Win32 (x86) Console Application")
!MESSAGE "SynchronizeTClk - Win32 Debug" (based on "Win32 (x86) Console Application")
!MESSAGE 
!ERROR An invalid configuration is specified.
!ENDIF 

!IF "$(OS)" == "Windows_NT"
NULL=
!ELSE 
NULL=nul
!ENDIF 

!IF  "$(CFG)" == "SynchronizeTClk - Win32 Release"

OUTDIR=.\Release
INTDIR=.\Release
# Begin Custom Macros
OutDir=.\Release
# End Custom Macros

ALL : "$(OUTDIR)\SynchronizeTClk.exe"


CLEAN :
	-@erase "$(INTDIR)\SynchronizeTClk.obj"
	-@erase "$(INTDIR)\vc60.idb"
	-@erase "$(OUTDIR)\SynchronizeTClk.exe"

"$(OUTDIR)" :
    @(if not exist "$(OUTDIR)/$(NULL)" mkdir "$(OUTDIR)")

CPP=cl.exe
CPP_PROJ=/nologo /MT /W3 /EHsc /O2 /I "$(NIIVIPATH)\Include" /I "$(VXIPNPPATH)\winnt\include" /D "WIN32" /D "NDEBUG" /D "_CONSOLE" /D "_MBCS" /D "_CRT_SECURE_NO_DEPRECATE" /Fp"$(INTDIR)\SynchronizeTClk.pch" /Fo"$(INTDIR)\\" /Fd"$(INTDIR)\\" /FD /c 

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
BSC32_FLAGS=/nologo /o"$(OUTDIR)\SynchronizeTClk.bsc" 
BSC32_SBRS= \
	
LINK32=link.exe
LINK32_FLAGS=nifgen.lib niTClk.lib /nologo /subsystem:console /pdb:"$(OUTDIR)\SynchronizeTClk.pdb" /machine:I386 /out:"$(OUTDIR)\SynchronizeTClk.exe" /libpath:"$(NIIVIPATH)\Lib\msc" /libpath:"$(VXIPNPPATH)\winnt\lib\msc" 
LINK32_OBJS= \
	"$(INTDIR)\SynchronizeTClk.obj"

"$(OUTDIR)\SynchronizeTClk.exe" : "$(OUTDIR)" $(DEF_FILE) $(LINK32_OBJS)
    $(LINK32) @<<
  $(LINK32_FLAGS) $(LINK32_OBJS)
<<

!ELSEIF  "$(CFG)" == "SynchronizeTClk - Win32 Debug"

OUTDIR=.\Debug
INTDIR=.\Debug
# Begin Custom Macros
OutDir=.\Debug
# End Custom Macros

ALL : "$(OUTDIR)\SynchronizeTClk.exe"


CLEAN :
	-@erase "$(INTDIR)\SynchronizeTClk.obj"
	-@erase "$(INTDIR)\vc60.idb"
	-@erase "$(INTDIR)\vc60.pdb"
	-@erase "$(OUTDIR)\SynchronizeTClk.exe"
	-@erase "$(OUTDIR)\SynchronizeTClk.ilk"
	-@erase "$(OUTDIR)\SynchronizeTClk.pdb"

"$(OUTDIR)" :
    @(if not exist "$(OUTDIR)/$(NULL)" mkdir "$(OUTDIR)")

CPP=cl.exe
CPP_PROJ=/nologo /MTd /W3 /Gm /EHsc /ZI /Od /I "$(NIIVIPATH)\Include" /I "$(VXIPNPPATH)\winnt\include" /D "WIN32" /D "_DEBUG" /D "_CONSOLE" /D "_MBCS" /D "_CRT_SECURE_NO_DEPRECATE" /Fp"$(INTDIR)\SynchronizeTClk.pch" /Fo"$(INTDIR)\\" /Fd"$(INTDIR)\\" /FD /RTC1 /c 

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
BSC32_FLAGS=/nologo /o"$(OUTDIR)\SynchronizeTClk.bsc" 
BSC32_SBRS= \
	
LINK32=link.exe
LINK32_FLAGS=nifgen.lib niTClk.lib /nologo /subsystem:console /incremental:yes /pdb:"$(OUTDIR)\SynchronizeTClk.pdb" /debug /machine:I386 /out:"$(OUTDIR)\SynchronizeTClk.exe" /libpath:"$(NIIVIPATH)\Lib\msc" /libpath:"$(VXIPNPPATH)\winnt\lib\msc" 
LINK32_OBJS= \
	"$(INTDIR)\SynchronizeTClk.obj"

"$(OUTDIR)\SynchronizeTClk.exe" : "$(OUTDIR)" $(DEF_FILE) $(LINK32_OBJS)
    $(LINK32) @<<
  $(LINK32_FLAGS) $(LINK32_OBJS)
<<

!ENDIF 


!IF "$(NO_EXTERNAL_DEPS)" != "1"
!IF EXISTS("SynchronizeTClk.dep")
!INCLUDE "SynchronizeTClk.dep"
!ENDIF 
!ENDIF 


!IF "$(CFG)" == "SynchronizeTClk - Win32 Release" || "$(CFG)" == "SynchronizeTClk - Win32 Debug"
SOURCE=.\SynchronizeTClk.c

"$(INTDIR)\SynchronizeTClk.obj" : $(SOURCE) "$(INTDIR)"



!ENDIF 

