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
using System.Threading;
using System.Windows.Forms;

namespace Epwing2Anki
{
  public partial class FormGenerate : Form
  {
    private string htmlInfoTemplate = "";
    private int selectedEntryIdx = 1;
    private int numDisambiguateEntries = 0;

    /// <summary>
    /// Show the dictionary entries for word in the GUI and allow user to choose one.
    /// </summary>
    private void disambiguate(List<Entry> entryList)
    {
      if (htmlInfoTemplate == "")
      {
        this.readHtmlInfoTemplate();
      }

      this.numDisambiguateEntries = entryList.Count;

      string bodyHtml = "";

      int count = 0;

      foreach (Entry entry in entryList)
      {
        string numExamplesStr = "";

        if (this.settings.CardLayout.containsExamplesField())
        {
          numExamplesStr = String.Format(" (# of examples: {0})", entry.ExampleList.Count);
        }

        string def = entry.Definition;

        if (this.settings.FineTuneOptions.CompactDefs)
        {
          def = def.Replace("<br />", " ");
        }

        bodyHtml += String.Format("<a href='{0}'>Entry {1}:</a><br />{2} 【{3}】{4}<br />{5}<hr />",
          count, count + 1, entry.Expression, entry.Reading, numExamplesStr, def);

        count++;
      }

      string htmlDoc = htmlInfoTemplate.Replace("$Body", bodyHtml);

      this.webBrowserDisambiguate.DocumentText = htmlDoc;
    }


    /// <summary>
    /// Read the html template into memory.
    /// </summary>
    private void readHtmlInfoTemplate()
    {
      StreamReader reader = new StreamReader(ConstantSettings.TemplatesDir + "info.html", Encoding.UTF8);
      htmlInfoTemplate = reader.ReadToEnd();
      reader.Close();
    }


    /// <summary>
    /// When a definition is clicked, save its index and allow this page to be close and
    /// processing to continue.
    /// </summary>
    private void webBrowserDisambiguate_Navigating(object sender, WebBrowserNavigatingEventArgs e)
    {
      // If navigated here by selecting a link (instead of setting the page)
      if (e.Url.OriginalString != "about:blank")
      {
        string numStr = e.Url.OriginalString.Replace("about:", "");

        // Windows XP might attach a "blank" in addition to the link, so remove it.
        // It doesn't seem to happen on all XP PCs, so maybe it's something else.
        if (numStr.StartsWith("blank"))
        {
          numStr = numStr.Substring(5);
        }

        int num = -1;

        try
        {
          num = Convert.ToInt32(numStr);
        }
        catch (Exception e1)
        {
          num = 0;
          Logger.Instance.error("webBrowserDisambiguate_Navigating: " + e1);
        }

        e.Cancel = true;

        this.setEntryIdxAndContinue(num);
      }
    }

    
    /// <summary>
    /// When 1-9 is pressed in the web browser, select the corresponding entry
    /// </summary>
    private void webBrowserDisambiguate_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
    {
      int num = -1;

      if (e.KeyCode == Keys.D1)
      {
        num = 0;
      }
      else if (e.KeyCode == Keys.D2)
      {
        num = 1;
      }
      else if (e.KeyCode == Keys.D3)
      {
        num = 2;
      }
      else if (e.KeyCode == Keys.D4)
      {
        num = 3;
      }
      else if (e.KeyCode == Keys.D5)
      {
        num = 4;
      }
      else if (e.KeyCode == Keys.D6)
      {
        num = 5;
      }
      else if (e.KeyCode == Keys.D7)
      {
        num = 6;
      }
      else if (e.KeyCode == Keys.D8)
      {
        num = 7;
      }
      else if (e.KeyCode == Keys.D9)
      {
        num = 8;
      }

      if ((num != -1) && (num < this.numDisambiguateEntries))
      {
        this.setEntryIdxAndContinue(num);
      }
    }


    /// <summary>
    /// Set the index of the entry that was selected and continue generation.
    /// </summary>
    private void setEntryIdxAndContinue(int entryIdx)
    {
      this.selectedEntryIdx = entryIdx;

      // Allow the generate background worker to proceed
      mre.Set();
    }


  }
}
