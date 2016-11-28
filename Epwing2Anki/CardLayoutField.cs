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
  /// Enumeration representing the possible card layout fields.
  /// </summary>
  public enum FieldEnum
  {
    Expression,
    ExpressionRuby,
    Reading,
    Def,
    DefJE,
    DefJJ,
    DefDicName,
    Examples,
    ExamplesNoTranslation,
    ExamplesTranslationOnly,
    Tags,
  }

  /// <summary>
  /// Represents a field in a card layout.
  /// </summary>
  [Serializable]
  public class CardLayoutField : IComparable
  {
    private FieldEnum fieldType;
    private string dicName = "";


    /// <summary>
    /// The type of field.
    /// </summary>
    public FieldEnum Type
    {
      get { return this.fieldType; }
    }

    /// <summary>
    /// If fieldType = DefDicName, the name of dictionary, otherwise blank.
    /// </summary>
    public string DicName
    {
      get { return this.dicName; }
    }


    /// <summary>
    /// Constructor 1.
    /// </summary>
    public CardLayoutField(FieldEnum fieldType)
    {
      this.fieldType = fieldType;
      this.dicName = "";
    }


    /// <summary>
    /// Constructor 2.
    /// </summary>
    public CardLayoutField(FieldEnum fieldType, string dicName)
    {
      this.fieldType = fieldType;
      this.dicName = dicName;
    }


    /// <summary>
    /// Is this field a definition type?
    /// </summary>
    public bool isDefType()
    {
      return ((this.Type == FieldEnum.Def)
        || (this.Type == FieldEnum.DefDicName)
        || (this.Type == FieldEnum.DefJE)
        || (this.Type == FieldEnum.DefJJ));
    }


    /// <summary>
    /// Is this field an expression type?
    /// </summary>
    public bool isExpressionType()
    {
      return ((this.Type == FieldEnum.Expression)
        || (this.Type == FieldEnum.ExpressionRuby));
    }


    /// <summary>
    /// Is this field an examples type?
    /// </summary>
    public bool isExamplesType()
    {
      return ((this.Type == FieldEnum.Examples)
        || (this.Type == FieldEnum.ExamplesNoTranslation)
        || (this.Type == FieldEnum.ExamplesTranslationOnly));
    }

    
    public override string ToString()
    {
      string str = "";

      if (this.fieldType == FieldEnum.Expression)
      {
        str = "Expression";
      }
      else if (this.fieldType == FieldEnum.ExpressionRuby)
      {
        str = "Expression (with ruby)";
      }
      else if (this.fieldType == FieldEnum.Reading)
      {
        str = "Reading";
      }
      else if (this.fieldType == FieldEnum.Def)
      {
        str = "Definition: Highest Priority";
      }
      else if (this.fieldType == FieldEnum.DefJE)
      {
        str = "Definition: Highest Priority J-E";
      }
      else if (this.fieldType == FieldEnum.DefJJ)
      {
        str = "Definition: Highest Priority J-J";
      }
      else if (this.fieldType == FieldEnum.DefDicName)
      {
        str = "Definition: " + dicName;
      }
      else if (this.fieldType == FieldEnum.Examples)
      {
        str = "Example Sentences";
      }
      else if (this.fieldType == FieldEnum.ExamplesNoTranslation)
      {
        str = "Example Sentences (without translation)";
      }
      else if (this.fieldType == FieldEnum.ExamplesTranslationOnly)
      {
        str = "Example Sentences (translation only)";
      }
      else if (this.fieldType == FieldEnum.Tags)
      {
        str = "Tags";
      }

      return str;
    }


    public override bool Equals(object other)
    {
      bool equal = false;

      if (other is CardLayoutField)
      {
        equal = (this.ToString() == ((CardLayoutField)other).ToString());
      }

      return equal;
    }


    public override int GetHashCode()
    {
      return this.fieldType.GetHashCode();
    }


    public int CompareTo(object other)
    {
      return this.ToString().CompareTo(((CardLayoutField)other).ToString());
    }

  } // CardLayoutField 
}
