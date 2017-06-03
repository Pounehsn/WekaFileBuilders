using System;
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

            CreatePublicationGraph("citation_test.txt");
            CreatePublicationGraph("citation_train.txt");
            System.Console.ReadLine();
        }

        private static void CreatePublicationGraph(string fileName)
        {
            var loader = new PublicationLoader(
                new FileInfo(
                    @"D:\Pouneh\Citation Problem\data_15403498_837706864\paper.txt"
                ),
                new FileInfo(
                    @"D:\Pouneh\Citation Problem\data_15403498_837706864\author.txt"
                ),
                new FileInfo(
                    $@"D:\Pouneh\Citation Problem\data_15403498_837706864\{fileName}"
                )
            );

            foreach (var authorDto in loader.ParseAuthorForTrain())
            {
                // ReSharper disable once UnusedVariable
                var author = Author.GetOrCreateInstance(
                    new Id(authorDto.Id.ToString()),
                    id => new Author(
                        new Id(authorDto.Id.ToString()),
                        authorDto.Name,
                        authorDto.NumberOfCitations
                    )
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

            var authers = Author.AllAuthors.ToArray();

            // X = experience
            // Mu = averag experience
            // n = number of authors
            // Sigma = Standard deviation (Enheraf meiar)
            // Standard deviation (SD, Sigma) = Sqrt(Sum((Xi-Mu)^2)/n)
            // Standardize experience = (X - Mu) / Sigma

            var mu = authers.Select(i => i.YearsOfExperience).Average();
            var n = authers.Length;
            var sd = Math.Sqrt(
                authers.Sum(
                    xi =>
                        (xi.YearsOfExperience - mu) *
                        (xi.YearsOfExperience - mu)
                ) / n
            );

            var wekaFile = WekaFile(
                "Publication",
                Attributes(
                    Attr("Id", Num),
                    //Attr("HIndex", Num),
                    //Attr("GIndex", Num),
                    //Attr("AutherRank", Num),
                    //Attr("AutherHotRank", Num),
                    //Attr("YearsOfExperience", Num),
                    //Attr("IdleYearsBefore2011", Num),
                    //Attr("NumberOfPublication", Num),
                    //Attr("Productivity", Num),
                    Attr("Coauthers", Num),
                    Attr("UniqueCoauthers", Num),
                    //Attr("Citation2000", Num),
                    //Attr("Citation2001", Num),
                    //Attr("Citation2002", Num),
                    //Attr("Citation2003", Num),
                    //Attr("Citation2004", Num),
                    //Attr("Citation2005", Num),
                    //Attr("Citation2006", Num),
                    //Attr("Citation2007", Num),
                    //Attr("Citation2008", Num),
                    Attr("Citation2009", Num),
                    Attr("Citation2010", Num),
                    Attr("Citation2011", Num),
                    //Attr("StandardizedExperience", Num),
                    //Attr("TotalFirstYearCitationsUntil2011", Num),
                    Attr("TotalCitationUntil2011", Num),
                    Attr("CitationOn2016", Num)
                ),
                authers
                    .Where(a => a.NumberOfCitationsOn2016 >= 0)
                    .Select(i => Inst(GetValues(i, mu, sd)))
            );

            using (
                var file = new StreamWriter(
                    $@"D:\Pouneh\Citation Problem\data_15403498_837706864\WekaInput_{fileName}.arff"
                )
            )
            {
                foreach (var fileLine in wekaFile.GetFileLines())
                {
                    file.WriteLine(fileLine);
                }
            }
        }

        private static string[] GetValues(
            Author author,
            double averageExperience,
            double standardDeviation
        )
        {
            //var startYear = author.StartOfActivity;
            //var endYear = author.LastYearOfActivity;
            //var yearsOfExperience = author.YearsOfExperience;
            return new[]
            {
                author.Id.ToString(),
                //author.HIndex.ToString(),
                //author.GIndex.ToString(),
                //$"{author.AuthorRank(startYear, endYear):F}",
                //$"{author.AuthorHotRank(startYear, endYear):F}",
                //yearsOfExperience.ToString(),
                //(2011 - endYear).ToString(),
                //author.NumberOfPublication.ToString(),
                //$"{(double)author.NumberOfPublication / yearsOfExperience:F}",
                author.NumberOfCoauthers.ToString(CultureInfo.InvariantCulture),
                author.NumberOfUniqueCoauthers.ToString(CultureInfo.InvariantCulture),
                //author.NumberOfCitationsInYear(2000).ToString(CultureInfo.InvariantCulture),
                //author.NumberOfCitationsInYear(2001).ToString(CultureInfo.InvariantCulture),
                //author.NumberOfCitationsInYear(2002).ToString(CultureInfo.InvariantCulture),
                //author.NumberOfCitationsInYear(2003).ToString(CultureInfo.InvariantCulture),
                //author.NumberOfCitationsInYear(2004).ToString(CultureInfo.InvariantCulture),
                //author.NumberOfCitationsInYear(2005).ToString(CultureInfo.InvariantCulture),
                //author.NumberOfCitationsInYear(2006).ToString(CultureInfo.InvariantCulture),
                //author.NumberOfCitationsInYear(2007).ToString(CultureInfo.InvariantCulture),
                //author.NumberOfCitationsInYear(2008).ToString(CultureInfo.InvariantCulture),
                author.NumberOfCitationsInYear(2009).ToString(CultureInfo.InvariantCulture),
                author.NumberOfCitationsInYear(2010).ToString(CultureInfo.InvariantCulture),
                author.NumberOfCitationsInYear(2011).ToString(CultureInfo.InvariantCulture),
                // Standardize experience = (X - Mu) / Sigma
                //(
                //    (author.YearsOfExperience - averageExperience) / standardDeviation 
                //).ToString(CultureInfo.InvariantCulture),
                //author.TotalFirstYearCitationsUntil(2011).ToString(CultureInfo.InvariantCulture),
                author.TotalCitationsUntil(2011).ToString(CultureInfo.InvariantCulture),
                author.NumberOfCitationsOn2016.ToString(CultureInfo.InvariantCulture)
            };
        }
    }
}
