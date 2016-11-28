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
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Security.Permissions;
using System.Windows.Forms;

namespace Epwing2Anki
{
  public static class PrefDefaults
  {
  }

  public static class ConstantSettings
  {
    private static string saveFile = "settings.e2a";
    private static string helpFile = Path.Combine("Help", "Epwing2Anki_Help.html");
    private static string utilsName = "Utils";
    private static string sampleDir = UtilsCommon.getAppDir(true) + "Sample" + Path.DirectorySeparatorChar;
    private static string sampleWordList = sampleDir + "sample_word_list.txt";
    private static string templatesDir = UtilsCommon.getAppDir(true) + "Templates" + Path.DirectorySeparatorChar;
    private static string utilsDir = UtilsCommon.getAppDir(true) + "Utils" + Path.DirectorySeparatorChar;
    private static string dicDir = UtilsCommon.getAppDir(true) + "Dic" + Path.DirectorySeparatorChar;
    private static string edictDb = dicDir + "edict.sqlite";
    private static string tatoebaDb = dicDir + "tatoeba.txt";
    private static string logDir = UtilsCommon.getAppDir(true) + "Logs" + Path.DirectorySeparatorChar;
    private static int maxLogFiles = 10;

    private static bool enableLogging = true;

    public static string SaveFile
    {
      get { return saveFile; }
    }

    public static string HelpFile
    {
      get { return helpFile; }
    }

    public static string UtilsName
    {
      get { return utilsName; }
    }

    public static string SampleDir
    {
      get { return sampleDir; }
    }

    public static string SampleWordList
    {
      get { return sampleWordList; }
    }

    public static string TemplatesDir
    {
      get { return templatesDir; }
    }

    public static string UtilsDir
    {
      get { return utilsDir; }
    }

    public static string DicDir
    {
      get { return dicDir; }
    }

    public static string EdictDb
    {
      get { return edictDb; }
    }

    public static string TatoebaDb
    {
      get { return tatoebaDb; }
    }

    public static string LogDir
    {
      get { return logDir; }
    }

    public static int MaxLogFiles
    {
      get { return maxLogFiles; }
    }

    public static bool EnableLogging
    {
      get { return enableLogging; }
      set { enableLogging = value; }
    }
  }


  /// <summary>
  /// Represents all settings.
  /// </summary>
  [Serializable]
  public class Settings
  {
    private string version = UtilsAssembly.Version;
    private DicList dicList = new DicList();
    private DicList exampleList = new DicList();
    private CardLayout availableLayout = new CardLayout();
    private CardLayout cardLayout = new CardLayout();
    private bool appendLineFromWordList = false;
    private bool expandExamples = false;
    private string inFile = "";
    private int inWordCol = 1;
    private string outFile = "";
    private string tags = "";
    private bool autoDisambiguate = false;
    private bool autoChooseExamples = true;
    private int maxAutoChooseExamples = 3;
    FineTune fineTuneOptions = new FineTune();
    [NonSerialized]
    private List<WordListItem> wordList = new List<WordListItem>();

    /// <summary>
    /// The version of Epwing2Anki that created these settings.
    /// </summary>
    public string Version
    {
      get { return this.version; }
      set { this.version = value; }
    }

    /// <summary>
    /// List of dictionaries in priority order.
    /// </summary>
    public DicList DicList
    {
      get { return this.dicList; }
      set { this.dicList = value; }
    }

    /// <summary>
    /// List of example sources in priority order.
    /// </summary>
    public DicList ExampleDicList
    {
      get { return this.exampleList; }
      set { this.exampleList = value; }
    }

    /// <summary>
    /// Available card layout fields.
    /// </summary>
    public CardLayout AvailableLayout
    {
      get { return this.availableLayout; }
      set { this.availableLayout = value; }
    }

    /// <summary>
    /// Card layout.
    /// </summary>
    public CardLayout CardLayout
    {
      get { return this.cardLayout; }
      set { this.cardLayout = value; }
    }

    /// <summary>
    /// Append lines from word list to corresponding lines of the generated import file.
    /// </summary>
    public bool AppendLineFromWordList
    {
      get { return this.appendLineFromWordList; }
      set { this.appendLineFromWordList = value; }
    }

    /// <summary>
    /// Make a separate card for each example sentence instead of placing all
    /// examples into a single field.
    /// </summary>
    public bool ExpandExamples
    {
      get { return this.expandExamples; }
      set { this.expandExamples = value; }
    }

    /// <summary>
    /// The input file
    /// </summary>
    public string InFile
    {
      get { return this.inFile; }
      set { this.inFile = value; }
    }

