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
        public SortedList<int, char> Known { get; private set; }

        /// <summary>
        /// List of charactors that are in the word, but not the correct position
        /// </summary>
        public List<char> Contains { get; private set; }

        /// <summary>
        /// 
        /// </summary>
        public List<char> Invalid { get; private set; }

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
        private int _guess;

        public Solver(int Length)
        {
            Guesses = new SortedList<int, string>();
            Known = new SortedList<int, char>();
            Contains = new List<char>();
            Invalid = new List<char>();

            _guess = 0;
            
            _dictionary = new List<string>();
            ReadFile(Length);
        }

        public void AddGuess(string Input)
        {
            _guess++;
            Guesses.Add(_guess, Input);
        }

        public void AddKnown(int Position, char Value)
        {
            Known.Add(Position, Value);
        }

        public void AddContains(char Value)
        {
            Contains.Add(Value);
        }

        public void AddInvalid(char Value)
        {
            Invalid.Add(Value);
        }

        public List<string> GetOptions()
        {

            var output = _dictionary.AsQueryable();
            
            foreach (var kvp in Known)
            {
                output = output.Where(s => s[kvp.Key] == kvp.Value);
            }

            foreach (var item in Invalid)
            {
                output = output.Where(s => !s.Contains(item));
            }

            foreach (var item in Contains)
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

        private void ReadFile(int Length, string Filename = "words_alpha.txt")
        {
            List<string> invalid = new() { "nilot" };
            using StreamReader reader = new(Filename);
            int x = 1;
            while (!reader.EndOfStream)
            {
                string? word = reader.ReadLine();
                if (!string.IsNullOrWhiteSpace(word) && word.Length == Length)
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
}