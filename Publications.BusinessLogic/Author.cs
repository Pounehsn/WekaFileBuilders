using System.Collections.Generic;
using System.Linq;
using Core;

namespace Publications.BusinessLogic
{
    public class Author : ObjectWithUniqueId<Author>
    {
        public Author(Id id, Name name) : base(id)
        {
            Name = name;
            nameToAuthor.Add(name, this);
        }

        private static Dictionary<Name, Author> nameToAuthor = new Dictionary<Name, Author>();

        private readonly Relationship<Paper> _papers = new Relationship<Paper>();

        public static Author GetByName(Name name) => nameToAuthor[name];

        public Name Name { get; }
        public IEnumerable<Paper> Papers => _papers;

        public void AddPaper(Paper paper)
        {
            if (_papers.Add(paper))
                paper.AddAuthor(this);
        }

        public int HIndex => Papers
            .Select(i => i.CitedIn.Count)
            .OrderBy(i => i)
            .TakeWhile((v, i) => v >= i)
            .Count();

        public int GIndex => Papers
            .Select(i => i.CitedIn.Count)
            .OrderBy(i => i)
            .SumSequence()
            .TakeWhile((v, i) => v >= i * i)
            .Count();

        public double AuthorRank(int startingYear, int numberOfYears) => Papers
            .Where(
                paper => paper.Years.Any(
                    year =>
                        year >= startingYear &&
                        year < startingYear + numberOfYears
                )
            )
            .Select(
                paper => paper.Citations.Count()
            )
            .Average();

        public int NumberOfPublication => Papers.Count();

        public int StartOfActivity => Papers
            .SelectMany(paper => paper.Years)
            .Min();

        public int LastYearOfActivity => Papers
            .SelectMany(paper => paper.Years)
            .Max();

        public int NumberOfCoauthers => Papers
            .Sum(i => i.Authors.Count);

        public int NumberOfUniqueCoauthers => Papers
            .SelectMany(i => i.Authors)
            .Distinct()
            .Count();
    }
}
