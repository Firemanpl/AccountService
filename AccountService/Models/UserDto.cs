using System;
using System.Collections.Generic;

namespace AccountService.Models
{
    public class UserDto
    {
        public int Id { get; set; }
        public DateTime RegistrationTime { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Nationality { get; set; }
        public string City { get; set; }
        public string Street { get; set; }
        public string PostalCode { get; set; }
        public virtual List<UserPaymentsDto> UserPayments{ get; set; }
    }
}