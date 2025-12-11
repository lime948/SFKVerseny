using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using SFKVerseny.Models;

namespace SFKVerseny.Adatbazis
{
    internal class Adatbazis
    {
    //    public List<Versenyzok>
    //        {

    //        MySqlConnection conn = new MySqlConnection();
    //    string connstring = "SERVER = localhost;DATABASE=sfkverseny;UID=root;PASSWORD=;";
    //    conn.ConnectionString = connstring;
    //        conn.Open();
    //        string sql = "SELECT * FROM versenyzok";
    //    MySqlCommand cmd = new MySqlCommand(sql, conn);
    //    List<Versenyzok> versenyzok = new List<Versenyzok>();
    //    MySqlDataReader reader = cmd.ExecuteReader();
    //        while (reader.Read())
    //        {
    //            Versenyzok user = new Versenyzok(
    //                nev: reader.GetString("Nev"),
    //                pont1: reader.GetInt16("Pont1"),
    //                pont2: reader.GetInt16("Pont2"),
    //                pont3: reader.GetInt16("Pont3"),
    //                legjobb: reader.GetInt16("Legjobb"),
    //                helyezes: reader.GetInt16("Helyezes")
    //                );
    //    users.Add(user);
    //        }
    //conn.Close();

    //        conn.Open();
    //        string insertSQL = "INSERT INTO `versenyzok`(`Nev`, `Pont1`, `Pont2`, `Pont3`, `Legjobb`, `Helyezes`) VALUES (@nev,@pont1,@pont2,@pont3,@legjobb,null)";
    //MySqlCommand comm = new MySqlCommand(insertSQL, conn);
    //comm.Parameters.AddWithValue("@nev", nev);
    //        comm.Parameters.AddWithValue("@pont1", pont1);
    //        comm.Parameters.AddWithValue("@pont2", pont2);
    //        comm.Parameters.AddWithValue("@pont3", pont3);
    //        comm.Parameters.AddWithValue("@legjobb", legjobb);
    //        int sorokSzama = comm.ExecuteNonQuery();
    //conn.Close();
    //        string valasz = sorokSzama > 0 ? "sikeres" : "sikertelen";
    //Console.WriteLine(valasz);
    //    }
    }
}