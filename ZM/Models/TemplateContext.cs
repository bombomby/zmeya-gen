using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;


namespace ZM.Models
{
    public class TemplateContext
    {
        public Node CurrentNode { get; set; }
        public NodeRegistry Registry { get; set; }
    }
}
