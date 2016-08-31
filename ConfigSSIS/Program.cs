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

        static void Main(string[] args)
        {
            //myPackageChange();
            myPackageWork();
            Console.ReadLine();
        }

        static void myPackageWork()
        {
            Package package;
            package = PackageUtils.LoadPackage("CopyByFullJoin");
            PackageUtils.ScanPackage(package);
            PackageUtils.ScanMyTaskHostPackage(package, "Data Flow Task");
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
    }
}
