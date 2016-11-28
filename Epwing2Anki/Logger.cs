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
using System.IO;
using System.Linq;
using System.Text;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading;

namespace Epwing2Anki
{
  /// <summary>
  /// Singleton logger.
  /// </summary>
  public class Logger
  {
    private static Mutex logFileMutex = new Mutex();
    private StringBuilder builder = new StringBuilder(1000);
    private string logFile = "log.txt";
    private bool initalized = false;
    private static readonly Logger instance = new Logger();


    // Singleton instance
    public static Logger Instance
    {
      get
      {
        return instance;
      }
    }


    private Logger()
    {
      if (!ConstantSettings.EnableLogging)
      {
        return;
      }

      DateTime now = DateTime.Now;

      logFile = String.Format("{0}log-{1:0000}{2:00}{3:00}-{4:00}{5:00}{6:00}.txt",
        ConstantSettings.LogDir,
        now.Year,
        now.Month,
        now.Day,
        now.Hour,
        now.Minute,
        now.Second);

      try
      {
        // Write blank file
        TextWriter writer = new StreamWriter(logFile, false, Encoding.UTF8);
        writer.Close();
        initalized = true;

        
        info(String.Format("{0} version: {1}", UtilsAssembly.Title, UtilsAssembly.Version));

        // Windows version. 
        // Example: Microsoft Windows NT 6.1.7601 Service Pack 1
        // ------------------------------------------------------------------------------------------------------------------------------------------+
        // |           |   Windows    |   Windows    |   Windows    |Windows NT| Windows | Windows | Windows | Windows | Windows | Windows | Windows |
        // |           |     95       |      98      |     Me       |    4.0   |  2000   |   XP    |  2003   |  Vista  |  2008   |    7    | 2008 R2 |
        // +-----------------------------------------------------------------------------------------------------------------------------------------+
        // |PlatformID | Win32Windows | Win32Windows | Win32Windows | Win32NT  | Win32NT | Win32NT | Win32NT | Win32NT | Win32NT | Win32NT | Win32NT |
        // +-----------------------------------------------------------------------------------------------------------------------------------------+
        // |Major      |              |              |              |          |         |         |         |         |         |         |         |
        // | version   |      4       |      4       |      4       |    4     |    5    |    5    |    5    |    6    |    6    |    6    |    6    |
        // +-----------------------------------------------------------------------------------------------------------------------------------------+
        // |Minor      |              |              |              |          |         |         |         |         |         |         |         |
        // | version   |      0       |     10       |     90       |    0     |    0    |    1    |    2    |    0    |    0    |    1    |    1    |
        // +-----------------------------------------------------------------------------------------------------------------------------------------+
        info(System.Environment.OSVersion.VersionString);

        // Delete all except the 10 newest logs in the log directory
        DirectoryInfo folder = new DirectoryInfo(ConstantSettings.LogDir);
        List<FileInfo> files = new List<FileInfo>(folder.GetFiles());

        if (files.Count >= ConstantSettings.MaxLogFiles)
        {
          files.Sort(compareFileInfo);

          for (int i = 0; i < files.Count - ConstantSettings.MaxLogFiles; i++)
          {
            if (files[i].FullName.Contains("log-"))
            {
              File.Delete(files[i].FullName);
            }
          }
        }
      }
      catch
      {
        // Don't care
      }
    }


    /// <summary>
    /// Compare FileInfo based on LastWriteTime.
    /// </summary>
    private static int compareFileInfo(FileInfo x, FileInfo y)
    {
      return x.LastWriteTime.CompareTo(y.LastWriteTime);
    }


    /// <summary>
    /// Creeate a timestamp for the log.
    /// </summary>
    private string createTimestamp()
    {
      DateTime now = DateTime.Now;

      string time = String.Format("{0:00}:{1:00}:{2:00}.{3:000}: ", 
        now.Hour, now.Minute, now.Second, now.Millisecond);

      return time;
    }


    /// <summary>
    /// Write the log information so far to file.
    /// </summary>
    public void flush()
    {
      if (!ConstantSettings.EnableLogging || !initalized)
      {
        return;
      }

      try
      {
        logFileMutex.WaitOne();

        TextWriter writer = new StreamWriter(logFile, true, Encoding.UTF8);
        writer.Write(builder.ToString());
        writer.Close();

        logFileMutex.ReleaseMutex();

        builder = new StringBuilder(1000);
      }
      catch
      {
        // Don't care
      }
    }


    /// <summary>
    /// Append line to the log.
    /// </summary>
    public void append(string text)
    {
      builder.Append(createTimestamp());
      builder.AppendLine(text);
    }

    /// <summary>
    /// Append informational line to the log.
    /// </summary>
    public void info(string text)
    {
      this.append("   " + text);
    }


    /// <summary>
    /// Append error line to the log.
    /// </summary>
    public void error(string text)
    {
      this.append("** " + text);
    }


    /// <summary>
    /// Append warning line to the log.
    /// </summary>
    public void warning(string text)
    {
      this.append("~~ " + text);
    }


