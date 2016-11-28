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
  /// Dialog for changing the fine-tune options.
  /// </summary>
  public partial class FormFineTune : Form
  {
    private FineTune fineTuneOptions = new FineTune();

    /// <summary>
    /// Options to fine-tune things related to dictionaries.
    /// </summary>
    public FineTune FineTuneOptions
    {
      get { return this.fineTuneOptions; }
      set 
      { 
        this.fineTuneOptions = value;
        this.updateGUI();
      }
    }


    /// <summary>
    /// Constructor.
    /// </summary>
    public FormFineTune()
    {
      InitializeComponent();
    }


    /// <summary>
    /// Cancel out of dialog.
    /// </summary>
    private void buttonCancel_Click(object sender, EventArgs e)
    {
      this.DialogResult = DialogResult.Cancel;
      this.Close();
    }


    /// <summary>
    /// OK clicked, save settings and close dialog.
    /// </summary>
    private void buttonOK_Click(object sender, EventArgs e)
    {
      this.fineTuneOptions.CompactDefs = this.checkBoxCompact.Checked;
      this.fineTuneOptions.AddPlaceholders = this.checkBoxAddPlaceholders.Checked;
      this.fineTuneOptions.AddRubyToExamples = this.checkBoxAddRubyToExamples.Checked;
      this.fineTuneOptions.PrependSourceDicToDef = (FineTune.AppendSource)this.comboBoxPrependDicNameToDef.SelectedIndex;
      this.fineTuneOptions.AppendSourceDicToExamples = (FineTune.AppendSource)this.comboBoxAppendDicNameToExamples.SelectedIndex;
      this.fineTuneOptions.ExamplePrependText = this.textBoxExamplePrependText.Text;
      this.fineTuneOptions.EdictNoWordIndicators = this.checkBoxEdictRemoveWordIndicators.Checked;
      this.fineTuneOptions.EdictNoP = this.checkBoxEdictRemoveP.Checked;
      this.fineTuneOptions.JeNoAlphaFallback = this.checkBoxKen5NoAlphaFallback.Checked;
      this.fineTuneOptions.JjKeepExamplesInDef = this.checkBoxJJKeepExamplesInDef.Checked;
      this.fineTuneOptions.JjRemoveSpecialReadingChars = this.checkBoxJJRemoveSpecialCharsFromReading.Checked;
      this.fineTuneOptions.JjFillInExampleBlanksWithWord = this.checkBoxJJFillInExampleBlanksWithWord.Checked;

      this.DialogResult = DialogResult.OK;
      this.Close();
    }


    /// <summary>
    /// Reset all fine-tune settings to their defaults.
    /// </summary>
    private void linkLabelReset_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
    {
      this.FineTuneOptions = new FineTune();
    }


    /// <summary>
    /// Update the GUI from this.fineTuneOptions.
    /// </summary>
    private void updateGUI()
    {
      this.checkBoxCompact.Checked = this.fineTuneOptions.CompactDefs;
      this.checkBoxAddPlaceholders.Checked = this.fineTuneOptions.AddPlaceholders;
      this.checkBoxAddRubyToExamples.Checked = this.fineTuneOptions.AddRubyToExamples;
      this.comboBoxPrependDicNameToDef.SelectedIndex = (int)this.fineTuneOptions.PrependSourceDicToDef;
      this.comboBoxAppendDicNameToExamples.SelectedIndex = (int)this.fineTuneOptions.AppendSourceDicToExamples;
      this.textBoxExamplePrependText.Text = this.fineTuneOptions.ExamplePrependText;
      this.checkBoxEdictRemoveWordIndicators.Checked = this.fineTuneOptions.EdictNoWordIndicators;
      this.checkBoxEdictRemoveP.Checked = this.fineTuneOptions.EdictNoP;
      this.checkBoxKen5NoAlphaFallback.Checked = this.fineTuneOptions.JeNoAlphaFallback;
      this.checkBoxJJKeepExamplesInDef.Checked = this.fineTuneOptions.JjKeepExamplesInDef;
      this.checkBoxJJRemoveSpecialCharsFromReading.Checked = this.fineTuneOptions.JjRemoveSpecialReadingChars;
      this.checkBoxJJFillInExampleBlanksWithWord.Checked = this.fineTuneOptions.JjFillInExampleBlanksWithWord;
    }
  }
}
