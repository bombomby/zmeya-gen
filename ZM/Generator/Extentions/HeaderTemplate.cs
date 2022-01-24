using ZM.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZM.Generator
{
    public partial class HeaderTemplate
    {
        public TemplateContext Context { get; set; }

        public String GenerateIncludeStatement(String includeName)
        {
            return (includeName.StartsWith('<') && includeName.EndsWith('>')) ? includeName : $"\"{includeName}\"";
        }

        public List<String> GenerateIncludeList()
        {
            HashSet<String> includes = new HashSet<String>();
            foreach (var dep in Context.CurrentNode.Dependencies) 
                foreach (var include in dep.TargetIncludes)
                    if (!Context.CurrentNode.TargetIncludes.Contains(include)) // Skipping cyclic dependencies on the same type
                        includes.Add(include);
            return includes.OrderBy(x => x).ToList();
        }
    }
}
