using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace TextToSrt
{
    class Captions
    {
        private IEnumerable<CaptionLine> Lines { get; }

        public Captions(IEnumerable<CaptionLine> lines)
        {
            this.Lines = lines.ToList();
        }

        public static Captions Parse(string[] text, TimeSpan clipDuration)
        {
            IEnumerable<string> lines = BreakLongLines(
                BreakIntoSentences(Cleanup(text)),
                95, 45).ToList();
            TextDurationMeter durationMeter = new TextDurationMeter(lines, clipDuration);
            IEnumerable<CaptionLine> captions = lines
                .Select(line => (text: line, duration: durationMeter.EstimateDuration(line)))
                .Select(tuple => new CaptionLine(tuple.text, tuple.duration));
            return new Captions(captions);
        }
        
        private static IEnumerable<string> BreakLongLines(
            IEnumerable<string> text, int maxLineCharacters, int minBrokenLength) =>
            new LinesBreaker().BreakLongLines(text, maxLineCharacters, minBrokenLength);

        private static IEnumerable<string> BreakIntoSentences(IEnumerable<string> text) =>
            new SentenceRules().Split(text);

        public void SaveAsSrt(FileInfo destination) => 
            File.WriteAllLines(destination.FullName, this.GenerateSrtFileContent());

        private IEnumerable<string> GenerateSrtFileContent() =>
            this.GenerateLineBoundaries()
                .SelectMany((tuple, index) =>
                    new[]
                    {
                        $"{index + 1}",
                        $"{tuple.begin:hh\\:mm\\:ss\\,fff} --> {tuple.end:hh\\:mm\\:ss\\,fff}",
                        $"{tuple.content}",
                        string.Empty
                    });

        private IEnumerable<(TimeSpan begin, TimeSpan end, string content)> GenerateLineBoundaries()
        {
            TimeSpan begin = new TimeSpan(0);
            foreach (CaptionLine line in this.Lines)
            {
                TimeSpan end = begin + line.Duration;
                yield return (begin, end, line.Content);
                begin = end;
            }
        }

        private static IEnumerable<string> Cleanup(string[] text) =>
            text
                .Select(line => line.Trim())
                .Where(line => line.Length > 0);
    }
}
