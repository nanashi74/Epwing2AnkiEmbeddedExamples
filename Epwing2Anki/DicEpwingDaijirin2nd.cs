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
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.IO;

namespace Epwing2Anki
{
  /// <summary>
  /// Represents 『大辞林 第2版』 (Daijirin 2nd Edition).
  /// J-J.
  /// Japanese-only example sentences.
  /// </summary>
  [Serializable]
  public class DicEpwingDaijirin2nd : DicEpwing
  {
    /// <summary>
    /// Constructor.
    /// </summary>
    public DicEpwingDaijirin2nd(string catalogsFile, int subBookIdx)
      : base(catalogsFile, subBookIdx)
    {
      this.Name = "大辞林 第2版";
      this.NameEng = "Daijirin 2nd Edition";
      this.ShortName = "大辞林2";
      this.ExamplesNotes = "Japanese only";
      this.DicType = DicTypeEnum.JJ;
      this.ExamplesType = DicExamplesEnum.J_ONLY;
    }


    /// <summary>
    /// Lookup the given word.
    /// </summary>
    public override List<Entry> lookup(string word, bool includeExamples, FineTune fineTune)
    {
      // The list of entries in the dictionary for this word
      List<Entry> dicEntryList = new List<Entry>();
      List<string> rawEntryList = this.searchEpwingDic(word, 
        EplkupTagFlagsEnum.Link | EplkupTagFlagsEnum.Keyword);

      foreach (string rawEntry in rawEntryList)
      {
        if (rawEntry.Length > 0)
        {
          if (this.isEntryTextFromDaijinJJ2nd(rawEntry))
          {
            Entry entry = new Entry();
            entry.DicName = this.Name;

            List<string> lines = rawEntry.Split(new char[] { '\n' }, 
              StringSplitOptions.RemoveEmptyEntries).ToList();

            if (!this.parseFirstLine(lines[0], entry))
            {
              // Unexpected format
              continue;
            }

            // If user want to remove special characters from the reading, do it
            if (fineTune.JjRemoveSpecialReadingChars)
            {
              entry.Reading = entry.Reading.Replace("-", "");
              entry.Reading = entry.Reading.Replace("・", "");
            }

            // Remove the line that was already parsed
            lines.RemoveAt(0);

            this.parseBody(lines, entry, includeExamples, fineTune);

            dicEntryList.Add(entry);
          }
        }
      }

      if (dicEntryList.Count == 0)
      {
        dicEntryList = null;
      }

      return dicEntryList;
    }


    /// <summary>
    /// Is the entry text from 『大辞林 第2版』 (Daijirin 2nd Edition)?
    /// Otherwise, it might be from 『デイリーコンサイス英和辞典 第5版』 
    /// (Daily Concise Japanese-English Dictionary 5th Edition).
    /// </summary>
    private bool isEntryTextFromDaijinJJ2nd(string entryText)
    {
      bool isJJ = true;

      if(entryText.Length == 0)
      {
        return false;
      }

      //
      // Try quick tests first and than move toward more lengthy tests.
      //

      // Many 『デイリーコンサイス英和辞典 第5版』contain this link
      if (entryText.Contains("→英和"))
      {
        isJJ = false;
      }
      else
      {
        string firstLine = UtilsCommon.getFirstLine(entryText);
        string firstLineNoKeyword = Regex.Replace(firstLine, "<KEYWORD>.*?</KEYWORD>", "").Trim();

        // If the first lines is blank after removing the keyword and doesn't contain '['
        // Example: "<KEYWORD>あじわう【味わう】</KEYWORD>"
        if((firstLineNoKeyword.Length == 0)
          && !firstLine.Contains('['))
        {
          // Is this point, for most words it is probably a J-E.
          // Some exceptions like one of the J-J "に" entries will also get to this point though
          isJJ = false;
          
          // The following tests will try to disprove that the entry is J-E.

          // 『デイリーコンサイス英和辞典 第5版』 will not contain an '-' except as maybe the first character
          if ((firstLine[0] != '-') && firstLine.Contains('-'))
          {
            isJJ = true;
          }
          else
          {
            string body = entryText.Substring(firstLine.Length).Trim();
            body = body.Replace("<LINK>", "");
            body = Regex.Replace(body, "</LINK.*?>", "");

            int alphaCount = 0;
            int nonAlphaCount = 0;

            for (int i = 0; i < body.Length; i++)
            {
              char c = body[i];

              if (((c >= 'A') && (c <= 'Z')) || ((c >= 'a') && (c <= 'z'))
                || (c == ' ') || (c == '.') || (c == '\''))
              {
                alphaCount++;
              }
              else
              {
                nonAlphaCount++;
              }
            }

            // If there are more non-alpha than alpha characters, is is probably a J-J.
            if (nonAlphaCount > alphaCount)
            {
              isJJ = true;
            }
          }
        }
      }

      return isJJ;
    }


