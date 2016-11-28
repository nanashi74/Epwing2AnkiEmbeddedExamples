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
using System.Text.RegularExpressions;

namespace Epwing2Anki
{
  /// <summary>
  /// Abstract EPWING dictionary base type.
  /// </summary>
  [Serializable]
  abstract public class DicEpwing : Dic
  {
    private string catalogsFile = "";
    private int subBookIdx = 0;


    /// <summary>
    /// Path of this dictionary's CATALOGS file.
    /// </summary>
    public string CatalogsFile
    {
      get { return this.catalogsFile; }
      set { this.catalogsFile = value; }
    }


    /// <summary>
    /// Consructor.
    /// </summary>
    public DicEpwing(string catalogsFile, int subBookIdx)
    {
      this.SourceType = DicSourceTypeEnum.EPWING;

      this.catalogsFile = catalogsFile;
      this.subBookIdx = subBookIdx;
    }

    public enum EplkupTagFlagsEnum
    {
      Emphasis = 1,
      Keyword = 2,
      Link = 4,
      Sub = 8,
      Sup = 16,
    }


    static string eplkupDir = Path.Combine(ConstantSettings.UtilsDir, "eplkup");
    static string eplkupInFile = Path.Combine(eplkupDir, "eplkup_in.txt");
    static string eplkupOutFile = Path.Combine(eplkupDir, "eplkup_out.txt");
    static string eplkupFullExe = Path.Combine(eplkupDir, "eplkup.exe");
    static string eplkupRelExe = String.Format("{0}{1}eplkup{1}eplkup.exe",
      ConstantSettings.UtilsName, Path.DirectorySeparatorChar);


    // eplkup version 1.3 by Christopher Brochtrup.
    //
    //
    // Usage: eplkup [--emphasis] [--gaiji] [--help] [--hit #] [--hit-num] [--html-sub] \
    //               [--html-sup] [--keyword] [--link] [--link-in] [--no-header] [--no-text] [--max-hits #] \
    //               [--show-count] [--subbook #] [--title] [--ver] <book-path> <input-file> <output-file>
    // 
    // Performs an exact search on the provided word in the provided EPWING book.
    // 
    // Required:
    //   book-path    - Directory that contains the EPWING "CATALOG" or "CATALOGS" file.
    //   input-file   - File that contains the word to lookup (in UTF-8 without BOM).
    //   output-file  - File that will contain the lookup text (in UTF-8 without BOM).
    // 
    // Optional:
    //   --emphasis   - Place HTML <em></em> tags around bold/emphasized text.
    //   --gaiji      - 0 = Replace gaiji with no UTF-8 equivalents with a '?' (default).
    //                  1 = Replace gaiji with no UTF-8 equivalents with HTML image tags containing
    //                      embedded base64 image data.
    //   --help       - Show help.
    //   --hit        - Specify which hit to output (starting at 0). If not specified, all hits will be output.
    //   --hit-num    - Output the number of the hit above the hit output (if multiple hits). Ex: {ENTRY: 3}.
    //   --html-sub   - Put HTML <sub></sub> tags around subscript text.
    //   --html-sup   - Put HTML <sup></sup> tags around supercript text.
    //   --keyword    - Put <KEYWORD></KEYWORD> tags around the keyword.
    //   --link       - Put <LINK></LINK> tags around links/references.
    //   --link-in    - The input file contains a link in the format: 'hex_page<whitespace>hex_offset'
    //   --max-hits   - Specify the number of hits to output when --hit is not specified. Default is 20.
    //   --no-header  - Don't print the headers.
    //   --no-text    - Don't print the text.
    //   --show-count - Output the number of lookup hits in the first line of the output file. Ex. {HITS: 6}
    //   --subbook    - The subbook to use in the EPWING book. Default is 0.
    //   --title      - Get the title of the subbook.
    //   --ver        - Show version.

