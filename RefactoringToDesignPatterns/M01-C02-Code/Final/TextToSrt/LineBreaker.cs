using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace TextToSrt
{
    class LinesBreaker
    {
        private IEnumerable<(string separatorPattern, string appendLeft, string prependRight)[]> 
            Rules { get; } = new []
        {
            new[] 
            {            
                (", ", "...", "... "),
                ("; ", "...", "... "),
                (" - ", "...", "... "),
            },
            new[] 
            {            
                (" and ", "...", "... and "),
                (" or ", "...", "... or "),
            },
            new[] 
            {            
                (" to ", "...", "... to "),
                (" then ", "...", "... then "),
            },
            new[] 
            {            
                (" ", "...", "... ")
            },
        };

        public IEnumerable<string> BreakLongLines(
            IEnumerable<string> text, int maxLineCharacters, int minBrokenLength) =>
            text.SelectMany(line => this.BreakLongLine(line, maxLineCharacters, minBrokenLength));

        public IEnumerable<string> BreakLongLine(string line, int maxLength, int minBrokenLength)
        {
            string remaining = line;

            while (remaining.Length > 0)
            {
                if (remaining.Length <= maxLength)
                {
                    yield return remaining;
                    break;
                }

                bool broken = false;
                foreach ((string separator, string toLeft, string toRight)[] rules in this.Rules)
                {
                    IEnumerable<(string left, string right)> split =
                        this.TryBreakLongLine(remaining, rules, maxLength, minBrokenLength)
                            .ToList();

                    if (split.Any())
                    {
                        (string left, string right) = split.First();
                        yield return left;
                        remaining = right;
                        broken = true;
                        break;
                    }
                }

                if (!broken)
                {
                    yield return remaining;
                    break;
                }
            }
        }

        private IEnumerable<(string left, string right)> TryBreakLongLine(
            string line, 
            IEnumerable<(string separatorPattern, string appendLeft, string prependRight)> rules,
            int maxLength, int minBrokenLength) =>
            rules.SelectMany(rule => this.BreakLongLine(line, rule, maxLength, minBrokenLength))
                .WithMinimumOrEmpty(split => maxLength - split.left.Length);

        private IEnumerable<(string left, string right)> BreakLongLine(
            string line, 
            (string separatorPattern, string appendLeft, string prependRight) rule, 
            int maxLength, int minBrokenLength) => 
            new Regex(rule.separatorPattern).Matches(line)
                .Select(match => (
                    left: line.Substring(0, match.Index) + rule.appendLeft, 
                    right: rule.prependRight + line.Substring(match.Index + match.Length)))
                .Where(split => 
                    minBrokenLength <= split.left.Length && 
                    split.left.Length <= maxLength);
    }
}