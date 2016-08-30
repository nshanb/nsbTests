using Microsoft.SqlServer.Dts.Runtime;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConfigSSIS
{
    // specially written to fail every now and then
    class ChangeTasks
    {
        public static StringCollection get_SchemaSync_TransferTables_TablesList(Package package)
        {
            if (package.Name != "SchemaSync") return null;
            TaskHost th = package.Executables.Cast<TaskHost>().Where(t => t.Name == "TransferTables").SingleOrDefault();
            DtsProperty tl = th.Properties.Cast<DtsProperty>().Where(t => t.Name == "TablesList").SingleOrDefault();

            return tl.GetValue(th) as StringCollection;
        }

        public static bool set_SchemaSync_TransferTables_TablesList(Package package, StringCollection newValue)
        {
            if (package.Name != "SchemaSync") return false;
            TaskHost th = package.Executables.Cast<TaskHost>().Where(t => t.Name == "TransferTables").SingleOrDefault();
            DtsProperty tl = th.Properties.Cast<DtsProperty>().Where(t => t.Name == "TablesList").SingleOrDefault();

            // check and format newValue
            StringCollection qualifiedList = new StringCollection();
            bool stagingfound = false;
            string s1;
            foreach (string s in newValue)
            {
                s1 = s;
                if (!stagingfound)
                {
                    if (s.ToLower() == "[staging].[teststaging]") { stagingfound = true; }
                }
                if (!s.Contains(".")) // TODO nsb check through regexp?
                {
                    s1 = "[dbo].[" + s + "]";
                }
                qualifiedList.Add(s1);
            }
            if(!stagingfound) qualifiedList.Add("[staging].[teststaging]");
            tl.SetValue(th, qualifiedList);

            return true;
        }
    }
}
