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

namespace Epwing2Anki
{
  /// <summary>
  /// Represents a list of dics.
  /// </summary>
  [Serializable]
  public class DicList
  {
    List<Dic> dics = new List<Dic>();


    /// <summary>
    /// The dic list.
    /// </summary>
    public List<Dic> Dics
    {
      get { return this.dics; }
      set { this.dics = value; }
    }


    /// <summary>
    /// Does the list contain an enabled dic?
    /// </summary>
    public bool containsEnabledDic()
    {
      bool contains = false;

      foreach (Dic dic in this.Dics)
      {
        if (dic.Enabled)
        {
          contains = true;
          break;
        }
      }

      return contains;
    }


    /// <summary>
    /// Does the list contains a dic that has examples enabled?
    /// </summary>
    public bool containsDicWithExamplesEnabled()
    {
      bool contains = false;

      foreach (Dic dic in this.Dics)
      {
        if (dic.ExamplesEnabled)
        {
          contains = true;
          break;
        }
      }

      return contains;
    }


    /// <summary>
    /// Does the list contains a dic with the given name?
    /// </summary>
    public bool containsDicName(string dicName)
    {
      bool contains = false;

      foreach (Dic dic in this.Dics)
      {
        if (dic.Name == dicName)
        {
          contains = true;
          break;
        }
      }

      return contains;
    }


    /// <summary>
    /// If the specified dic exists in the list return it,
    /// otherwise return null.
    /// </summary>
    public Dic getDicByName(string name)
    {
      foreach (Dic dic in this.Dics)
      {
        if (dic.Name == name)
        {
          return dic;
        }
      }

      return null;
    }


    /// <summary>
    /// Get list of dics based on type: J-E or J-J.
    /// </summary>
    public DicList getDicsByType(DicTypeEnum type)
    {
      DicList dicList = new DicList();

      foreach (Dic dic in this.Dics)
      {
        if (dic.DicType == type)
        {
          dicList.Dics.Add(dic);
        }
      }

      return dicList;
    }


    /// <summary>
    /// Get list of dics that are enabled.
    /// </summary>
    public DicList getEnabledDics()
    {
      DicList dicList = new DicList();

      foreach (Dic dic in this.Dics)
      {
        if (dic.Enabled)
        {
          dicList.Dics.Add(dic);
        }
      }

      return dicList;
    }


    /// <summary>
    /// Get list of dics that are enabled.
    /// </summary>
    public DicList getDicsWithExamplesEnabled()
    {
      DicList dicList = new DicList();

      foreach (Dic dic in this.Dics)
      {
        if (dic.ExamplesEnabled)
        {
          dicList.Dics.Add(dic);
        }
      }

      return dicList;
    }


    /// <summary>
    /// Get the EPWING dics whose CATALOGS file cannot be found (maybe it got deleted).
    /// </summary>
    public DicList getMissingEpwingDics()
    {
      DicList dicList = new DicList();

      foreach (Dic dic in this.Dics)
      {
        if (dic.Enabled 
          && dic.SourceType == DicSourceTypeEnum.EPWING
          && !File.Exists(((DicEpwing)dic).CatalogsFile))
        {
          dicList.Dics.Add(dic);
        }
      }

      return dicList;
    }


  }
}
