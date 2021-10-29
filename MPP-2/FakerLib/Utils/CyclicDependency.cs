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
        private static bool IsCyclic(Type type, Node parent) 
        {
            for (Node node = parent; node != null; node = node.Parent)
            {
                if (Equals(node.Type, type))
                {
                    return true;
                }
            }
            var currentNode = new Node()
            {
                Parent = parent,
                Type = type
            };
            var result =  type.GetConstructors()
                .SelectMany(constructor => constructor.GetParameters()) 
             
                .Select(p => p.ParameterType)
                .Any(t => IsCyclic(t, currentNode));
            
            currentNode.Parent = null;
            currentNode.Type = null;
            return result;
        }
    }
}