    /// <summary>
    /// Add new section.
    /// </summary>
    public void startSection(string text)
    {
      string sectionText = ">> [ " + text + " ]";

      this.append(sectionText);
    }


    /// <summary>
    /// End section.
    /// </summary>
    public void endSection(string text)
    {
      string sectionText = "<< [ " + text + " ]";

      this.append(sectionText);
    }


    /// <summary>
    /// Print the name and value of a variable to the log.
    /// </summary>
    public void var<T>(T item) where T : class 
    {
      this.append(String.Format("   {0} ", item));
    }


    /// <summary>
    /// Dump the most important settings to the log.
    /// </summary>
    public void writeSettingsToLog(Settings settings)
    {
      if (!ConstantSettings.EnableLogging || !initalized)
      {
        return;
      }

      startSection("Settings");

      int count = 0;

      var(new { settings.AutoChooseExamples });
      var(new { settings.AutoDisambiguate });

      count = 0;

      foreach (CardLayoutField field in settings.AvailableLayout.Fields)
      {
        info(String.Format("AvailableLayout {0}:", count));
        
        if (field.DicName != "")
        {
          var(new { field.DicName });
        }

        var(new { field.Type });
        count++;
      }

      var(new { settings.AppendLineFromWordList });

      count = 0;

      foreach (CardLayoutField field in settings.CardLayout.Fields)
      {
        info(String.Format("CardLayout {0}:", count));

        if (field.DicName != "")
        {
          var(new { field.DicName });
        }

        var(new { field.Type });
        count++;
      }

      count = 0;

      foreach(Dic dic in settings.DicList.Dics)
      {
        info(String.Format("DicList {0}:", count));
        var(new { dic.DicType });
        var(new { dic.Enabled });
        var(new { dic.ExamplesEnabled });
        var(new { dic.ExamplesName });
        var(new { dic.ExamplesNotes });
        var(new { dic.ExamplesType });
        var(new { dic.Name });
        var(new { dic.NameEng });
        var(new { dic.SourceType });
        count++;
      }

      count = 0;

      foreach (Dic dic in settings.ExampleDicList.Dics)
      {
        info(String.Format("ExampleDicList {0}:", count));
        var(new { dic.DicType });
        var(new { dic.Enabled });
        var(new { dic.ExamplesEnabled });
        var(new { dic.ExamplesName });
        var(new { dic.ExamplesNotes });
        var(new { dic.ExamplesType });
        var(new { dic.Name });
        var(new { dic.NameEng });
        var(new { dic.SourceType });
        count++;
      }

      var(new { settings.ExpandExamples });

      info(String.Format("Fine-tune options:"));
      var(new { settings.FineTuneOptions.AddPlaceholders });
      var(new { settings.FineTuneOptions.AddRubyToExamples });
      var(new { settings.FineTuneOptions.AppendSourceDicToExamples });
      var(new { settings.FineTuneOptions.CompactDefs });
      var(new { settings.FineTuneOptions.EdictNoP });
      var(new { settings.FineTuneOptions.EdictNoWordIndicators });
      var(new { settings.FineTuneOptions.ExamplePrependText });
      var(new { settings.FineTuneOptions.JeNoAlphaFallback });
      var(new { settings.FineTuneOptions.JjKeepExamplesInDef });
      var(new { settings.FineTuneOptions.JjRemoveSpecialReadingChars });
      var(new { settings.FineTuneOptions.JjFillInExampleBlanksWithWord });
      var(new { settings.FineTuneOptions.PrependSourceDicToDef });

      var(new { settings.InFile });
      var(new { settings.MaxAutoChooseExamples });
      var(new { settings.OutFile });
      var(new { settings.Tags });
      var(new { settings.Version });

      info("WordList words:");

      foreach (WordListItem word in settings.WordList)
      {
        var(new { word.Word });
      }

      info("WordList raw:");

      foreach (WordListItem word in settings.WordList)
      {
        var(new { word.RawLine });
      }

      var(new { settings.WordListCol });

      endSection("Settings");


      startSection("ConstantSettings");

      var(new { ConstantSettings.DicDir });
      var(new { ConstantSettings.EdictDb });
      var(new { ConstantSettings.EnableLogging });
      var(new { ConstantSettings.HelpFile });
      var(new { ConstantSettings.LogDir });
      var(new { ConstantSettings.MaxLogFiles });
      var(new { ConstantSettings.SaveFile });
      var(new { ConstantSettings.TatoebaDb });
      var(new { ConstantSettings.TemplatesDir });
      var(new { ConstantSettings.UtilsDir });
      var(new { ConstantSettings.UtilsName });

      endSection("ConstantSettings");

      flush();
    }


    /// <summary>
    /// Write an entire file to the log.
    /// </summary>
    public void writeFileToLog(string file, Encoding encoding)
    {
      if (!ConstantSettings.EnableLogging || !initalized)
      {
        return;
      }

      try
      {
        startSection("File: " + file);
        TextReader reader = new StreamReader(file, encoding);
        string text = reader.ReadToEnd();
        info(text);
        reader.Close();
        endSection("File: " + file);
        flush();
      }
      catch
      {
        // Don't care
      }
    }





  }
}
