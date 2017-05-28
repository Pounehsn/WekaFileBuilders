using Core;

namespace Publications.DataLayer
{
    public class AuthorDto
    {
        public Id Id { get; set; }
        public Name Name { get; set; }
        public int NumberOfCitations { get; set; }

        public override string ToString() => 
            $"{{{nameof(Id)} : {Id}, {nameof(Name)} : \"{Name}\"}}";
    }
}