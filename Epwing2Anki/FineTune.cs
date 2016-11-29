using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Epwing2Anki
{
  /// <summary>
  /// Options to fine-tune things related to dictionaries.
  /// </summary>
  [Serializable]
  public class FineTune
  {
    public enum AppendSource
    {
      No = 0,
      Yes,
      Yes_Non_Primary
    }

    private bool compactDefs = false;
    private AppendSource prependSourceDicToDef = AppendSource.No;
    private AppendSource appendSourceDicToExamples = AppendSource.No;
    private bool addPlaceholders = false;
    private bool addRubyToExamples = false;
    private string examplePrependText = "▲";
    private bool edictNoWordIndicators = false;
    private bool edictNoP = false;
    private bool jeNoAlphaFallback = false;
    private bool jjKeepExamplesInDefs = false;
    private bool jjRemoveSpecialReadingChars = false;
    private bool jjFillInExampleBlanksWithWord = false;
    private bool useEmbeddedExamples = false;   
  

    /// <summary>
    /// Compact definitions (place the entire definition on a single line)
    /// </summary>
    public bool CompactDefs
    {
      get { return this.compactDefs; }
      set { this.compactDefs = value; }
    }

    /// <summary>
    /// Append short name of source dictionary to definition.
    /// </summary>
    public AppendSource PrependSourceDicToDef
    {
      get { return this.prependSourceDicToDef; }
      set { this.prependSourceDicToDef = value; }
    }

    /// <summary>
    /// Append short name of source dictionary to example sentences
    /// </summary>
    public AppendSource AppendSourceDicToExamples
    {
      get { return this.appendSourceDicToExamples; }
      set { this.appendSourceDicToExamples = value; }
    }

    /// <summary>
    /// Add placeholders to import file for words that were not found.
    /// If in example mode, also add placeholders for words that do not have examples.
    /// </summary>
    public bool AddPlaceholders
    {
      get { return this.addPlaceholders; }
      set { this.addPlaceholders = value; }
    }

    /// <summary>
    /// Add ruby/furigana to example sentences.
    /// Example:
    /// ▲努力の限りを尽くす。  -->  ▲努力[どりょく]の限[かぎ]りを尽[つ]くす。
    /// </summary>
    public bool AddRubyToExamples
    {
      get { return this.addRubyToExamples; }
      set { this.addRubyToExamples = value; }
    }

    /// <summary>
    /// Text to place in front of examples
    /// </summary>
    public string ExamplePrependText
    {
      get { return this.examplePrependText; }
      set { this.examplePrependText = value; }
    }

    /// <summary>
    /// Remove word type indicators [example: (v1,n)].
    /// </summary>
    public bool EdictNoWordIndicators
    {
      get { return this.edictNoWordIndicators; }
      set { this.edictNoWordIndicators = value; }
    }

    /// <summary>
    /// Remove "popular" indicator [example: (P)].
    /// </summary>
    public bool EdictNoP
    {
      get { return this.edictNoP; }
      set { this.edictNoP = value; }
    }

    /// <summary>
    /// Fallback if no alpha characters (a-z or A-Z) are detected.
    /// </summary>
    public bool JeNoAlphaFallback
    {
      get { return this.jeNoAlphaFallback; }
      set { this.jeNoAlphaFallback = value; }
    }

    /// <summary>
    /// Keep examples in the definition.
    /// </summary>
    public bool JjKeepExamplesInDef
    {
      get { return this.jjKeepExamplesInDefs; }
      set { this.jjKeepExamplesInDefs = value; }
    }

    /// <summary>
    /// Remove the '‐' and '･' characters from readings.
    /// </summary>
    public bool JjRemoveSpecialReadingChars
    {
      get { return this.jjRemoveSpecialReadingChars; }
      set { this.jjRemoveSpecialReadingChars = value; }
    }

    /// <summary>
    /// Fill in example sentence blanks with expression.
    /// Example: ▲無罪を___する。  --->  ▲無罪を確信する。
    /// </summary>
    public bool JjFillInExampleBlanksWithWord
    {
      get { return this.jjFillInExampleBlanksWithWord; }
      set { this.jjFillInExampleBlanksWithWord = value; }
    }

        /// <summary>
        /// Treat the second column in the word list as an extra example sentence
        /// </summary>
        public bool UseEmbeddedExamples
        {
            get { return this.useEmbeddedExamples; }
            set { this.useEmbeddedExamples = value;  }
        }


  }
}
