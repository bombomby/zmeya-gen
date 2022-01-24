using ZM.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZM.Data
{
    public class GameObject
    {
        public Position Pos;
        public UInt32 Health = 100;
        public String Name = "<Unknown>";
        public int[] Items;
        public String[] Lines;
        public Ptr<GameObject> Target;
                
    }
}
