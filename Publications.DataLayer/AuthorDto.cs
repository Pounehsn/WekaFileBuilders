using Core;

namespace Publications.DataLayer
{
    public class AuthorDto
    {
        public int Id { get; set; }
        public Name Name { get; set; }

        public override string ToString() => 
            $"{{{nameof(Id)} : {Id}, {nameof(Name)} : \"{Name}\"}}";
    }
}