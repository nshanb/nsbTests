using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleGeometry
{
  public class Polygon : IShape
  {
    public int EdgeCount;
    protected Polygon( IEnumerable<Point> vertices)
    {
      if (vertices.Any(x => x == null)) throw new ArgumentNullException("vertices", "Null points are not accepted as vertices.");
      EdgeCount = vertices.Count();
      if (EdgeCount < 3 ) throw new ArgumentException("vertices", "Number of vertices must be 3 or more.");
    }
    public static Polygon Triangle(Point A, Point B, Point C)
    {
      List<Point> aList = new List<Point>();
      aList.Add(A); aList.Add(B); aList.Add(C);
      if (!checkVertices(aList)) return null;
      return new Polygon(aList);
    }
    private static bool checkVertices(IEnumerable<Point> vertices)
    {
      return false;
    }
    public double Area()
    {
      throw new NotImplementedException();
    }
    public double Perimeter()
    {
      throw new NotImplementedException();
    }
    public bool IsSelfIntersecting()
    {
      throw new NotImplementedException();
    }
  }
}
