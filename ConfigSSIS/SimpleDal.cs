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
        }
        #endregion

        public static List<SyncTable> getSyncTables()
        {
            SqlConnection conn = new SqlConnection(connStr);
            SqlCommand comm = new SqlCommand("select * from sync4partner.SyncTable", conn);
            DataTable table = new DataTable();
            comm.Connection.Open();
            table.Load(comm.ExecuteReader());
            //  x => new SyncTable() { Name = x[""], ToCopy = x["ToCopy"], ToStage = x["ToStage"] }
            List<SyncTable> lst = new List<SyncTable>(table.Rows.Count);
            SyncTable aST;
            foreach (DataRow r in table.Rows)
            {
                aST = new SyncTable()
                {
                    Name = r.Field<string>("Name"),
                    ToCopy = Convert.ToInt32(r["ToCopy"]) == 1,
                    ToStage = Convert.ToInt32(r["ToStage"]) == 1
                };
                lst.Add(aST);
            }
            return lst;
        }
    }
}
