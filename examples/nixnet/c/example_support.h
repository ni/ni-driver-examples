#ifndef EXAMPLE_SUPPORT_H
#define EXAMPLE_SUPPORT_H

#include <stdio.h>

#if defined _WIN64 || defined _WIN32

// Exclude rarely-used stuff from Windows headers
#ifndef WIN32_LEAN_AND_MEAN
#define WIN32_LEAN_AND_MEAN
#endif
#include <windows.h>    // Include file on Windows for Sleep
#include <conio.h>      // Include file on Windows for _kbhit, _getch

/* Ideally this would be inline, but the older compilers that NI-XNET supports don't have that keyword. */
static int PrintTimestamp(unsigned __int64 *Time)
{
   SYSTEMTIME l_STime;
   FILETIME l_LocalFTime;
   /* This Win32 function converts from UTC (international) time
   to the local time zone.  The NI-XNET card keeps time in UTC
   format (refer to the description of nxTimestamp_t in
   the NI-XNET reference for Read functions). */
   FileTimeToLocalFileTime((FILETIME *)Time, &l_LocalFTime);

   /* This Win32 function converts an absolute time (FILETIME)
   into SYSTEMTIME, a structure with fields for year, month, day,
   and so on. */
   FileTimeToSystemTime(&l_LocalFTime, &l_STime);

   return printf("%02d/%02d/%04d %02d:%02d:%02d.%03d", l_STime.wMonth, l_STime.wDay,
      l_STime.wYear, l_STime.wHour, l_STime.wMinute, l_STime.wSecond,
      l_STime.wMilliseconds);
}

#elif defined __linux__

#include <time.h>
#include <sys/ioctl.h> // Include file for ioctl
#include <termios.h>   // Include file for tcgetatt, tcsetattr
#include <unistd.h>    // Include file for read, usleep, STDIN_FILENO

struct keyboard_structure
{
   int descriptor;
   struct termios initial_settings;
   struct termios new_settings;
};

static inline struct keyboard_structure keyboard_constructor(int terminal_descriptor)
{
   // create a keyboard_structure, initialize the termios structures and return them;
   struct keyboard_structure key_struct = {0};
   key_struct.descriptor = terminal_descriptor;
   // gets the initial settings of the terminal so we can refer to them in the future
   tcgetattr(key_struct.descriptor, &key_struct.initial_settings);
   key_struct.new_settings = key_struct.initial_settings;
   key_struct.new_settings.c_lflag &= ~ICANON; // allows reads to be satisfied once any key is pressed, not just ENTER
   key_struct.new_settings.c_lflag &= ~ECHO; // makes input characters NOT be echoed
   key_struct.new_settings.c_lflag &= ~ISIG; // allows any character to be pressed even if its and interrupt or quit
   key_struct.new_settings.c_cc[VMIN] = 1; // sets the amount of bytes to be read to 1 byte
   key_struct.new_settings.c_cc[VTIME] = 0; // set return time to basically instant so reads happen very fast
   tcsetattr(key_struct.descriptor, TCSANOW, &key_struct.new_settings);
   return key_struct;
}

// sets the terminal back to its initial settings
static inline void keyboard_destructor(struct keyboard_structure *key_struct)
{
   tcsetattr(key_struct->descriptor, TCSANOW, &key_struct->initial_settings);
}

static inline int read_character(struct keyboard_structure *key_struct)
{
   char ch = 0;
   read(key_struct->descriptor, &ch, 1);
   return ch;
}

static inline int get_pending_bytes(struct keyboard_structure *key_struct)
{
   int pending_bytes = 0;
   ioctl(key_struct->descriptor, FIONREAD, &pending_bytes);
   return pending_bytes;
}

static inline void Sleep(int sleeptime)
{
   usleep(sleeptime * 1000);
}

static inline int _getch()
{
   struct keyboard_structure key_struct = keyboard_constructor(STDIN_FILENO);
   int key_pressed = read_character(&key_struct);
   keyboard_destructor(&key_struct);
   return key_pressed;
}

static inline int _kbhit()
{
   struct keyboard_structure key_struct = keyboard_constructor(STDIN_FILENO);
   int pending_bytes = get_pending_bytes(&key_struct);
   keyboard_destructor(&key_struct);
   return pending_bytes;
}

static inline int PrintTimestamp(unsigned long long *Time)
{
   // NI-XNET timestamps are a counter of 100 nanosecond ticks since Jan 1, 1601, but POSIX timestamps
   // are a counter of 1 second ticks since Jan 1, 1901, so we just need to convert units and subtract them.
   static const unsigned long long EPOCH_DIFFERENCE_IN_100NS_TICKS = 116444736000000000ull;
   const unsigned long long posix_timestamp_100ns_ticks = *Time - EPOCH_DIFFERENCE_IN_100NS_TICKS;
   const time_t posix_timestamp = posix_timestamp_100ns_ticks / 10000000;
   const int milliseconds = (int)(posix_timestamp_100ns_ticks / 10000) % 1000;
   struct tm local_time = {0};
   char buffer[32] = {0};

   localtime_r(&posix_timestamp, &local_time);
   if(strftime(buffer, sizeof(buffer), "%D %T", &local_time) == 0)
   {
      return printf("Error when formatting timestamp.");
   }

   return printf("%s.%03d", buffer, milliseconds);
}

#else
   #error The NI-XNET Examples do not support this Operating System.
#endif

#endif // EXAMPLE_SUPPORT_H

