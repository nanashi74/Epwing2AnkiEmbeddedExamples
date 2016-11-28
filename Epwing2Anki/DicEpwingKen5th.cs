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
  /// Represents 『研究社　新和英大辞典　第５版』 (Kenkyusha New Japanese-English Dictionary 5th Edition).
  /// J-E.
  /// Has example sentences.
  /// </summary>
  [Serializable]
  public class DicEpwingKen5th : DicEpwing
  {
    /// <summary>
    /// Constructor.
    /// </summary>
    public DicEpwingKen5th(string catalogsFile, int subBookIdx)
      : base(catalogsFile, subBookIdx)
    {
      this.Name = "研究社　新和英大辞典　第５版";
      this.NameEng = "Kenkyusha New Japanese-English Dictionary 5th Edition";
      this.ShortName = "研究社5";
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
        EplkupTagFlagsEnum.Sub | EplkupTagFlagsEnum.Sup);

      foreach (string rawEntry in rawEntryList)
      {
        Entry entry = new Entry();
        entry.DicName = this.Name;

        List<string> lines = rawEntry.Split(new char[] { '\n' }, 
          StringSplitOptions.RemoveEmptyEntries).ToList();

        if (!this.parseFirstLine(lines[0], entry))
        {
          // Unexpected format or something like 悪そう which has no def or reading
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
      bool subDefNumFound = false;
      int subDef = 1;
      bool defFound = false;
      List<string> subDefList = new List<string>();

      foreach (string line in lines)
      {
        Match match = Regex.Match(line, 
          @"^((?<Tilde>〜)|(?<Example>[▲・])|(?<Def>[1-9]+))?(?<Text>.*)");

        if (match.Success)
        {
          string prefixTilde = match.Groups["Tilde"].ToString().Trim();
          string prefixExample = match.Groups["Example"].ToString().Trim();
          string prefixDef = match.Groups["Def"].ToString().Trim();
          string text = match.Groups["Text"].ToString().Trim();

          if (prefixTilde != "")
          {
            subDefList.Add(line.Trim());
          }
          else if (prefixExample != "")
          {
            if (includeExamples)
            {
              Example example = new Example();
              example.DicName = this.Name;
              example.Text = line.Remove(0, 1).Trim();
              example.SubDefNumber = subDef;
              example.HasTranslation = true;

              // Make sure that the sentence contains a translation
              if (UtilsLang.containsAlpha(example.Text))
              {
                // Replace Japanese space with a tab to seperate the Japapnese Example and Translation
                example.Text = example.Text.Replace("　", "\t");

                entry.ExampleList.Add(example);
              }
            }
          }
          else
          {
            if (prefixDef != "")
            {
              subDefNumFound = true;
              subDef = Convert.ToInt32(prefixDef);
            }
            else if (defFound)
            {
              // Stop if sub word is encountered (see 悪い or 美しい for example)
              break;
            }

            // Case where entry has no definition, but does have example sentences
            if ((entry.ExampleList.Count >= 1) && !defFound)
            {
            }
            else
            {
              defFound = true;

              if (subDefNumFound)
              {
                string circledNum = UtilsLang.convertIntegerToCircledNumStr(subDef);
                subDefList.Add(circledNum + " " + text);
              }
              else
              {
                subDefList.Add(prefixDef + text);
              }
            }
          }
        }
      }

      int count = 1;

      // Create the definition text
      foreach (string def in subDefList)
      {       
        if(count == subDefList.Count)
        {
          entry.Definition += def;
        }
        else
        {
          entry.Definition += def + "<br />";
        }

        count++;
      }

      // Replace ugly tilde with nicer tilde
      entry.Definition = entry.Definition.Replace("〜", "～");

      // If the definition is blank, the example sentences cannot have a sub definition
      if (entry.Definition == "")
      {
        foreach (Example example in entry.ExampleList)
        {
          example.SubDefNumber = Example.NO_SUB_DEF;
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

      // Case 1: "うつくしい【美しい】 ﾛｰﾏ(utsukushii)"
      Match match = Regex.Match(line, @"^(?<Reading>.*?)【(?<Expression>.*?)】 ﾛｰﾏ\(.*?\)$");

      if (match.Success)
      {
        entry.Reading = match.Groups["Reading"].ToString().Trim();
        entry.Expression = match.Groups["Expression"].ToString().Trim();
      }
      else
      {
        // Case 2: "スキーマ ﾛｰﾏ(sukīma)"
        match = Regex.Match(line, @"^(?<Expression>.*?) ﾛｰﾏ\(.*?\)$");

        if (match.Success)
        {
          entry.Expression = match.Groups["Expression"].ToString().Trim();
          entry.Reading = entry.Expression;
        }
        else
        {
          // Case 3: 
          // "隙間産業　a niche industry; a niche business."
          // "◧侵襲期　the stage of invasion."
          match = Regex.Match(line, "^[◨◧]?(?<Expression>.*?)　(?<Definition>.*)$");

          if (match.Success)
          {
            entry.Expression = match.Groups["Expression"].ToString().Trim();
            entry.Definition = match.Groups["Definition"].ToString().Trim();

            // If the expression is kana only
            if (!UtilsLang.containsIdeograph(entry.Expression))
            {
              entry.Reading = entry.Expression;
            }
            else
            {
              // This case has no reading
            }
          }
          else
          {
            success = false;
          }
        }
      }

      return success;
    }




  }
}
