using System.Linq;

namespace WorldSolver.Common
{
    public class Solver
    {
        /// <summary>
        /// List containing the guesses
        /// </summary>
        public SortedList<int,string> Guesses { get; private set; }

        /// <summary>
        /// List of known characters by their position
        /// </summary>
        public SortedList<int, char> Correct { get; private set; }

        /// <summary>
        /// List of charactors that are in the word, but not the correct position
        /// </summary>
        public List<char> Present { get; private set; }

        /// <summary>
        /// Letters known to be invalid
        /// </summary>
        public List<char> Absent { get; private set; }

        /// <summary>
        /// Returns the the count of each letter from the remaining word in the dictionary. This is used as part of the weighting process for the next guess.
        /// </summary>
        public Dictionary<char, int> LetterCount
        {
            get
            {
                string alphabet = "abcdefghijklmnopqrstuvwxyz";
                Dictionary<char, int> output = new();
                var options = GetOptions();
                foreach (var letter in alphabet)
                {
                    int count = 0;
                    foreach (var value in options)
                    {
                        count += value.Count(s => s == letter);
                    }
                    output.Add(letter, count);
                }
                return output;
            }
        }

        private readonly List<string> _dictionary;
        private int _guessCount;

        public Solver(int length)
        {
            Guesses = new SortedList<int, string>();
            Correct = new SortedList<int, char>();
            Present = new List<char>();
            Absent = new List<char>();

            _guessCount = 0;
            
            _dictionary = new List<string>();
            ReadFile(length);
        }

        public void AddGuess(string input)
        {
            _guessCount++;
            Guesses.Add(_guessCount, input);
        }

        public void AddGuess(GuessResult result)
        {
            AddGuess(result.Guess);
            foreach (var item in result.ResultList)
            {
                switch (item.Result)
                {
                    case LetterResult.Absent:
                        AddAbsent(item.Letter);
                        break;
                    case LetterResult.Correct:
                        AddCorrect(item.Position, item.Letter);
                        if (Present.Contains(item.Letter))
                        {
                            Present.Remove(item.Letter);
                        }
                        break;
                    case LetterResult.Present:
                        AddPresent(item.Letter);
                        break;
                }
            }
        }

        public void AddCorrect(int position, char value)
        {
            Correct.Add(position, value);
        }

        public void AddPresent(char value)
        {
            Present.Add(value);
        }

        public void AddAbsent(char value)
        {
            Absent.Add(value);
        }

        /// <summary>
        /// Get the list of all words that fit with the current criteria
        /// </summary>
        /// <returns>List of all remaining words that match the current criteria</returns>
        public List<string> GetOptions()
        {
            var output = _dictionary.AsQueryable(); //currently using full dictionary each time, could improve by updating base dictionary after a guess is added
            
            foreach (var kvp in Correct)
            {
                output = output.Where(s => s[kvp.Key] == kvp.Value);
            }

            foreach (var item in Absent)
            {
                output = output.Where(s => !s.Contains(item));
            }

            foreach (var item in Present)
            {
                output = output.Where(s => s.Contains(item));

                foreach (var guess in Guesses.Values)
                {
                    int pos = guess.IndexOf(item);
                    if (pos > -1)
                    {
                        output = output.Where(s => s[pos] != item);
                    }
                }
            }

            output = output.Except(Guesses.Values);
            return output.ToList();
        }

        /// <summary>
        /// Gets the next guess based on the current current result set.
        /// </summary>
        /// <returns>The top weighted guess</returns>
        public string GetGuess()
        {
            var options = GetOptions();
            var letterCount = LetterCount;
            Dictionary<string, int> wordWeight = new();
            
            foreach (var option in options)
            {
                int weight = 0;
                foreach (var letter in option.Distinct())
                {
                    weight += letterCount[letter];
                }
                wordWeight.Add(option, weight);
            }

            return wordWeight.OrderByDescending(w => w.Value).First().Key;
        }

        private void ReadFile(int length, string filename = "words_alpha.txt")
        {
            List<string> invalid = new() { "nilot" };
            using StreamReader reader = new(filename);
            int x = 1;
            while (!reader.EndOfStream)
            {
                string? word = reader.ReadLine();
                if (!string.IsNullOrWhiteSpace(word) && word.Length == length)
                {
                    //Console.WriteLine($"{x} - {word}");
                    _dictionary.Add(word);
                    x++;
                }
            }
            _dictionary.RemoveAll(w => invalid.Contains(w));
            Console.WriteLine($"Loaded {_dictionary.Count} words");
        }
    }

    public class GuessResult
    {
        public string Guess { get; set; } = String.Empty;
        public List<GuessLetterResult> ResultList { get; set; } = new List<GuessLetterResult>();
    }

    public class GuessLetterResult
    {
        public int Position { get; set; }
        public char Letter { get; set; }
        public LetterResult Result { get;set; }
    }

    public enum LetterResult
    {
        Absent,
        Present,
        Correct
    }
}