using System;

namespace Publications.BusinessLogic
{
    public class DuplicatedIdException : Exception
    {
        public DuplicatedIdException(string message) : base(message)
        {}
    }
}