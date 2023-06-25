using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App
{
    internal class Employee
    {
        public string MANV { get; set; }
        public string TENNV { get; set; }
        public string PHAI { get; set; }
        public string NGAYSINH { get; set; }
        public string DIACHI { get; set; }
        public string SODT { get; set; }
        public string LUONG { get; set; }
        public string PHUCAP { get; set; }
        public string VAITRO { get; set; }
        public string MANQL { get; set; }
        public string PHG { get; set; }

        public Employee(string MANV, string TENNV, string PHAI, string NGAYSINH, string DIACHI, string SODT, string LUONG, string PHUCAP, string VAITRO, string MANQL, string PHG)
        {
            this.MANV = MANV;
            this.TENNV = TENNV;
            this.PHAI = PHAI;
            this.NGAYSINH = NGAYSINH;
            this.DIACHI = DIACHI;
            this.SODT = SODT;
            this.LUONG = LUONG;
            this.PHUCAP = PHUCAP;
            this.VAITRO = VAITRO;
            this.MANQL = MANQL;
            this.PHG = PHG;
        }
    }
}
