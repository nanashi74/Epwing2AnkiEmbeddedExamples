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
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows.Forms;

namespace Epwing2Anki
{
  public partial class FormGenerate : Form
  {
    // The information necassary for each word to generate a card
    // Key = word
    // value = Card info object
    private Dictionary<string, CardInfo> cardInfoDic = new Dictionary<string, CardInfo>();

    // The background worker used to gather information about each word
    private BackgroundWorker bw = new BackgroundWorker();

    // Used to pause/resume the background worker when GUI input in required
    private static ManualResetEvent mre = new ManualResetEvent(false);

    // The last disambiguated entry. Used to attempt to auto-disambiguate 
    // entries when in manual mode from a previous disambiguation.
    private Entry lastDisambiguatedEntry = new Entry();

    // List of words that required either an entry disambiguation or an example choice (used for undo)
    private List<string> manualWordList = new List<string>();

    // Will be set if the "< Back" button is pressed.
    private bool undoLastChoice = false;

    // The word that is currently being processed
    private WordListItem currentWord;


    /// <summary>
    /// The list of generated cards.
    /// </summary>
    public List<CardInfo> CardInfoList
    {
      get { return this.cardInfoDic.Values.ToList(); }
    }


    /// <summary>
    /// Start the card generation background worker.
    /// </summary>
    private void startGenerateCards()
    {
      try
      {
        // Create a background worker thread
        bw = new BackgroundWorker();
        bw.WorkerSupportsCancellation = true;
        bw.DoWork += new DoWorkEventHandler(doWorkGenerateCards);
        bw.RunWorkerCompleted += new RunWorkerCompletedEventHandler(generateWorkerCompleted);
        bw.RunWorkerAsync(null);
        Logger.Instance.startSection("Gathering Card Info");
      }
      catch (Exception e1)
      {
        UtilsMsg.showErrMsg("Could not create background worker.\n" + e1);
        return;
      }
    }


    /// <summary>
    /// Called when the background worker is completed or is canceled.
    /// </summary>
    private void generateWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
    {
      this.okayToClose = true;

      if ((e.Cancelled == true))
      {
        Logger.Instance.warning("Canceled Gather Card Info");
        this.DialogResult = DialogResult.Cancel;
      }
      else
      {
        List<CardInfo> cardInfoList = this.cardInfoDic.Values.ToList();
        ImportFile importFileGen = new ImportFile(this.settings, cardInfoList);

        if (this.settings.FineTuneOptions.AddRubyToExamples)
        {
          this.writeEvent("Please wait while ruby is generated for the example sentences (this can take several minutes).");
        }

        Logger.Instance.flush();
        importFileGen.generate();

        this.genLog.addFromCardList(cardInfoList, this.getCardLayoutDics());
        this.genLog.NumCardsInImportFile = importFileGen.NumCardsInImportFile;

        this.DialogResult = DialogResult.OK;
      }

      Logger.Instance.endSection("Gathering Card Info");
      Logger.Instance.flush();

      this.Close();
    }


