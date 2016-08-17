using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SimpleGeometry;

namespace TConsole
{
  class Program
  {
    static void Main(string[] args)
    {
      Console.WriteLine("Hello World");
      PlayWithPolygons();

      Console.Read();
    }

    static void PlayWithPolygons()
    {
      string D;
      Point A, B, C;
      double x, y;

      Console.Write("Point1.X");
      D = Console.ReadLine();
      x = double.Parse(D);

      Console.Write("Point1.Y");
      D = Console.ReadLine();
      y = double.Parse(D);

      A = new Point(x, y);
      Console.WriteLine(A);
      Console.WriteLine(Polygon.Triangle(A, A, A));
    }
  }
}
