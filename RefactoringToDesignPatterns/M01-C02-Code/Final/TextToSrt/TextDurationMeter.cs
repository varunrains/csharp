using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace TextToSrt
{
    internal class TextDurationMeter
    {
        private double FullTextLength { get; }
        private TimeSpan FullTextDuration { get; }

        internal TextDurationMeter(IEnumerable<string> fullText, TimeSpan duration)
        {
            this.FullTextLength = fullText.Sum(this.CountReadableLetters);
            this.FullTextDuration = duration;
        }

        public TimeSpan EstimateDuration(string text) =>
            TimeSpan.FromMilliseconds(this.EstimateMilliseconds(text));

        private double EstimateMilliseconds(string text) =>
            this.FullTextDuration.TotalMilliseconds * this.GetRelativeLength(text);

        private double GetRelativeLength(string text) =>
            this.CountReadableLetters(text) / this.FullTextLength;

        private int CountReadableLetters(string text) =>
            Regex.Matches(text, @"\w+").Sum(match => match.Value.Length);
    }
}
