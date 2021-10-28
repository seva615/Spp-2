using System;

namespace FakerLib.Generator
{
    public interface IGenerator //фабрика создания объектов по типу
    {
        bool CanGenerate(Type t);
        object Generate(Type t);
    }
}
