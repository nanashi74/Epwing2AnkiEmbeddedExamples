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
using System.Text.RegularExpressions;
using System.Text;
using System.Windows.Forms;
using System.Runtime.InteropServices;


namespace Epwing2Anki
{
  public partial class FormMain : Form
  {
    private Settings settings = new Settings();
    private FormGenerate formGenerate = new FormGenerate();
    private FormFineTune formFineTune = new FormFineTune();

    // All of the following is for disabling the silly click noise when IE changes pages.
    private const int FEATURE_DISABLE_NAVIGATION_SOUNDS = 21;
    private const int SET_FEATURE_ON_THREAD = 0x00000001;
    private const int SET_FEATURE_ON_PROCESS = 0x00000002;
    private const int SET_FEATURE_IN_REGISTRY = 0x00000004;
    private const int SET_FEATURE_ON_THREAD_LOCALMACHINE = 0x00000008;
    private const int SET_FEATURE_ON_THREAD_INTRANET = 0x00000010;
    private const int SET_FEATURE_ON_THREAD_TRUSTED = 0x00000020;
    private const int SET_FEATURE_ON_THREAD_INTERNET = 0x00000040;
    private const int SET_FEATURE_ON_THREAD_RESTRICTED = 0x00000080;
    [DllImport("urlmon.dll")]
    [PreserveSig]
    [return: MarshalAs(UnmanagedType.Error)]
    static extern int CoInternetSetFeatureEnabled(
    int FeatureEntry,
    [MarshalAs(UnmanagedType.U4)] int dwFlags,
    bool fEnable);


    /// <summary>
    /// Constructor.
    /// </summary>
    public FormMain()
    {
      InitializeComponent();

      this.disableWebBrowserClickNoise();
    }


    /// <summary>
    /// Setup initial state of form.
    /// </summary>
    private void FormMain_Load(object sender, EventArgs e)
    {
      Logger.Instance.info("FormMain_Load");

      this.loadSettings();

      this.setupTabs();
      this.setupWelcome();
      this.setupDicList();
      this.setupExampleList();
      this.setupCardLayout();
      this.setupIOPage();

      this.setPage(0);
      this.ActiveControl = this.buttonNext;
    }


    /// <summary>
    /// Load the settings file. It it doesn't exist, create a new 
    /// settings object with default data.
    /// </summary>
    private void loadSettings()
    {
      settings = Settings.load();

      if (settings == null)
      {
        settings = new Settings();
      }
    }


    /// <summary>
    /// Disable Web Broswer click noise with page changes.
    /// http://stackoverflow.com/questions/393166/how-to-disable-click-sound-in-webbrowser-control
    /// </summary>
    private void disableWebBrowserClickNoise()
    {
      int feature = FEATURE_DISABLE_NAVIGATION_SOUNDS;
      CoInternetSetFeatureEnabled(feature, SET_FEATURE_ON_PROCESS, true);
    }


    /// <summary>
    /// Goto appropriate tab page when nav button is clicked.
    /// </summary>
    private void buttonNav_Click(object sender, EventArgs e)
    {
      NavButton button = (NavButton)sender;

      int count = 0;

      foreach (TabPage tab in this.tabControlMain.TabPages)
      {
        if (tab.Text == button.Text)
        {
          this.setPage(count);
          break;
        }

        count++;
      }

      // Remove focus from button (looks ugly)
      this.ActiveControl = this.panelLine;
    }


    /// <summary>
    /// When tab index changes, set the large text at top of page to title of tab page.
    /// </summary>
    private void tabControlMain_SelectedIndexChanged(object sender, EventArgs e)
    {
      this.textBoxTitle.Text = this.tabControlMain.SelectedTab.Text;
    }


    /// <summary>
    /// When next button is clicked, goto the next tab page.
    /// </summary>
    private void buttonNext_Click(object sender, EventArgs e)
    {
      if (this.tabControlMain.SelectedIndex != this.tabControlMain.TabPages.Count - 1)
      {
        this.setPage(this.tabControlMain.SelectedIndex + 1);
      }
    }


    /// <summary>
    /// When back button is clicked, goto the previous tab page.
    /// </summary>
    private void buttonBack_Click(object sender, EventArgs e)
    {
      if (this.tabControlMain.SelectedIndex != 0)
      {
        this.setPage(this.tabControlMain.SelectedIndex - 1);
      }
    }