    /// <summary>
    /// Main background worker routine for generating cards.
    /// </summary>
    private void doWorkGenerateCards(object sender, DoWorkEventArgs e)
    {
      BackgroundWorker worker = sender as BackgroundWorker;

      this.genLog = new GenLog();
      this.cardInfoDic.Clear();
      this.manualWordList.Clear();
      this.undoLastChoice = false;
      
      // Clear events log text box
      this.Invoke((MethodInvoker)delegate { this.textBoxEvents.Text = ""; });

      int totalWords = this.settings.WordList.Count;
      float percentComplete = 0;

      string lastUndoWord = "";

      // For each word in the word list
      for (int wordIdx = 0; wordIdx < totalWords; wordIdx++)
      {
        this.currentWord = this.settings.WordList[wordIdx];

        // If this word hasn't already been looked up (can happen for words that 
        // follow the undo word) or is the undo word 
        if (!this.cardInfoDic.ContainsKey(this.currentWord.Word) || (this.currentWord.Word == lastUndoWord))
        {
          // Update progress
          percentComplete = Math.Min(100, (wordIdx + 1) / (float)totalWords);

          this.Invoke((MethodInvoker)delegate
          {
            this.textBoxExpression.Text = this.currentWord.Word;
            this.labelProgress.Text = String.Format("Generating {0}/{1}", (wordIdx + 1), totalWords);
            this.progressBarMain.Value = (int)(percentComplete * 100);
            this.labelPercent.Text = String.Format("{0:##.#%}", percentComplete);
          });

          // Create a card info for the given word and add to cardInfoDic
          this.generateCardInfoForWord(this.currentWord);

          // If the user wants to undo a previous choice
          if (this.undoLastChoice && (manualWordList.Count > 0))
          {
            // Reset undo flag
            this.undoLastChoice = false;

            // Get the last word that required a manual choice
            lastUndoWord = manualWordList[manualWordList.Count - 1];

            this.writeEvent(String.Format("Going back to {0}.", lastUndoWord));

            // Search for index of the undo word in word list
            for (int i = wordIdx; i >= 0; i--)
            {
              if (this.settings.WordList[i].Word == lastUndoWord)
              {
                wordIdx = (i - 1);
                break;
              }
            }

            manualWordList.RemoveAt(manualWordList.Count - 1);
          }
        }

        // Check for cancellation
        if (worker.CancellationPending)
        {
          e.Cancel = true;
          return;
        }
      }
    }


    /// <summary>
    /// Create a card info for the given word and add to cardInfoDic.
    /// </summary>
    private void generateCardInfoForWord(WordListItem word)
    {
      // Reset the last disambiguated entry
      this.lastDisambiguatedEntry = new Entry();

      CardInfo cardInfo = new CardInfo(word);

      // Search Highest Priority dics
      if (this.settings.CardLayout.containsFieldType(FieldEnum.Def))
      {
        this.lookupHighestPriorityDics(word.Word, cardInfo);
      }

      if (this.undoLastChoice)
      {
        return;
      }

      // Search J-E dics
      if (this.settings.CardLayout.containsFieldType(FieldEnum.DefJE))
      {
        this.lookupDicsBasedOnType(word.Word, cardInfo, DicTypeEnum.JE);
      }

      if (this.undoLastChoice)
      {
        return;
      }

      // Search J-J dics
      if (this.settings.CardLayout.containsFieldType(FieldEnum.DefJJ))
      {
        this.lookupDicsBasedOnType(word.Word, cardInfo, DicTypeEnum.JJ);
      }

      if (this.undoLastChoice)
      {
        return;
      }

      // Search named dics specifically added to the card layout
      this.lookupNamedDics(word.Word, cardInfo);

      if (this.undoLastChoice)
      {
        return;
      }

      // Search dics needed for examples
      this.lookupExampleDics(word.Word, cardInfo);

      if (this.undoLastChoice)
      {
        return;
      }

      // Find the best reading/expression to use
      bool readingFound = this.findBestExpressionAndReading(cardInfo);

      // If the reading was not found
      if (!readingFound)
      {
        // TODO: Search all dics in dic list, get all unique readings in each entry, 
        //       and allow user to choose one. A lot of work for something
        //       that rarely happens though.
      }

      // Choose example sentences for the card
      if (this.settings.CardLayout.containsExamplesField())
      {
        this.generateCardInfoExamples(cardInfo);
      }

      if (this.undoLastChoice)
      {
        return;
      }

      // If a card info already exist for this word, overwrite it, otherwise, add it.
      // This is to support the undo feature.
      if (this.cardInfoDic.ContainsKey(word.Word))
      {
        this.cardInfoDic[word.Word] = cardInfo; 
      }
      else
      {
        this.cardInfoDic.Add(word.Word, cardInfo);
      }
    }


