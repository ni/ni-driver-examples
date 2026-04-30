C/C++ 4882QUERY Sample Application

This directory contains the following:

     README.TXT            -  This readme file
     4882QUERY.C           -  Windows C NI-488.2 sample application


Description
-----------

This C sample application is a Windows console application. It
illustrates how to use the NI-488.2 API. A Windows console application
is a Windows application which uses text-based input and output, not a
graphical interface.  This allows you to quickly create a Windows
application by using simple input and output functions like printf 
and scanf.


Checking Status with Global Variables
-------------------------------------

Each NI-488.2 call updates three global variables to reflect the
of the device or board in use.  The three global variables are the
status (Ibsta()), the error (Iberr()), and the count (Ibcnt()). Your
application should check for the errors after each NI-488.2 call by
looking at Ibsta(). The ERR bit in Ibsta() indicates if the call
succeeded or not. If the ERR bit is set, Iberr() contains an error
code. For a complete description of Ibsta() bits and Iberr() error
codes, see the online help.  If you are writing a multithreaded
application, please refer to the online help on writing multithreaded
applications.


Compiling, Linking, and Running the Sample Application from the
Command Line
---------------------------------------------------------------

From the standard DOS shell command line, you can compile and link
the sample application, 4882query.c. The "NIEXTCCOMPILERSUPP"
environment variable is provided as an alias to the location of C
language support files.

  Microsoft Visual C++ (32-bit)
  -----------------------------

  With Microsoft Visual C++ (Version 6.0 or higher), this is done by
  typing in using the 32-bit environment:

    Build the application to use DLL-specific version of C Runtime Library:
      cl /I"%NIEXTCCOMPILERSUPP%\include" 4882query.c "%NIEXTCCOMPILERSUPP%\lib32\msvc\ni4882.obj" /MD

    Build the application to use static version of C Runtime Library:
      cl /I"%NIEXTCCOMPILERSUPP%\include" 4882query.c "%NIEXTCCOMPILERSUPP%\lib32\msvc\ni4882scrt.obj" /MT

  Microsoft Visual C++ (64-bit)
  -----------------------------

  With Microsoft Visual C++ (Version 8.0 or higher), this is done by
  typing in using the 64-bit environment:

    Build the application to use DLL-specific version of C Runtime Library:
      cl /I"%NIEXTCCOMPILERSUPP%\include" 4882query.c "%NIEXTCCOMPILERSUPP%\lib64\msvc\ni4882.obj" /MD

    Build the application to use static version of C Runtime Library:
      cl /I"%NIEXTCCOMPILERSUPP%\include" 4882query.c "%NIEXTCCOMPILERSUPP%\lib64\msvc\ni4882scrt.obj" /MT

To run the application from the DOS shell, just type in the executable
name at the prompt. To run it from within Windows choose the RUN...
option from the START menu. Enter the name of the compiled application
in the dialog box that pops up.


More Information
----------------

Refer to the NI-488.2 online help for more information on application
development.



Copyright National Instruments Corporation.
All Rights Reserved.
