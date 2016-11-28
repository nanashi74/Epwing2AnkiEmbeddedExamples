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
  /// Represents logs and stats related to card generation and import file creation.
  /// </summary>
  public class GenLog
  {
    private int numCardsInImportFile = 0;
    private List<string> events = new List<string>();
    private List<string> noEntry = new List<string>();
    private List<string> noReading = new List<string>();
    private List<string> noDef = new List<string>();
    private List<string> noExamples = new List<string>();


    /// <summary>
    /// The number of cards in the import file.
    /// </summary>
    public int NumCardsInImportFile
    {
      get { return this.numCardsInImportFile; }
      set { this.numCardsInImportFile = value; }
    }

    /// <summary>
    /// Event log.
    /// </summary>
    public List<string> Events
    {
      get { return this.events; }
      set { this.events = value; }
    }

    /// <summary>
    /// The words that did not have an entry in any dictionary and were skipped.
    /// </summary>
    public List<string> NoEntry
    {
      get { return this.noEntry; }
      set { this.noEntry = value; }
    }

    /// <summary>
    /// The words that do not have a reading.
    /// </summary>
    public List<string> NoReading
    {
      get { return this.noReading; }
      set { this.noReading = value; }
    }

    /// <summary>
    /// The words that do not have a definition.
    /// </summary>
    public List<string> NoDef
    {
      get { return this.noDef; }
      set { this.noDef = value; }
    }

    /// <summary>
    /// The words that do not have any exampleSentences.
    /// </summary>
    public List<string> NoExamples
    {
      get { return this.noExamples; }
      set { this.noExamples = value; }
    }


    /// <summary>
    /// Update the no entry, no reading, no defintion, and no example lists 
    /// from the given card list.
    /// </summary>
    public void addFromCardList(List<CardInfo> cardList, DicList dicList)
    {
      foreach (CardInfo card in cardList)
      {
        // If the card contains no entries
        if (card.getEntriesPriority(dicList).Count == 0)
        {
          noEntry.Add(card.Word.Word);
        }
        else
        {
          if (card.Reading == "")
          {
            this.noReading.Add(card.Word.Word);
          }

          if (!card.containsDefinition())
          {
            this.noDef.Add(card.Word.Word);
          }

          if (card.ChosenExampleList.Count == 0)
          {
            this.NoExamples.Add(card.Word.Word);
          }
        }
      }
    }


  } // ImportStatus
}
