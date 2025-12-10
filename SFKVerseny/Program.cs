using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SFKVerseny
{
    internal class Program
    {
        static void Main(string[] args)
        {
            List<TableRowData> versenyzok = new List<TableRowData>();
            Console.Write("Adja meg a versenyző nevét: ");
            string nev = Console.ReadLine();
            Console.Write("Adja meg az 1. lengetésen elért pontszámot: ");
            int pont1 = int.Parse(Console.ReadLine());
            Console.Write("Adja meg az 2. lengetésen elért pontszámot: ");
            int pont2 = int.Parse(Console.ReadLine());
            Console.Write("Adja meg az 3. lengetésen elért pontszámot: ");
            int pont3 = int.Parse(Console.ReadLine());
            int maxPont = Math.Max(pont1, Math.Max(pont2, pont3));

            versenyzok.Add(new TableRowData {Name = nev, Pont1 = pont1, Pont2 = pont2, Pont3 = pont3, Maxpont = maxPont});
            var sorted = versenyzok.OrderByDescending(r => r.Maxpont).ToList();

            var doc = new HtmlDocument();
            doc.Load("SFKVerseny.html");
            var table = doc.DocumentNode.SelectSingleNode("//table[@id='adatok']");
            table.RemoveAllChildren();
            var headerRow = HtmlNode.CreateNode(
                "<tr><th>Név</th><th>1. lengetés</th><th>2. lengetés</th><th>3. lengetés</th><th>Legjobb pontszám</th><th>Összesített pontszám</th></tr>"
                );
            foreach (var r in sorted)
            {
                var newRow = HtmlNode.CreateNode(
                        $"<tr><td>{nev}</td><td>{pont1}</td><td>{pont2}</td><td>{pont3}</td><td>{maxPont}</td><td>{r.Maxpont}</td></tr>"
                        );
                table.AppendChild(newRow);
            }
            doc.Save("SFKVerseny.html");

        }
        public class TableRowData
        {
            public string Name { get; set; }
            public int Pont1 { get; set; }
            public int Pont2 { get; set; }
            public int Pont3 { get; set; }
            public int Maxpont { get; set; }
        }

    }
}
