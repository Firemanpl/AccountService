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
                if (!_dbContext.Roles.Any())
                {
                    var roles = GetRoles();
                    _dbContext.Roles.AddRange(roles);
                    _dbContext.SaveChanges();
                }
                if (!_dbContext.Users.Any())
                {
                    var users = GetUsers();
                    _dbContext.Users.AddRange(users);
                    _dbContext.SaveChanges();
                    Console.ForegroundColor = ConsoleColor.Cyan;
                    Console.WriteLine("Pushing seder data to DB");
                    Console.ResetColor();
                }
            }
        }

        private IEnumerable<Role> GetRoles()
        {
            var roles = new List<Role>()
            {
                new Role(){
                Name = "User"
                },
                new Role(){
                Name = "Manager"
            },
                new Role(){
                Name = "Admin"
            },
                
            };
           return roles;
        }

        private IEnumerable<User> GetUsers()
        {
            var users = new List<User>()
            {
                new User()
                {
                    FirstName = "Paweł",
                    LastName = "Cieslak",
                    Email = "email@wp.pl",
                    VeryficationCode = "23457643",
                    LoginAttempts = 1,
                    Nationality = "PL",
                    PhoneNumber = "654378296",
                    RoleId = 1,
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
                    }
                },
                new User()
                {
                    FirstName = "Marcin", 
                    LastName = "Boruciak",
                    Email = "emyail@wp.pl",
                    VeryficationCode = "34423432",
                    LoginAttempts = 2,
                    Nationality = "PL",
                    PhoneNumber = "983652763",
                    RoleId = 1,
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
                }
                },
                new User()
                {
                    FirstName = "Krzysztof", 
                    LastName = "Marcińczak",
                    Email = "ewdadail@wp.pl",
                    VeryficationCode = "63422432",
                    LoginAttempts = 1,
                    Nationality = "EN",
                    PhoneNumber = "387365298",
                    RoleId = 1,
                    UserPayments = new List<UserPayments>()
                    {
                    
                        new UserPayments()
                        {
                            VehicleId = 2343,
                            Kilometers = 23,
                            KWh = 2.5,
                            Currency = "PLN",
                            Payment = 43,
                        },
                        new UserPayments()
                        {
                            VehicleId = 3424,
                            Kilometers = 34,
                            KWh = 34.5,
                            Currency = "PLN",
                            Payment = 4324,
                        },
                    },
                    Address = new Address()
                    {
                        Street = "Kruszewniki 32",
                        City = "Lubartów",
                        PostalCode = "20-854",
                    }
                }
            };
            return users;
        }
    }
}