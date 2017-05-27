using System;
using System.Collections.Generic;
using Core;

namespace Publications.BusinessLogic
{
    public class Paper : ObjectWithUniqueId
    {
        public Paper(Id id) : base(id)
        {
            _authors = new Relationship<Author>();
            _citers = new Relationship<Paper>();
            _citations = new Relationship<Paper>();
        }

        private readonly Relationship<Author> _authors;
        private readonly Relationship<Paper> _citers;
        private readonly Relationship<Paper> _citations;

        private Venue _venue;

        public IEnumerable<Author> Authors => _authors;
        public IEnumerable<Paper> CitedIn => _citers;
        public IEnumerable<Paper> Citations => _citations;

        public string Name { get; set; }
        public int Year { get; set; }
        public Venue Venue
        {
            get { return _venue; }
            set
            {
                if (value == null)
                    throw new ArgumentNullException(nameof(value));

                if (Equals(_venue, value))
                    return;

                _venue.AddPaper(this);

                _venue = value;
            }
        }
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

        public override ObjectWithUniqueId Create(Id id) => 
            IdToIndexMap.ContainsKey(id) ? IdToIndexMap[id] : new Paper(id);
    }
}