    /// <summary>
    /// Search the highest priority dics.
    /// Will skip any dics that have already been searched.
    /// Adds to cardInfo.
    /// </summary>
    private void lookupHighestPriorityDics(string word, CardInfo cardInfo)
    {
      DicList enabledDics = this.settings.DicList.getEnabledDics();

      // Search all dics
      foreach (Dic dic in enabledDics.Dics)
      {
        // If we haven't already searched in this dictionary
        if (!cardInfo.containsDicName(dic.Name))
        {
          Entry entry = this.lookupWordInDic(word, dic);
          cardInfo.Entries.Add(dic.Name, entry);

          // Highest priority, so first dic found is the one we will use
          if ((entry != null) && (entry.Definition != ""))
          {
            break;
          }

          if (this.undoLastChoice)
          {
            return;
          }
        }
      }
    }

    /// <summary>
    /// Search the dics based on type: J-E or J-J.
    /// Will skip any dics that have already been searched.
    /// Adds to cardInfo.
    /// </summary>
    private void lookupDicsBasedOnType(string word, CardInfo cardInfo, DicTypeEnum type)
    {
      DicList dicList = this.settings.DicList.getDicsByType(type).getEnabledDics();

      foreach (Dic dic in dicList.Dics)
      {
        // If we haven't already searched in this dictionary
        if (!cardInfo.containsDicName(dic.Name))
        {
          Entry entry = this.lookupWordInDic(word, dic);
          cardInfo.Entries.Add(dic.Name, entry);

          // Highest priority, so first dic found is the one we will use
          if ((entry != null) && (entry.Definition != ""))
          {
            break;
          }

          if (this.undoLastChoice)
          {
            return;
          }
        }
      }
    }


    /// <summary>
    /// Will search the dics specifically added in the card layout.
    /// Will skip any dics that have already been searched.
    /// Adds to cardInfo.
    /// </summary>
    private void lookupNamedDics(string word, CardInfo cardInfo)
    {
      CardLayout defFields = this.settings.CardLayout.getDefFields();
      DicList dicList = new DicList();

      // Get the list of dics to search
      foreach (CardLayoutField field in defFields.Fields)
      {
        if (field.Type == FieldEnum.DefDicName)
        {
          Dic dic = this.settings.DicList.getDicByName(field.DicName);

          // Don't care about enable flag because it is specifically named
          if (dic != null)
          {
            dicList.Dics.Add(dic);
          }
        }
      }

      foreach (Dic dic in dicList.Dics)
      {
        // If we haven't already searched in this dictionary
        if (!cardInfo.containsDicName(dic.Name))
        {
          Entry entry = this.lookupWordInDic(word, dic);
          cardInfo.Entries.Add(dic.Name, entry);

          if (this.undoLastChoice)
          {
            return;
          }
        }
      }
    }


    /// <summary>
    /// Will search the dics needed for examples.
    /// Will skip any dics that have already been searched.
    /// Adds to cardInfo.
    /// </summary>
    private void lookupExampleDics(string word, CardInfo cardInfo)
    {
      bool exampleDicsNeeded = this.settings.CardLayout.containsExamplesField();

      // If the example field is in the card layout
      if (exampleDicsNeeded)
      {
        // Search all dics in the example dic list
        foreach (Dic dic in this.settings.ExampleDicList.Dics)
        {
          // FIX: Removed in v1.1. This prevents examples from being fetched in example priority order!
          // If example are auto-generated and already have enough example to cover the max, break
          //if (this.settings.AutoChooseExamples
          //  && this.alreadyHaveEnoughExampleSentences(cardInfo))
          //{
          //  break;
          //}

          // If we haven't already searched in this dictionary
          if (dic.ExamplesEnabled && !cardInfo.containsDicName(dic.Name))
          {
            Entry entry = this.lookupWordInDic(word, dic);
            cardInfo.Entries.Add(dic.Name, entry);

            if (this.undoLastChoice)
            {
              return;
            }
          }
        }
      }
    }


