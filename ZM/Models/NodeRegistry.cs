using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using NodeDictionary = System.Collections.Generic.Dictionary<System.Type, ZM.Models.Node>;

namespace ZM.Models
{
    public class NodeRegistry
    {
        public NodeDictionary Nodes { get; set; } = new NodeDictionary();

        public void Register(Type type)
        {
            Node? node = RegisterType(type);
            if (node == null)
                return;

            foreach (var field in node.SourceType.GetFields())
            {
                Node? fieldNode = RegisterType(field.FieldType);
                if (fieldNode != null)
                {
                    node.Dependencies.Add(fieldNode);
                }
            }
         }

        Node? RegisterType(Type type)
        {
            Node? node = null;
            if (Nodes.TryGetValue(type, out node))
                return node;

            node = new Node(type);

            if (type.IsGenericType)
            {
                if (type.Name == "Ptr`1")
                {
                    if (type.GetGenericArguments().Length != 1) 
                        return null;

                    Type subType = type.GetGenericArguments()[0];
                    Node? subNode = RegisterType(subType);
                    if (subNode != null)
                    {
                        node.TargetType = $"zm::Pointer<{subNode.TargetType}>";
                        node.TargetIncludes.Add("Zmeya.h");
                        foreach (var include in subNode.TargetIncludes)
                            node.TargetIncludes.Add(include);
                    }
                    else
                    {
                        return null;
                    }
                }
            } 
            else if (type.IsArray)
            {
                Type? subType = type.GetElementType();
                Node? subNode = subType != null ? RegisterType(subType) : null;
                
                if (subNode != null)
                {
                    node.TargetType = $"zm::Array<{subNode.TargetType}>";
                    node.TargetIncludes.Add("Zmeya.h");
                    foreach (var include in subNode.TargetIncludes)
                        node.TargetIncludes.Add(include);
                }
                else
                {
                    return null;
                }
            }
            else
            {
                node.TargetIncludes.Add($"{node.TargetType}.h");
            }

            Nodes.Add(type, node);
            return node;
        }
    }
}
