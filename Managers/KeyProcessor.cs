using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace OpenSongWeb.Managers
{
    /// <summary>
    /// A helper to assist in key processing.
    /// This thing is surely really incorrect, I did not write this based on sound music theory.
    /// And it is a port of some really old work of mine.
    /// TODO: Find something better.
    /// </summary>
    public class KeyProcessor : IKeyProcessor
    {
        private Dictionary<string, string> m_keyMap;
        private Dictionary<string, int> m_keyVal;
        private Dictionary<int, string> m_valKeysFlat;
        private Dictionary<int, string> m_valKeysSharp;
        private Dictionary<string, int> m_songKeyGoesSharpOrFlat;
        private char[] m_transposeSpecialChars;

        public KeyProcessor()
        {
            Init();
        }

        private void Init()
        {
            if (m_keyMap == null)
            {
                m_keyMap = new Dictionary<string, string>();
                m_keyMap.Add("g", "G");
                m_keyMap.Add("G", "G");
                m_keyMap.Add("Ab", "Ab");
                m_keyMap.Add("ab", "Ab");
                m_keyMap.Add("g#", "G#");
                m_keyMap.Add("G#", "G#");
                //case "gs":
                //case "gsharp":
                m_keyMap.Add("a", "A");
                m_keyMap.Add("A", "A");
                m_keyMap.Add("Bb", "Bb");
                m_keyMap.Add("bb", "Bb");
                m_keyMap.Add("A#", "A#");
                m_keyMap.Add("a#", "A#");
                m_keyMap.Add("b", "B");
                m_keyMap.Add("B", "B");
                m_keyMap.Add("C", "C");
                m_keyMap.Add("c", "C");
                m_keyMap.Add("C#", "C#");
                m_keyMap.Add("c#", "C#");
                m_keyMap.Add("Db", "Db");
                m_keyMap.Add("db", "Db");
                m_keyMap.Add("D", "D");
                m_keyMap.Add("d", "D");
                m_keyMap.Add("D#", "D#");
                m_keyMap.Add("d#", "D#");
                m_keyMap.Add("Eb", "Eb");
                m_keyMap.Add("eb", "Eb");
                m_keyMap.Add("E", "E");
                m_keyMap.Add("e", "E");
                m_keyMap.Add("F", "F");
                m_keyMap.Add("f", "F");
                m_keyMap.Add("f#", "F#");
                m_keyMap.Add("F#", "F#");
                m_keyMap.Add("Gb", "Gb");
                m_keyMap.Add("gb", "Gb");

                m_keyVal = new Dictionary<string, int>();
                m_keyVal.Add("G", 0);
                m_keyVal.Add("G#", 1);
                m_keyVal.Add("Ab", 1);
                m_keyVal.Add("A", 2);
                m_keyVal.Add("A#", 3);
                m_keyVal.Add("Bb", 3);
                m_keyVal.Add("B", 4);
                m_keyVal.Add("C", 5);
                m_keyVal.Add("C#", 6);
                m_keyVal.Add("Db", 6);
                m_keyVal.Add("D", 7);
                m_keyVal.Add("D#", 8);
                m_keyVal.Add("Eb", 8);
                m_keyVal.Add("E", 9);
                m_keyVal.Add("F", 10);
                m_keyVal.Add("F#", 11);
                m_keyVal.Add("Gb", 11);

                m_valKeysFlat = new Dictionary<int, string>();
                m_valKeysFlat.Add(0, "G");
                m_valKeysFlat.Add(1, "Ab");
                m_valKeysFlat.Add(2, "A");
                m_valKeysFlat.Add(3, "Ab");
                m_valKeysFlat.Add(4, "B");
                m_valKeysFlat.Add(5, "C");
                m_valKeysFlat.Add(6, "Db");
                m_valKeysFlat.Add(7, "D");
                m_valKeysFlat.Add(8, "Eb");
                m_valKeysFlat.Add(9, "E");
                m_valKeysFlat.Add(10, "F");
                m_valKeysFlat.Add(11, "Gb");

                m_valKeysSharp = new Dictionary<int, string>();
                m_valKeysSharp.Add(0, "G");
                m_valKeysSharp.Add(1, "G#");
                m_valKeysSharp.Add(2, "A");
                m_valKeysSharp.Add(3, "A#");
                m_valKeysSharp.Add(4, "B");
                m_valKeysSharp.Add(5, "C");
                m_valKeysSharp.Add(6, "C#");
                m_valKeysSharp.Add(7, "D");
                m_valKeysSharp.Add(8, "D#");
                m_valKeysSharp.Add(9, "E");
                m_valKeysSharp.Add(10, "F");
                m_valKeysSharp.Add(11, "F#");

                m_songKeyGoesSharpOrFlat = new Dictionary<string, int>();
                // 1 is sharp, -1 is flat, and zero is special
                m_songKeyGoesSharpOrFlat.Add("g", 1);
                m_songKeyGoesSharpOrFlat.Add("G", 1);
                m_songKeyGoesSharpOrFlat.Add("Ab", -1);
                m_songKeyGoesSharpOrFlat.Add("ab", -1);
                m_songKeyGoesSharpOrFlat.Add("g#", 1);
                m_songKeyGoesSharpOrFlat.Add("G#", 1);
                //case "gs":
                //case "gsharp":
                m_songKeyGoesSharpOrFlat.Add("a", 1);
                m_songKeyGoesSharpOrFlat.Add("A", 1);
                m_songKeyGoesSharpOrFlat.Add("Bb", -1);
                m_songKeyGoesSharpOrFlat.Add("bb", -1);
                m_songKeyGoesSharpOrFlat.Add("A#", 1);
                m_songKeyGoesSharpOrFlat.Add("a#", 1);
                m_songKeyGoesSharpOrFlat.Add("b", 1);
                m_songKeyGoesSharpOrFlat.Add("B", 1);
                m_songKeyGoesSharpOrFlat.Add("C", 0);
                m_songKeyGoesSharpOrFlat.Add("c", 0);
                m_songKeyGoesSharpOrFlat.Add("C#", 1);
                m_songKeyGoesSharpOrFlat.Add("c#", 1);
                m_songKeyGoesSharpOrFlat.Add("Db", -1);
                m_songKeyGoesSharpOrFlat.Add("db", -1);
                m_songKeyGoesSharpOrFlat.Add("D", 1);
                m_songKeyGoesSharpOrFlat.Add("d", 1);
                m_songKeyGoesSharpOrFlat.Add("D#", 1);
                m_songKeyGoesSharpOrFlat.Add("d#", 1);
                m_songKeyGoesSharpOrFlat.Add("Eb", -1);
                m_songKeyGoesSharpOrFlat.Add("eb", -1);
                m_songKeyGoesSharpOrFlat.Add("E", 1);
                m_songKeyGoesSharpOrFlat.Add("e", 1);
                m_songKeyGoesSharpOrFlat.Add("F", -1);
                m_songKeyGoesSharpOrFlat.Add("f", -1);
                m_songKeyGoesSharpOrFlat.Add("f#", 1);
                m_songKeyGoesSharpOrFlat.Add("F#", 1);
                m_songKeyGoesSharpOrFlat.Add("Gb", -1);
                m_songKeyGoesSharpOrFlat.Add("gb", -1);
                m_songKeyGoesSharpOrFlat.Add("Am", 0);
                m_songKeyGoesSharpOrFlat.Add("am", 0);
                m_songKeyGoesSharpOrFlat.Add("Bbm", -1);
                m_songKeyGoesSharpOrFlat.Add("bbm", -1);
                m_songKeyGoesSharpOrFlat.Add("Cm", -1);
                m_songKeyGoesSharpOrFlat.Add("cm", -1);
                m_songKeyGoesSharpOrFlat.Add("Dm", -1);
                m_songKeyGoesSharpOrFlat.Add("dm", -1);
                m_songKeyGoesSharpOrFlat.Add("Fm", -1);
                m_songKeyGoesSharpOrFlat.Add("fm", -1);
                m_songKeyGoesSharpOrFlat.Add("Gm", -1);
                m_songKeyGoesSharpOrFlat.Add("gm", -1);
                m_songKeyGoesSharpOrFlat.Add("G#m", 1);
                m_songKeyGoesSharpOrFlat.Add("g#m", 1);
                m_songKeyGoesSharpOrFlat.Add("C#m", 1);
                m_songKeyGoesSharpOrFlat.Add("c#m", 1);
                m_songKeyGoesSharpOrFlat.Add("D#m", 1);
                m_songKeyGoesSharpOrFlat.Add("d#m", 1);
                m_songKeyGoesSharpOrFlat.Add("Em", 1);
                m_songKeyGoesSharpOrFlat.Add("em", 1);
                m_songKeyGoesSharpOrFlat.Add("F#m", 1);
                m_songKeyGoesSharpOrFlat.Add("f#m", 1);

                m_transposeSpecialChars = new char[]
                {
                    '/',
                    '\\',
                    ')',
                    '(',
                    ']',
                    '[',
                    '_',
                    '-',
                    '!',
                    '~',
                    '`',
                    '@',
                    '$',
                    '%',
                    '^',
                    '&',
                    '*',
                    '+',
                    '=',
                    '?',
                    '\'',
                    '"',
                    '|',
                    '{',
                    '}',
                    ';',
                    ':',
                    '1',
                    '2',
                    '3',
                    '4',
                    '5',
                    '6',
                    '7',
                    '8',
                    '9',
                    '0',
                    'H',
                    'h',
                    'I',
                    'i',
                    'J',
                    'j',
                    'K',
                    'k',
                    'L',
                    'l',
                    'M',
                    'm',
                    'N',
                    'n',
                    'O',
                    'o',
                    'P',
                    'p',
                    'Q',
                    'q',
                    'R',
                    'r',
                    'S',
                    's',
                    'T',
                    't',
                    'U',
                    'u',
                    'V',
                    'v',
                    'W',
                    'w',
                    'X',
                    'x',
                    'Y',
                    'y',
                    'Z',
                    'z'
                };
            }
        }

        /// <summary>
        /// Separate a key into its two parts.
        /// TODO: I think We should be able to go simpler then this by determining the longest case-agnostic match found in the m_keyVal keys, and then the remainder becomes the "suffix"?
        /// </summary>
        /// <param name="strKeyIn"></param>
        /// <param name="strTheKey"></param>
        /// <param name="strNoteSuffix"></param>
        /// <returns></returns>
        public bool ParseKey(string strKeyIn, out string strTheKey, out string strNoteSuffix)
        {
            strNoteSuffix = string.Empty;
            bool bResult;
            if (string.IsNullOrEmpty(strKeyIn))
            {
                strTheKey = "";
                bResult = false;
            }
            else
            {
                while (strKeyIn.Length > 0 && Regex.IsMatch(strKeyIn.Substring(strKeyIn.Length - 1, 1), "\\d"))
                {
                    strNoteSuffix = strKeyIn.Substring(strKeyIn.Length - 1, 1) + strNoteSuffix;
                    strKeyIn = strKeyIn.Substring(0, strKeyIn.Length - 1);
                }
                if (strKeyIn.Length < 4 && strKeyIn.Length > 1 && strKeyIn.EndsWith("m", System.StringComparison.InvariantCultureIgnoreCase))
                {
                    strNoteSuffix = strKeyIn.Substring(strKeyIn.Length - 1) + strNoteSuffix;
                    strKeyIn = strKeyIn.Substring(0, strKeyIn.Length - 1);
                }
                else if (strKeyIn.Length < 6 && strKeyIn.Length > 3)
                {
                    if (strKeyIn.EndsWith("min", System.StringComparison.InvariantCultureIgnoreCase) || strKeyIn.EndsWith("maj", System.StringComparison.InvariantCultureIgnoreCase) || strKeyIn.EndsWith("sus", System.StringComparison.InvariantCultureIgnoreCase) || strKeyIn.EndsWith("dim", System.StringComparison.InvariantCultureIgnoreCase))
                    {
                        strNoteSuffix = strKeyIn.Substring(strKeyIn.Length - 3) + strNoteSuffix;
                        strKeyIn = strKeyIn.Substring(0, strKeyIn.Length - 3);
                    }
                    else if (strKeyIn.IndexOf("add", System.StringComparison.InvariantCultureIgnoreCase) > 0)
                    {
                        strNoteSuffix = strKeyIn.Substring(strKeyIn.IndexOf("add", System.StringComparison.InvariantCultureIgnoreCase)) + strNoteSuffix;
                        strKeyIn = strKeyIn.Substring(0, strKeyIn.IndexOf("add", System.StringComparison.InvariantCultureIgnoreCase));
                    }
                }

                while (strKeyIn.Length > 0 && Regex.IsMatch(strKeyIn.Substring(strKeyIn.Length - 1, 1), "\\d"))
                {
                    strNoteSuffix = strKeyIn.Substring(strKeyIn.Length - 1, 1) + strNoteSuffix;
                    strKeyIn = strKeyIn.Substring(0, strKeyIn.Length - 1);
                }

                bool success = m_keyMap.ContainsKey(strKeyIn);
                if (success)
                {
                    strTheKey = m_keyMap[strKeyIn];
                }
                else
                {
                    strTheKey = "--";
                }

                bResult = success;
            }

            return bResult;
        }
    }
}
