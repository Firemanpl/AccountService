using System;

namespace AccountService.Exceptions
{
    public class WarningException:Exception
    {
        public WarningException(string messege) : base(messege)
        { }
    }
}