    /// <summary>
    /// Run the eplkup tool with the provided inputs.
    /// </summary>
    private List<string> runEplkup(string inText, string args)
    {
      // Write word to the input file (no BOM)
      StreamWriter writer = new StreamWriter(eplkupInFile, false, new UTF8Encoding(false));
      writer.Write(inText);
      writer.Close();

      // Delete the output file in case it was left over from last time
      File.Delete(eplkupOutFile);

      string eplkupArgs = args;

      UtilsCommon.startProcess(eplkupRelExe, eplkupFullExe, eplkupArgs);

      string lookupText = "";

      // Read the lookup text
      try
      {
        StreamReader reader = new StreamReader(eplkupOutFile, Encoding.UTF8);
        lookupText = reader.ReadToEnd().Trim();
        reader.Close();
      }
      catch(Exception e1)
      {
        // Don't care
        Logger.Instance.error("runEplkup: " + e1);
      }

      lookupText = lookupText.Replace("\r\n", "\n");

      List<string> rawEntryList = new List<string>();

      // Separate out each entry
      if (lookupText.Contains("{ENTRY: 0}"))
      {
        string[] entrySplit = Regex.Split(lookupText, @"{ENTRY: \d*?}");

        foreach (string entry in entrySplit)
        {
          string entryToAdd = entry.Trim();

          if (entryToAdd != "")
          {
            bool duplicate = false;

            foreach (string rawEntry in rawEntryList)
            {
              if (rawEntry == entryToAdd)
              {
                duplicate = true;
                break;
              }
            }

            // Sometimes eplkup returns duplicates, don't add them
            if (!duplicate)
            {
              rawEntryList.Add(entryToAdd);
            }
          }
        }
      }
      else
      {
        if (lookupText.Trim() != "")
        {
          rawEntryList.Add(lookupText);
        }
      }

      return rawEntryList;
    }


    /// <summary>
    /// Given a word, get list containing the raw text for each entry in the EPWING dictionary.
    /// List will not contains duplicate entries or blank lines.
    /// </summary>
    protected List<string> searchEpwingDic(string word, EplkupTagFlagsEnum tagFlags)
    {
      string epwingDir = Path.GetDirectoryName(this.CatalogsFile);
      string tagOptions = this.formatTagFlagStr(tagFlags);

      string eplkupArgs = 
        String.Format("--no-header --gaiji 1 --hit-num {0} --subbook {1} \"{2}\" \"{3}\" \"{4}\"",
          tagOptions, this.subBookIdx, epwingDir, eplkupInFile, eplkupOutFile);

      return this.runEplkup(word, eplkupArgs);
    }


    /// <summary>
    /// Search the EPWING dic with a page and offset.
    /// </summary>
    protected List<string> searchEpwingDic(uint page, uint offset, EplkupTagFlagsEnum tagFlags)
    {
      string epwingDir = Path.GetDirectoryName(this.CatalogsFile);
      string tagOptions = this.formatTagFlagStr(tagFlags);
      string pageAndOffset = String.Format("0x{0:x} 0x{1:x}", page, offset);

      string eplkupArgs =
        String.Format("--gaiji 1 --link-in --subbook {1} \"{2}\" \"{3}\" \"{4}\"",
         tagOptions, this.subBookIdx, epwingDir, eplkupInFile, eplkupOutFile);

      return this.runEplkup(pageAndOffset, eplkupArgs);
    }


    /// <summary>
    /// Format the EPWING tag flags into arguments that can be used by eplkup.
    /// </summary>
    private string formatTagFlagStr(EplkupTagFlagsEnum tagFlags)
    {
      string emphasisStr = "";
      string keywordStr = "";
      string linkStr = "";
      string subStr = "";
      string supStr = "";

      if ((tagFlags & EplkupTagFlagsEnum.Emphasis) == EplkupTagFlagsEnum.Emphasis)
      {
        emphasisStr = "--emphasis";
      }

      if ((tagFlags & EplkupTagFlagsEnum.Keyword) == EplkupTagFlagsEnum.Keyword)
      {
        keywordStr = "--keyword";
      }

      if ((tagFlags & EplkupTagFlagsEnum.Link) == EplkupTagFlagsEnum.Link)
      {
        linkStr = "--link";
      }

      if ((tagFlags & EplkupTagFlagsEnum.Sub) == EplkupTagFlagsEnum.Sub)
      {
        subStr = "--html-sub";
      }

      if ((tagFlags & EplkupTagFlagsEnum.Sup) == EplkupTagFlagsEnum.Sup)
      {
        supStr = "--html-sup";
      }

      string tagOptions = String.Format("{0} {1} {2} {3} {4}",
        emphasisStr, keywordStr, linkStr, subStr, supStr);

      return tagOptions;
    }


