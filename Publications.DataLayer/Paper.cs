using System.Collections.Generic;
using System.Linq;
using Core;

namespace Publications.DataLayer
{
    public class Paper
    {
        public Paper()
        {
            Authors = new List<Name>();
            Citations = new List<Id>();
        }
        public Name Name { get; set; }
        public List<Name> Authors { get; }
        public int Year { get; set; }
        public Name Venue { get; set; }
        public Id Index { get; set; }
        public List<Id> Citations { get; }

        public override string ToString() =>
            "{" +
            $"{nameof(Name)} : \"{Name}\", " +
            $"{nameof(Index)} : \"{Index}\", " +
            $"{nameof(Year)} : {Year}, " +
            $"{nameof(Venue)} : \"{Venue}\", " +
            $"{nameof(Authors)} : [{string.Join(", ", Authors.Select(i => $"\"{i.Value}\""))}], " +
            $"{nameof(Citations)} : [{string.Join(", ", Citations.Select(i => $"\"{i.Value}\""))}]" +
            "}";
    }
}