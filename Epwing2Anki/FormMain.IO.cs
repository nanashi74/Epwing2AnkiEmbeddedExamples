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
  // "Setup Inputs/Outputs" tab.
  //

  public partial class FormMain : Form
  {
    /// <summary>
    /// Setup the input/output tab page.
    /// </summary>
    private void setupIOPage()
    {
      this.textBoxWordList.Text = this.settings.InFile;
      this.numericUpDownWordListCol.Value = (Decimal)this.settings.WordListCol;
      this.textBoxOutputFile.Text = this.settings.OutFile;
      this.checkBoxAutoDisambiguate.Checked = this.settings.AutoDisambiguate;
      this.checkBoxAutoChooseExamples.Checked = this.settings.AutoChooseExamples;
      this.numericUpDownMaxExamples.Value = this.settings.MaxAutoChooseExamples;
      this.textBoxTags.Text = this.settings.Tags;
    }


    /// <summary>
    /// Click handler for the word list file chooser button.
    /// </summary>
    private void buttonWordList_Click(object sender, EventArgs e)
    {
      DialogResult result = this.openFileDialogWordList.ShowDialog();

      if (result == DialogResult.OK)
      {
        this.textBoxWordList.Text = this.openFileDialogWordList.FileName;
      }
    }


    /// <summary>
    /// Click handler for the output file chooser button.
    /// </summary>
    private void buttonOutputFile_Click(object sender, EventArgs e)
    {
      DialogResult result = this.saveFileDialogOutputFile.ShowDialog();

      if (result == DialogResult.OK)
      {
        this.textBoxOutputFile.Text = this.saveFileDialogOutputFile.FileName;
      }
    }


    /// <summary>
    /// Bring up the fine-tune dialog.
    /// </summary>
    private void linkLabelFineTune_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
    {
      this.formFineTune.FineTuneOptions = this.settings.FineTuneOptions;

      DialogResult result = this.formFineTune.ShowDialog();

      if (result == DialogResult.OK)
      {
        this.settings.FineTuneOptions = this.formFineTune.FineTuneOptions;
      }
    }


    /// <summary>
    /// When Create Anki Import File! button is clicked,
    /// bring up generate dialog and hide this form until generation is complete.
    /// </summary>
    private void buttonGo_Click(object sender, EventArgs e)
    {
      Logger.Instance.info("buttonGo_Click");

      if (!this.gatherGUI(true))
      {
        return;      
      }

      if (!this.checkForMissingEpwingDics())
      {
        return;
      }

      if (!this.readInputFile())
      {
        return;
      }

      if (!this.checkCardLayoutForSanity())
      {
        return;
      }

      Logger.Instance.writeSettingsToLog(this.settings);

      formGenerate.Settings = this.settings;
      formGenerate.WindowPosition = this.Location;

      this.Hide();

      formGenerate.ShowDialog();

      this.Location = formGenerate.WindowPosition;

      if (formGenerate.DialogResult == DialogResult.OK)
      {
        FormResults formResults = new FormResults(this.settings, formGenerate.Log);
        formResults.WindowPosition = this.Location;
        formResults.ShowDialog();
        this.Location = formResults.WindowPosition;
      }

      this.Show();
    }


    /// <summary>
    /// Check for any enabled EPWING dics that cannot be found.
    /// </summary>
    private bool checkForMissingEpwingDics()
    {
      DicList missingEpwingDics = this.settings.DicList.getMissingEpwingDics();
      bool noneMissing = true;

      if (missingEpwingDics.Dics.Count > 0)
      {
        string msgText = "Unable to find the CATALOGS file for the following EPWING dictionaries:\r\n\r\n";

        foreach (DicEpwing epwingDic in missingEpwingDics.Dics)
        {
          msgText += "●『" + epwingDic.Name + "』\r\n";
        }

        msgText += "\r\nPerhaps they were moved/deleted.";

        UtilsMsg.showErrMsg(msgText);

        noneMissing = false;
      }

      return noneMissing;
    }

    /// <summary>
    /// Check the card layout to help ensure that a card can successfully be generated from it.
    /// </summary>
    private bool checkCardLayoutForSanity()
    {
      bool sane = true;

      if(this.settings.CardLayout.getDefFields().Fields.Count == 1)
      {
        if(this.settings.CardLayout.containsFieldType(FieldEnum.DefJJ)
          && (this.settings.DicList.getDicsByType(DicTypeEnum.JJ).getEnabledDics().Dics.Count == 0))
        {
          UtilsMsg.showErrMsg("You have added \"Definition: Highest Priority J-J\" to your card layout\r\n"
            + "but there are no enabled J-J dictionaries in the list on the Setup Dictionaries page.");
          sane = false;
        }
        else if (this.settings.CardLayout.containsFieldType(FieldEnum.DefJE)
          && (this.settings.DicList.getDicsByType(DicTypeEnum.JE).getEnabledDics().Dics.Count == 0))
        {
          UtilsMsg.showErrMsg("You have added \"Definition: Highest Priority J-E\" to your card layout\r\n"
            + "but there are no enabled J-E dictionaries in the list on the Setup Dictionaries page.");
          sane = false;
        }
      }

      return sane;
    }
    

    /// <summary>
    /// Gather information from the GUI.
    /// </summary>
    private bool gatherGUI(bool validate)
    {
      this.settings.AppendLineFromWordList = this.checkBoxAppendLineFromWordList.Checked;
      this.settings.ExpandExamples = this.checkBoxExpandExamples.Checked;
      this.settings.InFile = this.textBoxWordList.Text.Trim();
      this.settings.WordListCol = (int)this.numericUpDownWordListCol.Value;
      this.settings.OutFile = this.textBoxOutputFile.Text.Trim();
      this.settings.Tags = this.textBoxTags.Text.Trim();
      this.settings.AutoDisambiguate = this.checkBoxAutoDisambiguate.Checked;
      this.settings.AutoChooseExamples = this.checkBoxAutoChooseExamples.Checked;
      this.settings.MaxAutoChooseExamples = (int)this.numericUpDownMaxExamples.Value;

      if (validate)
      {
        if (!File.Exists(this.settings.InFile))
        {
          UtilsMsg.showErrMsg("Please enter a valid word list file.");
          return false;
        }

        if (this.settings.OutFile == "")
        {
          UtilsMsg.showErrMsg("Please enter the Anki import file that you want Epwing2Anki to create.");
          return false;
        }
      }

      return true;
    }


    /// <summary>
    /// Parse word list. Place words in settings.WordList.
    /// </summary>
    private bool readInputFile()
    {
      Dictionary<string, WordListItem> words = new Dictionary<string, WordListItem>(); 

      try
      {
        StreamReader reader = new StreamReader(this.settings.InFile, Encoding.UTF8);
        string line = "";

        while ((line = reader.ReadLine()) != null)
        {
          string[] fields = line.Split(new char[] { '\t' });

          if (fields.Length >= this.settings.WordListCol)
          {
            string word = fields[this.settings.WordListCol - 1].Trim();
            string example = "";

            if (settings.FineTuneOptions.UseEmbeddedExamples)
            {
              // assume the example is in the second column for now
              example = fields[1].Trim();
            }

            // Disallow blanks and duplicates
            if ((word != "") && !words.ContainsKey(word))
            {
              words.Add(word, new WordListItem(word, example, line));
            }
          }
          else
          {
            throw new Exception("");
          }
        }

        reader.Close();

        this.settings.WordList.Clear();
        this.settings.WordList.AddRange(words.Values.ToList());
      }
      catch
      {
        UtilsMsg.showErrMsg("Unable to read the word list file."
          + "\r\n\r\nPerhaps you didn't select the correct word column."
          + "\r\nOr maybe the word list file doesn't exist or is locked.");
        return false;
      }

      return true;
    }


  }
}