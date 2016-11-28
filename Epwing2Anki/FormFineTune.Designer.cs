namespace Epwing2Anki
{
  partial class FormFineTune
  {
    /// <summary>
    /// Required designer variable.
    /// </summary>
    private System.ComponentModel.IContainer components = null;

    /// <summary>
    /// Clean up any resources being used.
    /// </summary>
    /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
    protected override void Dispose(bool disposing)
    {
      if (disposing && (components != null))
      {
        components.Dispose();
      }
      base.Dispose(disposing);
    }

    #region Windows Form Designer generated code

    /// <summary>
    /// Required method for Designer support - do not modify
    /// the contents of this method with the code editor.
    /// </summary>
    private void InitializeComponent()
    {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormFineTune));
            this.buttonOK = new System.Windows.Forms.Button();
            this.buttonCancel = new System.Windows.Forms.Button();
            this.checkBoxKen5NoAlphaFallback = new System.Windows.Forms.CheckBox();
            this.groupBoxKen5Only = new System.Windows.Forms.GroupBox();
            this.groupBoxEdictOnly = new System.Windows.Forms.GroupBox();
            this.checkBoxEdictRemoveP = new System.Windows.Forms.CheckBox();
            this.checkBoxEdictRemoveWordIndicators = new System.Windows.Forms.CheckBox();
            this.groupBoxJJOnly = new System.Windows.Forms.GroupBox();
            this.checkBoxJJFillInExampleBlanksWithWord = new System.Windows.Forms.CheckBox();
            this.checkBoxJJRemoveSpecialCharsFromReading = new System.Windows.Forms.CheckBox();
            this.checkBoxJJKeepExamplesInDef = new System.Windows.Forms.CheckBox();
            this.groupBoxGeneral = new System.Windows.Forms.GroupBox();
            this.labelAppendDicNameToExamples = new System.Windows.Forms.Label();
            this.labelPrependDicNameToDef = new System.Windows.Forms.Label();
            this.comboBoxAppendDicNameToExamples = new System.Windows.Forms.ComboBox();
            this.comboBoxPrependDicNameToDef = new System.Windows.Forms.ComboBox();
            this.labelExamplePrependText = new System.Windows.Forms.Label();
            this.textBoxExamplePrependText = new System.Windows.Forms.TextBox();
            this.checkBoxAddRubyToExamples = new System.Windows.Forms.CheckBox();
            this.checkBoxAddPlaceholders = new System.Windows.Forms.CheckBox();
            this.checkBoxCompact = new System.Windows.Forms.CheckBox();
            this.linkLabelReset = new System.Windows.Forms.LinkLabel();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.groupBoxEmbedded = new System.Windows.Forms.GroupBox();
            this.checkBoxEmbeddedUse = new System.Windows.Forms.CheckBox();
            this.groupBoxKen5Only.SuspendLayout();
            this.groupBoxEdictOnly.SuspendLayout();
            this.groupBoxJJOnly.SuspendLayout();
            this.groupBoxGeneral.SuspendLayout();
            this.groupBoxEmbedded.SuspendLayout();
            this.SuspendLayout();
            // 
            // buttonOK
            // 
            this.buttonOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonOK.Location = new System.Drawing.Point(272, 489);
            this.buttonOK.Name = "buttonOK";
            this.buttonOK.Size = new System.Drawing.Size(75, 23);
            this.buttonOK.TabIndex = 0;
            this.buttonOK.Text = "OK";
            this.buttonOK.UseVisualStyleBackColor = true;
            this.buttonOK.Click += new System.EventHandler(this.buttonOK_Click);
            // 
            // buttonCancel
            // 
            this.buttonCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonCancel.Location = new System.Drawing.Point(353, 489);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new System.Drawing.Size(75, 23);
            this.buttonCancel.TabIndex = 0;
            this.buttonCancel.Text = "Cancel";
            this.buttonCancel.UseVisualStyleBackColor = true;
            this.buttonCancel.Click += new System.EventHandler(this.buttonCancel_Click);
            // 
            // checkBoxKen5NoAlphaFallback
            // 
            this.checkBoxKen5NoAlphaFallback.AutoSize = true;
            this.checkBoxKen5NoAlphaFallback.Location = new System.Drawing.Point(6, 19);
            this.checkBoxKen5NoAlphaFallback.Name = "checkBoxKen5NoAlphaFallback";
            this.checkBoxKen5NoAlphaFallback.Size = new System.Drawing.Size(353, 17);
            this.checkBoxKen5NoAlphaFallback.TabIndex = 1;
            this.checkBoxKen5NoAlphaFallback.Text = "De-prioritize definitions that don\'t contain alpha characters (a-z or A-Z)";
            this.checkBoxKen5NoAlphaFallback.UseVisualStyleBackColor = true;
            // 
            // groupBoxKen5Only
            // 
            this.groupBoxKen5Only.Controls.Add(this.checkBoxKen5NoAlphaFallback);
            this.groupBoxKen5Only.Location = new System.Drawing.Point(15, 267);
            this.groupBoxKen5Only.Name = "groupBoxKen5Only";
            this.groupBoxKen5Only.Size = new System.Drawing.Size(415, 43);
            this.groupBoxKen5Only.TabIndex = 2;
            this.groupBoxKen5Only.TabStop = false;
            this.groupBoxKen5Only.Text = "Specific to J-E Dictionaries";
            // 
            // groupBoxEdictOnly
            // 
            this.groupBoxEdictOnly.Controls.Add(this.checkBoxEdictRemoveP);
            this.groupBoxEdictOnly.Controls.Add(this.checkBoxEdictRemoveWordIndicators);
            this.groupBoxEdictOnly.Location = new System.Drawing.Point(15, 195);
            this.groupBoxEdictOnly.Name = "groupBoxEdictOnly";
            this.groupBoxEdictOnly.Size = new System.Drawing.Size(415, 66);
            this.groupBoxEdictOnly.TabIndex = 3;
            this.groupBoxEdictOnly.TabStop = false;
            this.groupBoxEdictOnly.Text = "Specific to EDICT";
            // 
            // checkBoxEdictRemoveP
            // 
            this.checkBoxEdictRemoveP.AutoSize = true;
            this.checkBoxEdictRemoveP.Location = new System.Drawing.Point(6, 42);
            this.checkBoxEdictRemoveP.Name = "checkBoxEdictRemoveP";
            this.checkBoxEdictRemoveP.Size = new System.Drawing.Size(224, 17);
            this.checkBoxEdictRemoveP.TabIndex = 0;
            this.checkBoxEdictRemoveP.Text = "Remove \"popular\" indicator [example: (P)]";
            this.checkBoxEdictRemoveP.UseVisualStyleBackColor = true;
            // 
            // checkBoxEdictRemoveWordIndicators
            // 
            this.checkBoxEdictRemoveWordIndicators.AutoSize = true;
            this.checkBoxEdictRemoveWordIndicators.Location = new System.Drawing.Point(6, 19);
            this.checkBoxEdictRemoveWordIndicators.Name = "checkBoxEdictRemoveWordIndicators";
            this.checkBoxEdictRemoveWordIndicators.Size = new System.Drawing.Size(244, 17);
            this.checkBoxEdictRemoveWordIndicators.TabIndex = 0;
            this.checkBoxEdictRemoveWordIndicators.Text = "Remove word type indicators [example: (v1,n)]";
            this.checkBoxEdictRemoveWordIndicators.UseVisualStyleBackColor = true;
            // 
            // groupBoxJJOnly
            // 
            this.groupBoxJJOnly.Controls.Add(this.checkBoxJJFillInExampleBlanksWithWord);
            this.groupBoxJJOnly.Controls.Add(this.checkBoxJJRemoveSpecialCharsFromReading);
            this.groupBoxJJOnly.Controls.Add(this.checkBoxJJKeepExamplesInDef);
            this.groupBoxJJOnly.Location = new System.Drawing.Point(15, 316);
            this.groupBoxJJOnly.Name = "groupBoxJJOnly";
            this.groupBoxJJOnly.Size = new System.Drawing.Size(415, 90);
            this.groupBoxJJOnly.TabIndex = 3;
            this.groupBoxJJOnly.TabStop = false;
            this.groupBoxJJOnly.Text = "Specific to J-J Dictionaries";
            // 
            // checkBoxJJFillInExampleBlanksWithWord
            // 
            this.checkBoxJJFillInExampleBlanksWithWord.AutoSize = true;
            this.checkBoxJJFillInExampleBlanksWithWord.Location = new System.Drawing.Point(6, 65);
            this.checkBoxJJFillInExampleBlanksWithWord.Name = "checkBoxJJFillInExampleBlanksWithWord";
            this.checkBoxJJFillInExampleBlanksWithWord.Size = new System.Drawing.Size(247, 17);
            this.checkBoxJJFillInExampleBlanksWithWord.TabIndex = 0;
            this.checkBoxJJFillInExampleBlanksWithWord.Text = "Fill in example sentence blanks with expression";
            this.toolTip1.SetToolTip(this.checkBoxJJFillInExampleBlanksWithWord, "Example:\r\n▲無罪を___する。  --->  ▲無罪を確信する。");
            this.checkBoxJJFillInExampleBlanksWithWord.UseVisualStyleBackColor = true;
            // 
            // checkBoxJJRemoveSpecialCharsFromReading
            // 
            this.checkBoxJJRemoveSpecialCharsFromReading.AutoSize = true;
            this.checkBoxJJRemoveSpecialCharsFromReading.Location = new System.Drawing.Point(6, 42);
            this.checkBoxJJRemoveSpecialCharsFromReading.Name = "checkBoxJJRemoveSpecialCharsFromReading";
            this.checkBoxJJRemoveSpecialCharsFromReading.Size = new System.Drawing.Size(247, 17);
            this.checkBoxJJRemoveSpecialCharsFromReading.TabIndex = 0;
            this.checkBoxJJRemoveSpecialCharsFromReading.Text = "Remove the \'‐\' and \'･\' characters from readings";
            this.checkBoxJJRemoveSpecialCharsFromReading.UseVisualStyleBackColor = true;
            // 
            // checkBoxJJKeepExamplesInDef
            // 
            this.checkBoxJJKeepExamplesInDef.AutoSize = true;
            this.checkBoxJJKeepExamplesInDef.Location = new System.Drawing.Point(6, 19);
            this.checkBoxJJKeepExamplesInDef.Name = "checkBoxJJKeepExamplesInDef";
            this.checkBoxJJKeepExamplesInDef.Size = new System.Drawing.Size(172, 17);
            this.checkBoxJJKeepExamplesInDef.TabIndex = 0;
            this.checkBoxJJKeepExamplesInDef.Text = "Keep examples in the definition";
            this.checkBoxJJKeepExamplesInDef.UseVisualStyleBackColor = true;
            // 
            // groupBoxGeneral
            // 
            this.groupBoxGeneral.Controls.Add(this.labelAppendDicNameToExamples);
            this.groupBoxGeneral.Controls.Add(this.labelPrependDicNameToDef);
            this.groupBoxGeneral.Controls.Add(this.comboBoxAppendDicNameToExamples);
            this.groupBoxGeneral.Controls.Add(this.comboBoxPrependDicNameToDef);
            this.groupBoxGeneral.Controls.Add(this.labelExamplePrependText);
            this.groupBoxGeneral.Controls.Add(this.textBoxExamplePrependText);
            this.groupBoxGeneral.Controls.Add(this.checkBoxAddRubyToExamples);
            this.groupBoxGeneral.Controls.Add(this.checkBoxAddPlaceholders);
            this.groupBoxGeneral.Controls.Add(this.checkBoxCompact);
            this.groupBoxGeneral.Location = new System.Drawing.Point(12, 12);
            this.groupBoxGeneral.Name = "groupBoxGeneral";
            this.groupBoxGeneral.Size = new System.Drawing.Size(415, 177);
            this.groupBoxGeneral.TabIndex = 3;
            this.groupBoxGeneral.TabStop = false;
            this.groupBoxGeneral.Text = "General";
            // 
            // labelAppendDicNameToExamples
            // 
            this.labelAppendDicNameToExamples.AutoSize = true;
            this.labelAppendDicNameToExamples.Location = new System.Drawing.Point(4, 123);
            this.labelAppendDicNameToExamples.Name = "labelAppendDicNameToExamples";
            this.labelAppendDicNameToExamples.Size = new System.Drawing.Size(256, 13);
            this.labelAppendDicNameToExamples.TabIndex = 11;
            this.labelAppendDicNameToExamples.Text = "Append short name of source dictionary to examples:";
            this.toolTip1.SetToolTip(this.labelAppendDicNameToExamples, resources.GetString("labelAppendDicNameToExamples.ToolTip"));
            // 
            // labelPrependDicNameToDef
            // 
            this.labelPrependDicNameToDef.AutoSize = true;
            this.labelPrependDicNameToDef.Location = new System.Drawing.Point(3, 93);
            this.labelPrependDicNameToDef.Name = "labelPrependDicNameToDef";
            this.labelPrependDicNameToDef.Size = new System.Drawing.Size(257, 13);
            this.labelPrependDicNameToDef.TabIndex = 11;
            this.labelPrependDicNameToDef.Text = "Prepend short name of source dictionary to definition:";
            this.toolTip1.SetToolTip(this.labelPrependDicNameToDef, resources.GetString("labelPrependDicNameToDef.ToolTip"));
            // 
            // comboBoxAppendDicNameToExamples
            // 
            this.comboBoxAppendDicNameToExamples.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxAppendDicNameToExamples.FormattingEnabled = true;
            this.comboBoxAppendDicNameToExamples.Items.AddRange(new object[] {
            "No",
            "Yes",
            "Yes (if dic is not primary)"});
            this.comboBoxAppendDicNameToExamples.Location = new System.Drawing.Point(263, 120);
            this.comboBoxAppendDicNameToExamples.Name = "comboBoxAppendDicNameToExamples";
            this.comboBoxAppendDicNameToExamples.Size = new System.Drawing.Size(146, 21);
            this.comboBoxAppendDicNameToExamples.TabIndex = 10;
            // 
            // comboBoxPrependDicNameToDef
            // 
            this.comboBoxPrependDicNameToDef.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxPrependDicNameToDef.FormattingEnabled = true;
            this.comboBoxPrependDicNameToDef.Items.AddRange(new object[] {
            "No",
            "Yes",
            "Yes (if dic is not primary)"});
            this.comboBoxPrependDicNameToDef.Location = new System.Drawing.Point(263, 90);
            this.comboBoxPrependDicNameToDef.Name = "comboBoxPrependDicNameToDef";
            this.comboBoxPrependDicNameToDef.Size = new System.Drawing.Size(146, 21);
            this.comboBoxPrependDicNameToDef.TabIndex = 10;
            // 
            // labelExamplePrependText
            // 
            this.labelExamplePrependText.AutoSize = true;
            this.labelExamplePrependText.Location = new System.Drawing.Point(3, 153);
            this.labelExamplePrependText.Name = "labelExamplePrependText";
            this.labelExamplePrependText.Size = new System.Drawing.Size(166, 13);
            this.labelExamplePrependText.TabIndex = 9;
            this.labelExamplePrependText.Text = "Text to place in front of examples:";
            // 
            // textBoxExamplePrependText
            // 
            this.textBoxExamplePrependText.Location = new System.Drawing.Point(175, 150);
            this.textBoxExamplePrependText.Name = "textBoxExamplePrependText";
            this.textBoxExamplePrependText.Size = new System.Drawing.Size(75, 20);
            this.textBoxExamplePrependText.TabIndex = 8;
            // 
            // checkBoxAddRubyToExamples
            // 
            this.checkBoxAddRubyToExamples.AutoSize = true;
            this.checkBoxAddRubyToExamples.Location = new System.Drawing.Point(6, 65);
            this.checkBoxAddRubyToExamples.Name = "checkBoxAddRubyToExamples";
            this.checkBoxAddRubyToExamples.Size = new System.Drawing.Size(265, 17);
            this.checkBoxAddRubyToExamples.TabIndex = 7;
            this.checkBoxAddRubyToExamples.Text = "Add Anki-style ruby/furigana to example sentences";
            this.toolTip1.SetToolTip(this.checkBoxAddRubyToExamples, "Example:\r\n▲努力の限りを尽くす。  -->  ▲努力[どりょく]の限[かぎ]りを尽[つ]くす。");
            this.checkBoxAddRubyToExamples.UseVisualStyleBackColor = true;
            // 
            // checkBoxAddPlaceholders
            // 
            this.checkBoxAddPlaceholders.AutoSize = true;
            this.checkBoxAddPlaceholders.Location = new System.Drawing.Point(6, 42);
            this.checkBoxAddPlaceholders.Name = "checkBoxAddPlaceholders";
            this.checkBoxAddPlaceholders.Size = new System.Drawing.Size(307, 17);
            this.checkBoxAddPlaceholders.TabIndex = 7;
            this.checkBoxAddPlaceholders.Text = "Add placeholders in import file for words that were not found";
            this.toolTip1.SetToolTip(this.checkBoxAddPlaceholders, "Also, when the \"Create a separate card for each example sentence\" option on\r\nthe " +
        "Setup Card Layout page is checked, a placeholder will be created for entries\r\nth" +
        "at don\'t have example sentences.");
            this.checkBoxAddPlaceholders.UseVisualStyleBackColor = true;
            // 
            // checkBoxCompact
            // 
            this.checkBoxCompact.AutoSize = true;
            this.checkBoxCompact.Location = new System.Drawing.Point(6, 19);
            this.checkBoxCompact.Name = "checkBoxCompact";
            this.checkBoxCompact.Size = new System.Drawing.Size(318, 17);
            this.checkBoxCompact.TabIndex = 7;
            this.checkBoxCompact.Text = "Compact definitions (place the entire definition on a single line)";
            this.checkBoxCompact.UseVisualStyleBackColor = true;
            // 
            // linkLabelReset
            // 
            this.linkLabelReset.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.linkLabelReset.AutoSize = true;
            this.linkLabelReset.Location = new System.Drawing.Point(12, 494);
            this.linkLabelReset.Name = "linkLabelReset";
            this.linkLabelReset.Size = new System.Drawing.Size(43, 13);
            this.linkLabelReset.TabIndex = 4;
            this.linkLabelReset.TabStop = true;
            this.linkLabelReset.Text = "reset all";
            this.linkLabelReset.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabelReset_LinkClicked);
            // 
            // toolTip1
            // 
            this.toolTip1.AutoPopDelay = 19000;
            this.toolTip1.InitialDelay = 500;
            this.toolTip1.ReshowDelay = 100;
            // 
            // groupBoxEmbedded
            // 
            this.groupBoxEmbedded.Controls.Add(this.checkBoxEmbeddedUse);
            this.groupBoxEmbedded.Location = new System.Drawing.Point(15, 413);
            this.groupBoxEmbedded.Name = "groupBoxEmbedded";
            this.groupBoxEmbedded.Size = new System.Drawing.Size(412, 46);
            this.groupBoxEmbedded.TabIndex = 5;
            this.groupBoxEmbedded.TabStop = false;
            this.groupBoxEmbedded.Text = "Specific to Embedded Examples";
            // 
            // checkBoxEmbeddedUse
            // 
            this.checkBoxEmbeddedUse.AutoSize = true;
            this.checkBoxEmbeddedUse.Location = new System.Drawing.Point(7, 20);
            this.checkBoxEmbeddedUse.Name = "checkBoxEmbeddedUse";
            this.checkBoxEmbeddedUse.Size = new System.Drawing.Size(362, 17);
            this.checkBoxEmbeddedUse.TabIndex = 0;
            this.checkBoxEmbeddedUse.Text = "Consider the second tab column to be an embedded example sentence";
            this.checkBoxEmbeddedUse.UseVisualStyleBackColor = true;
            // 
            // FormFineTune
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(440, 524);
            this.Controls.Add(this.groupBoxEmbedded);
            this.Controls.Add(this.linkLabelReset);
            this.Controls.Add(this.groupBoxJJOnly);
            this.Controls.Add(this.groupBoxGeneral);
            this.Controls.Add(this.groupBoxEdictOnly);
            this.Controls.Add(this.groupBoxKen5Only);
            this.Controls.Add(this.buttonCancel);
            this.Controls.Add(this.buttonOK);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormFineTune";
            this.ShowInTaskbar = false;
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.Text = "Fine-Tune Options - Epwing2Anki";
            this.groupBoxKen5Only.ResumeLayout(false);
            this.groupBoxKen5Only.PerformLayout();
            this.groupBoxEdictOnly.ResumeLayout(false);
            this.groupBoxEdictOnly.PerformLayout();
            this.groupBoxJJOnly.ResumeLayout(false);
            this.groupBoxJJOnly.PerformLayout();
            this.groupBoxGeneral.ResumeLayout(false);
            this.groupBoxGeneral.PerformLayout();
            this.groupBoxEmbedded.ResumeLayout(false);
            this.groupBoxEmbedded.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

    }

    #endregion

    private System.Windows.Forms.Button buttonOK;
    private System.Windows.Forms.Button buttonCancel;
    private System.Windows.Forms.CheckBox checkBoxKen5NoAlphaFallback;
    private System.Windows.Forms.GroupBox groupBoxKen5Only;
    private System.Windows.Forms.GroupBox groupBoxEdictOnly;
    private System.Windows.Forms.CheckBox checkBoxEdictRemoveWordIndicators;
    private System.Windows.Forms.CheckBox checkBoxEdictRemoveP;
    private System.Windows.Forms.GroupBox groupBoxJJOnly;
    private System.Windows.Forms.CheckBox checkBoxJJRemoveSpecialCharsFromReading;
    private System.Windows.Forms.CheckBox checkBoxJJKeepExamplesInDef;
    private System.Windows.Forms.GroupBox groupBoxGeneral;
    private System.Windows.Forms.CheckBox checkBoxCompact;
    private System.Windows.Forms.TextBox textBoxExamplePrependText;
    private System.Windows.Forms.Label labelExamplePrependText;
    private System.Windows.Forms.LinkLabel linkLabelReset;
    private System.Windows.Forms.ToolTip toolTip1;
    private System.Windows.Forms.Label labelPrependDicNameToDef;
    private System.Windows.Forms.ComboBox comboBoxPrependDicNameToDef;
    private System.Windows.Forms.Label labelAppendDicNameToExamples;
    private System.Windows.Forms.ComboBox comboBoxAppendDicNameToExamples;
    private System.Windows.Forms.CheckBox checkBoxAddPlaceholders;
    private System.Windows.Forms.CheckBox checkBoxJJFillInExampleBlanksWithWord;
    private System.Windows.Forms.CheckBox checkBoxAddRubyToExamples;
        private System.Windows.Forms.GroupBox groupBoxEmbedded;
        private System.Windows.Forms.CheckBox checkBoxEmbeddedUse;
    }
}