    /// <summary>
    /// Parse the body of the entry.
    /// </summary>
    private void parseBody(List<string> lines, Entry entry, bool includeExamples, FineTune fineTune)
    {
      int subDef = 1;

      foreach (string line in lines)
      {
        string defLine = line;

        Match match = Regex.Match(defLine, @"^（(?<SubDef>\d.*?)）");

        if (match.Success)
        {
          try
          {
            string subDefStr = match.Groups["SubDef"].ToString().Trim();
            subDef = UtilsLang.convertJapNumToInteger(subDefStr);

            // Convert subdef number in definition to circled number
            defLine = Regex.Replace(defLine, @"^（(?<SubDef>\d.*?)）", "");
            defLine = UtilsLang.convertIntegerToCircledNumStr(subDef) + " " + defLine;
          }
          catch (Exception e1)
          {
            // Don't care
            Logger.Instance.error("daijirin.parseBody: " + e1);
          }
        }
        else
        {
          subDef = Example.NO_SUB_DEF;
        }

        // Remove the links from a line if the line doesn't contain other text and is not the only line.
        defLine = removeLinks(defLine, (lines.Count == 1));
        
        // Get the examples sentences
        List<string> exampleList = new List<string>();
        string defNoExamples = this.parseExamples(defLine, exampleList, entry.Expression, 
          fineTune.JjFillInExampleBlanksWithWord);

        // Remove example sentences from definition unless user says otherwise
        if (!fineTune.JjKeepExamplesInDef)
        {
          defLine = defNoExamples;
        }

        foreach (string exampleText in exampleList)
        {
          Example example = new Example();
          example.Text = exampleText.Trim();

          if (example.Text != "")
          {
            example.DicName = this.Name;
            example.SubDefNumber = subDef;
            example.HasTranslation = false;
            entry.ExampleList.Add(example);
          }
        }

        // Add English space after fullstop to allow wrapping of long lines.
        defLine = defLine.Replace("。", "。 ");

        // After removing the link, make sure that the line is not blank.
        if (defLine.Trim() != "")
        {
          defLine += "<br />";
          entry.Definition += defLine.Trim();
        }
      }

      // Remove trailing <br />
      if (entry.Definition.EndsWith("<br />"))
      {
        entry.Definition = entry.Definition.Remove(entry.Definition.Length - 6).TrimEnd();
      }
    }


    /// <summary>
    /// Remove the links from a line if the line doesn't contain other text and is not the only line.
    /// </summary>
    private string removeLinks(string line, bool onlyLine)
    {
      string defLine = line;
      string noLinkDef = Regex.Replace(defLine, "<LINK>.*?</LINK.*?>", "").Trim();

      // If the line would be blank after removing the links
      // and this was not the only line in the entry, remove the link text
      if ((noLinkDef == "") && !onlyLine)
      {
        // Remove the links
        defLine = noLinkDef;
      }
      else
      {
        // Remove the link tags, but keep the link text
        defLine = defLine.Replace("<LINK>", "");
        defLine = Regex.Replace(defLine, "</LINK.*?>", "");
      }

      return defLine;
    }


