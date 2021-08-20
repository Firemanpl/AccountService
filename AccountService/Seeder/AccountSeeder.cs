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
                    Email = "email@wp.pl",
                    VeryficationCode = 23457643,
                    LoginAttempts = 1,
                    Nationality = "PL",
                    PhoneNumber = 654378296,
                    UserHistory = new List<UserHistory>()
                    {
                        new UserHistory()
                        {
                            VehicleId = 2313,
                            Kilometers = 45,
                            KWh = 34.5,
                            Paid = true,
                        },
                        new UserHistory()
                        {
                            VehicleId = 342234,
                            Kilometers = 343,
                            KWh = 3424.5,
                            Paid = true,
                        },
                    },
                    Role = new Role
                    {
                        Name = "User",
                    }
                },
                new User()
                {
                Email = "email@wp.pl",
                VeryficationCode = 344234321,
                LoginAttempts = 2,
                Nationality = "PL",
                PhoneNumber = 983652763,
                UserHistory = new List<UserHistory>()
                {
                    new UserHistory()
                    {
                        VehicleId = 2333,
                        Kilometers = 23,
                        KWh = 23.5,
                        Paid = true,
                    },
                    new UserHistory()
                    {
                        VehicleId = 32312,
                        Kilometers = 332,
                        KWh = 23.5,
                        Paid = true,
                    },
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