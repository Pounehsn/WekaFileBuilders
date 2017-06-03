using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Core;

namespace Publications.DataLayer
{
    public class PublicationLoader
    {
        public PublicationLoader(FileInfo paperFile, FileInfo authorFile, FileInfo authorForTrainFile)
        {
            if (paperFile == null) throw new ArgumentNullException(nameof(paperFile));
            if (authorFile == null) throw new ArgumentNullException(nameof(authorFile));
            _paperFile = paperFile;
            _authorFile = authorFile;
            _authorForTrainFile = authorForTrainFile;
        }
        private readonly FileInfo _paperFile;
        private readonly FileInfo _authorFile;
        private readonly FileInfo _authorForTrainFile;

        public IEnumerable<PaperDto> ParsePapers() =>
            GetPapersString().Select(ParsPaper);
        public IEnumerable<AuthorDto> ParseAuthor()
        {
            using (var file = new StreamReader(_authorFile.FullName))
            {
                string line;
                while ((line = file.ReadLine()) != null)
                {
                    if (!string.IsNullOrWhiteSpace(line))
                    {
                        var author = new AuthorDto();
                        var parts = line.Split('\t');
                        author.Id = new Id(parts[0]);
                        author.Name = new Name(parts[1]);
                        yield return author;
                    }
                }
            }
        }
        public IEnumerable<AuthorDto> ParseAuthorForTrain()
        {
            using (var file = new StreamReader(_authorForTrainFile.FullName))
            {
                string line;
                while ((line = file.ReadLine()) != null)
                {
                    if (!string.IsNullOrWhiteSpace(line))
                    {
                        var author = new AuthorDto();
                        var parts = line.Split('\t');
                        author.Id = new Id(parts[0]);
                        author.Name = new Name(parts[1]);
                        author.NumberOfCitations = parts.Length > 2
                            ? int.Parse(parts[2])
                            : 0;
                        yield return author;
                    }
                }
            }
        }
        private IEnumerable<string> GetPapersString()
        {
            using (var file = new StreamReader(_paperFile.FullName))
            {
                var sb = new StringBuilder();
                string t;
                while (file.Peek() > 0)
                {
                    var line = file.ReadLine();
                    if (string.IsNullOrEmpty(line))
                    {
                        t = sb.ToString();

                        if (!string.IsNullOrWhiteSpace(t))
                            yield return t;

                        sb.Clear();
                    }
                    else
                    {
                        sb.AppendLine(line);
                    }
                }

                t = sb.ToString();

                if (!string.IsNullOrWhiteSpace(t))
                    yield return t;

                sb.Clear();
            }
        }
        private static PaperDto ParsPaper(string paperText)
        {
            var separator = new[] { Environment.NewLine };
            var paper = new PaperDto();

            var lines = paperText
                .Split(separator, StringSplitOptions.RemoveEmptyEntries)
                .Select(i => i.Trim());

            foreach (var line in lines)
            {
                if (line.StartsWith("#*", StringComparison.OrdinalIgnoreCase))
                {
                    paper.Name = CreateName(line.Substring(2).Trim());
                }
                else if (line.StartsWith("#@"))
                {
                    paper.Authors.AddRange(CreateAuthors(line.Substring(2).Trim()));
                }
                else if (line.StartsWith("#t"))
                {
                    paper.Year = CreateYear(line.Substring(2).Trim());
                }
                else if (line.StartsWith("#c"))
                {
                    paper.Venue = CreateVenue(line.Substring(2).Trim());
                }
                else if (line.StartsWith("#index"))
                {
                    paper.Index = CreateIndex(line.Substring(6).Trim());
                }
                else if (line.StartsWith("#%"))
                {
                    paper.Citations.Add(CreateIndex(line.Substring(2)));
                }
            }

            return paper;
        }
        private static Id CreateIndex(string value) => new Id(value);
        private static Name CreateVenue(string value) => new Name(value);
        private static int CreateYear(string year) => int.Parse(year);
        private static IEnumerable<Name> CreateAuthors(string substring)
        {
            var separator = new[] { ',' };
            var authors = substring
                .Split(separator, StringSplitOptions.RemoveEmptyEntries)
                .Select(i => new Name(i.Trim()));

            return authors;
        }
        private static Name CreateName(string name) => new Name(name);
    }
}
