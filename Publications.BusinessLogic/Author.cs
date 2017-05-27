using System.Collections.Generic;
using Core;

namespace Publications.BusinessLogic
{
    public class Author : ObjectWithUniqueId
    {
        public Author(Id id) : base(id) {}

        private readonly Relationship<Paper> _papers = new Relationship<Paper>();
        
        public IEnumerable<Paper> Papers => _papers;

        public void AddPaper(Paper paper)
        {
            if(_papers.Add(paper))
                paper.AddAuthor(this);
        }

        public override ObjectWithUniqueId Create(Id id) =>
            IdToIndexMap.ContainsKey(id) ? IdToIndexMap[id] : new Author(id);
    }
}
