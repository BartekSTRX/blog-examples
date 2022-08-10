using Bogus;
using Dapper.Contrib.Extensions;
using System;
using System.Data.SqlClient;

namespace DbSeed
{
    class Employee
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime DateOfBirth { get; set; }
        public int Salary { get; set; }
        public string Position { get; set; }
        public Guid EmployeeNumber { get; set; }
    }

    class Program
    {
        static void Main()
        {
            Randomizer.Seed = new Random(1234);

            var testEmployees = new Faker<Employee>()
                .RuleFor(e => e.EmployeeNumber, f => f.Random.Uuid())
                .RuleFor(e => e.FirstName, f => f.Name.FirstName())
                .RuleFor(e => e.LastName, f => f.Name.LastName())
                .RuleFor(e => e.DateOfBirth, f => f.Date.Past())
                .RuleFor(e => e.Salary, f => f.Random.Number(1000, 10000))
                .RuleFor(e => e.Position, f => f.Name.JobTitle());

            var employees = testEmployees.Generate(10000);


            using (var connection = new SqlConnection(@"Server=DESKTOP-BK;Initial Catalog=TestDb1;Integrated Security=true;"))
            {
                connection.Open();

                var identity = connection.Insert(employees);
            }
        }
    }
}
