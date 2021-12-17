using Microsoft.Extensions.Configuration;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;


namespace WebApplication3.Models
{

    public class User
    {

        public int Id { get; set; }

        [Required(ErrorMessage = "First Name can't be empty! ")]
        [StringLength(255)]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        [Required]
        [Range(0, int.MaxValue, ErrorMessage = "Insert a number!!")]
        public int Age { get; set; }

        [Required]
        public string Address { get; set; }

        [Required]
        public string State { get; set; }

        [Required]
        public string City { get; set; }

        [Required]
        public string ZipCode { get; set; }

        [Required]
        [Range(1, 2, ErrorMessage = "Select User or Admin")]
        public int Role { get; set; }

        [Required]
        [EmailAddress]
        public string Username { get; set; }

        [Required]
        public string Password { get; set; }

        [Required]
        public Role RoleEnum { get; set; }



        public List<User> GetAllUsers()
        {
            Database datab = new Database();
            var cnn = datab.database();
            List<User> users = new List<User>();
            cnn.Open();
            SqlCommand cmd = new SqlCommand("select * from Users", cnn);
            var reader = cmd.ExecuteReader();
            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    users.Add(new User()
                    {
                        Id = Convert.ToInt32(reader["ID"]),
                        FirstName = reader["firstName"].ToString(),
                        LastName = reader["lastName"].ToString(),
                        Age = (int)reader["age"],
                        Address = reader["address"].ToString(),
                        State = reader["state"].ToString(),
                        City = reader["city"].ToString(),
                        ZipCode = reader["zipCode"].ToString(),
                        RoleEnum = (Role)(Convert.ToInt32(reader["role"])),
                        Username = reader["username"].ToString()

                    });
                }
            }

            cnn.Close();
            return users;
        }



        public int SaveDetails(User umodel)
        {

            Database datab = new Database();
            var cnn = datab.database();

            string FirstName = umodel.FirstName;
            string LastName = umodel.LastName;
            int Age = umodel.Age;
            string Address = umodel.Address;
            string State = umodel.State;
            string City = umodel.City; ;
            string ZipCode = umodel.ZipCode;
            int Role = umodel.Role;
            string Username = umodel.Username;
            string Password = umodel.Password;

            string query = "INSERT INTO Users(FirstName, LastName, Age, Address, State, City, ZipCode, Role, Username, Password) values (@FirstName , @LastName ,@Age ,@Address ,@State ,@City,@ZipCode ,@Role, @Username, @Password)";

            SqlCommand cmd = new SqlCommand(query, cnn);
            cnn.Open();
            cmd.Parameters.AddWithValue("Firstname", FirstName);
            cmd.Parameters.AddWithValue("LastName", LastName);
            cmd.Parameters.AddWithValue("Age", Age);
            cmd.Parameters.AddWithValue("Address", Address);
            cmd.Parameters.AddWithValue("State", State);
            cmd.Parameters.AddWithValue("City", City);
            cmd.Parameters.AddWithValue("ZipCode", ZipCode);
            cmd.Parameters.AddWithValue("Role", Role);
            cmd.Parameters.AddWithValue("Username", Username);
            cmd.Parameters.AddWithValue("Password", Password);
            int i = cmd.ExecuteNonQuery();
            cnn.Close();
            return i;
        }

        public int EditUser(User umodel, Role role)
        {
            Database datab = new Database();
            var cnn = datab.database();

            string query = "Update Users set FirstName= @FirstName, LastName=@LastName, Age = @Age, Address=@Address , State=@State, City=@City , ZipCode=@ZipCode ,Role=@Role ,Username=@Username ,Password=@Password where id=@id";

            SqlCommand cmd = new SqlCommand(query, cnn);
            cnn.Open();
            cmd.Parameters.AddWithValue("Firstname", umodel.FirstName);
            cmd.Parameters.AddWithValue("LastName", umodel.LastName);
            cmd.Parameters.AddWithValue("Age", umodel.Age);
            cmd.Parameters.AddWithValue("Address", umodel.Address);
            cmd.Parameters.AddWithValue("State", umodel.State);
            cmd.Parameters.AddWithValue("City", umodel.City);
            cmd.Parameters.AddWithValue("ZipCode", umodel.ZipCode);
            cmd.Parameters.AddWithValue("Role", (int)role);
            cmd.Parameters.AddWithValue("Username", umodel.Username);
            cmd.Parameters.AddWithValue("Password", umodel.Password);
            cmd.Parameters.AddWithValue("Id", umodel.Id);

            int i = cmd.ExecuteNonQuery();
            cnn.Close();
            return i;
        }

        public User getUserById(int id)
        {
            Database datab = new Database();
            var cnn = datab.database();
            User user = new User();
            cnn.Open();
            SqlCommand cmd = new SqlCommand("select * from Users where id=@id", cnn);
            cmd.Parameters.AddWithValue("id", id);
            var reader = cmd.ExecuteReader();
            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    user.Id = (int)reader["id"];
                    user.FirstName = reader["firstName"].ToString();
                    user.LastName = reader["lastName"].ToString();
                    user.Age = (int)reader["age"];
                    user.Address = reader["address"].ToString();
                    user.State = reader["state"].ToString();
                    user.City = reader["city"].ToString();
                    user.ZipCode = reader["zipCode"].ToString();
                    user.RoleEnum = (Role)reader["role"];
                    user.Username = reader["username"].ToString();
                    user.Password = reader["password"].ToString();

                }
            }
            return user;
        }






        public int CheckUser(string username)

        {
            string connect = @"Data Source=DESKTOP-D29MF8M;Initial Catalog=login;User ID=firstapp;Password=login";

            string query = "SELECT COUNT(*) FROM Users WHERE Username = @UserName";

            using (SqlConnection conn = new SqlConnection(connect))

            {
                using (SqlCommand cmd = new SqlCommand(query, conn))

                {

                    cmd.Parameters.AddWithValue("UserName", username);

                    conn.Open();

                    return (int)cmd.ExecuteScalar();

                }
            }
        }

        public void DeleteUserById(int id)
        {
            User user = new User().getUserById(id);
            Database datab = new Database();
            var cnn = datab.database();
            cnn.Open();

            SqlCommand cmd = new SqlCommand("delete from Users where id=@id", cnn);
            SqlParameter param = new SqlParameter();
            cmd.Parameters.AddWithValue("id", id);
            cmd.ExecuteScalar();


        }


        public User FindUser(User user)
        {

            Database datab = new Database();
            var cnn = datab.database();
            cnn.Open();
            SqlCommand cmd = new SqlCommand("select * from Users where Username=@username and Password=@password", cnn);
            cmd.Parameters.AddWithValue("username", user.Username);
            cmd.Parameters.AddWithValue("password", user.Password);
            var reader = cmd.ExecuteReader();

            if (reader.HasRows)
            {
                while (reader.Read())
                {

                    user.Id = (int)reader["ID"];
                    user.FirstName = reader["firstName"].ToString();
                    user.LastName = reader["lastName"].ToString();
                    user.Age = (int)reader["age"];
                    user.Address = reader["address"].ToString();
                    user.State = reader["state"].ToString();
                    user.City = reader["city"].ToString();
                    user.ZipCode = reader["zipCode"].ToString();
                    user.Username = reader["username"].ToString();
                    user.Password = reader["password"].ToString();
                    user.Role = (int)reader["role"];
                    user.RoleEnum = (Role)reader["role"];

                }
                return user;
            }
            return null;
        }
    }
}