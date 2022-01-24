using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZM.Models
{
    internal class Structure
    {
        public Type StructureType { get; private set; }

        public Structure(Type type)
        {
            StructureType = type;
        }

        public string Name => StructureType.Name;
        public string? Namespace => StructureType.Namespace;
    }
}
