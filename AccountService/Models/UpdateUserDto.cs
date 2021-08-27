using System;
using System.ComponentModel.DataAnnotations;

namespace AccountService.Models
{
    public class UpdateUserDto
    {
        [EmailAddress]
        public string Email { get; set; }
        public string Name { get; set; }
        public string Vormane { get; set; }
        [MaxLength(8)]
        public int VeryficationCode { get; set; }
        public int LoginAttempts { get; set; }
        public string Nationality { get; set; }
        [Phone]
        public int PhoneNumber { get; set; }
        public string City { get; set; }
        public string Street { get; set; }
        public string PostalCode { get; set; }
    }
}