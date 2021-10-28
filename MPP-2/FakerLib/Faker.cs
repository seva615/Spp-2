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
            var generatorType = typeof(IGenerator);
            _generators = AppDomain.CurrentDomain.GetAssemblies()//получаем все сборки (dll)
                       .SelectMany(a => a.GetTypes())
                       .Where(t => t.IsClass && t.GetInterfaces().Contains(generatorType)) //если в I есть генератор
                       .Select(t =>
                       {
                           try
                           {
                               return (IGenerator)Activator.CreateInstance(t); //пробуем создать генератор
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
                    if (Equals(field.GetValue(obj), GetDefaultValue(field.FieldType))) //проверка на пустое поле
                    {
                        field.SetValue(obj, Create(field.FieldType));//рекурсивно создаём объект и присваиваем полю
                    }
                }
                catch
                {
                }
            }
        }
        public object Create(Type t)
        {
            if (CyclicDependency.IsCyclic(t))//проверка на циклическую зависимость
            {
                throw new Exception($"{t} contains cyclical dependency");
            }

            foreach (var generator in _generators)//проходим по всем готовым генераторам
            {
                if (generator.CanGenerate(t))//если может сделать объект по типу, то он его делает
                    return generator.Generate(t);
            }

            //если нет, то делаем объект сами
            foreach (var constructor in t.GetConstructors())
            {
                try
                {
                    var args = constructor.GetParameters()
                        .Select(p => p.ParameterType)
                        .Select(Create);//для каждого параметра конструктора рекурсивно создаём объект

                    object obj = constructor.Invoke(args.ToArray());//создаём объект с помощью конструктора
                    InitializeFields(obj);
                    return obj;
                }
                catch
                {
                }
            }
            throw new Exception($"Cannot create object of type: {t}");
        }
        public T Create<T>() => (T)Create(typeof(T)); //получаем тип и создаём объект, проводим его к T

        private static object GetDefaultValue(Type t) //получаем 0 либо null
        {
            return t.IsValueType ? Activator.CreateInstance(t) : null;
        }
    }
}