    // Sanseido Super Daijirin
    // 『三省堂　スーパー大辞林』
    //   Contains: 1) 『大辞林 第2版』 J-J. Contains Japanese-only examples.
    //             2) 『デイリーコンサイス英和辞典 第5版』 J-E. No examples.
    //
    // Dajirin 2nd Edition. J-J. Japanese-only examples.
    // 『大辞林 第2版』
    //
    // Daijisen. J-J. Japanese-only examples.
    // 『大辞泉』
    // 
    // Genius EJ 3rd J-E 2nd. Contains example sentences.
    // 『ジーニアス英和〈第３版〉・和英〈第２版〉辞典』
    // 
    // Genius EJ-JE. Contains example sentences.
    // 『ジーニアス英和・和英辞典』
    // 
    // Kenkyusha 5th J-E. Contains example sentences.
    // 『研究社　新和英大辞典　第５版』
    // 
    // Kenkyusha Shin Eiwa-Waei Chujiten J-E. Contains example sentences.
    // 『研究社　新英和・和英中辞典』
    // 
    // Kojien 6th Edition. J-J. Japanese-only examples.
    // 『広辞苑第六版』
    // 
    // Meikyo Kokugo Dictionary. J-J. Japanese-only examples.
    // 『明鏡国語辞典』

    /// <summary>
    /// Return appropriate object for supported EPWING dictionaries. 
    /// Otherwise return null.
    /// </summary>
    public static DicEpwing createEpwingDic(string catalogsFile)
    {
      string epwingDir = Path.GetDirectoryName(catalogsFile);
      DicEpwing epwingDic = null;

      // Delete the output file in case it was left over from last time
      File.Delete(eplkupOutFile);

      string eplkupArgs = String.Format("--title \"{0}\" \"{1}\" \"{2}\"",
        epwingDir, eplkupInFile, eplkupOutFile);

      UtilsCommon.startProcess(eplkupRelExe, eplkupFullExe, eplkupArgs);

      string title = "";

      try
      {
        StreamReader reader = new StreamReader(eplkupOutFile, Encoding.UTF8);
        title = reader.ReadToEnd().Trim();
        reader.Close();

        if (title == "")
        {
          throw new Exception();
        }
      }
      catch
      {
        UtilsMsg.showErrMsg("Could not find title of EPWING dictionary!\r\n\r\n"
          + "Please make sure that there are no non-ASCII characters in the\r\n"
          + "path to this dictionary or to Epwing2Anki.\r\n\r\n"
          + "Also make sure that you haven't placed Epwing2Anki in the\r\n"
          + "Program Files directory because of write permission issues.\r\n"
          );
      }

      Logger.Instance.info(String.Format("Selected Dictionary: 『{0}』", title));

      if (title == "三省堂　スーパー大辞林")
      {
        // Note: 『三省堂　スーパー大辞林』 also contains the 『デイリーコンサイス英和辞典 第5版』
        //        J-E dictionary but it is not currently supported.
        epwingDic = new DicEpwingDaijirin2nd(catalogsFile, 0);
      }
      else if (title == "大辞林 第2版")
      {
        epwingDic = new DicEpwingDaijirin2nd(catalogsFile, 0);
      }
      else if (title == "大辞泉")
      {
        epwingDic = new DicEpwingDaijisen(catalogsFile, 0);
      }
      else if (title == "ジーニアス英和〈第３版〉・和英〈第２版〉辞典")
      {

      }
      else if (title == "ジーニアス英和・和英辞典")
      {

      }
      else if (title == "研究社　新和英大辞典　第５版")
      {
        epwingDic = new DicEpwingKen5th(catalogsFile, 0);
      }
      else if (title == "研究社　新英和・和英中辞典")
      {
        epwingDic = new DicEpwingChujiten(catalogsFile, 0);
      }
      else if (title == "広辞苑第六版")
      {
        epwingDic = new DicEpwingKojien6th(catalogsFile, 0);
      }
      else if (title == "明鏡国語辞典")
      {
        epwingDic = new DicEpwingMeikyo(catalogsFile, 0);
      }
      
      return epwingDic;
    }


  }
}
