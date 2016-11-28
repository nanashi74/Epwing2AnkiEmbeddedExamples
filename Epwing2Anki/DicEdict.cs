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
using System.Data.SQLite;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Epwing2Anki
{
  /// <summary>
  /// Represents the EDICT J-E dictionary.
  /// ftp://ftp.edrdg.org/pub/Nihongo/00INDEX.html
  /// 
  /// Uses the Taboeba database for example sentences.
  /// </summary>
  [Serializable]
  public class DicEdict : Dic
  {
    [NonSerialized]
    private SQLiteConnection sqliteConnection = null;

    [NonSerialized]
    private Tatoeba tatoeba = null;

    /// <summary>
    /// Consructor.
    /// </summary>
    public DicEdict()
    {
      this.Name = "EDICT";
      this.NameEng = "EDICT";
      this.ExamplesName = "Tatoeba";
      this.ShortName = "EDICT";
      this.ExamplesNotes = "Includes Tanaka Corpus";
      this.SourceType = DicSourceTypeEnum.DEFAULT;
      this.DicType = DicTypeEnum.JE;
      this.ExamplesType = DicExamplesEnum.NO;
      this.SeparateExampleDictionary = true;
    }

    /// <summary>
    /// Load the EDICT database.
    /// </summary>
    private void openDatabase()
    {
      string edictFile = ConstantSettings.EdictDb;
      string connectStr = String.Format("Data Source={0};Version=3;New=True;Compress=True;", 
        edictFile);

      this.sqliteConnection = new SQLiteConnection(connectStr);
      this.sqliteConnection.Open();
    }


    /// <summary>
    /// Close the EDICT database.
    /// </summary>
    private void closeDatabase()
    {
      this.sqliteConnection.Close();
    }


    /// <summary>
    /// Lookup the given word.
    /// </summary>
    public override List<Entry> lookup(string word, bool includeExamples, FineTune fineTune)
    {
      // The list of entries in the dictionary for this word
      List<Entry> dicEntryList = new List<Entry>();

      // Database search fields have been converted to hiragana so we need to convert the 
      // provided word to hiragana to facilite the search.
      word = UtilsLang.convertKatakanaToHiragana(word);

      if (this.sqliteConnection == null)
      {
        this.openDatabase();
      }

      // Query database for the word
      SQLiteCommand command = sqliteConnection.CreateCommand();
      command.CommandText = 
        String.Format("SELECT * FROM dict WHERE kanji='{0}' OR kana='{0}' LIMIT 100", word);
      SQLiteDataReader sqliteDatareader = command.ExecuteReader();

      while (sqliteDatareader.Read())
      {
        Entry entry = new Entry();
        entry.DicName = this.Name;

        string expression = sqliteDatareader["kanji"].ToString();
        string reading = sqliteDatareader["kana"].ToString();
        string katakana = sqliteDatareader["katakana"].ToString();
        string definition = sqliteDatareader["def"].ToString();

        // Does this word consist of once or more katakana characters?
        if (katakana.Length > 0)
        {
          // Replace hiragana fields with the actual katakana versions.
          string[] fields = katakana.Split(new char[] { '/' });

          if (fields.Length == 1)
          {
            expression = "";
            reading = fields[0];
          }
          else
          {
            expression = fields[0];
            reading = fields[1];
          }
        }

        if (expression.Length == 0)
        {
          expression = reading;
        }

        entry.Expression = expression;
        entry.Reading = reading;
        entry.Definition = definition;

        // If the wants to remove word indicators, do it
        if (fineTune.EdictNoP)
        {
          entry.Definition = entry.Definition.Replace("/(P)/", "/");
        }

        entry.Definition = this.replaceSubDefNumber(entry.Definition);

        entry.Definition = entry.Definition.Replace("/", "; ");

        // Add newline before subdef number
        entry.Definition = Regex.Replace(entry.Definition, "([①-⑳㉑-㉟㊱-㊿])", "<br />$1");

        // If the wants to remove word indicators, do it
        if (fineTune.EdictNoWordIndicators)
        {
          entry.Definition = Regex.Replace(entry.Definition,
          @"\((?:adj-i|adj-na|adj-no|adj-pn|adj-t|adj-f|adj|adv|adv-n|adv-to|aux|aux-v|aux-adj|conj"
            + @"|ctr|exp|id|int|iv|n|n-adv|n-pref|n-suf|n-t|num|pn|pref|prt|suf|v1|v2a-s|v4h|v4r|v5"
            + @"|v5aru|v5b|v5g|v5k|v5k-s|v5m|v5n|v5r|v5r-i|v5s|v5t|v5u|v5u-s|v5uru|v5z|vz|vi|vk|vn"
            + @"|vs|vs-c|vs-i|vs-s|vt|Buddh|MA|comp|food|geom|gram|ling|math|mil|physics|X|abbr"
            + @"|arch|ateji|chn|col|derog|eK|ek|fam|fem|gikun|hon|hum|iK|id|io|m-sl|male|male-sl"
            + @"|ng|oK|obs|obsc|ok|on-mim|poet|pol|rare|sens|sl|uK|uk|vulg|kyb:|osb:|ksb:|ktb:|tsb:"
            + @"|thb:|tsug:|kyu:|rkb:|,)*?\)",
          "");

          entry.Definition = entry.Definition.TrimStart().Replace("  ", " ");

          if (entry.Definition.StartsWith("<br />"))
          {
            entry.Definition = entry.Definition.Remove(0, 6);
          }
        }

        // Replace final semicolon in subdef line with a period
        entry.Definition = Regex.Replace(entry.Definition, "; <br />", ".<br />");

        int lastSemiColon = entry.Definition.LastIndexOf(";");

        if (lastSemiColon > 0)
        {
          StringBuilder builder = new StringBuilder(entry.Definition);
          builder[lastSemiColon] = '.';
          entry.Definition = builder.ToString();
        }

        entry.Definition = entry.Definition.TrimEnd(new char[] { ';', ' ' });

        if (includeExamples)
        {
          if (this.tatoeba == null)
          {
            this.tatoeba = new Tatoeba();
          }

          this.tatoeba.lookup(entry);
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
    /// Replace sub-definition numbers with a single character.
    /// So (1) -> ①, etc.
    /// </summary>
    private string replaceSubDefNumber(string def)
    {
      string newDef = def;
      
      for (int i = 1; i <= 30; i++)
      {
        string parenNum = String.Format("({0})", i);

        if (newDef.Contains(parenNum))
        {
          newDef = newDef.Replace(parenNum, UtilsLang.convertIntegerToCircledNumStr(i));
        }
        else
        {
          break;
        }
      }

      return newDef;
    }

  }



}
