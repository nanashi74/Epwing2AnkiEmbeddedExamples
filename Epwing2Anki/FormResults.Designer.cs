namespace Epwing2Anki
{
  partial class FormResults
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
      System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormResults));
      this.buttonDone = new System.Windows.Forms.Button();
      this.buttonOpenDir = new System.Windows.Forms.Button();
      this.textBoxTitle = new System.Windows.Forms.TextBox();
      this.webBrowserResults = new System.Windows.Forms.WebBrowser();
      this.panelBottom = new System.Windows.Forms.Panel();
      this.linkLabelImport = new System.Windows.Forms.LinkLabel();
      this.panelLine = new System.Windows.Forms.Panel();
      this.buttonOpenFile = new System.Windows.Forms.Button();
      this.panelBottom.SuspendLayout();
      this.SuspendLayout();
      // 
      // buttonDone
      // 
      this.buttonDone.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
      this.buttonDone.Location = new System.Drawing.Point(733, 10);
      this.buttonDone.Name = "buttonDone";
      this.buttonDone.Size = new System.Drawing.Size(75, 23);
      this.buttonDone.TabIndex = 1;
      this.buttonDone.Text = "Done";
      this.buttonDone.UseVisualStyleBackColor = true;
      this.buttonDone.Click += new System.EventHandler(this.buttonDone_Click);
      // 
      // buttonOpenDir
      // 
      this.buttonOpenDir.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
      this.buttonOpenDir.Location = new System.Drawing.Point(581, 10);
      this.buttonOpenDir.Name = "buttonOpenDir";
      this.buttonOpenDir.Size = new System.Drawing.Size(146, 23);
      this.buttonOpenDir.TabIndex = 1;
      this.buttonOpenDir.Text = "Open Output Directory";
      this.buttonOpenDir.UseVisualStyleBackColor = true;
      this.buttonOpenDir.Click += new System.EventHandler(this.buttonOpenDir_Click);
      // 
      // textBoxTitle
      // 
      this.textBoxTitle.BackColor = System.Drawing.Color.LightSlateGray;
      this.textBoxTitle.BorderStyle = System.Windows.Forms.BorderStyle.None;
      this.textBoxTitle.Dock = System.Windows.Forms.DockStyle.Top;
      this.textBoxTitle.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.textBoxTitle.ForeColor = System.Drawing.Color.White;
      this.textBoxTitle.Location = new System.Drawing.Point(0, 0);
      this.textBoxTitle.Multiline = true;
      this.textBoxTitle.Name = "textBoxTitle";
      this.textBoxTitle.ReadOnly = true;
      this.textBoxTitle.Size = new System.Drawing.Size(820, 26);
      this.textBoxTitle.TabIndex = 3;
      this.textBoxTitle.TabStop = false;
      this.textBoxTitle.Text = " Card Creation Results";
      // 
      // webBrowserResults
      // 
      this.webBrowserResults.Dock = System.Windows.Forms.DockStyle.Fill;
      this.webBrowserResults.Location = new System.Drawing.Point(0, 26);
      this.webBrowserResults.MinimumSize = new System.Drawing.Size(20, 20);
      this.webBrowserResults.Name = "webBrowserResults";
      this.webBrowserResults.Size = new System.Drawing.Size(820, 509);
      this.webBrowserResults.TabIndex = 4;
      // 
      // panelBottom
      // 
      this.panelBottom.Controls.Add(this.linkLabelImport);
      this.panelBottom.Controls.Add(this.panelLine);
      this.panelBottom.Controls.Add(this.buttonDone);
      this.panelBottom.Controls.Add(this.buttonOpenFile);
      this.panelBottom.Controls.Add(this.buttonOpenDir);
      this.panelBottom.Dock = System.Windows.Forms.DockStyle.Bottom;
      this.panelBottom.Location = new System.Drawing.Point(0, 535);
      this.panelBottom.Name = "panelBottom";
      this.panelBottom.Size = new System.Drawing.Size(820, 43);
      this.panelBottom.TabIndex = 5;
      // 
      // linkLabelImport
      // 
      this.linkLabelImport.AutoSize = true;
      this.linkLabelImport.Location = new System.Drawing.Point(12, 20);
      this.linkLabelImport.Name = "linkLabelImport";
      this.linkLabelImport.Size = new System.Drawing.Size(131, 13);
      this.linkLabelImport.TabIndex = 4;
      this.linkLabelImport.TabStop = true;
      this.linkLabelImport.Text = "How do I import into Anki?";
      this.linkLabelImport.Visible = false;
      // 
      // panelLine
      // 
      this.panelLine.BackColor = System.Drawing.Color.Silver;
      this.panelLine.Dock = System.Windows.Forms.DockStyle.Top;
      this.panelLine.Location = new System.Drawing.Point(0, 0);
      this.panelLine.Name = "panelLine";
      this.panelLine.Size = new System.Drawing.Size(820, 1);
      this.panelLine.TabIndex = 3;
      // 
      // buttonOpenFile
      // 
      this.buttonOpenFile.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
      this.buttonOpenFile.Location = new System.Drawing.Point(429, 10);
      this.buttonOpenFile.Name = "buttonOpenFile";
      this.buttonOpenFile.Size = new System.Drawing.Size(146, 23);
      this.buttonOpenFile.TabIndex = 1;
      this.buttonOpenFile.Text = "Open Anki Import File";
      this.buttonOpenFile.UseVisualStyleBackColor = true;
      this.buttonOpenFile.Visible = false;
      this.buttonOpenFile.Click += new System.EventHandler(this.buttonOpenFile_Click);
      // 
      // FormResults
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.ClientSize = new System.Drawing.Size(820, 578);
      this.Controls.Add(this.webBrowserResults);
      this.Controls.Add(this.panelBottom);
      this.Controls.Add(this.textBoxTitle);
      this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
      this.Name = "FormResults";
      this.Text = "Results - Epwing2Anki";
      this.Load += new System.EventHandler(this.FormResults_Load);
      this.panelBottom.ResumeLayout(false);
      this.panelBottom.PerformLayout();
      this.ResumeLayout(false);
      this.PerformLayout();

    }

    #endregion

    private System.Windows.Forms.Button buttonDone;
    private System.Windows.Forms.Button buttonOpenDir;
    private System.Windows.Forms.TextBox textBoxTitle;
    private System.Windows.Forms.WebBrowser webBrowserResults;
    private System.Windows.Forms.Panel panelBottom;
    private System.Windows.Forms.Button buttonOpenFile;
    private System.Windows.Forms.Panel panelLine;
    private System.Windows.Forms.LinkLabel linkLabelImport;
  }
}