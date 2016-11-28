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

namespace Epwing2Anki
{
  /// <summary>
  /// Represents 『研究社　新英和・和英中辞典』 (Kenkyusha Shin Eiwa-Waei Chujiten).
  /// J-E.
  /// Has example sentences.
  /// </summary>
  [Serializable]
  class DicEpwingChujiten : DicEpwing
  {
    /// <summary>
    /// Constructor.
    /// </summary>
    public DicEpwingChujiten(string catalogsFile, int subBookIdx)
      : base(catalogsFile, subBookIdx)
    {
      this.Name = "研究社　新英和・和英中辞典";
      this.NameEng = "Kenkyusha Shin Eiwa-Waei Chujiten";
      this.ShortName = "中辞典";
      this.DicType = DicTypeEnum.JE;
      this.ExamplesType = DicExamplesEnum.YES;
    }


    /// <summary>
    /// Lookup the given word.
    /// </summary>
    public override List<Entry> lookup(string word, bool includeExamples, FineTune fineTune)
    {
      // The list of entries in the dictionary for this word
      List<Entry> dicEntryList = new List<Entry>();
      List<string> rawEntryList = this.searchEpwingDic(word,
        EplkupTagFlagsEnum.Link | EplkupTagFlagsEnum.Sub | EplkupTagFlagsEnum.Sup);

      foreach (string rawEntry in rawEntryList)
      {
        Entry entry = new Entry();
        entry.DicName = this.Name;

        List<string> lines = rawEntry.Split(new char[] { '\n' },
          StringSplitOptions.RemoveEmptyEntries).ToList();

        if (!this.parseFirstLine(lines[0], entry))
        {
          continue;
        }

        // Remove the line that was already parsed
        lines.RemoveAt(0);

        this.parseBody(lines, entry, includeExamples);

        // Don't use this entry for definition purposes if it doesn't contain alpha characters.
        if (fineTune.JeNoAlphaFallback
          && !this.containsAlpha(entry.Definition))
        {
          entry.SecondaryDefinition = entry.Definition;
          entry.Definition = "";
        }

        dicEntryList.Add(entry);
      }

      if (dicEntryList.Count == 0)
      {
        dicEntryList = null;
      }

      return dicEntryList;
    }


    /// <summary>
    /// Does the string contains alpha characters after the HTML has been stipped.
    /// </summary>
    private bool containsAlpha(string text)
    {
      string noHtml = Regex.Replace(text, "<.*?>", "");
      return UtilsLang.containsAlpha(noHtml);
    }


     /// <summary>
    /// Parse the body of the entry.
    /// </summary>
    private void parseBody(List<string> lines, Entry entry, bool includeExamples)
    {
      int subDef = 1;
      Match match;

      string expressionNoSpecialChars = UtilsFormatting.removeSpecialCharsFromExpression(entry.Expression);
      string readingNoSpecialChars = UtilsFormatting.removeSpecialCharsFromExpression(entry.Reading);

      foreach (string line in lines)
      {
        string defLine = line;

        // If example sentence
        if(defLine.StartsWith("◆"))
        {
          string jap = "";
          string eng = "";
          bool status = this.separateExample(defLine, out jap, out eng);

          if (status)
          {
            // Check for examples that are more like definitions (in the tradition of ken5). 
            // Example: 年上の in the 年上 entry.
            match = Regex.Match(jap, String.Format("^(?:{0}|{1})(?<TrailingText>で|する|と|な|に|の)$",
              expressionNoSpecialChars, readingNoSpecialChars));

            // If this is a really a definition.
            if (match.Success)
            {
              string trailingText = match.Groups["TrailingText"].ToString().Trim();

              entry.Definition += String.Format("～{0} {1}<br />",
                trailingText, eng);
            }
            else // It's a real example
            {
              Example example = new Example();
              example.DicName = this.Name;
              example.SubDefNumber = subDef;
              example.HasTranslation = true;
              example.Text = String.Format("{0}\t{1}", jap, eng);
              example.Text = example.Text.Replace("<LINK>", "");
              example.Text = Regex.Replace(example.Text, "</LINK.*?>", "");

              entry.ExampleList.Add(example);
            }
          }
        }
        // Else if this is an example sentence link
        else if (defLine.StartsWith("<LINK>→✎</LINK"))
        {
          this.getExamplesBehindLink(defLine, entry, subDef);
        }
        // Else if this line is entirely a link. skip it
        else if(defLine.StartsWith("<LINK>→</LINK"))
        {
          continue;
        }
        else // It is either a definition or a subword
        {
          match = Regex.Match(defLine, @"^(?:(?<SubDefNum>\d*) )?(?<Def>.*)");

          if (match.Success)
          {
            string subDefNumStr = match.Groups["SubDefNum"].ToString().Trim();
            string def = match.Groups["Def"].ToString().Trim();

            // If the definition is a sub definition
            if (subDefNumStr != "")
            {
              subDef = Convert.ToInt32(subDefNumStr);
              entry.Definition += String.Format("{0} {1}<br />", 
                UtilsLang.convertIntegerToCircledNumStr(subDef), def);
            }
            // Else if the definition is not a subword (like 独唱会 in the 独唱 entry);
            else if ((def.Length > 0) && !UtilsLang.containsIdeograph(def[0]))
            {
              entry.Definition += def + "<br />";
            }
          }
        }
      }

      // Remove the link tags, but keep the link text
      entry.Definition = entry.Definition.Replace("<LINK>", "");
      entry.Definition = Regex.Replace(entry.Definition, "</LINK.*?>", "");

      // Remove trailing <br /> from definition
      if (entry.Definition.EndsWith("<br />"))
      {
        entry.Definition = entry.Definition.Remove(entry.Definition.Length - 6);
      }
    }


