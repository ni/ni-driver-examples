#   National Instruments C Makefile
#
#   Copyright (c) 2009
#   National Instruments Corporation.
#   All rights reserved.

!IF "$(CFG)" != "Win32 Release" && "$(CFG)" != "Win32 Debug" && "$(CFG)" != "Win64 Release" && "$(CFG)" != "Win64 Debug"

!MESSAGE Invalid configuration "$(CFG)" specified.
!MESSAGE You can specify a configuration when running NMAKE
!MESSAGE by defining the macro CFG on the command line. For example:
!MESSAGE
!MESSAGE NMAKE [/f "Makefile"] CFG="Win32 Debug"
!MESSAGE
!MESSAGE Possible choices for configuration are:
!MESSAGE
!MESSAGE "Win32 Release" (based on "Win32 (x86) Console Application")
!MESSAGE "Win32 Debug" (based on "Win32 (x86) Console Application")
!MESSAGE "Win64 Release" (based on "Win32 (x64) Console Application")
!MESSAGE "Win64 Debug" (based on "Win32 (x64) Console Application")
!MESSAGE
!MESSAGE 64-bit support is available with Visual Studio 2005 and higher.
!MESSAGE To build an x64 configuration, you will need to set the environment to use the x64 tools.
!MESSAGE To do so, run vcvarsall.bat (usually in C:\Program Files\Microsoft Visual Studio (version)\VC\)
!MESSAGE with "amd64" as an argument if you are using a 64-bit build machine, or "x86_amd64" if
!MESSAGE you are building on a 32-bit machine. To build a 32-bit configuration, pass "x86" to vcvarsall.bat.
!MESSAGE
!MESSAGE If you accidentally try to compile a configuration with the wrong environment set,
!MESSAGE you will need to correct the configuration and run "clean" for that configuration before
!MESSAGE it can be built successfully.
!MESSAGE
!MESSAGE You can build 32-bit targets with older versions of Visual Studio (2003 or older)
!MESSAGE by first setting your environment with vcvars32.bat from that Visual Studio's directory.
!MESSAGE
!ERROR An invalid configuration is specified.
!ENDIF

!IF "$(OS)" == "Windows_NT"
NULL=
!ELSE
NULL=nul
!ENDIF

# Specify the file name to be compiled
FILENAME = niSwitchControllingAnIndividualRelay


!IF  "$(CFG)" == "Win32 Release"

OUTDIR=.\Release
INTDIR=.\Release
# Begin Custom Macros
OutDir=.\Release
# End Custom Macros

ALL : "$(OUTDIR)\$(FILENAME).exe"


CLEAN :
        -@erase "$(INTDIR)\*.obj"
        -@erase "$(OUTDIR)\$(FILENAME).exe"
        -@erase "$(OUTDIR)\*.idb"

"$(OUTDIR)" :
    if not exist "$(OUTDIR)/$(NULL)" mkdir "$(OUTDIR)"

CPP=cl.exe
CPP_PROJ=/nologo /W3 /O2 /I "$(NIIVIPATH)\Include" /I "$(VXIPNPPATH)\WinNT\include" /I ..\niSwitchErrorHandler /D "WIN32" /D "NDEBUG" /D "_CONSOLE" /D "_MBCS" /Fo"$(INTDIR)\\" /Fd"$(INTDIR)\\" /FD /c 

.c{$(INTDIR)}.obj::
   $(CPP) @<<
   $(CPP_PROJ) $<
<<

RSC=rc.exe
BSC32=bscmake.exe
BSC32_FLAGS=/nologo /o"$(OUTDIR)\$(FILENAME).bsc"
BSC32_SBRS= \


LINK32=link.exe
LINK32_FLAGS=niswitch.lib  /nologo /subsystem:console /incremental:no /pdb:"$(OUTDIR)\$(FILENAME).pdb" /machine:I386 /out:"$(OUTDIR)\$(FILENAME).exe" /libpath:"$(NIIVIPATH)\Lib\msc"
LINK32_OBJS= \
        "$(INTDIR)\$(FILENAME).obj" \
	"$(INTDIR)\niSwitchErrorHandler.obj" \


"$(OUTDIR)\$(FILENAME).exe" : "$(OUTDIR)" $(DEF_FILE) $(LINK32_OBJS)
    $(LINK32) @<<
  $(LINK32_FLAGS) $(LINK32_OBJS)
