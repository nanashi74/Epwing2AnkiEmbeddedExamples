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
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Epwing2Anki
{
  /// <summary>
  /// Represents a navigation button.
  /// </summary>
  class NavButton : Button
  {
    bool active = false;

    /// <summary>
    /// Is this button in the selected or "active" state?
    /// </summary>
    public bool Active
    {
      get 
      {
        return active; 
      }

      set 
      {
        bool isActive = value;

        if (isActive)
        {
          this.BackColor = Color.AntiqueWhite;
          this.FlatAppearance.BorderColor = Color.DarkOrange;
          this.FlatAppearance.BorderSize = 3;
        }
        else
        {
          this.BackColor = Color.White;
          this.FlatAppearance.BorderColor = Color.Black;
          this.FlatAppearance.BorderSize = 1;
        }
      }
    }


    /// <summary>
    /// Constructor.
    /// </summary>
    public NavButton(string text)
    {
      this.Text = text;
      this.FlatStyle = FlatStyle.Flat;
      this.BackColor = SystemColors.Control;
      this.Height = 40;
      this.TabStop = false;
    }

  }
}
