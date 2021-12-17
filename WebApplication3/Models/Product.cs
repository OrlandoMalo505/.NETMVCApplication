using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;

namespace WebApplication3.Models
{
    public class Product
    {


        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public string ProductType { get; set; }
        public float ProductPrice { get; set; } 
        public int ProductQuantity { get; set; }    
        public float ProductCost { get; set; }
        public int ProductCode { get; set; }
        public string ProductStatus { get; set; }






        public List<Product> GetAllProducts(int id)
        {
            Database datab = new Database();
            var cnn = datab.database();
            List<Product> products = new List<Product>();
            cnn.Open();
            SqlCommand cmd = new SqlCommand("select * from Products where UserId=@id", cnn);

            cmd.Parameters.AddWithValue("id", id);

            var reader = cmd.ExecuteReader();
            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    products.Add(new Product()
                    {
                        ProductId = Convert.ToInt32(reader["ProductId"]),
                        ProductCode = Convert.ToInt32(reader["ProductCode"]),
                        ProductCost = Convert.ToInt64(reader["ProductCost"]),
                        ProductName = reader["ProductName"].ToString(),
                        ProductPrice = Convert.ToInt64(reader["ProductPrice"]),
                        ProductQuantity = Convert.ToInt32(reader["ProductQuantity"]),
                        ProductType = reader["ProductType"].ToString(),
                        ProductStatus = reader["ProductStatus"].ToString()
                    });
                }
            }

            cnn.Close();

            return products;
        }

    }
}
