using SmartMirror.Sqlite;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartMirror.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Password { get; set; }
        public string Status { get; set; }
        public string Type { get; set; }

        public static string LastError { get; set; }

        public override string ToString()
        {
            return Name;
        }
        public bool Save()
        {
            try
            {

                var insertQuery = "INSERT INTO users(name,email,phone,password,status,type) VALUES(@name,@email,@phone,@password,@status,@type)";
                var cmd = new SQLiteCommand(insertQuery, Connection.get);
                cmd.Parameters.AddWithValue("name", this.Name);
                cmd.Parameters.AddWithValue("email", this.Email);
                cmd.Parameters.AddWithValue("phone", this.Phone);
                cmd.Parameters.AddWithValue("password", this.Password);
                cmd.Parameters.AddWithValue("status", this.Status);
                cmd.Parameters.AddWithValue("type", this.Type);
                var result = cmd.ExecuteNonQuery();
                return result > 0 ? true : false;
            }
            catch (Exception ex)
            {
                LastError = ex.ToString();
                return false;
            }
        }
        public static User find(int faceid)
        {
            try
            {

                var Query = "SELECT * FROM users inner join faces on users.id=faces.userid WHERE faces.id= @id";
                var cmd = new SQLiteCommand(Query, Connection.get);
                cmd.Parameters.AddWithValue("id", faceid);
                var result = cmd.ExecuteReader();
                if (!result.HasRows) return null;
                result.Read();
                return new User(){
                    Id = Convert.ToInt32(result["id"]),
                    Name = result["name"].ToString(),
                    Email = result["email"].ToString(),
                    Password = result["password"].ToString(),
                    Phone = result["phone"].ToString(),
                    Status = result["status"].ToString(),
                    Type = result["type"].ToString(),
                };
            }
            catch (Exception ex)
            {
                LastError = ex.ToString();
                Debug.Write(ex.Message);
                return null;
            }
        }
        public static List<User> get()
        {
            try
            {
                List<User> allUsers = new List<User>();
                var Query = "SELECT * FROM users";
                var cmd = new SQLiteCommand(Query, Connection.get);
                var result = cmd.ExecuteReader();
                if (!result.HasRows) return null;
                while (result.Read())
                {
                    allUsers.Add(
                        new User(){
                            Id = Convert.ToInt32(result["id"]),
                            Name = result["name"].ToString(),
                            Email = result["email"].ToString(),
                            Password = result["password"].ToString(),
                            Phone = result["phone"].ToString(),
                            Status = result["status"].ToString(),
                            Type = result["type"].ToString(),
                        }
                    );
                }
                return allUsers;
            }
            catch (Exception ex)
            {
                LastError = ex.ToString();
                return null;
            }
        }
    }
}
