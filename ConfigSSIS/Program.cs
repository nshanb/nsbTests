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
        static string destPath = @".\";

        static void Main(string[] args) // scan package in 3 ways
        {
            myPackageChange();
            //myPackageWork();
            //myPackageReplaceEdit_LogStart();
            //PackageUtils.ScanApplication();
            Console.WriteLine("Press any key to Continue.");
            Console.ReadLine();
        }

        static void myPackageWork()
        {
            Package package;
            package = PackageUtils.LoadPackage("CheckSchema", null as Project);
            PackageUtils.ScanPackage(package);
            //PackageUtils.ScanMyTaskHostPackage(package, "LogStart");
        }
        static void myPackageChange()
        {
            Package package = PackageUtils.LoadPackage("DoAllSchema", null as Project);
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
        static void myPackageReplaceEdit_LogStart()
        {
            string pText;
            Project project = Project.OpenProject(PackageUtils.projectPath);
            string taskHostName = "LogStart";

            Package packageS;
            packageS = PackageUtils.LoadPackage("EmptyPlaceHolder", project);
            TaskHost ths = packageS.Executables.Cast<TaskHost>().Where(t => t.Name == taskHostName).SingleOrDefault(); ;
            Variable vars1 = ths.Variables.Cast<Variable>().FirstOrDefault(x => x.Name == "vSource");
            Variable vars2 = ths.Variables.Cast<Variable>().FirstOrDefault(x => x.Name == "vDestination");
            ths.Variables.Remove(vars2);
            DtsProperty props = ths.Properties.Cast<DtsProperty>().FirstOrDefault(x => x.Name == "SqlStatementSource");
            Object propVal = props.GetValue(ths);
            props = ths.Properties.Cast<DtsProperty>().FirstOrDefault(x => x.Name == "ParameterBindings");
            Microsoft.SqlServer.Dts.Tasks.ExecuteSQLTask.IDTSParameterBindings propVal1 = props.GetValue(ths) as Microsoft.SqlServer.Dts.Tasks.ExecuteSQLTask.IDTSParameterBindings;


            List<Package> destinations = new List<Package>();
            TaskHost th;
            Package p;
            foreach(PackageItem pItem in project.PackageItems)
            {
                p = pItem.Package;
                //if (p.Name != "PlaceHolder_IdMod" && p.Name != "zMatchResult_IdMod") continue;
                foreach (Executable e in p.Executables)
                {
                    th = e as TaskHost;
                    if (th != null && th.Name == taskHostName)
                    {
                        Console.WriteLine("Package:{0};", p.Name);
                        destinations.Add(p);
                        //PackageUtils.ScanTaskHostPackage(th);
                        Variable var = th.Variables.Cast<Variable>().FirstOrDefault(x => x.Name == "vSource");
                        if (var == null)
                        {
                            Console.WriteLine("  NEEDS!");

                            Variable v = th.Variables.Add(vars1.Name, vars1.ReadOnly, vars1.Namespace, vars1.Value);
                            v.EvaluateAsExpression = vars1.EvaluateAsExpression;
                            v.Expression = vars1.Expression;
                            v = th.Variables.Add(vars2.Name, vars2.ReadOnly, vars2.Namespace, vars2.Value);
                            v.EvaluateAsExpression = vars2.EvaluateAsExpression;
                            v.Expression = vars2.Expression;

                            DtsProperty prop = ths.Properties.Cast<DtsProperty>().FirstOrDefault(x => x.Name == "SqlStatementSource");
                            prop.SetValue(th, propVal);
                            prop = ths.Properties.Cast<DtsProperty>().FirstOrDefault(x => x.Name == "ParameterBindings");
                            Microsoft.SqlServer.Dts.Tasks.ExecuteSQLTask.IDTSParameterBindings propVald = props.GetValue(th) as Microsoft.SqlServer.Dts.Tasks.ExecuteSQLTask.IDTSParameterBindings;

                            var pbind = propVald.Add();
                            pbind.DataType = propVal1.GetBinding(3).DataType;
                            pbind.ParameterName = "2";
                            pbind.ParameterSize = propVal1.GetBinding(3).ParameterSize;
                            pbind.ParameterDirection = propVal1.GetBinding(3).ParameterDirection;
                            pbind.DtsVariableName = "User::vSource";

                            pbind = propVald.Add();
                            pbind.DataType = propVal1.GetBinding(3).DataType;
                            pbind.ParameterName = "3";
                            pbind.ParameterSize = propVal1.GetBinding(3).ParameterSize;
                            pbind.ParameterDirection = propVal1.GetBinding(3).ParameterDirection;
                            pbind.DtsVariableName = "User::vDestination";

                            p.SaveToXML(out pText, null);
                            File.WriteAllText(destPath+p.Name+".dtsx", pText);
                        }
                    }
                }
            }
                
              
            //packageS.SaveToXML(out pText, null);
            //Executable exLogStartS = null;
            //foreach (Executable ex in packageS.Executables)
            //{
            //    if ((ex as TaskHost).Name == "LogStart")
            //    {
            //        exLogStartS = ex;
            //    }
            //}
            //Package packageD;
            //packageD = PackageUtils.LoadPackage("PlaceHolder_FullJoin", project);
            //Executable exLogStartD = null;
            //foreach (Executable ex in packageS.Executables)
            //{
            //    if ((ex as TaskHost).Name == "LogStart")
            //    {
            //        exLogStartD = ex;
            //    }
            //}

            //packageS.Executables.Remove(exLogStartS); // partadir e? !!!
            //(exLogStartS as TaskHost).Name = "LogStart1";
            //packageD.Executables.Join(exLogStartS);


            //packageD.SaveToXML(out pText, null);
            //File.WriteAllText(destFullPath, pText);
        }
    }
}
