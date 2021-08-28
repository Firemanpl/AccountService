using System;
using System.ComponentModel.DataAnnotations;
using AccountService.Entities;

namespace AccountService.Models
{
    public class CreateUserDto
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        public string Name { get; set; }
        public string Vormane { get; set; }
        [Required]
        [Range(00000000,99999999)]
        public int VeryficationCode { get; set; }
        public int LoginAttempts { get; set; }
        [Required]
        public string Nationality { get; set; }
        [Required]
        [Phone]
        public string PhoneNumber { get; set; }
        public string City { get; set; }
        public string Street { get; set; }
        public string PostalCode { get; set; }
    }
}