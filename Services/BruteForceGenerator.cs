using System.Collections.Generic;
using System.Linq;

namespace PasswordReset.Services;

public class BruteForceGenerator
{
    private static readonly char[] Alphabet = "abcdefghijklmnopqrstuvwxyz".ToCharArray();
    private const int MinLength = 1;
    private const int MaxLength = 6;

    public IEnumerable<string> GenerateCombinations()
    {
        for (int length = MinLength; length <= MaxLength; length++)
        {
            IEnumerable<string> combinations = [""];

            // Build all words of the current length from the alphabet.
            combinations = Enumerable.Repeat(Alphabet, length).Aggregate(combinations, (acc, chars) => acc.SelectMany(word => chars, (word, ch) => word + ch));

            // Yield one password at a time instead of storing everything in memory.
            foreach (string combination in combinations)
                yield return combination;
        }
    }
}
