using Publications.DataLayer;
using System.IO;
using System.Linq;
using Core;
using Weka;
using static Weka.WekaDsl;

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

            System.Console.ReadLine();
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
