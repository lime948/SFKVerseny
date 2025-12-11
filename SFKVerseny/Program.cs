using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SFKVerseny.Adatbazis;
using SFKVerseny.Models;

namespace SFKVerseny
{
    internal class Program
    {
        static void Main(string[] args)
        {
            List<TableRowData> versenyzok = new List<TableRowData>();
            Console.WriteLine("=== Új versenyző rögzítése ===");
            Console.Write("A versenyző neve: ");
            string nev = Console.ReadLine();
            Console.Write("Az 1. lengetésen elért pontszám: ");
            int pont1 = int.Parse(Console.ReadLine());
            Console.Write("Az 2. lengetésen elért pontszám: ");
            int pont2 = int.Parse(Console.ReadLine());
            Console.Write("Az 3. lengetésen elért pontszám: ");
            int pont3 = int.Parse(Console.ReadLine());
            int legjobb = Math.Max(pont1, Math.Max(pont2, pont3));

            versenyzok.Add(new TableRowData {Name = nev, Pont1 = pont1, Pont2 = pont2, Pont3 = pont3, LegjobbPont = legjobb});

            var doc = new HtmlDocument();
            doc.Load("SFKVerseny.html");
            var table = doc.DocumentNode.SelectSingleNode("//table[@id='adatok']");
            table.RemoveAllChildren();
            var headerRow = HtmlNode.CreateNode(
                "<tr><th>Név</th><th>1. lengetés</th><th>2. lengetés</th><th>3. lengetés</th><th>Legjobb pontszám</th><th>Összesített pontszám</th></tr>"
                );
            var newRow = HtmlNode.CreateNode(
                $"<tr><td>{nev}</td><td>{pont1}</td><td>{pont2}</td><td>{pont3}</td><td>{legjobb}</td><td></td></tr>"
                );
            table.AppendChild(headerRow);
            table.AppendChild(newRow);
            doc.Save("SFKVerseny.html");

        }
        public class TableRowData
        {
            public string Name { get; set; }
            public int Pont1 { get; set; }
            public int Pont2 { get; set; }
            public int Pont3 { get; set; }
            public int LegjobbPont { get; set; }
        }

    }
}
