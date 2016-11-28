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
  public class InfoMecab
  {
    public string Word { get; set; }
    public string Pos { get; set; } // Part-of-speech
    public string Root { get; set; } // 食べた -> 食べる
    public string Reading { get; set; }

    public InfoMecab()
    {
      this.Word = "";
      this.Pos = "";
      this.Root = "";
      this.Reading = "";
    }

    public InfoMecab(string word, string pos, string root, string reading)
    {
      this.Word = word;
      this.Pos = pos;
      this.Root = root;
      this.Reading = reading;
    }
  }
}
