using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.Intrinsics.X86;

namespace AccountService.Entities
{
    public class User
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public string Name { get; set; }
        public string Vormane { get; set; }
        public int VeryficationCode { get; set; }
        public int LoginAttempts { get; set; }
        public string Nationality { get; set; }
        public int PhoneNumber { get; set; }
        public DateTime RegistrationTime { get; set; }
        public virtual Role Role { get; set; }
        public int AddressId { get; set; }
        public virtual Address Address { get; set; }
        public virtual List<UserPayments> UserPayments{ get; set; }
    }
}