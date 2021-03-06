﻿using Microsoft.SqlServer.Dts.Runtime;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConfigSSIS
{
    class PackageUtils
    {
        static string projectRoot = ConfigurationManager.AppSettings["ProjectRoot"];

        static public string projectPath
        {
            get
            {
                return projectRoot + @"bin\Development\BetConstruct.SSIS.ispac";
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
            if (app == null)
            {
                app = new Application();
            }
            return app.LoadPackage(projectRoot + packageName + ".dtsx", null);
        }
        public static Package LoadPackage(string packageName, Project project)
        {
            if (project == null)
            {
                project = Project.OpenProject(projectPath);
            }
            var pi = project.PackageItems.SingleOrDefault(x => x.Package.Name == packageName);
            if (pi == null) return null;
            return pi.Package;
        }
        // Print package Propertes, Parameters, Tasks, Variables
        public static void ScanPackage(Package package)
        {
            Console.WriteLine("Scanning Package:{0}", package.Name);
            Console.WriteLine("CreationName:{0}; PackageType:{1}", package.CreationName, package.PackageType);
            Console.WriteLine("GetPackagePath:{0}; GetExecutionPath:{1}", package.GetPackagePath(), package.GetExecutionPath());

            Console.WriteLine("Parameters:");
            foreach (Parameter par in package.Parameters)
            {
                Console.WriteLine("CreationName:{0} - Name:{1} - DataType:{2} - Value:{3}", par.CreationName, par.Name, par.DataType, par.Value);
            }

            Console.WriteLine();
            Console.WriteLine("Variables:");
            foreach (Variable par in package.Variables.Cast<Variable>().OrderBy(x => x.QualifiedName))
            {
                Console.WriteLine("{0} - DataType:{1} - Value:{2}; ", par.QualifiedName, par.DataType, par.Value);
            }

            Console.WriteLine();
            Console.WriteLine("Properties:");
            foreach (DtsProperty par in package.Properties)
            {
                object val;
                try
                {
                    val = par.GetValue(package);
                }
                catch (Exception ex)
                {
                    val = "Exception:" + ex.Message;
                }
                Console.WriteLine("Name:{0} - PropertyKind:{1}", par.Name, par.PropertyKind, val);
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
            foreach (Microsoft.SqlServer.Dts.Runtime.Configuration par in package.Configurations)
            {
                Console.Write(par.Name + "; ");
            }

            Console.WriteLine();
            Console.WriteLine("Executables:");
            Microsoft.SqlServer.Dts.Runtime.TaskHost th;
            Microsoft.SqlServer.Dts.Runtime.ForEachLoop f;
            foreach (Executable par in package.Executables)
            {
                Console.WriteLine(par.ToString());
                th = par as Microsoft.SqlServer.Dts.Runtime.TaskHost;
                if (th != null)
                {
                    Console.WriteLine(th.CreationName + " - [" + th.Name + "]");
                }
                else
                {
                    f = par as Microsoft.SqlServer.Dts.Runtime.ForEachLoop;
                    Console.WriteLine(f.CreationName + " - [" + f.Name + "]");
                }
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
            foreach (Microsoft.SqlServer.Dts.Runtime.Configuration par in package.Configurations)
            {
                Console.Write(par.Name + "; ");
            }
        }
        // prints tasks properties, parameters, Variables
        public static TaskHost ScanTaskHostPackage(Package package, string taskHostName)
        {
            Console.WriteLine("Scanning taskHost:{0}/{1}", package.Name, taskHostName);
            // nsb TODO ForEachLoop ?
            TaskHost th = package.Executables.Cast<TaskHost>().Where(t => t.Name == taskHostName).SingleOrDefault();
            if (th == null)
            {
                Console.WriteLine("No such component!");
                return null;
            }
            return ScanTaskHostPackage(th);
        }
        public static TaskHost ScanTaskHostPackage(TaskHost th)
        {
            Console.WriteLine("HostType:{0}; CreationName:{1}; ParentName:{2}", th.HostType, th.CreationName, th.Parent.Name);

            Console.WriteLine("Properties:");
            foreach (DtsProperty par in th.Properties)
            {
                string val;
                try
                {
                    val = string.Format("{0}", par.GetValue(th));
                }
                catch (Exception ex)
                {
                    val = "Exception:" + ex.Message;
                }
                Console.WriteLine("{0} - Type:{1} - GetValue:{2}", par.Name, par.Type, val);
            }

            Console.WriteLine("Variables:");
            foreach (Variable par in th.Variables.Cast<Variable>().OrderBy(x => x.QualifiedName))
            {
                Console.WriteLine("{0} - DataType:{1} - Value:{2}; ", par.QualifiedName, par.DataType, par.Value);
            }

            return th;
        }

        // prints available (installed?) componenets
        public static void ScanApplication()
        {
            Application application = new Application();

            Console.WriteLine("PipelineComponentInfos:");
            foreach (PipelineComponentInfo componentInfo in application.PipelineComponentInfos)
            {
                Console.WriteLine("Name:{0} - CreationName:{1}", componentInfo.Name, componentInfo.CreationName);
            }
            Console.WriteLine("ConnectionInfos:");
            foreach (ConnectionInfo p in application.ConnectionInfos)
            {
                Console.WriteLine("Name:{0} - CreationName:{1}", p.Name, p.CreationName);
            }
            Console.WriteLine("TaskInfos:");
            foreach (Microsoft.SqlServer.Dts.Runtime.TaskInfo p in application.TaskInfos)
            {
                Console.WriteLine("Name:{0} - CreationName:{1}", p.Name, p.CreationName);
            }
        }

        public static int ChangeOrAddVaribaleExpression(Package p, string variableName, string expression)
        {
            Variable temp = null;
            int exists = 0;
            int update = 0;
            foreach (Variable var in p.Variables)
            {
                if (var.Name == variableName && var.Namespace == "User")
                {
                    temp = var;
                    exists++;
                }
            }
            if (temp == null)
            {
                exists++;
                temp = p.Variables.Add(variableName, false, "User", "");
                update++;
            }
            {
                if (!temp.EvaluateAsExpression)
                {
                    temp.EvaluateAsExpression = true;
                }
                if (temp.Expression != expression)
                {
                    temp.Expression = expression;
                    update++;
                }
            }

            return update;
        }
        public static int ChangeOrAddVaribaleExpression(TaskHost h, string variableName, string oldExpression, string newExpression)
        {
            Variable temp = null;
            int update = 0;
            foreach (Variable var in h.Variables)
            {
                if (var.Name == variableName && var.Namespace == "User")
                {
                    temp = var;
                }
            }
            if (temp == null)
            {
                temp = h.Variables.Add(variableName, false, "User", "");
                update++;
            }
            if (oldExpression != null)
            {
                if (temp.Expression == oldExpression)
                {
                    if (temp.Expression != newExpression)
                    {
                        temp.Expression = newExpression;
                        update++;
                    }
                }
            }
            else
            {
                if (!temp.EvaluateAsExpression)
                {
                    temp.EvaluateAsExpression = true;
                    update++;
                }
                if (temp.Expression != newExpression)
                {
                    temp.Expression = newExpression;
                    update++;
                }
            }

            return update;
        }

    }
}
// https://msdn.microsoft.com/en-us/library/ms135932.aspx
