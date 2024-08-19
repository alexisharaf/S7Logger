using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace S7Logger
{
    public class ParameterType
    {
        public string prameter_name { get; set; }

        public int db { get; set; }
        public int offset { get; set; }
        public string prameter_type { get; set; }

        public int bitnumber { get; set; }

        public object Value { get; set; }
    }
}