<<

!ELSEIF  "$(CFG)" == "Win32 Debug"

OUTDIR=.\Debug
INTDIR=.\Debug
# Begin Custom Macros
OutDir=.\Debug
# End Custom Macros

ALL : "$(OUTDIR)\$(FILENAME).exe" "$(OUTDIR)\$(FILENAME).bsc"


CLEAN :
        -@erase "$(INTDIR)\*.obj"
        -@erase "$(INTDIR)\*.sbr"
        -@erase "$(OUTDIR)\$(FILENAME).bsc"
        -@erase "$(OUTDIR)\$(FILENAME).exe"
        -@erase "$(OUTDIR)\$(FILENAME).ilk"
        -@erase "$(OUTDIR)\*.idb"
        -@erase "$(OUTDIR)\*.pdb"

"$(OUTDIR)" :
    if not exist "$(OUTDIR)/$(NULL)" mkdir "$(OUTDIR)"

CPP=cl.exe
CPP_PROJ=/nologo /W3 /Gm /ZI /Od /I "$(NIIVIPATH)\Include" /I "$(VXIPNPPATH)\WinNT\include" /I ..\niSwitchErrorHandler /D "WIN32" /D "_DEBUG" /D "_CONSOLE" /D "_MBCS" /FR"$(INTDIR)\\" /Fo"$(INTDIR)\\" /Fd"$(INTDIR)\\" /FD /c 

.c{$(INTDIR)}.obj::
   $(CPP) @<<
   $(CPP_PROJ) $<
<<

RSC=rc.exe
BSC32=bscmake.exe
BSC32_FLAGS=/nologo /o"$(OUTDIR)\$(FILENAME).bsc"
BSC32_SBRS= \
        "$(INTDIR)\$(FILENAME).sbr"

"$(OUTDIR)\$(FILENAME).bsc" : "$(OUTDIR)" $(BSC32_SBRS)
    $(BSC32) @<<
  $(BSC32_FLAGS) $(BSC32_SBRS)
<<

LINK32=link.exe
LINK32_FLAGS=niswitch.lib  /nologo /subsystem:console /incremental:yes /pdb:"$(OUTDIR)\$(FILENAME).pdb" /debug /machine:I386 /out:"$(OUTDIR)\$(FILENAME).exe" /libpath:"$(NIIVIPATH)\Lib\msc"
LINK32_OBJS= \
        "$(INTDIR)\$(FILENAME).obj" \
	"$(INTDIR)\niSwitchErrorHandler.obj" \


"$(OUTDIR)\$(FILENAME).exe" : "$(OUTDIR)" $(DEF_FILE) $(LINK32_OBJS)
    $(LINK32) @<<
  $(LINK32_FLAGS) $(LINK32_OBJS)
<<

#
# 64-bit support
#
!ELSEIF  "$(CFG)" == "Win64 Release"

OUTDIR=.\x64\Release
INTDIR=.\x64\Release
# Begin Custom Macros
OutDir=.\x64\Release
# End Custom Macros

ALL : "$(OUTDIR)\$(FILENAME).exe"


CLEAN :
        -@erase "$(INTDIR)\*.obj"
        -@erase "$(OUTDIR)\$(FILENAME).exe"
        -@erase "$(OUTDIR)\*.idb"

"$(OUTDIR)" :
    if not exist "$(OUTDIR)/$(NULL)" mkdir "$(OUTDIR)"

CPP=cl.exe
CPP_PROJ=/nologo /W3 /O2 /I "$(NIIVIPATH)\Include" /I "$(VXIPNPPATH)\WinNT\include" /I ..\niSwitchErrorHandler /D "WIN32" /D "NDEBUG" /D "_CONSOLE" /D "_MBCS" /Fo"$(INTDIR)\\" /Fd"$(INTDIR)\\" /FD /c 

.c{$(INTDIR)}.obj::
   $(CPP) @<<
   $(CPP_PROJ) $<
<<

RSC=rc.exe
BSC32=bscmake.exe
BSC32_FLAGS=/nologo /o"$(OUTDIR)\$(FILENAME).bsc"
BSC32_SBRS= \


