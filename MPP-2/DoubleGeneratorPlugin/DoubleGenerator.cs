using FakerLib.Generator;
using System;

namespace DoubleGeneratorPlugin
{
    public class DoubleGenerator : IGenerator//т.к у него есть интерфейс, Faker находит его
    {
        private readonly Random _random = new();

        public bool CanGenerate(Type t)
        {
            return t == typeof(double);
        }

        public object Generate(Type t)
        {
            if (CanGenerate(t))
                return _random.NextDouble();
            throw new ArgumentException($"Cannot create object of type: {t}");
        }
    }
}
