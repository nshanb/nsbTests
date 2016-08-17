using Microsoft.VisualStudio.TestTools.UnitTesting;
using SimpleGeometry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleGeometryUTest
{
  [TestClass()]
  public class PolygonUTests
  {
    private List<Point> _PointList;
    private Point _A;
    private Point _B;
    private Point _C;

    [TestInitialize]
    public void Initialize()
    {
      _A = new Point(0, 0);
      _B = new Point(0, 0);
      _C = new Point(0, 0);
    }

    [TestMethod()]
    public void AreaTest()
    {
      throw new NotImplementedException();
    }

    [TestMethod()]
    public void TriangleTest()
    {
      Polygon triangle = Polygon.Triangle(null, null, null);
      Assert.IsNull(triangle,"No null Point is accepted");
      triangle = Polygon.Triangle(_A, _A, _A);
      Assert.IsNull(triangle, "Two consecutive points must be different!");
    }
  }
}