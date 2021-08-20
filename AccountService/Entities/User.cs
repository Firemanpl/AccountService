using System.Collections.Generic;
using System.Runtime.Intrinsics.X86;

namespace AccountService.Entities
{
    public class User
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public int VeryficationCode { get; set; }
        public int LoginAttempts { get; set; }
        public string Nationality { get; set; }
        public int PhoneNumber { get; set; }
        public virtual Role Role { get; set; }
        public int UserHistoryId { get; set; }
        public virtual List<UserHistory> UserHistory{ get; set; }
    }
}