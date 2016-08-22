using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using RegExPG;
namespace RegExPG.Tests
{
    [TestClass()]
    public class FindInFilesTests
    {
        [TestMethod()]
        public void FindInFilesTest()
        {
            FindInFiles f = new FindInFiles();
            Assert.IsNull(f.FilesToSearch);
        }

    }
}