    /// <summary>
    /// The column of the expression in the input file.
    /// </summary>
    public int WordListCol
    {
      get { return this.inWordCol; }
      set { this.inWordCol = value; }
    }

    /// <summary>
    /// The Anki import file that is generated.
    /// </summary>
    public string OutFile
    {
      get { return this.outFile; }
      set { this.outFile = value; }
    }

    /// <summary>
    /// Tags to add to each card.
    /// </summary>
    public string Tags
    {
      get { return this.tags; }
      set { this.tags = value; }
    }

    /// <summary>
    /// Automatically choose the first entry if multiple entries exists for a word.
    /// </summary>
    public bool AutoDisambiguate
    {
      get { return this.autoDisambiguate; }
      set { this.autoDisambiguate = value; }
    }

    /// <summary>
    /// Automatically choose example sentences.
    /// </summary>
    public bool AutoChooseExamples
    {
      get { return this.autoChooseExamples; }
      set { this.autoChooseExamples = value; }
    }

    /// <summary>
    /// Maximum number of example sentences to automatically choose.
    /// </summary>
    public int MaxAutoChooseExamples
    {
      get { return this.maxAutoChooseExamples; }
      set { this.maxAutoChooseExamples = value; }
    }

    /// <summary>
    /// Options to fine-tune things related to dictionaries.
    /// </summary>
    public FineTune FineTuneOptions
    {
      get { return this.fineTuneOptions; }
      set { this.fineTuneOptions = value; }
    }


    /// <summary>
    /// The word list.
    /// </summary>
    public List<WordListItem> WordList
    {
      get { return this.wordList; }
      set { this.wordList = value; }
    }


    /// <summary>
    /// Constructor.
    /// </summary>
    public Settings()
    {
      Dic defaultDic = new DicEdict();
      this.dicList.Dics.Add(defaultDic);
      this.exampleList.Dics.Add(defaultDic);

      this.inFile = ConstantSettings.SampleWordList;

      this.outFile = Path.Combine(Environment.GetFolderPath(
        Environment.SpecialFolder.MyDocuments), "anki_import_file.tsv");

      this.availableLayout.Fields.Add(new CardLayoutField(FieldEnum.ExpressionRuby));
      this.availableLayout.Fields.Add(new CardLayoutField(FieldEnum.DefJE));
      this.availableLayout.Fields.Add(new CardLayoutField(FieldEnum.DefJJ));
      this.availableLayout.Fields.Add(new CardLayoutField(FieldEnum.ExamplesNoTranslation));
      this.availableLayout.Fields.Add(new CardLayoutField(FieldEnum.ExamplesTranslationOnly));
      this.availableLayout.Fields.Add(new CardLayoutField(FieldEnum.Tags));

      this.cardLayout.Fields.Add(new CardLayoutField(FieldEnum.Expression));
      this.cardLayout.Fields.Add(new CardLayoutField(FieldEnum.Reading));
      this.cardLayout.Fields.Add(new CardLayoutField(FieldEnum.Def));
      this.cardLayout.Fields.Add(new CardLayoutField(FieldEnum.Examples));
    }

    /// <summary>
    /// Save the settings to file.
    /// </summary>
    public void save()
    {
      try
      {
        FileStream fileStream = new FileStream(ConstantSettings.SaveFile, FileMode.Create);
        BinaryFormatter binaryFormatter = new BinaryFormatter();
        binaryFormatter.Serialize(fileStream, this);
        fileStream.Close();
      }
      catch
      {
        UtilsMsg.showErrMsg("Unable to save your settings.\r\n\r\n"
          + "Perhaps the Epwing2Anki directory does not have write permissions.");
      }
    }


    /// <summary>
    /// Load a settings file.
    /// </summary>
    public static Settings load()
    {
      Settings settings = null;

      try
      {
        if (File.Exists(ConstantSettings.SaveFile))
        {
          Stream fileStream = new FileStream(ConstantSettings.SaveFile, FileMode.Open, FileAccess.Read);
          BinaryFormatter binaryFormatter = new BinaryFormatter();
          settings = (Settings)(binaryFormatter.Deserialize(fileStream));
          fileStream.Close();

          settings.wordList = new List<WordListItem>();

          // If loaded settings file is from an old version of Epwing2Anki,
          // replace it with default settings for this version.
          if (settings.Version != UtilsAssembly.Version)
          {
            settings = new Settings();
          }
        }
      }
      catch (Exception e1)
      {
        // Don't care
        Logger.Instance.error("setttings.load: " + e1);
      }

      return settings;
    }



  }





}
