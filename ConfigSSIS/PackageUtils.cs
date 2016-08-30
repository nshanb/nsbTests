using Microsoft.SqlServer.Dts.Runtime;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConfigSSIS
{
    class PackageUtils
    {
        static string projectRoot = @"D:\Nshan\nsbTests\SSIS\";

        static string projectPath
        {
            get
            {
                return projectRoot + @"bin\Development\SSIS.ispac";
            }
        }
        public static Package LoadPackage(string packageName)
        {
            string xmlStr;
            xmlStr = File.ReadAllText(projectRoot + packageName + ".dtsx");
            Package package = new Package();
            package.LoadFromXML(xmlStr, null);

            return package;
        }
        public static Package LoadPackage(string packageName, Application app)
        {
            if(app == null )
            {
                app = new Application();
            }
            return app.LoadPackage(projectRoot + packageName + ".dtsx", null);
        }
        public static Package LoadPackage(string packageName, Project project)
        {
            if( project == null )
            {
                project = Project.OpenProject(projectPath);
            }
            var pi = project.PackageItems.SingleOrDefault(x => x.Package.Name == packageName);
            if( pi == null ) return null;
            return pi.Package;
        }
        public static void ScanPackage(Package package)
        {
            Console.WriteLine("Scanning Package:{0}", package.Name);
            Console.WriteLine("CreationName:{0}; PackageType:{1}", package.CreationName, package.PackageType);
            Console.WriteLine("GetPackagePath:{0}; GetExecutionPath:{1}", package.GetPackagePath(), package.GetExecutionPath());

            Console.WriteLine("Parameters:");
            foreach (Parameter par in package.Parameters)
            {
                Console.Write(par.CreationName + " - " + par.Name + "; ");
            }

            Console.WriteLine();
            Console.WriteLine("Variables:");
            foreach (Variable par in package.Variables.Cast<Variable>().OrderBy(x => x.QualifiedName))
            {
                Console.Write("{0} - DataType:{1} - Value:{2}; ", par.QualifiedName, par.DataType, par.Value);
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
            Console.WriteLine("Configurations:");
            foreach (Configuration par in package.Configurations)
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

            Console.WriteLine();
            Console.WriteLine("Errors:");
            foreach (DtsError par in package.Errors)
            {
                Console.Write(par.Source + " - " + par.Description + "; ");
            }

            Console.WriteLine();
            Console.WriteLine("Warnings:");
            foreach (DtsWarning par in package.Warnings)
            {
                Console.Write(par.SubComponent + " - " + par.Description + " ");
            }

            Console.WriteLine();
            Console.WriteLine("Configurations:");
            foreach (Configuration par in package.Configurations)
            {
                Console.Write(par.Name + "; ");
            }
        }
        public static TaskHost ScanMyTaskHostPackage(Package package, string taskHostName)
        {
            Console.WriteLine("Scanning taskHost:{0}/{1}", package.Name, taskHostName);
            TaskHost th = package.Executables.Cast<TaskHost>().Where(t => t.Name == taskHostName).SingleOrDefault();
            if( th == null )
            {
                Console.WriteLine("No such component!");
                return null;
            }
            Console.WriteLine("HostType:{0}; CreationName:{1}", th.HostType, th.CreationName);

            Console.WriteLine("Parameters:");
            foreach (DtsProperty par in th.Properties)
            {
                Console.WriteLine(par.Name + ":" + par.Type + " - " + par.GetValue(th) + "; ");

            }

            Console.WriteLine("Variables:");
            foreach (Variable par in th.Variables)
            {
                Console.Write(par.Name+"; ");
            }

            return th;
        }

    }
}