    /// <summary>
    /// Parse the examples from a line.
    /// Added to exampleList and returns the original line without the examples.
    /// </summary>
    public string parseExamples(string line, List<string> exampleList, string expression, bool fillInBlanks)
    {
      Match match;
      string noExamplesLine = line;
      int failsafe = 0;

      // Keep applying regex to get last example in line until no more examples are present.
      while (true)
      {
        // Samples:
        // 1) 「not_ex」def。def。source1「ex1」「ex2」「ex3」
        // 2) 「not_ex」def。def。source1「ex1」「ex2」「ex3」→traling。
        match = Regex.Match(noExamplesLine, @"(?<Example>(?<Source>[^。」]*?)「[^」]*?」。?)(?<Trailing>[↔→].*?)?$");

        if (match.Success)
        {
          failsafe++;

          string source = match.Groups["Source"].ToString().Trim();
          string example = match.Groups["Example"].ToString().Trim();
          string trailing = match.Groups["Trailing"].ToString().Trim();

          // Just in case
          if ((example == "") || (failsafe > 200))
          {
            break;
          }

          // Remove the example sentence, but leave in the trailing text
          noExamplesLine = noExamplesLine.Remove(
            noExamplesLine.Length - example.Length - trailing.Length, example.Length).Trim();

          // Remove the source text
          if (source != "")
          {
            example = example.Substring(source.Length);
          }

          // Remove '「' and '」'
          example = example.Replace("「", "").Replace("」", "");

          // Handle blank part of example
          example = this.processExampleBlanks(example, expression, fillInBlanks);

          exampleList.Add(example);
        }
        else
        {
          break;
        }
      }

      return noExamplesLine;
    }


    /// <summary>
    /// If user has "Fill in example sentence blanks with expression" checked, 
    /// 無罪を―する  --->  ▲無罪を確信する
    /// Otherwise,
    /// 無罪を―する  --->  ▲無罪を___する
    /// </summary>
    private string processExampleBlanks(string example, string expression, bool fillInBlanks)
    {
      string newExample = example;

      //  If user wants to fill in the blanks with the expression
      if (fillInBlanks)
      {
        string unformattedExpression = expression.Replace('・', '|');
        unformattedExpression = UtilsFormatting.removeSpecialCharsFromExpression(unformattedExpression);

        // Only use the first expression (零す・溢す  --->  零す)
        if (unformattedExpression.Contains('|'))
        {
          try
          {
            unformattedExpression = unformattedExpression.Substring(0, unformattedExpression.IndexOf('|'));
          }
          catch (Exception e1)
          {
            // Don't care
            Logger.Instance.error("daijirin.processExampleBlanks: " + e1);
          }
        }

        newExample = newExample.Replace("―", unformattedExpression);
      }
      else // Make expression placeholder more obvious
      {
        newExample = newExample.Replace("―", "___");
      }

      return newExample;
    }


    /// <summary>
    /// Parse the first line of an entry.
    /// Sets the reading and expression of the entry.
    /// Returns false on error.
    /// </summary>
    private bool parseFirstLine(string line, Entry entry)
    {
      bool success = true;

      // Three cases to handle:
      // 1) <KEYWORD>きょう-はく</KEYWORD> キヤウ― [0] 【強迫】 （名）スル
      // 2) <KEYWORD>バー</KEYWORD> [1] 〖bar〗
      // 3) <KEYWORD>ぱあ</KEYWORD> [1]

      Match match = Regex.Match(line, @"^<KEYWORD>(?<Reading>.*?)</KEYWORD>");

      if (match.Success)
      {
        entry.Reading = match.Groups["Reading"].ToString().Trim();

        match = Regex.Match(line, @"【(?<Expression>.*?)】");
        
        if (match.Success)
        {
          entry.Expression = match.Groups["Expression"].ToString().Trim();
        }

        // If this entry does not have an expression
        if (entry.Expression == "")
        {
          entry.Expression = entry.Reading.Replace("-", "");
        }
      }
      else
      {
        success = false;
      }

      return success;
    }




  }
}
