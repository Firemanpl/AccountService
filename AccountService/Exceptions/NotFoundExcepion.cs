using System;

namespace AccountService.Middleware
{
    public class NotFoundExcepion :Exception
    {
        public NotFoundExcepion(string messege): base(messege)
        {}
    }
}