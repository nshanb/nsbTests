using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace simpleGeometry
{
    public class PlayWithHiding
    {
        public int X { get; set; }
        public int Y { get { return 16;  } }

        public int Z { get => z; }

        protected int z;
    }
    public class PlayWithHiding1 : PlayWithHiding
    {
        public int X { get; set; }
        public int Y { get; set; }
        public int Z { set => z = value; }
    }
}

