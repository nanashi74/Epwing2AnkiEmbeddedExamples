//  Copyright (C) 2012 Christopher Brochtrup, Nanashi74
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

namespace Epwing2Anki
{
  /// <summary>
  /// Represents an embedded pseudo-dictionary that can serve as a source of example sentences
  /// </summary>
  [Serializable]
  public class DicEmbedded : Dic
  {

    /// <summary>
    /// Consructor.
    /// </summary>
    public DicEmbedded()
    {
      this.Name = "Embedded";
      this.NameEng = "Embedded";
      this.ExamplesName = "Embedded";
      this.ShortName = "Embedded";
      this.ExamplesNotes = "Embedded in the file";
      this.SourceType = DicSourceTypeEnum.DEFAULT;
      this.DicType = DicTypeEnum.JJ;
      this.ExamplesType = DicExamplesEnum.J_ONLY;
      this.SeparateExampleDictionary = false;
    }

    public override List<Entry> lookup(string word, bool includeExamples, FineTune fineTune)
    {
      // can't actually be used for lookups
      return null;
    }
  }
}
