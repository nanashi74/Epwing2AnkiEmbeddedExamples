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
  public enum DicTypeEnum
  {
    JE,
    JJ
  }

  public enum DicExamplesEnum
  {
    YES,    // Yes, dic has example sentences.
    NO,     // No,  dic does not have example sentences.
    J_ONLY  // Dic does not have translations for example setences.
  }

  public enum DicSourceTypeEnum
  {
    EPWING,
    WEB,
    DEFAULT
  }


  /// <summary>
  /// Abstract dictionary base type.
  /// </summary>
  [Serializable]
  public abstract class Dic
  {
    private string name = "";
    private string shortName = "";
    private string nameEng = "";
    private string examplesName = "";
    private string examplesNotes = "";
    private DicTypeEnum dicType = DicTypeEnum.JE;
    private DicSourceTypeEnum dicSourceType = DicSourceTypeEnum.DEFAULT;
    private DicExamplesEnum examplesType = DicExamplesEnum.NO;
    private bool enabled = true;
    private bool examplesEnabled = true;
    private bool separateExampleDictionary = false;

    /// <summary>
    /// The name of the dictionary.
    /// </summary>
    public string Name
    {
      get { return this.name; }
      set { this.name = value; }
    }

    /// <summary>
    /// The short name of the dictionary.
    /// </summary>
    public string ShortName
    {
      get { return this.shortName; }
      set { this.shortName = value; }
    }

    /// <summary>
    /// The name of the dictinoary in English.
    /// </summary>
    public string NameEng
    {
      get { return this.nameEng; }
      set { this.nameEng = value; }
    }

    /// <summary>
    /// The name of the example dictionary. If not given, Name will be used.
    /// </summary>
    public string ExamplesName
    {
      get 
      {
        string exName = name;

        if (this.examplesName != "")
        {
          exName = this.examplesName;
        }

        return exName; 
      }
      set { this.examplesName = value; }
    }


    /// <summary>
    /// Notes related to the example sentences.
    /// </summary>
    public string ExamplesNotes
    {
      get { return this.examplesNotes; }
      set { this.examplesNotes = value; }
    }


    /// <summary>
    /// The type of dictionary: J-E or J-J.
    /// </summary>
    public DicTypeEnum DicType
    {
      get { return this.dicType; }
      set { this.dicType = value; }
    }

    /// <summary>
    /// The source of dictionary: web, EPWING, default.
    /// </summary>
    public DicSourceTypeEnum SourceType
    {
      get { return this.dicSourceType; }
      set { this.dicSourceType = value; }
    }

    /// <summary>
    /// Info about the example sentences present in this dictionary.
    /// </summary>
    public DicExamplesEnum ExamplesType
    {
      get { return this.examplesType; }
      set { this.examplesType = value; }
    }

    /// <summary>
    /// Are the definitions from this dictionary enabled?
    /// </summary>
    public bool Enabled
    {
      get { return this.enabled; }
      set { this.enabled = value; }
    }

    /// <summary>
    /// Are the example sentences from this dictionary enabled?
    /// </summary>
    public bool ExamplesEnabled
    {
      get { return this.examplesEnabled; }
      set { this.examplesEnabled = value; }
    }


    /// <summary>
    /// Do the examples come from a separate database?
    /// For example, EDICT uses Tatoeba.
    /// </summary>
    public bool SeparateExampleDictionary
    {
      get { return this.separateExampleDictionary; }
      set { this.separateExampleDictionary = value; }
    }


    public override bool Equals(object other)
    {
      bool equal = false;

      if (other is Dic)
      {
        equal = (this.name == ((Dic)other).name);
      }

      return equal;
    }


    public override int GetHashCode()
    {
      return this.Name.GetHashCode();
    }


    /// <summary>
    /// Lookup the given word.
    /// If includeExamples is set, also gather example sentences.
    /// </summary>
    public abstract List<Entry> lookup(string word, bool includeExamples, FineTune fineTune);

    



    
  }
}
