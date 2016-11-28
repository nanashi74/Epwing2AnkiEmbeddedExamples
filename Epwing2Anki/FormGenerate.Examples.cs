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
    private List<Example> userChosenExampleList = new List<Example>();
    private List<Entry> exampleEntryList = new List<Entry>();
    private CardInfo exampleCardInfo = new CardInfo(new WordListItem("", ""));

    /// <summary>
    /// Show the dictionary entries for word in the GUI and allow user to choose one.
    /// </summary>
    private void chooseExamples(List<Example> exampleList, List<Entry> exampleEntryList, CardInfo cardInfo)
    {
      this.exampleCardInfo = cardInfo;
      this.exampleEntryList = exampleEntryList;

      listViewExamples.Items.Clear();

      foreach (Example example in exampleList)
      {
        ListViewItem listItem = new ListViewItem(example.DicName);
        listItem.Tag = example;
        
        if (example.SubDefNumber == Example.NO_SUB_DEF)
        {
          listItem.SubItems.Add("N/A");
        }
        else
        {
          listItem.SubItems.Add(example.SubDefNumber.ToString());
        }

        listItem.SubItems.Add(this.formatExample(example.Text));
        listViewExamples.Items.Add(listItem);
      }

      // Select the first example
      this.listViewExamples.Items[0].Selected = true;

      this.updateAutoChooseExampleButton();
    }


    /// <summary>
    /// Format a single example.
    /// </summary>
    private string formatExample(string exampleText)
    {
      string[] fields = exampleText.Split('\t');

      if (fields.Length == 2)
      {
        string japText = UtilsFormatting.addPunctuationToJapText(fields[0].Trim());
        string engText = UtilsFormatting.addPunctuationToEngText(fields[1].Trim());

        exampleText = String.Format("{0} {1}", japText, engText);
      }
      else
      {
        exampleText = UtilsFormatting.addPunctuationToJapText(exampleText);
      }

      // Get rid of any left over tabs
      exampleText = exampleText.Replace("\t", "");

      return exampleText;
    }



    /// <summary>
    /// Display appropriate definition based on selected item.
    /// </summary>
    private void displayExampleDefinition()
    {
      if (this.listViewExamples.SelectedItems.Count > 0)
      {
        Entry entry = new Entry();

        foreach (Entry exEntry in exampleEntryList)
        {
          if (exEntry.DicName == ((Example)this.listViewExamples.SelectedItems[0].Tag).DicName)
          {
            entry = exEntry;
            break;
          }
        }

        if (this.htmlInfoTemplate == "")
        {
          this.readHtmlInfoTemplate();
        }

        string def = entry.Definition;

        if (this.settings.FineTuneOptions.CompactDefs)
        {
          def = def.Replace("<br />", " ");
        }

        string bodyHtml = String.Format("{0} 【{1}】　『{2}』<br />{3}",
          entry.Expression, entry.Reading, entry.DicName, def);

        string htmlDoc = htmlInfoTemplate.Replace("$Body", bodyHtml);

        this.webBrowserExampleDef.DocumentText = htmlDoc;
      }
    }


    /// <summary>
    /// Magnify the selected example sentence;
    /// </summary>
    private void listViewExamples_SelectedIndexChanged(object sender, EventArgs e)
    {
      if (this.listViewExamples.SelectedItems.Count > 0)
      {
        this.displayExampleDefinition();

        if (this.htmlInfoTemplate == "")
        {
          this.readHtmlInfoTemplate();
        }

        string bodyHtml = this.formatExample(((Example)this.listViewExamples.SelectedItems[0].Tag).Text);

        string htmlDoc = htmlInfoTemplate.Replace("$Body", bodyHtml);

        this.webBrowserExample.DocumentText = htmlDoc;
      }
    }


    /// <summary>
    /// When the user clicks the Done button, save off the checked examples and resume generation.
    /// </summary>
    private void buttonExDone_Click(object sender, EventArgs e)
    {
      this.userChosenExampleList = this.getCheckedExamples();

      // Allow the generate background worker to proceed
      mre.Set();
    }


    /// <summary>
    /// Get the examples that have a check next to them in the list.
    /// </summary>
    private List<Example> getCheckedExamples()
    {
      List<Example> exampleList = new List<Example>();

      foreach (ListViewItem item in this.listViewExamples.CheckedItems)
      {
        exampleList.Add((Example)item.Tag);
      }

      return exampleList;
    }


    /// <summary>
    /// When the auto choose button has been clicked, auto choose examples
    /// </summary>
    private void buttonAutoChooseExamples_Click(object sender, EventArgs e)
    {
      List<Example> checkedExamples = getCheckedExamples();

      List<Example> bestExamples = this.autoChooseExamples(this.exampleCardInfo,
        checkedExamples.Count + this.settings.MaxAutoChooseExamples);

      foreach (Example example in bestExamples)
      {
        if (!checkedExamples.Contains(example))
        {
          checkedExamples.Add(example);

          if (checkedExamples.Count == this.settings.MaxAutoChooseExamples)
          {
            break;
          }
        }
      }

      this.userChosenExampleList = checkedExamples;

      // Allow the generate background worker to proceed
      mre.Set();
    }


    /// <summary>
    /// Update the auto choose button text when an item has been checked/unchecked.
    /// </summary>
    private void listViewExamples_ItemChecked(object sender, ItemCheckedEventArgs e)
    {
      this.updateAutoChooseExampleButton();
    }


    /// <summary>
    /// Update the auto choose button text.
    /// </summary>
    private void updateAutoChooseExampleButton()
    {
      int numChecked = this.listViewExamples.CheckedItems.Count;
      int needToCheck = this.settings.MaxAutoChooseExamples - numChecked;

      this.buttonAutoChooseExamples.Visible = true;

      if (numChecked == 0)
      {
        if (this.settings.MaxAutoChooseExamples == 1)
        {
          this.buttonAutoChooseExamples.Text = String.Format("Auto-choose 1 example");
        }
        else
        {
          this.buttonAutoChooseExamples.Text = String.Format("Auto-choose up to {0} examples",
            this.settings.MaxAutoChooseExamples);
        }
      }
      else if (needToCheck == 1)
      {
        this.buttonAutoChooseExamples.Text = String.Format("Auto-choose 1 more example");
      }
      else if (needToCheck >= 1)
      {
        this.buttonAutoChooseExamples.Text = String.Format("Auto-choose {0} more examples",
          needToCheck);
      }
      else
      {
        this.buttonAutoChooseExamples.Visible = false;
      }
    }


	}
}
