using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HarvestConsole.Typesetting
{
    class StringTokenizer
    {
        enum TokState
        {
            None,
            Normal,
            Whitespace,
            Escaped,
            Punctuation
        }

        static Dictionary<char[], TokState> lookAheadTable = new Dictionary<char[], TokState>()
        {
            [new[] { ' ', '~' }] = TokState.Whitespace,
            [new[] { '$' }] = TokState.Escaped,
            [new[] { '.', ',' }] = TokState.Punctuation,
        };

        static TokState[] breakStates = new TokState[] { TokState.Whitespace, TokState.Punctuation };

        public static List<string> Tokenize(string input)
        {
            List<string> words = new List<string>();
            if (string.IsNullOrEmpty(input))
                return words;

            int curLetter = 0;
            int wordStart = 0;
            TokState state = TokState.Normal;
            for (; curLetter < input.Length; curLetter++)
            {
                char letter = input[curLetter];
                TokState newState = TokState.None;
                foreach (var entry in lookAheadTable)
                {
                    if (entry.Key.Contains(letter))
                    {
                        newState = entry.Value;
                        break;
                    }
                }

                if (newState == TokState.None && breakStates.Contains(state))
                {
                    // if it's just normal letters and we're in a state that gets broken by that
                    // then we need to return to normal here and also break
                    newState = TokState.Normal;
                }

                if (newState != TokState.None && newState != state)
                {
                    words.Add(input.Substring(wordStart, curLetter - wordStart));
                    wordStart = curLetter;
                    state = newState;
                }
            }

            if (wordStart != curLetter)
            {
                words.Add(input.Substring(wordStart, curLetter - wordStart));
            }

            words = StripEmpty(words);
            NormalizeWhitespace(words);
            TrimWhitespace(words);
            return words;
        }

        private static List<string> StripEmpty(List<string> input)
        {
            return input.Where(x => !string.IsNullOrEmpty(x) && x != "~").ToList();
        }

        private static void NormalizeWhitespace(List<string> input)
        {
            for (int i = 0; i < input.Count; i++)
            {
                if (string.IsNullOrWhiteSpace(input[i]))
                {
                    input[i] = " ";
                }
            }
        }

        private static void TrimWhitespace(List<string> input)
        {
            if (string.IsNullOrWhiteSpace(input[input.Count - 1]))
                input.RemoveAt(input.Count - 1);

            if (string.IsNullOrWhiteSpace(input[0]))
                input.RemoveAt(0);
        }
    }
}
