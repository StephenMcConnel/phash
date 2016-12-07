﻿/*
* dirent.c
* This file has no copyright assigned and is placed in the Public Domain.
* This file is a part of the mingw-runtime package.
* No warranty is given; refer to the file DISCLAIMER within the package.
*
* Derived from DIRLIB.C by Matt J. Weinstein
* This note appears in the DIRLIB.H
* DIRLIB.H by M. J. Weinstein   Released to public domain 1-Jan-89
*
* Updated by Jeremy Bettis <jeremy@hksys.com>
* Significantly revised and rewinddir, seekdir and telldir added by Colin
* Peters <colin@fu.is.saga-u.ac.jp>
*	
*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shipwreck.Phash
{

    // dirent.h

    //    #ifndef _DIRENT_H_
    //#define _DIRENT_H_


    //#ifdef _UNICODE
    //#define _tdirent	_wdirent
    //#define _TDIR 		_WDIR
    //#define _topendir	_wopendir
    //#define _tclosedir	_wclosedir
    //#define _treaddir	_wreaddir
    //#define _trewinddir	_wrewinddir
    //#define _ttelldir	_wtelldir
    //#define _tseekdir	_wseekdir
    //#else
    //#define _tdirent	dirent
    //#define _TDIR 		DIR
    //#define _topendir	opendir
    //#define _tclosedir	closedir
    //#define _treaddir	readdir
    //#define _trewinddir	rewinddir
    //#define _ttelldir	telldir
    //#define _tseekdir	seekdir
    //#endif

    //#define SUFFIX	_T("*")
    //#define	SLASH	_T("\\")




    //#include <tchar.h>
    //#include <stdio.h>
    //#include <io.h>

    //#ifdef __cplusplus
    //extern "C" {
    //#endif


    //struct dirent
    //{
    //    long d_ino;     /* Always zero. */
    //    unsigned short d_reclen;    /* Always zero. */
    //    unsigned short d_namlen;    /* Length of name in d_name. */
    //    char d_name[FILENAME_MAX]; /* File name. */
    //};

    ///*
    // * This is an internal data structure. Good programmers will not use it
    // * except as an argument to one of the functions below.
    // * dd_stat field is now int (was short in older versions).
    // */
    //__declspec(dllexport)
    //typedef struct
    //{
    //    /* disk transfer area for this dir */
    //    struct _finddata_t  dd_dta;

    //	/* dirent struct to return from dir (NOTE: this makes this thread
    //	 * safe as long as only one thread uses a particular DIR struct at
    //	 * a time) */
    //	struct dirent       dd_dir;

    //	/* _findnext handle */
    //	long dd_handle;

    ///*
    //     * Status of search:
    // *   0 = not started yet (next entry to read is first entry)
    // *  -1 = off the end
    // *   positive = 0 based index of next entry
    // */
    //int dd_stat;

    ///* given path for dir with search pattern (struct is extended) */
    //char dd_name[1];
    //} DIR;

    //__declspec(dllexport) DIR* opendir(const char*);
    //__declspec(dllexport) struct dirent* readdir(DIR*);
    //__declspec(dllexport) int __cdecl closedir(DIR*);
    //__declspec(dllexport) void rewinddir(DIR*);
    //__declspec(dllexport) long telldir(DIR*);
    //__declspec(dllexport) void seekdir(DIR*, long);


    ///* wide char versions */
    //struct _wdirent
    //{
    //    long d_ino;     /* Always zero. */
    //    unsigned short d_reclen;    /* Always zero. */
    //    unsigned short d_namlen;    /* Length of name in d_name. */
    //    wchar_t d_name[FILENAME_MAX]; /* File name. */
    //};

    ///*
    // * This is an internal data structure. Good programmers will not use it
    // * except as an argument to one of the functions below.
    // */
    //__declspec(dllexport)
    //typedef struct
    //{
    //    /* disk transfer area for this dir */
    //    struct _wfinddata_t dd_dta;

    //	/* dirent struct to return from dir (NOTE: this makes this thread
    //	 * safe as long as only one thread uses a particular DIR struct at
    //	 * a time) */
    //	struct _wdirent     dd_dir;

    //	/* _findnext handle */
    //	long dd_handle;

    ///*
    //     * Status of search:
    // *   0 = not started yet (next entry to read is first entry)
    // *  -1 = off the end
    // *   positive = 0 based index of next entry
    // */
    //int dd_stat;

    ///* given path for dir with search pattern (struct is extended) */
    //wchar_t dd_name[1];
    //} _WDIR;



    //__declspec(dllexport) _WDIR* _wopendir(const wchar_t*);
    //__declspec(dllexport) struct _wdirent* _wreaddir(_WDIR*);
    //__declspec(dllexport) int _wclosedir(_WDIR*);
    //__declspec(dllexport) void _wrewinddir(_WDIR*);
    //__declspec(dllexport) long _wtelldir(_WDIR*);
    //__declspec(dllexport) void _wseekdir(_WDIR*, long);


    //#ifdef	__cplusplus
    //}
    //#endif

    //#endif	/* Not _DIRENT_H_ */


    // dirent.c



    //# include "dirent.h"
    //# include <string.h>
    //# include <errno.h>
    //# include <windows.h>

    ///*
    // * opendir
    // *
    // * Returns a pointer to a DIR structure appropriately filled in to begin
    // * searching a directory.
    // */
    //__declspec(dllexport)
    //_TDIR* _topendir(const _TCHAR* szPath)
    //{
    //    _TDIR* nd;
    //    unsigned int rc;
    //    _TCHAR szFullPath[MAX_PATH];

    //    errno = 0;

    //    if (!szPath)
    //    {
    //        errno = EFAULT;
    //        return (_TDIR*)0;
    //    }

    //    if (szPath[0] == _T('\0'))
    //    {
    //        errno = ENOTDIR;
    //        return (_TDIR*)0;
    //    }

    //    /* Attempt to determine if the given path really is a directory. */
    //    rc = GetFileAttributes(szPath);
    //    if (rc == (unsigned int)-1)
    //    {
    //        /* call GetLastError for more error info */
    //        errno = ENOENT;
    //        return (_TDIR*)0;
    //    }
    //    if (!(rc & FILE_ATTRIBUTE_DIRECTORY))
    //    {
    //        /* Error, entry exists but not a directory. */
    //        errno = ENOTDIR;
    //        return (_TDIR*)0;
    //    }

    //    /* Make an absolute pathname.  */
    //    _tfullpath(szFullPath, szPath, MAX_PATH);

    //    /* Allocate enough space to store DIR structure and the complete
    //     * directory path given. */
    //    nd = (_TDIR*)malloc(sizeof(_TDIR) + (_tcslen(szFullPath) + _tcslen(SLASH) +
    //               _tcslen(SUFFIX) + 1) * sizeof(_TCHAR));

    //    if (!nd)
    //    {
    //        /* Error, out of memory. */
    //        errno = ENOMEM;
    //        return (_TDIR*)0;
    //    }

    //    /* Create the search expression. */
    //    _tcscpy(nd->dd_name, szFullPath);

    //    /* Add on a slash if the path does not end with one. */
    //    if (nd->dd_name[0] != _T('\0') &&
    //        nd->dd_name[_tcslen(nd->dd_name) - 1] != _T('/') &&
    //        nd->dd_name[_tcslen(nd->dd_name) - 1] != _T('\\'))
    //    {
    //        _tcscat(nd->dd_name, SLASH);
    //    }

    //    /* Add on the search pattern */
    //    _tcscat(nd->dd_name, SUFFIX);

    //    /* Initialize handle to -1 so that a premature closedir doesn't try
    //     * to call _findclose on it. */
    //    nd->dd_handle = -1;

    //    /* Initialize the status. */
    //    nd->dd_stat = 0;

    //    /* Initialize the dirent structure. ino and reclen are invalid under
    //     * Win32, and name simply points at the appropriate part of the
    //     * findfirst_t structure. */
    //    nd->dd_dir.d_ino = 0;
    //    nd->dd_dir.d_reclen = 0;
    //    nd->dd_dir.d_namlen = 0;
    //    memset(nd->dd_dir.d_name, 0, FILENAME_MAX);

    //    return nd;
    //}


    ///*
    // * readdir
    // *
    // * Return a pointer to a dirent structure filled with the information on the
    // * next entry in the directory.
    // */
    //__declspec(dllexport)
    //struct _tdirent * _treaddir(_TDIR* dirp)
    //{
    //    errno = 0;

    //    /* Check for valid DIR struct. */
    //    if (!dirp)
    //    {
    //        errno = EFAULT;
    //        return (struct _tdirent *) 0;
    //    }

    //  if (dirp->dd_stat< 0)
    //    {
    //      /* We have already returned all files in the directory
    //       * (or the structure has an invalid dd_stat). */
    //      return (struct _tdirent *) 0;
    //    }
    //  else if (dirp->dd_stat == 0)
    //    {
    //      /* We haven't started the search yet. */
    //      /* Start the search */
    //      dirp->dd_handle = _tfindfirst(dirp->dd_name, &(dirp->dd_dta));

    //  	  if (dirp->dd_handle == -1)
    //	{
    //	  /* Whoops! Seems there are no files in that
    //	   * directory. */
    //	  dirp->dd_stat = -1;
    //	}
    //      else
    //	{
    //	  dirp->dd_stat = 1;
    //	}
    //    }
    //  else
    //    {
    //      /* Get the next search entry. */
    //      if (_tfindnext(dirp->dd_handle, &(dirp->dd_dta)))
    //	{
    //	  /* We are off the end or otherwise error.	
    //	     _findnext sets errno to ENOENT if no more file
    //	     Undo this. */
    //	  DWORD winerr = GetLastError();
    //	  if (winerr == ERROR_NO_MORE_FILES)
    //	    errno = 0;	

    //      _findclose(dirp->dd_handle);
    //dirp->dd_handle = -1;
    //	  dirp->dd_stat = -1;
    //	}
    //      else
    //	{
    //	  /* Update the status to indicate the correct
    //	   * number. */
    //	  dirp->dd_stat++;
    //	}
    //    }

    //  if (dirp->dd_stat > 0)
    //    {
    //      /* Successfully got an entry. Everything about the file is
    //       * already appropriately filled in except the length of the
    //       * file name. */
    //      dirp->dd_dir.d_namlen = _tcslen(dirp->dd_dta.name);
    //      _tcscpy(dirp->dd_dir.d_name, dirp->dd_dta.name);
    //      return &dirp->dd_dir;
    //    }

    //  return (struct _tdirent *) 0;
    //}


    ///*
    // * closedir
    // *
    // * Frees up resources allocated by opendir.
    // */
    //__declspec(dllexport)
    //int _tclosedir(_TDIR* dirp)
    //{
    //    int rc;

    //    errno = 0;
    //    rc = 0;

    //    if (!dirp)
    //    {
    //        errno = EFAULT;
    //        return -1;
    //    }

    //    if (dirp->dd_handle != -1)
    //    {
    //        rc = _findclose(dirp->dd_handle);
    //    }

    //    /* Delete the dir structure. */
    //    free(dirp);

    //    return rc;
    //}

    ///*
    // * rewinddir
    // *
    // * Return to the beginning of the directory "stream". We simply call findclose
    // * and then reset things like an opendir.
    // */
    //__declspec(dllexport)
    //void _trewinddir(_TDIR* dirp)
    //{
    //    errno = 0;

    //    if (!dirp)
    //    {
    //        errno = EFAULT;
    //        return;
    //    }

    //    if (dirp->dd_handle != -1)
    //    {
    //        _findclose(dirp->dd_handle);
    //    }

    //    dirp->dd_handle = -1;
    //    dirp->dd_stat = 0;
    //}

    ///*
    // * telldir
    // *
    // * Returns the "position" in the "directory stream" which can be used with
    // * seekdir to go back to an old entry. We simply return the value in stat.
    // */
    //__declspec(dllexport)
    //long _ttelldir(_TDIR* dirp)
    //{
    //    errno = 0;

    //    if (!dirp)
    //    {
    //        errno = EFAULT;
    //        return -1;
    //    }
    //    return dirp->dd_stat;
    //}

    ///*
    // * seekdir
    // *
    // * Seek to an entry previously returned by telldir. We rewind the directory
    // * and call readdir repeatedly until either dd_stat is the position number
    // * or -1 (off the end). This is not perfect, in that the directory may
    // * have changed while we weren't looking. But that is probably the case with
    // * any such system.
    // */
    //__declspec(dllexport)
    //void _tseekdir(_TDIR* dirp, long lPos)
    //{
    //    errno = 0;

    //    if (!dirp)
    //    {
    //        errno = EFAULT;
    //        return;
    //    }

    //    if (lPos < -1)
    //    {
    //        /* Seeking to an invalid position. */
    //        errno = EINVAL;
    //        return;
    //    }
    //    else if (lPos == -1)
    //    {
    //        /* Seek past end. */
    //        if (dirp->dd_handle != -1)
    //        {
    //            _findclose(dirp->dd_handle);
    //        }
    //        dirp->dd_handle = -1;
    //        dirp->dd_stat = -1;
    //    }
    //    else
    //    {
    //        /* Rewind and read forward to the appropriate index. */
    //        _trewinddir(dirp);

    //        while ((dirp->dd_stat < lPos) && _treaddir(dirp))
    //            ;
    //    }
    //}

}