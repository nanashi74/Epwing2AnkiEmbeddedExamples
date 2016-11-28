namespace Epwing2Anki
{
  partial class FormGenerate
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
      System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormGenerate));
      this.panelButtom = new System.Windows.Forms.Panel();
      this.textBoxExpression = new System.Windows.Forms.TextBox();
      this.labelPercent = new System.Windows.Forms.Label();
      this.progressBarMain = new System.Windows.Forms.ProgressBar();
      this.panelLine = new System.Windows.Forms.Panel();
      this.buttonBack = new System.Windows.Forms.Button();
      this.buttonCancel = new System.Windows.Forms.Button();
      this.panelTop = new System.Windows.Forms.Panel();
      this.textBoxHelp = new System.Windows.Forms.TextBox();
      this.textBoxTitle = new System.Windows.Forms.TextBox();
      this.tabControlMain = new System.Windows.Forms.TabControl();
      this.tabPageProgress = new System.Windows.Forms.TabPage();
      this.labelProgress = new System.Windows.Forms.Label();
      this.textBoxEvents = new System.Windows.Forms.TextBox();
      this.tabPageDisambiguate = new System.Windows.Forms.TabPage();
      this.webBrowserDisambiguate = new System.Windows.Forms.WebBrowser();
      this.tabPageChooseExamples = new System.Windows.Forms.TabPage();
      this.listViewExamples = new System.Windows.Forms.ListView();
      this.columnHeaderDic = new System.Windows.Forms.ColumnHeader();
      this.columnHeaderSubDef = new System.Windows.Forms.ColumnHeader();
      this.columnHeaderExample = new System.Windows.Forms.ColumnHeader();
      this.splitter2 = new System.Windows.Forms.Splitter();
      this.splitter1 = new System.Windows.Forms.Splitter();
      this.webBrowserExampleDef = new System.Windows.Forms.WebBrowser();
      this.panelExampleTopDummy = new System.Windows.Forms.Panel();
      this.webBrowserExample = new System.Windows.Forms.WebBrowser();
      this.panelExBottom = new System.Windows.Forms.Panel();
      this.panelLine2 = new System.Windows.Forms.Panel();
      this.buttonAutoChooseExamples = new System.Windows.Forms.Button();
      this.buttonExDone = new System.Windows.Forms.Button();
      this.panelButtom.SuspendLayout();
      this.panelTop.SuspendLayout();
      this.tabControlMain.SuspendLayout();
      this.tabPageProgress.SuspendLayout();
      this.tabPageDisambiguate.SuspendLayout();
      this.tabPageChooseExamples.SuspendLayout();
      this.panelExBottom.SuspendLayout();
      this.SuspendLayout();
      // 
      // panelButtom
      // 
      this.panelButtom.Controls.Add(this.textBoxExpression);
      this.panelButtom.Controls.Add(this.labelPercent);
      this.panelButtom.Controls.Add(this.progressBarMain);
      this.panelButtom.Controls.Add(this.panelLine);
      this.panelButtom.Controls.Add(this.buttonBack);
      this.panelButtom.Controls.Add(this.buttonCancel);
      this.panelButtom.Dock = System.Windows.Forms.DockStyle.Bottom;
      this.panelButtom.Location = new System.Drawing.Point(0, 535);
      this.panelButtom.Name = "panelButtom";
      this.panelButtom.Size = new System.Drawing.Size(820, 43);
      this.panelButtom.TabIndex = 1;
      // 
      // textBoxExpression
      // 
      this.textBoxExpression.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                  | System.Windows.Forms.AnchorStyles.Right)));
      this.textBoxExpression.BorderStyle = System.Windows.Forms.BorderStyle.None;
      this.textBoxExpression.Font = new System.Drawing.Font("MS PMincho", 21.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.textBoxExpression.Location = new System.Drawing.Point(417, 7);
      this.textBoxExpression.Name = "textBoxExpression";
      this.textBoxExpression.ReadOnly = true;
      this.textBoxExpression.Size = new System.Drawing.Size(229, 29);
      this.textBoxExpression.TabIndex = 4;
      this.textBoxExpression.Text = "隙間産業";
      // 
      // labelPercent
      // 
      this.labelPercent.AutoSize = true;
      this.labelPercent.Location = new System.Drawing.Point(375, 15);
      this.labelPercent.Name = "labelPercent";
      this.labelPercent.Size = new System.Drawing.Size(36, 13);
      this.labelPercent.TabIndex = 3;
      this.labelPercent.Text = "99.9%";
      // 
      // progressBarMain
      // 
      this.progressBarMain.Location = new System.Drawing.Point(12, 10);
      this.progressBarMain.Name = "progressBarMain";
      this.progressBarMain.Size = new System.Drawing.Size(357, 23);
      this.progressBarMain.TabIndex = 2;
      // 
      // panelLine
      // 
      this.panelLine.BackColor = System.Drawing.Color.Silver;
      this.panelLine.Dock = System.Windows.Forms.DockStyle.Top;
      this.panelLine.Location = new System.Drawing.Point(0, 0);
      this.panelLine.Name = "panelLine";
      this.panelLine.Size = new System.Drawing.Size(820, 1);
      this.panelLine.TabIndex = 1;
      // 
      // buttonBack
      // 
      this.buttonBack.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
      this.buttonBack.Location = new System.Drawing.Point(652, 10);
      this.buttonBack.Name = "buttonBack";
      this.buttonBack.Size = new System.Drawing.Size(75, 23);
      this.buttonBack.TabIndex = 0;
      this.buttonBack.Text = "< Back";
      this.buttonBack.UseVisualStyleBackColor = true;
      this.buttonBack.Visible = false;
      this.buttonBack.Click += new System.EventHandler(this.buttonBack_Click);
      // 
      // buttonCancel
      // 
      this.buttonCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
      this.buttonCancel.Location = new System.Drawing.Point(733, 10);
      this.buttonCancel.Name = "buttonCancel";
      this.buttonCancel.Size = new System.Drawing.Size(75, 23);
      this.buttonCancel.TabIndex = 0;
      this.buttonCancel.Text = "Cancel";
      this.buttonCancel.UseVisualStyleBackColor = true;
      this.buttonCancel.Click += new System.EventHandler(this.buttonCancel_Click);
      // 
      // panelTop
      // 
      this.panelTop.BackColor = System.Drawing.Color.LightSlateGray;
      this.panelTop.Controls.Add(this.textBoxHelp);
      this.panelTop.Controls.Add(this.textBoxTitle);
      this.panelTop.Dock = System.Windows.Forms.DockStyle.Top;
      this.panelTop.Location = new System.Drawing.Point(0, 0);
      this.panelTop.Name = "panelTop";
      this.panelTop.Size = new System.Drawing.Size(820, 43);
      this.panelTop.TabIndex = 2;
      // 
      // textBoxHelp
      // 
      this.textBoxHelp.BackColor = System.Drawing.Color.LightSlateGray;
      this.textBoxHelp.BorderStyle = System.Windows.Forms.BorderStyle.None;
      this.textBoxHelp.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.textBoxHelp.ForeColor = System.Drawing.Color.White;
      this.textBoxHelp.Location = new System.Drawing.Point(5, 25);
      this.textBoxHelp.Margin = new System.Windows.Forms.Padding(0);
      this.textBoxHelp.Multiline = true;
      this.textBoxHelp.Name = "textBoxHelp";
      this.textBoxHelp.ReadOnly = true;
      this.textBoxHelp.Size = new System.Drawing.Size(803, 18);
      this.textBoxHelp.TabIndex = 1;
      this.textBoxHelp.Text = "This is some help text. abcdefghijklmnopqrstuvwxyz";
      // 
      // textBoxTitle
      // 
      this.textBoxTitle.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                  | System.Windows.Forms.AnchorStyles.Right)));
      this.textBoxTitle.BackColor = System.Drawing.Color.LightSlateGray;
      this.textBoxTitle.BorderStyle = System.Windows.Forms.BorderStyle.None;
      this.textBoxTitle.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.textBoxTitle.ForeColor = System.Drawing.Color.White;
      this.textBoxTitle.Location = new System.Drawing.Point(4, -1);
      this.textBoxTitle.Margin = new System.Windows.Forms.Padding(0);
      this.textBoxTitle.Multiline = true;
      this.textBoxTitle.Name = "textBoxTitle";
      this.textBoxTitle.ReadOnly = true;
      this.textBoxTitle.Size = new System.Drawing.Size(804, 26);
      this.textBoxTitle.TabIndex = 0;
      this.textBoxTitle.Text = "Title abcdefghijklmnopqrstuvwxyz";
      // 
      // tabControlMain
      // 
      this.tabControlMain.Appearance = System.Windows.Forms.TabAppearance.FlatButtons;
      this.tabControlMain.Controls.Add(this.tabPageProgress);
      this.tabControlMain.Controls.Add(this.tabPageDisambiguate);
      this.tabControlMain.Controls.Add(this.tabPageChooseExamples);
      this.tabControlMain.Dock = System.Windows.Forms.DockStyle.Fill;
      this.tabControlMain.ItemSize = new System.Drawing.Size(0, 1);
      this.tabControlMain.Location = new System.Drawing.Point(0, 0);
      this.tabControlMain.Name = "tabControlMain";
      this.tabControlMain.Padding = new System.Drawing.Point(0, 0);
      this.tabControlMain.SelectedIndex = 0;
      this.tabControlMain.Size = new System.Drawing.Size(820, 535);
      this.tabControlMain.TabIndex = 3;
      // 
      // tabPageProgress
      // 
      this.tabPageProgress.Controls.Add(this.labelProgress);
      this.tabPageProgress.Controls.Add(this.textBoxEvents);
      this.tabPageProgress.Location = new System.Drawing.Point(4, 5);
      this.tabPageProgress.Name = "tabPageProgress";
      this.tabPageProgress.Size = new System.Drawing.Size(812, 526);
      this.tabPageProgress.TabIndex = 0;
      this.tabPageProgress.Text = "Progress";
      this.tabPageProgress.UseVisualStyleBackColor = true;
      // 
      // labelProgress
      // 
      this.labelProgress.AutoSize = true;
      this.labelProgress.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.labelProgress.Location = new System.Drawing.Point(-5, 41);
      this.labelProgress.Margin = new System.Windows.Forms.Padding(3);
      this.labelProgress.Name = "labelProgress";
      this.labelProgress.Size = new System.Drawing.Size(190, 25);
      this.labelProgress.TabIndex = 1;
      this.labelProgress.Text = "Generating 24/150";
      // 
      // textBoxEvents
      // 
      this.textBoxEvents.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                  | System.Windows.Forms.AnchorStyles.Left)
                  | System.Windows.Forms.AnchorStyles.Right)));
      this.textBoxEvents.BackColor = System.Drawing.Color.GhostWhite;
      this.textBoxEvents.Font = new System.Drawing.Font("Meiryo UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.textBoxEvents.Location = new System.Drawing.Point(0, 69);
      this.textBoxEvents.Margin = new System.Windows.Forms.Padding(0);
      this.textBoxEvents.Multiline = true;
      this.textBoxEvents.Name = "textBoxEvents";
      this.textBoxEvents.ReadOnly = true;
      this.textBoxEvents.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
      this.textBoxEvents.Size = new System.Drawing.Size(812, 457);
      this.textBoxEvents.TabIndex = 0;
      this.textBoxEvents.Text = resources.GetString("textBoxEvents.Text");
      // 
      // tabPageDisambiguate
      // 
      this.tabPageDisambiguate.Controls.Add(this.webBrowserDisambiguate);
      this.tabPageDisambiguate.Location = new System.Drawing.Point(4, 5);
      this.tabPageDisambiguate.Margin = new System.Windows.Forms.Padding(0);
      this.tabPageDisambiguate.Name = "tabPageDisambiguate";
      this.tabPageDisambiguate.Size = new System.Drawing.Size(812, 526);
      this.tabPageDisambiguate.TabIndex = 1;
      this.tabPageDisambiguate.Text = "Disambiguate Entries";
      this.tabPageDisambiguate.UseVisualStyleBackColor = true;
      // 
      // webBrowserDisambiguate
      // 
      this.webBrowserDisambiguate.AllowWebBrowserDrop = false;
      this.webBrowserDisambiguate.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                  | System.Windows.Forms.AnchorStyles.Left)
                  | System.Windows.Forms.AnchorStyles.Right)));
      this.webBrowserDisambiguate.Location = new System.Drawing.Point(0, 41);
      this.webBrowserDisambiguate.MinimumSize = new System.Drawing.Size(20, 20);
      this.webBrowserDisambiguate.Name = "webBrowserDisambiguate";
      this.webBrowserDisambiguate.Size = new System.Drawing.Size(812, 485);
      this.webBrowserDisambiguate.TabIndex = 0;
      this.webBrowserDisambiguate.Navigating += new System.Windows.Forms.WebBrowserNavigatingEventHandler(this.webBrowserDisambiguate_Navigating);
      this.webBrowserDisambiguate.PreviewKeyDown += new System.Windows.Forms.PreviewKeyDownEventHandler(this.webBrowserDisambiguate_PreviewKeyDown);
      // 
      // tabPageChooseExamples
      // 
      this.tabPageChooseExamples.Controls.Add(this.listViewExamples);
      this.tabPageChooseExamples.Controls.Add(this.splitter2);
      this.tabPageChooseExamples.Controls.Add(this.splitter1);
      this.tabPageChooseExamples.Controls.Add(this.webBrowserExampleDef);
      this.tabPageChooseExamples.Controls.Add(this.panelExampleTopDummy);
      this.tabPageChooseExamples.Controls.Add(this.webBrowserExample);
      this.tabPageChooseExamples.Controls.Add(this.panelExBottom);
      this.tabPageChooseExamples.Location = new System.Drawing.Point(4, 5);
      this.tabPageChooseExamples.Margin = new System.Windows.Forms.Padding(0);
      this.tabPageChooseExamples.Name = "tabPageChooseExamples";
      this.tabPageChooseExamples.Size = new System.Drawing.Size(812, 526);
      this.tabPageChooseExamples.TabIndex = 2;
      this.tabPageChooseExamples.Text = "Choose Examples";
      this.tabPageChooseExamples.UseVisualStyleBackColor = true;
      // 
      // listViewExamples
      // 
      this.listViewExamples.BorderStyle = System.Windows.Forms.BorderStyle.None;
      this.listViewExamples.CheckBoxes = true;
      this.listViewExamples.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeaderDic,
            this.columnHeaderSubDef,
            this.columnHeaderExample});
      this.listViewExamples.Dock = System.Windows.Forms.DockStyle.Fill;
      this.listViewExamples.Font = new System.Drawing.Font("Meiryo UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.listViewExamples.FullRowSelect = true;
      this.listViewExamples.GridLines = true;
      this.listViewExamples.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
      this.listViewExamples.Location = new System.Drawing.Point(0, 129);
      this.listViewExamples.Margin = new System.Windows.Forms.Padding(0);
      this.listViewExamples.MultiSelect = false;
      this.listViewExamples.Name = "listViewExamples";
      this.listViewExamples.ShowGroups = false;
      this.listViewExamples.Size = new System.Drawing.Size(812, 325);
      this.listViewExamples.TabIndex = 1;
      this.listViewExamples.UseCompatibleStateImageBehavior = false;
      this.listViewExamples.View = System.Windows.Forms.View.Details;
      this.listViewExamples.ItemChecked += new System.Windows.Forms.ItemCheckedEventHandler(this.listViewExamples_ItemChecked);
      this.listViewExamples.SelectedIndexChanged += new System.EventHandler(this.listViewExamples_SelectedIndexChanged);
      // 
      // columnHeaderDic
      // 
      this.columnHeaderDic.Text = "Source";
      this.columnHeaderDic.Width = 72;
      // 
      // columnHeaderSubDef
      // 
      this.columnHeaderSubDef.Text = "Sub Def";
      this.columnHeaderSubDef.Width = 55;
      // 
      // columnHeaderExample
      // 
      this.columnHeaderExample.Text = "Example Sentence";
      this.columnHeaderExample.Width = 2000;
      // 
      // splitter2
      // 
      this.splitter2.BackColor = System.Drawing.Color.Silver;
      this.splitter2.Dock = System.Windows.Forms.DockStyle.Bottom;
      this.splitter2.Location = new System.Drawing.Point(0, 454);
      this.splitter2.Name = "splitter2";
      this.splitter2.Size = new System.Drawing.Size(812, 2);
      this.splitter2.TabIndex = 5;
      this.splitter2.TabStop = false;
      // 
      // splitter1
      // 
      this.splitter1.BackColor = System.Drawing.Color.Silver;
      this.splitter1.Dock = System.Windows.Forms.DockStyle.Top;
      this.splitter1.Location = new System.Drawing.Point(0, 127);
      this.splitter1.Name = "splitter1";
      this.splitter1.Size = new System.Drawing.Size(812, 2);
      this.splitter1.TabIndex = 4;
      this.splitter1.TabStop = false;
      // 
      // webBrowserExampleDef
      // 
      this.webBrowserExampleDef.Dock = System.Windows.Forms.DockStyle.Top;
      this.webBrowserExampleDef.Location = new System.Drawing.Point(0, 39);
      this.webBrowserExampleDef.Margin = new System.Windows.Forms.Padding(0);
      this.webBrowserExampleDef.MinimumSize = new System.Drawing.Size(20, 20);
      this.webBrowserExampleDef.Name = "webBrowserExampleDef";
      this.webBrowserExampleDef.Size = new System.Drawing.Size(812, 88);
      this.webBrowserExampleDef.TabIndex = 2;
      // 
      // panelExampleTopDummy
      // 
      this.panelExampleTopDummy.Dock = System.Windows.Forms.DockStyle.Top;
      this.panelExampleTopDummy.Location = new System.Drawing.Point(0, 0);
      this.panelExampleTopDummy.Margin = new System.Windows.Forms.Padding(0);
      this.panelExampleTopDummy.Name = "panelExampleTopDummy";
      this.panelExampleTopDummy.Size = new System.Drawing.Size(812, 39);
      this.panelExampleTopDummy.TabIndex = 3;
      // 
      // webBrowserExample
      // 
      this.webBrowserExample.Dock = System.Windows.Forms.DockStyle.Bottom;
      this.webBrowserExample.Location = new System.Drawing.Point(0, 456);
      this.webBrowserExample.Margin = new System.Windows.Forms.Padding(0);
      this.webBrowserExample.MinimumSize = new System.Drawing.Size(20, 20);
      this.webBrowserExample.Name = "webBrowserExample";
      this.webBrowserExample.Size = new System.Drawing.Size(812, 46);
      this.webBrowserExample.TabIndex = 2;
      // 
      // panelExBottom
      // 
      this.panelExBottom.Controls.Add(this.panelLine2);
      this.panelExBottom.Controls.Add(this.buttonAutoChooseExamples);
      this.panelExBottom.Controls.Add(this.buttonExDone);
      this.panelExBottom.Dock = System.Windows.Forms.DockStyle.Bottom;
      this.panelExBottom.Location = new System.Drawing.Point(0, 502);
      this.panelExBottom.Name = "panelExBottom";
      this.panelExBottom.Size = new System.Drawing.Size(812, 24);
      this.panelExBottom.TabIndex = 6;
      // 
      // panelLine2
      // 
      this.panelLine2.BackColor = System.Drawing.Color.Silver;
      this.panelLine2.Dock = System.Windows.Forms.DockStyle.Top;
      this.panelLine2.Location = new System.Drawing.Point(0, 0);
      this.panelLine2.Name = "panelLine2";
      this.panelLine2.Size = new System.Drawing.Size(812, 1);
      this.panelLine2.TabIndex = 2;
      // 
      // buttonAutoChooseExamples
      // 
      this.buttonAutoChooseExamples.Location = new System.Drawing.Point(0, 2);
      this.buttonAutoChooseExamples.Name = "buttonAutoChooseExamples";
      this.buttonAutoChooseExamples.Size = new System.Drawing.Size(190, 23);
      this.buttonAutoChooseExamples.TabIndex = 0;
      this.buttonAutoChooseExamples.Text = "Auto-choose up to 99 examples";
      this.buttonAutoChooseExamples.UseVisualStyleBackColor = true;
      this.buttonAutoChooseExamples.Click += new System.EventHandler(this.buttonAutoChooseExamples_Click);
      // 
      // buttonExDone
      // 
      this.buttonExDone.Location = new System.Drawing.Point(348, 2);
      this.buttonExDone.Name = "buttonExDone";
      this.buttonExDone.Size = new System.Drawing.Size(116, 23);
      this.buttonExDone.TabIndex = 0;
      this.buttonExDone.Text = "Done choosing";
      this.buttonExDone.UseVisualStyleBackColor = true;
      this.buttonExDone.Click += new System.EventHandler(this.buttonExDone_Click);
      // 
      // FormGenerate
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.ClientSize = new System.Drawing.Size(820, 578);
      this.Controls.Add(this.panelTop);
      this.Controls.Add(this.tabControlMain);
      this.Controls.Add(this.panelButtom);
      this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
      this.Name = "FormGenerate";
      this.Text = "Generate";
      this.Load += new System.EventHandler(this.FormChoose_Load);
      this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormGenerate_FormClosing);
      this.panelButtom.ResumeLayout(false);
      this.panelButtom.PerformLayout();
      this.panelTop.ResumeLayout(false);
      this.panelTop.PerformLayout();
      this.tabControlMain.ResumeLayout(false);
      this.tabPageProgress.ResumeLayout(false);
      this.tabPageProgress.PerformLayout();
      this.tabPageDisambiguate.ResumeLayout(false);
      this.tabPageChooseExamples.ResumeLayout(false);
      this.panelExBottom.ResumeLayout(false);
      this.ResumeLayout(false);

    }

    #endregion

    private System.Windows.Forms.Panel panelButtom;
    private System.Windows.Forms.Panel panelTop;
    private System.Windows.Forms.TabControl tabControlMain;
    private System.Windows.Forms.TabPage tabPageProgress;
    private System.Windows.Forms.TabPage tabPageDisambiguate;
    private System.Windows.Forms.Button buttonCancel;
    private System.Windows.Forms.TabPage tabPageChooseExamples;
    private System.Windows.Forms.Panel panelLine;
    private System.Windows.Forms.Label labelPercent;
    private System.Windows.Forms.ProgressBar progressBarMain;
    private System.Windows.Forms.Label labelProgress;
    private System.Windows.Forms.TextBox textBoxEvents;
    private System.Windows.Forms.TextBox textBoxHelp;
    private System.Windows.Forms.WebBrowser webBrowserDisambiguate;
    private System.Windows.Forms.TextBox textBoxExpression;
    private System.Windows.Forms.TextBox textBoxTitle;
    private System.Windows.Forms.ListView listViewExamples;
    private System.Windows.Forms.ColumnHeader columnHeaderSubDef;
    private System.Windows.Forms.ColumnHeader columnHeaderExample;
    private System.Windows.Forms.WebBrowser webBrowserExampleDef;
    private System.Windows.Forms.Splitter splitter2;
    private System.Windows.Forms.Splitter splitter1;
    private System.Windows.Forms.Panel panelExampleTopDummy;
    private System.Windows.Forms.WebBrowser webBrowserExample;
    private System.Windows.Forms.Panel panelExBottom;
    private System.Windows.Forms.Button buttonExDone;
    private System.Windows.Forms.ColumnHeader columnHeaderDic;
    private System.Windows.Forms.Panel panelLine2;
    private System.Windows.Forms.Button buttonAutoChooseExamples;
    private System.Windows.Forms.Button buttonBack;


  }
}