    /// <summary>
    /// When exit button is clicked, close the form.
    /// </summary>
    private void buttonExit_Click(object sender, EventArgs e)
    {
      this.Close();
    }


    /// <summary>
    /// When form is closing, save settings.
    /// </summary>
    private void FormMain_FormClosing(object sender, FormClosingEventArgs e)
    {
      //bool exit =  UtilsMsg.showConfirm("Are you sure that you want to quit?");
      bool exit = true;

      if (exit)
      {
        this.gatherGUI(false);
        this.settings.save();
        Logger.Instance.info("FormMain_FormClosing");
        Logger.Instance.flush();
      }
      else
      {
        e.Cancel = true;
      }
    }


    /// <summary>
    /// When about link is clicked, Show the about dialog.
    /// </summary>
    private void linkLabelAbout_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
    {
      UtilsMsg.showInfoMsg(String.Format("Version: {0}\nAuthor: {1}\nContact: {2}\n",
          UtilsAssembly.Version, UtilsAssembly.Author, "cb4960@gmail.com"));
    }


    /// <summary>
    /// Setup tab related stuff (including nav buttons).
    /// </summary>
    private void setupTabs()
    {
      int count = 0;

      foreach (TabPage tab in this.tabControlMain.TabPages)
      {
        // Set the tab pages to the normal control color
        tab.BackColor = SystemColors.Control;

        // Create navigate buttons
        NavButton button = new NavButton(tab.Text);
        button.Width = this.flowLayoutPanelLeft.Width - 7;
        button.Click += new EventHandler(buttonNav_Click);

        // Activate the first tab
        if (count == 0)
        {
          button.Active = true;
        }

        this.flowLayoutPanelLeft.Padding = new Padding(0, this.panelTop.Height - 3, 0, 0);
        this.flowLayoutPanelLeft.Controls.Add(button);

        count++;
      }

      // Set the title to first tab page
      this.textBoxTitle.Text = this.tabControlMain.SelectedTab.Text;
    }


    /// <summary>
    /// Set the current tab page. Also actives the appropriate nav button.
    /// </summary>
    private void setPage(int selectedIdx)
    {
      int count = 0;

      // Set active state of all buttons
      foreach (Control control in this.flowLayoutPanelLeft.Controls)
      {
        NavButton button = (NavButton)control;
        button.Active = (count == selectedIdx);
        count++;
      }

      // Switch to the associated tab
      this.tabControlMain.SelectedIndex = selectedIdx;

      this.buttonBack.Enabled = (this.tabControlMain.SelectedIndex != 0);
      this.buttonNext.Enabled =
        (this.tabControlMain.SelectedIndex != this.tabControlMain.TabPages.Count - 1);

      this.ActiveControl = this.panelLine;
    }


    /// <summary>
    /// When the TEST ME button is clicked, test something.
    /// </summary>
    private void buttonTest_Click(object sender, EventArgs e)
    {
      FineTune fineTune = new FineTune();

      //DicEpwingKen5th dic = new DicEpwingKen5th(@"G:\Japanese\Japanese Software\Epwing\[EPWING] Kenkyusha 5th\CATALOGS", 0);
      DicEpwingChujiten dic = new DicEpwingChujiten(@"G:\Japanese\Japanese Software\Epwing\[EPWING] Kenkyusha Shin Eiwa-Waei Chujiten\CATALOGS", 0);
      //DicEpwingDaijisen dic = new DicEpwingDaijisen(@"G:\Japanese\Japanese Software\Epwing\[EPWING] Daijisen JJ\daijisen\CATALOGS", 0);
      //DicEpwingKojien6th dic = new DicEpwingKojien6th(@"G:\Japanese\Japanese Software\Epwing\[EPWING] Kojien 6th\CATALOGS", 0);
      //DicEpwingDaijirin2nd dic = new DicEpwingDaijirin2nd(@"G:\Japanese\Japanese Software\Epwing\[EPWING] Daijirin 2nd\CATALOGS", 0);
      //DicEpwingMeikyo dic = new DicEpwingMeikyo(@"G:\Japanese\Japanese Software\Epwing\[EPWING] Meikyo J-J\CATALOGS", 0);

      List<Entry> entryList = dic.lookup(this.textBoxTest.Text.Trim(), true, fineTune);

      if ((entryList != null) && (entryList.Count > 0))
      {

      }
    }







  }




}
