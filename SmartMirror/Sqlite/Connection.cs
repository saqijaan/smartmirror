using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SQLite;
using System.Windows.Forms;
namespace SmartMirror.Sqlite
{
    class Connection
    {
        private static string path=Application.StartupPath+"\\Database\\smartMirror.db";
        private static SQLiteConnection con=null;

        public static string Path
        {
            get
            {
                return path;
            }
        }

        public static SQLiteConnection get
        {
            get
            {
                if (con == null)
                {
                    con = new SQLiteConnection(String.Format("Data Source={0};Version=3;", path));
                }
                if (con.State== System.Data.ConnectionState.Closed)
                {
                    con.Open();
                }
                return con;
            }
        }
        public static void Close()
        {
            if (con != null)
            {
                con.Close();
            }
        }

        
    }
}
