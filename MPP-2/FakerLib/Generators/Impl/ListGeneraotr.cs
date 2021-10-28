using FakerLib.Fakers.Impl;
using System;
using System.Collections;
using System.Collections.Generic;

namespace FakerLib.Generator.Impl
{
    public class ListGenerator : IGenerator
    {
        private readonly Random rnd = new();

        public bool CanGenerate(Type t)
        {
            return t.IsGenericType && t.GetGenericTypeDefinition() == typeof(List<>);
        }

        public object Generate(Type t)
        {
            if (!CanGenerate(t))
                throw new ArgumentException($"Cannot create object of type: {t}");
            var list = (IList)Activator.CreateInstance(t);
            var amount = rnd.Next(2, 10);

            for (int i = 0; i < amount; i++)
            {
                list.Add(Faker.DefaultFaker.Create(t.GetGenericArguments()[0]));//рекурсивно заполняем элементы списка
            }

            return list;
        }
    }
}