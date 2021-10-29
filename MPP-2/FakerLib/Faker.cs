using FakerLib.Generator;
using FakerLib.Utils;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FakerLib.Fakers.Impl
{
   
    public class Faker
    {
        private static readonly Faker _inctanсe = new();
        public static Faker DefaultFaker => _inctanсe;

        private readonly IEnumerable<IGenerator> _generators;

        public Faker()
        {
            var generatorType = typeof(IGenerator)
            _generators = AppDomain.CurrentDomain.GetAssemblies()
                       .SelectMany(a => a.GetTypes())
                       .Where(t => t.IsClass && t.GetInterfaces().Contains(generatorType)) 
                       .Select(t =>
                       {
                           try
                           {
                               return (IGenerator)Activator.CreateInstance(t); 
                           }
                           catch
                           {
                               return null;
                           }
                       })
                       .Where(generator => generator != null)
                       .ToArray();
        }
        private void InitializeFields(object obj)
        {
            foreach (var field in obj.GetType().GetFields())
            {
                try
                {
                    if (Equals(field.GetValue(obj), GetDefaultValue(field.FieldType))) 
                    {
                        field.SetValue(obj, Create(field.FieldType));
                    }
                }
                catch
                {
                }
            }
        }
        public object Create(Type t)
        {
            if (CyclicDependency.IsCyclic(t))
            {
                throw new Exception($"{t} contains cyclical dependency");
            }

            foreach (var generator in _generators)
            {
                if (generator.CanGenerate(t))
                    return generator.Generate(t);
            }

            foreach (var constructor in t.GetConstructors())
            {
                try
                {
                    var args = constructor.GetParameters()
                        .Select(p => p.ParameterType)
                        .Select(Create);
                    object obj = constructor.Invoke(args.ToArray());
                    InitializeFields(obj);
                    return obj;
                }
                catch
                {
                }
            }
            throw new Exception($"Cannot create object of type: {t}");
        }
        public T Create<T>() => (T)Create(typeof(T)); 
        private static object GetDefaultValue(Type t) 
        {
            return t.IsValueType ? Activator.CreateInstance(t) : null;
        }
    }
}
