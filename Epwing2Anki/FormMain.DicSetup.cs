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
  // "Setup Dictionaries" tab.
  //


  public partial class FormMain : Form
  {
    /// <summary>
    /// Convert DicTypeEnum to a string.
    /// </summary>
    private string dicType2Str(DicTypeEnum dicType)
    {
      string str = "J-E";

      if (dicType == DicTypeEnum.JJ)
      {
        str = "J-J";
      }

      return str;
    }


    /// <summary>
    /// Convert DicExamplesEnum to a string.
    /// </summary>
    private string dicExamples2Str(DicExamplesEnum dicExamples)
    {
      string str = "No";

      if (dicExamples == DicExamplesEnum.YES)
      {
        str = "Yes";
      }
      else if(dicExamples == DicExamplesEnum.J_ONLY)
      {
        str = "Japanese only";
      }

      return str;
    }


    /// <summary>
    /// Convert DicSourceTypeEnum to a string.
    /// </summary>
    private string sourceType2Str(DicSourceTypeEnum dicSourceType)
    {
      string str = "Default";

      if (dicSourceType == DicSourceTypeEnum.EPWING)
      {
        str = "EPWING";
      }
      else if (dicSourceType == DicSourceTypeEnum.WEB)
      {
        str = "Web";
      }

      return str;
    }


    /// <summary>
    /// Setup dictionary listview from settings.DicList.
    /// </summary>
    private void setupDicList()
    {
      this.listViewDic.Items.Clear();

      int count = 0;

      // Add the dics to the listview
      foreach (Dic dic in this.settings.DicList.Dics)
      {
        count++;

        string[] itemCols = new String[6];
        itemCols[0] = count.ToString();

        if (dic.Enabled)
        {
          itemCols[1] = "Yes";
        }
        else
        {
          itemCols[1] = "No";
        }

        itemCols[2] = dic.Name;
        itemCols[3] = this.sourceType2Str(dic.SourceType);
        itemCols[4] = this.dicType2Str(dic.DicType);
        itemCols[5] = this.dicExamples2Str(dic.ExamplesType);

        ListViewItem item = new ListViewItem(itemCols);

        if (!dic.Enabled)
        {
          item.ForeColor = Color.Red;
        }

        this.listViewDic.Items.Add(item);
      }

      // Select the first item
      this.selectDicListViewItem(0);

      // Update possibile dictionaries in the card layout tab
      this.setupCardLayout();
    }


    /// <summary>
    /// Select an item in the dictionary listview.
    /// </summary>
    private void selectDicListViewItem(int index)
    {
      // Range check
      if ((index >= 0) && (index <= this.listViewDic.Items.Count - 1))
      {
        // Remove existing selections
        foreach (ListViewItem item in this.listViewDic.Items)
        {
          item.Selected = false;
        }

        this.listViewDic.Focus();
        this.listViewDic.Items[index].Selected = true;
        this.listViewDic.EnsureVisible(index);
      }
    }


    /// <summary>
    /// Allow user to add an EPWING dictionary to the dic list.
    /// </summary>
    private void buttonAddEpwingDic_Click(object sender, EventArgs e)
    {
      DialogResult result = openFileDialogAddEpwingDic.ShowDialog();

      if (result == DialogResult.OK)
      {
        DicEpwing dic = DicEpwing.createEpwingDic(openFileDialogAddEpwingDic.FileName);

        if (dic == null)
        {
          UtilsMsg.showInfoMsg("Sorry, this EPWING dictionary is not supported yet.");
        }
        else
        {
          if (!this.settings.DicList.containsDicName(dic.Name))
          {
            this.settings.DicList.Dics.Add(dic);
            this.addExampleDic(dic);
          }
          else
          {
            UtilsMsg.showInfoMsg(String.Format("{0} has already been added.", dic.Name));
          }
        }
      }

      this.setupDicList();
    }


    /// <summary>
    /// Increase priority of selected dictionary.
    /// </summary>
    private void buttonDicUp_Click(object sender, EventArgs e)
    {
      // If an item was selected
      if (this.listViewDic.SelectedIndices.Count > 0)
      {
        int index = this.listViewDic.SelectedIndices[0];

        // If the selected item was not the first item
        if (index > 0)
        {
          // Swap the selected item with the item above
          Dic temp = this.settings.DicList.Dics[index - 1];
          this.settings.DicList.Dics[index - 1] = this.settings.DicList.Dics[index];
          this.settings.DicList.Dics[index] = temp;

          this.setupDicList();

          // Reselect the line
          this.selectDicListViewItem(index - 1);
        }
      }
    }


    /// <summary>
    /// Decrease priority of selected dictionary.
    /// </summary>
    private void buttonDicDown_Click(object sender, EventArgs e)
    {
      // If an item was selected
      if (this.listViewDic.SelectedIndices.Count > 0)
      {
        int index = this.listViewDic.SelectedIndices[0];

        // If the selected item was not the last item
        if (index != this.listViewDic.Items.Count - 1)
        {
          // Swap the selected item with the item below
          Dic temp = this.settings.DicList.Dics[index + 1];
          this.settings.DicList.Dics[index + 1] = this.settings.DicList.Dics[index];
          this.settings.DicList.Dics[index] = temp;

          this.setupDicList();

          // Reselect the line
          this.selectDicListViewItem(index + 1);
        }
      }
    }


    /// <summary>
    /// Remove the selected dictionary from the dic list.
    /// </summary>
    private void buttonDicRemove_Click(object sender, EventArgs e)
    {
      // If an item was selected
      if (this.listViewDic.SelectedIndices.Count > 0)
      {
        int index = this.listViewDic.SelectedIndices[0];

        int newIndex = index;

        // If the last dic was removed, select the dic above
        if (index == this.listViewDic.Items.Count - 1)
        {
          newIndex--;
        }

        // Prevent removal of EDICT
        if (this.settings.DicList.Dics[index].Name == "EDICT")
        {
          UtilsMsg.showInfoMsg("The default EDICT dictionary cannot be removed.");
          return;
        }

        this.removeDicFromCardLayout(this.settings.DicList.Dics[index].Name);

        this.removeExampleDic(this.settings.DicList.Dics[index]);
        this.settings.DicList.Dics.RemoveAt(index);
        
        // Enable the last dictionary if it is disabled
        if (this.settings.DicList.Dics.Count == 1)
        {
          this.settings.DicList.Dics[0].Enabled = true;
        }

        this.setupDicList();

        // Select another dic
        this.selectDicListViewItem(newIndex);
      }
    }


    /// <summary>
    /// Remove named dic from available fields and card layout.
    /// </summary>
    private void removeDicFromCardLayout(string dicName)
    {
      this.settings.AvailableLayout.removeDicNameField(dicName);
      this.settings.CardLayout.removeDicNameField(dicName);

      // If the card no longer contains a definition field, add one.
      if (!this.settings.CardLayout.containsDefField())
      {
        this.settings.AvailableLayout.removeField(FieldEnum.Def);
        this.settings.CardLayout.Fields.Add(new CardLayoutField(FieldEnum.Def));
      }

      this.setupCardLayoutListView();
    }


    /// <summary>
    /// Enabled/disable the selected dic.
    /// </summary>
    private void buttonDicEnable_Click(object sender, EventArgs e)
    {
      // If an item was selected
      if (this.listViewDic.SelectedIndices.Count > 0)
      {
        int index = this.listViewDic.SelectedIndices[0];

        // Range check
        if ((index >= 0) && (index <= this.listViewDic.Items.Count - 1))
        {
          bool enabledState = this.settings.DicList.Dics[index].Enabled;
          this.settings.DicList.Dics[index].Enabled = !enabledState;

          if (!this.settings.DicList.containsEnabledDic())
          {
            UtilsMsg.showInfoMsg("At least one dictionary must be enabled.");

            // Set the dic back to enabled
            this.settings.DicList.Dics[index].Enabled = true;
          }
          else
          {
            this.setupDicList();
            this.selectDicListViewItem(index);
          }
        }
      }
    }


    /// <summary>
    /// When the selected index changes see if the buttons should be grayed out.
    /// </summary>
    private void listViewDic_SelectedIndexChanged(object sender, EventArgs e)
    {
      //bool buttonEnable = (this.listViewDic.SelectedIndices.Count > 0)
      //  && (this.listViewDic.Items.Count > 1);

      //this.buttonDicUp.Enabled = buttonEnable;
      //this.buttonDicDown.Enabled = buttonEnable;
      //this.buttonDicEnable.Enabled = buttonEnable;
      //this.buttonDicRemove.Enabled = buttonEnable;
    }


    /// <summary>
    /// Displays a list of supported EPWING dictionaries.
    /// </summary>
    private void linkLabelSupportedDictionaries_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
    {
      UtilsMsg.showInfoMsg(
        "The following EPWING dictionaries are supported:\r\n\r\n"
        + "●『研究社　新和英大辞典　第５版』\r\n(Kenkyusha Shin Waei Daijiten 5th Edition [J-E])"
        + "\r\n\r\n"
        + "●『研究社　新英和・和英中辞典』\r\n(Kenkyusha Shin Eiwa-Waei Chujiten [J-E])"
        + "\r\n\r\n"
        + "●『大辞林 第2版』/『三省堂　スーパー大辞林』\r\n(Daijirin 2nd Edition [J-J])"
        + "\r\n\r\n"
        + "●『広辞苑第六版』\r\n(Kojien 6th Edition [J-J])"
        + "\r\n\r\n"
        + "●『大辞泉』\r\n(Daijisen [J-J])"
        + "\r\n\r\n"
        + "●『明鏡国語辞典』\r\n(Meikyo Kokugojiten [J-J])"
        );
    }

  }
}
