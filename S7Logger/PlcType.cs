using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace S7Logger
{
    public class PlcType
    {
        public string plc_name { get; set; }
        public string plc_ip { get; set; }
        public int plc_rack { get; set; }
        public int plc_slot { get; set; }

    }
}
