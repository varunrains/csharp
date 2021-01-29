using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace TextToSrt
{
    class SentenceRules
    {
        private IEnumerable<(string pattern, string extract, string remove)> Rules { get; } = new[]
        {
            (@"^(?<remove>(?<extract>(\.\.\.|[^\.])+)\.)$", "${extract}", "${remove}"),
            (@"^(?<remove>(?<extract>[^\.]+),)$", "${extract}", "${remove}"),
            (@"^(?<remove>(?<extract>(\.\.\.|[^\.])+)\.)[^\.].*$", "${extract}", "${remove}"),
            (@"^(?<remove>(?<extract>[^:]+):).*$", "${extract}", "${remove}"),
            (@"^(?<extract>.+\?).*$", "${extract}", "${extract}"),
            (@"^(?<extract>.+\!).*$", "${extract}", "${extract}"),
        };

        public IEnumerable<string> Split(IEnumerable<string> text) => 
            text.SelectMany(this.BreakSentences);

        private IEnumerable<string> BreakSentences(string text)
        {
            string remaining = text.Trim();
            while (remaining.Length > 0)
            {
                (string extracted, string rest) =
                    this.FindShortestExtractionRule(this.Rules, remaining)
                        .Select(tuple => (
                            extracted: tuple.extracted, 
                            removedLength: tuple.remove.Length))
                        .Select(tuple => (
                            extracted: tuple.extracted, 
                            remaining: remaining.Substring(tuple.removedLength).Trim()))
                        .DefaultIfEmpty((extracted: remaining, remaining: string.Empty))
                        .First();

                yield return extracted;
                remaining = rest;
            }
        }

        private IEnumerable<(string extracted, string remove)> FindShortestExtractionRule(
            IEnumerable<(string pattern, string extractPattern, string removePattern)> rules,
            string text) =>
            rules
                .Select(rule => (
                    pattern: new Regex(rule.pattern), 
                    extractPattern: rule.extractPattern, 
                    removePattern: rule.removePattern))
                .Select(rule => (
                    pattern: rule.pattern, 
                    match: rule.pattern.Match(text), 
                    extractPattern: rule.extractPattern, 
                    removePattern: rule.removePattern))
                .Where(rule => rule.match.Success)
                .Select(rule => (
                    extracted: rule.pattern.Replace(text, rule.extractPattern), 
                    remove: rule.pattern.Replace(text, rule.removePattern)))
                .WithMinimumOrEmpty(tuple => tuple.remove.Length);
    }
}
