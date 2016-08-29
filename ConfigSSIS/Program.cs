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
            package = app.LoadPackage(sourcePath,null);

            lookMyPackage(package);

            //app.SaveToXml(destPath, package,null);
        }
        static void changeMyPackage(Package package)
        {
        }
        static void lookMyPackage(Package package)
        {
            Console.WriteLine("Parameters:");
            foreach (Parameter par in package.Parameters)
            {
                Console.Write(par.Name+"; ");
            }

            Console.WriteLine();
            Console.WriteLine("Variables:");
            foreach (Variable par in package.Variables)
            {
                Console.Write(par.Name + "; ");
            }

            Console.WriteLine();
            Console.WriteLine("Properties:");
            foreach (DtsProperty par in package.Properties)
            {
                Console.Write(par.Name + "; ");
            }

            Console.WriteLine();
            Console.WriteLine("Connections:");
            foreach (ConnectionManager par in package.Connections)
            {
                Console.Write(par.Name + "; ");
            }

            Console.WriteLine();
            Console.WriteLine("Executables:");
            Microsoft.SqlServer.Dts.Runtime.TaskHost th;
            foreach (Executable par in package.Executables)
            {
                Console.Write(par.ToString() + "; ");
                th = par as Microsoft.SqlServer.Dts.Runtime.TaskHost;
                Console.Write(th.Name + "; ");
            }

        }
    }
}
