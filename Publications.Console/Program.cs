using System.Dynamic;
using Publications.DataLayer;
using System.IO;
using System.Linq;
using Core;
using Weka;
using static Weka.WekaDsl;
using Publications.BusinessLogic;

namespace Publications.Console
{
    public static class Program
    {
        private static void Main()
        {
            //TestReadingAuthorsAndPapers();

            //var file = WekaFile(
            //    "weather",
            //    Attributes(
            //        Attr("outlook", Nom("sunny", "overcast", "rainy")),
            //        Attr("temperature", Num)
            //    ),
            //    Instances(
            //        Inst("sunny", "85"),
            //        Inst("sunny", "80")
            //    )
            //);

            CreatePublicationGraph();

            System.Console.ReadLine();
        }

        private static void CreatePublicationGraph()
        {
            var loader = new PublicationLoader(
                new FileInfo(
                    @"D:\Pouneh\Citation Problem\data_15403498_837706864\paper.txt"
                ),
                new FileInfo(
                    @"D:\Pouneh\Citation Problem\data_15403498_837706864\author.txt"
                )
            );

            foreach (var authorDto in loader.ParseAuthor())
            {
                var author = new Author(new Id(authorDto.Id.ToString()), authorDto.Name);
            }

            var numberOfPapers = loader.ParsePapers().Count();
            System.Console.WriteLine($"Overal number of papers {numberOfPapers}");
            var loadedPapers = 0;
            var delta = numberOfPapers / 100;
            foreach (var paperDto in loader.ParsePapers())
            {
                if(loadedPapers++ % delta == 0)
                    System.Console.WriteLine($"{(double)loadedPapers/numberOfPapers:F}");
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

            foreach (var paperDto in loader.ParsePapers())
            {
                if (loadedPapers++ % delta == 0)
                    System.Console.WriteLine($"{(double)loadedPapers / numberOfPapers:F}");

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
        }

        private static void TestReadingAuthorsAndPapers()
        {
            var loader = new PublicationLoader(
                new FileInfo(
                    @"D:\Pouneh\Citation Problem\data_15403498_837706864\paper.txt"
                ),
                new FileInfo(
                    @"D:\Pouneh\Citation Problem\data_15403498_837706864\author.txt"
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
