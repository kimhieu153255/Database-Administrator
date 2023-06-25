using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App
{
    internal class Department
    {
        public string MAPB { get; set; }
        public string TENPB { get; set; }
        public string TRPHG { get; set; }
        public Department(string MAPB,string TENPB, string TRPHG)
        {
            this.MAPB = MAPB;
            this.TRPHG = TRPHG;
            this.TENPB = TENPB;
        }
    }
}
