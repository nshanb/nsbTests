using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.SqlServer.Dts.Runtime;
using System.IO;
using System.Collections.Specialized;

namespace ConfigSSIS
{
    class Program
    {
        static string destFullPath = @".\dest.dtsx";

        static void Main(string[] args) // scan package in 3 ways
        {
            myPackageChange();
            //myPackageWork();
            //myPackageReplace();
            //PackageUtils.ScanApplication();
            Console.WriteLine("Press any key to Continue.");
            Console.ReadLine();
        }

        static void myPackageWork()
        {
            Package package;
            package = PackageUtils.LoadPackage("EmptyPlaceHolder");
            PackageUtils.ScanPackage(package);
            PackageUtils.ScanMyTaskHostPackage(package, "LogStart");
        }
        static void myPackageChange()
        {
            Package package = PackageUtils.LoadPackage("SchemaSync");
            StringCollection tabelList = ChangeTasks.get_SchemaSync_TransferTables_TablesList(package);
            foreach (string s in tabelList) Console.WriteLine("{0}; ", s);

            List<SimpleDal.SyncTable> newTables = SimpleDal.getSyncTables();
            tabelList = new StringCollection();
            foreach (SimpleDal.SyncTable s in newTables) { Console.WriteLine("{0}; ", s.Name); tabelList.Add(s.Name); }
            ChangeTasks.set_SchemaSync_TransferTables_TablesList(package, tabelList);

            string pText;
            package.SaveToXML(out pText, null);
            File.WriteAllText(destFullPath, pText);
        }
        static void myPackageReplace()
        {
            Project project = Project.OpenProject(PackageUtils.projectPath);
            Package packageS;
            packageS = PackageUtils.LoadPackage("EmptyPlaceHolder",project);
            Executable exLogStartS = null;
            foreach ( Executable ex in packageS.Executables)
            {
                if( (ex as TaskHost).Name == "LogStart")
                {
                    exLogStartS = ex;
                }
            }
            Package packageD;
            packageD = PackageUtils.LoadPackage("PlaceHolder_FullJoin", project);
            Executable exLogStartD = null;
            foreach (Executable ex in packageS.Executables)
            {
                if ((ex as TaskHost).Name == "LogStart")
                {
                    exLogStartD = ex;
                }
            }

            packageS.Executables.Remove(exLogStartS); // partadir e? !!!
            (exLogStartS as TaskHost).Name = "LogStart1";
            packageD.Executables.Join(exLogStartS);

            string pText;
            packageD.SaveToXML(out pText, null);
            File.WriteAllText(destFullPath, pText);
        }
    }
}
