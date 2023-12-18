using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Ast
{
    public abstract class Building
    {
        public string Type { get; set; }
        public string Address { get; set; }

        // Абстрактный метод для расчета приближенного среднего количества жильцов/работников строения
        public abstract double CalculateAverage();
    }

    // Класс для жилых строений, наследуется от базового класса Building
    public class ResidentialBuilding : Building
    {
        public int RoomCount { get; set; }
        public int ApartmentCount { get; set; }

        public override double CalculateAverage()
        {
            return (ApartmentCount * RoomCount) * 1.3;
        }
    }

    // Класс для нежилых строений, наследуется от базового класса Building
    public class NonResidentialBuilding : Building
    {
        public double Area { get; set; }
        public int EmployeeCount { get; set; }

        public override double CalculateAverage()
        {
            return Area * 0.2;
        }
    }

    // Класс управляющей компании
    public class ManagementCompany
    {
        public List<Building> Buildings { get; set; }
        public double AveragePopulation { get; private set; }

        public ManagementCompany()
        {
            Buildings = new List<Building>();
        }

        // Добавление строения в компанию
        public void AddBuilding(Building building)
        {
            Buildings.Add(building);
            UpdateAveragePopulation();
        }

        // Обновление среднего количества жильцов/работников по компании
        private void UpdateAveragePopulation()
        {
            AveragePopulation = Buildings.Average(b => b.CalculateAverage());
        }

        // Упорядочивание списка строений по указанным критериям и вывод информации
        public void SortAndPrintBuildings()
        {
            var sortedBuildings = Buildings.OrderBy(b => b.CalculateAverage())
                                           .ThenBy(b => b.Type == "Residential" ? 0 : 1)
                                           .ThenBy(b => b.Address)
                                           .ToList();

            Console.WriteLine("Sorted Buildings:");
            foreach (var building in sortedBuildings)
            {
                Console.WriteLine($"Adress: {building.Address}, Average: {building.CalculateAverage()}");
            }
        }

        // Вывод первых двух объектов из отсортированного списка
        public void PrintFirstTwoBuildings()
        {
            Console.WriteLine("First Two Buildings:");
            for (int i = 0; i < Math.Min(2, Buildings.Count); i++)
            {
                var building = Buildings[i];
                Console.WriteLine($"Adress: {building.Address}, Average: {building.CalculateAverage()}");
            }
        }

        // Вывод последних трех адресов из отсортированного списка
        public void PrintLastThreeAddresses()
        {
            Console.WriteLine("Last Three Addresses:");
            for (int i = 0; i < Math.Min(3, Buildings.Count); i++)
            {
                int index = Buildings.Count - 1 - i;
                var building = Buildings[index];
                Console.WriteLine($"Address: {building.Address}, Average: {building.CalculateAverage()}");
            }
        }

        // Сохранение данных в JSON файл
        public void SaveToJson(string filename)
        {
            var json = JsonConvert.SerializeObject(Buildings, Formatting.Indented);
            File.WriteAllText("C:/Users/NITRO/Downloads/Lab_3_Nastya/" + filename + ".json", json);
        }

        // Загрузка данных из JSON файла
        public void LoadFromJson(string filename)
        {
            var json = File.ReadAllText(filename);
            double var1 = 0; int var2 = 0;
            string type = "", Adress = "";
            int index = 0;
            for (int i = 0; i < json.Length; i++)
            {
                if (json[i] == ':')
                {
                    string buff = "";
                    if (index < 2) { i += 2; }
                    else { i += 3; }
                    for (int j = i; json[j] != ',' && json[j] != '\\' && json[j] != '\"'; j++)
                    {
                        buff += json[j];
                        i++;
                    }
                    switch (index)
                    {
                        case 0:
                            // for RoomCount or Area
                            var1 = double.Parse(buff, System.Globalization.CultureInfo.InvariantCulture); index++; continue;
                        case 1:
                            // for ApartamentCount or EmployeeCount
                            var2 = Int32.Parse(buff); index++; continue;
                        case 2:
                            // for Type
                            type = buff; index++; continue;
                        case 3:
                            // for Adress
                            Adress = buff;
                            if (type[0] == 'r')
                            {
                                var building = new ResidentialBuilding()
                                {
                                    Type = type,
                                    ApartmentCount = var2,
                                    RoomCount = Convert.ToInt32(var1),
                                    Address = Adress
                                };
                                AddBuilding(building);
                            }
                            else
                            {
                                var building = new NonResidentialBuilding()
                                {
                                    Type = "NonResidential",
                                    Area = var1,
                                    EmployeeCount = var2,
                                    Address = Adress
                                };
                                AddBuilding(building);
                            }
                            type = ""; Adress = ""; index = 0;
                            var1 = 0; var2 = 0; continue;
                    }

                }
            }
        }
    }
}