LINK32=link.exe
LINK32_FLAGS=niswitch.lib  /nologo /subsystem:console /incremental:no /pdb:"$(OUTDIR)\$(FILENAME).pdb" /machine:X64 /out:"$(OUTDIR)\$(FILENAME).exe" /libpath:"$(NIIVIPATH)\Lib_x64\msc"
LINK32_OBJS= \
        "$(INTDIR)\$(FILENAME).obj" \
	"$(INTDIR)\niSwitchErrorHandler.obj" \


"$(OUTDIR)\$(FILENAME).exe" : "$(OUTDIR)" $(DEF_FILE) $(LINK32_OBJS)
    $(LINK32) @<<
  $(LINK32_FLAGS) $(LINK32_OBJS)
<<

!ELSEIF  "$(CFG)" == "Win64 Debug"

OUTDIR=.\x64\Debug
INTDIR=.\x64\Debug
# Begin Custom Macros
OutDir=.\x64\Debug
# End Custom Macros

ALL : "$(OUTDIR)\$(FILENAME).exe" "$(OUTDIR)\$(FILENAME).bsc"


CLEAN :
        -@erase "$(INTDIR)\*.obj"
        -@erase "$(INTDIR)\*.sbr"
        -@erase "$(OUTDIR)\$(FILENAME).bsc"
        -@erase "$(OUTDIR)\$(FILENAME).exe"
        -@erase "$(OUTDIR)\$(FILENAME).ilk"
        -@erase "$(OUTDIR)\*.idb"
        -@erase "$(OUTDIR)\*.pdb"

"$(OUTDIR)" :
    if not exist "$(OUTDIR)/$(NULL)" mkdir "$(OUTDIR)"

CPP=cl.exe
CPP_PROJ=/nologo /W3 /Gm /Zi /Od /I "$(NIIVIPATH)\Include" /I "$(VXIPNPPATH)\WinNT\include" /I ..\niSwitchErrorHandler /D "WIN32" /D "_DEBUG" /D "_CONSOLE" /D "_MBCS" /FR"$(INTDIR)\\" /Fo"$(INTDIR)\\" /Fd"$(INTDIR)\\" /FD /c 

.c{$(INTDIR)}.obj::
   $(CPP) @<<
   $(CPP_PROJ) $<
<<

RSC=rc.exe
BSC32=bscmake.exe
BSC32_FLAGS=/nologo /o"$(OUTDIR)\$(FILENAME).bsc"
BSC32_SBRS= \
        "$(INTDIR)\$(FILENAME).sbr"

"$(OUTDIR)\$(FILENAME).bsc" : "$(OUTDIR)" $(BSC32_SBRS)
    $(BSC32) @<<
  $(BSC32_FLAGS) $(BSC32_SBRS)
<<

LINK32=link.exe
LINK32_FLAGS=niswitch.lib  /nologo /subsystem:console /incremental:yes /pdb:"$(OUTDIR)\$(FILENAME).pdb" /debug /machine:X64 /out:"$(OUTDIR)\$(FILENAME).exe" /libpath:"$(NIIVIPATH)\Lib_x64\msc"
LINK32_OBJS= \
        "$(INTDIR)\$(FILENAME).obj" \
	"$(INTDIR)\niSwitchErrorHandler.obj" \


"$(OUTDIR)\$(FILENAME).exe" : "$(OUTDIR)" $(DEF_FILE) $(LINK32_OBJS)
    $(LINK32) @<<
  $(LINK32_FLAGS) $(LINK32_OBJS)
<<
!ENDIF

!IF "$(CFG)" == "Win32 Release" || "$(CFG)" == "Win32 Debug" || "$(CFG)" == "Win64 Release" || "$(CFG)" == "Win64 Debug"
SOURCE=".\$(FILENAME).c"

!IF  "$(CFG)" == "Win32 Release"


"$(INTDIR)\$(FILENAME).obj" : $(SOURCE) "$(INTDIR)"


!ELSEIF  "$(CFG)" == "Win32 Debug"


"$(INTDIR)\$(FILENAME).obj"     "$(INTDIR)\$(FILENAME).sbr" : $(SOURCE) "$(INTDIR)"


!ENDIF


SOURCE="..\niSwitchErrorHandler\niSwitchErrorHandler.c"

"$(INTDIR)\niSwitchErrorHandler.obj" : $(SOURCE) "$(INTDIR)"
        $(CPP) $(CPP_PROJ) $(SOURCE)


!ENDIF

