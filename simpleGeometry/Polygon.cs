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
    private Polygon( IEnumerable<Point> vertices)
    {
      if (vertices.Any(x => x == null)) throw new ArgumentNullException("vertices", "Null points are not accepted as vertices.");
      EdgeCount = vertices.Count();
    }
    public static Polygon Triangle(Point A, Point B, Point C)
    {
      List<Point> aList = new List<Point>();
      aList.Add(A); aList.Add(B); aList.Add(C);
      return new Polygon(aList);
    }
    public int Area()
    {
      throw new NotImplementedException();
    }
    public int Perimeter()
    {
      throw new NotImplementedException();
    }
    public bool IsSelfIntersecting()
    {
      throw new NotImplementedException();
    }
  }
}
