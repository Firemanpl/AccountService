using System;

namespace AccountService.Exceptions
{
    public class NotFoundExcepion :Exception
    {
        public NotFoundExcepion(string messege): base(messege)
        { }
    }
}