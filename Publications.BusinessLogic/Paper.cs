using System;
using System.Collections.Generic;
using Core;

namespace Publications.BusinessLogic
{
    public class Paper : ObjectWithUniqueId<Paper>
    {
        public Paper(Id id) : base(id)
        { }

        private readonly Relationship<Author> _authors = new Relationship<Author>();
        private readonly Relationship<Paper> _citers = new Relationship<Paper>();
        private readonly Relationship<Paper> _citations = new Relationship<Paper>();
        private readonly List<int> _years = new List<int>();

        private Venue _venue;

        public IRelationship<Author> Authors => _authors;
        public IRelationship<Paper> CitedIn => _citers;
        public IRelationship<Paper> Citations => _citations;
        public IEnumerable<int> Years => _years;

        public Name Name { get; set; }
        public Venue Venue
        {
            get { return _venue; }
            set
            {
                if (value == null)
                    throw new ArgumentNullException(nameof(value));

                if (Equals(_venue, value))
                    return;

                _venue = value;

                _venue.AddPaper(this);
            }
        }

        public void AddYear(int year) => _years.Add(year);
        public void AddAuthor(Author author)
        {
            if (_authors.Add(author))
                author.AddPaper(this);
        }
        public void AddCiter(Paper paper)
        {
            if (_citers.Add(paper))
                paper.AddCitation(this);
        }
        public void AddCitation(Paper paper)
        {
            if (_citations.Add(paper))
                paper.AddCiter(this);
        }
    }
}
