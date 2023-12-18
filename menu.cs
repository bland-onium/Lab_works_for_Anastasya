using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Ast;

namespace Ast
{
    public class Menu
    {
        public static void ShowMenu(ManagementCompany managementCompany)
        {
            bool exitMenu = false;

            while (!exitMenu)
            {
                Console.WriteLine("\nMenu:");
                Console.WriteLine("1. Sort and print all buildings");
                Console.WriteLine("2. Print first two buildings");
                Console.WriteLine("3. Print last three addresses");
                Console.WriteLine("4. Add a building");
                Console.WriteLine("5. Save to JSON file");
                Console.WriteLine("6. Load from JSON file");
                Console.WriteLine("7. Exit\n");

                Console.Write("Select an option: ");
                string option = Console.ReadLine();
                Console.WriteLine();

                switch (option)
                {
                    case "1":
                        managementCompany.SortAndPrintBuildings();
                        break;
                    case "2":
                        managementCompany.PrintFirstTwoBuildings();
                        break;
                    case "3":
                        managementCompany.PrintLastThreeAddresses();
                        break;
                    case "4":
                        FuncClass.AddBuilding(managementCompany);
                        break;
                    case "5":
                        FuncClass.SaveToFile(managementCompany);
                        break;
                    case "6":
                        FuncClass.LoadFromFile(managementCompany);
                        break;
                    case "7":
                        exitMenu = true;
                        break;
                    default:
                        Console.WriteLine("Invalid option. Please try again.");
                        break;
                }
            }
        }
    }
}