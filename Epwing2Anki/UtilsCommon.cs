//  Copyright (C) 2012 Christopher Brochtrup
//
//  This file is part of Epwing2Anki.
//
//  Epwing2Anki is free software: you can redistribute it and/or modify
//  it under the terms of the GNU General Public License as published by
//  the Free Software Foundation, either version 3 of the License, or
//  (at your option) any later version.
//
//  Epwing2Anki is distributed in the hope that it will be useful,
//  but WITHOUT ANY WARRANTY; without even the implied warranty of
//  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//  GNU General Public License for more details.
//
//  You should have received a copy of the GNU General Public License
//  along with Epwing2Anki.  If not, see <http://www.gnu.org/licenses/>.
//
//////////////////////////////////////////////////////////////////////////////

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using System.Drawing;


namespace Epwing2Anki
{
  /// <summary>
  /// General utilies.
  /// </summary>
  public class UtilsCommon
  {
    /// <summary>
    /// Get the directory that the executable resides in.
    /// </summary>
    public static string getAppDir(bool addSlash)
    {
      string appDir = Application.ExecutablePath.Substring(
        0, Application.ExecutablePath.LastIndexOf(Path.DirectorySeparatorChar));

      if (addSlash)
      {
        appDir += Path.DirectorySeparatorChar;
      }

      return appDir;
    }


    /// <summary>
    /// Convert C# color to HTML color
    /// </summary>
    static public string toHtmlColor(Color color)
    {
      return String.Format("#{0:X2}{1:X2}{2:X2}", color.R, color.G, color.B);
    }


    /// <summary>
    /// Get the first line of a multi-lined text.
    /// </summary>
    static public string getFirstLine(string text)
    {
      string firstLine = "";

      if (text.TrimEnd().Contains('\n'))
      {
        firstLine = text.Substring(0, text.IndexOf('\n', 0));
      }
      else
      {
        firstLine = text;
      }

      return firstLine;
    }

    /// <summary>
    /// Call an exe and pass it the provided arguments. Blocking.
    /// </summary>
    static private bool callExe(string exe, string args, bool useShellExecute, bool createNoWindow)
    {
      Process process = new Process();
      bool status = false;

      try
      {
        process.StartInfo.FileName = exe;
        process.StartInfo.Arguments = args;
        process.StartInfo.UseShellExecute = useShellExecute;
        process.StartInfo.CreateNoWindow = createNoWindow;
        process.Start();

        process.WaitForExit(); // Blocking

        status = true;
      }
      catch (Exception e1)
      {
        try
        {
          process.Kill();
        }
        catch
        {
          // Don't care
          Logger.Instance.error("callExe: " + e1);
        }
      }

      return status;
    }


    /// <summary>
    /// Call an exe with the provided arguments.
    /// </summary>
    static public void startProcess(string relExePath, string fullExePath, string args,
      bool useShellExecute, bool createNoWindow)
    {
      Process process = new Process();
      bool status = true;

      // Try several different ways of calling ffmpeg because of the dreaded
      // "System.ComponentModel.Win32Exception: The system cannot find the drive specified" exception

      // Try relative path from subs2srs.exe
      status = callExe(relExePath, args, useShellExecute, createNoWindow);

      // Try absolute path to exe
      if (!status)
      {
        status = callExe(fullExePath, args, useShellExecute, createNoWindow);
      }

      // Try setting PATH to include the absolute path of exe
      if (!status)
      {
        string oldPath = Environment.GetEnvironmentVariable("Path");
        string dir = Path.GetDirectoryName(fullExePath);

        if (!oldPath.Contains(dir))
        {
          string newPath = oldPath + ";" + dir;
          Environment.SetEnvironmentVariable("Path", newPath);
        }

        status = callExe(Path.GetFileName(fullExePath), args, useShellExecute, createNoWindow);
      }
    }


    /// <summary>
    /// Call an exe with the provided arguments. Don't open a DOS window.
    /// </summary>
    static public void startProcess(string relExePath, string fullExePath, string args)
    {
      startProcess(relExePath, fullExePath, args, false, true);
    }




  }
}
