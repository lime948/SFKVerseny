using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SFKVerseny.Models
{
    internal class Versenyzok
    {
        public Versenyzok() { }

        public Versenyzok(string nev, int pont1, int pont2, int pont3, int legjobb, int helyezes)
        {
            Nev = nev;
            Pont1 = pont1;
            Pont2 = pont2;
            Pont3 = pont3;
            Legjobb = legjobb;
            Helyezes = helyezes;
        }

        public string Nev { get; set; }
        public int Pont1 { get; set; }
        public int Pont2 { get; set; }
        public int Pont3 { get; set; }
        public int Legjobb { get; set; }
        public int Helyezes { get; set;  }
    }
}