using System.Collections.Generic;
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
    }
}