    /// <summary>
    /// Does cardInfo have enough example sentences to reach the user specified max amount.
    /// </summary>
    private bool alreadyHaveEnoughExampleSentences(CardInfo cardInfo)
    {
      return (cardInfo.getExamples(this.settings.ExampleDicList).Count
        >= this.settings.MaxAutoChooseExamples);
    }


    /// <summary>
    /// Set the provided card info's reading and expression fields to the 
    /// best reading and expression based on the card info's entries.
    /// </summary>
    private bool findBestExpressionAndReading(CardInfo cardInfo)
    {
      bool found = false;
      List<Entry> readingEntryList = this.getEntriesForReadingLookup(cardInfo);

      foreach (Entry entry in readingEntryList)
      {
        if (entry.Reading.Trim() != "")
        {
          cardInfo.Reading = entry.Reading;
          cardInfo.Expression = entry.Expression;
          found = true;
          break;
        }
        else if ((cardInfo.Expression.Trim() == "")
          && (entry.Expression != ""))
        {
          // Try to at least get the expression (in case a reading is never found)
          cardInfo.Expression = entry.Expression;
        }
      }

      return found;
    }


    /// <summary>
    /// Get the entries from card info that we can use to lookup the reading based on 
    /// order they appear in card layout.
    /// Example:
    /// If card layout is Expression, Reading, Definition J-J, Definition J-E, then 
    /// return a list containing the highest priority J-J entry in the card info followed
    /// by the highest priority J-E entry in the card info.
    /// </summary>
    private List<Entry> getEntriesForReadingLookup(CardInfo cardInfo)
    {
      DicList dicList = this.getCardLayoutDics();
      List<Entry> entryList = cardInfo.getEntriesPriority(dicList);

      return entryList;
    }


    /// <summary>
    /// Get the list of dics mentioned in the card layout. Order is maintained. No duplicates.
    /// </summary>
    private DicList getCardLayoutDics()
    {
      DicList dicList = new DicList();

      foreach (CardLayoutField field in this.settings.CardLayout.Fields)
      {
        DicList curDicList = new DicList();

        if (field.Type == FieldEnum.Def)
        {
          curDicList = this.settings.DicList.getEnabledDics();
        }
        else if (field.Type == FieldEnum.DefJE)
        {
          curDicList = this.settings.DicList.getDicsByType(DicTypeEnum.JE).getEnabledDics();
        }
        else if (field.Type == FieldEnum.DefJJ)
        {
          curDicList = this.settings.DicList.getDicsByType(DicTypeEnum.JJ).getEnabledDics();
        }
        else if (field.Type == FieldEnum.DefDicName)
        {
          if (this.settings.DicList.containsDicName(field.DicName))
          {
            // Note: Not checking if dic is enabled
            curDicList.Dics.Add(this.settings.DicList.getDicByName(field.DicName));
          }
        }

        foreach (Dic dic in curDicList.Dics)
        {
          if (!dicList.containsDicName(dic.Name))
          {
            dicList.Dics.Add(dic);
          }
        }
      }

      return dicList;
    }


    /// <summary>
    /// Generate examples for the provided card info.
    /// </summary>
    private void generateCardInfoExamples(CardInfo cardInfo)
    {
      if (cardInfo.getExamples(this.settings.ExampleDicList).Count > 0)
      {
        if (this.settings.AutoChooseExamples)
        {
          cardInfo.ChosenExampleList = this.autoChooseExamples(cardInfo, this.settings.MaxAutoChooseExamples);

          this.writeEvent(String.Format("Automatically chose {0} example sentences for {1}.",
            cardInfo.ChosenExampleList.Count, cardInfo.Word.Word));
        }
        else
        {
          cardInfo.ChosenExampleList = this.manuallyChooseExamples(cardInfo);

          if (!this.undoLastChoice)
          {
            if (cardInfo.ChosenExampleList.Count == 0)
            {
              this.writeEvent(String.Format("User chose not to select example sentences for {0}.",
                cardInfo.Word.Word));
            }
            else
            {
              this.writeEvent(String.Format("User chose {0} example sentences for {1}.",
                cardInfo.ChosenExampleList.Count, cardInfo.Word.Word));
            }
          }
        }
      }
      else
      {
        this.writeEvent(String.Format("No example sentences found for {0}.", cardInfo.Word.Word));
      }
    }


