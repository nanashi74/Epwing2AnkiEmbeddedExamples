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
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Epwing2Anki
{
  //
  // "Welcome" tab.
  //

  public partial class FormMain : Form
  {
    /// <summary>
    /// Setup the welcome tab.
    /// </summary>
    private void setupWelcome()
    {
      StreamReader reader = new StreamReader(ConstantSettings.TemplatesDir + "welcome.html", Encoding.UTF8);
      string welcomeHtml = reader.ReadToEnd();
      reader.Close();

      welcomeHtml = welcomeHtml.Replace("$BColor",
        String.Format("#{0:X2}{1:X2}{2:X2}",
          SystemColors.Control.R, SystemColors.Control.G, SystemColors.Control.B));

      welcomeHtml = welcomeHtml.Replace("$Dir", ConstantSettings.TemplatesDir);

      this.webBrowserWelcome.DocumentText = welcomeHtml;
    }


  }
}
