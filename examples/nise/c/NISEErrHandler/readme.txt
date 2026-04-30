This library can be used to ease error handling when calling Switch Executive
 from C or C++ programs.  While it could be used as is, you may want to modify
 it to return error information in a more convenient method (such as exceptions,
 message boxes, etc.)

It consists of a helper function which checks for errors.  If it finds one, it
 retrieves all description information from Switch Executive and prints an
 appropriate message to the stdout console.

