using Core;

namespace Publications.BusinessLogic
{
    public class Venue : ObjectWithUniqueId<Venue>
    {
        public Venue(Id id) : base(id)
        {
            _papers = new Relationship<Paper>();
        }

        private Relationship<Paper> _papers;

        public void AddPaper(Paper paper)
        {
            if (_papers.Add(paper))
                paper.Venue = this;
        }
    }
}
