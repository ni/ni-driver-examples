#
# makefile for the project DynamicAcquisitionHardwareReferenceTrigger
#
# To run this from command prompt type:
#    nmake /f DynamicAcquisitionHardwareReferenceTrigger.mak CFG=<debug | release>
#
# defaults to debug build if no configuration is specified.
#
PROJECT=DynamicAcquisitionHardwareReferenceTrigger
!IF "$(CFG)" == ""
CFG=debug
!MESSAGE No configuration specified. Defaulting to debug
!ENDIF

!IF "$(CFG)" != "debug" && "$(CFG)" != "release"
!MESSAGE Invalid configuration "$(CFG)" specified.
!MESSAGE You can specify a configuration when running NMAKE
!MESSAGE by defining the macro CFG on the command line. For example:
!MESSAGE
!MESSAGE NMAKE /f "$(PROJECT).mak" CFG="debug"
!MESSAGE
!MESSAGE Possible choices for configuration are:
!MESSAGE
!MESSAGE release (based on "Win32 (x86) Console Application")
!MESSAGE debug (based on "Win32 (x86) Console Application")
!MESSAGE
!ERROR An invalid configuration is specified.
!ENDIF

CC=cl.exe
RSC=rc.exe

#
# This makefile uses the "Program Files\Ivi" directory for niHSDIO.lib and for
# niTClk.lib
#

LIBRARIES=\
    niHSDIO.lib \
    niTClk.lib


INCLUDES=/I "$(NIIVIPATH)\Include" /I "$(VXIPNPPATH)\WinNT\include" /I "$(NIEXTCCOMPILERSUPP)\include"
LIBS=/libpath:"$(NIIVIPATH)\Lib\msc" /libpath:"$(VXIPNPPATH)\WinNT\lib\msc" /libpath:"$(NIEXTCCOMPILERSUPP)\lib32\msvc"


DEFINES=/D "WIN32" /D "_CONSOLE" /D "_MBCS"

#
# discriminate between a release and debug build
#

!IF  "$(CFG)" == "release"

OUTDIR=.\Release
INTDIR=.\Release

all : "$(OUTDIR)\$(PROJECT).exe"

clean :
	-@erase "$(INTDIR)\$(PROJECT).obj"
	-@erase "$(INTDIR)\vc60.idb"
	-@erase "$(OUTDIR)\$(PROJECT).exe"

CC_FLAGS=\
   /nologo /ML /W3 /GX /O2 $(INCLUDES) $(DEFINES) /D "NDEBUG" \
   /Fp"$(INTDIR)\$(PROJECT).pch" /YX /Fo"$(INTDIR)\\" \
   /Fd"$(INTDIR)\\" /FD /c

BSC32=bscmake.exe
BSC32_FLAGS=/nologo /o"$(OUTDIR)\$(PROJECT).bsc"
BSC32_SBRS= \
	
LINK32=link.exe
LINK32_FLAGS=\
   $(LIBRARIES) /nologo /subsystem:console /incremental:no \
   /pdb:"$(OUTDIR)\$(PROJECT).pdb" /machine:I386 \
   /out:"$(OUTDIR)\$(PROJECT).exe" \
   $(LIBS)

LINK32_OBJS= \
	"$(INTDIR)\$(PROJECT).obj"

"$(OUTDIR)\$(PROJECT).exe" : "$(OUTDIR)" $(DEF_FILE) $(LINK32_OBJS)
    $(LINK32) @<<
  $(LINK32_FLAGS) $(LINK32_OBJS)
<<

!ELSEIF  "$(CFG)" == "debug"

OUTDIR=.\Debug
INTDIR=.\Debug

all : "$(OUTDIR)\$(PROJECT).exe"


clean :
	-@erase "$(INTDIR)\$(PROJECT).obj"
	-@erase "$(INTDIR)\vc60.idb"
	-@erase "$(INTDIR)\vc60.pdb"
	-@erase "$(OUTDIR)\$(PROJECT).exe"
	-@erase "$(OUTDIR)\$(PROJECT).ilk"
	-@erase "$(OUTDIR)\$(PROJECT).pdb"

CC_FLAGS=\
   /nologo /MLd /W3 /Gm /GX /ZI /Od $(INCLUDES) $(DEFINES) /D "_DEBUG" \
   /Fp"$(INTDIR)\$(PROJECT).pch" /YX /Fo"$(INTDIR)\\" \
   /Fd"$(INTDIR)\\" /FD /GZ /c

BSC32=bscmake.exe
BSC32_FLAGS=/nologo /o"$(OUTDIR)\$(PROJECT).bsc"
BSC32_SBRS= \
	
LINK32=link.exe
LINK32_FLAGS=\
   $(LIBRARIES) /nologo /subsystem:console /incremental:yes \
   /pdb:"$(OUTDIR)\$(PROJECT).pdb" /debug /machine:I386 \
   /out:"$(OUTDIR)\$(PROJECT).exe" /pdbtype:sept \
   $(LIBS)

LINK32_OBJS= \
	"$(INTDIR)\$(PROJECT).obj"

"$(OUTDIR)\$(PROJECT).exe" : "$(OUTDIR)" $(DEF_FILE) $(LINK32_OBJS)
    $(LINK32) @<<
  $(LINK32_FLAGS) $(LINK32_OBJS)
<<

!ENDIF

"$(OUTDIR)" :
    if not exist "$(OUTDIR)/$(NULL)" mkdir "$(OUTDIR)"

.c{$(INTDIR)}.obj::
   $(CC) @<<
   $(CC_FLAGS) $<
<<

.cpp{$(INTDIR)}.obj::
   $(CC) @<<
   $(CC_FLAGS) $<
<<

.cxx{$(INTDIR)}.obj::
   $(CC) @<<
   $(CC_FLAGS) $<
<<

.c{$(INTDIR)}.sbr::
   $(CC) @<<
   $(CC_FLAGS) $<
<<

.cpp{$(INTDIR)}.sbr::
   $(CC) @<<
   $(CC_FLAGS) $<
<<

.cxx{$(INTDIR)}.sbr::
   $(CC) @<<
   $(CC_FLAGS) $<
<<


!IF "$(NO_EXTERNAL_DEPS)" != "1"
!IF EXISTS("$(PROJECT).dep")
!INCLUDE "$(PROJECT).dep"
!ENDIF
!ENDIF


!IF "$(CFG)" == "release" || "$(CFG)" == "debug"
SOURCE=.\$(PROJECT).c

"$(INTDIR)\$(PROJECT).obj" : $(SOURCE) "$(INTDIR)"

!ENDIF
