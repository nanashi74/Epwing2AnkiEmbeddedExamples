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
using System.Linq;
using System.Text;

namespace Epwing2Anki
{
  public class UtilsRuby
  {
    /// <summary>
    /// Add Anki-style ruby to the provided text.
    /// </summary>
    public static string addAnkiRubyToText(string text)
    {
      string rubyText = "";

      // Replace existing brackets to prevent nested ruby
      // Example: 1日だけ仕事[家事]から逃れたい。
      //      --> 1日[にち]だけ 仕事[しごと]{ 家事[かじ]}から 逃[のが]れたい。
      string bracketReplacementText = text.Replace("[", "{").Replace("]", "}");

      List<InfoMecab> mecabList = new List<InfoMecab>();

      try
      {
        mecabList = Mecab.parseFields(bracketReplacementText, true);
      }
      catch(Exception e1)
      {
        Logger.Instance.error("addAnkiRubyToText: " + e1);
        return text;
      }

      if (mecabList.Count == 0)
      {
        return text;
      }

      foreach (InfoMecab infoMecab in mecabList)
      {
        // Should ruby be applied?
        if ((infoMecab.Reading != "")
          && (infoMecab.Word != infoMecab.Reading)
          && UtilsLang.containsIdeograph(infoMecab.Word))
        {
          string kanji = infoMecab.Word;
          string reading = infoMecab.Reading;
          string wordHiragana = UtilsLang.convertKatakanaToHiragana(infoMecab.Word);

          int readingEndIdx = reading.Length - 1;

          for (int i = infoMecab.Word.Length - 1; i >= 0; i--)
          {
            if ((readingEndIdx >= 0) 
              && (wordHiragana[i].ToString() == reading[readingEndIdx].ToString()))
            {
              kanji = kanji.Substring(0, kanji.Length - 1);
              reading = reading.Substring(0, reading.Length - 1);

              readingEndIdx--;
            }
            else
            {
              break;
            }
          }

          // Note: the initial space is necassary for Anki to correctly format the ruby
          rubyText += String.Format(" {0}[{1}]{2}",
            kanji, reading, infoMecab.Word.Substring(kanji.Length));
        }
        else
        {
          rubyText += infoMecab.Word;
        }
      }


      return rubyText;
    }

  }
}