    /// <summary>
    /// Automatically choose example sentences.
    /// </summary>
    private List<Example> autoChooseExamples(CardInfo cardInfo, int num)
    {
      // Contains all examples, sorted, from every dictionary in seperate lists
      List<List<Example>> completeExampleList
        = cardInfo.getExamplesSeparatePriority(this.settings.ExampleDicList);

      List<Example> chosenExamples = new List<Example>();

      foreach (List<Example> exampleList in completeExampleList)
      {
        List<List<Example>> subDefExampleLists = new List<List<Example>>();
        List<Example> curList = new List<Example>();
        int curSubDef = 1;

        // Split examples into seperate lists by sub-definition
        foreach (Example example in exampleList)
        {
          if (example.SubDefNumber == curSubDef)
          {
            curList.Add(example);
          }
          else
          {
            if (curList.Count > 0)
            {
              curList.Add(example);
              subDefExampleLists.Add(curList);
            }

            curList = new List<Example>();
            curSubDef = example.SubDefNumber;
          }
        }

        if (curList.Count > 0)
        {
          subDefExampleLists.Add(curList);
        }

        // Select one example from each sub-def
        // TODO: Take priority into consideration.
        foreach (List<Example> subDefExampleList in subDefExampleLists)
        {
          Random rand = new Random();
          int randIdx = rand.Next(0, subDefExampleList.Count - 1);
         
          chosenExamples.Add(subDefExampleList[randIdx]);
          exampleList.Remove(subDefExampleList[randIdx]);

          if ((chosenExamples.Count >= num)
            || (exampleList.Count == 0))
          {
            break;
          }
        }

        // If don't have enough example sentences yet, choose at random
        for (int i = 0;
          ((chosenExamples.Count < num)
          && (exampleList.Count > 0)); 
          i++)
        {
          Random rand = new Random();
          int randIdx = rand.Next(0, exampleList.Count - 1);

          chosenExamples.Add(exampleList[randIdx]);
          exampleList.RemoveAt(randIdx);
        }

        if (chosenExamples.Count >= num)
        {
          break;
        }
      }

      return chosenExamples;
    }


    /// <summary>
    /// Manually choose example sentences via the GUI.
    /// </summary>
    private List<Example> manuallyChooseExamples(CardInfo cardInfo)
    {
      // Contains all examples from every dictionary
      List<Example> completeExampleList
        = cardInfo.getExamplesPriority(this.settings.ExampleDicList);

      // The list of entries for word in priority order
      List<Entry> priorityEntryList
        = cardInfo.getEntriesPriority(this.settings.DicList.getEnabledDics());

      this.Invoke((MethodInvoker)delegate
      {
        this.setPage(Page.Examples);
        this.chooseExamples(completeExampleList, priorityEntryList, cardInfo);
      });

      // Wait for user to choose example sentences
      if (!this.bw.CancellationPending)
      {
        mre.Reset();
      }

      mre.WaitOne();

      this.Invoke((MethodInvoker)delegate
      {
        this.setPage(Page.Progress);
      });

      if (!this.undoLastChoice && !this.manualWordList.Contains(cardInfo.Word.Word))
      {
        this.manualWordList.Add(cardInfo.Word.Word);
      }

      return this.userChosenExampleList;
    }


