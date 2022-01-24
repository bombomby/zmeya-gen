using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZM.Models
{
    public class Node
    {
        public Type SourceType { get; set; }
        public string TargetType { get; set; }
        public HashSet<String> TargetIncludes { get; set; } = new HashSet<string>();
        public HashSet<Node> Dependencies { get; set; } = new HashSet<Node>();

        public Node(Type type)
        {
            SourceType = type;
            TargetType = type.Name;
        }

        public Node(Type sourceType, String targetType, String targetInclude)
        {
            SourceType = sourceType;
            TargetType = targetType;
            TargetIncludes = new HashSet<String>() { targetInclude };
        }
    }
}
