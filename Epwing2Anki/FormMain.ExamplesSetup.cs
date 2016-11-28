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
  //
  // "Setup Examples" tab.
  //

  public partial class FormMain : Form
  {
    /// <summary>
    /// Setup the examples listview.
    /// </summary>
    private void setupExampleList()
    {
      this.listViewSetupExamples.Items.Clear();

      int count = 0;

      // Add the dics to the listview
      foreach (Dic dic in this.settings.ExampleDicList.Dics)
      {
        count++;

        string[] itemCols = new String[5];
        itemCols[0] = count.ToString();

        if (dic.ExamplesEnabled)
        {
          itemCols[1] = "Yes";
        }
        else
        {
          itemCols[1] = "No";
        }

        itemCols[2] = dic.ExamplesName;
        itemCols[3] = this.sourceType2Str(dic.SourceType);
        itemCols[4] = dic.ExamplesNotes;

        ListViewItem item = new ListViewItem(itemCols);

        if (!dic.ExamplesEnabled)
        {
          item.ForeColor = Color.Red;
        }

        this.listViewSetupExamples.Items.Add(item);
      }

      // Select the first item
      this.selectSetupExamplesListViewItem(0);
    }


    /// <summary>
    /// Remove an example sentences dictionary.
    /// </summary>
    private void removeExampleDic(Dic dic)
    {
      this.settings.ExampleDicList.Dics.Remove(dic);

      this.setupExampleList();
    }


    /// <summary>
    /// Add an dictionary to use for example sentences.
    /// </summary>
    private void addExampleDic(Dic dic)
    {
      if(!this.settings.ExampleDicList.Dics.Contains(dic))
      {
        this.settings.ExampleDicList.Dics.Add(dic);
      }

      this.setupExampleList();
    }


    /// <summary>
    /// Select an item in the setup examples listview.
    /// </summary>
    private void selectSetupExamplesListViewItem(int index)
    {
      // Range check
      if ((index >= 0) && (index <= this.listViewSetupExamples.Items.Count - 1))
      {
        // Remove existing selections
        foreach (ListViewItem item in this.listViewSetupExamples.Items)
        {
          item.Selected = false;
        }

        this.listViewSetupExamples.Focus();
        this.listViewSetupExamples.Items[index].Selected = true;
        this.listViewSetupExamples.EnsureVisible(index);
      }
    }


    /// <summary>
    /// Increase priority of selected example dic.
    /// </summary>
    private void buttonSetupExamplesMoveUp_Click(object sender, EventArgs e)
    {
      // If an item was selected
      if (this.listViewSetupExamples.SelectedIndices.Count > 0)
      {
        int index = this.listViewSetupExamples.SelectedIndices[0];

        // If the selected item was not the first item
        if (index > 0)
        {
          // Swap the selected item with the item above
          Dic temp = this.settings.ExampleDicList.Dics[index - 1];
          this.settings.ExampleDicList.Dics[index - 1] = this.settings.ExampleDicList.Dics[index];
          this.settings.ExampleDicList.Dics[index] = temp;

          this.setupExampleList();

          // Reselect the line
          this.selectSetupExamplesListViewItem(index - 1);
        }
      }
    }


    /// <summary>
    /// Decrease priority of selected example dic.
    /// </summary>
    private void buttonSetupExamplesMoveDown_Click(object sender, EventArgs e)
    {
      // If an item was selected
      if (this.listViewSetupExamples.SelectedIndices.Count > 0)
      {
        int index = this.listViewSetupExamples.SelectedIndices[0];

        // If the selected item was not the last item
        if (index != this.listViewSetupExamples.Items.Count - 1)
        {
          // Swap the selected item with the item below
          Dic temp = this.settings.ExampleDicList.Dics[index + 1];
          this.settings.ExampleDicList.Dics[index + 1] = this.settings.ExampleDicList.Dics[index];
          this.settings.ExampleDicList.Dics[index] = temp;

          this.setupExampleList();

          // Reselect the line
          this.selectSetupExamplesListViewItem(index + 1);
        }
      }
    }


    /// <summary>
    /// Enable/disable the selected example dic.
    /// </summary>
    private void buttonSetupExamplesEnable_Click(object sender, EventArgs e)
    {
      // If an item was selected
      if (this.listViewSetupExamples.SelectedIndices.Count > 0)
      {
        int index = this.listViewSetupExamples.SelectedIndices[0];

        // Range check
        if ((index >= 0) && (index <= this.listViewSetupExamples.Items.Count - 1))
        {
          bool enabledState = this.settings.ExampleDicList.Dics[index].ExamplesEnabled;
          this.settings.ExampleDicList.Dics[index].ExamplesEnabled = !enabledState;

          if (!this.settings.ExampleDicList.containsDicWithExamplesEnabled())
          {
            UtilsMsg.showInfoMsg("At least one example sentence dictionary must be enabled." 
              + "\r\n\r\n(Even if you don't want to include example sentences in your card layout).");

            // Set the dic back to enabled
            this.settings.ExampleDicList.Dics[index].ExamplesEnabled = true;
          }
          else
          {
            this.setupExampleList();
            this.selectSetupExamplesListViewItem(index);
          }
        }
      }
    }


    /// <summary>
    /// When the selected index changes see if the buttons should be e grayed out.
    /// </summary>
    private void listViewSetupExamples_SelectedIndexChanged(object sender, EventArgs e)
    {
      //bool buttonEnable = (this.listViewSetupExamples.SelectedIndices.Count > 0) 
      //  && (this.listViewSetupExamples.Items.Count > 1);

      //this.buttonSetupExamplesMoveUp.Enabled = buttonEnable;
      //this.buttonSetupExamplesMoveDown.Enabled = buttonEnable;
      //this.buttonSetupExamplesEnable.Enabled = buttonEnable;
    }


    


  }
}