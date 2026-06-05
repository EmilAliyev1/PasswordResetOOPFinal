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

            combinations = Enumerable.Repeat(Alphabet, length).Aggregate(combinations, (acc, chars) => acc.SelectMany(word => chars, (word, ch) => word + ch));

            foreach (string combination in combinations)
                yield return combination;
        }
    }
}