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
using System.Runtime.InteropServices;

namespace Epwing2Anki
{
  public partial class FormGenerate : Form
  {
    private Settings settings = null;
    private GenLog genLog = new GenLog();

    // true = background worker has finished/canceled and form may close
    private bool okayToClose = false;
    private Point windowPosition = new Point(0, 0);


    /// <summary>
    /// Represents one of the possible current tab pages.
    /// </summary>
    private enum Page
    {
      Progress,
      Disambiguate,
      Examples
    }


    /// <summary>
    /// The settings.
    /// </summary>
    public Settings Settings
    {
      get { return this.settings; }
      set { this.settings = value; }
    }

    /// <summary>
    /// The position of this dialog.
    /// </summary>
    public Point WindowPosition
    {
      get { return this.Location; }
      set { this.windowPosition = value; }
    }

    /// <summary>
    /// Get generation log.
    /// </summary>
    public GenLog Log
    {
      get { return this.genLog; }
    }


    /// <summary>
    /// Constructor.
    /// </summary>
    public FormGenerate()
    {
      InitializeComponent();
    }


    /// <summary>
    /// When form loads, start the card generation process.
    /// </summary>
    private void FormChoose_Load(object sender, EventArgs e)
    {
      // Setting location before showDialog() doesn't seem to work
      this.Location = this.windowPosition;

      this.okayToClose = false;
      this.setPage(Page.Progress);
      this.startGenerateCards();
    }


    /// Setup tab related stuff.
    /// </summary>
    private void setupTabs()
    {
      foreach (TabPage tab in this.tabControlMain.TabPages)
      {
        // Set the tab pages to the normal control color
        tab.BackColor = SystemColors.Control;
      }

      this.setPage(0);
    }


    /// <summary>
    /// Set the current tab page.
    /// </summary>
    private void setPage(Page page)
    {
      // Switch to the associated tab
      this.tabControlMain.SelectedIndex = (int)page;

      this.Text = String.Format("{0} - {1}",
        this.tabControlMain.SelectedTab.Text, UtilsAssembly.Title);

      this.textBoxTitle.Text = this.tabControlMain.SelectedTab.Text;

      // Set help text
      if (page == Page.Progress)
      {
        this.textBoxHelp.Text = "";
        this.buttonBack.Visible = false;
      }
      else if (page == Page.Disambiguate)
      {
        this.textBoxHelp.Text
          = "Multiple entries were found for this word. Choose an entry "
          + "by clicking the appropriate link.";

        if (this.manualWordList.Count > 0)
        {
          this.buttonBack.Visible = true;
        }
      }
      else
      {
        this.textBoxHelp.Text = "Select the example sentences to use for this word.";

        if (this.manualWordList.Count > 0)
        {
          this.buttonBack.Visible = true;
        }
      }
    }


    /// <summary>
    /// Write an event to the event log in the progress tab.
    /// </summary>
    private void writeEvent(string text)
    {
      string eventText = text.Trim();

      this.Invoke((MethodInvoker)delegate
      {
        // Setting the text this way prevents a flicker effect
        this.textBoxEvents.SelectionStart = this.textBoxEvents.Text.Length;
        this.textBoxEvents.SelectedText = eventText + "\r\n";

        this.textBoxEvents.ScrollToCaret();
      });

      genLog.Events.Add(eventText);

      Logger.Instance.info(eventText);
      Logger.Instance.flush();
    }


    /// <summary>
    /// When the form is about to close, cancel the background worker
    /// </summary>
    private void FormGenerate_FormClosing(object sender, FormClosingEventArgs e)
    {
      // If the background worker has not yet signaled that it has completed
      if (!this.okayToClose)
      {
        // Prevent this dialog from closing until the background worker has completed/stopped.
        e.Cancel = true;

        // Cancel the background worker
        this.bw.CancelAsync();

        // Unblock background worker if it is waiting for user input
        mre.Set();
      }
      else
      {
        // Reset
        this.okayToClose = false;
      }
    }


    /// <summary>
    /// Close the form when the cancel button is clicked.
    /// </summary>
    private void buttonCancel_Click(object sender, EventArgs e)
    {
      this.Close();
    }


    /// <summary>
    /// Undo previous entry/example selection.
    /// </summary>
    private void buttonBack_Click(object sender, EventArgs e)
    {
      // Select dummy 
      this.selectedEntryIdx = 0;
      this.userChosenExampleList = new List<Example>();

      // Set flag indicating the an undo is requested
      this.undoLastChoice = true;

      // Allow the generate background worker to proceed
      mre.Set();
    }

 

   

  

  }
}
