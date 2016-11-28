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

namespace Epwing2Anki
{
  /// <summary>
  /// Represents all the information gathered for a single card.
  /// </summary>
  public class CardInfo
  {
    private WordListItem word = new WordListItem("", "");

    // Key = dictionary name
    private Dictionary<string, Entry> entries = new Dictionary<string, Entry>();

    private string expression = "";
    private string reading = "";

    private List<Example> chosenExampleList = new List<Example>();


    /// <summary>
    /// The original word list item.
    /// </summary>
    public WordListItem Word
    {
      get { return this.word; }
      set { this.word = value; }
    }

    /// <summary>
    /// C# dictionary of entries from different dics.
    /// </summary>
    public Dictionary<string, Entry> Entries
    {
      get { return this.entries; }
      set { this.entries = value; }
    }

    /// <summary>
    /// The expression to use.
    /// </summary>
    public string Expression
    {
      get { return this.expression; }
      set { this.expression = value; }
    }

    /// <summary>
    /// The reading to use.
    /// </summary>
    public string Reading
    {
      get { return this.reading; }
      set { this.reading = value; }
    }


    /// <summary>
    /// The examples chosen for this card.
    /// </summary>
    public List<Example> ChosenExampleList
    {
      get { return this.chosenExampleList; }
      set { this.chosenExampleList = value; }
    }


    /// <summary>
    /// Constructor.
    /// </summary>
    public CardInfo(WordListItem word)
    {
      this.word = word;
    }


    /// <summary>
    /// Does this card contain an entry for the given dictionary.
    /// </summary>
    public bool containsDicName(string dicName)
    {
      return this.entries.ContainsKey(dicName);
    }


    /// <summary>
    /// Does this card contain any non-null entries?
    /// </summary>
    public bool containsNonNullEntry()
    {
      bool found = false;

      foreach (Entry entry in this.entries.Values)
      {
        if (entry != null)
        {
          found = true;
          break;
        }
      }

      return found;
    }


    /// <summary>
    /// Does this card contain a non-null entry from the given dictionary?
    /// </summary>
    public bool containsNonNullEntryForDic(string dicName)
    {
      return (this.containsDicName(dicName) && (this.entries[dicName] != null));
    }


    /// <summary>
    /// Does the card have at least one entry with a definition?
    /// </summary>
    public bool containsDefinition()
    {
      bool hasDef = false;

      foreach (Entry entry in entries.Values)
      {
        if ((entry != null) && (entry.Definition != ""))
        {
          hasDef = true;
          break;
        }
      }

      return hasDef;
    }


    /// <summary>
    /// Does this card contain any example sentences?
    /// </summary>
    public bool containsExampleSentences()
    {
      bool hasExamples = false;

      foreach (Entry entry in entries.Values)
      {
        if ((entry != null) && (entry.ExampleList.Count > 0))
        {
          hasExamples = true;
          break;
        }
      }

      return hasExamples;
    }


    /// <summary>
    /// Get all example sentences based on dic list order.
    /// </summary>
    public List<Example> getExamples(DicList dicList)
    {
      List<Example> exampleList = new List<Example>();

      foreach (Dic dic in dicList.Dics)
      {
        if (dic.ExamplesEnabled 
          && this.containsDicName(dic.Name) 
          && (this.entries[dic.Name] != null))
        {
          exampleList.AddRange(entries[dic.Name].ExampleList);
        }
      }

      return exampleList;
    }


    /// <summary>
    /// Get all example sentences based on dic list order in seperate lists.
    /// </summary>
    public List<List<Example>> getExamplesSeparate(DicList dicList)
    {
      List<List<Example>> exampleList = new List<List<Example>>();

      foreach (Dic dic in dicList.Dics)
      {
        if (dic.ExamplesEnabled
          && this.containsDicName(dic.Name)
          && (this.entries[dic.Name] != null))
        {
          exampleList.Add(entries[dic.Name].ExampleList);
        }
      }

      return exampleList;
    }


    /// <summary>
    /// Get all examples in lists seperated by dic. Examples from higher priority dics appear first.
    /// The examples in each list are sorted by:
    /// 1) Example sub-def
    /// 2) Example priority
    /// </summary>
    public List<List<Example>> getExamplesSeparatePriority(DicList dicList)
    {
      List<List<Example>> completeExampleList = new List<List<Example>>();
      List<List<Example>> seperateExampleList = this.getExamplesSeparate(dicList);

      foreach (List<Example> exampleList in seperateExampleList)
      {
        List<Example> sortedExampleList =
          (from example in exampleList
           where example.Text != ""
           orderby example.SubDefNumber, example.Priority
           select example).ToList();

        completeExampleList.Add(sortedExampleList);
      }

      return completeExampleList;
    }


    /// <summary>
    /// Get all examples in one big list sorted first by
    /// 1) Highest priority dic
    /// 2) Example sub-def
    /// 3) Example priority
    /// </summary>
    public List<Example> getExamplesPriority(DicList dicList)
    {
      List<Example> completeExampleList = new List<Example>();
      List<List<Example>> separateExampleList = this.getExamplesSeparate(dicList);

      foreach (List<Example> exampleList in separateExampleList)
      {
        List<Example> sortedExampleList =
          (from example in exampleList
           where example.Text != ""
           orderby example.SubDefNumber, example.Priority
           select example).ToList();

        completeExampleList.AddRange(sortedExampleList);
      }

      return completeExampleList;
    }


    /// <summary>
    /// Get all enabled, non-null entries that come from dics in dicList (in order).
    /// </summary>
    public List<Entry> getEntriesPriority(DicList dicList)
    {
      List<Entry> entryList = new List<Entry>();

      foreach (Dic dic in dicList.Dics)
      {
        if (dic.Enabled
          && this.entries.ContainsKey(dic.Name)
          && (this.entries[dic.Name] != null))
        {
          entryList.Add(this.entries[dic.Name]);
        }
      }

      return entryList;
    }


    /// <summary>
    /// Get the dics from the provided list that don't have an entry in this card info.
    /// </summary>
    public DicList getDicsNotInEntries(DicList dicList)
    {
      DicList outDicList = new DicList();

      foreach (Dic dic in dicList.Dics)
      {
        if (!this.entries.ContainsKey(dic.Name))
        {
          outDicList.Dics.Add(dic);
        }
      }

      return outDicList;
    }





  }
}
