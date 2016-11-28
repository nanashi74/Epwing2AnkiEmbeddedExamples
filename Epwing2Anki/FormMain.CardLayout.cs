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
  // "Setup Card Layout" tab.
  //

  public partial class FormMain : Form
  {
    /// <summary>
    /// Setup the available layout and card layout lists.
    /// </summary>
    private void setupCardLayout()
    {
      this.checkBoxAppendLineFromWordList.Checked = this.settings.AppendLineFromWordList;
      this.checkBoxExpandExamples.Checked = this.settings.ExpandExamples;

      // Add a field for each dictionary
      foreach (Dic dic in this.settings.DicList.Dics)
      {
        if (!this.settings.AvailableLayout.containsDicName(dic.Name)
          && !this.settings.CardLayout.containsDicName(dic.Name))
        {
          this.settings.AvailableLayout.Fields.Add(new CardLayoutField(FieldEnum.DefDicName, dic.Name));
        }
      }

      this.settings.AvailableLayout.Fields.Sort();

      this.setupCardLayoutListView();
    }


    /// <summary>
    /// Setup the avialable layout and card layout listviews.
    /// </summary>
    private void setupCardLayoutListView()
    {
      // Update Available Fields
      this.listBoxAvailableFields.Items.Clear();

      foreach (CardLayoutField field in this.settings.AvailableLayout.Fields)
      {
        this.listBoxAvailableFields.Items.Add(field);
      }

      // Update Card Layout
      this.listBoxCardLayout.Items.Clear();

      foreach (CardLayoutField field in this.settings.CardLayout.Fields)
      {
        this.listBoxCardLayout.Items.Add(field);
      }

      // Set button enable status
      this.buttonCardLayoutAdd.Enabled = (this.listBoxAvailableFields.Items.Count > 0);
      this.buttonCardLayoutRemove.Enabled = (this.listBoxCardLayout.Items.Count > 0);
      this.buttonCardLayoutMoveUp.Enabled = (this.listBoxCardLayout.Items.Count > 0);
      this.buttonCardLayoutMoveDown.Enabled = (this.listBoxCardLayout.Items.Count > 0);

      // Make sure that one of the lists has a selected item
      if ((this.listBoxAvailableFields.SelectedIndex == -1) &&
        this.listBoxCardLayout.SelectedIndex == -1)
      {
        if (this.listBoxAvailableFields.Items.Count > 0)
        {
          this.listBoxAvailableFields.SelectedIndex = 0;
        }

        if (this.listBoxCardLayout.Items.Count > 0)
        {
          this.listBoxCardLayout.SelectedIndex = 0;
        }
      }
    }


    /// <summary>
    /// Add field to card layout and remove from available fields list.
    /// </summary>
    private void buttonCardLayoutAdd_Click(object sender, EventArgs e)
    {
      int index = this.listBoxAvailableFields.SelectedIndex;

      if (index >= 0)
      {
        CardLayoutField field = this.settings.AvailableLayout.Fields[index];

        this.settings.AvailableLayout.Fields.Remove(field);
        this.settings.CardLayout.Fields.Add(field);

        int newIndex = index;

        // If the last dic was removed, select the dic above
        if (index == this.listBoxAvailableFields.Items.Count - 1)
        {
          newIndex--;
        }

        this.setupCardLayoutListView();

        this.listBoxAvailableFields.SelectedIndex = newIndex;

        this.listBoxCardLayout.SelectedIndex = (this.listBoxCardLayout.Items.Count - 1);
      }
    }


    /// <summary>
    /// Remove field from the card layout and add back tot he available fields list.
    /// </summary>
    private void buttonCardLayoutRemove_Click(object sender, EventArgs e)
    {
      int index = this.listBoxCardLayout.SelectedIndex;

      if (index >= 0)
      {
        CardLayoutField field = this.settings.CardLayout.Fields[index];

        if (this.settings.CardLayout.Fields.Count == 1)
        {
          UtilsMsg.showInfoMsg("You can't remove the only field. \r\n\r\nIf you really want to remove it, add another field first.");
          return;
        }

        if (field.isExpressionType())
        {
          if (this.settings.CardLayout.getExpressionFields().Fields.Count == 1)
          {
            UtilsMsg.showInfoMsg("You can't remove the only Expression field from your card.");
            return;
          }
        }

        if (field.Type == FieldEnum.Reading)
        {
          UtilsMsg.showInfoMsg("You can't remove the Reading field from your card.");
          return;
        }

        if (field.isDefType())
        {
          if (this.settings.CardLayout.getDefFields().Fields.Count == 1)
          {
            UtilsMsg.showInfoMsg("You can't remove the only Definition field from your card.");
            return;
          }
        }

        this.settings.CardLayout.Fields.Remove(field);
        this.settings.AvailableLayout.Fields.Add(field);
        this.settings.AvailableLayout.Fields.Sort();

        int newIndex = index;

        // If the last dic was removed, select the dic above
        if (index == this.listBoxCardLayout.Items.Count - 1)
        {
          newIndex--;
        }

        this.setupCardLayoutListView();

        this.listBoxCardLayout.SelectedIndex = newIndex;

        int availIndex = 0;

        // Select the item that was placed back into the avaiable list
        foreach (CardLayoutField listField in this.listBoxAvailableFields.Items)
        {
          if (listField == field)
          {
            this.listBoxAvailableFields.SelectedIndex = availIndex;
            break;
          }

          availIndex++;
        }
      }
    }


    /// <summary>
    /// Move the field up in the card layout.
    /// </summary>
    private void buttonCardLayoutMoveUp_Click(object sender, EventArgs e)
    {
      // If an item was selected
      if (this.listBoxCardLayout.Items.Count > 0)
      {
        int index = this.listBoxCardLayout.SelectedIndex;

        // If the selected item was not the first item
        if (index > 0)
        {
          // Swap the selected item with the item above
          CardLayoutField temp = this.settings.CardLayout.Fields[index - 1];
          this.settings.CardLayout.Fields[index - 1] = this.settings.CardLayout.Fields[index];
          this.settings.CardLayout.Fields[index] = temp;

          this.setupCardLayoutListView();

          // Reselect the line
          this.listBoxCardLayout.SelectedIndex = index - 1;
        }
      }
    }


    /// <summary>
    /// Move the field down in the card layout.
    /// </summary>
    private void buttonCardLayoutMoveDown_Click(object sender, EventArgs e)
    {
      // If an item was selected
      if (this.listBoxCardLayout.Items.Count > 0)
      {
        int index = this.listBoxCardLayout.SelectedIndex;

        // If the selected item was not the last item
        if (index != this.listBoxCardLayout.Items.Count - 1)
        {
          // Swap the selected item with the item below
          CardLayoutField temp = this.settings.CardLayout.Fields[index + 1];
          this.settings.CardLayout.Fields[index + 1] = this.settings.CardLayout.Fields[index];
          this.settings.CardLayout.Fields[index] = temp;

          this.setupCardLayoutListView();

          // Reselect the line
          this.listBoxCardLayout.SelectedIndex = index + 1;
        }
      }
    }



  }
}