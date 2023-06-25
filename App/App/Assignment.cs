using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App
{
    public class Assignment
    {
        public string MANV { get; set; }
        public string MADA { get; set;}
        public string THOIGIAN { get; set;}

        public Assignment(string MANV, string MADA, string THOIGIAN)
        {
            this.MANV = MANV;
            this.MADA = MADA;
            this.THOIGIAN = THOIGIAN;
        }
    }
}
