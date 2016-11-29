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
  public class WordListItem
  {
    private string word = "";
    private string example = "";
    private string rawLine = "";

    /// <summary>
    /// Word of the word list line to use.
    /// </summary>
    public string Word
    {
      get { return this.word; }
      set { this.word = value; }
    }

    /// <summary>
    /// The raw word list line.
    /// </summary>
    public string RawLine
    {
      get { return this.rawLine; }
      set { this.rawLine = value; }
    }

    /// <summary>
    /// An example sentence to use
    /// </summary>
    public string Example
        {
            get { return this.example; }
            set { this.example = value; }
        }

    /// <summary>
    /// Constructor.
    /// </summary>
    public WordListItem(string word, string rawLine)
    {
      this.word = word;
      this.rawLine = rawLine;
      this.example = ""; 
    }

    public WordListItem(string word, string example, string rawLine)
    {
        this.word = word;
        this.example = example;
        this.rawLine = rawLine;
    }
  }
}
