using System;

namespace AccountService.Exceptions
{
    public class ForbidException : Exception
    {
        public ForbidException(string message) :base(message)
        {
            
        }
    }
}