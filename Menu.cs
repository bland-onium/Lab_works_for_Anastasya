using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nastya
{
    class Menu
    {
        public static void StartProgram()
        {
            Parallelepiped Parall_1 = new Parallelepiped(5, 7, 2);
            Parall_1.Move(0, 0, 0);

            Parallelepiped Parall_2 = new Parallelepiped(3, 4, 6);
            Parall_2.Move(0, 0, 0);

            Console.WriteLine("List of comands which you can use\n\n" +
                    "\t1. Print <number of figure> - to show info about the chosen parallelepiped\n" +
                    "\t2. Move - to move figures in x|y|z coordinates\n" +
                    "\t3. Resize - to change length/width/height of figure\n" +
                    "\t4. Intersection - to find parallelepiped which contaned in another two\n" +
                    "\t5. Summ - to build parallelogramm which contains two another\n" +
                    "\t6. quit - to exit from program\n");

            bool quit = false;

            while (!quit)
            {
                Console.Write("Select the comand (write number of chosen comand): ");
                int comand = Convert.ToInt32(Console.ReadLine());

                switch (comand)
                {
                    //Getting Info
                    case 1:
                        Console.Write("Select parallelepiped to get info (1 or 2)");
                        string answer = Convert.ToString(Console.ReadLine());
                        if (answer == "1")
                        {
                            Parall_1.PrintDescription();
                            break;
                        }
                        if (answer == "2")
                        {
                            Parall_2.PrintDescription();
                            break;
                        }
                        else { break; }

                    //Move    
                    case 2:
                        Console.WriteLine("Enter number of parallelepiped: ");
                        int parnum = Convert.ToInt32(Console.ReadLine());
                        Console.Write("Enter new position (x, y, z): ");
                        double newX = Convert.ToDouble(Console.ReadLine());
                        double newY = Convert.ToDouble(Console.ReadLine());
                        double newZ = Convert.ToDouble(Console.ReadLine());
                        if (parnum == 1)
                        {
                            Parall_1.Move(newX, newY, newZ);
                            Console.WriteLine("Parallelepiped moved successfully");
                            Parall_1.PrintDescription();
                        }
                        else if (parnum == 2)
                        {
                            Parall_2.Move(newX, newY, newZ);
                            Console.WriteLine("Parallelepiped moved successfully");
                            Parall_2.PrintDescription();
                        }
                        else { Console.WriteLine("Wrong number"); }
                        break;

                    //Resize
                    case 3:
                        Console.WriteLine("Enter number of parallelepiped: ");
                        parnum = Convert.ToInt32(Console.ReadLine());
                        Console.Write("Enter new length, width, height: ");
                        double newLength = Convert.ToDouble(Console.ReadLine());
                        double newWidth = Convert.ToDouble(Console.ReadLine());
                        double newHeight = Convert.ToDouble(Console.ReadLine());
                        if (parnum == 1)
                        {
                            Parall_1.Resize(newLength, newWidth, newHeight);
                            Console.WriteLine("Parallelepiped resized successfully");
                            Parall_1.PrintDescription();
                        }
                        else if (parnum == 2)
                        {
                            Parall_2.Resize(newLength, newWidth, newHeight);
                            Console.WriteLine("Parallelepiped resized successfully");
                            Parall_2.PrintDescription();
                        }
                        else { Console.WriteLine("Wrong number"); }
                        break;

                    //Intersection
                    case 4:
                        Parallelepiped intersecting = Parall_1.Intersecting(Parall_2);

                        if (intersecting != null)
                        {
                            Console.WriteLine("Intersecting parallelepiped:");
                            intersecting.PrintDescription();
                        }
                        break;

                    //Summ of two
                    case 5:
                        Parallelepiped combined = Parall_1.Combined(Parall_2);

                        if (combined != null)
                        {
                            Console.WriteLine("Combined parallelepiped:");
                            combined.PrintDescription();
                        }
                        break;

                    //exit
                    case 6:
                        quit = true;
                        break;

                    //errors and mistakes
                    default:
                        Console.WriteLine("Invalid option");
                        break;
                }

                Console.WriteLine();
            }
        }
    }
}
