# Microsoft Developer Studio Generated NMAKE File, Based on RFSA Getting Started Spectrum.dsp

# ------------------------------------------------------------------------
#  You may need to edit the path specified for IVI if it is not installed
#  in the expected $(PROGRAMFILES) location.
# ------------------------------------------------------------------------

!IF "$(CFG)" == ""
CFG=RFSA Getting Started Spectrum - Win32 Debug
!MESSAGE No configuration specified. Defaulting to RFSA Getting Started Spectrum - Win32 Debug.
!ENDIF 

!IF "$(CFG)" != "RFSA Getting Started Spectrum - Win32 Release" && "$(CFG)" != "RFSA Getting Started Spectrum - Win32 Debug"
!MESSAGE Invalid configuration "$(CFG)" specified.
!MESSAGE You can specify a configuration when running NMAKE
!MESSAGE by defining the macro CFG on the command line. For example:
!MESSAGE 
!MESSAGE NMAKE /f "RFSA Getting Started Spectrum.mak" CFG="RFSA Getting Started Spectrum - Win32 Debug"
!MESSAGE 
!MESSAGE Possible choices for configuration are:
!MESSAGE 
!MESSAGE "RFSA Getting Started Spectrum - Win32 Release" (based on "Win32 (x86) Console Application")
!MESSAGE "RFSA Getting Started Spectrum - Win32 Debug" (based on "Win32 (x86) Console Application")
!MESSAGE 
!ERROR An invalid configuration is specified.
!ENDIF 

!IF "$(OS)" == "Windows_NT"
NULL=
!ELSE 
NULL=nul
!ENDIF 

!IF  "$(CFG)" == "RFSA Getting Started Spectrum - Win32 Release"

OUTDIR=.\Release
INTDIR=.\Release
# Begin Custom Macros
OutDir=.\Release
# End Custom Macros

ALL : "$(OUTDIR)\RFSA Getting Started Spectrum.exe"


CLEAN :
	-@erase "$(INTDIR)\RFSA Getting Started Spectrum.obj"
	-@erase "$(INTDIR)\vc60.idb"
	-@erase "$(OUTDIR)\RFSA Getting Started Spectrum.exe"

"$(OUTDIR)" :
    if not exist "$(OUTDIR)/$(NULL)" mkdir "$(OUTDIR)"

CPP=cl.exe
CPP_PROJ=/nologo /MT /W3 /EHsc /O2 /I "$(NIIVIPATH)\Include" /I "$(VXIPNPPATH)\winnt\include" /D "WIN32" /D "NDEBUG" /D "_CONSOLE" /D "_MBCS" /D "_CRT_SECURE_NO_DEPRECATE" /Fp"$(INTDIR)\RFSA Getting Started Spectrum.pch" /Fo"$(INTDIR)\\" /Fd"$(INTDIR)\\" /FD /c 

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
BSC32_FLAGS=/nologo /o"$(OUTDIR)\RFSA Getting Started Spectrum.bsc" 
BSC32_SBRS= \
	
LINK32=link.exe
LINK32_FLAGS=niRFSA.lib /nologo /subsystem:console /pdb:"$(OUTDIR)\RFSA Getting Started Spectrum.pdb" /machine:I386 /out:"$(OUTDIR)\RFSA Getting Started Spectrum.exe" /libpath:"$(NIIVIPATH)\Lib\msc" /libpath:"$(VXIPNPPATH)\winnt\lib\msc" 
LINK32_OBJS= \
	"$(INTDIR)\RFSA Getting Started Spectrum.obj"

"$(OUTDIR)\RFSA Getting Started Spectrum.exe" : "$(OUTDIR)" $(DEF_FILE) $(LINK32_OBJS)
    $(LINK32) @<<
  $(LINK32_FLAGS) $(LINK32_OBJS)
<<

!ELSEIF  "$(CFG)" == "RFSA Getting Started Spectrum - Win32 Debug"

OUTDIR=.\Debug
INTDIR=.\Debug
# Begin Custom Macros
OutDir=.\Debug
# End Custom Macros

ALL : "$(OUTDIR)\RFSA Getting Started Spectrum.exe"


CLEAN :
	-@erase "$(INTDIR)\RFSA Getting Started Spectrum.obj"
	-@erase "$(INTDIR)\vc60.idb"
	-@erase "$(INTDIR)\vc60.pdb"
	-@erase "$(OUTDIR)\RFSA Getting Started Spectrum.exe"
	-@erase "$(OUTDIR)\RFSA Getting Started Spectrum.ilk"
	-@erase "$(OUTDIR)\RFSA Getting Started Spectrum.pdb"

"$(OUTDIR)" :
    if not exist "$(OUTDIR)/$(NULL)" mkdir "$(OUTDIR)"

CPP=cl.exe
CPP_PROJ=/nologo /MTd /W3 /Gm /EHsc /ZI /Od /I "$(NIIVIPATH)\Include" /I "$(VXIPNPPATH)\winnt\include" /D "WIN32" /D "_DEBUG" /D "_CONSOLE" /D "_MBCS" /D "_CRT_SECURE_NO_DEPRECATE" /Fp"$(INTDIR)\RFSA Getting Started Spectrum.pch" /Fo"$(INTDIR)\\" /Fd"$(INTDIR)\\" /FD /RTC1 /c 

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
BSC32_FLAGS=/nologo /o"$(OUTDIR)\RFSA Getting Started Spectrum.bsc" 
BSC32_SBRS= \
	
LINK32=link.exe
LINK32_FLAGS=niRFSA.lib /nologo /subsystem:console /incremental:yes /pdb:"$(OUTDIR)\RFSA Getting Started Spectrum.pdb" /debug /machine:I386 /out:"$(OUTDIR)\RFSA Getting Started Spectrum.exe" /libpath:"$(NIIVIPATH)\Lib\msc" /libpath:"$(VXIPNPPATH)\winnt\lib\msc" 
LINK32_OBJS= \
	"$(INTDIR)\RFSA Getting Started Spectrum.obj"

"$(OUTDIR)\RFSA Getting Started Spectrum.exe" : "$(OUTDIR)" $(DEF_FILE) $(LINK32_OBJS)
    $(LINK32) @<<
  $(LINK32_FLAGS) $(LINK32_OBJS)
<<

!ENDIF 


!IF "$(NO_EXTERNAL_DEPS)" != "1"
!IF EXISTS("RFSA Getting Started Spectrum.dep")
!INCLUDE "RFSA Getting Started Spectrum.dep"
!ELSE 
!MESSAGE Warning: cannot find "RFSA Getting Started Spectrum.dep"
!ENDIF 
!ENDIF 


!IF "$(CFG)" == "RFSA Getting Started Spectrum - Win32 Release" || "$(CFG)" == "RFSA Getting Started Spectrum - Win32 Debug"
SOURCE=".\RFSA Getting Started Spectrum.c"

"$(INTDIR)\RFSA Getting Started Spectrum.obj" : $(SOURCE) "$(INTDIR)"



!ENDIF 


