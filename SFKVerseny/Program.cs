using HtmlAgilityPack;
using MySql.Data.MySqlClient;
using SFKVerseny.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SFKVerseny
{
    internal class Program
    {
        static void Main(string[] args)
        {
            List<TableRowData> versenyzok1 = new List<TableRowData>();
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

            

            versenyzok1.Add(new TableRowData { Name = nev, Pont1 = pont1, Pont2 = pont2, Pont3 = pont3, LegjobbPont = legjobb });

            var doc = new HtmlDocument();
            string filePath = "SFKVerseny.html";

            // Ensure file exists with basic structure
            if (!File.Exists(filePath))
            {
                var initialHtml = "<!doctype html><html><head><meta charset=\"utf-8\"><title>SFK Verseny</title></head><body><table id='adatok'></table></body></html>";
                File.WriteAllText(filePath, initialHtml, Encoding.UTF8);
            }

            doc.Load(filePath);

            // Find or create table
            var table = doc.DocumentNode.SelectSingleNode("//table[@id='adatok']");
            if (table == null)
            {
                var body = doc.DocumentNode.SelectSingleNode("//body") ?? doc.DocumentNode;
                table = HtmlNode.CreateNode("<table id='adatok'></table>");
                body.AppendChild(table);
            }

            // Ensure header row exists
            bool hasHeader = table.SelectSingleNode(".//th") != null;
            if (!hasHeader)
            {
                var headerRow = HtmlNode.CreateNode(
                    "<tr><th>Név</th><th>1. lengetés</th><th>2. lengetés</th><th>3. lengetés</th><th>Legjobb pontszám</th><th>Összesített pontszám</th></tr>"
                    );
                table.AppendChild(headerRow);
            }

            int osszesitett = pont1 + pont2 + pont3;

            // Create new row with HTML-encoded name
            var encodedName = HtmlEntity.Entitize(nev ?? string.Empty);
            var newRow = HtmlNode.CreateNode(
                $"<tr><td>{encodedName}</td><td>{pont1}</td><td>{pont2}</td><td>{pont3}</td><td>{legjobb}</td><td>{osszesitett}</td></tr>"
                );

            table.AppendChild(newRow);
    
            doc.Save(filePath, Encoding.UTF8);

            MySqlConnection conn = new MySqlConnection();
            string connstring = "SERVER = localhost;DATABASE=sfkverseny;UID=root;PASSWORD=;";
            conn.ConnectionString = connstring;
            conn.Open();
            string sql = "SELECT * FROM versenyzok";
            MySqlCommand cmd = new MySqlCommand(sql, conn);
            List<Versenyzok> versenyzok = new List<Versenyzok>();
            MySqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                Versenyzok user = new Versenyzok(
                    nev: reader.GetString("Nev"),
                    pont1: reader.GetInt16("Pont1"),
                    pont2: reader.GetInt16("Pont2"),
                    pont3: reader.GetInt16("Pont3"),
                    legjobb: reader.GetInt16("Legjobb"),
                    helyezes: reader.GetInt32("Helyezes")
                    );
                versenyzok.Add(user);
            }
            conn.Close();
            conn.Open();
            string insertSQL = "INSERT INTO `versenyzok`(`Nev`, `Pont1`, `Pont2`, `Pont3`, `Legjobb`, `Helyezes`) VALUES (@nev,@pont1,@pont2,@pont3,@legjobb,null)";
            MySqlCommand comm = new MySqlCommand(insertSQL, conn);
            comm.Parameters.AddWithValue("@nev", nev);
            comm.Parameters.AddWithValue("@pont1", pont1);
            comm.Parameters.AddWithValue("@pont2", pont2);
            comm.Parameters.AddWithValue("@pont3", pont3);
            comm.Parameters.AddWithValue("@legjobb", legjobb);
            int sorokSzama = comm.ExecuteNonQuery();
            conn.Close();
            string valasz = sorokSzama > 0 ? "sikeres" : "sikertelen";
            Console.WriteLine(valasz);
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
