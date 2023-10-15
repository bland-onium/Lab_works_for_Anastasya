using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Nastya
{
    class Parallelepiped
    {
        private double length;
        private double width;
        private double height;
        private double x;
        private double y;
        private double z;

        public Parallelepiped(double length, double width, double height)
        {
            this.length = length;
            this.width = width;
            this.height = height;
        }

        public void Move(double x, double y, double z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }

        public void Resize(double length, double width, double height)
        {
            this.length = length;
            this.width = width;
            this.height = height;
        }

        public Parallelepiped Combined(Parallelepiped par)
        {
            double newX = Math.Min(this.x, par.x);
            double newY = Math.Min(this.y, par.y);
            double newZ = Math.Min(this.z, par.z);

            double newLength = Math.Max(this.x + this.length, par.x + par.length) - newX;
            double newWidth = Math.Max(this.y + this.width, par.y + par.width) - newY;
            double newHeight = Math.Max(this.z + this.height, par.z + par.height) - newZ;

            Parallelepiped Combined = new Parallelepiped(newLength, newWidth, newHeight);
            Combined.Move(newX, newY, newZ);

            return Combined;
        }

        public Parallelepiped Intersecting(Parallelepiped par)
        {
            double newX = Math.Max(this.x, par.x);
            double newY = Math.Max(this.y, par.y);
            double newZ = Math.Max(this.z, par.z);

            double newLength = Math.Min(this.x + this.length, par.x + par.length) - newX;
            double newWidth = Math.Min(this.y + this.width, par.y + par.width) - newY;
            double newHeight = Math.Min(this.z + this.height, par.z + par.height) - newZ;

            if (newLength < 0 || newWidth < 0 || newHeight < 0)
            {
                Console.WriteLine("No intersection");
                return null;
            }

            Parallelepiped intersecting = new Parallelepiped(newLength, newWidth, newHeight);
            intersecting.Move(newX, newY, newZ);

            return intersecting;
        }

        public void PrintDescription()
        {
            Console.WriteLine("Length: " + length);
            Console.WriteLine("Width: " + width);
            Console.WriteLine("Height: " + height);
            Console.WriteLine("Position (x, y, z): (" + x + ", " + y + ", " + z + ")");
        }
    }
}
