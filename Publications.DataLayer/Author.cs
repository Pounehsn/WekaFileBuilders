using Core;

namespace Publications.DataLayer
{
    public class Author
    {
        public int Id { get; set; }
        public Name Name { get; set; }

        public override string ToString() => 
            $"{{{nameof(Id)} : {Id}, {nameof(Name)} : \"{Name}\"}}";
    }
}