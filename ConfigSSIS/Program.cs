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
            myPackageChange_Build_AllzTables_UpdateParams();
            //myPackageChange_Build_AllzTables_Change();
            //myPackageChange_Build_AllzPackages_List();
            //myPackageDisable_ClearStaging();
            //myPackageReplaceEdit_LogStart1();
            //myPackageAdd_AddPackageConnection();
            //myPackageAdd_PartnerIdDefaultUserPSWD();
            //myPackageChangeDoAllSchema();
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
        static void myPackageChange_Build_AllzPackages_List()
        {
            Project project = Project.OpenProject(PackageUtils.projectPath);
            List<SimpleDal.SyncTable> tables = SimpleDal.getSyncTables();
            List<Package> zpackages = new List<Package>();
            Package etalon = null;
            foreach (var pi in project.PackageItems)
            {
                if (pi.Package.Name == "EmptyPlaceHolder")
                {
                    etalon = pi.Package;
                    continue;
                }
                if (pi.Package.Name.Substring(0, 1) != "z") continue;
                zpackages.Add(pi.Package);
            }
            Console.WriteLine("Synctable:{0}; Packages:{1}", tables.Count, zpackages.Count);
            int exists = 0;
            foreach (SimpleDal.SyncTable t in tables.OrderBy(x => x.Name))
            {
                Package p = zpackages.Where(x => (x.Name + ".dtsx").ToLower() == t.PackageName.ToLower()).SingleOrDefault();
                if (p == null)
                {
                    Console.WriteLine("Synctable:{0}; Packages:{1}", t.Name, "missing!");
                    exists++;
                    continue;
                }
                if (!p.Name.Contains(t.Name))
                {
                    Console.WriteLine("Synctable:{0}; Packages:{1} {2}", t.Name, p.Name, "Wrong package name!");
                    exists++;
                    continue;
                }
            }
            foreach (var p in zpackages)
            {
                var t = tables.Where(x => x.PackageName.ToLower() == (p.Name + ".dtsx").ToLower()).SingleOrDefault();
                if (t == null)
                {
                    Console.WriteLine("Unused package: {0}", p.Name);
                }
            }
            if (exists != 0)
            {
                Console.WriteLine("Error! Processing stopped");
            }
            zpackages.Add(etalon);
            tables.Add(new SimpleDal.SyncTable() { Name = "EmptyPlaceHolder", PackageName = "EmptyPlaceHolder.dtsx" });
            foreach (var t in tables.OrderBy(x => x.Name))
            {
                exists = 0;
                //if (!t.FullJoin) continue;
                Package p = zpackages.Where(x => (x.Name + ".dtsx").ToLower() == t.PackageName.ToLower()).SingleOrDefault();
                foreach (Parameter par in p.Parameters)
                {
                    if (par.Name == "partnerSpec")
                    {
                        exists++;
                        //p.Parameters.Remove("Delete_FriendlyExpression");
                        break;
                    }
                }
                //Variable variable = null;
                //foreach (Variable v in p.Variables)
                //{
                //    if (v.Name == "destDelStagingTable")
                //    {
                //        variable = v;
                //        exists++;
                //        break;
                //    }
                //}
                if (exists == 0)
                {
                    var par = p.Parameters.Add("partnerSpec", TypeCode.String);
                    par.Value = "None";
                    //p.Variables.Remove(variable);
                    string pText;
                    Console.WriteLine("changing : {0}", p.Name);
                    p.SaveToXML(out pText, null);
                    File.WriteAllText(destPath + p.Name + ".dtsx", pText);
                }
            }
        }
        static void myPackageChange_Build_AllzTables_UpdateParams()
        {
            Project project = Project.OpenProject(PackageUtils.projectPath);
            List<SimpleDal.SyncTable> tables = SimpleDal.getSyncTables();
            tables.Add(new SimpleDal.SyncTable() { Name = "EmptyPlaceHolder", PackageName = "EmptyPlaceHolder.dtsx", PartnerSpec = "" });
            int exists;
            int update;
            Package p;
            foreach (SimpleDal.SyncTable t in tables.OrderBy(x => x.Name))
            {
                p = project.PackageItems.Where(x => (x.Package.Name + ".dtsx").ToLower() == t.PackageName.ToLower()).SingleOrDefault().Package;
                if (p.Name == "EmptyPlaceHolder")
                {
                    Console.WriteLine("");
                }
                exists = 0;
                update = 0;
                Parameter deltaModifiedSeconds_P = null;
                foreach (Parameter par in p.Parameters)
                {
                    if (par.Name == "partnerSpec")
                    {
                        exists++;
                        var s = (string)par.Value;
                        if (s != t.PartnerSpec)
                        {
                            par.Value = String.IsNullOrEmpty(t.PartnerSpec) ? "" : t.PartnerSpec;
                            update++;
                        }
                    }
                    if (par.Name == "SyncTableName")
                    {
                        exists++;
                        var s = (string)par.Value;
                        if (s != t.Name)
                        {
                            par.Value = t.Name;
                            update++;
                        }
                    }
                    if (par.Name == "deltaModifiedSeconds")
                    {
                        exists++;
                        deltaModifiedSeconds_P = par;
                        var s = (int)par.Value;
                        if (s != 0)
                        {
                            par.Value = 0;
                            update++;
                        }
                    }
                }
                if (deltaModifiedSeconds_P == null)
                {
                    deltaModifiedSeconds_P = p.Parameters.Add("deltaModifiedSeconds", TypeCode.Int32);
                    exists++;
                    deltaModifiedSeconds_P.Value = 0;
                    update++;
                }
                Variable tempVar = null;
                foreach (Variable var in p.Variables)
                {
                    if (var.Name == "partnerSpecBool" && var.Namespace == "User")
                    {
                        tempVar = var;
                    }
                }
                if (tempVar != null)
                {
                    p.Variables.Remove(tempVar);
                    update++;
                }
                update += PackageUtils.ChangeOrAddVaribaleExpression(p, "deltaModifiedSeconds", "@[$Package::deltaModifiedSeconds]+ @[$Project::deltaModifiedSeconds]");
                update += PackageUtils.ChangeOrAddVaribaleExpression(p, "partnerSpec1", "REPLACE( @[$Package::partnerSpec], \"?\", (DT_WSTR,10) @[$Package::PartnerId] )");
                update += PackageUtils.ChangeOrAddVaribaleExpression(p, "tableNameCanonical", "\"[\"+@[$Package::SyncTableName]+\"]\"");

                TaskHost th;
                foreach (Executable e in p.Executables)
                {
                    th = e as TaskHost;
                    if (th == null)
                    {
                        continue;
                    }
                    if (th.Name == "Data Flow Task" && th.CreationName.Contains("SSIS.Pipeline.5"))
                    {
                        //Console.WriteLine("package : {0},{1}", p.Name, th.CreationName);"Select * from " + @[$Package::SyncTableName] + " where Id>"+ (DT_WSTR,10) @[User::lastKey] +" order by Id"
                        exists++;
                        //update += PackageUtils.ChangeOrAddVaribaleExpression(th, "sourceSelect", "\"Select * from \" + @[User::tableNameCanonical] + \" order by Id\""
                        //                                                                       , "\"Select * from \" + @[User::tableNameCanonical] + \" \" + @[User::partnerSpec1] + \" order by Id\"");
                        //update += PackageUtils.ChangeOrAddVaribaleExpression(th, "sourceSelect", "\"Select * from \" + @[$Package::SyncTableName] + \" where Id > \"+ (DT_WSTR,10) @[User::lastKey] +\" order by Id\""
                        //                                                                       , "kuku");
                    }
                }

                if (exists != 4)
                {
                    Console.WriteLine("package : {0}, missing parameter! : {1}", p.Name, exists);
                    //Parameter par =  p.Parameters.Add("partnerSpec", TypeCode.String);
                    //par.Value = String.IsNullOrEmpty(t.PartnerSpec) ? "None" : t.PartnerSpec;
                }
                if (update > 0)
                {
                    string pText;
                    Console.WriteLine("package : {0}, changeN : {1}", p.Name, update);
                    p.SaveToXML(out pText, null);
                    File.WriteAllText(destPath + p.Name + ".dtsx", pText);
                }
            }
        }

        static void myPackageChange_Build_AllzTables_Change()
        {
            Project project = Project.OpenProject(PackageUtils.projectPath);
            List<SimpleDal.SyncTable> tables = SimpleDal.getSyncTables();
            tables.Add(new SimpleDal.SyncTable() { Name = "EmptyPlaceHolder", PackageName = "EmptyPlaceHolder.dtsx" });
            foreach (SimpleDal.SyncTable t in tables.OrderBy(x => x.Name))
            {
                Package p = project.PackageItems.Where(x => (x.Package.Name + ".dtsx").ToLower() == t.PackageName.ToLower()).SingleOrDefault().Package;
                TaskHost th;
                Variable variable = null;
                int update = 0;
                Executable executable = null;
                foreach (Executable e in p.Executables)
                {
                    th = e as TaskHost;
                    if (th == null)
                    {
                        //Console.WriteLine("type : {0}", e.GetType()); // Microsoft.SqlServer.Dts.Runtime.Sequence; Microsoft.SqlServer.Dts.Runtime.ForEachLoop
                        continue;
                    }
                    if (th.Name == "GetLastKey" && th.CreationName.Contains("ExecuteSQLTask"))
                    {
                        Console.WriteLine("package : {0}", p.Name);
                        //foreach (Variable v in p.Variables)
                        //{
                        //    if (v.Name == "syncLogName")
                        //    {
                        //        variable = v;
                        //        executable = e;
                        //        update++;
                        //        break;
                        //    }
                        //}
                    }
                }
                if (update == 1)
                {
                    variable.EvaluateAsExpression = true;
                    variable.Expression = "\"Sync \"+@[$Package::SyncTableName]+\" \"+ REPLACE( @[System::PackageName], \"z\" + @[$Package::SyncTableName]+\"_\", \"\" )";
                    p.Executables.Remove(executable);
                    string pText;
                    Console.WriteLine("changing : {0}", p.Name);
                    p.SaveToXML(out pText, null);
                    File.WriteAllText(destPath + p.Name + ".dtsx", pText);
                    continue;
                }
                if (update > 0) Console.WriteLine("!!!{0}!!!", p.Name);
            }

        }
        static void myPackageChangeDoAllSchema()
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
        static void myPackageAdd_AddPackageConnection()
        {
            string pText;
            Project project = Project.OpenProject(PackageUtils.projectPath);
            // get Connection Example
            Package packageEx = PackageUtils.LoadPackage("CheckSchema", project);
            ConnectionManager conn = null;
            foreach (ConnectionManager par in packageEx.Connections)
            {
                if (par.Name == "PdestConnmgr")
                {
                    conn = par;
                    break;
                }
            }
            packageEx.Connections.Remove(conn);
            if (conn == null)
            {
                Console.WriteLine("Example not found!");
                return;
            }
            foreach (var pi in project.PackageItems)
            {
                Package packageS = pi.Package;
                if (packageS.Name == "CheckSchema") continue;
                int exists = 0;
                // check if change is needed
                foreach (Parameter par in packageS.Parameters)
                {
                    if (par.Name == "dPSWD" && (string)par.Value == "1") exists++;
                    if (par.Name == "dUser" && (string)par.Value == "SSIS_partnersync") exists++;
                }
                if (exists != 2)
                {
                    Console.WriteLine("NF {0} Skipped:{1}", packageS.Name, exists);
                    continue;
                }
                exists = 0;
                foreach (ConnectionManager par in packageS.Connections)
                {
                    if (par.Name == "PdestConnmgr")
                    {
                        exists++;
                        break;
                    }
                }
                if (exists == 1)
                {
                    Console.WriteLine("FF {0} Skipped:{1}", packageS.Name, exists);
                    continue;
                }

                Console.WriteLine("Changing {0}", packageS.Name);
                packageS.Connections.Join(conn);

                packageS.SaveToXML(out pText, null);
                File.WriteAllText(destPath + packageS.Name + ".dtsx", pText);
                packageS.Connections.Remove(conn);
            }
        }
        static void myPackageAdd_PartnerIdDefaultUserPSWD()
        {
            Project project = Project.OpenProject(PackageUtils.projectPath);
            string pText;
            foreach (var pi in project.PackageItems)
            {
                Package packageS = pi.Package;
                int exists = 0;
                foreach (Parameter par in packageS.Parameters)
                {
                    if (par.Name == "dPSWD")
                    {
                        if ((string)par.Value != "1")
                        {
                            exists++;
                            par.Value = "1";
                            Console.WriteLine("pswd! Package:{0} - Name:{1} - DataType:{2} - Value:{3}", packageS.Name, par.Name, par.DataType, par.Value);
                            continue;
                        }
                    }
                    if (par.Name == "dUser")
                    {
                        if ((string)par.Value != "SSIS_partnersync")
                        {
                            exists++;
                            par.Value = "1";
                            Console.WriteLine("user! Package:{0} - Name:{1} - DataType:{2} - Value:{3}", packageS.Name, par.Name, par.DataType, par.Value);
                            continue;
                        }
                    }
                }
                if (exists > 0)
                {
                    //Parameter par = packageS.Parameters.Add("dCatalogName", TypeCode.String);
                    //par.Value = "SBdest";
                    packageS.SaveToXML(out pText, null);
                    File.WriteAllText(destPath + packageS.Name + ".dtsx", pText);
                }

            }
        }
        static void myPackageDisable_ClearStaging()
        {
            string pText;
            Project project = Project.OpenProject(PackageUtils.projectPath);
            string taskHostName = "Clear Staging";
            Package p;
            TaskHost th, thCh = null;
            Variable var = null;
            foreach (PackageItem pItem in project.PackageItems)
            {
                p = pItem.Package;
                int exists = 0;
                foreach (Executable e in p.Executables)
                {
                    th = e as TaskHost;
                    //Console.WriteLine("Package:{0}", th==null?"-":th.Name);

                    if (th != null && th.Name == taskHostName)
                    {
                        exists++;
                        var = p.Variables.Cast<Variable>().FirstOrDefault(x => x.Name == "destDelStagingTable");
                        if (var != null)
                        {
                            thCh = th;
                            exists++;
                        }
                    }
                }
                if (exists == 2)
                {
                    Console.WriteLine("{1} Package:{0}", p.Name, exists);
                    thCh.Disable = true;
                    p.SaveToXML(out pText, null);
                    File.WriteAllText(destPath + p.Name + ".dtsx", pText);
                }
            }
        }
        static void myPackageReplaceEdit_LogStart1()
        {
            string pText;
            Project project = Project.OpenProject(PackageUtils.projectPath);
            string taskHostName = "LogStart";
            Package p;
            TaskHost th;
            Variable var = null;
            foreach (PackageItem pItem in project.PackageItems)
            {
                p = pItem.Package;
                int exists = 0;
                foreach (Executable e in p.Executables)
                {
                    th = e as TaskHost;
                    if (th != null && th.Name == taskHostName)
                    {
                        exists++;
                        var = th.Variables.Cast<Variable>().FirstOrDefault(x => x.Name == "vDestination");
                        if (var != null)
                        {
                            exists++;
                            if (var.EvaluateAsExpression)
                            {
                                if (var.Expression == "\" / \" +   @[$Project::dCatalogName]")
                                {
                                    var.Expression = "@[$Package::dDataSource] + \" / \" +   @[$Package::dCatalogName]";
                                    exists++;
                                }
                            }
                        }
                    }
                }
                if (exists == 3)
                {
                    //Console.WriteLine("Not Package:{0};{1}    {2}", p.Name, exists, var.Expression);
                    Console.WriteLine("Not Package:{0}", p.Name);
                    p.SaveToXML(out pText, null);
                    File.WriteAllText(destPath + p.Name + ".dtsx", pText);
                }
            }
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
            foreach (PackageItem pItem in project.PackageItems)
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
                            File.WriteAllText(destPath + p.Name + ".dtsx", pText);
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
