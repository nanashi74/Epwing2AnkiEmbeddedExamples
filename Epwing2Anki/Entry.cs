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
  /// Represents a dictionary entry.
  /// </summary>
  [Serializable]
  public class Entry
  {
    private string expression = "";
    private string reading = "";
    private string definition = "";
    private string secondaryDefinition = "";
    private string dicName = "";
    private List<Example> exampleList = new List<Example>();

    /// <summary>
    /// Expression/word.
    /// </summary>
    public string Expression
    {
      get { return this.expression; }
      set { this.expression = value; }
    }

    /// <summary>
    /// Reading of expression.
    /// </summary>
    public string Reading
    {
      get { return this.reading; }
      set { this.reading = value; }
    }

    /// <summary>
    /// Definition.
    /// </summary>
    public string Definition
    {
      get { return this.definition; }
      set { this.definition = value; }
    }

    /// <summary>
    /// Used for Ken5 and Chujiten when user check the "De-prioritize definitions that don't
    /// contain alpha characters (a-z or A-Z)" fine-tune option.
    /// </summary>
    public string SecondaryDefinition
    {
      get { return this.secondaryDefinition; }
      set { this.secondaryDefinition = value; }
    }

    /// <summary>
    /// The name of the dictionary that the expression and reading came form.
    /// </summary>
    public string DicName
    {
      get { return this.dicName; }
      set { this.dicName = value; }
    }

    /// <summary>
    /// List of example sentences.
    /// </summary>
    public List<Example> ExampleList
    {
      get { return this.exampleList; }
      set { this.exampleList = value; }
    }


    /// <summary>
    /// Does this entry have the same reading and expression as other entry?
    /// </summary>
    public bool haveSameReadingAndExpression(Entry otherEntry)
    {
      bool same = false;

      string thisExpression = UtilsFormatting.removeSpecialCharsFromExpression(this.expression);
      string otherExpression = UtilsFormatting.removeSpecialCharsFromExpression(otherEntry.expression);

      if(thisExpression == otherExpression)
      {
        string thisReading = UtilsFormatting.removeSpecialCharsFromReading(this.reading);
        string otherReading = UtilsFormatting.removeSpecialCharsFromReading(otherEntry.reading);

        if (thisReading == otherReading)
        {
          same = true;
        }
      }

      return same;
    }





  }
}
