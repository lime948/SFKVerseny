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
            var doc = new HtmlDocument();
            doc.Load("SFKVerseny.html");
            var table = doc.DocumentNode.SelectSingleNode("//table[@id='myTable']");
            var newRow = HtmlNode.CreateNode($"<tr><td></td><td>Anna</td><td>3</td><td>Anna</td><td>3</td><td>Anna</td></tr>");
            table.AppendChild(newRow);
            doc.Save("table.html");

        }

    }
}
