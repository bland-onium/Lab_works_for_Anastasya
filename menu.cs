using Ast;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Lab_4_Nastya
{
    internal class Menu
    {
       public static void menu()
        {
            Console.WriteLine("Write URL of site which you wanna scan:");
            string trylink = Convert.ToString(Console.ReadLine());
            Uri link;
            try
            {
                link = new Uri(trylink, UriKind.Absolute);
            }
            catch
            {
                link = null;
                Console.WriteLine("Wrong link, impossible to use\n");
                menu();
            }
            Console.WriteLine("How deep we will seek?");
            int depth = Int32.Parse(Console.ReadLine());
            Parser.ParseSite(link, depth);
        }
    }
}
