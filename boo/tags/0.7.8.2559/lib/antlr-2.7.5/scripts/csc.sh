#!/bin/sh
##xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx##
## This file is part of ANTLR. See LICENSE.txt for licence  ##
## details. Written by W. Haefelinger.                      ##
##                                                          ##
##       Copyright (C) Wolfgang Haefelinger, 2004           ##
##                                                          ##
##xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx##
test -z "${verbose}" && { 
  verbose=0
}
## This script will be called to compile a list of C#  files
## on all UNIX/Cygwin platforms.

##xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx
## pre-set some variables required or useful  to  compile C# 
## source files:

## srcdir shall contain absolute path to package directory.
srcdir="/home/rodrigob/java/antlr-2.7.5/scripts/.."

## objdir shall contain absolute path to this build directory.
objdir="/home/rodrigob/java/antlr-2.7.5"

## variable javac contains the canonical java compiler name.
## At point of writing known names are csc.
csc="@csc@"

antlr_net="/home/rodrigob/java/antlr-2.7.5/lib/antlr.runtime.dll"

## When on cygwin we translage paths into DOS notation as 
## csc appears not to understand mixed notation.

case linux-gnu in
  cygwin)
    test -n "$1" && {
      ARGV="`cygpath -w $*`"
    }
    test -n "${srcdir}" && {
      srcdir="`cygpath -w ${srcdir}`"
    }
    test -n "${objdir}" && {
      objdir="`cygpath -w ${objdir}`"
    }
    test -n "${antlr_net}" && {
      antlr_net="`cygpath -w ${antlr_net}`"
    }
    ;;
  *)
    ARGV="$*"
    ;;
esac

## The very first argument shall alway tell us what we are
## going to build. We support here either a DLL, or an EXE.
set x ${ARGV}
case "$2" in
  *.dll|*.DLL)
    TARGET="$2"; shift; shift
    ;;
  *.exe|*.EXE)
    TARGET="$2"; shift; shift
    ;;
  *.cs|*.CS)
    TARGET="main.exe" ; shift
    ;;
  *)
    cat <<EOF
%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%
                     *** ERROR ***
The very first argument to csc.sh must be either a DLL or
EXE path name. However, I got

 $2

and I have no idear what you want me to build. Please check
your arguments again and change appropriatly. Thank you.
%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%
EOF
    exit 1
    ;;
esac
ARGV="$*"

## Command CHARPC is precomputed but user may override.
test -z "${CSHARPC}" && {
  CSHARPC="/usr/local/mono/bin/mcs"
}

## Compute the flags for well known compilers. Note that a user
## may override this settings by providing CSHARPCFLAGS - see be-
## low.
case "${csharpc}" in
  *)
    case "${TARGET}" in
      *.exe|*.EXE)
        csharpcflags="/nologo /t:exe /out:${TARGET} /r:${antlr_net}"
        ;;
      *.dll|*.DLL)
        csharpcflags="/nologo /t:library /out:${TARGET} /r:System.Windows.Forms.dll /r:System.Drawing.dll /r:System.dll"
        ;;
    esac
    ;;
esac


## If specific flags have been configured then they overrule
## our precomputed flags. Still a user can override by using
## environment variable $CSHARPCFLAGS - see below.
test -n "" && {
  set x   ; shift
  case $1 in
    +)
      shift
      CSHARPCFLAGS="${csharpcflags} $*"
      ;;
    -)
      shift
      csharpcflags="$* ${csharpcflags}"
      ;;
    =)
      shift
      csharpcflags="$*"
      ;;
    *)
      if test -z "$1" ; then
        csharpcflags="${csharpcflags}"
      else
        csharpcflags="$*"
      fi
      ;;
  esac
}

## Regardless what has been configured, a user should always
## be able to  override  without  the need to reconfigure or
## change this file. Therefore we check variable $CSHARPCFLAGS.
## In almost all cases the precomputed flags are just ok but
## some  additional  flags are needed. To support this in an
## easy way, we check for the very first value. If this val-
## ue is 
## '+'  -> append content of CSHARPCFLAGS to precomputed flags
## '-'  -> prepend content    -*-
## '='  -> do not use precomputed flags
## If none of these characters are given, the behaviour will
## be the same as if "=" would have been given.

set x ${CSHARPCFLAGS}  ; shift
case $1 in
  +)
    shift
    CSHARPCFLAGS="${csharpcflags} $*"
    ;;
  -)
    shift
    CSHARPCFLAGS="$* ${csharpcflags}"
    ;;
  =)
    shift
    CSHARPCFLAGS="$*"
    ;;
  *)
    if test -z "$1" ; then
      CSHARPCFLAGS="${csharpcflags}"
    else
      CSHARPCFLAGS="$*"
    fi
    ;;
esac

## Any special treatment goes here ..
case "${csharpc}" in
  csc)
    ;;
  *)
    ;;
esac

## go ahead ..
cmd="${CSHARPC} ${CSHARPCFLAGS} ${ARGV}"
##xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx##
##        standard template to execute a command          ##
##xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx##
case "${verbose}" in
  0|no|nein|non)
    set x `echo $ARGV | wc`
    case $3 in
      1) files="file" ;; 
      *) files="files";;
    esac
    echo "*** compiling $3 C# ${files}"
    ;;
  *)
    echo $cmd
    ;;
esac

$cmd || {
  rc=$?
  cat <<EOF

xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx
                      >> E R R O R <<
============================================================

$cmd

============================================================
Got an error while trying to execute  command  above.  Error
messages (if any) must have shown before. The exit code was:
exit($rc)
xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx
EOF
  exit $rc
}
exit 0