    /// <summary>
    /// Return the Japanese and English portions of the example sentence.
    /// All this because numbers, letters, and spaces can exist in the Japanese part of the definition.
    /// </summary>
    private bool separateExample(string example, out string jap, out string eng)
    {
      // Example: ◆年を取る grow older; age; 《fml》 get on in years; 《口語》 get on; 〈老人になる〉 get [grow, 《fml》

      jap = "";
      eng = "";

      // Delete ◆
      example = example.Substring(1);

      bool status = true;
      int lastJapDefIdx = 0;

      bool inBracket1 = false; // 《...》
      bool inBracket2 = false; // 〈...〉
      bool inBracket3 = false; // (...)

      for (int i = example.Length - 1; i >= 0; i--)
      {
        char c = example[i];

        if (c == '》')
        {
          inBracket1 = true;
        }
        else if (c == '《')
        {
          inBracket1 = false;
        }
        else if (c == '〉')
        {
          inBracket2 = true;
        }
        else if (c == '〈')
        {
          inBracket2 = false;
        }
        else if (c == ')')
        {
          inBracket3 = true;
        }
        else if (c == '(')
        {
          inBracket3 = false;
        }
        else if (!inBracket1 && !inBracket2 && !inBracket3 
          && (UtilsLang.containsIdeograph(c) 
            || UtilsLang.containsHiragana(c)
            || UtilsLang.containsKatakana(c)))
        {
          lastJapDefIdx = i;
          break;
        }
      }

      try
      {
        // If didn't end in a Japanese character as in "◆主要産業 ☛<LINK>→</LINK[28879:1FC]>さんぎょう"
        if (lastJapDefIdx != example.Length - 1)
        {
          jap = example.Substring(0, lastJapDefIdx + 1);
          eng = example.Substring(lastJapDefIdx + 2);
        }
      }
      catch (Exception e1)
      {
        // Don't care
        Logger.Instance.error("chujiten.separateExample: " + e1);
      }

      if ((jap == "") || (eng == ""))
      {
        status = false;
      }

      return status;
    }


    /// <summary>
    /// Get the example sentences that are behind a link.
    /// 
    /// </summary>
    private void getExamplesBehindLink(string defLine, Entry entry, int subDef)
    {
      // Example: <LINK>→✎</LINK[2C40F:5CA]>

      Match match = Regex.Match(defLine, @"\[(?<Page>[0-9A-F]*):(?<Offset>[0-9A-F]*)\]");

      if (match.Success)
      {
        uint page = Convert.ToUInt32(match.Groups["Page"].ToString().Trim(), 16);
        uint offset = Convert.ToUInt32(match.Groups["Offset"].ToString().Trim(), 16);

        List<string> lines = this.searchEpwingDic(page, offset, EplkupTagFlagsEnum.Sub | EplkupTagFlagsEnum.Sup);

        if (lines.Count > 0)
        {
          string[] splitLines = lines[0].Split(new char[] { '\n' }, StringSplitOptions.RemoveEmptyEntries);

          foreach (string line in splitLines)
          {
            match = Regex.Match(line, @"^(?<Jap>.*?\.) (?<Eng>.*)");

            if (match.Success)
            {
              string jap = match.Groups["Jap"].ToString().Trim();
              string eng = match.Groups["Eng"].ToString().Trim();

              if ((jap != "") && (eng != ""))
              {
                Example example = new Example();
                example.DicName = this.Name;
                example.SubDefNumber = subDef;
                example.HasTranslation = true;
                example.Text = String.Format("{0}\t{1}", jap, eng);
                example.Text = example.Text.Replace("<LINK>", "");
                example.Text = Regex.Replace(example.Text, "</LINK.*?>", "");

                entry.ExampleList.Add(example);
              }
            }
          }
        }
      }
    }


    /// <summary>
    /// Parse the first line of an entry.
    /// Sets the reading, expression, and maybe definition of the entry.
    /// Returns false on error.
    /// </summary>
    private bool parseFirstLine(string line, Entry entry)
    {
      bool success = true;

      // Remove sup tag
      line = Regex.Replace(line, "<sup>.*?</sup>", "");

      // Cases to handle:
      // 1) どくしょう 独唱
      // 2) 独唱会 a (vocal) recital
      // 3) バー
      Match match = Regex.Match(line, @"^(?<Reading>.*?)(?:(?: (?<Expression>.*))|$)");

      if (match.Success)
      {
        entry.Reading = match.Groups["Reading"].ToString().Trim();
        entry.Expression = match.Groups["Expression"].ToString().Trim();

        // Case 3: "バー"
        if (entry.Expression == "")
        {
          entry.Expression = entry.Reading;
        }
        // Case 2: "独唱会 a (vocal) recital"
        else if (UtilsLang.containsAlpha(entry.Expression))
        {
          entry.Definition = entry.Expression;
          entry.Expression = entry.Reading;
          entry.Reading = "";
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
