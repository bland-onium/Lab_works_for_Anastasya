using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Ast
{
    public class FuncClass
    {
        public static void AddBuilding(ManagementCompany managementCompany)
        {
            var building = new ResidentialBuilding
            {
                Type = "r",
                Address = "Lenina_1",
                RoomCount = 10,
                ApartmentCount = 10
            };
            managementCompany.AddBuilding(building);

            /*
            Console.Write("Enter the type of the building (Residential/NonResidential)(R/N): ");
            string type = Console.ReadLine();

            if (type == "Residential" || type.ToLower() == "r")
            {
                type = "Residential"
                Console.Write("Enter the address of the building: ");
                string address = Console.ReadLine();

                Console.Write("Enter the number of rooms: ");
                int roomCount = int.Parse(Console.ReadLine());

                Console.Write("Enter the number of apartments: ");
                int apartmentCount = int.Parse(Console.ReadLine());

                var building = new ResidentialBuilding
                {
                    Type = type,
                    Address = address,
                    RoomCount = roomCount,
                    ApartmentCount = apartmentCount
                };

                managementCompany.AddBuilding(building);
            }
            else if (type == "NonResidential" || type.ToLower() == "n")
            {
                type = "NonResidential"
                Console.Write("Enter the address of the building: ");
                string address = Console.ReadLine();

                Console.Write("Enter the area of the building (in meters^2): ");
                double area = double.Parse(Console.ReadLine());

                Console.Write("Enter the number of employees: ");
                int employeeCount = int.Parse(Console.ReadLine());

                var building = new NonResidentialBuilding
                {
                    Type = type,
                    Address = address,
                    Area = area,
                    EmployeeCount = employeeCount
                };

                managementCompany.AddBuilding(building);
            }
            else
            {
                Console.WriteLine("Invalid building type. Please try again.");
            }
            */
        }

        public static void SaveToFile(ManagementCompany managementCompany)
        {
            Console.Write("Enter the filename (file saves at C:/Users/NITRO/Downloads/Lab_3_Nastya/: ");
            string filename = Console.ReadLine();

            managementCompany.SaveToJson(filename);
            Console.WriteLine("Data saved to file successfully.");
        }

        public static void LoadFromFile(ManagementCompany managementCompany)
        {
            Console.Write("Enter the filename: ");

            string filename = Console.ReadLine();

            managementCompany.LoadFromJson(filename);
            Console.WriteLine("Data loaded from file successfully.");
        }
    }
}