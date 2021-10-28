using System;
using System.Linq;

namespace FakerLib.Utils
{
    class Node
    {
        public Node Parent { get; set; }
        public Type Type { get; set; }
    }

    public static class CyclicDependency
    {
        public static bool IsCyclic(Type type)
        {
            return IsCyclic(type, null);
        }
        private static bool IsCyclic(Type type, Node parent) //строим дерево типов 
        {
            for (Node node = parent; node != null; node = node.Parent) //связный список
            {
                if (Equals(node.Type, type))//если на пути от листа к корню, есть такой тип, то у нас циклическая зависимость
                {
                    return true;
                }
            }
            var currentNode = new Node()//создание листа
            {
                Parent = parent,
                Type = type
            };
            var result =  type.GetConstructors()
                .SelectMany(constructor => constructor.GetParameters()) 
                //получаем типы всех параметров конструкторов
                .Select(p => p.ParameterType)
                .Any(t => IsCyclic(t, currentNode));//рекурсивно вызываем проверку для каждого из типов, если один true => return true
            //Node нам больше не нужен удалим ссылки чтоб сборщик мусора его удалил
            currentNode.Parent = null;
            currentNode.Type = null;
            return result;
        }
    }
}
