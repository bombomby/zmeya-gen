using ZM.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ZM.Generator
{
    public partial class StructureTemplate
    {
        public TemplateContext Context { get; set; }
        private StringBuilder FieldTypeBuffer = new StringBuilder();

        public StructureTemplate(TemplateContext context)
        {
            Context = context;
        }

        public FieldInfo[] PublicFields => Context.CurrentNode.SourceType.GetFields();

        public String GenerateFieldDeclaration(FieldInfo field)
        {
            Node? node = null;
            if (!Context.Registry.Nodes.TryGetValue(field.FieldType, out node))
                return $"/* Unsupported Type: {field.FieldType} {field.Name} */";
            return $"{node.TargetType} {field.Name};";
        }
    }
}
