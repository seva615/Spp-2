using System;

namespace FakerLib.Generator
{
    public interface IGenerator 
    {
        bool CanGenerate(Type t);
        object Generate(Type t);
    }
}
