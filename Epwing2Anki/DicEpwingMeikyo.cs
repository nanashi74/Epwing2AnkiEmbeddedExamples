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
  /// Represents 『明鏡国語辞典』 (Meikyo Kokugo Dictionary).
  /// J-J.
  /// Japanese-only example sentences.
  /// </summary>
  [Serializable]
  class DicEpwingMeikyo : DicEpwing
  {
    /// <summary>
    /// Constructor.
    /// </summary>
    public DicEpwingMeikyo(string catalogsFile, int subBookIdx)
      : base(catalogsFile, subBookIdx)
    {
      this.Name = "明鏡国語辞典";
      this.NameEng = "Meikyo Kokugo Dictionary";
      this.ShortName = "明鏡";
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
        EplkupTagFlagsEnum.Sub | EplkupTagFlagsEnum.Sup);

      foreach (string rawEntry in rawEntryList)
      {
        if (rawEntry.Length > 0)
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
            entry.Reading = entry.Reading.Replace("‐", "");
            entry.Reading = entry.Reading.Replace("・", "");
          }

          // Remove the line that was already parsed
          lines.RemoveAt(0);

          this.parseBody(lines, entry, includeExamples, fineTune);

          dicEntryList.Add(entry);
        }
      }

      if (dicEntryList.Count == 0)
      {
        dicEntryList = null;
      }

      return dicEntryList;
    }


    /// <summary>
    /// Parse the body of the entry.
    /// </summary>
    private void parseBody(List<string> lines, Entry entry, bool includeExamples, FineTune fineTune)
    {
      int subDef = 1;
      bool subDefFound = false;

      foreach (string line in lines)
      {
        string defLine = line;

        // Get the subdef number
        int newSubDef = UtilsLang.convertCircledNumToInteger(defLine[0]);

        if (newSubDef != -1)
        {
          subDefFound = true;
          subDef = newSubDef;

          // Add a space between the subdef number and the def
          defLine = defLine[0] + " " + defLine.Substring(1);
        }
        else if (subDefFound)
        {
          subDef = Example.NO_SUB_DEF;
        }

        // Parse out examples sentences
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

        defLine += "<br />";
        entry.Definition += defLine.Trim();
      }

      // Remove trailing <br />
      if (entry.Definition.EndsWith("<br />"))
      {
        entry.Definition = entry.Definition.Remove(entry.Definition.Length - 6).TrimEnd();
      }
    }


    /// <summary>
    /// Parse the examples from a line.
    /// Added to exampleList nd returns the original line without the examples.
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
            Logger.Instance.error("meikryo.processExampleBlanks: " + e1);
          }
        }

        newExample = newExample.Replace("─", unformattedExpression);
      }
      else // Make expression placeholder more obvious
      {
        newExample = newExample.Replace("─", "___");
      }

      return newExample;
    }


    /// <summary>
    /// Parse the first line of an entry.
    /// Sets the reading and expression of the entry.
    /// Will ignore kanji entries (by design).
    /// Returns false on error.
    /// </summary>
    private bool parseFirstLine(string line, Entry entry)
    {
      bool success = true;

      // Three cases to handle:
      // 1) きょう‐はく【強迫】<sup>キャウ─</sup>〘名・他サ変〙
      // 2) バー[bar]〘名〙
      // 3) はあ〘感〙

      Match match = Regex.Match(line, 
        @"(?:^(?<Reading>.*?)【(?<Expression>.*?)】)|(?:^(?<Reading>.*?)\[(?<EngExpression>.*?)\])|(?:^(?<Reading>.*?)〘)");

      if (match.Success)
      {
        entry.Reading = match.Groups["Reading"].ToString().Trim();
        entry.Expression = match.Groups["Expression"].ToString().Trim();

        if (entry.Reading == "")
        {
          success = false;
        }
        else
        {
          if (entry.Expression == "")
          {
            // Will get here in cases 2 and 3
            entry.Expression = entry.Expression = entry.Reading.Replace("‐", "");
          }
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
