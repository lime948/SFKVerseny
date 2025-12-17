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
using System.Net;

namespace SFKVerseny
{
    internal class Program
    {
        static void Main(string[] args)
        {
            bool tf = true;
            while (tf)
            {
                Console.WriteLine("=== SFK Verseny Menedzsment Rendszer ===");
                Console.WriteLine("1. Versenyzők listázása");
                Console.WriteLine("2. Új versenyző rögzítése");
                Console.WriteLine("3. Versenyző törlése");
                Console.WriteLine("4. Versenyző adatainak módosítása");
                Console.WriteLine("5. Kilépés");
                Console.Write("Válasszon egy lehetőséget (1-5): ");
                string choice = Console.ReadLine();
                Program program = new Program();
                MySqlConnection conn1 = new MySqlConnection();
                string connstring1 = "SERVER = localhost;DATABASE=sfkverseny;UID=root;PASSWORD=;";
                conn1.ConnectionString = connstring1;
                switch (choice)
                {
                    case "1":
                        program.VersenyzoHozzaadasa(conn1);
                        break;
                    case "2":
                        program.VersenyzoTorlese(conn1);
                        break;
                    case "3":
                        program.VersenyzoModositasa(conn1);
                        break;
                    case "4":
                        tf = false;
                        break;
                    default:
                        Console.WriteLine("Érvénytelen választás. Kérem, próbálja újra.");
                        break;
                }
            }
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
            
        }

        public void VersenyzoHozzaadasa(MySqlConnection conn)
        {
            List<TableRowData> versenyzok1 = new List<TableRowData>();
            Console.WriteLine("=== Új versenyző rögzítése ===");
            Console.Write("A versenyző neve: ");
            string nev = Console.ReadLine();
            Console.Write("Az 1. lengetésen elért pontszám: ");
            int pont1 = int.Parse(Console.ReadLine());
            Console.Write("A 2. lengetésen elért pontszám: ");
            int pont2 = int.Parse(Console.ReadLine());
            Console.Write("A 3. lengetésen elért pontszám: ");
            int pont3 = int.Parse(Console.ReadLine());
            int legjobb = Math.Max(pont1, Math.Max(pont2, pont3));
            int helyezes = 1;

            versenyzok1.Add(new TableRowData { Name = nev, Pont1 = pont1, Pont2 = pont2, Pont3 = pont3, LegjobbPont = legjobb });

            foreach (var versenyzo in versenyzok1)
            {
                if (legjobb < versenyzo.LegjobbPont)
                {
                    versenyzo.Helyezes++;
                    helyezes = versenyzo.Helyezes;
                    versenyzok1.Add(new TableRowData { Helyezes = helyezes });
                }
            }
            conn.Open();
            string insertSQL = "INSERT INTO `versenyzok`(`Id`, `Nev`, `Pont1`, `Pont2`, `Pont3`, `Legjobb`, `Helyezes`) VALUES (null,@nev,@pont1,@pont2,@pont3,@legjobb,null)";
            MySqlCommand comm = new MySqlCommand(insertSQL, conn);
            comm.Parameters.AddWithValue("@nev", nev);
            comm.Parameters.AddWithValue("@pont1", pont1);
            comm.Parameters.AddWithValue("@pont2", pont2);
            comm.Parameters.AddWithValue("@pont3", pont3);
            comm.Parameters.AddWithValue("@legjobb", legjobb);
            int sorokSzama = comm.ExecuteNonQuery();
            conn.Close();

            var doc = new HtmlDocument();
            string filePath = "SFKVerseny.html";

            doc.Load(filePath);

            var table = doc.DocumentNode.SelectSingleNode("//tbody[@id='adatok']");

            int osszesitett = pont1 + pont2 + pont3;

            var encodedName = HtmlEntity.Entitize(nev ?? string.Empty);
            var newRow = HtmlNode.CreateNode(
                $"<tr><td>{encodedName}</td><td>{pont1}</td><td>{pont2}</td><td>{pont3}</td><td>{legjobb}</td><td>{osszesitett}</td><th scope=\"row\">{helyezes}</th></tr>"
                );

            table.AppendChild(newRow);

            doc.Save(filePath, Encoding.UTF8);
        }

        public void VersenyzoTorlese(MySqlConnection conn)
        {
            Console.WriteLine("=== Versenyző törlése ===");
            Console.Write("Adja meg a törlendő versenyző id-jét: ");
            int torlendoId = int.Parse(Console.ReadLine());
            conn.Open();
            string deleteSQL = "DELETE FROM `versenyzok` WHERE Id=@id";
            MySqlCommand deleteCommand = new MySqlCommand(deleteSQL, conn);
            deleteCommand.Parameters.AddWithValue("@id", torlendoId);
            int deletedRows = deleteCommand.ExecuteNonQuery();
            conn.Close();
        }

        public void VersenyzoModositasa(MySqlConnection conn)
        {
            Console.WriteLine("=== Felhasználó adatainak módosítása ===");
            Console.Write("Adja meg a módosítandó versenyző Id-jét: ");
            int modositandoId = int.Parse(Console.ReadLine());
            Console.Write("Adja meg az új nevet: ");
            string ujnev = Console.ReadLine();
            Console.Write("Adja meg az 1. lengetésen elért pontszámot: ");
            int ujpont1 = int.Parse(Console.ReadLine());
            Console.Write("Adja meg a 2. lengetésen elért pontszámot: ");
            int ujpont2 = int.Parse(Console.ReadLine());
            Console.Write("Adja meg a 3. lengetésen elért pontszámot: ");
            int ujpont3 = int.Parse(Console.ReadLine());
            int ujlegjobb = Math.Max(ujpont1, Math.Max(ujpont2, ujpont3));
            conn.Open();
            string updateSQL = "UPDATE `versenyzok` SET `Nev`=@nev, `Pont1`=@pont1, `Pont2`=@pont2, `Pont3`=@pont3, `Legjobb`=@legjobb, WHERE Id=@id";
            MySqlCommand updateCommand = new MySqlCommand(updateSQL, conn);
            updateCommand.Parameters.AddWithValue("@nev", ujnev);
            updateCommand.Parameters.AddWithValue("@pont1", ujpont1);
            updateCommand.Parameters.AddWithValue("@pont2", ujpont2);
            updateCommand.Parameters.AddWithValue("@pont3", ujpont3);
            updateCommand.Parameters.AddWithValue("@legjobb", ujlegjobb);
            int updatedRows = updateCommand.ExecuteNonQuery();
            conn.Close();
        }

        public class TableRowData
        {
            public string Name { get; set; }
            public int Pont1 { get; set; }
            public int Pont2 { get; set; }
            public int Pont3 { get; set; }
            public int LegjobbPont { get; set; }
            public int Helyezes { get; set; }
        }
    }
}
