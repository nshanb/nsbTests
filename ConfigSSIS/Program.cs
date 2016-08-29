using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.SqlServer.Dts.Runtime;
using System.IO;

namespace ConfigSSIS
{
    class Program
    {
        static string sourcePath = @".\SchemaSync.dtsx";
        static string destPath = @".\dest.dtsx";
        static void Main(string[] args)
        {
            myPackage1();
            Console.ReadLine();
        }

        static void myPackage()
        {
            Package package;
            string xmlStr;

            xmlStr = File.ReadAllText(sourcePath);
            package = new Package();
            package.LoadFromXML(xmlStr, null);

            package.SaveToXML(out xmlStr, null);
            File.WriteAllText(destPath, xmlStr);

        }
        static void myPackage1()
        {
            Package package;
            Application app = new Application();
            package = app.LoadPackage(sourcePath, null);

            //lookMyPackage(package);
            //Console.WriteLine(); Console.WriteLine("TransferTables:");
            lookMyTaskHostPackage(package, "TransferTables");

            app.SaveToXml(destPath, package,null);
        }
        static void changeMyPackage(Package package)
        {
        }
        static void lookMyPackage(Package package)
        {
            Console.WriteLine("Parameters:");
            //(package.Parameters as ICollection<Parameter>).ToList().ForEach(p => Console.Write(p.Name + "; "));
            foreach (Parameter par in package.Parameters)
            {
                Console.Write(par.CreationName + " - " + par.Name + "; ");
            }

            Console.WriteLine();
            Console.WriteLine("Variables:");
            foreach (Variable par in package.Variables)
            {
                Console.Write(par.Namespace + ":" + par.Name + "; ");
            }

            Console.WriteLine();
            Console.WriteLine("Properties:");
            foreach (DtsProperty par in package.Properties)
            {
                Console.Write(par.PropertyKind.ToString() + " - " + par.Name + "; ");
            }

            Console.WriteLine();
            Console.WriteLine("Connections:");
            foreach (ConnectionManager par in package.Connections)
            {
                Console.Write(par.Name + "; ");
            }

            Console.WriteLine();
            Console.WriteLine("ExtendedProperties:");
            foreach (ExtendedProperty par in package.ExtendedProperties)
            {
                Console.Write(par.Name + "; ");
            }

            Console.WriteLine();
            Console.WriteLine("Executables:");
            Microsoft.SqlServer.Dts.Runtime.TaskHost th;
            foreach (Executable par in package.Executables)
            {
                Console.WriteLine(par.ToString());
                th = par as Microsoft.SqlServer.Dts.Runtime.TaskHost;
                Console.WriteLine(th.CreationName + " - [" + th.Name + "]");
            }

        }
        static void lookMyTaskHostPackage(Package package, string taskHostName)
        {
            TaskHost th = package.Executables.Cast<TaskHost>().Where(t => t.Name == taskHostName).SingleOrDefault();

            Console.WriteLine("Parameters:");
            //(package.Parameters as ICollection<Parameter>).ToList().ForEach(p => Console.Write(p.Name + "; "));
            DtsProperty tableLst = null;
            foreach (DtsProperty par in th.Properties)
            {
                Console.WriteLine(par.Name + ":" + par.Type + " - " + par.GetValue(th) + "; ");
                if (par.Name == "TablesList") tableLst = par;
            }

            Console.WriteLine("Parameters:");
            if (tableLst != null )
            {
                System.Collections.Specialized.StringCollection lst = tableLst.GetValue(th) as System.Collections.Specialized.StringCollection;
                foreach(string s in lst)
                {
                    Console.Write(s + "; ");
                }
                System.Collections.Specialized.StringCollection lst1 = new System.Collections.Specialized.StringCollection() { "[dbo].[Table2]" };
                tableLst.SetValue(th, lst1);

            }
        }
    }
}
