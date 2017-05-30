using System.Globalization;
using Publications.DataLayer;
using System.IO;
using System.Linq;
using Core;
using Publications.BusinessLogic;
using static Weka.WekaDsl;

namespace Publications.Console
{
    public static class Program
    {
        private static void Main()
        {
            //TestReadingAuthorsAndPapers();

            CreatePublicationGraph();

            System.Console.ReadLine();
        }

        private static void CreatePublicationGraph()
        {
            var loader = new PublicationLoader(
                new FileInfo(
                    @"C:\Pouneh\Education\Tsinghua\Machine Learning\HW3\Data\paper.txt"
                ),
                new FileInfo(
                    @"C:\Pouneh\Education\Tsinghua\Machine Learning\HW3\Data\author.txt"
                ),
                new FileInfo(
                    @"C:\Pouneh\Education\Tsinghua\Machine Learning\HW3\Data\citation_train.txt"
                )
            );
            //var loader = new PublicationLoader(
            //    new FileInfo(
            //        @"D:\Pouneh\Citation Problem\Test files\test.txt"
            //    ),
            //    new FileInfo(
            //        @"D:\Pouneh\Citation Problem\Test files\Authors.txt"
            //    ),
            //    new FileInfo(
            //        @"D:\Pouneh\Citation Problem\Test files\Authors.txt"
            //    )
            //);

            foreach (var authorDto in loader.ParseAuthorForTrain())
            {
                // ReSharper disable once UnusedVariable
                var author = new Author(
                    new Id(authorDto.Id.ToString()),
                    authorDto.Name,
                    authorDto.NumberOfCitations
                );
            }
            foreach (var authorDto in loader.ParseAuthor())
            {
                // ReSharper disable once UnusedVariable
                var author = Author.GetOrCreateInstance(
                    new Id(authorDto.Id.ToString()),
                    id => new Author(
                        new Id(authorDto.Id.ToString()),
                        authorDto.Name,
                        -1
                    )
                );
            }
            //System.Console.WriteLine($"Author : \n[{string.Join("\n", Author.GetAll())}]");

            var allJob = loader.ParsePapers().Count();
            System.Console.WriteLine($"Overal number of papers {allJob}");
            var doneJob = 0;
            var delta = allJob / 100 == 0 ? 1 : allJob / 100;
            foreach (var paperDto in loader.ParsePapers())
            {
                if (doneJob++ % delta == 0)
                    System.Console.WriteLine($"{(double)doneJob / allJob:P}");

                var paper = Paper.GetOrCreateInstance(paperDto.Index, id => new Paper(id));
                paper.Name = paperDto.Name;
                paperDto.Authors.ForEach(
                    author => paper.AddAuthor(Author.GetByName(author))
                );
                paper.Venue = Venue.GetOrCreateInstance(
                    new Id(paperDto.Venue.Value ?? "NotSet"),
                    id => new Venue(id)
                );
                paper.AddYear(paperDto.Year);
            }
            doneJob = 0;
            foreach (var paperDto in loader.ParsePapers())
            {
                if (doneJob++ % delta == 0)
                    System.Console.WriteLine($"{(double)doneJob / allJob:P}");

                var paper = Paper.Get(paperDto.Index);

                paperDto.Citations.ForEach(
                    pi =>
                    {
                        Paper p;
                        if (Paper.TryGet(new Id(pi.Value), out p))
                            paper.AddCitation(p);
                    }
                );
            }
            //System.Console.WriteLine($"Papers : \n[{string.Join("\n", Paper.GetAll())}]");

            var authers = Author.AllAuthors;

            var wekaFile = WekaFile(
                "Publication",
                Attributes(
                    Attr("Id", Num),
                    Attr("HIndex", Num),
                    Attr("GIndex", Num),
                    Attr("AutherRank", Num),
                    Attr("AutherHotRank", Num),
                    Attr("YearsOfExperience", Num),
                    Attr("IdleYearsBefore2011", Num),
                    Attr("NumberOfPublication", Num),
                    Attr("Productivity", Num),
                    Attr("Coauthers", Num),
                    Attr("UniqueCoauthers", Num),
                    Attr("Citation2000", Num),
                    Attr("Citation2001", Num),
                    Attr("Citation2002", Num),
                    Attr("Citation2003", Num),
                    Attr("Citation2004", Num),
                    Attr("Citation2005", Num),
                    Attr("Citation2006", Num),
                    Attr("Citation2007", Num),
                    Attr("Citation2008", Num),
                    Attr("Citation2009", Num),
                    Attr("Citation2010", Num),
                    Attr("Citation2011", Num),
                    Attr("TotalFirstYearCitationsUntil2011", Num),
                    Attr("TotalCitationUntil2011", Num),
                    Attr("CitationOn2016", Num)
                ),
                authers.Where(a => a.NumberOfCitationsOn2016 >= 0).Select(i => Inst(GetValues(i)))
            );

            using (var file = new StreamWriter(@"D:\Pouneh\Citation Problem\data_15403498_837706864\WekaInput.arff"))
            {
                foreach (var fileLine in wekaFile.GetFileLines())
                {
                    file.WriteLine(fileLine);
                }
            }
        }

