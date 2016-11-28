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
using System.Data.SQLite;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Epwing2Anki
{
  // From http://www.edrdg.org/wiki/index.php/Tanaka_Corpus
  // 
  // The file consists of pairs of lines, beginning with "A: " and "B: "
  // respectively. There may also be comment lines which begin with a "#".
  // 
  // The "A:" lines contain the Japanese sentence and the English
  // translation, separated by a TAB character. At the end of the English
  // translation is a pair of sequence numbers in the format:
  // #ID=nnnnnn_mmmmmm which identify the English and Japanese sentence pair
  // in the Tatoteba Project;
  // 
  // The "B:" lines contain a space-delimited list of Japanese words found in
  // the preceding sentence.
  // 
  // The Japanese words in the "B:" lines can have the following appended:
  // 
  // a reading in hiragana. This is to resolve cases where the word can be
  // read different ways. WWWJDIC uses this to ensure that only the
  // appropriate sentences are linked. The reading is in "round" parentheses.
  // 
  // a sense number. This occurs when the word has multiple senses in the
  // EDICT file, and indicates which sense applies in the sentence. WWWJDIC
  // displays these numbers. The sense number is in "square" parentheses.
  // 
  // the form in which the word appears in the sentence. This will differ
  // from the indexing word if it has been inflected, for example. This field
  // is in "curly" parentheses.
  // 
  // a "~" character to indicate that the sentence pair is a good and checked
  // example of the usage of the word. Words are marked to enable appropriate
  // sentences to be selected by dictionary software. Typically only one
  // instance per sense of a word will be marked.The WWWJDIC server displays
  // these sentences below the display of the related dictionary entry.
  // 
  // The following example pair illustrates the format:
  // 
  // A: その家はかなりぼろ屋になっている。[TAB]The house is quite run down.#ID=25507
  // 
  // B: 其の{その} 家(いえ)[1] は 可也{かなり} ぼろ屋[1]~ になる[1]{になっている}
    

  /// <summary>
  /// Represents the Tatoeba example sentence database.
  /// </summary>
  public class Tatoeba
  {
    private List<TatoebaPair> exampleList = new List<TatoebaPair>();

    /// <summary>
    /// Load the Tatoeba database into memory.
    /// </summary>
    private void loadDatabase()
    {
      StreamReader reader = new StreamReader(ConstantSettings.TatoebaDb, Encoding.UTF8, true);
      string line = "";
      string lineA = "";
      this.exampleList = new List<TatoebaPair>(150000);
      Regex regex = new Regex(@"#ID=\d.*", RegexOptions.Compiled);

      while ((line = reader.ReadLine()) != null)
      {
        if (line.StartsWith("A:"))
        {
          // Remove "A: ", tabs, and ID
          //lineA = regex.Replace(line.Replace("\t", " ").Remove(0, 3), "");

          // Remove "A: " and ID
          lineA = regex.Replace(line.Remove(0, 3), "");
        }
        else
        {
          this.exampleList.Add(new TatoebaPair(lineA, line.Remove(0, 3)));
        }
      }
    }


    /// <summary>
    /// Get the sentence of the given entry. Populates the entry's example list.
    /// </summary>
    public void lookup(Entry entry)
    {
      // If Tatoeba has not been parsed yet, parse it
      if (exampleList.Count == 0)
      {
        this.loadDatabase();
      }

      foreach (TatoebaPair pair in exampleList)
      {
        // Check to see if word is in line.
        // Do this first check because it is MUCH faster than the regex below
        if (pair.B.Contains(entry.Expression))
        {
          Match match = Regex.Match(pair.B,
            "(?:^| )" + entry.Expression
            + @"(?:\((?<Reading>.*?)\))?(?:\[(?<Sense>\d*?)\])?(?:{(?<Usage>.*?)})?(?<Validated>~)?(?:$| )");

          if (match.Success)
          {
            // How the word should be read in the sentence
            string exReading = match.Groups["Reading"].ToString().Trim();

            // Sense is the sub-definition
            string sense = match.Groups["Sense"].ToString().Trim();

            // Usage is how word appears in the A: sentence (in a conjugated form for example)
            string usage = match.Groups["Usage"].ToString().Trim();

            // If "the sentence pair is a good and checked example of the usage of the word."
            string validated = match.Groups["Validated"].ToString().Trim();

            // If the reading in the example is appropriate for this entry
            if ((exReading == entry.Reading) || (exReading.Length == 0))
            {
              Example example = new Example();
              example.DicName = entry.DicName;
              example.Priority = 3;
              example.HasTranslation = true;

              string[] fields = pair.A.Split('\t');

              // There should be two fields: Japanese sentence and Translation
              if (fields.Length != 2)
              {
                return;
              }

              // Replace the occasional spaces from the Japanese sentence with a "punctuation space" (0x2008)
              // And separate the Japanese sentence and translation with a tab
              example.Text = String.Format("{0}\t{1}",
                fields[0].Replace(" ", " ").Replace("　", " "),
                fields[1]);
   
              // If word has sense info
              if (sense.Length > 0)
              {
                example.Priority = 2;
                example.SubDefNumber = Convert.ToInt32(sense);
              }
              else
              {
                // If word does not have sense info but the definition contains multiple sub-definitions
                if (entry.Definition.Contains("②"))
                {
                  example.SubDefNumber = Example.NO_SUB_DEF;
                }
                else // Definition does not contain multiple sub-definitions
                {
                  example.SubDefNumber = 1;
                }
              }

              // If word was validated, increase priority
              if (validated.Length > 0)
              {
                example.Priority = 1;
              }

              entry.ExampleList.Add(example);
            }
          }
        }
      }
    }

  }



  /// <summary>
  /// Represents a pair of sentences in Tatoeba.
  /// </summary>
  public class TatoebaPair
  {
    public string A { get; set; }
    public string B { get; set; }


    public TatoebaPair(string A, string B)
    {
      this.A = A;
      this.B = B;
    }
  }

}
