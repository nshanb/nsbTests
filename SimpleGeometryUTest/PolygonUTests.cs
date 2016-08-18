using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SimpleGeometry;

namespace SimpleGeometryUTest
{
  [TestClass()]
  public class PolygonUTests
  {
    class PolygonProtectedTester : Polygon
    {
      public PolygonProtectedTester(IEnumerable<Point> vertices) : base(vertices)
      {

      }
    }
    private List<Point> _PointList;
    private Point _A;
    private Point _B;
    private Point _C;
    private Point _D;

    [TestInitialize]
    public void Initialize()
    {
      _A = new Point(0, 0);
      _B = new Point(2, 0);
      _C = new Point(0, 2);
      _D = new Point(0, 2);
    }

    [TestMethod()]
    public void AreaTest()
    {
      throw new NotImplementedException();
    }

    [TestMethod()]
    [ExpectedException(typeof(ArgumentException))]
    public void PolygonConstructorTest()
    {
      PolygonProtectedTester polygon = new PolygonProtectedTester(new List<Point>() { _A, _B });
    }

    [TestMethod()]
    public void TriangleTest()
    {
      Polygon triangle;
      try
      {
        triangle = Polygon.Triangle(null, null, null);
        Assert.Fail("ArgumentNullException didn't happen.");
      }
      catch (ArgumentNullException) { }

      triangle = Polygon.Triangle(_A, _A, _A);
      Assert.IsNull(triangle, "Two consecutive points must be different.");

      triangle = Polygon.Triangle(_A, _B, _A);
      Assert.IsNull("Two consecutive points in polygon must be different.");

      triangle = Polygon.Triangle(_A, _B, _C);
      Assert.IsNotNull(triangle, "triangle polygon constructor failed.");
    }
  }
}