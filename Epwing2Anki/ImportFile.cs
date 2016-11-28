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
  /// Used to generate an Anki import file.
  /// </summary>
  public class ImportFile
  {
    private List<CardInfo> cardList = null;
    private Settings settings = null;
    private int numCardsInImportFile = 0;


    /// <summary>
    /// The number of cards in the import file.
    /// </summary>
    public int NumCardsInImportFile
    {
      get { return this.numCardsInImportFile; }
      set { this.numCardsInImportFile = value; }
    }


    /// <summary>
    /// Constructor.
    /// </summary>
    public ImportFile(Settings settings, List<CardInfo> cardList)
    {
      this.cardList = cardList;
      this.settings = settings;
    }


    /// <summary>
    /// Get a list of cards that have one or more entries.
    /// </summary>
    private List<CardInfo> getFoundCardsOnly()
    {
      List<CardInfo> cards = new List<CardInfo>();

      foreach (CardInfo card in this.cardList)
      {
        if ((card.getEntriesPriority(this.settings.DicList).Count > 0)
          && (card.Expression != ""))
        {
          cards.Add(card);
        }
      }

      return cards;
    }


    /// <summary>
    /// Generate the Anki import file.
    /// </summary>
    public void generate()
    {
      List<List<string>> lines = null;

      // If user want to create one card per example sentence
      if (this.settings.ExpandExamples)
      {
        lines = this.generateImportFileLinesExpandExamples();
      }
      else
      {
        // Create normal vocabulary cards
        lines = this.generateImportFileLines();
      }

      this.numCardsInImportFile = lines.Count;

      StreamWriter writer = new StreamWriter(this.settings.OutFile, false, Encoding.UTF8);

      // Write each line to file
      foreach (List<string> line in lines)
      {
        string lineText = "";

        for(int i = 0; i < line.Count; i++)
        {
          lineText += line[i];

          // Add tab if not the last field in line
          if (i != line.Count - 1)
          {
            lineText += "\t";
          }
        }

        writer.WriteLine(lineText);
      }

      writer.Close();
    }


    /// <summary>
    /// Generate the lines of the import file for normal vocabulary cards.
    /// </summary>
    private List<List<string>> generateImportFileLines()
    {
      List<List<string>> lines = new List<List<string>>();

      List<CardInfo> foundCards = new List<CardInfo>();

      // If user wants to add placeholders for entries that were not found
      if (this.settings.FineTuneOptions.AddPlaceholders)
      {
        foundCards = this.cardList;

        foreach (CardInfo card in foundCards)
        {
          if (card.Expression == "")
          {
            card.Expression = card.Word.Word;
          }
        }
      }
      else // Don't add placeholders
      {
        foundCards = this.getFoundCardsOnly();
      }

      foreach (CardInfo card in foundCards)
      {
        List<string> lineFields = new List<string>();

        // Expand all fields for this card
        foreach (CardLayoutField field in this.settings.CardLayout.Fields)
        {
          string fieldText = "";

          if (field.Type == FieldEnum.Expression)
          {
            fieldText = card.Expression;
          }
          else if (field.Type == FieldEnum.ExpressionRuby)
          {
            fieldText = this.formatExpressionRuby(card);
          }
          else if (field.Type == FieldEnum.Reading)
          {
            fieldText = card.Reading;
          }
          else if (field.Type == FieldEnum.Def)
          {
            fieldText = this.formatDefinitionHiPri(card);
          }
          else if (field.Type == FieldEnum.DefJE)
          {
            fieldText = this.formatDefinitionType(card, DicTypeEnum.JE);
          }
          else if (field.Type == FieldEnum.DefJJ)
          {
            fieldText = this.formatDefinitionType(card, DicTypeEnum.JJ);
          }
          else if (field.Type == FieldEnum.DefDicName)
          {
            fieldText = this.formatDefinitionDicName(card, field);
          }
          else if (field.Type == FieldEnum.Examples)
          {
            fieldText = this.formatExamples(card);
          }
          else if (field.Type == FieldEnum.ExamplesNoTranslation)
          {
            fieldText = this.formatExamplesNoTranslation(card);
          }
          else if (field.Type == FieldEnum.ExamplesTranslationOnly)
          {
            fieldText = this.formatExamplesTranslationOnly(card);
          }
          else if (field.Type == FieldEnum.Tags)
          {
            fieldText = this.settings.Tags;
          }

          lineFields.Add(fieldText);
        }

        // If user wants to append line from original word list
        if (this.settings.AppendLineFromWordList)
        {
          lineFields.Add(card.Word.RawLine);
        }

        // Add lines fields to line list
        lines.Add(lineFields);
      }

      return lines;
    }


    /// <summary>
    /// Generate the lines of the import file for sentence cards.
    /// </summary>
    private List<List<string>> generateImportFileLinesExpandExamples()
    {
      List<List<string>> lines = new List<List<string>>();

      List<CardInfo> foundCards = new List<CardInfo>();

      // If user wants to add placeholders for entries that were not found
      if (this.settings.FineTuneOptions.AddPlaceholders)
      {
        foundCards = this.cardList;

        foreach (CardInfo card in foundCards)
        {
          if (card.Expression == "")
          {
            card.Expression = card.Word.Word;
          }
        }
      }
      else // Don't add placeholders
      {
        foundCards = this.getFoundCardsOnly();
      }

      foreach (CardInfo card in foundCards)
      {
        bool addedFakeExample = false;

        // If user wants to add placeholders and entry has no examples,
        // add a fake one
        if(this.settings.FineTuneOptions.AddPlaceholders && 
          (card.ChosenExampleList.Count == 0))
        {
          addedFakeExample = true;
          card.ChosenExampleList.Add(new Example());
        }

        foreach (Example example in card.ChosenExampleList)
        {
          List<string> lineFields = new List<string>();

          // Expand all fields for this card
          foreach (CardLayoutField field in this.settings.CardLayout.Fields)
          {
            string fieldText = "";

            if (field.Type == FieldEnum.Expression)
            {
              fieldText = card.Expression;
            }
            else if (field.Type == FieldEnum.ExpressionRuby)
            {
              fieldText = this.formatExpressionRuby(card);
            }
            else if (field.Type == FieldEnum.Reading)
            {
              fieldText = card.Reading;
            }
            else if (field.Type == FieldEnum.Def)
            {
              fieldText = this.formatDefinitionHiPri(card);
            }
            else if (field.Type == FieldEnum.DefJE)
            {
              fieldText = this.formatDefinitionType(card, DicTypeEnum.JE);
            }
            else if (field.Type == FieldEnum.DefJJ)
            {
              fieldText = this.formatDefinitionType(card, DicTypeEnum.JJ);
            }
            else if (field.Type == FieldEnum.DefDicName)
            {
              fieldText = this.formatDefinitionDicName(card, field);
            }
            else if (field.Type == FieldEnum.Examples)
            {
              fieldText = this.formatSingleExample(example, false);
            }
            else if (field.Type == FieldEnum.ExamplesNoTranslation)
            {
              fieldText = this.formatSingleExampleNoTranslation(example, false);
            }
            else if (field.Type == FieldEnum.ExamplesTranslationOnly)
            {
              fieldText = this.formatSingleExampleTranslationOnly(example, false);
            }
            else if (field.Type == FieldEnum.Tags)
            {
              fieldText = this.settings.Tags;
            }

            lineFields.Add(fieldText);
          }

          // If user wants to append line from original word list
          if (this.settings.AppendLineFromWordList)
          {
            lineFields.Add(card.Word.RawLine);
          }

          // Add lines fields to line list
          lines.Add(lineFields);
        }

        // If added a fake example placeholder, remove it so it doesn't affect
        // the stats on the Results page
        if (addedFakeExample)
        {
          card.ChosenExampleList = new List<Example>();
        }
      }

      return lines;
    }


    /// <summary>
    /// Format the Definition: Highest Priority field for provided card.
    /// </summary>
    private string formatExpressionRuby(CardInfo card)
    {
      return String.Format("{0}[{1}]", card.Expression, card.Reading);
    }


    /// <summary>
    /// Format the Definition: Highest Priority field for provided card.
    /// </summary>
    private string formatDefinitionHiPri(CardInfo card)
    {
      string defOut = "";

      List<Entry> entryList = card.getEntriesPriority(this.settings.DicList.getEnabledDics());

      if (entryList.Count > 0)
      {
        Entry defEntry = this.getFirstDefinitionEntry(entryList);
        string def = defEntry.Definition;

        def = this.addSourceDicToDefText(defEntry.DicName, FieldEnum.Def) + def;

        if (this.settings.FineTuneOptions.CompactDefs)
        {
          def = def.Replace("<br />", " ").Trim();
        }

        defOut = def;
      }
      else
      {
        defOut = "";
      }

      return defOut;
    }


    /// <summary>
    /// Format the Definition: Highest Priority JJ/JE field for provided card.
    /// </summary>
    private string formatDefinitionType(CardInfo card, DicTypeEnum defType)
    {
      string defOut = "";

      List<Entry> entryList = card.getEntriesPriority(
                   this.settings.DicList.getEnabledDics().getDicsByType(defType));

      if (entryList.Count > 0)
      {
        Entry defEntry = this.getFirstDefinitionEntry(entryList);
        string def = defEntry.Definition;

        def = this.addSourceDicToDefText(defEntry.DicName, defType) + def;

        if (this.settings.FineTuneOptions.CompactDefs)
        {
          def = def.Replace("<br />", " ");
        }

        defOut = def;
      }
      else
      {
        defOut = "";
      }

      return defOut;
    }


    /// <summary>
    /// Format the 'Definition: DICNAME' field for provided card.
    /// </summary>
    private string formatDefinitionDicName(CardInfo card, CardLayoutField field)
    {
      string defOut = "";

      if (card.containsNonNullEntryForDic(field.DicName))
      {
        string def = card.Entries[field.DicName].Definition;

        def = this.addSourceDicToDefText(field.DicName, FieldEnum.DefDicName) + def;

        if (this.settings.FineTuneOptions.CompactDefs)
        {
          def = def.Replace("<br />", " ");
        }

        defOut = def;
      }
      else
      {
        defOut = "";
      }

      return defOut;
    }


    /// <summary>
    /// Format the Example Sentences field for provided card.
    /// </summary>
    private string formatExamples(CardInfo card)
    {
      int count = 0;
      string exampleText = "";

      foreach (Example example in card.ChosenExampleList)
      {
        count++;
        exampleText += this.formatSingleExample(example, true);

        if (count != card.ChosenExampleList.Count)
        {
          exampleText += "<br />";
        }
      }

      return exampleText;
    }


    /// <summary>
    /// Format a single example.
    /// </summary>
    private string formatSingleExample(Example example, bool useStartChar)
    {
      string exampleText = example.Text;
      string startChar = "";

      if (useStartChar)
      {
        startChar = this.settings.FineTuneOptions.ExamplePrependText;
      }

      string[] fields = exampleText.Split('\t');

      if (fields.Length == 2)
      {
        string japText = UtilsFormatting.addPunctuationToJapText(fields[0].Trim());

        if (this.settings.FineTuneOptions.AddRubyToExamples)
        {
          japText = UtilsRuby.addAnkiRubyToText(japText);
        }

        string engText = UtilsFormatting.addPunctuationToEngText(fields[1].Trim());

        exampleText = String.Format("{0} {1}", japText, engText);
      }
      else
      {
        exampleText = UtilsFormatting.addPunctuationToJapText(exampleText);

        if (this.settings.FineTuneOptions.AddRubyToExamples)
        {
          exampleText = UtilsRuby.addAnkiRubyToText(exampleText);
        }
      }

      // Get rid of any left over tabs
      exampleText = exampleText.Replace("\t", "");

      exampleText = String.Format("{0}{1}", startChar, exampleText);

      exampleText += this.addSourceDicToExampleText(example.DicName);

      return exampleText;
    }


    /// <summary>
    /// Format the Example Sentences (no translation) field for provided card.
    /// </summary>
    private string formatExamplesNoTranslation(CardInfo card)
    {
      int count = 0;
      string exampleText = "";

      foreach (Example example in card.ChosenExampleList)
      {
        count++;
        exampleText += this.formatSingleExampleNoTranslation(example, true);

        if (count != card.ChosenExampleList.Count)
        {
          exampleText += "<br />";
        }
      }

      return exampleText;
    }


    /// <summary>
    /// Format a single no translation example.
    /// </summary>
    private string formatSingleExampleNoTranslation(Example example, bool useStartChar)
    {
      string exampleText = "";
      string startChar = "";

      if (useStartChar)
      {
        startChar = this.settings.FineTuneOptions.ExamplePrependText;
      }
      
      int spaceIdx = example.Text.IndexOf("\t");

      if (spaceIdx == -1)
      {
        // No translation in this example, return as is
        exampleText = String.Format("{0}{1}", startChar, example.Text);
      }
      else
      {
        exampleText = String.Format("{0}{1}", startChar, 
          example.Text.Substring(0, spaceIdx).Trim());
      }

      if (this.settings.FineTuneOptions.AddRubyToExamples)
      {
        exampleText = UtilsRuby.addAnkiRubyToText(exampleText);
      }

      exampleText = UtilsFormatting.addPunctuationToJapText(exampleText);

      exampleText += this.addSourceDicToExampleText(example.DicName);

      return exampleText;
    }


    /// <summary>
    /// Format the Example Sentences (translation only) field for provided card.
    /// </summary>
    private string formatExamplesTranslationOnly(CardInfo card)
    {
      int count = 0;
      string exampleText = "";

      foreach (Example example in card.ChosenExampleList)
      {
        if (example.HasTranslation)
        {
          count++;
          exampleText += formatSingleExampleTranslationOnly(example, true);
          　
          if (count != card.ChosenExampleList.Count)
          {
            exampleText += "<br />";
          }
        }
      }

      return exampleText;
    }


    /// <summary>
    /// Format a single translation-only example.
    /// </summary>
    private string formatSingleExampleTranslationOnly(Example example, bool useStartChar)
    {
      string exampleText = "";

      if (example.HasTranslation)
      {
        string startChar = "";

        if (useStartChar)
        {
          startChar = this.settings.FineTuneOptions.ExamplePrependText;
        }

        int spaceIdx = example.Text.IndexOf("\t");

        if (spaceIdx == -1)
        {
          // This example does not have a translation, return blank
          exampleText = "";
        }
        else
        {
          exampleText = String.Format("{0}{1}", 
            startChar, example.Text.Substring(spaceIdx).Trim());

          exampleText = UtilsFormatting.addPunctuationToEngText(exampleText);

          exampleText += this.addSourceDicToExampleText(example.DicName);
        }
      }

      return exampleText;
    }


    /// <summary>
    /// Get the short name of the dic that the provided def/example came from.
    /// </summary>
    private string getSourceDicShortName(string dicName)
    {
      string shortName = "";

      Dic dic = this.settings.DicList.getDicByName(dicName);

      if(dic != null)
      {
        shortName = String.Format("({0})<br />", dic.ShortName);
      }

      return shortName;
    }


    /// <summary>
    /// Get the short name of the dic that the provided example came from.
    /// </summary>
    private string getSourceExampleDicShortName(string dicName)
    {
      string shortName = "";

      Dic dic = this.settings.DicList.getDicByName(dicName);

      if (dic != null)
      {
        if (dic.SeparateExampleDictionary)
        {
          shortName = String.Format(" ({0})", dic.ExamplesName);
        }
        else
        {
          shortName = String.Format(" ({0})", dic.ShortName);
        }
      }

      return shortName;
    }


    /// <summary>
    /// Format the text to add to a definition if the AppendSourceDicToDef fine-tune option is set.
    /// </summary>
    private string addSourceDicToDefText(string dicName, FieldEnum fieldType)
    {
      string shortName = "";

      if (this.settings.FineTuneOptions.PrependSourceDicToDef == FineTune.AppendSource.Yes)
      {
        shortName = this.getSourceDicShortName(dicName);
      }
      else if(this.settings.FineTuneOptions.PrependSourceDicToDef == FineTune.AppendSource.Yes_Non_Primary)
      {
        DicList dicList = new DicList();

        if (fieldType == FieldEnum.Def)
        {
          dicList = this.settings.DicList.getEnabledDics();
        }
        else if (fieldType == FieldEnum.DefJE)
        {
          dicList = this.settings.DicList.getDicsByType(DicTypeEnum.JE).getEnabledDics();
        }
        else if (fieldType == FieldEnum.DefJJ)
        {
          dicList = this.settings.DicList.getDicsByType(DicTypeEnum.JJ).getEnabledDics();
        }

        if ((dicList.Dics.Count > 0) && (dicName != dicList.Dics[0].Name))
        {
          shortName = this.getSourceDicShortName(dicName);
        }
      }

      return shortName;
    }


    private string addSourceDicToDefText(string dicName, DicTypeEnum fieldType)
    {
      string shortName = "";

      if (fieldType == DicTypeEnum.JE)
      {
        shortName = this.addSourceDicToDefText(dicName, FieldEnum.DefJE);
      }
      else
      {
        shortName = this.addSourceDicToDefText(dicName, FieldEnum.DefJJ);
      }

      return shortName;
    }


    /// <summary>
    /// Format the text to add to an example if the AppendSourceDicToExamples fine-tune option is set.
    /// </summary>
    private string addSourceDicToExampleText(string dicName)
    {
      string shortName = "";

      // Note: dicName can be black when generating placeholders
      if (dicName != "")
      {
        if (this.settings.FineTuneOptions.AppendSourceDicToExamples == FineTune.AppendSource.Yes)
        {
          shortName = this.getSourceExampleDicShortName(dicName);
        }
        else if (this.settings.FineTuneOptions.AppendSourceDicToExamples == FineTune.AppendSource.Yes_Non_Primary)
        {
          DicList dicList = this.settings.ExampleDicList.getDicsWithExamplesEnabled();

          if ((dicList.Dics.Count > 0) && (dicName != dicList.Dics[0].Name))
          {
            shortName = this.getSourceExampleDicShortName(dicName);
          }
        }
      }

      return shortName;
    }


    /// <summary>
    /// Get the first entry that contains a definition the provided entry list.
    /// </summary>
    private Entry getFirstDefinitionEntry(List<Entry> entryList)
    {
      Entry defEntry = new Entry();

      foreach (Entry entry in entryList)
      {
        if (entry.Definition != "")
        {
          defEntry = entry;
          break;
        }
      }

      // Definition not found, try finding a secondary definition
      if (defEntry.Definition == "")
      {
        foreach (Entry entry in entryList)
        {
          if (entry.SecondaryDefinition != "")
          {
            defEntry = entry;
            defEntry.Definition = entry.SecondaryDefinition;
            break;
          }
        }
      }

      return defEntry;
    }





  } // ImportFile
}
