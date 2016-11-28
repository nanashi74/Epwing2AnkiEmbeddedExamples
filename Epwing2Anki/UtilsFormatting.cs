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
using System.Text.RegularExpressions;

namespace Epwing2Anki
{
  public class UtilsFormatting
  {

    /// <summary>
    /// Add puncuation to the end of a some Japanese text (if it doesn't already have any).
    /// </summary>
    public static string addPunctuationToJapText(string text)
    {
      string newText = text.Trim();

      if (newText != "")
      {
        char lastChar = newText[newText.Length - 1];

        if (lastChar == '.')
        {
          // To be consistant, replace . with 。
          newText = newText.Remove(newText.Length - 1) + "。";
        }
        else if ((lastChar != '。') && (lastChar != '！') && (lastChar != '？')
          && (lastChar != '!') && (lastChar != '?') && (lastChar != '…'))
        {
          newText += "。";
        }
      }

      return newText;
    }


    /// <summary>
    /// Capitalize the first character of English text and add a period to the end.
    /// </summary>
    public static string addPunctuationToEngText(string text)
    {
      string newText = text.Trim();

      if (newText != "")
      {
        newText = char.ToUpper(newText[0]) + newText.Substring(1);

        char lastChar = newText[newText.Length - 1];

        if ((lastChar != '.') && (lastChar != '!') 
          && (lastChar != '?') && (lastChar != '…'))
        {
          newText += ".";
        }
      }

      return newText;
    }


    /// <summary>
    /// Remove special characters from an expression.
    /// ×猪突   --->   猪突
    /// </summary>
    public static string removeSpecialCharsFromExpression(string expression)
    {
      expression = Regex.Replace(expression, "<.*?>", "");
      expression = Regex.Replace(expression, @"\(.*?\)", "");
      expression = Regex.Replace(expression, @"\（.*?\）", "");
      expression = Regex.Replace(expression, "[-‐・▽▼△×《》]", "");

      return expression;
    }


    /// <summary>
    /// Remove special characters from a reading.
    /// す‐てき    --->   すてき
    /// </summary>
    public static string removeSpecialCharsFromReading(string reading)
    {
      return Regex.Replace(reading, "[-‐・]", "");
    }


  }
}
