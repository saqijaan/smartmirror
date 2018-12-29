using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SmartMirror.Sqlite;
using System.Data;

namespace SmartMirror.Models
{
    public class face
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public byte[] Image { get; set; }
        public static string LastError { get; set; }

        public bool Save()
        {
            try
            {

                var insertQuery = "INSERT INTO faces(userid, image) VALUES(@userid,@image)";
                var cmd = new SQLiteCommand(insertQuery, Connection.get);
                cmd.Parameters.AddWithValue("userid", this.UserId);
                cmd.Parameters.Add("image", DbType.Binary, this.Image.Length).Value = this.Image;
                var result = cmd.ExecuteNonQuery();
                return result > 0 ? true : false;
            }
            catch (Exception ex)
            {
                LastError = ex.ToString();
                return false;
            }
        }
        public static face find(int id)
        {
            try
            {

                var Query = "SELECT * FROM faces WHERE id= @id";
                var cmd = new SQLiteCommand(Query, Connection.get);
                cmd.Parameters.AddWithValue("id", id);
                var result = cmd.ExecuteReader();
                if (!result.HasRows) return null;
                result.Read();
                return new face() { Id= Convert.ToInt32(result["id"]), UserId= Convert.ToInt32(result["userid"]),Image=(byte[])result["image"] };
            }
            catch (Exception ex)
            {
                LastError = ex.ToString();
                return null;
            }
        }
        public static List<face> get()
        {
            try
            {
                List<face> allfaces=new List<face>();
                var Query = "SELECT * FROM faces";
                var cmd = new SQLiteCommand(Query, Connection.get);
                var result = cmd.ExecuteReader();
                if (!result.HasRows) return null;
                while(result.Read())
                {
                    allfaces.Add(new face() { Id = Convert.ToInt32(result["id"]), UserId = Convert.ToInt32(result["userid"]), Image = (byte[])result["image"] });
                }
                return allfaces;
            }
            catch (Exception ex)
            {
                LastError = ex.ToString();
                return null;
            }
        }
    }
}
