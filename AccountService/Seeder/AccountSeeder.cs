using System;
using System.Collections.Generic;
using System.Linq;
using AccountService.Entities;

namespace AccountService.Seeder
{
    public class AccountSeeder
    {
        private readonly AccountDbContext _dbContext;


        public AccountSeeder(AccountDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public void Seed()
        {
            if (_dbContext.Database.CanConnect())
            {
                if (!_dbContext.User.Any())
                {
                    var users = GetUsers();
                    _dbContext.User.AddRange(users);
                    _dbContext.SaveChanges();
                    Console.ForegroundColor = ConsoleColor.Cyan;
                    Console.WriteLine("Pushing seder data to DB");
                    Console.ResetColor();
                }
            }
        }

        private IEnumerable<User> GetUsers()
        {
            var users = new List<User>()
            {
                new User()
                {
                    Name = "Paweł",
                    Vormane = "Cieslak",
                    Email = "email@wp.pl",
                    VeryficationCode = 23457643,
                    LoginAttempts = 1,
                    Nationality = "PL",
                    PhoneNumber = 654378296,
                    UserPayments = new List<UserPayments>()
                    {
                        new UserPayments()
                        {
                            VehicleId = 2313,
                            Kilometers = 45,
                            KWh = 34.5,
                            Currency = "PLN",
                            Payment = 2131,
                        },
                        new UserPayments()
                        {
                            VehicleId = 342234,
                            Kilometers = 343,
                            KWh = 3424.5,
                            Currency = "PLN",
                            Payment = 3242342,
                        },
                    },
                    Address = new Address()
                    {
                        Street = "Tulipanowa 10",
                        City = "Lublin",
                        PostalCode = "20-643",
                    },
                    Role = new Role
                    {
                        Name = "User",
                    }
                },
                new User()
                {
                    Name = "Marcin", 
                    Vormane = "Boruciak",
                    Email = "emyail@wp.pl",
                    VeryficationCode = 34423432,
                    LoginAttempts = 2,
                    Nationality = "PL",
                    PhoneNumber = 983652763,
                    UserPayments = new List<UserPayments>()
                {
                    
                    new UserPayments()
                    {
                        VehicleId = 2333,
                        Kilometers = 23,
                        KWh = 23.5,
                        Currency = "PLN",
                        Payment = 2131,
                    },
                    new UserPayments()
                    {
                        VehicleId = 32312,
                        Kilometers = 332,
                        KWh = 23.5,
                        Currency = "PLN",
                        Payment = 2342,
                    },
                },
                Address = new Address()
                {
                    Street = "Płouszowice-Kolonia 68",
                    City = "Lublin",
                    PostalCode = "21-008",
                },
                Role = new Role
                {
                    Name = "User",
                }
            }
            };
            return users;
        }
    }
}