    /// <summary>
    /// Lookup a word in the provided dictionary and disambiguate if necessary.
    /// </summary>
    private Entry lookupWordInDic(string word, Dic dic)
    {
      Entry finalEntry = new Entry();

      List<Entry> entryList = dic.lookup(word, 
        this.settings.CardLayout.containsExamplesField(),
        this.settings.FineTuneOptions);

      if ((entryList != null) && (entryList.Count > 0))
      {
        Dic curDic = this.settings.DicList.getDicByName(entryList[0].DicName);

        // If this dic is only used for example sentences,
        // remove all entries that do not contain example sentences
        if ((curDic != null) && !curDic.Enabled)
        {
          List<Entry> entryListWithNoExampleEntriesRemoved = new List<Entry>();

          // Add only enabled that have examples
          foreach (Entry entry in entryList)
          {
            if (entry.ExampleList.Count > 0)
            {
              entryListWithNoExampleEntriesRemoved.Add(entry);
            }
          }

          // If no examples were found in any of the entries, ignore the entries for this dic
          if (entryListWithNoExampleEntriesRemoved.Count == 0)
          {
            this.writeEvent(String.Format("Skipping {0} in 『{1}』 because none of the entries have examples.",
              word, dic.Name));
            return null;
          }
          else
          {
            this.writeEvent(String.Format("Removed entries without examples for {0} in 『{1}』.", word, dic.Name));
            entryList = entryListWithNoExampleEntriesRemoved;
          }
        }
      }

      // If no entries exist for this word
      if ((entryList == null) || (entryList.Count == 0))
      {
        this.writeEvent(String.Format("No entries for {0} in 『{1}』.", word, dic.Name));

        finalEntry = null;
      }
      // Else if exactly one entry was found, use it
      else if (entryList.Count == 1)
      {
        finalEntry = entryList[0];

        // Save the disambiguation if this is the first one for this word
        if (this.lastDisambiguatedEntry.Expression == "")
        {
          this.lastDisambiguatedEntry = finalEntry;
        }

        this.writeEvent(String.Format("Automatically chose only entry for {0} from 『{1}』.",
          word, dic.Name));
      }
      // Else many entries were found, disambiguate
      else
      {
        // If user wants system to automatically choose the best one
        if (this.settings.AutoDisambiguate)
        {
          finalEntry = this.autoDisambiguate(entryList);
          
          this.writeEvent(String.Format("Automatically disambiguated {0} from 『{1}』.",
            word, dic.Name));
        }
        else // User wants to manually disambiguate
        {
          bool autoChoseBasedOnPrev = false;

          // Can we try to disambiguate based on a previous disambiguation for this word?
          if (this.lastDisambiguatedEntry.Expression != "")
          {
            int numWithSameReadingAndExpression = 0;

            foreach (Entry entry in entryList)
            {
              if (entry.haveSameReadingAndExpression(this.lastDisambiguatedEntry))
              {
                numWithSameReadingAndExpression++;

                // Save the auto-disambiguated entry if it's the first one
                if (numWithSameReadingAndExpression == 1)
                {
                  finalEntry = entry;
                }
              }
            }

            // Only auto-choose based on previous disambiguation if only one entry matches.
            // If multiple entries match the previous disambiguation, the user will have to choose manually.
            if (numWithSameReadingAndExpression == 1)
            {
              autoChoseBasedOnPrev = true;
            }
          }

          // Has the entry been chosen based on a previous disambiguation?
          if (autoChoseBasedOnPrev)
          {
            this.writeEvent(String.Format("Automatically disambiguated {0} from 『{1}』 based on previous disambiguation.",
              word, dic.Name));
          }
          else // Manualy disambiguate
          {
            this.Invoke((MethodInvoker)delegate
            {
              this.setPage(Page.Disambiguate);
              this.textBoxTitle.Text = String.Format("Disambiguate Entries for {0} 『{1}』",
                word, dic.Name);
              this.disambiguate(entryList);
            });

            // Wait for user to choose an entry
            if (!this.bw.CancellationPending)
            {
              mre.Reset();
            }

            mre.WaitOne();

            try
            {
              finalEntry = entryList[this.selectedEntryIdx];
            }
            catch (Exception e1)
            {
              finalEntry = entryList[0];
              Logger.Instance.error("lookupWordInDic: " + e1);
            }

            // Save the disambiguation if this is the first one for this word
            if (this.lastDisambiguatedEntry.Expression == "")
            {
              this.lastDisambiguatedEntry = finalEntry;
            }

            if (!this.undoLastChoice && !this.manualWordList.Contains(word))
            {
              this.manualWordList.Add(word);
            }

            this.Invoke((MethodInvoker)delegate
            {
              this.setPage(Page.Progress);
            });

            if (!this.undoLastChoice)
            {
              this.writeEvent(String.Format("User disambiguated {0} from 『{1}』.",
                word, dic.Name));
            }
          }
        }
      }

      return finalEntry;
    }



