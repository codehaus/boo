#!/bin/sh
##xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx##
## This file is part of ANTLR. See LICENSE.txt for licence  ##
## details. Written by W. Haefelinger.                      ##
##                                                          ##
##       Copyright (C) Wolfgang Haefelinger, 2004           ##
##                                                          ##
##xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx##
## This script shall wrap/hide how we are going to run a C/C++
## preprocessor within the ANTLR (www.antlr.org) project.
test -z "${verbose}" && {
  verbose=0
}

## check whether we have something to do ..
if test -z "$1" ; then
  exit 0
fi

ARCHFLAGS=
INCLUDE="-I /home/rodrigob/java/antlr-2.7.5/scripts/../lib/cpp"
DEBUG=
EXTRA_CFLAGS=

C_CMD="cc -g -O2 ${ARCHFLAGS} ${INCLUDE} ${DEBUG} ${EXTRA_CFLAGS} -c"

while test $# -gt 0 ; do
  x="$1" ; shift
  echo "compiling (C) $x .."
  c_cmd="$C_CMD $x"
  $c_cmd || {
    echo ""
    echo "error caught on .."
    echo ">>> $c_cmd"
    echo ""
    exit 1
  }
done
