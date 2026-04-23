# Microsoft Developer Studio Generated NMAKE File, Based on OSPBasicIF.dsp
!IF "$(CFG)" == ""
CFG=OSPBasicIF - Win32 Debug
!MESSAGE No configuration specified. Defaulting to OSPBasicIF - Win32 Debug.
!ENDIF 

!IF "$(CFG)" != "OSPBasicIF - Win32 Release" && "$(CFG)" != "OSPBasicIF - Win32 Debug"
!MESSAGE Invalid configuration "$(CFG)" specified.
!MESSAGE You can specify a configuration when running NMAKE
!MESSAGE by defining the macro CFG on the command line. For example:
!MESSAGE 
!MESSAGE NMAKE /f "OSPBasicIF.mak" CFG="OSPBasicIF - Win32 Debug"
!MESSAGE 
!MESSAGE Possible choices for configuration are:
!MESSAGE 
!MESSAGE "OSPBasicIF - Win32 Release" (based on "Win32 (x86) Console Application")
!MESSAGE "OSPBasicIF - Win32 Debug" (based on "Win32 (x86) Console Application")
!MESSAGE 
!ERROR An invalid configuration is specified.
!ENDIF 

!IF "$(OS)" == "Windows_NT"
NULL=
!ELSE 
NULL=nul
!ENDIF 

!IF  "$(CFG)" == "OSPBasicIF - Win32 Release"

OUTDIR=.\Release
INTDIR=.\Release
# Begin Custom Macros
OutDir=.\Release
# End Custom Macros

ALL : "$(OUTDIR)\OSPBasicIF.exe"


CLEAN :
	-@erase "$(INTDIR)\OSPBasicIF.obj"
	-@erase "$(INTDIR)\vc60.idb"
	-@erase "$(OUTDIR)\OSPBasicIF.exe"

"$(OUTDIR)" :
    @(if not exist "$(OUTDIR)/$(NULL)" mkdir "$(OUTDIR)")

CPP=cl.exe
CPP_PROJ=/nologo /MT /W3 /EHsc /O2 /I "$(NIIVIPATH)\Include" /I "$(VXIPNPPATH)\winnt\include" /D "WIN32" /D "NDEBUG" /D "_CONSOLE" /D "_MBCS" /D "_CRT_SECURE_NO_DEPRECATE" /Fp"$(INTDIR)\OSPBasicIF.pch" /Fo"$(INTDIR)\\" /Fd"$(INTDIR)\\" /FD /c 

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
BSC32_FLAGS=/nologo /o"$(OUTDIR)\OSPBasicIF.bsc" 
BSC32_SBRS= \
	
LINK32=link.exe
LINK32_FLAGS=nifgen.lib /nologo /subsystem:console /pdb:"$(OUTDIR)\OSPBasicIF.pdb" /machine:I386 /out:"$(OUTDIR)\OSPBasicIF.exe" /libpath:"$(NIIVIPATH)\Lib\msc" /libpath:"$(VXIPNPPATH)\winnt\lib\msc" 
LINK32_OBJS= \
	"$(INTDIR)\OSPBasicIF.obj"

"$(OUTDIR)\OSPBasicIF.exe" : "$(OUTDIR)" $(DEF_FILE) $(LINK32_OBJS)
    $(LINK32) @<<
  $(LINK32_FLAGS) $(LINK32_OBJS)
<<

!ELSEIF  "$(CFG)" == "OSPBasicIF - Win32 Debug"

OUTDIR=.\Debug
INTDIR=.\Debug
# Begin Custom Macros
OutDir=.\Debug
# End Custom Macros

ALL : "$(OUTDIR)\OSPBasicIF.exe"


CLEAN :
	-@erase "$(INTDIR)\OSPBasicIF.obj"
	-@erase "$(INTDIR)\vc60.idb"
	-@erase "$(INTDIR)\vc60.pdb"
	-@erase "$(OUTDIR)\OSPBasicIF.exe"
	-@erase "$(OUTDIR)\OSPBasicIF.ilk"
	-@erase "$(OUTDIR)\OSPBasicIF.pdb"

"$(OUTDIR)" :
    @(if not exist "$(OUTDIR)/$(NULL)" mkdir "$(OUTDIR)")

CPP=cl.exe
CPP_PROJ=/nologo /MTd /W3 /Gm /EHsc /ZI /Od /I "$(NIIVIPATH)\Include" /I "$(VXIPNPPATH)\winnt\include" /D "WIN32" /D "_DEBUG" /D "_CONSOLE" /D "_MBCS" /D "_CRT_SECURE_NO_DEPRECATE" /Fp"$(INTDIR)\OSPBasicIF.pch" /Fo"$(INTDIR)\\" /Fd"$(INTDIR)\\" /FD /RTC1 /c 

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
BSC32_FLAGS=/nologo /o"$(OUTDIR)\OSPBasicIF.bsc" 
BSC32_SBRS= \
	
LINK32=link.exe
LINK32_FLAGS=nifgen.lib /nologo /subsystem:console /incremental:yes /pdb:"$(OUTDIR)\OSPBasicIF.pdb" /debug /machine:I386 /out:"$(OUTDIR)\OSPBasicIF.exe" /libpath:"$(NIIVIPATH)\Lib\msc" /libpath:"$(VXIPNPPATH)\winnt\lib\msc" 
LINK32_OBJS= \
	"$(INTDIR)\OSPBasicIF.obj"

"$(OUTDIR)\OSPBasicIF.exe" : "$(OUTDIR)" $(DEF_FILE) $(LINK32_OBJS)
    $(LINK32) @<<
  $(LINK32_FLAGS) $(LINK32_OBJS)
<<

!ENDIF 


!IF "$(NO_EXTERNAL_DEPS)" != "1"
!IF EXISTS("OSPBasicIF.dep")
!INCLUDE "OSPBasicIF.dep"
!ENDIF 
!ENDIF 


!IF "$(CFG)" == "OSPBasicIF - Win32 Release" || "$(CFG)" == "OSPBasicIF - Win32 Debug"
SOURCE=.\OSPBasicIF.c

"$(INTDIR)\OSPBasicIF.obj" : $(SOURCE) "$(INTDIR)"



!ENDIF 