    /// <summary>
    /// Auto select the best entry from the given list.
    /// </summary>
    private Entry autoDisambiguate(List<Entry> entryList)
    {
      Entry bestEntry = new Entry();

      // Table if card layout contains examples
      bool[,] importanceExamples = new bool[6, 3]
      {
        // Reading, def, sentences
        { true, true, true },
        { true, true, false },
        { false, true, true },
        { false, true, false },
        { true, false, true },
        { true, false, false },
      };

      // Table if card layout does not contain examples
      bool[,] importanceNoExamples = new bool[3, 3]
      {
        // Reading, def, sentences
        { true, true, false },
        { false, true, false },
        { true, false, false }
      };

      bool[,] importanceTable;

      // Choose table to use
      if (this.settings.CardLayout.containsExamplesField())
      {
        importanceTable = importanceExamples;
      }
      else
      {
        importanceTable = importanceNoExamples;
      }

      for (int i = 0; i < importanceTable.GetLength(0); i++)
      {
        bestEntry = this.getBestEntryBasedOnCriteria(entryList, 
          importanceTable[i, 0],
          importanceTable[i, 1], 
          importanceTable[i, 2]);

        if (bestEntry.Expression != "")
        {
          break;
        }
      }

      return bestEntry;
    }


    /// <summary>
    /// Get the best entry from the list based on the provided criteria.
    /// </summary>
    private Entry getBestEntryBasedOnCriteria(List<Entry> entryList,
      bool checkReading, bool checkDefinition, bool checkExamples)
    {
      List<Entry> matchList = new List<Entry>();
      Entry bestEntry = new Entry();

      foreach (Entry entry in entryList)
      {
        if ((!checkReading || (entry.Reading != ""))
          && (!checkDefinition || (entry.Definition != ""))
          && (!checkExamples || (entry.ExampleList.Count > 0)))
        {
          matchList.Add(entry);
        }
      }

      if (matchList.Count == 1)
      {
        bestEntry = matchList[0];
      }
      else if (matchList.Count > 0)
      {
        if (checkDefinition)
        {
          bestEntry = this.getEntryWithLongestDefinition(matchList);
        }
        else if (checkExamples)
        {
          bestEntry = getEntryWithMostExampleSentences(matchList);
        }
        else
        {
          bestEntry = matchList[0];
        }
      }

      return bestEntry;
    }


    /// <summary>
    /// Get the entry with the longest definition from the list.
    /// </summary>
    private Entry getEntryWithLongestDefinition(List<Entry> entryList)
    {
      Entry largestDefEntry = new Entry();

      foreach (Entry entry in entryList)
      {
        if (entry.Definition.Length > largestDefEntry.Definition.Length)
        {
          largestDefEntry = entry;
        }
      }

      return largestDefEntry;
    }


    /// <summary>
    /// Get the entry wth the most example sentences from the list.
    /// </summary>
    private Entry getEntryWithMostExampleSentences(List<Entry> entryList)
    {
      Entry mostExamplesEntry = new Entry();

      foreach (Entry entry in entryList)
      {
        if (entry.ExampleList.Count > mostExamplesEntry.ExampleList.Count)
        {
          mostExamplesEntry = entry;
        }
      }

      return mostExamplesEntry;
    }


   


  }
}