using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Security.Cryptography;

namespace WebApplication3.Models
{
    public class Order
    {

        public int OrderId { get; set; }
        public int OrderNumber { get; set; }
        public string OrderName { get; set; }
        public string OrderDescription { get; set; }
        public int UserId { get; set; }
        public List<Product> Products { get; set; }
        





        public List<Order> GetAllOrders(int id)
        {
            Database datab = new Database();
            var cnn = datab.database();
            SqlCommand cmd = new SqlCommand("select * from Orders where UserId=@id", cnn);
            cmd.Parameters.AddWithValue("id", id);

            return ListOfOrders(cmd, cnn);
        }

        public List<Order> ListOfOrders(SqlCommand cmd, SqlConnection cnn)
        {
            var orders = new List<Order>();
            cnn.Open();
            var reader = cmd.ExecuteReader();
            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    orders.Add(OrderReader(reader));
                }
            }
            cnn.Close();
            return orders;
        }

        private Order OrderReader(SqlDataReader reader)
        {
            if (reader.HasRows)
            {
                return new Order()
                {
                    OrderId = Convert.ToInt32(reader["OrderID"]),
                    OrderName = reader["OrderName"].ToString(),
                    OrderNumber = (int)reader["OrderNumber"],
                    OrderDescription = (reader["OrderDescription"]).ToString(),
                    UserId = Convert.ToInt32(reader["UserId"])
                };
            }
            return null;
        }


        public uint Get8Digits()
        {
            var bytes = new byte[4];
            var rng = RandomNumberGenerator.Create();
            rng.GetBytes(bytes);
            uint random = BitConverter.ToUInt32(bytes, 0) % 100000000;
            return random;
        }


        public int SaveOrder(int id, Order order)
        {
            Database datab = new Database();
            var cnn = datab.database();

            string OrderName = order.OrderName;
            string OrderDescription = order.OrderDescription;
            int OrderNumber = order.OrderNumber;
            int UserId = id;
           

            string query = "INSERT INTO Orders(OrderName, OrderDescription, OrderNumber, UserId) values (@OrderName , @OrderDescription, @OrderNumber ,@UserId )";

            SqlCommand cmd = new SqlCommand(query, cnn);
            cnn.Open();
            cmd.Parameters.AddWithValue("OrderName", OrderName);
            cmd.Parameters.AddWithValue("OrderDescription", OrderDescription);
            cmd.Parameters.AddWithValue("OrderNumber", OrderNumber);
            cmd.Parameters.AddWithValue("UserId", id);
            int i = cmd.ExecuteNonQuery();
            cnn.Close();
            return i;
        }
    }
}
