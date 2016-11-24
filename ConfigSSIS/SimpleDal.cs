using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Configuration;
using System.Data;

namespace ConfigSSIS
{
    class SimpleDal
    {
        static string connStr = ConfigurationManager.ConnectionStrings["SSIScontroll"].ConnectionString;

        #region POCO
        public class SyncTable
        {
            public string Name;
            public bool ToCopy;
            public bool ToStage;
            public bool FullJoin;
            public string PackageName;
            public string PartnerSpec;
        }
        #endregion

        public static List<SyncTable> getSyncTables()
        {
            SqlConnection conn = new SqlConnection(connStr);
            SqlCommand comm = new SqlCommand("select t.*,o.PackageName from sync4partner.SyncTable t join sync4partner.SyncTableOperational o on t.name=o.name where ToCopy=1", conn);
            DataTable table = new DataTable();
            comm.Connection.Open();
            table.Load(comm.ExecuteReader());
            conn.Close();
            //  x => new SyncTable() { Name = x[""], ToCopy = x["ToCopy"], ToStage = x["ToStage"] }
            List<SyncTable> lst = new List<SyncTable>(table.Rows.Count);
            SyncTable aST;
            foreach (DataRow r in table.Rows)
            {
                aST = new SyncTable()
                {
                    Name = r.Field<string>("Name"),
                    ToCopy = Convert.ToInt32(r["ToCopy"]) == 1,
                    ToStage = Convert.ToInt32(r["ToStage"]) == 1,
                    PackageName = r.Field<string>("PackageName"),
                    FullJoin = Convert.ToInt32(r["FullJoin"]) == 1,
                    PartnerSpec = r.Field<string>("PartnerSpec")
                };
                lst.Add(aST);
            }
            return lst;
        }
    }
}
