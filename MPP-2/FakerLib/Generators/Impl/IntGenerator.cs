using System;

namespace FakerLib.Generator.Impl
{
    public class IntGenerator : IGenerator
    {
        private readonly Random _random = new();

        public bool CanGenerate(Type t)
        {
            return t == typeof(int);
        }

        public object Generate(Type t)
        {
            if (CanGenerate(t))
                return _random.Next();
            throw new ArgumentException($"Cannot create object of type: {t}");
        }
    }
}
