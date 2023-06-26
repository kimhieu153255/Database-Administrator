using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App
{
    public class Project
    {
        public string MADA { get; set; }
        public string TENDA { get; set; }
        public string NGAYBD { get; set; }
        public string PHONG { get; set; }

        public Project(string MADA, string TENDA, string NGAYBD, string PHONG)
        {
            this.MADA = MADA;
            this.TENDA = TENDA;
            this.NGAYBD = NGAYBD;
            this.PHONG = PHONG;
        }
    }
}
