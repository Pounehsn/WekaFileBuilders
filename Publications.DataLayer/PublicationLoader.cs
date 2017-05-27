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
        public PublicationLoader(FileInfo paperFile, FileInfo authorFile)
        {
            if (paperFile == null) throw new ArgumentNullException(nameof(paperFile));
            if (authorFile == null) throw new ArgumentNullException(nameof(authorFile));
            _paperFile = paperFile;
            _authorFile = authorFile;
        }
        private readonly FileInfo _paperFile;
        private readonly FileInfo _authorFile;

        public IEnumerable<PaperDto> ParsePapers() =>
            GetPapersString().Select(ParsPaper);
        public IEnumerable<AuthorDto> ParseAuthor()
        {
            using (var file = new StreamReader(_authorFile.FullName))
            {
                string line;
                while ((line = file.ReadLine()) != null)
                {
                    var author = new AuthorDto();
                    var parts = line.Split('\t');
                    author.Id = int.Parse(parts[0]);
                    author.Name = new Name(parts[1]);
                    yield return author;
                }
            }
        } 
        private IEnumerable<string> GetPapersString()
        {
            using (var file = new StreamReader(_paperFile.FullName))
            {
                var sb = new StringBuilder();
                string line;
                while ((line = file.ReadLine()) != null)
                {
                    if (line == string.Empty)
                    {
                        yield return sb.ToString();
                        sb.Clear();
                    }
                    else
                    {
                        sb.AppendLine(line);
                    }
                }
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
                    paper.Name = CreateName(line.Substring(2));
                }
                else if (line.StartsWith("#@"))
                {
                    paper.Authors.AddRange(CreateAuthors(line.Substring(2)));
                }
                else if (line.StartsWith("#t"))
                {
                    paper.Year = CreateYear(line.Substring(2));
                }
                else if (line.StartsWith("#c"))
                {
                    paper.Venue = CreateVenue(line.Substring(2));
                }
                else if (line.StartsWith("#index"))
                {
                    paper.Index = CreateIndex(line.Substring(6));
                }
                else if (line.StartsWith("#index"))
                {
                    paper.Index = CreateIndex(line.Substring(6));
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
