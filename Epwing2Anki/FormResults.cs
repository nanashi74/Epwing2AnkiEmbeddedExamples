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
using System.IO;
using System.Diagnostics;

namespace Epwing2Anki
{
  public partial class FormResults : Form
  {
    private Settings settings = null;
    private GenLog genLog = null;
    private Point windowPosition = new Point(0, 0);

    /// <summary>
    /// The position of this dialog.
    /// </summary>
    public Point WindowPosition
    {
      get { return this.Location; }
      set { this.windowPosition = value; }
    }

    /// <summary>
    /// Constructor.
    /// </summary>
    public FormResults(Settings settings, GenLog genLog)
    {
      InitializeComponent();

      this.settings = settings;
      this.genLog = genLog;
    }


    /// <summary>
    /// When the form loads, display the results.
    /// </summary>
    private void FormResults_Load(object sender, EventArgs e)
    {
      // Setting location before showDialog() doesn't seem to work
      this.Location = this.windowPosition;

      this.displayResults();
    }


    /// <summary>
    /// Display the results in the web browser.
    /// </summary>
    private void displayResults()
    {
      string html = this.loadTemplate();

      html = html.Replace("$BColor", UtilsCommon.toHtmlColor(SystemColors.Control));

      string htmlBody = "";

      htmlBody += String.Format("Total number of words in original word list: {0}<br /><br />",
        this.settings.WordList.Count);

      htmlBody += String.Format("Location of the Anki import file that was just created: <br />{0}<br /><br />",
        this.settings.OutFile);
 
      htmlBody += String.Format("Number of cards in Anki import file: {0}<br /><br />",
        (this.genLog.NumCardsInImportFile));

      if (this.genLog.NoEntry.Count > 0)
      {
        htmlBody += this.formTextArea(
          String.Format("Words that could not be found in any dictionary ({0} total):", this.genLog.NoEntry.Count),
          this.genLog.NoEntry);
      }

      if (this.settings.CardLayout.containsFieldType(FieldEnum.Reading) && (this.genLog.NoReading.Count > 0))
      {
        htmlBody += this.formTextArea(
          String.Format("Cards with no reading ({0} total):", this.genLog.NoReading.Count),
          this.genLog.NoReading);
      }

      if (this.settings.CardLayout.containsDefField() && (this.genLog.NoDef.Count > 0))
      {
        htmlBody += this.formTextArea(
          String.Format("Cards with no definition ({0} total):", this.genLog.NoDef.Count),
          this.genLog.NoDef);
      }

      if (this.settings.CardLayout.containsExamplesField() 
        && (this.genLog.NoExamples.Count > 0))
      {
        htmlBody += this.formTextArea(
          String.Format("Cards with no example sentences ({0} total):", this.genLog.NoExamples.Count),
          this.genLog.NoExamples);
      }

      Logger.Instance.startSection("Results");
      Logger.Instance.var(htmlBody);
      Logger.Instance.endSection("Results");

      htmlBody += this.formTextArea("Event Log:", this.genLog.Events, 20);

      html = html.Replace("$Body", htmlBody);

      this.webBrowserResults.DocumentText = html;
    }


    /// <summary>
    /// Format an HTML text area (including title).
    /// </summary>
    private string formTextArea(string title, List<string> lines)
    {
      int numRows = 5;

      if (lines.Count < numRows)
      {
        numRows = lines.Count;
      }

      return this.formTextArea(title, lines, numRows);
    }

    private string formTextArea(string title, List<string> lines, int rows)
    {
      string text = "";

      foreach (string line in lines)
      {
        text += String.Format("{0}\n", line);
      }

      return String.Format("{0}<br /><textarea rows='{1}' readonly='readonly'>{2}</textarea><br /><br />",
        title, rows, text);
    }


    /// <summary>
    /// Load the HTML results template from file.
    /// </summary>
    private string loadTemplate()
    {
      StreamReader reader = new StreamReader(ConstantSettings.TemplatesDir + "results.html", Encoding.UTF8);
      string html = reader.ReadToEnd();
      reader.Close();

      return html;
    }


    /// <summary>
    /// When the done button is pressed, close this form.
    /// </summary>
    private void buttonDone_Click(object sender, EventArgs e)
    {
      this.Close();
    }


    /// <summary>
    /// When the open output dir button is clicked, do it.
    /// </summary>
    private void buttonOpenDir_Click(object sender, EventArgs e)
    {
      string outDir = Path.GetDirectoryName(this.settings.OutFile);

      if (Directory.Exists(outDir))
      {
        Process.Start("explorer.exe", String.Format(@"""{0}""", outDir));
      }
    }


    /// <summary>
    /// When the open Anki import file button is clicked, do it.
    /// </summary>
    private void buttonOpenFile_Click(object sender, EventArgs e)
    {
      if (File.Exists(this.settings.OutFile))
      {
        Process.Start(String.Format(@"""{0}""", this.settings.OutFile));
      }
    }




  }
}
