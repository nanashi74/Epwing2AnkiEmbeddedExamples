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
  /// Represents the layout of the cards to be generated.
  /// </summary>
  [Serializable]
  public class CardLayout
  {
    private List<CardLayoutField> fields = new List<CardLayoutField>();

    /// <summary>
    /// The fields of the card layout.
    /// </summary>
    public List<CardLayoutField> Fields
    {
      get { return this.fields; }
      set { this.fields = value; }
    }


    /// <summary>
    /// Does the layout contain the specified field?
    /// </summary>
    public bool containsFieldType(FieldEnum fieldEnum)
    {
      bool containsField = false;

      foreach (CardLayoutField field in fields)
      {
        if (field.Type == fieldEnum)
        {
          containsField = true;
          break;
        }
      }

      return containsField;
    }


    /// <summary>
    /// Does the layout contain a defintion field?
    /// </summary>
    public bool containsDefField()
    {
      bool containsField = false;

      foreach (CardLayoutField field in fields)
      {
        if (field.isDefType())
        {
          containsField = true;
          break;
        }
      }

      return containsField;
    }


    /// <summary>
    /// Does the layout contain a particular named dic field.
    /// </summary>
    public bool containsDicName(string dicName)
    {
      bool containsName = false;

      foreach (CardLayoutField field in fields)
      {
        if ((field.Type == FieldEnum.DefDicName) 
          &&  (field.DicName == dicName))
        {
          containsName = true;
          break;
        }
      }

      return containsName;
    }


    /// <summary>
    /// Does the layout contain an example field?
    /// </summary>
    public bool containsExamplesField()
    {
      bool containsField = false;

      foreach (CardLayoutField field in fields)
      {
        if (field.isExamplesType())
        {
          containsField = true;
          break;
        }
      }

      return containsField;
    }


    /// <summary>
    /// Get all of the def fields.
    /// </summary>
    public CardLayout getDefFields()
    {
      CardLayout list = new CardLayout();

      foreach (CardLayoutField field in fields)
      {
        if (field.isDefType())
        {
          list.Fields.Add(field);
        }
      }

      return list;
    }


    /// <summary>
    /// Get all of the expression fields.
    /// </summary>
    public CardLayout getExpressionFields()
    {
      CardLayout list = new CardLayout();

      foreach (CardLayoutField field in fields)
      {
        if (field.isExpressionType())
        {
          list.Fields.Add(field);
        }
      }

      return list;
    }


    /// <summary>
    /// Remove a named dic from the layout.
    /// </summary>
    public void removeDicNameField(string dicName)
    {
      for(int i = 0; i < this.Fields.Count; i++)
      {
        if ((this.Fields[i].Type == FieldEnum.DefDicName)
          && (this.Fields[i].DicName == dicName))
        {
          this.fields.RemoveAt(i);
          break;
        }
      }
    }


    /// <summary>
    /// Remove a field from the layout.
    /// For named dics, use removeDicNameField() instead.
    /// </summary>
    public void removeField(FieldEnum field)
    {
      for (int i = 0; i < this.Fields.Count; i++)
      {
        if (this.Fields[i].Type == field)
        {
          this.fields.RemoveAt(i);
          break;
        }
      }
    }


  } // CardLayout
}
