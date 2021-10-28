using FakerLib.Fakers.Impl;
using System;
using System.Collections.Generic;

namespace FakerApp
{
    class Program
    {
        static void Main(string[] args)
        {
            var faker = new Faker();

            var list = faker.Create<List<List<DateTime>>>();
            foreach (var values in list)
            {
                foreach (var v in values)
                {
                    Console.WriteLine(v);
                }
            }

            int numbers = faker.Create<int>();
            Console.WriteLine(numbers);
        }
    }
}