        private static string[] GetValues(Author author)
        {
            var startYear = author.StartOfActivity;
            var endYear = author.LastYearOfActivity;
            var yearsOfExperience = author.YearsOfExperience;
            return new[]
            {
                author.Id.ToString(),
                author.HIndex.ToString(),
                author.GIndex.ToString(),
                $"{author.AuthorRank(startYear, endYear):F}",
                $"{author.AuthorHotRank(startYear, endYear):F}",
                yearsOfExperience.ToString(),
                (2011 - endYear).ToString(),
                author.NumberOfPublication.ToString(),
                $"{(double)author.NumberOfPublication / yearsOfExperience:F}",
                author.NumberOfCoauthers.ToString(CultureInfo.InvariantCulture),
                author.NumberOfUniqueCoauthers.ToString(CultureInfo.InvariantCulture),
                author.NumberOfCitationsInYear(2000).ToString(CultureInfo.InvariantCulture),
                author.NumberOfCitationsInYear(2001).ToString(CultureInfo.InvariantCulture),
                author.NumberOfCitationsInYear(2002).ToString(CultureInfo.InvariantCulture),
                author.NumberOfCitationsInYear(2003).ToString(CultureInfo.InvariantCulture),
                author.NumberOfCitationsInYear(2004).ToString(CultureInfo.InvariantCulture),
                author.NumberOfCitationsInYear(2005).ToString(CultureInfo.InvariantCulture),
                author.NumberOfCitationsInYear(2006).ToString(CultureInfo.InvariantCulture),
                author.NumberOfCitationsInYear(2007).ToString(CultureInfo.InvariantCulture),
                author.NumberOfCitationsInYear(2008).ToString(CultureInfo.InvariantCulture),
                author.NumberOfCitationsInYear(2009).ToString(CultureInfo.InvariantCulture),
                author.NumberOfCitationsInYear(2010).ToString(CultureInfo.InvariantCulture),
                author.NumberOfCitationsInYear(2011).ToString(CultureInfo.InvariantCulture),
                author.TotalFirstYearCitationsUntil(2011).ToString(CultureInfo.InvariantCulture),
                author.TotalCitationsUntil(2011).ToString(CultureInfo.InvariantCulture),
                author.NumberOfCitationsOn2016.ToString(CultureInfo.InvariantCulture)
            };
        }

        private static void TestReadingAuthorsAndPapers()
        {
            var loader = new PublicationLoader(
                new FileInfo(
                    @"D:\Pouneh\Citation Problem\data_15403498_837706864\paper.txt"
                ),
                new FileInfo(
                    @"D:\Pouneh\Citation Problem\data_15403498_837706864\author.txt"
                ),
                new FileInfo(
                    @"D:\Pouneh\Citation Problem\data_15403498_837706864\citation_train.txt"
                )
            );

            var papers = loader.ParsePapers();

            foreach (var paper in papers.Select(i => i.ToString()).Take(10))
            {
                System.Console.WriteLine(new string('-', 80));
                System.Console.WriteLine(paper);
            }

            foreach (var result in loader.ParseAuthor().Select(i => i.ToString()).Take(10))
            {
                System.Console.WriteLine(new string('-', 80));
                System.Console.WriteLine(result);
            }
        }
    }
}
