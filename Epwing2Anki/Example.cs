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
  /// Represents a single example sentence.
  /// </summary>
  [Serializable]
  public class Example
  {
    /// <summary>
    /// Used for indicating that an example is not attached to a particular sub-definition.
    /// </summary>
    public const int NO_SUB_DEF = 9999;

    private string text = "";
    private int priority = 1;
    private int subDefNumber = 1;
    private string dicName = "";
    private bool hasTranslation = true;


    /// <summary>
    /// The text of the example sentence.
    /// </summary>
    public string Text
    {
      get { return this.text; }
      set { this.text = value; }
    }

    /// <summary>
    /// The priority of this example sentence. Lower number = higher priority.
    /// </summary>
    public int Priority
    {
      get { return this.priority; }
      set { this.priority = value; }
    }

    /// <summary>
    /// The number of the sub-definition that this sentence belongs to.
    /// </summary>
    public int SubDefNumber
    {
      get { return this.subDefNumber; }
      set { this.subDefNumber = value; }
    }

    /// <summary>
    /// The name of the dictionary that this sentence came from.
    /// </summary>
    public string DicName
    {
      get { return this.dicName; }
      set { this.dicName = value; }
    }

    /// <summary>
    /// Does example have a translation?
    /// </summary>
    public bool HasTranslation
    {
      get { return this.hasTranslation; }
      set { this.hasTranslation = value; }
    }


  }
}
