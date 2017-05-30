using System.Collections.Generic;
using System.Linq;
using Core;

namespace Publications.BusinessLogic
{
    public class Author : ObjectWithUniqueId<Author>
    {
        public Author(Id id, Name name, int numberOfCitations) : base(id)
        {
            Name = name;
            NumberOfCitations = numberOfCitations;
            NameToAuthor.Add(name, this);
        }


        private static readonly Dictionary<Name, Author> NameToAuthor = new Dictionary<Name, Author>();
        private readonly Relationship<Paper> _papers = new Relationship<Paper>();

        public static IEnumerable<Author> AllAuthors => NameToAuthor.Values;
        public static Author GetByName(Name name) => NameToAuthor[name];
        public Name Name { get; }
        public int NumberOfCitations { get; }
        public IEnumerable<Paper> Papers => _papers;

        public void AddPaper(Paper paper)
        {
            if (_papers.Add(paper))
                paper.AddAuthor(this);
        }

        public int HIndex => Papers
            .Select(i => i.CitedIn.Count)
            .OrderByDescending(i => i)
            .TakeWhile((v, i) => v >= i + 1)
            .Count();

        public int GIndex => Papers
            .Select(i => i.CitedIn.Count)
            .OrderByDescending(i => i)
            .SumSequence()
            .TakeWhile((v, i) => v >= (i + 1) * (i + 1))
            .Count();

        public double AuthorRank(int startYear, int endYear) => Papers
            .Where(
                paper => paper.Years.Any(
                    year =>
                        year >= startYear &&
                        year <= endYear
                )
            )
            .Select(
                paper => paper.CitedIn.Count()
            )
            .Average();

        public double AuthorHotRank(int startYear, int endYear) => Papers
            .Where(
                paper => paper.Years.Any(
                    year =>
                        year >= startYear &&
                        year <= endYear
                )
            )
            .Select(
                paper => paper
                    .CitedIn
                    .Count(cite => cite.Years.Min() - paper.Years.Max() <= 1)
            )
            .Average();

        public int NumberOfCitationsInYear(int year) => Papers
            .SelectMany(i => i.CitedIn)
            .Count(i => i.Years.Contains(year));

        public int NumberOfPublication => Papers.Count();

        public int StartOfActivity => Papers
            .SelectMany(paper => paper.Years)
            .Min();

        public int LastYearOfActivity => Papers
            .SelectMany(paper => paper.Years)
            .Max();

        public int YearsOfExperience =>
            LastYearOfActivity - StartOfActivity + 1;

        public int NumberOfCoauthers => Papers
            .Sum(i => i.Authors.Count - 1);

        public int NumberOfUniqueCoauthers => Papers
            .SelectMany(i => i.Authors)
            .Distinct()
            .Count() - 1;
        public override string ToString() => $"{{Id:{Id}, Name:{Name}, Citations:{NumberOfCitations}}}";
    }
}
